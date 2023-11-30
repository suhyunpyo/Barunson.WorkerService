using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.LMSBatchJob.Jobs
{
    /// <summary>
    /// 바른손카드 샘플 주문 + LMS 수신 허용
    /// 회원가입 시, 예식장 '호텔' 설정 or 바른손카드 샘플 선택 중, 소비자가 1,200원 이상 2종 이상 선택
    /// LMS 발송일 기준 예식일 6개월 이후 or 예식일 1개월 전 고객 제외
    /// 청첩장 구매 고객 제외
    /// 샘플 발송일 기준, +4일 이후 LMS 자동 발송
    /// </summary>
    internal class SendSampleOrderMMS: LMSBaseJob
    {
        public SendSampleOrderMMS(ILogger logger, IServiceProvider services, BarShopContext taskContext, TelemetryClient tc, IMailSendService mail, ILMSSendService mms
           , string workerName)
           : base(logger, services, taskContext, tc, mail, mms, workerName, "SendSampleOrderMMS", "0 14 * * *")
        {
        }

        public override async Task Excute(CancellationToken cancellationToken)
        {
            try
            {
                if (!await IsExecute(cancellationToken))
                    return;

                _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is working.");

                var Now = DateTime.Now;

                #region MMS Template
                var defaultInfo = ILMSSendService.LMSSiteInfos["SB"];

                var subject = $"[광고] {defaultInfo.Brand} 고객을 위한 프리미엄 청첩장 제안";
                var mmsMsg = string.Format(@"[광고] {0} 고객을 위한
프리미엄 청첩장 제안-

하나뿐인 우리의 결혼식,
주인공이 되는 특별한 날인만큼
기억에 남을 아름다운 웨딩을 꿈꾼다면!

셀럽들의 특별한 날을 함께한
프리미어페이퍼 청첩장으로
당신의 한 번 뿐인 웨딩을 빛내보세요!

▶ 셀럽이 선택한 청첩장 보러가기
https://m.premierpaper.co.kr/mobile/product/c_choice_lms.asp

[수신거부] {0} 고객센터
 {1}로 수신거부 문자 전송", defaultInfo.Brand, defaultInfo.CallBack);

                #endregion                                

                var fromDt = Now.AddDays(-5).Date;
                var toDt = Now.AddDays(-4).Date;
                //14:30 발송예약
                var sendTime = Now.Date.AddHours(14).AddMinutes(30);

                var sendModels = new List<MmsSendModel>();

                using (var fncScope = _serviceProvider.CreateScope())
                {
                    var barshopContext = fncScope.ServiceProvider.GetRequiredService<BarShopContext>();

                    //발송대상
                    var targetQ = from c in barshopContext.CUSTOM_SAMPLE_ORDER
                                  join u in barshopContext.VW_USER_INFO on
                                     new { uid = c.MEMBER_ID, site = c.SALES_GUBUN }
                                     equals new { uid = u.uid, site = u.site_div }
                                  where !string.IsNullOrEmpty(c.MEMBER_ID) && u.chk_sms == "Y"
                                  && c.SALES_GUBUN == "SB"
                                  && c.DELIVERY_DATE >= fromDt && c.DELIVERY_DATE < toDt
                                  && u.HPHONE.Length > 12
                                  && u.WEDDING_DAY.CompareTo(Now.AddMonths(6).Date.ToString("yyyy-MM-dd")) < 0
                                  && u.WEDDING_DAY.CompareTo(Now.AddMonths(1).Date.ToString("yyyy-MM-dd")) > 0
                                  && !barshopContext.custom_order.Any(x => x.member_id == c.MEMBER_ID && x.status_seq > 0
                                     && x.status_seq != 3 && x.status_seq != 5
                                     && (x.order_type == "1" || x.order_type == "6" || x.order_type == "7"))
                                  select new
                                  {
                                      c.SALES_GUBUN,
                                      u.HPHONE,
                                      u.WEDDING_HALL,
                                      CardCount = (from ci in barshopContext.CUSTOM_SAMPLE_ORDER_ITEM
                                                   join s in barshopContext.S2_Card on ci.CARD_SEQ equals s.Card_Seq
                                                   where ci.SAMPLE_ORDER_SEQ == c.sample_order_seq
                                                   && s.CardSet_Price >= 1200
                                                   select ci.CARD_SEQ).Count()
                                  };
                    var targets = await targetQ.Where(x => x.WEDDING_HALL == "H" || x.CardCount >= 2).ToListAsync(cancellationToken);
                    foreach (var target in targets)
                    {
                        sendModels.Add(new MmsSendModel
                        {
                            UserId = "",
                            Subject = subject,
                            Message = mmsMsg,
                            ScheduledType = LMSScheduledType.Scheduled,
                            SendTime = sendTime,
                            CallBack = defaultInfo.CallBack,
                            DestCount = 1,
                            DestInfo = $"AA^{target.HPHONE}",
                            ContentCount = 0,
                            ContentData = "",
                            MsgType = LMSMessageType.Text,
                            Reserved1 = target.SALES_GUBUN,
                            Reserved2 = "",
                            Reserved3 = "",
                            Reserved4 = "",
                            Reserved5 = ""
                        });
                    }
                }
                var success = await _mms.SendMMSAsync(sendModels, cancellationToken);

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
