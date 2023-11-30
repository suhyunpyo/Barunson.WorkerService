using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.DBModels.BarShop;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.CommonBatchJob.Jobs
{
    /// <summary>
    /// 바른손 베스트
    /// 시즌2 베스트SP
    /// 매일 오전 0:40
    /// </summary>
    internal class CardBestRanking : BaseJob
    {
        public CardBestRanking(ILogger<Worker> logger, IServiceProvider services, BarShopContext barShopContext,
            TelemetryClient tc, IMailSendService mail, string workerName)
            : base(logger, services, barShopContext, tc, mail, workerName, "CardBestRanking", "40 0 * * *")
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
                    var barshopContext = fncScope.ServiceProvider.GetRequiredService<BarShopContext>();
                    barshopContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));

                    var targetDate = Now.Date;
                    var targetDatestr = targetDate.ToString("yyy-MM-dd");

                    var compaySeqs = new List<int> { 5001, 5002, 5003, 5004, 5005 };
                    var orderTypes = new List<string> { "1", "6", "7" };

                    //우선 현제 날짜에 생성된 데이터 모두 지움.
                    var delQueyr = from m in barshopContext.BestRanking_new where m.Gubun_data == targetDatestr select m;
                    barshopContext.BestRanking_new.RemoveRange(delQueyr);
                    await barshopContext.SaveChangesAsync(cancellationToken);

                    //전일자 랭킹 데이터 읽기 순위변동 계산
                    var oQuery = from m in barshopContext.BestRanking_new
                                 where m.Gubun_data == targetDate.AddDays(-1).ToString("yyy-MM-dd")
                                 select m;
                    var oldItems = await oQuery.ToListAsync();

                    foreach (var compay in compaySeqs)
                    {
                        #region 주간 주문 수량.(30위 안의 데이타는 BestRanking_new 테이블에 저장, S2_salessite.ranking_w 업데이트)
                        {
                            var Query = from m in barshopContext.custom_order
                                        where m.company_seq == compay && m.status_seq == 15 && orderTypes.Contains(m.order_type)
                                        && m.src_send_date >= targetDate.AddDays(-7) && m.src_send_date < targetDate
                                        group m by m.card_seq into g
                                        select new BestRankingModel { CardSeq = g.Key.Value, Count = g.Sum(x => x.order_count) ?? 0 };

                            await InsertBestRankingAsync(barshopContext, compay, "0", targetDatestr, Query, oldItems, cancellationToken);
                        }
                        #endregion

                        #region 월간 주문 수량.(30위 안의 데이타는 BestRanking_new 테이블에 저장, S2_salessite.ranking_m 업데이트)
                        {
                            var Query = from m in barshopContext.custom_order
                                        where m.company_seq == compay && m.status_seq == 15 && orderTypes.Contains(m.order_type)
                                        && m.src_send_date >= targetDate.AddMonths(-1) && m.src_send_date < targetDate
                                        group m by m.card_seq into g
                                        select new BestRankingModel { CardSeq = g.Key.Value, Count = g.Sum(x => x.order_count) ?? 0 };

                            await InsertBestRankingAsync(barshopContext, compay, "1", targetDatestr, Query, oldItems, cancellationToken);
                        }
                        #endregion

                        #region 누적 주문(바른손카드에 해당)(30위 데이타까지만 가져와 BestRanking_new 테이블에 저장)
                        {
                            var Query = from m in barshopContext.custom_order
                                        where m.company_seq == compay && m.status_seq == 15 && orderTypes.Contains(m.order_type)
                                        group m by m.card_seq into g
                                        select new BestRankingModel { CardSeq = g.Key.Value, Count = g.Sum(x => x.order_count) ?? 0 };

                            await InsertBestRankingAsync(barshopContext, compay, "3", targetDatestr, Query, oldItems, cancellationToken);
                        }
                        #endregion

                        #region 월간 샘플 주문(30위 데이타까지만 가져와 BestRanking_new 테이블에 저장)
                        {
                            var Query = from m in barshopContext.CUSTOM_SAMPLE_ORDER
                                        join i in barshopContext.CUSTOM_SAMPLE_ORDER_ITEM on m.sample_order_seq equals i.SAMPLE_ORDER_SEQ
                                        where m.COMPANY_SEQ == compay && m.STATUS_SEQ == 12
                                            && m.DELIVERY_DATE >= targetDate.AddMonths(-1) && m.DELIVERY_DATE < targetDate
                                        group m by i.CARD_SEQ into g
                                        select new BestRankingModel { CardSeq = g.Key, Count = g.Count() };

                            await InsertBestRankingAsync(barshopContext, compay, "2", targetDatestr, Query, oldItems, cancellationToken);
                        }
                        #endregion

                        #region 이용후기(30위 데이타까지만 가져와 BestRanking_new 테이블에 저장)
                        {
                            var Query = from m in barshopContext.S2_UserComment
                                        where m.company_seq == compay
                                        group m by m.card_seq into g
                                        select new BestRankingModel { CardSeq = g.Key, Count = g.Count() };

                            await InsertBestRankingAsync(barshopContext, compay, "4", targetDatestr, Query, oldItems, cancellationToken);
                        }
                        #endregion
                    }

                    //exec sp_S4BestTotalRanking 
                    await barshopContext.Database.ExecuteSqlRawAsync("exec sp_S4BestTotalRanking", cancellationToken);
                }

                await SetNextTimeTaskItemAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}, has error.");
            }

            _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is end.");
        }
        private async Task InsertBestRankingAsync(BarShopContext barshopContext, int compay, string gubun, string targetDatestr, IQueryable<BestRankingModel> query, List<BestRanking_new> oldItems, CancellationToken cancellationToken)
        {
            short rank = 0;
            var Items = await query.OrderByDescending(x => x.Count).Take(30).ToListAsync(cancellationToken);
            foreach (var Item in Items)
            {
                rank++;
                var addItem = new BestRanking_new
                {
                    company_seq = compay,
                    Rank = rank,
                    Card_Seq = Item.CardSeq,
                    Cnt = Item.Count,
                    Gubun = gubun,
                    Gubun_data = targetDatestr
                };

                var oldItem = oldItems.FirstOrDefault(x =>
                    x.company_seq == addItem.company_seq &&
                    x.Gubun == addItem.Gubun &&
                    x.Card_Seq == addItem.Card_Seq);
                if (oldItem != null) //순위변동 Update
                {
                    addItem.RankChangeGubun = (addItem.Rank == oldItem.Rank) ? "BLANK" : (addItem.Rank < oldItem.Rank) ? "UP" : "DOWN";
                    addItem.RankChangeNo = (addItem.Rank == oldItem.Rank) ? "" : Math.Abs(addItem.Rank - oldItem.Rank).ToString("D");
                }
                else
                {
                    addItem.RankChangeGubun = "NEW";
                    addItem.RankChangeNo = "";
                }
                barshopContext.BestRanking_new.Add(addItem);

                if (compay == 5001 && (gubun == "0" || gubun == "1")) //
                {
                    var qrysalesite = from m in barshopContext.S2_CardSalesSite where m.card_seq == Item.CardSeq && m.Company_Seq == compay select m;
                    var sailsiteItem = await qrysalesite.FirstOrDefaultAsync(cancellationToken);
                    if (sailsiteItem != null)
                    {
                        if (gubun == "0")
                            sailsiteItem.Ranking_w = rank;
                        else if (gubun == "1")
                            sailsiteItem.Ranking_m = rank;
                    }
                }
            }
            await barshopContext.SaveChangesAsync(cancellationToken);
        }
        private class BestRankingModel
        {
            public int CardSeq { get; set; }
            public int Count { get; set; }
        }
    }
}
