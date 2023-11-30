using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.DBModels.BarShop;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Services;
using HtmlAgilityPack;
using Microsoft.ApplicationInsights;
using System.Net;
using System.Text.Json.Nodes;

namespace Barunson.WorkerService.CommonBatchJob.Jobs
{
    /// <summary>
    /// 경쟁사 사이트 확인,  매일 오전 11:50
    /// </summary>
    public class CompetitorSiteCheck : BaseJob
    {
        private readonly IHttpClientFactory _clientFactory;
        private BarShopContext context { get; set; }

        public CompetitorSiteCheck(ILogger<Worker> logger, IServiceProvider services, BarShopContext barShopContext,
            TelemetryClient tc, IMailSendService mail, string workerName,
            IHttpClientFactory clientFactory)
            : base(logger, services, barShopContext, tc, mail, workerName, "CompetitorSiteCheck", "50 11 * * *")
        {
            _clientFactory = clientFactory;
        }
        

        public override async Task Excute(CancellationToken cancellationToken)
        {
            try
            {
                if (!await IsExecute(cancellationToken))
                    return;
                _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is working.");

                
                using (var scope = _serviceProvider.CreateScope())
                {
                    context = scope.ServiceProvider.GetRequiredService<BarShopContext>();

                    await DeleteCard(cancellationToken);
                    await BojagicardCard(cancellationToken);
                    await Card1st(cancellationToken);
                    await CardMarket(cancellationToken);
                    await CardQ(cancellationToken);
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
        /// 사이트의 Set 쿠키 정보 읽기
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<string> RequestCookie(Uri url, CancellationToken cancellationToken)
        {
            string result = "";
            var client = _clientFactory.CreateClient();

            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = url;

                var response = await client.SendAsync(request, cancellationToken);

                response.EnsureSuccessStatusCode();

                var cookies = new CookieContainer();
                foreach (var cookieHeader in response.Headers.GetValues("Set-Cookie"))
                    cookies.SetCookies(url, cookieHeader);

                result = string.Join(";", cookies.GetAllCookies().Select(m => $"{m.Name}={m.Value}"));

            }

            return result;
        }

        /// <summary>
        /// 사이트 페이지 호출
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="headerCookie"></param>
        /// <returns></returns>
        private async Task<string> RequestPage(Uri url, CancellationToken cancellationToken, string headerCookie = null)
        {
            string result = null;
            var client = _clientFactory.CreateClient();

            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = url;
                if (headerCookie != null)
                    request.Headers.Add("Cookie", headerCookie);

                var response = await client.SendAsync(request, cancellationToken);

                response.EnsureSuccessStatusCode();

                result = await response.Content.ReadAsStringAsync();
            }

            return result;
        }

        /// <summary>
        /// 오래된 항목 삭제
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task DeleteCard(CancellationToken cancellationToken)
        {
            try
            {
                DateTime dateTime = DateTime.Now.AddDays(-7);
                var deleteItmes = context.COMPETITOR_CARD_MST.Where(x => x.REG_DATE < dateTime);
                context.COMPETITOR_CARD_MST.RemoveRange(deleteItmes);
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}, has error.");
            }
        }

