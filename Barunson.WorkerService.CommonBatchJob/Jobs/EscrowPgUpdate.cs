using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Models;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Barunson.WorkerService.CommonBatchJob.Jobs
{
    /// <summary>
    /// Escrow 정보 업데이트 매일 23: 30
    /// </summary>
    internal class EscrowPgUpdate : BaseJob
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly Uri apiUri = new Uri("https://pgweb.dacom.net/pg/wmp/mertadmin/jsp/escrow/rcvdlvinfo.jsp");
        private readonly List<PgMertInfo> _pgInfos;


        public EscrowPgUpdate(ILogger<Worker> logger, IServiceProvider services, BarShopContext barShopContext,
            TelemetryClient tc, IMailSendService mail, string workerName,
            IHttpClientFactory clientFactory, List<PgMertInfo> pgInfos)
            : base(logger, services, barShopContext, tc, mail, workerName, "EscrowPgUpdate", "30 23 * * *")
        {
            _clientFactory = clientFactory;
            _pgInfos = pgInfos;
        }
        public override async Task Excute(CancellationToken cancellationToken)
        {
            try
            {
                if (!await IsExecute(cancellationToken))
                    return;
                _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is working.");
                var Now = DateTime.Now;


                var sDate = DateTime.Today;
                var eDate = sDate.AddDays(1);

                var postItems = new List<Dictionary<string, string>>();

                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<BarShopContext>();

                    #region SQL Text
                    var queryText = @"SELECT	A.PG_SHOPID,		  		
		CASE WHEN A.SALES_GUBUN = 'ST' THEN ISNULL(( SELECT TOP 1 PG_TID FROM CUSTOM_ORDER_GROUP WHERE ORDER_G_SEQ = A.ORDER_G_SEQ ), '') ELSE A.PG_TID END AS PG_TID,  
		C.CARD_CODE, A.SETTLE_DATE, CONVERT(char(1), B.DELIVERY_METHOD) as DELIVERY_METHOD, B.[NAME],  
		CASE B.DELIVERY_METHOD  
			WHEN '1' THEN A.SRC_SEND_DATE  
			WHEN '2' THEN DATEADD(HOUR,3,SRC_SEND_DATE)  
			WHEN '3' THEN A.SRC_SEND_DATE  
			ELSE  A.SRC_SEND_DATE 
		END AS SEND_DATE,  
		B.DELIVERY_CODE_NUM, B.DELIVERY_COM  
FROM	CUSTOM_ORDER A  
	INNER JOIN DELIVERY_INFO B ON A.ORDER_SEQ = B.ORDER_SEQ AND B.DELIVERY_SEQ=1  
	INNER JOIN S2_CARDVIEW C   ON A.CARD_SEQ = C.CARD_SEQ  
WHERE	A.STATUS_SEQ=15  
AND		A.ISASCROW='1'  
AND		A.SRC_SEND_DATE >= @SDATE  
AND		A.SRC_SEND_DATE < @EDATE
UNION ALL
SELECT	A.PG_MERTID AS PG_SHOPID,  
		A.PG_TID, 
		(SELECT CARD_CODE FROM S2_CARD WHERE CARD_SEQ = (SELECT TOP 1 CARD_SEQ FROM CUSTOM_SAMPLE_ORDER_ITEM WHERE SAMPLE_ORDER_SEQ = A.SAMPLE_ORDER_SEQ)) AS CARD_CODE,  
		A.SETTLE_DATE AS SETTLE_DATE , A.DELIVERY_METHOD AS DELIVERY_METHOD , A.MEMBER_NAME AS [NAME],  
		DELIVERY_DATE AS SEND_DATE ,  
		CASE WHEN MULTI_PACK_SEQ IS NULL THEN DELIVERY_CODE_NUM   
			ELSE (SELECT DELIVERY_CODE_NUM FROM CUSTOM_SAMPLE_ORDER WHERE MULTI_PACK_SEQ = A.MULTI_PACK_SEQ AND MULTI_PACK_SUB_SEQ = 1 AND MULTI_PACK_REG_DATE = A.MULTI_PACK_REG_DATE )   
		END  AS DELIVERY_CODE_NUM ,   
		A.DELIVERY_COM AS DELIVERY_COM   
FROM	CUSTOM_SAMPLE_ORDER A  
WHERE	A.STATUS_SEQ=12  
AND		A.ISASCROW='1'  
AND		A.DELIVERY_DATE >= @SDATE  
AND		A.DELIVERY_DATE < @EDATE
UNION ALL
SELECT	A.PG_SHOPID,A.PG_TID,
		CARD_CODE = (SELECT CARD_CODE FROM S2_CARD WHERE CARD_SEQ = (SELECT TOP 1 CARD_SEQ FROM CUSTOM_ETC_ORDER_ITEM WHERE order_seq = A.order_seq)),
		A.SETTLE_DATE,A.DELIVERY_METHOD,A.RECV_NAME as [NAME],
		CASE A.DELIVERY_METHOD WHEN '1' THEN A.DELIVERY_DATE WHEN '2' THEN DATEADD(HOUR,3,DELIVERY_DATE) WHEN '3' THEN A.DELIVERY_DATE ELSE A.DELIVERY_DATE END AS SEND_DATE ,
		DELIVERY_CODE AS DELIVERY_CODE_NUM,DELIVERY_COM 
FROM CUSTOM_ETC_ORDER A 
WHERE	A.STATUS_SEQ=12 
AND		DELIVERY_DATE >= @SDATE  
AND		DELIVERY_DATE < @EDATE
AND		ISASCROW = '1'
";
                    #endregion

                    using (var command = (SqlCommand)context.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = queryText;
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@SDATE", sDate);
                        command.Parameters.AddWithValue("@EDATE", eDate);

                        await context.Database.OpenConnectionAsync(cancellationToken);

                        using (var dsr = await command.ExecuteReaderAsync(cancellationToken))
                        {
                            while (await dsr.ReadAsync(cancellationToken))
                            {
                                var mertid = (string)dsr["PG_SHOPID"];
                                var oid = (string)dsr["PG_TID"];
                                var productid = (string)dsr["CARD_CODE"];

                                var dlvtype = "";
                                var dlvdate = "";
                                var dlvcompcode = "";
                                var dlvcomp = "";
                                var dlvno = "";
                                var rcvdate = "";
                                var rcvname = "";
                                var rcvrelation = "";
                                var infodata = "";
                                var delivery_method = dsr["DELIVERY_METHOD"] == DBNull.Value ? "" : (string)dsr["DELIVERY_METHOD"];

                                if (delivery_method == "1")
                                {
                                    dlvtype = "03";
                                    dlvdate = ((DateTime)dsr["SEND_DATE"]).ToString("yyyyMMddHHmm");
                                    dlvcompcode = dsr["DELIVERY_COM"] == DBNull.Value ? "" : (string)dsr["DELIVERY_COM"];
                                    if (dlvcompcode == "CJ")
                                        dlvcomp = "CJ대한통운";
                                    else
                                        dlvcomp = "한진택배";
                                    dlvno = dsr["DELIVERY_CODE_NUM"] == DBNull.Value ? "" : (string)dsr["DELIVERY_CODE_NUM"];
                                }
                                else
                                {
                                    dlvtype = "01";
                                    rcvdate = ((DateTime)dsr["SEND_DATE"]).ToString("yyyyMMddHHmm");
                                    rcvname = dsr["NAME"] == DBNull.Value ? "" : (string)dsr["NAME"];
                                    rcvrelation = "본인";
                                }
                                var mertkey = _pgInfos.FirstOrDefault(m => m.Id == mertid)?.MertKey;

                                if (dlvtype == "01")
                                    infodata = CryptoService.ComputeMd5Hash(mertid + oid + dlvtype + rcvdate + mertkey);
                                else
                                    infodata = CryptoService.ComputeMd5Hash(mertid + oid + dlvdate + dlvcompcode + dlvno + mertkey);

                                var formData = new Dictionary<string, string>();
                                formData.Add("mid", mertid);
                                formData.Add("oid", oid);
                                formData.Add("productid", productid);
                                formData.Add("orderdate", "");
                                formData.Add("dlvtype", dlvtype);
                                formData.Add("rcvdate", rcvdate);
                                formData.Add("rcvname", rcvname);
                                formData.Add("rcvrelation", rcvrelation);
                                formData.Add("dlvdate", dlvdate);
                                formData.Add("dlvcompcode", dlvcompcode);
                                formData.Add("dlvcomp", dlvcomp);
                                formData.Add("dlvno", dlvno);
                                formData.Add("dlvworker", "");
                                formData.Add("dlvworkertel", "");
                                formData.Add("hashdata", infodata);

                                postItems.Add(formData);
                            }
                        }
                    }
                }

                //전송
                if (postItems.Count > 0)
                {
                    await CallPgAPIAsync(postItems, cancellationToken);
                }

                await SetNextTimeTaskItemAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}, has error.");

                var mailSubject = "Escrow 결제건 PG사측 배송정 업데이트 오류";
                var mailBody = new StringBuilder();
                mailBody.AppendLine("<table cellpadding=\"0\" cellspacing =\"0\" width=\"100%\">");
                mailBody.AppendLine("<tr><td>에러내용</td></tr>");
                mailBody.AppendLine("<tr><td><pre>");
                mailBody.AppendLine(ex.ToString());
                mailBody.AppendLine("</pre></td></tr>");
                mailBody.AppendLine("</table>");
                await _mail.SendAsync(mailSubject, mailBody.ToString());
            }

            _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is end.");
        }

        private async Task CallPgAPIAsync(List<Dictionary<string, string>> postItems, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient();
            Encoding encode = System.Text.Encoding.GetEncoding("euc-kr");

            foreach (var postitem in postItems)
            {
                try
                {
                    using (var request = new HttpRequestMessage())
                    {
                        request.Method = HttpMethod.Post;
                        request.RequestUri = apiUri;

                        request.Headers.TryAddWithoutValidation("Content-Type", "text/html;Charset=euc-kr");

                        var contentList = new List<string>();
                        foreach (var frm in postitem)
                        {
                            contentList.Add($"{frm.Key}={HttpUtility.UrlEncode(frm.Value, encode)}");
                        }
                        request.Content = new StringContent(string.Join("&", contentList), encode, "application/x-www-form-urlencoded");

                        var response = await client.SendAsync(request, cancellationToken);
                        response.EnsureSuccessStatusCode();

                        var contentString = await response.Content.ReadAsStringAsync();
                        //응답 로고 확인 필요시 아래 주석제거
                        //contentString = contentString.Replace(" ", "");
                        //contentString = contentString.Replace("\r", "");
                        //contentString = contentString.Replace("\n", "");
                        //_logger.LogInformation(contentString);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName} api call error.");
                }
            }
        }
    }
}
