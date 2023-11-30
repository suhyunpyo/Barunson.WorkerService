using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.LMSBatchJob.Jobs
{
    /// <summary>
    /// 입고 알림 문자 발송
    /// </summary>
    internal class SendRestockSMS: LMSBaseJob
    {
        public SendRestockSMS(ILogger logger, IServiceProvider services, BarShopContext taskContext, TelemetryClient tc, IMailSendService mail, ILMSSendService mms
            , string workerName)
            : base(logger, services, taskContext, tc, mail, mms, workerName, "SendRestockSMS", "10 9,12,15,19 * * *")
        {
        }

        public override async Task Excute(CancellationToken cancellationToken)
        {
            try
            {
                if (!await IsExecute(cancellationToken))
                    return;

                _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is working.");

                var Now = DateTime.Now;
                #region MMS Template

                var subject = @"[{0}] {1} 제품이 입고되었습니다";  //Brand, card code
                var mmsMsg = @"[{0}]고객님! {1}({2})제품이 입고되었습니다. {0} 사이트에서 확인하세요"; //Brand, card code,CARD_name

                #endregion

                var sendModels = new List<MmsSendModel>();
                using (var fncScope = _serviceProvider.CreateScope())
                {
                    var barshopContext = fncScope.ServiceProvider.GetRequiredService<BarShopContext>();
                    //발송대상
                    var targetQ = from s in barshopContext.S4_Stock_Alarm
                                  join c in barshopContext.S2_CardSalesSite on new { comseq = s.company_seq, cardseq = s.card_seq } equals new { comseq = c.Company_Seq, cardseq = c.card_seq }
                                  join sc in barshopContext.S2_Card on s.card_seq equals sc.Card_Seq
                                  where s.isAlarm_send == "N" && c.IsJumun == "1" && c.IsDisplay == "1"
                                  select new { Alarm = s, CardCode = sc.Card_Code, CardName = sc.Card_Name };

                    var targets = await targetQ.ToListAsync(cancellationToken);
                    foreach (var target in targets)
                    {
                        var defaultInfo = ILMSSendService.LMSSiteInfos["SB"];
                        if (ILMSSendService.LMSSiteInfos.FirstOrDefault(x => x.Value.CompaySeq == target.Alarm.company_seq).Value != null)
                            defaultInfo = ILMSSendService.LMSSiteInfos.FirstOrDefault(x => x.Value.CompaySeq == target.Alarm.company_seq).Value;

                        sendModels.Add(new MmsSendModel
                        {
                            UserId = "",
                            Subject = String.Format(subject, defaultInfo.Brand, target.CardCode),
                            Message = String.Format(mmsMsg, defaultInfo.Brand, target.CardCode, target.CardName),
                            ScheduledType = LMSScheduledType.Immediate,
                            SendTime = DateTime.Now,
                            CallBack = defaultInfo.CallBack,
                            DestCount = 1,
                            DestInfo = $"AA^{target.Alarm.hand_phone1 + target.Alarm.hand_phone2 + target.Alarm.hand_phone3}",
                            ContentCount = 0,
                            ContentData = "",
                            MsgType = LMSMessageType.Text,
                            Reserved1 = "TR_ETC2",
                            Reserved2 = "재입고 알리미",
                            Reserved3 = "",
                            Reserved4 = "",
                            Reserved5 = ""
                        });

                        target.Alarm.isAlarm_send = "Y";
                        target.Alarm.send_date = DateTime.Now;
                        barshopContext.Update(target.Alarm);
                    }

                    var success = await _mms.SendMMSAsync(sendModels, cancellationToken);
                    if (success)
                    {
                        await barshopContext.SaveChangesAsync(cancellationToken);
                    }
                }

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
