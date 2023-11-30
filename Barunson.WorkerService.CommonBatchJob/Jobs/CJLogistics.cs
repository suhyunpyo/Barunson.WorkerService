using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.DBModels.BarShop;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Services;
using Barunson.WorkerService.CommonBatchJob.Config;
using Barunson.WorkerService.CommonBatchJob.Models;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace Barunson.WorkerService.CommonBatchJob.Jobs
{
    /// <summary>
    /// CJ 배송정보 업데이트 3시간(30분) 간격
    /// </summary>
    internal class CJLogistics : BaseJob
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly Uri apiUri;
        private readonly string Cust_id; //거래처번호 
        private readonly string BIZ_Reg_Num; //사업자번호 


        public CJLogistics(ILogger<Worker> logger, IServiceProvider services, BarShopContext barShopContext,
            TelemetryClient tc, IMailSendService mail, string workerName,
            IHttpClientFactory clientFactory, CJLogisticsAPIConfig cjConfig)
            : base(logger, services, barShopContext, tc, mail, workerName, "CJLogistics", "30 1/3 * * *")
        {
            _clientFactory = clientFactory;
            apiUri = cjConfig.ApiUrl;
            Cust_id = cjConfig.CustId;
            BIZ_Reg_Num = cjConfig.BizRegNum;
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
                    var context = scope.ServiceProvider.GetRequiredService<BarShopContext>();

                    var startDate = DateTime.Now.AddDays(-3).Date;
                    var endDate = DateTime.Now.Date.AddDays(1);

                    var query = from a in context.VW_DELIVERY_MST
                                join b in context.COMPANY on a.COMPANY_SEQ equals b.COMPANY_SEQ
                                join c in context.CJ_DELCODE on a.DELIVERY_CODE equals c.CODE
                                where a.SEND_DATE >= startDate && a.SEND_DATE < endDate
                                && !string.IsNullOrEmpty(a.DELIVERY_CODE)
                                && a.ISHJ == "0"
                                && c.API_YN == "Y"
                                select new DelivertyData
                                {
                                    ORDER_SEQ = a.ORDER_SEQ,
                                    ORDER_TABLE_NAME = a.ORDER_TABLE_NAME,
                                    DELIVERY_CODE = a.DELIVERY_CODE,
                                    DELIVERY_SEQ = a.DELIVERY_SEQ,
                                    DELIVERY_MSG = a.DELIVERY_MSG,
                                    RECV_NAME = a.RECV_NAME,
                                    RECV_ZIP = string.IsNullOrEmpty(a.RECV_ZIP) ? "111111" : a.RECV_ZIP,
                                    RECV_ADDR = a.RECV_ADDR,
                                    RECV_ADDR_DETAIL = string.IsNullOrWhiteSpace(a.RECV_ADDR_DETAIL) ? "." : a.RECV_ADDR_DETAIL,
                                    RECV_PHONE = a.RECV_PHONE,
                                    RECV_HPHONE = a.RECV_HPHONE,
                                    SEND_DATE = a.SEND_DATE,
                                    ERP_PartCode = b.ERP_PartCode,
                                    SALES_GUBUN = b.SALES_GUBUN
                                };
                    var items = await query.ToListAsync(cancellationToken);

                    if (items.Count > 0)
                    {
                        //토큰 확인 또는 생성
                        var totken = await GetOndDayTokenAsync(context, cancellationToken);
                        var postURl = new Uri(apiUri, "RegBook");
                        _logger.LogInformation("Call: {0}, token: {1}", postURl, totken);

                        foreach (var item in items)
                        {
                            //API 호출 모델 생성
                            var regBookModel = GetCJRegBookModel(item, totken);

                            //API Call
                            var cJResponse = await CalCJApiAsync<CJModelRoot<CJModelRegBook>, CJResponse<CJResonseEmpty>>(postURl, regBookModel, totken, cancellationToken);
                            if (cJResponse.RESULT_CD == "S")
                            {
                                await UpDateDeliveryInfoAsync(item.ORDER_TABLE_NAME, item.ORDER_SEQ, item.DELIVERY_CODE, context, cancellationToken);
                            }
                            else
                            {
                                _telemetryClient.TrackTrace($"CJ API Failed: {cJResponse.RESULT_DETAIL}");
                                var log = new CJ_API_LOG
                                {
                                    ORDER_SEQ = item.ORDER_SEQ.ToString(),
                                    KIND = "주문연동",
                                    RESULT_CODE = cJResponse.RESULT_CD,
                                    RESULT_MSG = (cJResponse.RESULT_DETAIL.Length > 1000) ? cJResponse.RESULT_DETAIL.Substring(0, 1000) : cJResponse.RESULT_DETAIL,
                                    REG_DATE = DateTime.Now
                                };
                                context.CJ_API_LOG.Add(log);
                                await context.SaveChangesAsync(cancellationToken);
                            }
                        }
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


        private async Task UpDateDeliveryInfoAsync(string orderTableName, int orderSeq, string deliveryCode, BarShopContext context, CancellationToken cancellationToken)
        {
            if (orderTableName == "CUSTOM_ORDER")
            {
                var item = await (from m in context.DELIVERY_INFO_DELCODE
                                  where m.order_seq == orderSeq && m.delivery_code_num == deliveryCode
                                  select m).FirstOrDefaultAsync();
                if (item != null)
                {
                    item.isHJ = "1";
                    await context.SaveChangesAsync(cancellationToken);
                }
            }
            else if (orderTableName == "CUSTOM_SAMPLE_ORDER")
            {
                var item = await (from m in context.CUSTOM_SAMPLE_ORDER
                                  where m.sample_order_seq == orderSeq && m.DELIVERY_CODE_NUM == deliveryCode
                                  select m).FirstOrDefaultAsync();
                if (item != null)
                {
                    item.isHJ = "1";
                    await context.SaveChangesAsync(cancellationToken);
                }
            }
            else if (orderTableName == "CUSTOM_ETC_ORDER")
            {
                var item = await (from m in context.CUSTOM_ETC_ORDER
                                  where m.order_seq == orderSeq && m.delivery_code == deliveryCode
                                  select m).FirstOrDefaultAsync();
                if (item != null)
                {
                    item.isHJ = "1";
                    await context.SaveChangesAsync(cancellationToken);
                }
            }

        }

        private CJModelRoot<CJModelRegBook> GetCJRegBookModel(DelivertyData item, string totken)
        {
            var result = new CJModelRoot<CJModelRegBook>();

            var rcptYmd = item.SEND_DATE?.ToString("yyyyMMdd");
            var rcvrTels = GetSpliteTelNo(item.RECV_PHONE);
            var rcvrCellTels = GetSpliteTelNo(item.RECV_HPHONE);
            var busoName = GetBusoName(item.ERP_PartCode, item.SALES_GUBUN);
            var prodName = GetProdName(item.ORDER_TABLE_NAME);

            var data = new CJModelRegBook
            {
                CUST_ID = Cust_id,
                TOKEN_NUM = totken,
                RCPT_YMD = rcptYmd,
                CUST_USE_NO = item.ORDER_SEQ.ToString(),
                RCPT_DV = "01",             //01 : 일반, 02 : 반품
                WORK_DV_CD = "01",          //01 : 일반, 02 : 교환, 03 : A/S
                REQ_DV_CD = "01",           //01 : 요청, 02 : 취소
                MPCK_KEY = $"{rcptYmd}_{Cust_id}_{item.DELIVERY_CODE}_1_{item.DELIVERY_SEQ}",  //자체출력사 : YYYYMMDD_고객ID_운송장번호
                CAL_DV_CD = "01",           //01 : 계약 운임, 02 : 자료 운임
                FRT_DV_CD = "03",           //01 : 선불, 02 : 착불, 03 : 신용
                CNTR_ITEM_CD = "01",        //01 : 일반품목
                BOX_TYPE_CD = "01",
                BOX_QTY = "1",
                FRT = "0",
                CUST_MGMT_DLCM_CD = Cust_id,
                SENDR_NM = "바른컴퍼니(파주)",
                SENDR_TEL_NO1 = "02",
                SENDR_TEL_NO2 = "1644",
                SENDR_TEL_NO3 = "0708",
                SENDR_CELL_NO1 = "",
                SENDR_CELL_NO2 = "",
                SENDR_CELL_NO3 = "",
                SENDR_SAFE_NO1 = "",
                SENDR_SAFE_NO2 = "",
                SENDR_SAFE_NO3 = "",
                SENDR_ZIP_NO = "413120",
                SENDR_ADDR = "경기도 파주시 회동길",
                SENDR_DETAIL_ADDR = "219 (주)바른컴퍼니",
                RCVR_NM = item.RECV_NAME,
                RCVR_TEL_NO1 = rcvrTels.Item1,
                RCVR_TEL_NO2 = rcvrTels.Item2,
                RCVR_TEL_NO3 = rcvrTels.Item3,
                RCVR_CELL_NO1 = rcvrCellTels.Item1,
                RCVR_CELL_NO2 = rcvrCellTels.Item2,
                RCVR_CELL_NO3 = rcvrCellTels.Item3,
                RCVR_SAFE_NO1 = "",
                RCVR_SAFE_NO2 = "",
                RCVR_SAFE_NO3 = "",
                RCVR_ZIP_NO = item.RECV_ZIP,
                RCVR_ADDR = item.RECV_ADDR,
                RCVR_DETAIL_ADDR = item.RECV_ADDR_DETAIL,
                ORDRR_NM = item.RECV_NAME,
                ORDRR_TEL_NO1 = rcvrTels.Item1,
                ORDRR_TEL_NO2 = rcvrTels.Item2,
                ORDRR_TEL_NO3 = rcvrTels.Item3,
                ORDRR_CELL_NO1 = rcvrCellTels.Item1,
                ORDRR_CELL_NO2 = rcvrCellTels.Item2,
                ORDRR_CELL_NO3 = rcvrCellTels.Item3,
                ORDRR_SAFE_NO1 = "",
                ORDRR_SAFE_NO2 = "",
                ORDRR_SAFE_NO3 = "",
                ORDRR_ZIP_NO = item.RECV_ZIP,
                ORDRR_ADDR = item.RECV_ADDR,
                ORDRR_DETAIL_ADDR = item.RECV_ADDR_DETAIL,
                INVC_NO = item.DELIVERY_CODE,
                ORI_INVC_NO = "",
                ORI_ORD_NO = "",
                COLCT_EXPCT_YMD = "",
                COLCT_EXPCT_HOUR = "",
                SHIP_EXPCT_YMD = "",
                SHIP_EXPCT_HOUR = "",
                PRT_ST = "02",       //01 : 미출력, 02 : 선출력, 03 : 선발번
                ARTICLE_AMT = "",
                REMARK_1 = item.DELIVERY_MSG,
                REMARK_2 = "",
                REMARK_3 = "",
                COD_YN = "",
                ETC_1 = busoName,
                ETC_2 = "",
                ETC_3 = "",
                ETC_4 = "",
                ETC_5 = "",
                DLV_DV = "01",
                RCPT_SERIAL = "",
                ARRAY = new List<CJModelMPCK>()
            };

            data.ARRAY.Add(new CJModelMPCK
            {
                MPCK_SEQ = "1",
                GDS_CD = "",
                GDS_NM = prodName,
                GDS_QTY = "",
                UNIT_CD = "",
                UNIT_NM = "",
                GDS_AMT = ""
            });
            result.DATA = data;

            return result;
        }
        private (string, string, string) GetSpliteTelNo(string telno)
        {
            var s = telno.Split("-");

            if (s.Length >= 3)
            {
                return (s[0], s[1], s[2]);
            }
            else
            {
                return ("", "", "");
            }
        }
        private string GetProdName(string tbName)
        {
            if (tbName.ToUpper() == "CUSTOM_ORDER")
                return "청첩장";
            else if (tbName.ToUpper() == "CUSTOM_SAMPLE_ORDER")
                return "샘플";
            else if (tbName.ToUpper() == "CUSTOM_ETC_ORDER")
                return "부가 상품";
            else
                return "기타 상품";

        }
        private string GetBusoName(string partcode, string saleGubun)
        {

            var r = "";
            if (string.IsNullOrEmpty(partcode))
            {
                if (saleGubun == "P")
                    r = "직매장";
                else if (saleGubun == "Q")
                    r = "직매장(대리점영업)";
            }
            else
            {
                switch (partcode)
                {
                    case "110":
                        r = "바른손카드";
                        break;
                    case "230":
                        r = "비핸즈카드";
                        break;
                    case "130":
                        r = "프리미어페이퍼";
                        break;
                    case "140":
                        r = "더카드";
                        break;
                    case "340":
                        r = "직매장";
                        break;
                    case "365":
                    case "366":
                        r = "웨딩제휴";
                        break;
                    case "390":
                        r = "제휴영업";
                        break;
                    case "410":
                        r = "해외영업";
                        break;
                    case "340-1":
                        r = "직매장";
                        break;
                    case "240":
                        r = "디얼디어";
                        break;
                }
            }
            return r;
        }
        /// <summary>
        /// Ond Day token 발급 및 저장
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<string> GetOndDayTokenAsync(BarShopContext context, CancellationToken cancellationToken)
        {
            string token = null;

            var exprTime = DateTime.Now.AddMinutes(30).ToString("yyyyMMddHHmmss");
            var query = from a in context.CJ_ONEDAYTOKEN
                        where a.TOKEN_EXPRTN_DTM.CompareTo(exprTime) > 0
                        orderby a.TOKEN_EXPRTN_DTM descending
                        select a;
            var item = await query.FirstOrDefaultAsync(cancellationToken);
            if (item != null)
            {
                token = item.TOKEN_NUM;
            }
            else
            {
                var requestBody = new CJModelRoot<CJModelToken>();
                requestBody.DATA = new CJModelToken
                {
                    CUST_ID = Cust_id,
                    BIZ_REG_NUM = BIZ_Reg_Num
                };

                var tokenData = await CalCJApiAsync<CJModelRoot<CJModelToken>, CJResponse<CJResponseToken>>(new Uri(apiUri, "ReqOneDayToken"), requestBody, null, cancellationToken);
                if (tokenData.RESULT_CD == "S")
                {
                    token = tokenData.DATA.TOKEN_NUM;

                    var newtoken = new CJ_ONEDAYTOKEN
                    {
                        TOKEN_NUM = token,
                        TOKEN_EXPRTN_DTM = tokenData.DATA.TOKEN_EXPRTN_DTM,
                        REG_DATE = DateTime.Now
                    };
                    context.CJ_ONEDAYTOKEN.Add(newtoken);
                    await context.SaveChangesAsync();
                }
            }
            return token;
        }

        /// <summary>
        /// CJ API CALL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="apiUri"></param>
        /// <param name="postBody"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task<U> CalCJApiAsync<T, U>(Uri apiUri, T postBody, string token, CancellationToken cancellationToken)
        {
            U data = default(U);
            var client = _clientFactory.CreateClient();

            try
            {
                var bodystr = JsonSerializer.Serialize(postBody);
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Post;
                    request.RequestUri = apiUri;

                    if (!string.IsNullOrEmpty(token))
                        request.Headers.Add("CJ-Gateway-APIKey", token);

                    request.Content = new StringContent(bodystr, Encoding.UTF8, "application/json");

                    var response = await client.SendAsync(request, cancellationToken);
                    response.EnsureSuccessStatusCode();

                    var restr = await response.Content.ReadAsStringAsync();
                    data = JsonSerializer.Deserialize<U>(restr);

                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{time:yyyy-MM-dd HH:mm:ss} {WorkerName} has error.", DateTime.Now, WorkerName);
                _telemetryClient.TrackException(e);
            }

            return data;
        }
    }
}
