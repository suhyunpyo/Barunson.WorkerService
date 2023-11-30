using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.DBModels.BarShop;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barunson.WorkerService.CommonBatchJob.Jobs
{
    /// <summary>
    /// 셈플 주문 통계 계산, 매일 오전 1:30
    /// </summary>
    internal class SampleOrderStatistics : BaseJob
    {
        public SampleOrderStatistics(ILogger<Worker> logger, IServiceProvider services, BarShopContext barShopContext,
           TelemetryClient tc, IMailSendService mail, string workerName)
           : base(logger, services, barShopContext, tc, mail, workerName, "SampleOrderStatistics", "30 1 * * *")
        { }


        public override async Task Excute(CancellationToken cancellationToken)
        {
            try
            {
                if (!await IsExecute(cancellationToken))
                    return;
                _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is working.");
                
                var endday = DateTime.Today;
                var fromDay = endday.AddDays(-3);

                while (fromDay < endday)
                {
                    var today = fromDay.AddDays(1);
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<BarShopContext>();

                        //샘플주문 사이트
                        await ActualSampleSites(context, fromDay, today);
                        //주문: 사이트, 건수
                        await ActualOrder(context, fromDay, today);
                    }

                    fromDay = today;
                }
                await SetNextTimeTaskItemAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}, has error.");
            }

            _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is end.");
        }
        
        /// <summary>
        /// 샘플 주문 업데이트 대상 레코드
        /// hPhone
        /// </summary>
        /// <param name="context"></param>
        /// <param name="hphone"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        private async Task<List<Custom_Sample_Order_Statistics>> GetStatisticsItems(BarShopContext context, string hphone, string email, DateTime day)
        {
            var fromDay = day.AddYears(-1);
            var result = new  List<Custom_Sample_Order_Statistics>();

            var query = from m in context.CUSTOM_SAMPLE_ORDER
                        where (m.MEMBER_HPHONE == hphone || m.MEMBER_EMAIL == email)
                        && m.REQUEST_DATE > fromDay && m.REQUEST_DATE < day
                        && m.STATUS_SEQ >= 1
                        && (string.IsNullOrEmpty(m.MEMBER_ID) || m.MEMBER_ID != "s4guest")
                        select m.sample_order_seq;

            var seqs = await query.ToListAsync();
            var query2 = from m in context.Custom_Sample_Order_Statistics
                         where seqs.Contains(m.sample_order_seq)
                         select m;

            result.AddRange(await query2.ToListAsync());
            var newSeq = seqs.Where(x => !result.Any(a => a.sample_order_seq == x));
            // 집계데이터 없으면 신규 생성
            foreach (var seq in newSeq)
            {
                var cardOptionQuery = from i in context.CUSTOM_SAMPLE_ORDER_ITEM
                               join c in context.S2_CardOption on i.CARD_SEQ equals c.Card_Seq
                               where i.SAMPLE_ORDER_SEQ == seq
                               select new {c.isLaser, c.isInternalDigital, c.PrintMethod };
                var cardOptions = await cardOptionQuery.ToListAsync();

                var newItem = new Custom_Sample_Order_Statistics
                {
                    sample_order_seq = seq,
                    HasLazer = cardOptions.Any(x => x.isLaser != "0"),
                    HasDigital = cardOptions.Any(x => x.isInternalDigital == "1"),
                    HasPressure = cardOptions.Any(x => x.PrintMethod.Substring(2,1) == "1"),
                    HasRolled = cardOptions.Any(x => x.PrintMethod.Substring(0, 1) != "0"),
                };
                context.Custom_Sample_Order_Statistics.Add(newItem);
                result.Add(newItem);
            }

            return result;
        }

        /// <summary>
        /// 문자 병합, 업데이트시 이전 데이터 삭제 방지
        /// 사이트 구분, 카드 SEQ
        /// </summary>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <returns></returns>
        private string? TextJoin(string? before, string? after)
        {
            var outList = new List<string>();
            if (!string.IsNullOrEmpty(before))
                outList.AddRange(before.Split("|"));

            if (!string.IsNullOrEmpty(after))
            {
                var afterNew = after.Split("|");
                foreach (var a in afterNew)
                {
                    if (!outList.Contains(a))
                        outList.Add(a);
                }
            }
            if (outList.Count > 0)
                return string.Join("|", outList.OrderBy(m => m));
            else
                return null;
        }

        /// <summary>
        /// 샘플주문 사이트
        /// </summary>
        /// <returns></returns>
        private async Task ActualSampleSites(BarShopContext context, DateTime fromDay, DateTime toDay)
        {
            var query = from m in context.CUSTOM_SAMPLE_ORDER
                        where m.DELIVERY_DATE >= fromDay && m.DELIVERY_DATE < toDay
                        && m.STATUS_SEQ >= 1
                        && (string.IsNullOrEmpty(m.MEMBER_ID) || m.MEMBER_ID != "s4guest")
                        && (!string.IsNullOrEmpty(m.MEMBER_HPHONE) && m.MEMBER_HPHONE != "--" && !string.IsNullOrEmpty(m.MEMBER_EMAIL))
                        select new { m.MEMBER_HPHONE, m.MEMBER_EMAIL, m.SALES_GUBUN };

            var items = await query.ToListAsync();

            //회원,비회원: email, hphone 일치
            var GroupHPhone = items.GroupBy(m => new { m.MEMBER_HPHONE, m.MEMBER_EMAIL })
                                .Select(m => new { hphone = m.Key.MEMBER_HPHONE, email = m.Key.MEMBER_EMAIL, Sales = string.Join("|", m.Select(x => x.SALES_GUBUN)) });
            foreach (var GroupItem in GroupHPhone)
            {
                var statistics = await GetStatisticsItems(context, GroupItem.hphone, GroupItem.email, toDay);
                statistics.ForEach(m =>
                {
                    m.ActualSampleSites = TextJoin(m.ActualSampleSites, GroupItem.Sales);
                });
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 주문 사이트
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fromDay"></param>
        /// <param name="toDay"></param>
        /// <returns></returns>
        private async Task ActualOrder(BarShopContext context, DateTime fromDay, DateTime toDay)
        {
            var query = from m in context.custom_order
                        where m.src_send_date >= fromDay && m.src_send_date < toDay
                        && m.status_seq == 15 && m.up_order_seq == null
                        && (string.IsNullOrEmpty(m.member_id) || m.member_id != "s4guest")
                        && (!string.IsNullOrEmpty(m.order_hphone) && m.order_hphone != "--" && !string.IsNullOrEmpty(m.order_email))
                        select new {m.order_seq, m.order_hphone, m.order_email, m.sales_Gubun, m.card_seq, m.order_date, m.settle_date, m.src_send_date };
            var items = await query.ToListAsync();

            //사이트 구분 회원,비회원: hphone 일치
            var GroupHPhone = items.GroupBy(m => new { m.order_hphone, m.order_email })
                                .Select(m => 
                                    new 
                                    { 
                                        hphone = m.Key.order_hphone, 
                                        email = m.Key.order_email,
                                        OrderSeqs = string.Join("|", m.Select(x => x.order_seq.ToString())),
                                        Sales = string.Join("|", m.Select(x => x.sales_Gubun)), 
                                        CardSeqs = string.Join("|", m.Select(x => x.card_seq?.ToString())),
                                        OrderDate = m.Max(x => x.order_date),
                                        SettleDate = m.Max(x => x.settle_date),
                                        SrcSendDate = m.Max(x => x.src_send_date)
                                    });
            foreach (var GroupItem in GroupHPhone)
            {
                var statistics = await GetStatisticsItems(context, GroupItem.hphone, GroupItem.email, toDay);
                foreach (var m in statistics)
                {
                    m.ActualOrderSites = TextJoin(m.ActualOrderSites, GroupItem.Sales);
                    m.ActualOrderCardSeqs = TextJoin(m.ActualOrderCardSeqs, GroupItem.CardSeqs);

                    var orSeqs = TextJoin(m.ActualOrderSeqs, GroupItem.OrderSeqs);
                    m.ActualOrderSeqs = orSeqs;
                    m.ActualOrderCount = orSeqs.Split("|").Length;

                    m.LatestOrderDate = (m.LatestOrderDate == null || m.LatestOrderDate.Value < GroupItem.OrderDate) ? GroupItem.OrderDate : m.LatestOrderDate;
                    m.LatestSettleDate = (m.LatestSettleDate == null || m.LatestSettleDate.Value < GroupItem.SettleDate) ? GroupItem.SettleDate : m.LatestSettleDate; 
                    m.LatestSrcSendDate = (m.LatestSrcSendDate == null || m.LatestSrcSendDate.Value < GroupItem.SrcSendDate) ? GroupItem.SrcSendDate : m.LatestSrcSendDate; 
                }
                
                await context.SaveChangesAsync();
            }
        }
    }
}
