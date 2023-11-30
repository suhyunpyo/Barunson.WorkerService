using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.DBModels.Barunson;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.CommonBatchJob.Jobs
{
    /// <summary>
    /// 바른손M카드_쿠폰_자동_발행
    /// 모바일 청첩장을 구매한 고객(유/무료 구매 상관없음)에게 쿠폰 발급
    /// 쿠폰 ID가 이미 발급되어 있으면 무시
    /// 10분간격
    /// </summary>
    internal class MobileOrderCouponPublish : BaseJob
    {
        public MobileOrderCouponPublish(ILogger<Worker> logger, IServiceProvider services, BarShopContext barShopContext,
            TelemetryClient tc, IMailSendService mail, string workerName)
            : base(logger, services, barShopContext, tc, mail, workerName, "MobileOrderCouponPublish", "0/10 * * * *")
        { }
        public override async Task Excute(CancellationToken cancellationToken)
        {
            try
            {
                if (!await IsExecute(cancellationToken))
                    return;
                _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is working.");
                var Now = DateTime.Now;
                var count = 0;

                using (var fncScope = _serviceProvider.CreateScope())
                {
                    var barunsonContext = fncScope.ServiceProvider.GetRequiredService<BarunsonContext>();
                    var barshopContext = fncScope.ServiceProvider.GetRequiredService<BarShopContext>();

                    //바른손M카드_쿠폰_자동_발행
                    //자동발행 쿠폰 정보, 청첩장 결제완료 + 고객 컨펌 완료 
                    var couponInfos = await GetAutoPublishCouponInfoAsync(barunsonContext, "PTC01", cancellationToken);

                    if (couponInfos.Count > 0)
                    {
                        var targetDate = Now.Date.AddMonths(-3);

                        //초안컨펌완료, 결제완료, 디디제외, 사고건 제외,청첩장주문건, 
                        var weddingQ = from o in barshopContext.custom_order
                                       where o.status_seq >= 9 && o.settle_status == 2 && o.sales_Gubun != "SD"
                                           && o.pay_Type != "4" && (o.order_type == "1" || o.order_type == "6" || o.order_type == "7")
                                           && o.src_confirm_date > targetDate
                                           && !string.IsNullOrWhiteSpace(o.member_id)
                                       group o by o.member_id into g
                                       select g.Key;
                        var weddingItems = await weddingQ.ToListAsync(cancellationToken);
                        if (weddingItems.Count > 0)
                        {
                            count += await SetAutoPublishCouponAsync(barunsonContext, couponInfos, weddingItems, cancellationToken);
                        }
                    }

                    //모바일 청첩장을 구매
                    //자동발행 쿠폰 정보, 모초결제완료
                    couponInfos = await GetAutoPublishCouponInfoAsync(barunsonContext, "PTC03", cancellationToken);
                    if (couponInfos.Count > 0)
                    {
                        var targetDate = Now.Date.AddDays(-7);

                        //모초, 결제완료, 
                        var weddingQ = from o in barunsonContext.TB_Order
                                       join op in barunsonContext.TB_Order_Product on o.Order_ID equals op.Order_ID
                                       join p in barunsonContext.TB_Product on op.Product_ID equals p.Product_ID
                                       where p.Product_Category_Code == "PCC01"  //모초
                                           && o.Payment_Status_Code == "PSC02"   //결제완료
                                           && o.Payment_DateTime > targetDate
                                           && !string.IsNullOrWhiteSpace(o.User_ID)
                                       group o by o.User_ID into g
                                       select g.Key;
                        var weddingItems = await weddingQ.ToListAsync(cancellationToken);
                        if (weddingItems.Count > 0)
                        {
                            count += await SetAutoPublishCouponAsync(barunsonContext, couponInfos, weddingItems, cancellationToken);
                        }
                    }
                }
                await SetNextTimeTaskItemAsync(cancellationToken);
                _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is end., Publish Coupon Count: {count}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}, has error.");
            }
        }

        #region 자동발행 쿠폰 정보
        /// <summary>
        /// 자동발급 되는 쿠폰 정보 출력
        /// 출력사전: CouponID, Expiration_Date
        /// </summary>
        /// <param name="barunsonContext"></param>
        /// <param name="targetCode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<Dictionary<int, string>> GetAutoPublishCouponInfoAsync(BarunsonContext barunsonContext, string targetCode, CancellationToken cancellationToken)
        {
            var couponInfos = new Dictionary<int, string>();
            var couponQ = from m in barunsonContext.TB_Coupon
                          where m.Publish_Method_Code == "PMC01"    //자동발행
                            && m.Publish_Target_Code == targetCode
                          select new
                          {
                              m.Coupon_ID,
                              m.Period_Method_Code,
                              m.Publish_Period_Code,
                              m.Publish_End_Date
                          };
            var coupons = await couponQ.ToListAsync(cancellationToken);
            coupons.ForEach(x =>
            {
                string Expiration_Date = null;
                if (x.Period_Method_Code == "PMC01") //날짜 지정
                {
                    Expiration_Date = x.Publish_End_Date;
                }
                else if (x.Period_Method_Code == "PMC02") //발행일로부터
                {
                    TB_Common_Code code = barunsonContext.TB_Common_Code.Where(s => s.Code_Group == "Publish_Period_Code" && s.Code == x.Publish_Period_Code).FirstOrDefault();
                    Expiration_Date = DateTime.Now.AddDays(Convert.ToInt32(code.Code_Name)).ToString("yyyy-MM-dd");
                }
                couponInfos.Add(x.Coupon_ID, Expiration_Date);
            });

            return couponInfos;
        }

        /// <summary>
        /// 쿠폰 발행
        /// 이미 발행된 쿠폰이 CoupponID 있으면 발행하지 않음 
        /// </summary>
        /// <param name="barunsonContext"></param>
        /// <param name="couponInfos">쿠폰정보</param>
        /// <param name="users">대상</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<int> SetAutoPublishCouponAsync(BarunsonContext barunsonContext, Dictionary<int, string> couponInfos, List<string> users, CancellationToken cancellationToken)
        {
            var Now = DateTime.Now;
            foreach (var user in users)
            {
                var userid = user.Trim();

                var existsCopupons = await (from m in barunsonContext.TB_Coupon_Publish
                                            where m.User_ID == userid
                                            select new { m.User_ID, m.Coupon_ID }).ToListAsync(cancellationToken);

                foreach (var co in couponInfos)
                {
                    //이미 발급된 Coupon ID가 존재시 발급하지 않음.
                    if (existsCopupons.Any(x => x.Coupon_ID == co.Key))
                        continue;

                    var newCoupon = new TB_Coupon_Publish
                    {
                        Coupon_ID = co.Key,
                        User_ID = userid,
                        Use_YN = "N",
                        Use_DateTime = null,
                        Expiration_Date = co.Value,
                        Retrieve_DateTime = null,
                        Regist_User_ID = "BATCH",
                        Regist_DateTime = Now,
                        Update_User_ID = "BATCH",
                        Update_DateTime = Now
                    };

                    barunsonContext.TB_Coupon_Publish.Add(newCoupon);
                }
            }
            return await barunsonContext.SaveChangesAsync(cancellationToken);
        }
        #endregion
    }

}
