using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.DBModels.Barunson;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.CommonBatchJob.Jobs
{
    /// <summary>
    /// 무통장 결제 대기 상태에서 3일 이상 경과한 주문건을 일괄 입금대기 취소 처리
    /// 매일 오전 0:10
    /// </summary>
    internal class MobileCardDepositWaitingOrder : BaseJob
    {
        public MobileCardDepositWaitingOrder(ILogger<Worker> logger, IServiceProvider services, BarShopContext barShopContext,
            TelemetryClient tc, IMailSendService mail, string workerName)
            : base(logger, services, barShopContext, tc, mail, workerName, "MobileCardDepositWaitingOrder", "10 0 * * *")
        { }

        public override async Task Excute(CancellationToken cancellationToken)
        {
            try
            {
                if (!await IsExecute(cancellationToken))
                    return;
                _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is working.");
                var Now = DateTime.Now;

                using (var fncScope = _serviceProvider.CreateScope())
                {
                    var barunsonContext = fncScope.ServiceProvider.GetRequiredService<BarunsonContext>();

                    using (var trans = await barunsonContext.Database.BeginTransactionAsync(cancellationToken))
                    {
                        //입금대기 주문 목록
                        var query = from o in barunsonContext.TB_Order
                                    where o.Payment_Status_Code == "PSC04"
                                        && o.Deposit_DeadLine_DateTime < Now
                                    select o;
                        var items = await query.ToListAsync(cancellationToken);
                        foreach (var item in items)
                        {
                            //입금대기취소 
                            item.Payment_Status_Code = "PSC05";
                            item.Cancel_DateTime = Now;

                            // REFUND 테이블에 입금대기취소 / 입금대기취소완료로 추가
                            var refundItem = new TB_Refund_Info
                            {
                                Order_ID = item.Order_ID,
                                Refund_Type_Code = "RTC05",
                                Refund_Price = 0,
                                Bank_Type_Code = "",
                                AccountNumber = "",
                                Refund_Status_Code = "RSC05",
                                Depositor_Name = "",
                                Refund_Content = "",
                                Regist_DateTime = Now,
                                Refund_DateTime = Now
                            };
                            barunsonContext.TB_Refund_Info.Add(refundItem);

                            //쿠폰 사용 제거
                            var couponQ = from m in barunsonContext.TB_Order_Coupon_Use
                                          where m.Order_ID == item.Order_ID
                                          select m;
                            var couponItem = await couponQ.FirstOrDefaultAsync(cancellationToken);
                            if (couponItem != null)
                            {
                                var cpItem = await (from m in barunsonContext.TB_Coupon_Publish
                                                    where m.Coupon_Publish_ID == couponItem.Coupon_Publish_ID
                                                    select m).FirstOrDefaultAsync();
                                if (cpItem != null)
                                {
                                    cpItem.Use_YN = "N";
                                    cpItem.Use_DateTime = null;
                                }
                                barunsonContext.TB_Order_Coupon_Use.Remove(couponItem);
                            }
                        }

                        await barunsonContext.SaveChangesAsync(cancellationToken);
                        await trans.CommitAsync(cancellationToken);
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
