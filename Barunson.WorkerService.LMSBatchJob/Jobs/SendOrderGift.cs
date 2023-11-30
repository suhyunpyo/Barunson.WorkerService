using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.LMSBatchJob.Jobs
{
    /// <summary>
    /// 전일(00:00~24:00) 답례품 업체별 발생한 주문건/취소건 익일 AM08:00발송
    /// </summary>
    internal class SendOrderGift : LMSBaseJob
    {
        public SendOrderGift(ILogger logger, IServiceProvider services, BarShopContext taskContext, TelemetryClient tc, IMailSendService mail, ILMSSendService mms
            , string workerName)
            : base(logger, services, taskContext, tc, mail, mms, workerName, "SendOrderGift", "0 8 * * *")
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

                var smsMsg = "[바른손답례품] 전일주문건> 주문 {0} /  취소 {1}";
                var fromDt = Now.AddDays(-1).Date;
                var toDt = Now.Date;
                var smsList = new List<SmsSendModel>();

                using (var fncScope = _serviceProvider.CreateScope())
                {
                    var barshopContext = fncScope.ServiceProvider.GetRequiredService<BarShopContext>();
                    var orders = new List<OrderCount>();

                    #region 답례폼 주문/취소건 읽기
                    var orderYQ = from o in barshopContext.CUSTOM_ETC_ORDER
                                  join m in barshopContext.manage_code on o.order_type equals m.code
                                  where o.settle_date >= fromDt && o.settle_date < toDt
                                  && o.status_seq != 3 && o.status_seq != 5
                                  && m.code_type == "etcprod"
                                  group o by o.order_type into g
                                  select new
                                  {
                                      OrderType = g.Key,
                                      Count = g.Count()
                                  };
                    foreach (var yitem in await orderYQ.ToListAsync(cancellationToken))
                    {
                        orders.Add(new OrderCount
                        {
                            OrderType = yitem.OrderType,
                            OrderY = yitem.Count,
                            OrderN = 0
                        });
                    }

                    var orderNQ = from o in barshopContext.CUSTOM_ETC_ORDER
                                  join m in barshopContext.manage_code on o.order_type equals m.code
                                  where o.settle_Cancel_Date >= fromDt && o.settle_Cancel_Date < toDt && o.settle_date < fromDt
                                  && (o.status_seq == 3 || o.status_seq == 5)
                                  && m.code_type == "etcprod"
                                  group o by o.order_type into g
                                  select new
                                  {
                                      OrderType = g.Key,
                                      Count = g.Count()
                                  };
                    foreach (var nitem in await orderNQ.ToListAsync(cancellationToken))
                    {
                        var item = orders.SingleOrDefault(m => m.OrderType == nitem.OrderType);
                        if (item == null)
                        {
                            orders.Add(new OrderCount
                            {
                                OrderType = nitem.OrderType,
                                OrderY = 0,
                                OrderN = nitem.Count
                            });
                        }
                        else
                        {
                            item.OrderN = nitem.Count;
                        }
                    }
                    #endregion

                    #region 회사 전화번호 읽기
                    var comCods = orders.Select(m => m.OrderType).ToList();
                    var comTels = await (from m in barshopContext.gift_company_tel
                                         where comCods.Contains(m.code) && m.isYN == "Y" && !string.IsNullOrEmpty(m.company_tel)
                                         select new { m.code, m.company_tel }
                                         ).ToListAsync(cancellationToken);
                    #endregion

                    var defaultInfo = ILMSSendService.LMSSiteInfos["SB"];
                    foreach (var item in orders)
                    {
                        var comTel = comTels.FirstOrDefault(m => m.code == item.OrderType);
                        if (comTel != null)
                        {
                            smsList.Add(new SmsSendModel
                            {
                                UserId = "",
                                Subject = "",
                                Message = String.Format(smsMsg, item.OrderY, item.OrderN),
                                ScheduledType = LMSScheduledType.Immediate,
                                SendTime = DateTime.Now,
                                CallBack = defaultInfo.CallBack,
                                DestCount = 1,
                                DestInfo = $"AA^{comTel.company_tel}",
                                Reserved1 = "SB",
                                Reserved2 = "",
                                Reserved3 = "",
                                Reserved4 = "2",
                                Reserved5 = ""
                            });
                        }
                    }
                }
                var successMMS = await _mms.SendSMSAsync(smsList, cancellationToken);

                await SetNextTimeTaskItemAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}, has error.");
            }
            
            _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is end.");
        }

        private class OrderCount
        {
            public string OrderType { get; set; }
            public int OrderY { get; set; }
            public int OrderN { get; set; }
        };
    }
}
