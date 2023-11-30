using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.DBModels.Barunson;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Models;
using Barunson.WorkerService.Common.Services;
using Barunson.WorkerService.CommonBatchJob.Models;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Barunson.WorkerService.CommonBatchJob.Jobs
{
    internal class TossPaymentCheck : BaseJob
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly List<PgMertInfo> _pgMertInfoList;
        private readonly Uri _url = new Uri("https://api.tosspayments.com/");

        private List<TB_Common_Code> bankList = new List<TB_Common_Code>();
        private List<TB_Common_Code> cardList = new List<TB_Common_Code>();


        public TossPaymentCheck(ILogger logger, IServiceProvider services, BarShopContext barShopContext, TelemetryClient tc, IMailSendService mail, string worker, IHttpClientFactory clientFactory, List<PgMertInfo> mertInfos) 
            : base(logger, services, barShopContext, tc, mail, worker, "TossPaymentCheck", "30 7-8 * * *")
        {
            this._clientFactory = clientFactory;
            //상점키는 모두 사용하는지?
            this._pgMertInfoList = mertInfos;
        }

        public override async Task Excute(CancellationToken cancellationToken)
        {
            DateTime YesterDay = DateTime.Today.AddDays(-1);
            string jsonStr = "";

            try
            {
                if (!await IsExecute(cancellationToken))
                    return;
                _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is working.");

                List<TossPayment> failOrders = new List<TossPayment>();
                List<TossPayment> failSampleOrders = new List<TossPayment>();
                List<TossPayment> failEtcOrders = new List<TossPayment>();
                int failCount = 0;
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<BarShopContext>();
                    var barunsonContext = scope.ServiceProvider.GetRequiredService<BarunsonContext>();

                    bankList = barunsonContext.TB_Common_Code.Where(x => x.Code_Group == "toss_bank").ToList();
                    cardList = barunsonContext.TB_Common_Code.Where(x => x.Code_Group == "toss_card").ToList();

                    foreach (PgMertInfo mert in _pgMertInfoList)
                    {
                        if (string.IsNullOrEmpty(mert.SecretKey))
                            continue;

                        jsonStr = await GetData(mert.SecretKey, $"/v1/transactions?startDate={YesterDay.ToString("yyyy-MM-dd")}T00:00:00&endDate={YesterDay.ToString("yyyy-MM-dd")}T23:59:59");

                        if (string.IsNullOrEmpty(jsonStr))
                            continue;

                        var records = JsonSerializer.Deserialize<List<TossTransaction>>(jsonStr);
                        var cancelIds = records.Where(m => m.status == "CANCELED").Select(m => m.orderId).ToList();
                        var doneList = records.Where(m => m.status == "DONE" && !cancelIds.Contains(m.orderId)).ToList();

                        List<string> orders = new();
                        List<string> sampleOrders = new();
                        List<string> etcOrders = new();

                        foreach (TossTransaction record in doneList)
                        {
                            if (record.orderId.StartsWith("IS"))
                            {
                                // 샘플주문
                                sampleOrders.Add(record.orderId);
                            }
                            else if (record.orderId.StartsWith("ET"))
                            {
                                // 부가상품 주문
                                etcOrders.Add(record.orderId);
                            }
                            else
                            {
                                // 청첩장 주문
                                orders.Add(record.orderId);
                            }
                        }
                        List<string> customOrderIssues = new List<string>();
                        if (orders.Count > 0)
                        {
                            var query = from o in context.custom_order
                                        where (o.settle_status == 0 || o.settle_status == 1) && orders.Contains(o.pg_tid)
                                        select new
                                        {
                                            o.pg_tid
                                        };

                            var tids = await query.ToListAsync();

                            if (tids != null)
                            {
                                foreach (var item in tids)
                                {
                                    TossPayment payment = await GetTossPayment(mert.SecretKey, item.pg_tid);
                                    if (payment == null) 
                                        continue;
                                                
                                    failOrders.Add(payment);
                                    failCount++;
                                }
                            }
                        }

                        if (sampleOrders.Count > 0)
                        {
                            var query = from o in context.CUSTOM_SAMPLE_ORDER
                                        where (o.STATUS_SEQ == 0 || o.STATUS_SEQ == 1) && sampleOrders.Contains(o.PG_TID)

                                        select new
                                        {
                                            o.PG_TID
                                        };

                            var tids = await query.ToListAsync();

                            if (tids != null)
                            {
                                foreach (var item in tids)
                                {
                                    TossPayment payment = await GetTossPayment(mert.SecretKey, item.PG_TID);
                                    if (payment == null)
                                        continue;

                                    failSampleOrders.Add(payment);
                                    failCount++;
                                }
                            }
                        }

                        if (etcOrders.Count > 0)
                        {
                            var query = from o in context.CUSTOM_ETC_ORDER
                                        where (o.status_seq == 0 || o.status_seq == 1) && sampleOrders.Contains(o.pg_tid)

                                        select new
                                        {
                                            o.pg_tid
                                        };

                            var tids = await query.ToListAsync();

                            if (tids != null)
                            {
                                foreach (var item in tids)
                                {
                                    TossPayment payment = await GetTossPayment(mert.SecretKey, item.pg_tid);
                                    if (payment == null)
                                        continue;

                                    failEtcOrders.Add(payment);

                                    failCount++;
                                }
                            }
                        }
                    }
                }

                if(failCount > 0)
                {
                    string mailContent = "";
                    foreach(var item in failOrders)
                    {
                        mailContent += GetMailForm("card", item);
                    }

                    foreach (var item in failSampleOrders)
                    {
                        mailContent += GetMailForm("sample", item);
                    }

                    foreach (var item in failEtcOrders)
                    {
                        mailContent += GetMailForm("etc", item);
                    }

                    await _mail.SendAsync($"[{YesterDay.ToString("yyyy-MM-dd")}]결제 업데이트 실패건 안내({failCount}건)", mailContent);
                }

                await SetNextTimeTaskItemAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}, has error.");
            }
            _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is end.");
        }

        string GetMailForm(string type, TossPayment payment)
        {
            string typeStr = string.Empty;
            string selectSql = string.Empty;
            string updateSql = string.Empty;

            if (type == "card")
            {
                typeStr = "청첩장";
                selectSql = $"select order_seq, settle_price, settle_status, settle_method, settle_date, pg_shopid, pg_tid, dacom_tid, card_installmonth, card_nointyn, pg_receipt_tid, pg_resultinfo, pg_resultinfo2 from custom_order  where order_seq = {payment.orderId.Substring(2)}";

                if (payment.method == "카드")
                {
                    updateSql = "update custom_order set settle_status = 2, settle_method = 2, "
                            + $" settle_price = {payment.totalAmount}, "
                            + $" settle_date = convert(datetime,'{Convert.ToDateTime(payment.approvedAt).ToString("yyyy-MM-dd HH:mm:ss", null)}'), "
                            + $" pg_shopid = '{payment.mId}', "
                            + $" dacom_tid = '{payment.paymentKey}', "
                            + $" card_installmonth = '{payment.card.installmentPlanMonths}', "
                            + $" card_nointyn = '{(payment.card.installmentPlanMonths > 0 ? "1" : "0")}', "
                            + $" pg_resultinfo = '{cardList.Where(x=>x.Code == payment.card.issuerCode).First().Code_Name} {payment.card.approveNo}', "
                            + " pg_resultinfo2 = '', "
                            + $" receiptUrl = '{payment.receipt.url}' "
                            + $" where pg_tid = '{payment.orderId} '";

                }
                else if (payment.method == "계좌이체")
                {
                    updateSql = "update custom_order set settle_status = 2, settle_method = 1, "
                            + $" settle_price = {payment.totalAmount}, "
                            + $" settle_date = convert(datetime,'{Convert.ToDateTime(payment.approvedAt).ToString("yyyy-MM-dd HH:mm:ss", null)}'), "
                            + $" pg_shopid = '{payment.mId}', "
                            + $" dacom_tid = '{payment.paymentKey}', "
                            + " card_installmonth = '', "
                            + " card_nointyn = '0', "
                            + $" pg_resultinfo = '{bankList.Where(x=>x.Code == payment.transfer.bankCode).First().Code_Name}', "
                            + " pg_resultinfo2 = '', "
                            + $" receiptUrl = '{(payment.cashReceipt != null ? payment.cashReceipt.receiptUrl : payment.receipt.url)}'"
                            + $" where pg_tid = '{payment.orderId}'";
                }
                else if (payment.method == "가상계좌")
                {
                    updateSql = "update custom_order set settle_status = 2, settle_method = 3,"
                            + $" settle_price = {payment.totalAmount}, "
                            + $" settle_date = convert(datetime,'{Convert.ToDateTime(payment.approvedAt).ToString("yyyy-MM-dd HH:mm:ss", null)}'), "
                            + $" pg_shopid = '{payment.mId}', "
                            + $" dacom_tid = '{payment.paymentKey}', "
                            + " card_installmonth = '', "
                            + " card_nointyn = '0', "
                            + $" pg_resultinfo = '{bankList.Where(x => x.Code == payment.transfer.bankCode).First().Code_Name} {payment.virtualAccount.accountNumber}', "
                            + $" pg_resultinfo2 = '{payment.virtualAccount.customerName}', "
                            + $" receiptUrl = '{(payment.cashReceipt != null ? payment.cashReceipt.receiptUrl : payment.receipt.url)}'"
                            + $" where pg_tid = '{payment.orderId}'";

                }
                else if (payment.method == "간편결제")
                {
                    updateSql = "update custom_order set settle_status = 2, settle_method = 2, "
                            + $" settle_price = {payment.totalAmount}, "
                            + $" settle_date = convert(datetime,'{Convert.ToDateTime(payment.approvedAt).ToString("yyyy-MM-dd HH:mm:ss", null)}'), "
                            + $" pg_shopid = '{payment.mId}', "
                            + $" dacom_tid = '{payment.paymentKey}', "
                            + " card_installmonth = '', "
                            + " card_nointyn = '0', "
                            + $" pg_resultinfo = '{payment.method} {payment.easyPay.provider}', "
                            + " pg_resultinfo2 = '', "
                            + $" receiptUrl = '{payment.receipt.url}'"
                            + $" where pg_tid = '{payment.orderId}'";
                }
            }
            else if (type == "etc")
            {
                selectSql = $"select order_seq, settle_price, status_seq, SETTLE_DATE , settle_method, pg_shopid, pg_tid, DACOM_TID, card_installmonth, card_nointyn, PG_RESULTINFO, PG_RESULTINFO2, ReceiptUrl from custom_etc_order where order_seq = {payment.orderId.Substring(2)}";
                typeStr = "부가상품";

                if (payment.method == "카드")
                {
                    updateSql = "update custom_etc_order set status_seq = 4, settle_method = 2, "
                            + $" settle_price = {payment.totalAmount}, "
                            + $" settle_date = convert(datetime,'{Convert.ToDateTime(payment.approvedAt).ToString("yyyy-MM-dd HH:mm:ss", null)}'), "
                            + $" pg_shopid = '{payment.mId}', "
                            + $" dacom_tid = '{payment.paymentKey}', "
                            + $" card_installmonth = '{payment.card.installmentPlanMonths}', "
                            + $" card_nointyn = '{(payment.card.installmentPlanMonths > 0 ? "1" : "0")}', "
                            + $" pg_resultinfo = '{cardList.Where(x => x.Code == payment.card.issuerCode).First().Code_Name} {payment.card.approveNo}', "
                            + " pg_resultinfo2 = '', "
                            + $" receiptUrl = '{payment.receipt.url}' "
                            + $" where pg_tid = '{payment.orderId}'";
                }
                else if (payment.method == "계좌이체")
                {
                    updateSql = "update custom_etc_order set status_seq = 4, settle_method = 1, "
                            + $" settle_price = {payment.totalAmount}, "
                            + $" settle_date=convert(datetime,'{Convert.ToDateTime(payment.approvedAt).ToString("yyyy-MM-dd HH:mm:ss", null)}'), "
                            + $" pg_shopid = '{payment.mId}', "
                            + $" dacom_tid = '{payment.paymentKey}', "
                            + " card_installmonth = '', "
                            + " card_nointyn = '0', "
                            + $" pg_resultinfo = '{bankList.Where(x => x.Code == payment.transfer.bankCode).First().Code_Name}', "
                            + " pg_resultinfo2 = '', "
                            + $" receiptUrl = '{(payment.cashReceipt != null ? payment.cashReceipt.receiptUrl : payment.receipt.url)}'"
                            + $" where pg_tid = '{payment.orderId}'";
                }
                else if (payment.method == "가상계좌")
                {
                    updateSql = "update custom_etc_order set status_seq = 4, settle_method = 3, "
                            + $" settle_price = {payment.totalAmount}, "
                            + $" settle_date = convert(datetime,'{Convert.ToDateTime(payment.approvedAt).ToString("yyyy-MM-dd HH:mm:ss", null)}'), "
                            + $" pg_shopid = '{payment.mId}', "
                            + $" dacom_tid = '{payment.paymentKey}', "
                            + " card_installmonth = '', "
                            + " card_nointyn = '0', "
                            + $" pg_resultinfo = '{bankList.Where(x => x.Code == payment.transfer.bankCode).First().Code_Name} {payment.virtualAccount.accountNumber}', "
                            + $" pg_resultinfo2 = '{payment.virtualAccount.customerName}', "
                            + $" receiptUrl = '{(payment.cashReceipt != null ? payment.cashReceipt.receiptUrl : payment.receipt.url)}'"
                            + $" where pg_tid = '{payment.orderId} '";

                }
                else if (payment.method == "간편결제")
                {
                    updateSql = "update custom_etc_order set status_seq = 4, settle_method = 2,"
                            + $" settle_price = {payment.totalAmount}, "
                            + $" settle_date = convert(datetime,'{Convert.ToDateTime(payment.approvedAt).ToString("yyyy-MM-dd HH:mm:ss", null)}'), "
                            + $" pg_shopid = '{payment.mId}', "
                            + $" dacom_tid = '{payment.paymentKey}', "
                            + " card_installmonth = '', "
                            + " card_nointyn = '0', "
                            + $" pg_resultinfo = '{payment.method} {payment.easyPay.provider}', "
                            + " pg_resultinfo2 = '', "
                            + $" receiptUrl = '{payment.receipt.url}'"
                            + $" where pg_tid = '{payment.orderId} '";
                }
            }
            else if (type == "sample")
            {
                typeStr = "샘플";
                selectSql = $"select sample_order_seq, settle_price, status_seq, SETTLE_DATE , settle_method, pg_mertid, pg_tid, DACOM_TID, card_installmonth, card_nointyn, PG_RESULTINFO, PG_RESULTINFO2, ReceiptUrl from custom_sample_order where sample_order_seq = {payment.orderId.Substring(2)}";

                if (payment.method == "카드")
                {
                    updateSql = "update custom_sample_order set status_seq = 4, settle_method = 2, "
                            + $" settle_price = {payment.totalAmount}, "
                            + $" settle_date = convert(datetime,'{Convert.ToDateTime(payment.approvedAt).ToString("yyyy-MM-dd HH:mm:ss", null)}'), "
                            + $" pg_shopid = '{payment.mId}', "
                            + $" dacom_tid = '{payment.paymentKey}', "
                            + $" card_installmonth = '{payment.card.installmentPlanMonths}', "
                            + $" card_nointyn = '{(payment.card.installmentPlanMonths > 0 ? "1" : "0")}', "
                            + $" pg_resultinfo = '{cardList.Where(x => x.Code == payment.card.issuerCode).First().Code_Name} {payment.card.approveNo}', "
                            + " pg_resultinfo2 = '', "
                            + $" receiptUrl = '{payment.receipt.url}' "
                            + $" where pg_tid = '{payment.orderId}'";
                }
                else if (payment.method == "계좌이체")
                {
                    updateSql = "update custom_sample_order set status_seq = 4, settle_method = 1, "
                            + $" settle_price = {payment.totalAmount}, "
                            + $" settle_date = convert(datetime,'{Convert.ToDateTime(payment.approvedAt).ToString("yyyy-MM-dd HH:mm:ss", null)}'), "
                            + $" pg_shopid = '{payment.mId}', "
                            + $" dacom_tid = '{payment.paymentKey}', "
                            + " card_installmonth = '', "
                            + " card_nointyn = '0', "
                            + $" pg_resultinfo = '{bankList.Where(x => x.Code == payment.transfer.bankCode).First().Code_Name}', "
                            + " pg_resultinfo2 = '', "
                            + $" receiptUrl = '{(payment.cashReceipt != null ? payment.cashReceipt.receiptUrl : payment.receipt.url)}'"
                            + $" where pg_tid = '{payment.orderId}'";
                }
                else if (payment.method == "가상계좌")
                {
                    updateSql = "update custom_sample_order set status_seq = 4, settle_method = 3,"
                            + $" settle_price = " + payment.totalAmount + ", "
                            + $" settle_date = convert(datetime,'{Convert.ToDateTime(payment.approvedAt).ToString("yyyy-MM-dd HH:mm:ss", null)}'), "
                            + $" pg_shopid = '" + payment.mId + "', "
                            + $" dacom_tid = '" + payment.paymentKey + "', "
                            + " card_installmonth = '', "
                            + " card_nointyn = '0', "
                            + $" pg_resultinfo = '{bankList.Where(x => x.Code == payment.transfer.bankCode).First().Code_Name} {payment.virtualAccount.accountNumber}', "
                            + $" pg_resultinfo2 = '" + payment.virtualAccount.customerName + "', "
                            + $" receiptUrl = '" + (payment.cashReceipt != null ? payment.cashReceipt.receiptUrl : payment.receipt.url) + "'"
                            + $" where pg_tid = '" + payment.orderId + "'";

                }
                else if (payment.method == "간편결제")
                {
                    updateSql = "update custom_sample_order set status_seq = 4, settle_method = 2,"
                            + $" settle_price = {payment.totalAmount}, "
                            + $" settle_date = convert(datetime,'{Convert.ToDateTime(payment.approvedAt).ToString("yyyy-MM-dd HH:mm:ss", null)}'), "
                            + $" pg_shopid = '{payment.mId}', "
                            + $" dacom_tid = '{payment.paymentKey}', "
                            + " card_installmonth = '', "
                            + " card_nointyn = '0', "
                            + $" pg_resultinfo = '{payment.method} {payment.easyPay.provider}', "
                            + " pg_resultinfo2 = '', "
                            + $" receiptUrl = '{payment.receipt.url}'"
                            + $" where pg_tid = '{payment.orderId}'";
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append($"<table style=\"font-size:9pt;\" cellpadding=\"0\" cellspacing=\"0\">\r\n");
            sb.Append($"<tr><th style=\"width:300px; background:#ddd;\">주문유형</th><td>{typeStr}</td></tr>\r\n");
            sb.Append($"<tr><th style=\"width:300px; background:#ddd;\">주문번호<br />(order_seq)</th><td>{payment.orderId.Substring(2)}</td></tr>\r\n");
            sb.Append($"<tr><th style=\"width:300px; background:#ddd;\">결제주문번호<br />(pg_tid)</th><td>{payment.orderId}</td></tr>\r\n");
            sb.Append($"<tr><th style=\"width:300px; background:#ddd;\">결제금액<br />(settle_price)</th><td class=\"fw-bold\">{payment.totalAmount}</td></tr>\r\n");
            sb.Append($"<tr><th style=\"width:300px; background:#ddd;\">결제일시<br />(settle_date)</th><td>{payment.approvedAt}</td></tr>\r\n");
            sb.Append($"<tr><th style=\"width:300px; background:#ddd;\">상점ID<br />(pg_shopid)</th><td>{payment.mId}</td></tr>\r\n");
            sb.Append($"<tr><th style=\"width:300px; background:#ddd;\">결제키<br />(dacom_tid)</th><td>{payment.paymentKey}</td></tr>\r\n");
            sb.Append($"<tr><th style=\"width:300px; background:#ddd;\">조회</th><td>{selectSql}</td></tr>\r\n");
            sb.Append($"<tr><th style=\"width:300px; background:#ddd;\">수정</th><td>{updateSql}</td></tr>\r\n");
            sb.Append($"<tr><th style=\"width:300px; background:#ddd;\">원문데이터</th><td><pre style=\"font-size:9pt;\">{payment.jsonStrBeauty}</pre></td></tr>\r\n");
            sb.Append($"</table>\r\n<br/><br/>");

            return sb.ToString();
        }

        async Task<TossPayment> GetTossPayment(string clientKey, string tid)
        {
            TossPayment payment = null;
            try
            {
                string _json = await GetData(clientKey, $"/v1/payments/{tid}");
                if (!string.IsNullOrEmpty(_json))
                {
                    payment = JsonSerializer.Deserialize<TossPayment>(_json);
                    payment.jsonStr = _json;

                    JsonDocument parsedJson = JsonDocument.Parse(_json);
                    string prettyJson = JsonSerializer.Serialize(parsedJson.RootElement, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        AllowTrailingCommas = true,
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    });
                    payment.jsonStrBeauty = prettyJson;
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}, has error.");
            }

            return payment;
        }

        async Task<string> GetData(string clientKey, string path)
        {
            var httpClient = _clientFactory.CreateClient();

            string jsonStr = "";
            try
            {
                var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(clientKey + ":"));

                var apiUri = new Uri(_url, path);

                string responseText = string.Empty;

                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Get;
                    request.RequestUri = apiUri;
                    //인증헤더는 아래와 같이...
                    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

                    var response = await httpClient.SendAsync(request);
                    //웹 호출시 예외가 발생하면 에러를 발생 함.
                    response.EnsureSuccessStatusCode();

                    jsonStr = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e) {
                _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}, has error.");
            }

            return jsonStr;
        }
    }
}
