using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Barunson.WorkerService.MCardResourceCleanJob
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IServiceProvider _serviceProvider;
        private TelemetryClient _telemetryClient;
        private readonly IMailSendService _mail;

        private static string WorkerName => "MCardResourceCleanJob";

        private static string fileroot => "/mnt/mcardshare/barunsonmcard/";
        private static string invitationFolder => fileroot+ "upload/invitation/";
        private static string removeFolder => fileroot + "RemovedFile/upload/invitation/";
        private DateTime now { get; set; }

        public Worker(ILogger<Worker> logger, TelemetryClient tc, IHostApplicationLifetime hostApplicationLifetime, IServiceProvider services, IMailSendService mail, IConfiguration appconfig)
        {
            _logger = logger;
            _telemetryClient = tc;
            _hostApplicationLifetime = hostApplicationLifetime;
            _serviceProvider = services;
            _mail = mail;

        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{WorkerName} running at: {time}", WorkerName, DateTimeOffset.Now);

            try
            {
                now = DateTime.Now;

                //초대장 Temp 파일 지우기
                DeleteTempFIles(fileroot + "upload/invitation/temp/", cancellationToken);
                //상품 Temp 파일 지우기
                DeleteTempFIles(fileroot + "upload/product/temp/", cancellationToken);
                //템플릿 Temp 파일 지우기
                DeleteTempFIles(fileroot + "upload/template/temp/", cancellationToken);
                //이미지 Temp 파일 지우기
                DeleteTempFIles(fileroot + "upload/img/temp/", cancellationToken);

                //초대장 파일 폴더 기준으로 데이터 처리 작업
                await InviationJobAsync(cancellationToken);

                //Remove 폴더의 오래된 파일 영구삭제
                DeleteOldRemoveFiles(cancellationToken);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{time:yyyy-MM-dd HH:mm:ss} {WorkerName} has error.", DateTime.Now, WorkerName);

                var mailSubject = "[모초]리소스 정리 작업 오류";
                var mailBody = new StringBuilder();
                mailBody.AppendLine("<table cellpadding=\"0\" cellspacing =\"0\" width=\"100%\">");
                mailBody.AppendLine("<tr><td>에러내용</td></tr>");
                mailBody.AppendLine("<tr><td><pre>");
                mailBody.AppendLine(ex.ToString());
                mailBody.AppendLine("</pre></td></tr>");
                mailBody.AppendLine("</table>");
                await _mail.SendAsync(mailSubject, mailBody.ToString());
            }

            await _telemetryClient.FlushAsync(cancellationToken);

            _hostApplicationLifetime.StopApplication();

        }

        
        private async Task InviationJobAsync(CancellationToken cancellationToken)
        {
            //7일전 데이터까지는 유지, 이전 데이터만 삭제 작업
            var MaxDate = now.AddDays(-7).ToString("yyMMdd");
            var MinDate = now.AddMonths(-3).ToString("yyMMdd");
            var check30Day = now.AddDays(-30);
            var check90Day = now.AddDays(-90);

            var rootDir = new DirectoryInfo(invitationFolder);
            var dateDirsAll = rootDir.GetDirectories();
            var dateDirs = dateDirsAll.Where(m => m.Name.CompareTo(MaxDate) <= 0 && m.Name.CompareTo(MinDate) > 0).OrderBy(m => m.Name).ToList();
            foreach (var dateDir in dateDirs)
            {
                if (cancellationToken.IsCancellationRequested)
                    cancellationToken.ThrowIfCancellationRequested();

                if (dateDir.Name == "")
                    continue;

                //초대장 폴더의 하위 폴더 읽기
                var invitationDirs = dateDir.GetDirectories();

                await Parallel.ForEachAsync(invitationDirs, async (invitationDir, cancellationToken) =>
                {
                    if (cancellationToken.IsCancellationRequested)
                        cancellationToken.ThrowIfCancellationRequested();

                    if (invitationDir.Name == "temp")
                        return;

                    var id = int.Parse(invitationDir.Name);
                    var invitFiles = new List<string>();
                    try
                    {
                        invitFiles.AddRange(Directory.GetFiles(invitationDir.FullName));

                        var galleryDirs = invitationDir.GetDirectories("gallery");
                        if (galleryDirs.Any())
                            invitFiles.AddRange(Directory.GetFiles(galleryDirs.First().FullName));
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Error! Get File list: {0}", invitationDir.FullName);
                        return;
                    }

                    // 삭제 이동할 파일 목록
                    var removeFiles = new List<string>();

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<BarunsonContext>();

                        
                        // 청첩장
                        var inviationQuery = from a in context.TB_Invitation
                                             where a.Invitation_ID == id
                                             select a;
                        var inviationItem = await inviationQuery.FirstOrDefaultAsync(cancellationToken);

                        //청접장 세부 사항의 이미지
                        var invitationDetailQuery = from a in context.TB_Invitation_Detail
                                                    where a.Invitation_ID == id
                                                    select a;
                        var invitationDetailItem = await invitationDetailQuery.FirstOrDefaultAsync(cancellationToken);

                        // DB에 Inviation 이 없는 경우 해당 폴더 파일 모두 제거
                        if (inviationItem != null && invitationDetailItem != null)
                        {
                            var orderQuery = from a in context.TB_Order
                                             where a.Order_ID == inviationItem.Order_ID
                                             select a;
                            var orderItem = await orderQuery.FirstAsync(cancellationToken);

                            var weddingDate = string.IsNullOrEmpty(invitationDetailItem.WeddingDate) ? new DateTime(1900, 1, 1) : DateTime.Parse(invitationDetailItem.WeddingDate);
                            if (weddingDate < check90Day && inviationItem.Regist_DateTime < check90Day)
                            {
                                //등록일 및 웨딩일이 90일이 지난 경우 만료 처리
                                if (inviationItem.Invitation_Display_YN != "N" || invitationDetailItem.Invitation_Display_YN != "N")
                                {
                                    inviationItem.Invitation_Display_YN = "N";
                                    invitationDetailItem.Invitation_Display_YN = "N";

                                    await context.SaveChangesAsync();
                                }
                            }
                            if (orderItem.Update_DateTime < check30Day && orderItem.Order_Status_Code == "OSC03" && orderItem.Payment_Status_Code == "PSC01")
                            {
                                //제작중 30일 경과 휴지통 이동
                                removeFiles.AddRange(invitFiles);
                            }
                            else if (!(orderItem.Order_Status_Code == "OSC03" && orderItem.Payment_Status_Code == "PSC01"))
                            {
                                //현제 제작중이 아닌 파일만 

                                // 유지 데이터 중 db에 기록되지 않은 파일 찾아 제거
                                // DB에 기록된 파일 URL 
                                var dbFiles = new List<string>();

                                if (!string.IsNullOrEmpty(invitationDetailItem.Delegate_Image_URL))
                                    dbFiles.Add(invitationDetailItem.Delegate_Image_URL);
                                if (!string.IsNullOrEmpty(invitationDetailItem.SNS_Image_URL))
                                    dbFiles.Add(invitationDetailItem.SNS_Image_URL);
                                if (!string.IsNullOrEmpty(invitationDetailItem.Outline_Image_URL))
                                    dbFiles.Add(invitationDetailItem.Outline_Image_URL);

                                //청접장 아이템 이미지
                                var invitationItemQuery = from a in context.TB_Invitation_Item
                                                          join b in context.TB_Item_Resource on a.Resource_ID equals b.Resource_ID
                                                          where a.Invitation_ID == id &&
                                                              !string.IsNullOrEmpty(b.Resource_URL)
                                                          select b;
                                var invitationItems = await invitationItemQuery.ToListAsync(cancellationToken);
                                foreach (var invitationItem in invitationItems)
                                {
                                    dbFiles.Add(invitationItem.Resource_URL);
                                }

                                //Gallery
                                var galeryItemQuery = from a in context.TB_Gallery
                                                      where a.Invitation_ID == id &&
                                                        !string.IsNullOrEmpty(a.Image_URL)
                                                      select a;
                                var galeryItems = await galeryItemQuery.ToListAsync(cancellationToken);
                                foreach (var galeryItem in galeryItems)
                                {
                                    dbFiles.Add(galeryItem.Image_URL);
                                    if (!string.IsNullOrEmpty(galeryItem.SmallImage_URL))
                                        dbFiles.Add(galeryItem.SmallImage_URL);
                                }

                                //삭제 대상 파일 검색
                                removeFiles.AddRange(invitFiles.Where(m => !dbFiles.Any(d => m.Contains(d))));
                            }
                        }
                        else
                        {
                            removeFiles.AddRange(invitFiles);
                        }
                    }
                    // 삭제 파일
                    if (removeFiles.Count > 0)
                    {
                        foreach (var removeFile in removeFiles)
                        {
                            if (cancellationToken.IsCancellationRequested)
                                cancellationToken.ThrowIfCancellationRequested();
                            try
                            {

                                var delFIle = new FileInfo(removeFile);
                                if (delFIle.Exists)
                                {
                                    delFIle.Delete();
                                    _logger.LogInformation("Delete UnUsed File: {0}", removeFile);
                                }
                            }
                            catch (UnauthorizedAccessException) { }
                            catch (DirectoryNotFoundException) { }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Error! Delete File: {0}", removeFile);
                            }
                        }

                        // 파일 삭제 후 해당 폴더에 파일이 없을경우 폴더 삭제
                        DeleteEmptyDirs(invitationDir);
                    }
                });
               
            }
        }

        /// <summary>
        /// 임시폴더 파일 삭제
        /// </summary>
        /// <param name="tempPath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private void DeleteTempFIles(string tempPath, CancellationToken cancellationToken)
        {
            var checkTime = now.AddDays(-7);

            var dir = new DirectoryInfo(tempPath);
            if (dir.Exists)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var subdirs = dir.GetDirectories();
                foreach (var subdir in subdirs)
                {
                    DeleteTempFIles(subdir.FullName, cancellationToken);
                }

                var files = dir.GetFiles();
                foreach(var file in files)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (file.Exists && file.LastWriteTime < checkTime)
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (UnauthorizedAccessException) { }
                    }
                }
 
            }
        }

        /// <summary>
        /// 삭제 폴더에서 오래된 파일 영구 삭제
        /// </summary>
        /// <param name="removePath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private void DeleteOldRemoveFiles(CancellationToken cancellationToken)
        {
            var checkTime = now.AddDays(-90).ToString("yyMMdd");

            var dir = new DirectoryInfo(removeFolder);
            if (dir.Exists)
            {
                var subdirs = dir.GetDirectories();
                foreach (var subdir in subdirs)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (subdir.Name.CompareTo(checkTime) < 0)
                    {
                        try
                        {
                            _logger.LogInformation("Delete Old Dir: {0}", subdir.FullName);
                            subdir.Delete(true);
                        }
                        catch (UnauthorizedAccessException) { }
                        catch (DirectoryNotFoundException) { }
                    }
                }
            }

        }

        private void DeleteEmptyDirs(DirectoryInfo dir)
        {
            if (dir.Exists)
            {
                try
                {
                    foreach (var d in dir.GetDirectories())
                    {
                        DeleteEmptyDirs(d);
                    }
                    var entries = dir.GetFiles();
                    if (!entries.Any())
                    {
                        dir.Delete();
                        _logger.LogInformation("Delete Empty Dir: {0}", dir.FullName);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error! Delete Empty Dir: {0}", dir.FullName);
                }
            }
        }

    }
}