        /// <summary>
        /// 보자기 카드
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task BojagicardCard(CancellationToken cancellationToken)
        {
            try
            {
                var nowDate = DateTime.Now;
                var siteUrl = new Uri("http://www.bojagicard.com/card/doc/product/product_list.php?view_article=10000&CardType=gb&sid=ingi&special=&page=1&tag=&shape=");
                var cookie = await RequestCookie(siteUrl, cancellationToken);
                cookie = "bojagi_list_page_count=999;" + cookie;

                var responseString = await RequestPage(siteUrl, cancellationToken, cookie);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(responseString);

                var CompetitorCardMstList = new List<COMPETITOR_CARD_MST>();
                foreach (HtmlNode n in htmlDocument.DocumentNode.SelectNodes("//div[contains(@class,'card_block_wrapper')]"))
                {
                    var item = new COMPETITOR_CARD_MST();

                    item.CARD_SEQ = 0;
                    item.CARD_CODE = n.SelectSingleNode(".//img[contains(@class, 'product_list_card_img')]").Attributes["cardCode"].Value;
                    item.CARD_IMAGE = n.SelectSingleNode(".//img[contains(@class, 'product_list_card_img')]").Attributes["src"].Value;
                    item.CARD_NAME = n.SelectSingleNode(".//span[contains(@class, 'sname_head')]").InnerText;
                    item.CARD_NAME = item.CARD_NAME + n.SelectSingleNode(".//span[contains(@class, 'sname_num')]").InnerText;

                    string discountRate = n.SelectSingleNode(".//span[contains(@id, '" + item.CARD_CODE + "_sale')]").InnerText;
                    item.DISCOUNT_RATE = !string.IsNullOrEmpty(discountRate) ? decimal.Parse(discountRate) : 0;

                    string price = n.SelectSingleNode(".//span[contains(@id, '" + item.CARD_CODE + "_price')]").InnerText.Replace(",", "");
                    var cardprice = !string.IsNullOrEmpty(price) ? decimal.Parse(price) : 0;
                    item.CARD_PRICE = (decimal)Math.Round((cardprice / 300) / Math.Round(1 - (item.DISCOUNT_RATE.Value / 100), 3), 1);

                    item.SITE_NAME = "보자기카드";
                    item.REG_DATE = nowDate;

                    CompetitorCardMstList.Add(item);
                }

                context.COMPETITOR_CARD_MST.AddRange(CompetitorCardMstList);
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}-BojagicardCard, has error.");
            }
        }

