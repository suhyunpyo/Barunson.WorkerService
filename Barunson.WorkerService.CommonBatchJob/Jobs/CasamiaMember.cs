using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.DBModels.BarShop;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Barunson.WorkerService.CommonBatchJob.Jobs
{
    /// <summary>
    /// casamia 멤버십 생성 및 전송, 매일 5:30 
    /// </summary>
    internal class CasamiaMember: BaseJob
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILMSSendService _mms;

        private readonly Uri apiUri = new Uri("https://guudmembers.casamia.co.kr/api/members/joinMem");
        private readonly Byte[] HMACKey = Encoding.UTF8.GetBytes("8aloDWRRcr");

        public CasamiaMember(ILogger<Worker> logger, IServiceProvider services, BarShopContext barShopContext,
            TelemetryClient tc, IMailSendService mail, string workerName,
           IHttpClientFactory clientFactory, ILMSSendService mms)
           : base(logger, services, barShopContext, tc, mail, workerName, "CasamiaMember", "30 5 * * *")
        {
            _clientFactory = clientFactory;
            _mms = mms;
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
                    var insertCount = await CreateDataAsync(cancellationToken);
                    var sendModels = await GetDataModelAsync(cancellationToken);
                    var sendCount = await SendDataAsync(sendModels, cancellationToken);

                    _telemetryClient.TrackTrace($"생성된 회원수: {insertCount}, 전송대상 회원수: {sendModels.Count}, 전송성공 회원수: {sendCount}");

                    var sendMMSCount = await SendMMSAsync(cancellationToken);
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


        /// <summary>
        /// 바른손, 비핸즈(제휴포함), 더카드, 프리미어 일별 회원가입 데이터 -> CASAMIA_DAILY_INFO 로 중복제거 후 인서트
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<int> CreateDataAsync(CancellationToken cancellationToken)
        {
            var todt = DateTime.Today;
            var fromdt = todt.AddDays(-3); //기본값 3일전 데이터를 읽음
            int addCount = 0;

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BarShopContext>();

                //카사미아 마지막 생성일 기준으로 User 정보 읽기, 프로그램이 실행되지 않았을 경우 마지막 실행일 기준으로 데이터 읽기
                var lastWorkTime = await (from m in context.CASAMIA_DAILY_INFO select m.create_date).MaxAsync(cancellationToken);
                if (lastWorkTime.HasValue && lastWorkTime.Value < fromdt)
                    fromdt = lastWorkTime.Value.Date;

                var query = from m in context.S2_UserInfo
                            where m.site_div == "SB"
                                && m.casamiaship_reg_Date >= fromdt
                                && m.casamiaship_reg_Date < todt
                                && m.chk_casamiamembership == "Y"
                                && m.casamiaship_leave_date == null
                            select new
                            {
                                m.uid,
                                m.uname,
                                m.ConnInfo,
                                m.Gender,
                                m.birth_div,
                                m.birth,
                                m.phone1,
                                m.phone2,
                                m.phone3,
                                m.hand_phone1,
                                m.hand_phone2,
                                m.hand_phone3,
                                m.zip1,
                                m.zip2,
                                m.address,
                                m.addr_detail,
                                m.umail,
                                m.REFERER_SALES_GUBUN,
                                m.INTERGRATION_DATE
                            };
                var items = await query.ToListAsync(cancellationToken);

                //CASAMIA_DAILY_INFO 에 등록된 회원 목록 읽기
                var connInfos = items.Select(m => m.ConnInfo);
                var existsQuery = from m in context.CASAMIA_DAILY_INFO
                                  where connInfos.Contains(m.ConnInfo)
                                  select m.ConnInfo;
                var existsItems = await existsQuery.ToListAsync(cancellationToken);

                var addItmes = new List<CASAMIA_DAILY_INFO>();
                foreach (var item in items)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    //등록된 회원은 추가하지 않음.
                    if (existsItems.Contains(item.ConnInfo))
                        continue;

                    addItmes.Add(new CASAMIA_DAILY_INFO
                    {
                        uid = item.uid,
                        uname = item.uname,
                        ConnInfo = item.ConnInfo,
                        gender = item.Gender == "0" ? "F" : item.Gender == "1" ? "M" : item.Gender,
                        birth_div = item.birth_div,
                        Birth_date = item.birth?.Replace("-", ""),
                        phone = item.phone1 + "-" + item.phone2 + "-" + item.phone3,
                        hand_phone = item.hand_phone1 + "-" + item.hand_phone2 + "-" + item.hand_phone3,
                        zipcode = item.zip1 + item.zip2,
                        address = item.address,
                        addr_detail = item.addr_detail,
                        umail = item.umail,
                        wedding_day = await (from s in context.VW_USER_INFO where s.uid == item.uid select s.WEDDING_DAY).FirstOrDefaultAsync(cancellationToken),
                        barun_reg_site = item.REFERER_SALES_GUBUN,
                        barun_reg_Date = item.INTERGRATION_DATE,
                        create_date = DateTime.Now
                    });
                }
                if (addItmes.Count > 0)
                {
                    context.CASAMIA_DAILY_INFO.AddRange(addItmes);
                    addCount = await context.SaveChangesAsync(cancellationToken);
                }
            }
            return addCount;
        }

        /// <summary>
        /// CASAMIA_DAILY_INFO에서 전달할 데이터 모델 생성
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<Dictionary<int, SendModel>> GetDataModelAsync(CancellationToken cancellationToken)
        {
            var sendModels = new Dictionary<int, SendModel>();

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BarShopContext>();

                //카사미아에 전달하지 않은 데이터
                var query = from m in context.CASAMIA_DAILY_INFO
                            where m.casamia_send_date == null
                            select new
                            {
                                m.uid,
                                m.uname,
                                m.umail,
                                m.birth_div,
                                m.Birth_date,
                                m.gender,
                                m.ConnInfo,
                                m.wedding_day,
                                m.hand_phone,
                                m.phone,
                                m.zipcode,
                                m.address,
                                m.addr_detail,
                                m.barun_reg_Date,
                                m.barun_reg_site,
                                m.seq
                            };

                var items = await query.ToListAsync(cancellationToken);
                foreach (var item in items)
                {
                    var now = DateTime.Now;
                    var smodel = new SendModel
                    {
                        SEND_DT = now.ToString("yyyyMMdd"),
                        SEND_TIME = now.ToString("HHmm") + "00",
                        TR_NO = $"bhnads{item.seq:D18}",
                        TR_DT = item.barun_reg_Date?.ToString("yyyyMMdd"),
                        TR_TIME = item.barun_reg_Date?.ToString("HHmmss"),
                        CUST_NM = item.uname,
                        BIRTH_DT = item.Birth_date,
                        GEN_GBN_CD = item.gender,
                        LUNAR_GBN_CD = item.birth_div,
                        IDENTI_VAL = item.ConnInfo,
                        REALNM_CERTI_DATE = item.barun_reg_Date?.ToString("yyyyMMddHHmmss"),
                        HHP_NO = item.hand_phone,
                        EMAIL_ADDR = item.umail,
                        ADDR2_ZIP = item.zipcode,
                        ADDR2_DFLT = item.address,
                        ADDR2_DTL = item.addr_detail,
                        CLUB_DTL_INFO = new SendModelCLUB_DTL_INFO
                        {
                            WED_DT = item.wedding_day?.Replace("-", "")
                        }
                    };
                    sendModels.Add(item.seq, smodel);
                }
            }
            return sendModels;
        }

        /// <summary>
        /// 카사미아 API 호출 및 성공여부 업데이트
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<int> SendDataAsync(Dictionary<int, SendModel> models, CancellationToken cancellationToken)
        {
            int count = 0;

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BarShopContext>();
                foreach (var model in models)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var now = DateTime.Now;
                    var item = model.Value;
                    item.SEND_DT = now.ToString("yyyyMMdd");
                    item.SEND_TIME = now.ToString("HHmm") + "00";

                    //API 호출
                    var rst_cd = await PostMessageAsync(item);
                    if (!string.IsNullOrEmpty(rst_cd) && rst_cd != "106") //비인가 오류가 아닐시에만 기록
                    {
                        var updateItemQuery = from m in context.CASAMIA_DAILY_INFO where m.seq == model.Key select m;
                        var updateItem = await updateItemQuery.FirstOrDefaultAsync(cancellationToken);
                        if (updateItem != null)
                        {
                            updateItem.casamia_rst_cd = rst_cd;
                            updateItem.casamia_send_date = DateTime.Now;

                            await context.SaveChangesAsync(cancellationToken);

                            count++;
                        }
                    }
                }
            }
            return count;
        }

        private async Task<string> PostMessageAsync(SendModel model)
        {
            string result = null;

            var requestBody = JsonConvert.SerializeObject(model, Newtonsoft.Json.Formatting.None);
            //HMAC Hash는 사용하지 않음. IP로 허용됨, 향후 보안 설정이변경 될 경우 아래 주석 제거
            //var signature = GenerateHMAC(requestBody);

            var client = _clientFactory.CreateClient();
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = apiUri;

                request.Headers.TryAddWithoutValidation("User-Agent", "ASP/3.0");

                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                try
                {
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    var contentString = await response.Content.ReadAsStringAsync();

                    var json = JObject.Parse(contentString);
                    result = (string)json["HEAD_DATA"][0]["RST_CD"];

                    //전송 응답 로그 확인 필요시 아래 주석 제거
                    //_logger.LogInformation($"logseq: {model.TR_NO}, json: {json.ToString(Formatting.None)}, rst_cd: {result}");

                    /* api 응답 예)
                    {
                      "BODY_DATA": [],
                      "HEAD_DATA": [
                        {
                          "RST": "E",
                          "RST_CD": "106",
                          "RST_MSG": "비인가요청오류-FO"
                        }
                      ]
                    }*/
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} CaamiaJob Post api call error. TR_NO: {model.TR_NO}");
                }
            }
            return result;

        }

        /// <summary>
        /// HMAC 헤시 값 생성
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string GenerateHMAC(string msg)
        {
            string digest = null;

            Byte[] textBytes = Encoding.UTF8.GetBytes(msg);
            Byte[] hashBytes;

            using (HMACSHA256 hash = new HMACSHA256(HMACKey))
                hashBytes = hash.ComputeHash(textBytes);

            digest = BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();

            return digest;
        }

        /// <summary>
        /// 까사미아 DB제휴 중복고객 대상 자동 LMS발송
        /// 매주 화, 목요일 11시 30분 발송
        /// </summary>
        /// <returns></returns>
        private async Task<int> SendMMSAsync(CancellationToken cancellationToken)
        {
            var todt = DateTime.Today;
            int count = 0;
            if (todt.DayOfWeek == DayOfWeek.Tuesday || todt.DayOfWeek == DayOfWeek.Thursday)
            {
                //발송시간 11:30
                var sendDateTime = todt.AddHours(11).AddMinutes(30);
                var fromDate = todt.AddDays(-5);

                var sendModels = new List<MmsSendModel>();

                var subject = "(광고) {0} X 까사미아";
                var mmsMsg = @"(광고)이 문자는 {0}에 가입하신 까사미아 회원님들께만 발송됩니다.

까사미아 웨딩클럽까지 가입하셔서 
풍성한 혜택으로 신혼가구 졸업하세요:)  

<웨딩클럽 혜택>  
① 10% 상시할인(정상가 기준)  
② 굳포인트 2만점 적립(신규 가입시)

<가입서류>  
가입자와 예식일이 표기된 청첩장, 웨딩홀, 스튜디오, 컨설팅 계약서 중 1개  

<유지기간>  
클럽 승인 후 6개월간 

웨딩클럽 가입하기☞https://vo.la/2ipTy  

*유의사항*  
- 회원명과 가입서류의 이름이 다르거나 
예식일이 6개월이 지난 경우 승인이 거절될 수 있습니다  
- 당 혜택은 신세계까사와 제휴사의 
사정에 의해 예고 없이 변경될 수 있습니다.
- 굳포인트는 웨딩클럽 승인 후 3일 내 적립되며, 
유효 기간은 적립일로부터 3개월입니다.
- 자세한 내용은 매장 직원 또는 
신세계까사 고객센터(1588-3408)에 문의 바랍니다.";

                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<BarShopContext>();

                    //카사미아에 전달하지 않은 데이터
                    var query = from m in context.CASAMIA_DAILY_INFO
                                where m.casamia_rst_cd == "209" && m.mms_send_Date == null && m.create_date > fromDate
                                select m;

                    var items = await query.ToListAsync(cancellationToken);
                    count = items.Count;

                    foreach (var item in items)
                    {
                        var defaultInfo = ILMSSendService.LMSSiteInfos["SS"];
                        if (!string.IsNullOrEmpty(item.barun_reg_site) && ILMSSendService.LMSSiteInfos.ContainsKey(item.barun_reg_site))
                            defaultInfo = ILMSSendService.LMSSiteInfos[item.barun_reg_site];

                        sendModels.Add(new MmsSendModel
                        {
                            UserId = item.uid,
                            Subject = String.Format(subject, defaultInfo.Brand),
                            Message = String.Format(mmsMsg, defaultInfo.Brand),
                            ScheduledType = LMSScheduledType.Scheduled,
                            SendTime = sendDateTime,
                            CallBack = defaultInfo.CallBack,
                            DestCount = 1,
                            DestInfo = $"{item.uid}^{item.hand_phone}",
                            ContentCount = 0,
                            ContentData = "",
                            MsgType = LMSMessageType.Text,
                            Reserved1 = item.barun_reg_site,
                            Reserved2 = "",
                            Reserved3 = "",
                            Reserved4 = "1",
                            Reserved5 = ""
                        });

                        item.mms_send_Date = sendDateTime;
                    }
                    var successMMS = await _mms.SendMMSAsync(sendModels, cancellationToken);
                    if (successMMS)
                    {
                        await context.SaveChangesAsync(cancellationToken);
                    }
                }
            }
            return count;
        }


        public class SendModel
        {
            public string REQ_TYPE { get; set; } = "REQ_INT_1004";
            public string SEND_DT { get; set; }
            public string SEND_TIME { get; set; }
            public string TR_NO { get; set; }
            public string TR_DT { get; set; }
            public string TR_TIME { get; set; }
            public string CUST_NM { get; set; }
            public string BIRTH_DT { get; set; }
            public string GEN_GBN_CD { get; set; }
            public string LUNAR_GBN_CD { get; set; }
            public string IDENTI_VAL { get; set; }
            public string REALNM_CERTI_DATE { get; set; }
            public string HHP_NO { get; set; }
            public string EMAIL_ADDR { get; set; }
            public string ADDR2_ZIP { get; set; }
            public string ADDR2_DFLT { get; set; }
            public string ADDR2_DTL { get; set; }
            public string CHNL_CD { get; set; } = "CH0000028";
            public string PGM_NO { get; set; } = "P000001";
            public string JOIN_STORE_CD { get; set; } = "1000";
            public string JOIN_CHNL_CD { get; set; } = "CH0000046";
            public string JOIN_DEVICE_TYPE { get; set; } = "PCWEB";
            public string MBR_LOCK_TY { get; set; } = "1";
            public string CUST_TYPE_CD { get; set; } = "01";
            public string REALNM_CERTI_YN { get; set; } = "Y";
            public string FOREIGN_CD { get; set; } = "N";
            public string IDENTI_TYPE_CD { get; set; } = "01";
            public string CHNL_AGRE_SMS { get; set; } = "Y";
            public string CHNL_AGRE_TEL { get; set; } = "N";
            public string CHNL_AGRE_DM { get; set; } = "N";
            public string CHNL_AGRE_EMAIL { get; set; } = "N";
            public string CHNL_AGRE_KAKAO { get; set; } = "N";
            public string MBR_TERMS_AGRE { get; set; } = "Y";
            public string MKT_AGRE_PRV_INFO { get; set; } = "Y";
            public string MKT_AGRE_USE { get; set; } = "Y";
            public string MKT_AGRE_LBS_SVC { get; set; } = "Y";
            public string MKT_AGRE_PRV_REF { get; set; } = "Y";
            public string NEW_USER_YN { get; set; } = "Y";
            public string AGE_AGRE_YN { get; set; } = "Y";
            public string CLUB_CD { get; set; } = "CLB001";
            public SendModelCLUB_DTL_INFO CLUB_DTL_INFO { get; set; }

        }
        public class SendModelCLUB_DTL_INFO
        {
            public string WED_DT { get; set; }
            public string BED_RM_YN { get; set; } = "N";
            public string DRS_RM_YN { get; set; } = "N";
            public string DIN_RM_YN { get; set; } = "N";
            public string LIV_RM_YN { get; set; } = "N";
            public string JNR_RM_YN { get; set; } = "N";
            public string KID_RM_YN { get; set; } = "N";
            public string ACTIVE_YN { get; set; } = "Y";
        }
    }
}
