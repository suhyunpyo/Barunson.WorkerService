using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.DBModels.Barunson;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Barunson.WorkerService.CommonBatchJob.Jobs
{
    /// <summary>
    /// 화환 선물, 매 10 분
    /// </summary>
    internal class FlaSystem : BaseJob
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly Uri apiUri = new Uri("https://barunsonflower.com/api/order_list");

        public FlaSystem(ILogger<Worker> logger, IServiceProvider services, BarShopContext barShopContext,
            TelemetryClient tc, IMailSendService mail, string workerName,
            IHttpClientFactory clientFactory)
            : base(logger, services, barShopContext, tc, mail, workerName, "FlaSystem", "*/10 * * * *")
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

                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<BarunsonContext>();

                    //마지막 업데이트 날짜 읽기
                    var lastUpdateTime = await GetFlaLastUpdateTimeAsync(context);
                    var nowTime = DateTime.Now;
                    var queryFromTime = lastUpdateTime;


                    while (queryFromTime < nowTime)
                    {
                        var jsonResponse = await GetFlaOrderListAsync(queryFromTime);
                        if (jsonResponse != null && jsonResponse.Count > 0)
                        {
                            int maxItmes = 100;
                            List<JToken> jitems;
                            if (jsonResponse.Count > maxItmes)
                                jitems = jsonResponse.Take(maxItmes).ToList();
                            else
                                jitems = jsonResponse.ToList();

                            var orderList = new JArray();
                            foreach (var jitem in jitems)
                                orderList.Add(jitem);

                            await UpdateFlaDataAsync(orderList, context);
                        }
                        queryFromTime = queryFromTime.AddMinutes(10);
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


        /// <summary>
        /// 마지막 수정일 읽기
        /// 시간은 10분 간격으로 초는 0로 초기화
        /// </summary>
        /// <returns></returns>
        private async Task<DateTime> GetFlaLastUpdateTimeAsync(BarunsonContext context)
        {
            DateTime lastUpdateTime = DateTime.Now.AddDays(-1);

            var query = from m in context.TB_Order_PartnerShip where m.P_Id == "flasystem" select m.LastUpdateTime;
            var item = await query.MaxAsync(e => (DateTime?)e);

            if (item.HasValue)
                lastUpdateTime = item.Value;

            var result = new DateTime
            (
                lastUpdateTime.Year,
                lastUpdateTime.Month,
                lastUpdateTime.Day,
                lastUpdateTime.Hour,
                Convert.ToInt32(Math.Floor(lastUpdateTime.Minute / (decimal)10)) * 10,
                0
            );

            return result;
        }
        /// <summary>
        /// API 호출
        /// </summary>
        /// <param name="queryDate"></param>
        /// <returns></returns>
        private async Task<JArray> GetFlaOrderListAsync(DateTime queryDate)
        {
            JArray result = null;
            var client = _clientFactory.CreateClient();
            using (var request = new HttpRequestMessage())
            {
                var url = new UriBuilder(apiUri);
                url.Query = $"order_mdate={queryDate:yyy-MM-dd HH:mm:ss}";

                request.Method = HttpMethod.Get;
                request.RequestUri = url.Uri;

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var contentString = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(contentString))
                {
                    var json = JToken.Parse(contentString);
                    if (json.Type == JTokenType.Array)
                        result = (JArray)json;
                }
                _logger.LogInformation("Query: {0}, Result Count: {1}", queryDate, result?.Count());

            }
            return result;
        }
        /// <summary>
        /// 데이터 추가 또는 업데이트
        /// 마지막 수정일 비교 후 수정사항 만 업데이트
        /// </summary>
        /// <param name="orderList"></param>
        /// <returns></returns>
        private async Task UpdateFlaDataAsync(JArray orderList, BarunsonContext context)
        {
            var PID = "flasystem";
            var orderGroup = from m in orderList
                             where m["order_mdate"].Value<string>() != "0000-00-00 00:00:00"
                             group m by (string)m["order_code"] into g
                             select new { ordercode = g.Key, itmes = g.ToList() };

            foreach (var gItem in orderGroup)
            {
                var lastItem = gItem.itmes.OrderByDescending(x => DateTime.Parse(x["order_mdate"].Value<string>())).FirstOrDefault();
                var lastUpdateTime = DateTime.Parse(lastItem["order_mdate"].Value<string>());

                var query = from m in context.TB_Order_PartnerShip
                            where m.P_Id == PID && m.P_OrderCode == gItem.ordercode
                            select m;
                var item = await query.FirstOrDefaultAsync();

                //최신업데이트 반영되어 있으면 다음 주문으로 넘김
                if (item != null && item.LastUpdateTime >= lastUpdateTime)
                    continue;
                //바른손 주문번호가 없을시 무시
                if (string.IsNullOrEmpty((string)lastItem["cardno"]))
                    continue;

                try
                {
                    if (item == null) //신규
                    {
                        item = new TB_Order_PartnerShip
                        {
                            P_OrderCode = gItem.ordercode,
                            P_Id = PID,
                            Order_ID = int.Parse((string)lastItem["cardno"]),
                        };
                        context.TB_Order_PartnerShip.Add(item);
                    }
                    item.P_OrderDate = DateTime.Parse((string)lastItem["order_wdate"]);
                    item.LastUpdateTime = lastUpdateTime;
                    item.P_OrderState = flaOrderStateCodeTo((string)lastItem["order_state"]);
                    item.P_ProductCode = (string)lastItem["prd_code"];
                    item.P_ProductName = (string)lastItem["prd_name"];
                    item.P_Order_Name = (string)lastItem["order_name"];
                    item.P_Order_Phone = (string)lastItem["order_phone1"];
                    item.Payment_Price = string.IsNullOrEmpty((string)lastItem["pay_price"]) ? 0 : int.Parse((string)lastItem["pay_price"]);
                    item.Payment_Status_Code = flaPayStateCodeTo((string)lastItem["pay_state"]);
                    item.Payment_Method_Code = flaPayMethodCodeTo((string)lastItem["pay_method"]);
                    item.Payment_DateTime = string.IsNullOrEmpty((string)lastItem["pay_date"]) ? null : DateTime.Parse((string)lastItem["pay_date"]);
                    item.Is_Refund = ((string)lastItem["is_refund"] == "Y");
                    item.Refund_DateTime = string.IsNullOrEmpty((string)lastItem["refund_date"]) ? null : DateTime.Parse((string)lastItem["refund_date"]);
                    item.P_ExtendData = lastItem.ToString(Formatting.None);

                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} FlaSystemJob Db Update error. data: {lastItem.ToString(Formatting.None)}");
                }

            }
        }

        /// <summary>
        /// 플라 주문 상태코드 변환
        /// </summary>
        /// <param name="code">상태코드: 10:주문접수,30:배송준비,50:배송완료,60:주문취소</param>
        /// <returns></returns>
        private string flaOrderStateCodeTo(string code)
        {
            var result = code;
            switch (code)
            {
                case "10":
                    result = "주문접수";
                    break;
                case "30":
                    result = "배송준비";
                    break;
                case "50":
                    result = "배송완료";
                    break;
                case "60":
                    result = "주문취소";
                    break;
            }
            return result;

        }
        /// <summary>
        /// 플라 결제 상태 코드 변환
        /// </summary>
        /// <param name="code">결제상태 / 10:결제대기,20:결제완료,90:결제취소</param>
        /// <returns></returns>
        private string flaPayStateCodeTo(string code)
        {
            var result = code;
            switch (code)
            {
                case "10":
                    result = "PSC01";
                    break;
                case "20":
                    result = "PSC02";
                    break;
                case "90":
                    result = "PSC03";
                    break;
            }
            return result;
        }

        /// <summary>
        /// 플라 결제 방식 코드 변환
        /// </summary>
        /// <param name="code">결제방법 / 10:가상계좌,20:신용카드,30:간편결제  추가'50'=>'가상계좌', '99' => '간편결제' </param>
        /// <returns></returns>
        private string flaPayMethodCodeTo(string code)
        {
            var result = code;
            switch (code)
            {
                case "10":
                case "50":
                    result = "PMC02";
                    break;
                case "20":
                    result = "PMC01";
                    break;
                case "30":
                case "99":
                    result = "PMC03";
                    break;

            }
            return result;
        }
    }
}
