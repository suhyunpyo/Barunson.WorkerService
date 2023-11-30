using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Models;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using SMBLibrary;
using SMBLibrary.Client;
using System.Net;
using System.Text;

namespace Barunson.WorkerService.CommonBatchJob.Jobs
{
    /// <summary>
    /// ASP Error Log 파일 취합, 매시 10분
    /// </summary>
    internal class AspErrorLogGathering : BaseJob
    {
        private readonly SMBUser _sMBUser;

        public AspErrorLogGathering(ILogger<Worker> logger, IServiceProvider services, BarShopContext barShopContext,
            TelemetryClient tc, IMailSendService mail, string workerName, SMBUser sMBUser)
            : base(logger, services, barShopContext, tc, mail, workerName, "AspErrorLogGathering", "10 */1 * * *")
        { 
            _sMBUser = sMBUser;
        }
        public override async Task Excute(CancellationToken cancellationToken)
        {
            try
            {
                if (!await IsExecute(cancellationToken))
                    return;
                _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is working.");
                var Now = DateTime.Now;

                var minFileTimeName = DateTime.Now.AddHours(-48).ToString("yyyy-MM-dd HH") + ".txt";
                var maxFileTimeName = DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt";

                var shareName = "Logs";
                //사이트별 로그 폴더
                var logDirs = new Dictionary<string, string>
                {
                    { "m.barunsoncard.com","SiteErrorLog\\m.barunsoncard.com" },
                    { "m.barunsonmall.com","SiteErrorLog\\m.barunsonmall.com" },
                    { "m.premierpaper.co.kr","SiteErrorLog\\m.premierpaper.co.kr"},
                    { "www.barunsoncard.com", "SiteErrorLog\\www.barunsoncard.com"},
                    { "www.barunsonmall.com", "SiteErrorLog\\www.barunsonmall.com"},
                    { "www.premierpaper.co.kr", "SiteErrorLog\\www.premierpaper.co.kr"}
                };
                //로그 프로퍼티
                var logProperties = new Dictionary<string, string>()
                {
                    { "ServerIp","" },
                    { "Site", ""},
                    { "LogTime", ""},
                    { "ClientIp", ""},
                    { "FullUrl", ""},
                    { "RefererUrl", ""},
                    { "UserInfo", ""},
                    { "File", ""}
                };
                var WebServers = new List<IPAddress> { IPAddress.Parse("172.16.1.6"), IPAddress.Parse("172.16.1.8") };

                //웹서버 단위로 로그 취합
                WebServers.ForEach(ip =>
                {
                    //로그 서버 IP
                    logProperties["ServerIp"] = ip.ToString();

                    var smb = new SMB2Client();
                    var con = smb.Connect(ip, SMBTransportType.DirectTCPTransport);
                    ISMBFileStore webFileStore = null;

                    object webdirectoryHandle = null;
                    FileStatus fileStatus;
                    try
                    {
                        if (con)
                        {
                            var status = smb.Login(String.Empty, _sMBUser.UserID, _sMBUser.Password);
                            if (status == NTStatus.STATUS_SUCCESS)
                            {
                                webFileStore = smb.TreeConnect(shareName, out status);

                                if (status == NTStatus.STATUS_SUCCESS)
                                {
                                    //각 사이트별 폴더 단위로 반복
                                    foreach (var logDir in logDirs)
                                    {
                                        logProperties["Site"] = logDir.Key;

                                        status = webFileStore.CreateFile(out webdirectoryHandle, out fileStatus,
                                            logDir.Value,
                                            AccessMask.GENERIC_READ,
                                            SMBLibrary.FileAttributes.Directory,
                                            ShareAccess.Read | ShareAccess.Write,
                                            CreateDisposition.FILE_OPEN,
                                            CreateOptions.FILE_DIRECTORY_FILE,
                                            null);

                                        if (status != NTStatus.STATUS_SUCCESS)
                                            continue;

                                        List<QueryDirectoryFileInformation> fileList;
                                        status = webFileStore.QueryDirectory(out fileList, webdirectoryHandle, $"*", FileInformationClass.FileDirectoryInformation);

                                        foreach (FileDirectoryInformation fileobject in fileList.Cast<FileDirectoryInformation>())
                                        {
                                            if (fileobject.FileAttributes == SMBLibrary.FileAttributes.Archive)
                                            {
                                                if (fileobject.FileName.CompareTo(maxFileTimeName) < 0
                                                    && fileobject.FileName.CompareTo(minFileTimeName) >= 0)
                                                {
                                                    object fileHandle;
                                                    status = webFileStore.CreateFile(out fileHandle,
                                                        out fileStatus,
                                                        logDir.Value + "\\" + fileobject.FileName,
                                                        AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE | AccessMask.SYNCHRONIZE,
                                                        SMBLibrary.FileAttributes.Normal,
                                                        ShareAccess.Read,
                                                        CreateDisposition.FILE_OPEN,
                                                        CreateOptions.FILE_NON_DIRECTORY_FILE | CreateOptions.FILE_SYNCHRONOUS_IO_ALERT,
                                                        null);

                                                    try
                                                    {
                                                        using (MemoryStream stream = new MemoryStream())
                                                        {
                                                            byte[] data;
                                                            long bytesRead = 0;
                                                            //파일 읽기
                                                            while (true)
                                                            {
                                                                status = webFileStore.ReadFile(out data, fileHandle, bytesRead, (int)smb.MaxReadSize);
                                                                if (status != NTStatus.STATUS_SUCCESS && status != NTStatus.STATUS_END_OF_FILE)
                                                                    break;

                                                                if (status == NTStatus.STATUS_END_OF_FILE || data.Length == 0)
                                                                    break;

                                                                bytesRead += data.Length;
                                                                stream.Write(data, 0, data.Length);
                                                            }
                                                            stream.Position = 0;
                                                            //로그 텍스트 처리
                                                            using (StreamReader streamReader = new StreamReader(stream))
                                                            {
                                                                var message = new StringBuilder();
                                                                string line;
                                                                while ((line = streamReader.ReadLine()) != null)
                                                                {
                                                                    if (line.StartsWith("Date:")) //로그 시작
                                                                    {
                                                                        //이전 메시지가 있으면 저장
                                                                        if (logProperties["LogTime"] != "")
                                                                        {
                                                                            var telemetry = new TraceTelemetry(message.ToString(), SeverityLevel.Error)
                                                                            {
                                                                                Timestamp = new DateTimeOffset(DateTime.Parse(logProperties["LogTime"]), new TimeSpan(9, 0, 0)),
                                                                            };
                                                                            foreach (var lp in logProperties)
                                                                                telemetry.Properties.Add(lp.Key, lp.Value);

                                                                            _telemetryClient.TrackTrace(telemetry);

                                                                            //로그 데이터 초기화
                                                                            logProperties["LogTime"] = "";
                                                                            logProperties["ClientIp"] = "";
                                                                            logProperties["FullUrl"] = "";
                                                                            logProperties["RefererUrl"] = "";
                                                                            logProperties["UserInfo"] = "";
                                                                            logProperties["File"] = "";
                                                                            message.Clear();
                                                                        }
                                                                        //로그날짜
                                                                        logProperties["LogTime"] = line.Replace("Date:", "").Trim();
                                                                    }
                                                                    else if (line.StartsWith("IP:"))
                                                                        logProperties["ClientIp"] = line.Replace("IP:", "").Trim();
                                                                    else if (line.StartsWith("FullUrl:"))
                                                                    {
                                                                        var fullurlstr = line.Replace("FullUrl:", "").Trim();
                                                                        if (fullurlstr.Contains(";http"))
                                                                        {
                                                                            var spliturl = fullurlstr.Split(';');
                                                                            fullurlstr = spliturl.Last();
                                                                        }
                                                                        logProperties["FullUrl"] = fullurlstr;
                                                                    }
                                                                    else if (line.StartsWith("RefererUrl:"))
                                                                        logProperties["RefererUrl"] = line.Replace("RefererUrl:", "").Trim();
                                                                    else if (line.StartsWith("UserInfo:"))
                                                                        logProperties["UserInfo"] = line.Replace("UserInfo:", "").Trim();
                                                                    else if (line.StartsWith("File:"))
                                                                        logProperties["File"] = line.Replace("File:", "").Trim();
                                                                    else
                                                                    {
                                                                        if (!string.IsNullOrWhiteSpace(line))
                                                                            message.AppendLine(line.Replace("Message:", "").TrimStart());
                                                                    }


                                                                }
                                                                //마지막 메시지가 있으면 저장
                                                                if (logProperties["LogTime"] != "")
                                                                {
                                                                    var telemetry = new TraceTelemetry(message.ToString(), SeverityLevel.Error)
                                                                    {
                                                                        Timestamp = new DateTimeOffset(DateTime.Parse(logProperties["LogTime"]), new TimeSpan(9, 0, 0)),
                                                                    };
                                                                    foreach (var lp in logProperties)
                                                                        telemetry.Properties.Add(lp.Key, lp.Value);

                                                                    _telemetryClient.TrackTrace(telemetry);

                                                                    //로그 데이터 초기화
                                                                    logProperties["LogTime"] = "";
                                                                    logProperties["ClientIp"] = "";
                                                                    logProperties["FullUrl"] = "";
                                                                    logProperties["RefererUrl"] = "";
                                                                    logProperties["UserInfo"] = "";
                                                                    logProperties["File"] = "";
                                                                    message.Clear();
                                                                }

                                                            }
                                                        }
                                                        //삭제
                                                        FileDispositionInformation fileDispositionInformation = new FileDispositionInformation();
                                                        fileDispositionInformation.DeletePending = true;
                                                        status = webFileStore.SetFileInformation(fileHandle, fileDispositionInformation);
                                                    }
                                                    catch { }
                                                    finally
                                                    {
                                                        if (fileHandle != null)
                                                            webFileStore.CloseFile(fileHandle);

                                                        _telemetryClient.Flush();
                                                        Thread.Sleep(1000);
                                                    }

                                                }
                                            }
                                        }
                                        webFileStore.CloseFile(webdirectoryHandle);
                                    }
                                }
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}, has error.");
                    }
                    finally
                    {
                        if (webFileStore != null)
                            webFileStore.Disconnect();

                        smb.Disconnect();
                    }
                });

                await SetNextTimeTaskItemAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}, has error.");
            }

            _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is end.");
        }
    }
}
