using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.LMSBatchJob.Jobs
{
    /// <summary>
    /// 답례품 관련 LMS 발송
    /// 예식일 기준 D-12 고객 LMS 발송
    /// LMS 수신 동의 고객 (바, 몰, 고객)
    /// </summary>
    internal class SendForGift : LMSBaseJob
    {
        public SendForGift(ILogger<Worker> logger, IServiceProvider services, BarShopContext taskContext,
           TelemetryClient tc, IMailSendService mail, ILMSSendService mms, string workerName)
           : base(logger, services, taskContext, tc, mail, mms, workerName, "SendForGift", "0 11 * * *")
        { }

        public override async Task Excute(CancellationToken cancellationToken)
        {
            try
            {
                if (!await IsExecute(cancellationToken))
                    return;

                _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is working.");

                #region MMS Template
                var subject = "(광고)[바른손카드] 답례품 할인 쿠폰 안내";
                var mmsMsg = @"(광고)[바른손카드] 답례품 할인 쿠폰 안내
{0}님 답례품 챙기셨나요?

회사원분들이 선호하는 베스트 답례품을
바른손지샵 최대 혜택으로 만나보세요!

▶ 배송비 무료쿠폰
1개만 구매해도 배송비가 무료!
배송비는 바른손에서 지원해 드립니다

▶ 4종의 쿠폰팩
바른손 회원 가입 시 최대 100,000원까지 할인 받을 수 있는
쿠폰 4종을 즉시 발급해 드려요.

▶ 바로 사용 가능한 적립금 3,000원까지!

▶ 쿠폰확인하기
https://bit.ly/3QbIt8y

*확인시점에 따라 쿠폰여부가 상이할 수 있습니다.

▶ 답례품도 바른손에서,바른손지샵
https://bit.ly/3PPq1C8

[수신거부] {1} 고객센터
 {2}로 수신거부 문자 전송";
                #endregion

                var weddDate = DateTime.Now.AddDays(12).Date.ToString("yyyy-MM-dd");
                var now = DateTime.Now;

                var sendModels = new List<MmsSendModel>();
                var logInserts = new List<string>();

                using (var fncScope = _serviceProvider.CreateScope())
                {
                    var barshopContext = fncScope.ServiceProvider.GetRequiredService<BarShopContext>();

                    //발송대상
                    var query = from w in barshopContext.custom_order_WeddInfo
                                join m in barshopContext.custom_order on w.order_seq equals m.order_seq
                                join u in barshopContext.VW_USER_INFO on
                                    new { uid = m.member_id, site = (m.sales_Gubun == "C" || m.sales_Gubun == "H") ? "B" : m.sales_Gubun }
                                    equals new { uid = u.uid, site = u.site_div }
                                where m.status_seq == 15 && u.chk_sms == "Y"
                                && (u.site_div == "SB" || u.site_div == "B")
                                && w.wedd_date == weddDate
                                && m.order_hphone.Length == 13
                                select new
                                {
                                    u.site_div,
                                    u.uid,
                                    m.order_hphone,
                                    m.order_name,
                                    m.company_seq
                                };

                    var targets = await query.ToListAsync(cancellationToken);

                    foreach (var item in targets)
                    {

                        var defaultInfo = ILMSSendService.LMSSiteInfos["SB"];
                        var site = item.site_div;

                        if (ILMSSendService.LMSSiteInfos.ContainsKey(site))
                            defaultInfo = ILMSSendService.LMSSiteInfos[site];

                        sendModels.Add(new MmsSendModel
                        {
                            UserId = item.uid,
                            Subject = subject,
                            Message = String.Format(mmsMsg, item.order_name, defaultInfo.Brand, defaultInfo.CallBack),
                            ScheduledType = LMSScheduledType.Immediate,
                            SendTime = DateTime.Now,
                            CallBack = defaultInfo.CallBack,
                            DestCount = 1,
                            DestInfo = $"{item.uid}^{item.order_hphone}",
                            ContentCount = 0,
                            ContentData = "",
                            MsgType = LMSMessageType.Text,
                            Reserved1 = site,
                            Reserved2 = "",
                            Reserved3 = "",
                            Reserved4 = "2",
                            Reserved5 = ""
                        });

                        logInserts.Add($"insert into GIFT_DAILY_MMS (send_dt,uid) values ('{now:yyyy-MM-dd}', '{item.uid}')");

                    }
                    var successMMS = await _mms.SendMMSAsync(sendModels, cancellationToken);
                    if (successMMS)
                    {
                        foreach (var log in logInserts)
                            await barshopContext.Database.ExecuteSqlRawAsync(log);
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
    }
}
