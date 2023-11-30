using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.DBModels.Barunson;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.CommonBatchJob.Jobs
{
    /// <summary>
    /// 바른손M카드_송금_통계
    /// 바른손M카드_주문_통계
    /// 매일 오전 0:50
    /// 이전 3개월치 재계산 하도록 변경
    /// </summary>
    internal class MobileCardStatistice : BaseJob
    {
        public MobileCardStatistice(ILogger<Worker> logger, IServiceProvider services, BarShopContext barShopContext,
            TelemetryClient tc, IMailSendService mail, string workerName)
            : base(logger, services, barShopContext, tc, mail, workerName, "MobileCardStatistice", "50 0 * * *")
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
                    var barshopContext = fncScope.ServiceProvider.GetRequiredService<BarShopContext>();

                    var endDate = Now.Date;
                    var startDate = endDate.AddMonths(-3);

                    var fromDate = startDate;
                    var dateRange = new Dictionary<DateTime, DateTime>();
                    do
                    {
                        var nextdate = fromDate.AddDays(1);
                        dateRange.Add(fromDate, nextdate);
                        fromDate = nextdate;
                    }
                    while (fromDate < endDate);

                    #region 날짜 별 통계 계산

                    foreach (var workDate in dateRange)
                    {
                        var strWorkDate = workDate.Key.ToString("yyyyMMdd");

                        #region 기존 통계 산출 데이터 읽기, 없으면 생성

                        //전체현황
                        var statQuery = from m in barunsonContext.TB_Total_Statistic_Day
                                        where m.Date == strWorkDate
                                        select m;
                        var statItem = await statQuery.FirstOrDefaultAsync(cancellationToken);
                        if (statItem == null)
                        {
                            statItem = new TB_Total_Statistic_Day
                            {
                                Date = strWorkDate,
                                Free_Order_Count = 0,
                                Charge_Order_Count = 0,
                                Cancel_Count = 0,
                                Payment_Price = 0,
                                Cancel_Refund_Price = 0,
                                Profit_Price = 0,
                                Memberjoin_Count = 0
                            };
                            barunsonContext.TB_Total_Statistic_Day.Add(statItem);
                        }

                        //매출현황
                        var saleStatQuery = from m in barunsonContext.TB_Sales_Statistic_Day
                                            where m.Date == strWorkDate
                                            select m;
                        var saleStatItem = await saleStatQuery.FirstOrDefaultAsync(cancellationToken);
                        if (saleStatItem == null)
                        {
                            saleStatItem = new TB_Sales_Statistic_Day
                            {
                                Date = strWorkDate,
                                Barunn_Sales_Price = 0,
                                Barunn_Free_Order_Count = 0,
                                Barunn_Charge_Order_Count = 0,
                                Bhands_Sales_Price = 0,
                                Bhands_Charge_Order_Count = 0,
                                Bhands_Free_Order_Count = 0,
                                Thecard_Sales_Price = 0,
                                Thecard_Charge_Order_Count = 0,
                                Thecard_Free_Order_Count = 0,
                                Premier_Sales_Price = 0,
                                Premier_Free_Order_Count = 0,
                                Premier_Charge_Order_Count = 0,
                                Total_Sales_Price = 0,
                                Total_Charge_Order_Count = 0,
                                Total_Free_Order_Count = 0
                            };
                            barunsonContext.TB_Sales_Statistic_Day.Add(saleStatItem);
                        }

                        //구매수단
                        var payStatQuery = from m in barunsonContext.TB_Payment_Status_Day
                                           where m.Date == strWorkDate
                                           select m;
                        var payStatItem = await payStatQuery.FirstOrDefaultAsync(cancellationToken);
                        if (payStatItem == null)
                        {
                            payStatItem = new TB_Payment_Status_Day
                            {
                                Date = strWorkDate,
                                Card_Payment_Price = 0,
                                Account_Transfer_Price = 0,
                                Virtual_Account_Price = 0,
                                Etc_Price = 0,
                                Total_Price = 0,
                                Cancel_Refund_Price = 0,
                                Profit_Price = 0
                            };
                            barunsonContext.TB_Payment_Status_Day.Add(payStatItem);
                        }

                        //송금 일별 집계
                        var remitDayQ = from m in barunsonContext.TB_Remit_Statistics_Daily
                                        where m.Date == strWorkDate
                                        select m;
                        var remitDayItem = await remitDayQ.FirstOrDefaultAsync(cancellationToken);
                        if (remitDayItem == null)
                        {
                            remitDayItem = new TB_Remit_Statistics_Daily
                            {
                                Date = strWorkDate,
                                Remit_Count = 0,
                                Remit_Price = 0,
                                Tax = 0,
                                Calculate_Tax = 0,
                                Hits_Tax = 0,
                                User_Count = 0,
                                Account_Count = 0,
                                Remit_Tax = 0
                            };
                            barunsonContext.TB_Remit_Statistics_Daily.Add(remitDayItem);
                        }
                        #endregion

                        #region 일별 주문 통계 계산
                        var orderQuery = from o in barunsonContext.TB_Order
                                         join op in barunsonContext.TB_Order_Product on o.Order_ID equals op.Order_ID
                                         join p in barunsonContext.TB_Product on op.Product_ID equals p.Product_ID
                                         where o.Payment_DateTime >= workDate.Key && o.Payment_DateTime < workDate.Value
                                            && (o.Payment_Status_Code == "PSC02" || o.Payment_Status_Code == "PSC03")
                                         group new { o, p } by 1 into g
                                         select new
                                         {
                                             PayCount = g.Sum(x => x.o.Payment_Price > 0 ? 1 : 0),
                                             FreeCount = g.Sum(x => (x.o.Payment_Price == null || x.o.Payment_Price == 0) ? 1 : 0),
                                             TotalPrice = g.Sum(x => x.o.Payment_Price ?? 0),

                                             BcardPayCount = g.Sum(x => x.p.Product_Brand_Code == "PBC01" && x.o.Payment_Price > 0 ? 1 : 0),
                                             BcardFreeCount = g.Sum(x => x.p.Product_Brand_Code == "PBC01" && (x.o.Payment_Price == null || x.o.Payment_Price == 0) ? 1 : 0),
                                             BcardPrice = g.Sum(x => x.p.Product_Brand_Code == "PBC01" ? x.o.Payment_Price ?? 0 : 0),

                                             ThecardPayCount = g.Sum(x => x.p.Product_Brand_Code == "PBC03" && x.o.Payment_Price > 0 ? 1 : 0),
                                             ThecardFreeCount = g.Sum(x => x.p.Product_Brand_Code == "PBC03" && (x.o.Payment_Price == null || x.o.Payment_Price == 0) ? 1 : 0),
                                             ThecardPrice = g.Sum(x => x.p.Product_Brand_Code == "PBC03" ? x.o.Payment_Price ?? 0 : 0),

                                             PpcardPayCount = g.Sum(x => x.p.Product_Brand_Code == "PBC04" && x.o.Payment_Price > 0 ? 1 : 0),
                                             PpcardFreeCount = g.Sum(x => x.p.Product_Brand_Code == "PBC04" && (x.o.Payment_Price == null || x.o.Payment_Price == 0) ? 1 : 0),
                                             PpcardPrice = g.Sum(x => x.p.Product_Brand_Code == "PBC04" ? x.o.Payment_Price ?? 0 : 0),

                                             CardPayPrice = g.Sum(x => x.o.Payment_Method_Code == "PMC01" ? x.o.Payment_Price ?? 0 : 0),
                                             BankPayPrice = g.Sum(x => x.o.Payment_Method_Code == "PMC03" ? x.o.Payment_Price ?? 0 : 0),
                                             vBankPayPrice = g.Sum(x => x.o.Payment_Method_Code == "PMC02" ? x.o.Payment_Price ?? 0 : 0),
                                             EtcPayPrice = g.Sum(x => x.o.Payment_Method_Code == "PMC04" ? x.o.Payment_Price ?? 0 : 0),
                                         };
                        var orderItem = await orderQuery.FirstOrDefaultAsync(cancellationToken);
                        if (orderItem != null)
                        {
                            statItem.Charge_Order_Count = orderItem.PayCount;
                            statItem.Free_Order_Count = orderItem.FreeCount;
                            statItem.Payment_Price = orderItem.TotalPrice;
                            statItem.Profit_Price = orderItem.TotalPrice;

                            saleStatItem.Barunn_Charge_Order_Count = orderItem.BcardPayCount;
                            saleStatItem.Barunn_Free_Order_Count = orderItem.BcardFreeCount;
                            saleStatItem.Barunn_Sales_Price = orderItem.BcardPrice;
                            saleStatItem.Thecard_Charge_Order_Count = orderItem.ThecardPayCount;
                            saleStatItem.Thecard_Free_Order_Count = orderItem.ThecardFreeCount;
                            saleStatItem.Thecard_Sales_Price = orderItem.ThecardPrice;
                            saleStatItem.Premier_Charge_Order_Count = orderItem.PpcardPayCount;
                            saleStatItem.Premier_Free_Order_Count = orderItem.PpcardFreeCount;
                            saleStatItem.Premier_Sales_Price = orderItem.PpcardPrice;
                            saleStatItem.Total_Charge_Order_Count = orderItem.PayCount;
                            saleStatItem.Total_Free_Order_Count = orderItem.FreeCount;
                            saleStatItem.Total_Sales_Price = orderItem.TotalPrice;

                            payStatItem.Card_Payment_Price = orderItem.CardPayPrice;
                            payStatItem.Account_Transfer_Price = orderItem.BankPayPrice;
                            payStatItem.Virtual_Account_Price = orderItem.vBankPayPrice;
                            payStatItem.Etc_Price = orderItem.EtcPayPrice;
                            payStatItem.Total_Price = orderItem.TotalPrice;
                            payStatItem.Profit_Price = orderItem.TotalPrice;

                        }
                        #endregion

                        #region 일별 주문 취소 통계 계산
                        var cancelOrderQueyr = from o in barunsonContext.TB_Order
                                               join op in barunsonContext.TB_Order_Product on o.Order_ID equals op.Order_ID
                                               join p in barunsonContext.TB_Product on op.Product_ID equals p.Product_ID
                                               where o.Cancel_DateTime >= workDate.Key && o.Cancel_DateTime < workDate.Value
                                                    && o.Payment_Status_Code == "PSC03"
                                               group new { o, p } by 1 into g
                                               select new
                                               {
                                                   CancelCount = g.Count(),
                                                   CancelPrice = g.Sum(x => x.o.Payment_Price ?? 0),
                                                   BcardCancelPrice = g.Sum(x => x.p.Product_Brand_Code == "PBC01" ? x.o.Payment_Price ?? 0 : 0),
                                                   ThecardCancelPrice = g.Sum(x => x.p.Product_Brand_Code == "PBC03" ? x.o.Payment_Price ?? 0 : 0),
                                                   PpcardCancelPrice = g.Sum(x => x.p.Product_Brand_Code == "PBC04" ? x.o.Payment_Price ?? 0 : 0),
                                               };
                        var cancelOrderItem = await cancelOrderQueyr.FirstOrDefaultAsync(cancellationToken);
                        if (cancelOrderItem != null)
                        {
                            statItem.Cancel_Count = cancelOrderItem.CancelCount;
                            statItem.Cancel_Refund_Price = cancelOrderItem.CancelPrice;
                            statItem.Profit_Price = statItem.Payment_Price - cancelOrderItem.CancelPrice;

                            saleStatItem.Barunn_Sales_Price = saleStatItem.Barunn_Sales_Price - cancelOrderItem.BcardCancelPrice;
                            saleStatItem.Thecard_Sales_Price = saleStatItem.Thecard_Sales_Price - cancelOrderItem.ThecardCancelPrice;
                            saleStatItem.Premier_Sales_Price = saleStatItem.Premier_Sales_Price - cancelOrderItem.PpcardCancelPrice;
                            saleStatItem.Total_Sales_Price = saleStatItem.Total_Sales_Price - cancelOrderItem.CancelPrice;

                            payStatItem.Cancel_Refund_Price = cancelOrderItem.CancelPrice;
                            payStatItem.Profit_Price = payStatItem.Total_Price - cancelOrderItem.CancelPrice;
                        }
                        #endregion

                        #region 일별 유저 등록 통계 계산
                        var userQuery = from m in barshopContext.S2_UserInfo
                                        where m.reg_date >= workDate.Key && m.reg_date < workDate.Value
                                           && m.REFERER_SALES_GUBUN == "BM"
                                        group m by 1 into g
                                        select new
                                        {
                                            JoinCount = g.Select(x => x.uid).Distinct().Count()
                                        };
                        var userItem = await userQuery.FirstOrDefaultAsync(cancellationToken);
                        if (userItem != null)
                        {
                            statItem.Memberjoin_Count = userItem.JoinCount;
                        }

                        #endregion

                        #region 송금 통계 계산
                        //세금 기준 정보
                        var ctax = await (from m in barunsonContext.TB_Company_Tax
                                          where m.Apply_Start_Date.CompareTo(strWorkDate) <= 0
                                          orderby m.Company_Tax_ID descending
                                          select m
                                                  ).FirstAsync(cancellationToken);

                        //Hit 수
                        var hitCount = await barunsonContext.TB_Depositor_Hits
                                .Where(m => m.Request_Date == strWorkDate && m.Status_Code != null)
                                .Select(m => m.Depositor_Hits_ID)
                                .CountAsync(cancellationToken);

                        remitDayItem.Hits_Tax = hitCount * ctax.Hits_Tax;

                        //계좌 수
                        remitDayItem.Account_Count = await barunsonContext.TB_Account
                            .Where(m => m.Regist_DateTime >= workDate.Key && m.Regist_DateTime < workDate.Value)
                            .Select(m => m.Account_ID)
                            .CountAsync(cancellationToken);

                        //금액, 수, 수수료
                        var remitQ = from a in barunsonContext.TB_Remit
                                     join b in barunsonContext.TB_Invitation_Tax on a.Invitation_ID equals b.Invitation_ID
                                     join c in barunsonContext.TB_Tax on b.Tax_ID equals c.Tax_ID
                                     where a.Result_Code == "RC005" && a.Complete_Date == strWorkDate
                                     group new { a, c } by 1 into g
                                     select new
                                     {
                                         Total_Price = g.Sum(x => x.a.Total_Price ?? 0),
                                         Remit_Count = g.Count(),
                                         Tax = g.Sum(x => x.c.Tax)
                                     };
                        var remitI = await remitQ.FirstOrDefaultAsync(cancellationToken);
                        if (remitI != null)
                        {
                            remitDayItem.Remit_Price = remitI.Total_Price;
                            remitDayItem.Tax = remitI.Tax;
                            remitDayItem.Remit_Tax = remitI.Remit_Count * ctax.Remit_Tax;
                            remitDayItem.Calculate_Tax = remitI.Remit_Count * ctax.Calculate_Tax;
                            remitDayItem.Remit_Count = remitI.Remit_Count;
                        }
                        //이용자 수
                        var remitUQ = from a in barunsonContext.TB_Remit
                                      join b in barunsonContext.TB_Account on a.Account_ID equals b.Account_ID
                                      where a.Result_Code == "RC005" && a.Complete_Date == strWorkDate
                                      select b.User_ID;
                        remitDayItem.User_Count = await remitUQ.Distinct().CountAsync(cancellationToken);

                        #endregion

                        await barunsonContext.SaveChangesAsync(cancellationToken);
                    }
                    #endregion

                    #region 월 통계 재 계산
                    var workMon = startDate;
                    do
                    {
                        var strMon = workMon.ToString("yyyyMM");

                        #region 월별 전체현황 
                        var statMonItem = await (from m in barunsonContext.TB_Total_Statistic_Month
                                                 where m.Date == strMon
                                                 select m).FirstOrDefaultAsync(cancellationToken);
                        if (statMonItem == null)
                        {
                            statMonItem = new TB_Total_Statistic_Month
                            {
                                Date = strMon
                            };
                            barunsonContext.TB_Total_Statistic_Month.Add(statMonItem);
                        }
                        var orderSumQ = from m in barunsonContext.TB_Total_Statistic_Day
                                        where m.Date.StartsWith(strMon)
                                        group m by 1 into g
                                        select new
                                        {
                                            Free_Order_Count = g.Sum(x => x.Free_Order_Count),
                                            Charge_Order_Count = g.Sum(x => x.Charge_Order_Count),
                                            Cancel_Count = g.Sum(x => x.Cancel_Count),
                                            Payment_Price = g.Sum(x => x.Payment_Price),
                                            Cancel_Refund_Price = g.Sum(x => x.Cancel_Refund_Price),
                                            Profit_Price = g.Sum(x => x.Profit_Price),
                                            Memberjoin_Count = g.Sum(x => x.Memberjoin_Count)
                                        };
                        var orderSumItem = await orderSumQ.FirstOrDefaultAsync(cancellationToken);
                        if (orderSumItem != null)
                        {
                            statMonItem.Free_Order_Count = orderSumItem.Free_Order_Count;
                            statMonItem.Charge_Order_Count = orderSumItem.Charge_Order_Count;
                            statMonItem.Cancel_Count = orderSumItem.Cancel_Count;
                            statMonItem.Payment_Price = orderSumItem.Payment_Price;
                            statMonItem.Cancel_Refund_Price = orderSumItem.Cancel_Refund_Price;
                            statMonItem.Profit_Price = orderSumItem.Profit_Price;
                            statMonItem.Memberjoin_Count = orderSumItem.Memberjoin_Count;
                        }
                        #endregion

                        #region 월별 매출현황 
                        var saleMonItem = await (from m in barunsonContext.TB_Sales_Statistic_Month
                                                 where m.Date == strMon
                                                 select m).FirstOrDefaultAsync(cancellationToken);
                        if (saleMonItem == null)
                        {
                            saleMonItem = new TB_Sales_Statistic_Month
                            {
                                Date = strMon
                            };
                            barunsonContext.TB_Sales_Statistic_Month.Add(saleMonItem);
                        }
                        var saleSumQ = from m in barunsonContext.TB_Sales_Statistic_Day
                                       where m.Date.StartsWith(strMon)
                                       group m by 1 into g
                                       select new
                                       {
                                           Barunn_Sales_Price = g.Sum(x => x.Barunn_Sales_Price),
                                           Barunn_Free_Order_Count = g.Sum(x => x.Barunn_Free_Order_Count),
                                           Barunn_Charge_Order_Count = g.Sum(x => x.Barunn_Charge_Order_Count),
                                           Bhands_Sales_Price = g.Sum(x => x.Bhands_Sales_Price),
                                           Bhands_Free_Order_Count = g.Sum(x => x.Bhands_Free_Order_Count),
                                           Bhands_Charge_Order_Count = g.Sum(x => x.Bhands_Charge_Order_Count),
                                           Thecard_Sales_Price = g.Sum(x => x.Thecard_Sales_Price),
                                           Thecard_Free_Order_Count = g.Sum(x => x.Thecard_Free_Order_Count),
                                           Thecard_Charge_Order_Count = g.Sum(x => x.Thecard_Charge_Order_Count),
                                           Premier_Sales_Price = g.Sum(x => x.Premier_Sales_Price),
                                           Premier_Free_Order_Count = g.Sum(x => x.Premier_Free_Order_Count),
                                           Premier_Charge_Order_Count = g.Sum(x => x.Premier_Charge_Order_Count),
                                           Total_Sales_Price = g.Sum(x => x.Total_Sales_Price),
                                           Total_Free_Order_Count = g.Sum(x => x.Total_Free_Order_Count),
                                           Total_Charge_Order_Count = g.Sum(x => x.Total_Charge_Order_Count)
                                       };
                        var saleSumItem = await saleSumQ.FirstOrDefaultAsync(cancellationToken);
                        if (saleSumItem != null)
                        {
                            saleMonItem.Barunn_Sales_Price = saleSumItem.Barunn_Sales_Price;
                            saleMonItem.Barunn_Free_Order_Count = saleSumItem.Barunn_Free_Order_Count;
                            saleMonItem.Barunn_Charge_Order_Count = saleSumItem.Barunn_Charge_Order_Count;
                            saleMonItem.Bhands_Sales_Price = saleSumItem.Bhands_Sales_Price;
                            saleMonItem.Bhands_Free_Order_Count = saleSumItem.Bhands_Free_Order_Count;
                            saleMonItem.Bhands_Charge_Order_Count = saleSumItem.Bhands_Charge_Order_Count;
                            saleMonItem.Thecard_Sales_Price = saleSumItem.Thecard_Sales_Price;
                            saleMonItem.Thecard_Free_Order_Count = saleSumItem.Thecard_Free_Order_Count;
                            saleMonItem.Thecard_Charge_Order_Count = saleSumItem.Thecard_Charge_Order_Count;
                            saleMonItem.Premier_Sales_Price = saleSumItem.Premier_Sales_Price;
                            saleMonItem.Premier_Free_Order_Count = saleSumItem.Premier_Free_Order_Count;
                            saleMonItem.Premier_Charge_Order_Count = saleSumItem.Premier_Charge_Order_Count;
                            saleMonItem.Total_Sales_Price = saleSumItem.Total_Sales_Price;
                            saleMonItem.Total_Free_Order_Count = saleSumItem.Total_Free_Order_Count;
                            saleMonItem.Total_Charge_Order_Count = saleSumItem.Total_Charge_Order_Count;
                        }
                        #endregion

                        #region 월별 결제수단
                        var payMonItem = await (from m in barunsonContext.TB_Payment_Status_Month
                                                where m.Date == strMon
                                                select m).FirstOrDefaultAsync(cancellationToken);
                        if (payMonItem == null)
                        {
                            payMonItem = new TB_Payment_Status_Month
                            {
                                Date = strMon
                            };
                            barunsonContext.TB_Payment_Status_Month.Add(payMonItem);
                        }
                        var paySumQ = from m in barunsonContext.TB_Payment_Status_Day
                                      where m.Date.StartsWith(strMon)
                                      group m by 1 into g
                                      select new
                                      {
                                          Card_Payment_Price = g.Sum(x => x.Card_Payment_Price),
                                          Account_Transfer_Price = g.Sum(x => x.Account_Transfer_Price),
                                          Virtual_Account_Price = g.Sum(x => x.Virtual_Account_Price),
                                          Etc_Price = g.Sum(x => x.Etc_Price),
                                          Total_Price = g.Sum(x => x.Total_Price),
                                          Cancel_Refund_Price = g.Sum(x => x.Cancel_Refund_Price),
                                          Profit_Price = g.Sum(x => x.Profit_Price)
                                      };
                        var paySumItem = await paySumQ.FirstOrDefaultAsync(cancellationToken);
                        if (paySumItem != null)
                        {
                            payMonItem.Card_Payment_Price = paySumItem.Card_Payment_Price;
                            payMonItem.Account_Transfer_Price = paySumItem.Account_Transfer_Price;
                            payMonItem.Virtual_Account_Price = paySumItem.Virtual_Account_Price;
                            payMonItem.Etc_Price = paySumItem.Etc_Price;
                            payMonItem.Total_Price = paySumItem.Total_Price;
                            payMonItem.Cancel_Refund_Price = paySumItem.Cancel_Refund_Price;
                            payMonItem.Profit_Price = paySumItem.Profit_Price;
                        }
                        #endregion

                        #region 월별 송금 통계 계산
                        var strRemitMon = strMon + "00";
                        var remitMonQ = from m in barunsonContext.TB_Remit_Statistics_Daily
                                        where m.Date == strRemitMon
                                        select m;
                        var remitMonItem = await remitMonQ.FirstOrDefaultAsync(cancellationToken);
                        if (remitMonItem == null)
                        {
                            remitMonItem = new TB_Remit_Statistics_Daily
                            {
                                Date = strRemitMon
                            };
                            barunsonContext.TB_Remit_Statistics_Daily.Add(remitMonItem);
                        }
                        var remitSumQ = from m in barunsonContext.TB_Remit_Statistics_Daily
                                        where m.Date.StartsWith(strMon) && m.Date != strRemitMon
                                        group m by 1 into g
                                        select new
                                        {
                                            Remit_Price = g.Sum(x => x.Remit_Price),
                                            Tax = g.Sum(x => x.Tax),
                                            Remit_Tax = g.Sum(x => x.Remit_Tax),
                                            Calculate_Tax = g.Sum(x => x.Calculate_Tax),
                                            Hits_Tax = g.Sum(x => x.Hits_Tax),
                                            User_Count = g.Sum(x => x.User_Count),
                                            Account_Count = g.Sum(x => x.Account_Count),
                                            Remit_Count = g.Sum(x => x.Remit_Count),
                                        };
                        var remitSumItem = await remitSumQ.FirstOrDefaultAsync(cancellationToken);
                        if (remitSumItem != null)
                        {
                            remitMonItem.Remit_Price = remitSumItem.Remit_Price;
                            remitMonItem.Tax = remitSumItem.Tax;
                            remitMonItem.Remit_Tax = remitSumItem.Remit_Tax;
                            remitMonItem.Calculate_Tax = remitSumItem.Calculate_Tax;
                            remitMonItem.Hits_Tax = remitSumItem.Hits_Tax;
                            remitMonItem.User_Count = remitSumItem.User_Count;
                            remitMonItem.Account_Count = remitSumItem.Account_Count;
                            remitMonItem.Remit_Count = remitSumItem.Remit_Count;

                            //이용자 수 중복제거
                            var remitUQ = from a in barunsonContext.TB_Remit
                                          join b in barunsonContext.TB_Account on a.Account_ID equals b.Account_ID
                                          where a.Result_Code == "RC005" && a.Complete_Date.StartsWith(strMon)
                                          select b.User_ID;
                            remitMonItem.User_Count = await remitUQ.Distinct().CountAsync(cancellationToken);

                            //월 집계
                            var Remit_Statistics_Monthly = await (from m in barunsonContext.TB_Remit_Statistics_Monthly
                                                                  where m.Date == strMon
                                                                  select m).FirstOrDefaultAsync();
                            if (Remit_Statistics_Monthly == null)
                            {
                                Remit_Statistics_Monthly = new TB_Remit_Statistics_Monthly
                                {
                                    Date = strMon
                                };
                                barunsonContext.TB_Remit_Statistics_Monthly.Add(Remit_Statistics_Monthly);
                            }
                            Remit_Statistics_Monthly.Remit_Price = remitMonItem.Remit_Price;
                            Remit_Statistics_Monthly.Tax = remitMonItem.Tax;
                            Remit_Statistics_Monthly.Remit_Tax = remitMonItem.Remit_Tax;
                            Remit_Statistics_Monthly.Calculate_Tax = remitMonItem.Calculate_Tax;
                            Remit_Statistics_Monthly.Hits_Tax = remitMonItem.Hits_Tax;
                            Remit_Statistics_Monthly.User_Count = remitMonItem.User_Count;
                            Remit_Statistics_Monthly.Account_Count = remitMonItem.Account_Count;
                            Remit_Statistics_Monthly.Remit_Count = remitMonItem.Remit_Count;
                        }

                        #endregion

                        await barunsonContext.SaveChangesAsync(cancellationToken);
                        workMon = workMon.AddMonths(1);
                    }
                    while (workMon <= endDate);
                    #endregion

                    #region 연 통계 재 계산
                    var workYear = startDate.Year;
                    do
                    {
                        var strRemitYear = workYear.ToString() + "00";
                        var remitYearQ = from m in barunsonContext.TB_Remit_Statistics_Monthly
                                         where m.Date == strRemitYear
                                         select m;
                        var remitYearItem = await remitYearQ.FirstOrDefaultAsync(cancellationToken);
                        if (remitYearItem == null)
                        {
                            remitYearItem = new TB_Remit_Statistics_Monthly
                            {
                                Date = strRemitYear
                            };
                            barunsonContext.TB_Remit_Statistics_Monthly.Add(remitYearItem);
                        }

                        var remitSumQ = from m in barunsonContext.TB_Remit_Statistics_Monthly
                                        where m.Date.StartsWith(workYear.ToString()) && m.Date != strRemitYear
                                        group m by 1 into g
                                        select new
                                        {
                                            Remit_Price = g.Sum(x => x.Remit_Price),
                                            Tax = g.Sum(x => x.Tax),
                                            Remit_Tax = g.Sum(x => x.Remit_Tax),
                                            Calculate_Tax = g.Sum(x => x.Calculate_Tax),
                                            Hits_Tax = g.Sum(x => x.Hits_Tax),
                                            User_Count = g.Sum(x => x.User_Count),
                                            Account_Count = g.Sum(x => x.Account_Count),
                                            Remit_Count = g.Sum(x => x.Remit_Count),
                                        };
                        var remitSumItem = await remitSumQ.FirstOrDefaultAsync(cancellationToken);
                        if (remitSumItem != null)
                        {
                            remitYearItem.Remit_Price = remitSumItem.Remit_Price;
                            remitYearItem.Tax = remitSumItem.Tax;
                            remitYearItem.Remit_Tax = remitSumItem.Remit_Tax;
                            remitYearItem.Calculate_Tax = remitSumItem.Calculate_Tax;
                            remitYearItem.Hits_Tax = remitSumItem.Hits_Tax;
                            remitYearItem.User_Count = remitSumItem.User_Count;
                            remitYearItem.Account_Count = remitSumItem.Account_Count;
                            remitYearItem.Remit_Count = remitSumItem.Remit_Count;

                            //이용자 수 중복제거
                            var remitUQ = from a in barunsonContext.TB_Remit
                                          join b in barunsonContext.TB_Account on a.Account_ID equals b.Account_ID
                                          where a.Result_Code == "RC005" && a.Complete_Date.StartsWith(workYear.ToString())
                                          select b.User_ID;
                            remitYearItem.User_Count = await remitUQ.Distinct().CountAsync(cancellationToken);

                        }
                        await barunsonContext.SaveChangesAsync(cancellationToken);

                        workYear++;
                    }
                    while (workYear <= endDate.Year);
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
    }
}