        /// <summary>
        /// 카드일번가
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task Card1st(CancellationToken cancellationToken)
        {
            try
            {
                var nowDate = DateTime.Now;
                var siteUrl = new Uri("http://www.card1st.co.kr/_common/json/list.category.json.php?table=product&where=dp_pos%253D%27332%27&orderby=dp_sort%2520ASC&rowcnt=10000&paging=false&pagenum=1&qty=300");

                var responseString = await RequestPage(siteUrl, cancellationToken);
                var jObject = JsonNode.Parse(responseString);
                var rows = jObject["rows"].AsArray();

                var CompetitorCardMstList = new List<COMPETITOR_CARD_MST>();
                foreach (var row in rows)
                {
                    var item = new COMPETITOR_CARD_MST();
                    item.CARD_SEQ = int.Parse((string)row["p_idx"]);
                    item.CARD_CODE = (string)row["p_code"];
                    item.CARD_NAME = (string)row["c_nm"];
                    item.CARD_PRICE = decimal.Parse((string)row["p_price"]);
                    item.DISCOUNT_RATE = decimal.Parse((string)row["dc_rate"]);
                    item.CARD_IMAGE = "http://img.card1st.co.kr/product/t235/" + item.CARD_CODE + ".png";

                    item.SITE_NAME = "카드일번가";
                    item.REG_DATE = nowDate;
                    CompetitorCardMstList.Add(item);
                }
                context.COMPETITOR_CARD_MST.AddRange(CompetitorCardMstList);
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}-Card1st, has error.");
            }
        }

        /// <summary>
        /// 카드마켓
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task CardMarket(CancellationToken cancellationToken)
        {
            try
            {
                var nowDate = DateTime.Now;
                var siteUrl = new Uri("http://www.card-market.co.kr/_common/json/list.category.json.php?table=product&where=dp_pos%253D%27332%27&orderby=dp_sort%2520ASC&rowcnt=10000&paging=false&pagenum=1&qty=300");

                var responseString = await RequestPage(siteUrl, cancellationToken);
                var jObject = JsonNode.Parse(responseString);
                var rows = jObject["rows"].AsArray();

                var CompetitorCardMstList = new List<COMPETITOR_CARD_MST>();
                foreach (var row in rows)
                {
                    var item = new COMPETITOR_CARD_MST();
                    item.CARD_SEQ = int.Parse((string)row["p_idx"]);
                    item.CARD_CODE = (string)row["p_code"];
                    item.CARD_NAME = (string)row["p_name"];
                    item.CARD_PRICE = decimal.Parse((string)row["p_price"]);
                    item.DISCOUNT_RATE = decimal.Parse((string)row["dc_rate"]);
                    item.CARD_IMAGE = (string)row["main_img_1x1"];

                    item.SITE_NAME = "카드마켓";
                    item.REG_DATE = nowDate;
                    CompetitorCardMstList.Add(item);
                }
                context.COMPETITOR_CARD_MST.AddRange(CompetitorCardMstList);
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}-CardMarket, has error.");
            }
        }

        /// <summary>
        /// 카드 큐
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task CardQ(CancellationToken cancellationToken)
        {
            try
            {
                var nowDate = DateTime.Now;
                var siteUrl = new Uri("http://www.cardq.co.kr/card/list-type2.asp?cate=1&code=A01");

                var responseString = await RequestPage(siteUrl, cancellationToken);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(responseString);
                string TotCnt = htmlDocument.DocumentNode.SelectSingleNode(".//div[contains(@class, 'pg-heading')]/span").InnerText;
                TotCnt = TotCnt.Replace("Total", "").Replace("개", "");

                int TotalCnt = 0;
                if (int.TryParse(TotCnt, out TotalCnt))
                {
                    double TotalPageCnt = Math.Ceiling(Convert.ToDouble(1.0 * TotalCnt / 12));
                    var CompetitorCardMstList = new List<COMPETITOR_CARD_MST>();
                    for (int i = 1; i <= TotalPageCnt; i++)
                    {
                        Uri targetUri = new Uri("http://www.cardq.co.kr/card/prdList.asp?orderby=NEW&cPage=" + i + "&sel_cnt=300&gCate=A01&gubun=list2");
                        var pagestring = await RequestPage(targetUri, cancellationToken);
                        var s = new HtmlDocument();
                        s.LoadHtml(pagestring);
                        foreach (HtmlNode n in s.DocumentNode.SelectNodes("//li"))
                        {
                            var item = new COMPETITOR_CARD_MST();

                            string cardSeq = n.SelectSingleNode(".//div[contains(@class, 'img')]/a").Attributes["href"].Value;
                            cardSeq = cardSeq.Split('&').Where(x => x.Split('=')[0] == "key").FirstOrDefault().Split('=')[1];
                            item.CARD_SEQ = int.Parse(cardSeq);

                            item.CARD_IMAGE = "http://www.cardq.co.kr" + n.SelectSingleNode(".//a/img").Attributes["src"].Value;
                            item.CARD_NAME = n.SelectSingleNode(".//div[contains(@class, 'info')]").InnerText;
                            item.CARD_CODE = n.SelectSingleNode(".//div[contains(@class, 'prd-over-btns is1')]/button[contains(@btm_gubun, 'sam')]").Attributes["btm_gc"].Value;

                            string price = n.SelectSingleNode(".//div[contains(@class, 'price clearfix')]/div/span[contains(@id, 'gPrice')]").InnerText;
                            price = price.Replace(",", "").Replace("won", "").Replace("ea", "").Replace("원", "").Replace("%", "").Replace(" ", "");

                            string discountRate = n.SelectSingleNode(".//div[contains(@class, 'price clearfix')]/div/span[contains(@id, 'gRate')]").InnerText;
                            discountRate = discountRate.Replace("(", "").Replace(")", "").Replace("&darr;", "").Replace("  ", "").Replace("%", "");
                            decimal dRate = 0;
                            if (decimal.TryParse(discountRate, out dRate))
                                item.DISCOUNT_RATE = dRate;
                            else
                                item.DISCOUNT_RATE = 0;

                            item.CARD_PRICE = (decimal)Math.Round((decimal.Parse(price) / 300) / Math.Round(1 - (dRate / 100), 3), 2);

                            item.SITE_NAME = "카드큐";
                            item.REG_DATE = nowDate;

                            CompetitorCardMstList.Add(item);
                        }
                    }
                    context.COMPETITOR_CARD_MST.AddRange(CompetitorCardMstList);
                    await context.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}-CardQ, has error.");
            }

        }
    }

}
