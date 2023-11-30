using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.DBModels.BarShop;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.LMSBatchJob.Jobs
{
    /// <summary>
    /// 초안확인!/체크사항확인후 초안확정및인쇄요청
    /// </summary>
    internal class SendPreSettleSMS: LMSBaseJob
    {
        public SendPreSettleSMS(ILogger logger, IServiceProvider services, BarShopContext taskContext, TelemetryClient tc, IMailSendService mail, ILMSSendService mms
            , string workerName)
            : base(logger, services, taskContext, tc, mail, mms, workerName, "SendPreSettleSMS", "0 10 * * *")
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
                var smsMsg = "[{0}]초안확인!/체크사항확인후 초안확정및인쇄요청 클릭해야 인쇄진행됩니다.";

                var fromDt = Now.AddDays(-1).Date;
                var toDt = Now.Date;

                var smsList = new List<SmsSendModel>();
                var comSeqs = new List<int> { 5001, 5003 };
                using (var fncScope = _serviceProvider.CreateScope())
                {
                    var barshopContext = fncScope.ServiceProvider.GetRequiredService<BarShopContext>();

                    var targets = new List<PreSettle>();

                    var shopQ = from o in barshopContext.custom_order
                                where (o.status_seq == 7 || o.status_seq == 8)
                                && o.settle_status == 2
                                && ((o.src_compose_date >= fromDt && o.src_compose_date < toDt) || (o.src_compose_mod_date >= fromDt && o.src_compose_mod_date < toDt) || (o.settle_date >= fromDt && o.settle_date < toDt))
                                && comSeqs.Contains(o.company_seq.Value)
                                && o.member_id != "s4guest"
                                select new PreSettle { PhoneNum = o.order_hphone.Replace("-", ""), Site = o.sales_Gubun, OrderSeq = o.order_seq };
                    targets.AddRange(await shopQ.ToListAsync(cancellationToken));

                    var mallQ = from o in barshopContext.custom_order
                                where (o.status_seq == 7 || o.status_seq == 8)
                                && o.settle_status == 2
                                && ((o.src_compose_date >= fromDt && o.src_compose_date < toDt) || (o.src_compose_mod_date >= fromDt && o.src_compose_mod_date < toDt) || (o.settle_date >= fromDt && o.settle_date < toDt))
                                && o.member_id != "s4guest"
                                && (o.sales_Gubun == "B" || o.sales_Gubun == "H" || o.sales_Gubun == "C" || o.sales_Gubun == "SD") // 디얼디어추가(강주연님 요청 231128)
                                select new PreSettle { PhoneNum = o.order_hphone.Replace("-", ""), Site = o.sales_Gubun, OrderSeq = o.order_seq };
                    targets.AddRange(await mallQ.ToListAsync(cancellationToken));

                    foreach (var group in targets.GroupBy(g => new { g.PhoneNum, g.Site }))
                    {
                        var defaultInfo = ILMSSendService.LMSSiteInfos["SB"];
                        if (!string.IsNullOrEmpty(group.Key.Site) && ILMSSendService.LMSSiteInfos.ContainsKey(group.Key.Site))
                            defaultInfo = ILMSSendService.LMSSiteInfos[group.Key.Site];

                        var msg = String.Format(smsMsg, defaultInfo.Brand);

                        smsList.Add(new SmsSendModel
                        {
                            UserId = "",
                            Subject = "",
                            Message = msg,
                            ScheduledType = LMSScheduledType.Immediate,
                            SendTime = DateTime.Now,
                            CallBack = defaultInfo.CallBack,
                            DestCount = 1,
                            DestInfo = $"AA^{group.Key.PhoneNum}",
                            Reserved1 = group.Key.Site,
                            Reserved2 = "",
                            Reserved3 = "",
                            Reserved4 = "",
                            Reserved5 = ""
                        });

                        foreach (var item in group)
                        {
                            barshopContext.CUSTOM_ORDER_ADMIN_MENT.Add(new CUSTOM_ORDER_ADMIN_MENT
                            {
                                ISWOrder = "1",
                                MENT = msg,
                                ORDER_SEQ = item.OrderSeq,
                                PCHECK = null,
                                STATUS = 0,
                                ADMIN_ID = "admin",
                                REG_DATE = DateTime.Now,
                                isJumun = "1",
                                intype = 4,
                                sgubun = "",
                                stype = "기타",
                                Mtype = "AM01",
                                Category = "AMC0701"
                            });
                        }
                    }
                    var successMMS = await _mms.SendSMSAsync(smsList, cancellationToken);
                    if (successMMS)
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
        private class PreSettle
        {
            public string PhoneNum { get; set; }
            public string Site { get; set; }
            public int OrderSeq { get; set; }
        }
    }
}
