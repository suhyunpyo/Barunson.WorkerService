using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.LMSBatchJob.Jobs
{
    /// <summary>
    /// 초안등록 3일후 미결제고객에게 알림톡 바/비/더/프/몰
    /// </summary>
    internal class SendChoanConfirmBizTalk: LMSBaseJob
    {
        public SendChoanConfirmBizTalk(ILogger logger, IServiceProvider services, BarShopContext taskContext, TelemetryClient tc, IMailSendService mail, ILMSSendService mms
            , string workerName)
            : base(logger, services, taskContext, tc, mail, mms, workerName, "SendChoanConfirmBizTalk", "10 11 * * *")
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

                var fromDt = Now.AddDays(-4).Date;
                var toDt = Now.AddDays(-3).Date;
                var sendModels = new List<BizTalkModel>();

                using (var fncScope = _serviceProvider.CreateScope())
                {
                    var barshopContext = fncScope.ServiceProvider.GetRequiredService<BarShopContext>();

                    // 발송 비즈톡 정보
                    var bizTemplateQ = from m in barshopContext.wedd_biztalk
                                       where m.USE_YORN == "Y"
                                       && (m.div == "초특급초안확정유도" || m.div == "초안확정유도")
                                       select m;
                    var bizTemplateList = await bizTemplateQ.ToListAsync(cancellationToken);

                    //발송대상
                    var query = from c in barshopContext.custom_order
                                join s in barshopContext.S2_Card on c.card_seq equals s.Card_Seq
                                where (c.status_seq == 7 || c.status_seq == 8)
                                && (c.settle_status == 0 || c.settle_status == 1)
                                && c.src_compose_date > fromDt && c.src_compose_date < toDt
                                && c.sales_Gubun != "SD"
                                && c.member_id != "s4guest"
                                select new
                                {
                                    c.order_seq,
                                    c.order_name,
                                    c.order_hphone,
                                    s.Card_Name,
                                    c.company_seq,
                                    c.isSpecial,
                                    sales_Gubun = (c.sales_Gubun == "C" || c.sales_Gubun == "H") ? "B" : c.sales_Gubun
                                };
                    var items = await query.ToListAsync(cancellationToken);

                    foreach (var item in items)
                    {
                        var bizTemplate = bizTemplateList.FirstOrDefault(m => m.sales_gubun == item.sales_Gubun && m.div == (item.isSpecial == "1" ? "초특급초안확정유도" : "초안확정유도"));
                        if (bizTemplate == null)
                            continue;

                        var message = bizTemplate.content.Replace("#{name}", item.order_name.Trim())
                                        .Replace("#{0000000}", item.order_seq.ToString())
                                        .Replace("#{상품명}", item.Card_Name);

                        sendModels.Add(new BizTalkModel
                        {
                            SendTime = DateTime.Now,
                            Subject = bizTemplate.lms_subject,
                            Message = message,
                            CallBack = bizTemplate.callback,
                            RecipientNum = item.order_hphone,
                            MessageType = (BizTalkMsgType)bizTemplate.msg_type,
                            Senderkey = bizTemplate.sender_key,
                            TemplateCode = bizTemplate.template_code,
                            KKOBtnType = bizTemplate.kko_btn_type,
                            KKOBtnInfo = bizTemplate.kko_btn_info,
                            EtcText1 = item.sales_Gubun,
                            EtcText2 = $"{WorkerName}-{funcName}",
                            EtcNum1 = item.company_seq
                        });
                    }
                }
                var success = await _mms.SendBizTalkAsync(sendModels, cancellationToken);

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
