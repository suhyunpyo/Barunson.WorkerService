using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.DBModels.BarShop;
using Barunson.WorkerService.Common.DBModels.MoSvr;
using Barunson.WorkerService.Common.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Barunson.WorkerService.Common.Services
{
    /// <summary>
    /// SMS, MMS, BizTalk 발송 서비스
    /// (중요) 문자발송 테스트를 위한 환경
    /// Debug 모드시 _test로 등록된 번호로 1건만 발송하고 응답은 무조건 False로 출력, 
    /// 사용자 암호관리에서 TestDataOption (AppConfig.cs 참조) 을 등록 해야 함. 예) LMS용, 알림톡용
    /// "TestData": {
    ///     "LMSDestInfo": "이름^010-1234-1234",
    ///     "RecipientNum": "010-1234-1234"
    /// }
    /// Release 모드에서는 TestData 사용되지 않음.
    /// </summary>
    public interface ILMSSendService
    {
        /// <summary>
        /// SMS 발송
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        Task<bool> SendSMSAsync(List<SmsSendModel> models, CancellationToken cancellationToken);

        /// <summary>
        /// MMS 발송
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        Task<bool> SendMMSAsync(List<MmsSendModel> models, CancellationToken cancellationToken);

        static Dictionary<string, SiteInfo> LMSSiteInfos = SiteInfo.GetSiteInfos();

        /// <summary>
        /// 알림톡 발송
        /// </summary>
        /// <param name="models"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> SendBizTalkAsync(List<BizTalkModel> models, CancellationToken cancellationToken);
    }

    /// <summary>
    /// SMS, MMS, BizTalk 발송 서비스
    /// (중요) 문자발송 테스트를 위한 환경
    /// Debug 모드시 _test로 등록된 번호로 1건만 발송하고 응답은 무조건 False로 출력, 
    /// 사용자 암호관리에서 TestDataOption (AppConfig.cs 참조) 을 등록 해야 함. 예) LMS용, 알림톡용
    /// "TestData": {
    ///     "LMSDestInfo": "이름^010-1234-1234",
    ///     "RecipientNum": "010-1234-1234"
    /// }
    /// Release 모드에서는 TestData 사용되지 않음.
    /// </summary>
    public class LMSSendService : ILMSSendService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<LMSSendService> _logger;
        private readonly TestDataOption _options;

#if DEBUG
        private readonly bool IsSendLMS = false;
#else
        private readonly bool IsSendLMS = true;
#endif
        //아래 값을 테스트 할 전화 번호로 수정.
        /// <summary>
        /// 이름^전화번호
        /// </summary>
        private readonly string _testLMSDestInfo = "";
        /// <summary>
        /// 전화번호, 비즈톡용
        /// </summary>
        private readonly string _testRecipientNum = "";

        public LMSSendService(ILogger<LMSSendService> logger, IServiceProvider serviceProvider, TestDataOption options)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _options = options;
            if (_options != null)
            {
                _testLMSDestInfo = _options.LMSDestInfo;
                _testRecipientNum = _options.RecipientNum;
            }
        }

        public async Task<bool> SendSMSAsync(List<SmsSendModel> models, CancellationToken cancellationToken)
        {
            bool result = false;
            try
            {
                if (models == null || models.Count == 0)
                    return result;

                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<MoSvrContext>();
                    using (var trans = await context.Database.BeginTransactionAsync())
                    {
                        if (IsSendLMS)
                        {
                            foreach (var model in models)
                            {
                                var addItem = GetSMSModel(model);

                                context.SDK_SMS_SEND.Add(addItem);
                            }
                        }
                        else
                        {
                            var addItem = GetSMSModel(models.First());
                            addItem.DEST_INFO = _testLMSDestInfo;
                            context.SDK_SMS_SEND.Add(addItem);
                        }
                        await context.SaveChangesAsync();
                        await trans.CommitAsync();

                        if (IsSendLMS)
                            result = true;

                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now:hh:mm:ss} LMSSendService has error.");
            }

            return result;
        }

        private SDK_SMS_SEND GetSMSModel(SmsSendModel model)
        {
            return new SDK_SMS_SEND
            {
                USER_ID = model.UserId,
                SCHEDULE_TYPE = (int)model.ScheduledType,
                SUBJECT = String.Empty,
                SMS_MSG = model.Message,
                NOW_DATE = DateTime.Now.ToString("yyyyMMddHHmmss"),
                SEND_DATE = model.SendTime.ToString("yyyyMMddHHmmss"),
                CALLBACK = model.CallBack,
                DEST_COUNT = model.DestCount,
                DEST_INFO = model.DestInfo,
                RESERVED1 = model.Reserved1,
                RESERVED2 = model.Reserved2,
                RESERVED3 = model.Reserved3,
                RESERVED4 = model.Reserved4,
                RESERVED5 = model.Reserved5,
                RESERVED6 = model.Reserved6,
                RESERVED7 = model.Reserved7,
                RESERVED8 = model.Reserved8,
                RESERVED9 = model.Reserved9
            };
        }

        public async Task<bool> SendMMSAsync(List<MmsSendModel> models, CancellationToken cancellationToken)
        {

            bool result = false;
            try
            {
                if (models == null || models.Count == 0)
                    return result;

                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<MoSvrContext>();
                    using (var trans = await context.Database.BeginTransactionAsync())
                    {
                        if (IsSendLMS)
                        {
                            foreach (var model in models)
                            {
                                var addItem = GetMMSModel(model);

                                context.SDK_MMS_SEND.Add(addItem);
                            }
                        }
                        else
                        {
                            var addItem = GetMMSModel(models.First());
                            addItem.DEST_INFO = _testLMSDestInfo;
                            context.SDK_MMS_SEND.Add(addItem);
                        }
                        await context.SaveChangesAsync();
                        await trans.CommitAsync();
                    }
                    if (IsSendLMS)
                        result = true;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now:hh:mm:ss} LMSSendService has error.");
            }

            return result;
        }

        private SDK_MMS_SEND GetMMSModel(MmsSendModel model)
        {
            return new SDK_MMS_SEND
            {
                USER_ID = model.UserId,
                SCHEDULE_TYPE = (int)model.ScheduledType,
                SUBJECT = model.Subject,
                MMS_MSG = model.Message,
                NOW_DATE = DateTime.Now.ToString("yyyyMMddHHmmss"),
                SEND_DATE = model.SendTime.ToString("yyyyMMddHHmmss"),
                CALLBACK = model.CallBack,
                DEST_COUNT = model.DestCount,
                DEST_INFO = model.DestInfo,
                CONTENT_COUNT = model.ContentCount,
                CONTENT_DATA = model.ContentData,
                MSG_TYPE = (int)model.MsgType,
                RESERVED1 = model.Reserved1,
                RESERVED2 = model.Reserved2,
                RESERVED3 = model.Reserved3,
                RESERVED4 = model.Reserved4,
                RESERVED5 = model.Reserved5,
                RESERVED6 = model.Reserved6,
                RESERVED7 = model.Reserved7,
                RESERVED8 = model.Reserved8,
                RESERVED9 = model.Reserved9
            };
        }

        public async Task<bool> SendBizTalkAsync(List<BizTalkModel> models, CancellationToken cancellationToken)
        {
            bool result = false;
            try
            {
                if (models == null || models.Count == 0)
                    return result;

                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<BarShopContext>();
                    using (var trans = await context.Database.BeginTransactionAsync())
                    {
                        if (IsSendLMS)
                        {
                            foreach (var model in models)
                            {
                                var addItem = GetBizTalkModel(model);

                                context.ata_mmt_tran.Add(addItem);
                            }
                        }
                        else
                        {
                            var addItem = GetBizTalkModel(models.First());
                            addItem.recipient_num = _testRecipientNum;
                            context.ata_mmt_tran.Add(addItem);
                        }
                        await context.SaveChangesAsync();
                        await trans.CommitAsync();
                        if (IsSendLMS)
                            result = true;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now:hh:mm:ss} LMSSendService has error.");
            }

            return result;
        }

        private ata_mmt_tran GetBizTalkModel(BizTalkModel model)
        {
            return new ata_mmt_tran
            {
                date_client_req = model.SendTime,
                subject = model.Subject,
                content = model.Message,
                callback = model.CallBack,
                msg_status = "1",
                recipient_num = model.RecipientNum,
                msg_type = (int)model.MessageType,
                sender_key = model.Senderkey,
                template_code = model.TemplateCode,
                kko_btn_type = model.KKOBtnType,
                kko_btn_info = model.KKOBtnInfo,
                etc_text_1 = model.EtcText1,
                etc_text_2 = model.EtcText2,
                etc_text_3 = model.EtcText3,
                etc_num_1 = model.EtcNum1,
                etc_num_2 = model.EtcNum2,
                etc_num_3 = model.EtcNum3
            };
        }
    }

    /// <summary>
    /// SMS 문자 전송 데이터모델
    /// </summary>
    public class SmsSendModel
    {
        /// <summary>
        /// (공통)회원 ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// (공통)제목(MMS에서만 발송시 사용)
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// (공통)발송 메시지
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// (공통)발송시점 구분(즉시전송:0, 예약전송:1)
        /// </summary>
        public LMSScheduledType ScheduledType { get; set; }
        /// <summary>
        /// (공통)발송희망시간(예약발송시 사용) ex)YYYYMMDDHHMMSS
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// (공통)회신번호
        /// </summary>
        public string CallBack { get; set; }
        /// <summary>
        /// (공통)수신자 목록 개수(Max:100)
        /// </summary>
        public int DestCount { get; set; }
        /// <summary>
        /// (공통)수신자이름^전화번호 ex)홍길동^01012341234|홍길순^01012341234|~
        /// | 으로 구분
        /// </summary>
        public string DestInfo { get; set; }

        /// <summary>
        /// 여분필드 1 > SALES_GUBUN ex)SA, ST, SS, SB, B, H 기타 등등
        /// </summary>
        public string Reserved1 { get; set; }
        /// <summary>
        /// 여분필드 2 > 문자메시지 용도
        /// </summary>
        public string Reserved2 { get; set; }

        /// <summary>
        /// 여분필드 3 > 비고, 기타 내용
        /// </summary>
        public string Reserved3 { get; set; }
        /// <summary>
        /// 여분필드 4
        /// </summary>
        public string Reserved4 { get; set; }
        /// <summary>
        /// 여분필드 5
        /// </summary>
        public string Reserved5 { get; set; }
        /// <summary>
        /// 여분필드 6
        /// </summary>
        public string Reserved6 { get; set; }
        /// <summary>
        /// 여분필드 7
        /// </summary>
        public string Reserved7 { get; set; }
        /// <summary>
        /// 여분필드 8
        /// </summary>
        public string Reserved8 { get; set; }
        /// <summary>
        /// 여분필드 9
        /// </summary>
        public string Reserved9 { get; set; }
    }

    /// <summary>
    /// MMS 문자 전송 데이터모델
    /// </summary>
    public class MmsSendModel : SmsSendModel
    {

        /// <summary>
        /// (MMS)전송파일수
        /// </summary>
        public int ContentCount { get; set; }
        /// <summary>
        /// (MMS)파일명^컨텐츠타입^컨텐츠서브타입 
        /// ex)http://www.test.com/test.jpg^1^0|http://www.test.com/test.jpg^1^0|~
        /// </summary>
        public string ContentData { get; set; }
        /// <summary>
        /// (MMS)메시지 구분(TEXT:0, HTML:1)
        /// </summary>
        public LMSMessageType MsgType { get; set; }

    }

    /// <summary>
    /// 알림톡 데이터모델 모델
    /// </summary>
    public class BizTalkModel
    {
        /// <summary>
        /// 전송 예약 시간
        /// date_client_req
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// 제목
        /// subject
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 발송 메시지
        /// content
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 회신번호
        /// </summary>
        public string CallBack { get; set; }
        /// <summary>
        /// 수신자전화번호
        /// </summary>
        public string RecipientNum { get; set; }
        /// <summary>
        /// 메시지 종류(1008-알림톡, 1009-친구톡)
        ///  msg_type
        /// </summary>
        public BizTalkMsgType MessageType { get; set; } = BizTalkMsgType.Notice;
        /// <summary>
        /// 발신 프로필 키
        /// </summary>
        public string Senderkey { get; set; }
        /// <summary>
        /// 메시지 유형 템플릿 코드
        /// </summary>
        public string TemplateCode { get; set; }
        /// <summary>
        /// 카카오톡 전송방식 1-format string 2-JSON 3-XML
        /// </summary>
        public string KKOBtnType { get; set; }
        /// <summary>
        /// 버튼템플릿 전송시 버튼 정보
        /// </summary>
        public string KKOBtnInfo { get; set; }
        /// <summary>
        /// Sale Gubun
        /// </summary>
        public string EtcText1 { get; set; }
        /// <summary>
        /// 호출프로시저 , function name
        /// </summary>
        public string EtcText2 { get; set; }
        public string EtcText3 { get; set; }
        /// <summary>
        /// company_Seq 
        /// </summary>
        public int? EtcNum1 { get; set; }
        public int? EtcNum2 { get; set; }
        public int? EtcNum3 { get; set; }
    }

    /// <summary>
    /// 발송시점 구분(즉시전송:0, 예약전송:1)
    /// </summary>
    public enum LMSScheduledType
    {
        /// <summary>
        /// 즉시전송
        /// </summary>
        Immediate = 0,
        /// <summary>
        /// 예약전송
        /// </summary>
        Scheduled = 1
    }

    /// <summary>
    /// 메시지 구분(TEXT:0, HTML:1)
    /// </summary>
    public enum LMSMessageType
    {
        Text = 0,
        HTML = 1
    }

    /// <summary>
    /// 메시지 종류(1008-알림톡, 1009-친구톡)
    /// </summary>
    public enum BizTalkMsgType
    {
        Notice = 1008,
        Friend = 1009
    }
}
