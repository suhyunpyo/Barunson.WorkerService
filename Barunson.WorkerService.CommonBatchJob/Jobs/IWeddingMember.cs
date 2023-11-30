using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.DBModels.BarShop;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Services;
using HtmlAgilityPack;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Web;

namespace Barunson.WorkerService.CommonBatchJob.Jobs
{
    /// <summary>
    /// IWedding 멤버십 생성 및 전송, 매일 6:10 
    /// </summary>
    internal class IWeddingMember: BaseJob
    {
        private readonly IHttpClientFactory _clientFactory;

        private readonly Uri apiUri = new Uri("https://iwedding.co.kr/event/complete_paper_action.php");

        public IWeddingMember(ILogger<Worker> logger, IServiceProvider services, BarShopContext barShopContext,
            TelemetryClient tc, IMailSendService mail, string workerName,
           IHttpClientFactory clientFactory)
           : base(logger, services, barShopContext, tc, mail, workerName, "IWeddingMember", "10 6 * * *")
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
                var Now = DateTime.Now;
                var sDate = DateTime.Today.AddDays(-60);
                var eDate = DateTime.Today.AddDays(1);
                var count = 0;

                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<BarShopContext>();
                    var query = from a in context.custom_order
                                join b in context.S2_CardView on a.card_seq equals b.card_seq into g
                                from b in g.DefaultIfEmpty()
                                where a.status_seq == 15
                                && a.src_send_date >= sDate && a.src_send_date < eDate
                                && (a.company_seq == 5115 || a.company_seq == 5568)
                                && !(context.iwedding_Sending.Select(b => b.order_seq).Contains(a.order_seq))
                                select new
                                {
                                    a.order_seq,
                                    a.order_type,
                                    a.sales_Gubun,
                                    a.order_name,
                                    a.order_email,
                                    a.order_phone,
                                    a.order_hphone,
                                    a.src_send_date,
                                    a.last_total_price,
                                    a.settle_price,
                                    a.order_count,
                                    a.card_seq,
                                    b.card_code
                                };
                    var items = await query.ToListAsync(cancellationToken);

                    foreach (var item in items)
                    {
                        var formData = new Dictionary<string, string>();

                        formData.Add("type", item.sales_Gubun == "B" ? "7" : "9"); //<!-- 7:비핸즈카드 제휴, 9:프리미어비핸즈 제휴 (고정값입니다.) -->
                        formData.Add("user_name", item.order_name);         //<!-- 고객님 이름 -->
                        formData.Add("user_email", item.order_email);       //<!-- 고객님 이메일 주소 -->
                        formData.Add("user_hp1", item.order_hphone);        //<!-- 고객님 휴대폰이나 전화번호1 (형식, 000-0000-0000) -->
                        formData.Add("user_hp2", item.order_phone);         //<!-- 고객님 휴대폰이나 전화번호2 (형식, 000-0000-0000) -->
                        formData.Add("goods_money1", item.last_total_price?.ToString("d")); //<!-- 판매가 (반드시 숫자만 입력 콤바 안됨) -->

                        var price = item.last_total_price - (item.last_total_price * 0.17);
                        formData.Add("goods_money2", price?.ToString("#")); //<!-- 납품가 (반드시 숫자만 입력 콤바 안됨) -->
                        formData.Add("ex_day", item.src_send_date?.ToString("yyyy-MM-dd")); //<!-- 출고일(배송일자를 말합니다.배송완료 시점에 찍히게 되면 됩니다. 형식 0000-00-00) -->
                        formData.Add("r_url", "http://localhost/sending");
                        formData.Add("r_param", $"order_seq={item.order_seq}");

                        //<!-- 상품내역 -->
                        if (string.IsNullOrEmpty(item.card_code))
                            formData.Add("goods_name", $"{GetNameOfOrderType(item.order_type)}_{item.order_count}매");
                        else
                            formData.Add("goods_name", $"{GetNameOfOrderType(item.order_type)}_{item.card_code}_{item.order_count}매");

                        //전송
                        var response = await SendDataAsync(formData, cancellationToken);
                        if (response)
                        {
                            context.iwedding_Sending.Add(new iwedding_Sending
                            {
                                order_seq = item.order_seq,
                                reg_date = DateTime.Now
                            });

                            count++;
                        }
                    }
                    await context.SaveChangesAsync(cancellationToken);

                }

                await SetNextTimeTaskItemAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}, has error.");
            }
            finally
            {
                await _telemetryClient.FlushAsync(cancellationToken);
            }

            _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is end.");
        }

        private async Task<bool> SendDataAsync(Dictionary<string, string> postItem, CancellationToken cancellationToken)
        {
            bool result = false;
            var client = _clientFactory.CreateClient();

            try
            {
                #region 인코딩 참조 코드
                /* 참조 코드
                1. 인코딩 방법
                var httpClient = new HttpClient(); //static readonly in real code
                var iso = Encoding.GetEncoding("ISO-8859-9");

                var content = new StringContent("id_6=" +
                    HttpUtility.UrlEncode("some text with Turkish characters öçşığüÖÇŞİĞÜ", iso), iso,
                    "application/x-www-form-urlencoded");
                var response = httpClient.PostAsync(url, content).Result;//Using Result because I don't have a UI thread or the context is not ASP.NET
                var responseInString = response.Content.ReadAsStringAsync().Result;


                2. 폼데이터 생성
                var contentList = new List<string>();
                contentList.Add($"data={Uri.EscapeDataString("P1;P2")}");
                contentList.Add($"format={Uri.EscapeDataString("json")}");
                request.Content = new StringContent(string.Join("&", contentList));
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                var response = await httpClient.SendAsync(request);

                */
                #endregion

                Encoding encode = System.Text.Encoding.GetEncoding("euc-kr");
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Post;
                    request.RequestUri = apiUri;

                    //기본 폼방식 주석 처리, 한글 인코딩 문제
                    //request.Content = new FormUrlEncodedContent(postItem);

                    var contentList = new List<string>();
                    foreach (var frm in postItem)
                    {
                        contentList.Add($"{frm.Key}={HttpUtility.UrlEncode(frm.Value, encode)}");
                    }
                    request.Content = new StringContent(string.Join("&", contentList), encode, "application/x-www-form-urlencoded");

                    var response = await client.SendAsync(request, cancellationToken);
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    var html = new HtmlDocument();
                    html.LoadHtml(content);

                    var ok = html.DocumentNode.SelectSingleNode("//input[@name='result_ok']")?.Attributes["value"].Value;
                    result = ok == "1";
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName} api call error.");
            }
            return result;
        }
        private string GetNameOfOrderType(string orderType)
        {
            var result = "";
            switch (orderType)
            {
                case "1":
                    result = "청첩장";
                    break;
                case "2":
                    result = "답례장";
                    break;
                case "3":
                    result = "초대장";
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
