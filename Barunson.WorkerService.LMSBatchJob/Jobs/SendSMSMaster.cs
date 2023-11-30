using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.DBModels.BarShop;
using Barunson.WorkerService.Common.Services;
using Barunson.WorkerService.LMSBatchJob.Models;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Barunson.WorkerService.LMSBatchJob.Jobs
{
    /// <summary>
    /// 광고, 정보 문자 발송
    /// </summary>
    internal class SendSMSMaster: LMSBaseJob
    {
        public SendSMSMaster(ILogger logger, IServiceProvider services, BarShopContext taskContext, TelemetryClient tc, IMailSendService mail, ILMSSendService mms
            , string workerName)
            : base(logger, services, taskContext, tc, mail, mms, workerName, "SendSMSMaster", "*/10 * * * *")
        {
        }

        public override async Task Excute(CancellationToken cancellationToken)
        {
            try
            {
                if (!await IsExecute(cancellationToken))
                    return;

                _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is working.");

                var now = DateTime.Now;
                using (var fncScope = _serviceProvider.CreateScope())
                {
                    var barshopContext = fncScope.ServiceProvider.GetRequiredService<BarShopContext>();

                    #region 테스트 발송 대상 
                    var testQuery = from m in barshopContext.SMSSendMaster
                                    where m.Status == (byte)SMSManagerStatus.WaitingTest
                                    orderby m.Id
                                    select m;

                    var testMasters = await testQuery.ToListAsync(cancellationToken);
                    foreach (var testMaster in testMasters)
                    {
                        var testTarget = await (from m in barshopContext.SMSSendTargetList
                                                where m.Id == testMaster.Id && m.Seq == 1
                                                select m).FirstOrDefaultAsync(cancellationToken);
                        if (testTarget != null)
                        {
                            var sendModels = new List<MmsSendModel>();
                            var sendData = GetSMSMasterTargetModel(testMaster, testTarget);

                            //테스트 발송은 대상을 변경 해야 함.
                            var targets = testMaster.TestSendTarget.Split('|');
                            sendData.DestCount = targets.Length;
                            sendData.DestInfo = testMaster.TestSendTarget;
                            sendModels.Add(sendData);

                            var success = await _mms.SendMMSAsync(sendModels, cancellationToken);
                            if (success)
                            {
                                testMaster.Status = (byte)SMSManagerStatus.ComplateTest;
                                await barshopContext.SaveChangesAsync(cancellationToken);
                            }
                        }
                    }
                    #endregion

                    #region  발송 
                    //발송 대상 읽기 (발송 대기(예약은 예약시간), 발송 중 미완료
                    //발송은 최대 5000 건
                    var query = from m in barshopContext.SMSSendMaster
                                where (m.Status == (byte)SMSManagerStatus.WaitingSend || m.Status == (byte)SMSManagerStatus.Sending)
                                    && (m.SendTime <= now || m.SendTime == null)
                                orderby m.Id
                                select m;
                    var masters = await query.ToListAsync(cancellationToken);
                    int maxCount = 5000; //최대 전송 수

                    foreach (var master in masters)
                    {
                        if (maxCount <= 0)
                            break;

                        //즉시 발송이고 발송 대기 일경우 발송시간 업데이트
                        if (master.Status == (byte)SMSManagerStatus.WaitingSend && master.SendType == (byte)SMSManagerSendType.Immediately)
                            master.SendTime = now;

                        //상태 전송 중 변경
                        master.Status = (byte)SMSManagerStatus.Sending;

                        //최대 발송 수 만큼 읽음.
                        var targets = await (from m in barshopContext.SMSSendTargetList
                                             where m.Id == master.Id && m.Seq > 0 && m.Status == (byte)SMSManagerStatus.WaitingSend
                                             orderby m.Seq
                                             select m)
                                            .Take(maxCount)
                                            .ToListAsync(cancellationToken);
                        //발송 대상 수
                        var targetCount = targets.Count;
                        //발송 대상이 없으면 발송 완료로 수정
                        if (targetCount == 0)
                        {
                            master.Status = (byte)SMSManagerStatus.Complated;
                            await barshopContext.SaveChangesAsync(cancellationToken);
                            continue;
                        }
                        maxCount -= targetCount;

                        foreach (var target in targets)
                        {
                            var sendModels = new List<MmsSendModel>();
                            var sendData = GetSMSMasterTargetModel(master, target);
                            sendModels.Add(sendData);
                            var success = await _mms.SendMMSAsync(sendModels, cancellationToken);
                            if (success)
                            {
                                master.SuccessCount += 1;
                                target.Status = (byte)SMSManagerStatus.Complated;
                                target.SendTime = now;
                            }
                            else
                            {
                                master.FailCount += 1;
                                target.Status = (byte)SMSManagerStatus.Fail;
                                target.StatusText = "발송 실패";
                                target.SendTime = now;
                            }
                            await barshopContext.SaveChangesAsync(cancellationToken);
                        }
                    }
                    #endregion
                }

                await SetNextTimeTaskItemAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}, has error.");
            }

            _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is end.");
        }

        private MmsSendModel GetSMSMasterTargetModel(SMSSendMaster master, SMSSendTargetList target)
        {
            var message = master.Message;
            message = message.Replace("{#이름}", target.Name ?? "");
            message = message.Replace("{#A}", target.A ?? "");
            message = message.Replace("{#B}", target.B ?? "");
            message = message.Replace("{#C}", target.C ?? "");
            message = message.Replace("{#D}", target.D ?? "");

            //이름^전화번호|이름^전화번호
            var destInfo = $"{target.Name}^{target.PhoneNo}";

            var ContentCount = 0;
            var ContentData = "";
            //MMS 전송 시 
            //SDK_MMS_SEND 테이블 CONTENT_DATA 컬럼(250자까지 입력 가능)
            //입력데이터 패턴: 파일명 ^ 컨텐츠타입 ^ 컨텐츠서브타입 ex)https://www.test.com/test.jpg^1^0
            if (!string.IsNullOrEmpty(master.FIleContent))
            {
                var fileUrl = JsonSerializer.Deserialize<List<string>>(master.FIleContent);
                ContentCount = fileUrl.Count;
                ContentData = string.Join("|", fileUrl.Select(m => $"{m}^1^0"));
            }

            return new MmsSendModel
            {
                UserId = "",
                Subject = master.Title,
                Message = message,
                ScheduledType = LMSScheduledType.Immediate,
                SendTime = DateTime.Now,
                CallBack = master.SenderPhone,
                DestCount = 1,
                DestInfo = destInfo,
                ContentCount = ContentCount,
                ContentData = ContentData,
                MsgType = LMSMessageType.Text,
                Reserved1 = master.Id.ToString(),
                Reserved2 = target.Seq.ToString(),
                Reserved3 = "문자발송",
                Reserved4 = "",
                Reserved5 = ""
            };
        }
    }
}
