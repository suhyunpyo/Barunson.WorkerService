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
    /// 주문모니터링, 매 1시간
    /// </summary>
    internal class OrderMonitoring: LMSBaseJob
    {
        public OrderMonitoring(ILogger logger, IServiceProvider services, BarShopContext taskContext, TelemetryClient tc, IMailSendService mail, ILMSSendService mms
            , string workerName)
            : base(logger, services, taskContext, tc, mail, mms, workerName, "OrderMonitoring", "5 */1 * * *")
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

                var taskItem = await GetCurruntWorkTask();

                var configs = JsonSerializer.Deserialize<List<OrderFailConfig>>(taskItem.ConfigJson);
                var checkdate = DateTime.Now.AddDays(-1);
                var saleGubuns = new List<string>() { "B", "H", "SS", "SB", "SD" };
                var statusSeqs = new List<int>() { 1, 4, 10, 12 };

                var smsList = new List<SmsSendModel>();

                using (var fncScope = _serviceProvider.CreateScope())
                {
                    var barshopContext = fncScope.ServiceProvider.GetRequiredService<BarShopContext>();
                    var barunsoncontext = fncScope.ServiceProvider.GetRequiredService<BarunsonContext>();

                    #region 바,프,몰,디

                    //sales_Gubun   B:제휴,H:프페 제휴,  SS:프페,SB: 바른손, ST:더카드, SD: 디얼디어, 
                    var orderquery = from a in barshopContext.custom_order
                                     where a.order_date > checkdate
                                     && saleGubuns.Contains(a.sales_Gubun)
                                     group a by a.sales_Gubun into g
                                     select new
                                     {
                                         SaleGubun = g.Key,
                                         LastOrderDate = g.Max(m => m.order_date),
                                         LastOrderSeq = g.Max(m => m.order_seq)
                                     };
                    var orderItems = await orderquery.ToListAsync(cancellationToken);

                    var sampleQuery = from a in barshopContext.CUSTOM_SAMPLE_ORDER
                                      where a.REQUEST_DATE > checkdate
                                      && saleGubuns.Contains(a.SALES_GUBUN)
                                      && statusSeqs.Contains(a.STATUS_SEQ)
                                      group a by a.SALES_GUBUN into g
                                      select new
                                      {
                                          SaleGubun = g.Key,
                                          LastOrderDate = g.Max(m => m.REQUEST_DATE),
                                          LastOrderSeq = g.Max(m => m.sample_order_seq)
                                      };
                    var sampleItems = await sampleQuery.ToListAsync(cancellationToken);

                    //바른손카드
                    smsList.AddRange(
                        OrderMonitoringCheck(configs.Single(m => m.SiteName == "바른손카드"),
                            new Dictionary<string, (DateTime?, string)> {
                                        { "청첩장", (orderItems.FirstOrDefault(m => m.SaleGubun == "SB")?.LastOrderDate, orderItems.FirstOrDefault(m => m.SaleGubun == "SB")?.LastOrderSeq.ToString()) },
                                        { "샘플", (sampleItems.FirstOrDefault(m => m.SaleGubun == "SB")?.LastOrderDate, sampleItems.FirstOrDefault(m => m.SaleGubun == "SB")?.LastOrderSeq.ToString()) }
                            })
                        );
                    //프리미어페이퍼
                    smsList.AddRange(
                        OrderMonitoringCheck(configs.Single(m => m.SiteName == "프리미어페이퍼"),
                            new Dictionary<string, (DateTime?, string)> {
                                        { "청첩장", (orderItems.FirstOrDefault(m => m.SaleGubun == "SS")?.LastOrderDate, orderItems.FirstOrDefault(m => m.SaleGubun == "SS")?.LastOrderSeq.ToString()) },
                                        { "샘플", (sampleItems.FirstOrDefault(m => m.SaleGubun == "SS")?.LastOrderDate, sampleItems.FirstOrDefault(m => m.SaleGubun == "SS")?.LastOrderSeq.ToString()) }
                            })
                        );
                    //디얼디어
                    smsList.AddRange(
                        OrderMonitoringCheck(configs.Single(m => m.SiteName == "디얼디어"),
                            new Dictionary<string, (DateTime?, string)> {
                                        { "청첩장", (orderItems.FirstOrDefault(m => m.SaleGubun == "SD")?.LastOrderDate, orderItems.FirstOrDefault(m => m.SaleGubun == "SD")?.LastOrderSeq.ToString()) },
                                        { "샘플", (sampleItems.FirstOrDefault(m => m.SaleGubun == "SD")?.LastOrderDate, sampleItems.FirstOrDefault(m => m.SaleGubun == "SD")?.LastOrderSeq.ToString()) }
                            })
                        );
                    //바른손몰
                    smsList.AddRange(
                        OrderMonitoringCheck(configs.Single(m => m.SiteName == "바른손몰"),
                            new Dictionary<string, (DateTime?, string)> {
                                        { "청첩장", (orderItems.FirstOrDefault(m => m.SaleGubun == "B")?.LastOrderDate, orderItems.FirstOrDefault(m => m.SaleGubun == "B")?.LastOrderSeq.ToString()) },
                                        { "샘플", (sampleItems.FirstOrDefault(m => m.SaleGubun == "B")?.LastOrderDate, sampleItems.FirstOrDefault(m => m.SaleGubun == "B")?.LastOrderSeq.ToString()) }
                            })
                        );
                    #endregion

                    #region 모초
                    var morderQuery = from a in barunsoncontext.TB_Order
                                      where a.Order_DateTime > checkdate
                                      && a.Order_Status_Code == "OSC01"
                                      orderby a.Order_DateTime descending
                                      select new { a.Order_DateTime, a.Order_Code };
                    var mLastOrderTime = await morderQuery.FirstOrDefaultAsync(cancellationToken);

                    var mflaorderQuery = from a in barunsoncontext.TB_Order_PartnerShip
                                         join b in barunsoncontext.TB_Order on a.Order_ID equals b.Order_ID
                                         where a.P_OrderDate > checkdate
                                         orderby a.P_OrderDate descending
                                         select new { a.P_OrderDate, b.Order_Code };
                    var mflaLastOrderTime = await mflaorderQuery.FirstOrDefaultAsync(cancellationToken);

                    smsList.AddRange(
                        OrderMonitoringCheck(configs.Single(m => m.SiteName == "모바일초대장"),
                            new Dictionary<string, (DateTime?, string)> {
                                        { "모초", (mLastOrderTime?.Order_DateTime, mLastOrderTime?.Order_Code)},
                                        { "모초화환", (mflaLastOrderTime?.P_OrderDate, mflaLastOrderTime?.Order_Code) }
                            })
                        );
                    #endregion
                    if (smsList.Count > 0)
                    {
                        //로그 기록
                        foreach (var sms in smsList)
                        {
                            var logDetail = new OrderFailLog
                            {
                                SiteName = sms.Reserved5,
                                OrderName = sms.Reserved4,
                                LastOrderTime = string.IsNullOrEmpty(sms.Reserved3) ? (DateTime?)null : DateTime.Parse(sms.Reserved3)
                            };
                            barshopContext.BarunWorkerLog.Add(new BarunWorkerLog
                            {
                                LogTime = sms.SendTime,
                                FunctionName = taskItem.FunctionName,
                                WorkerName = taskItem.WorkerName,
                                LogDetail = JsonSerializer.Serialize(logDetail)
                            });

                        }
                        await barshopContext.SaveChangesAsync(cancellationToken);

                        //발송
                        await _mms.SendSMSAsync(smsList, cancellationToken);
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
        private List<SmsSendModel> OrderMonitoringCheck(OrderFailConfig config, Dictionary<string, (DateTime?, string)> lastOrder)
        {
            var now = DateTime.Now;
            var nowtime = TimeOnly.FromDateTime(now);
            var smsList = new List<SmsSendModel>();

            //예외시간 이면 중지
            if ((config.UnCheckTimeFrom < config.UnCheckTimeTo) && (config.UnCheckTimeFrom <= nowtime && config.UnCheckTimeTo >= nowtime))
                return smsList;
            if ((config.UnCheckTimeFrom > config.UnCheckTimeTo) && (config.UnCheckTimeFrom <= nowtime || config.UnCheckTimeTo >= nowtime))
                return smsList;

            //수신대상 없으면 중지
            if (config.SmsTargets.Count == 0)
                return smsList;

            var msgFormat = "<{0} 주문>\r\n[{1}] 마지막 주문 ({2},{3})이후 {4}분 동안 주문없음";

            var defaultInfo = ILMSSendService.LMSSiteInfos["SB"];
            var dest = string.Join("|", config.SmsTargets.Select(x => $"{x.Name}^{x.PhoneNum.Replace("-", "")}"));
            foreach (var orderConfig in config.CheckMinute)
            {
                var checkTime = now.AddMinutes(orderConfig.Value * -1);

                //check 안함 조건
                if (orderConfig.Value == 0 || TimeOnly.FromDateTime(checkTime) < config.UnCheckTimeTo)
                    continue;

                if (lastOrder.ContainsKey(orderConfig.Key) && (lastOrder[orderConfig.Key].Item1 == null || lastOrder[orderConfig.Key].Item1 < checkTime))
                {
                    var lastOrderSeq = lastOrder[orderConfig.Key].Item2 ?? "N/A";
                    var lastOrderTime = (lastOrder[orderConfig.Key].Item1 != null) ? lastOrder[orderConfig.Key].Item1?.ToString("MM-dd HH:mm") : "N/A";
                    //SMS 발송
                    smsList.Add(new SmsSendModel
                    {
                        UserId = "",
                        Subject = "",
                        Message = String.Format(msgFormat, orderConfig.Key, config.ShortName, lastOrderSeq, lastOrderTime, orderConfig.Value),
                        ScheduledType = LMSScheduledType.Immediate,
                        SendTime = now,
                        CallBack = defaultInfo.CallBack,
                        DestCount = 1,
                        DestInfo = dest,
                        Reserved1 = "SB",
                        Reserved2 = "주문모니터",
                        Reserved3 = lastOrder[orderConfig.Key].Item1?.ToString("yyyy-MM-dd HH:mm:ss"),
                        Reserved4 = orderConfig.Key,
                        Reserved5 = config.SiteName
                    });
                }
            }

            return smsList;

        }
    }
}
