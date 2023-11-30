using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.DBModels.BarShop;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.LMSBatchJob.Jobs
{
    /// <summary>
    /// 예식일 임박/경과 고객 LMS 발송
    /// </summary>
    internal class SendLMSComeNPassWeddingDay: LMSBaseJob
    {
        public SendLMSComeNPassWeddingDay(ILogger logger, IServiceProvider services, BarShopContext taskContext, TelemetryClient tc, IMailSendService mail, ILMSSendService mms
            , string workerName)
            : base(logger, services, taskContext, tc, mail, mms, workerName, "SendLMSComeNPassWeddingDay", "10 10 * * *")
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
                int totalCnt = 0;

                #region LMS Template
                var Come_subject = "[#SITE_NAME] 예식일 임박";
                var Pass_subject = "[#SITE_NAME] 예식일 경과";
                var Come_lmsMsg_Format = @"[#SITE_NAME] 
안녕하세요 고객님, #SITE_NAME입니다.

고객님의 예식일이 임박하였습니다. 
주문 후 결제까지 완료 된 청첩장 주문건이 있어, 미진행 시 환불 처리 도움드리고자 알림 드립니다. 

마이페이지-1:1친절상담란에 글 남겨주시면 신속하게 처리해드리겠습니다. 감사합니다.

#SITE_NAME 고객센터
#PHONE_NUMBER
#SITE_DOMAIN";
                var Pass_lmsMsg_Format = @"[#SITE_NAME] 
안녕하세요 고객님, #SITE_NAME입니다.

고객님의 예식일이 지났습니다. 
주문 후 결제까지 완료 된 청첩장 주문건이 있어, 환불 처리 도움드리고자 알림 드립니다. 

마이페이지-1:1친절상담란에 글 남겨주시면 신속하게 처리해드리겠습니다. 감사합니다.

#SITE_NAME 고객센터
#PHONE_NUMBER
#SITE_DOMAIN";

                #endregion

                var toDt = Now.Date;
                var fromSettleDt = toDt.AddMonths(-6);   //--결제일 기준 (6개월내)
                var fromPassDt = toDt.AddDays(-14);      //--경과기준일 (14일 전일)
                var fromComeDt = toDt.AddDays(14);       //--임박기준일 (14일 후일)

                var lmsList = new List<MmsSendModel>();
                var salesGubun = new List<string> { "SB", "SS", "B", "H", "C", "SD" };
                var orderStatus = new List<int> { 1, 6, 7, 8 };
                using (var fncScope = _serviceProvider.CreateScope())
                {
                    var barshopContext = fncScope.ServiceProvider.GetRequiredService<BarShopContext>();

                    var targets = new List<WeddingComePass>();

                    //--예식일 경과 주문 추출
                    var weddingPass = from o in barshopContext.custom_order
                                      join w in barshopContext.custom_order_WeddInfo on o.order_seq equals w.order_seq
                                      where orderStatus.Contains(o.status_seq)
                                      && salesGubun.Contains(o.sales_Gubun)
                                      && o.settle_status == 2
                                      && o.settle_date > fromSettleDt
                                      && o.settle_price > 0
                                      && o.order_type != "2"
                                      && o.order_type != "3"
                                      && o.member_id != "s4guest"
                                      && w.event_year == fromPassDt.Year.ToString()
                                      && w.event_month == fromPassDt.Month.ToString()
                                      && w.event_Day == fromPassDt.Day.ToString()
                                      select new WeddingComePass
                                      {
                                          IsCome = false,
                                          PhoneNum = o.order_hphone.Replace("-", ""),
                                          Site = o.sales_Gubun,
                                          OrderSeq = o.order_seq
                                      };

                    targets.AddRange(await weddingPass.ToListAsync(cancellationToken));

                    //--예식일 임박 주문 추출
                    var weddingCome = from o in barshopContext.custom_order
                                      join w in barshopContext.custom_order_WeddInfo on o.order_seq equals w.order_seq
                                      where orderStatus.Contains(o.status_seq)
                                      && salesGubun.Contains(o.sales_Gubun)
                                      && o.settle_status == 2
                                      && o.settle_date > fromSettleDt
                                      && o.settle_price > 0
                                      && o.order_type != "2"
                                      && o.order_type != "3"
                                      && o.member_id != "s4guest"
                                      && w.event_year == fromComeDt.Year.ToString()
                                      && w.event_month == fromComeDt.Month.ToString()
                                      && w.event_Day == fromComeDt.Day.ToString()
                                      select new WeddingComePass
                                      {
                                          IsCome = true,
                                          PhoneNum = o.order_hphone.Replace("-", ""),
                                          Site = o.sales_Gubun,
                                          OrderSeq = o.order_seq
                                      };

                    targets.AddRange(await weddingCome.ToListAsync(cancellationToken));

                    foreach (var item in targets)
                    {
                        var defaultInfo = ILMSSendService.LMSSiteInfos["SB"];
                        if (!string.IsNullOrEmpty(item.Site) && ILMSSendService.LMSSiteInfos.ContainsKey(item.Site))
                        {
                            defaultInfo = ILMSSendService.LMSSiteInfos[item.Site];
                        }
                        else
                        {
                            continue;
                        }

                        string subject = string.Empty;
                        string msg = string.Empty;
                        if (item.IsCome)
                        {
                            subject = Come_subject.Replace("#SITE_NAME", defaultInfo.Brand);
                            msg = Come_lmsMsg_Format.Replace("#SITE_NAME", defaultInfo.Brand).Replace("#PHONE_NUMBER", defaultInfo.CallBack).Replace("#SITE_DOMAIN", defaultInfo.Site);
                        }
                        else
                        {
                            subject = Pass_subject.Replace("#SITE_NAME", defaultInfo.Brand);
                            msg = Pass_lmsMsg_Format.Replace("#SITE_NAME", defaultInfo.Brand).Replace("#PHONE_NUMBER", defaultInfo.CallBack).Replace("#SITE_DOMAIN", defaultInfo.Site);
                        }

                        lmsList.Add(new MmsSendModel
                        {
                            UserId = "",
                            Subject = subject,
                            Message = msg,
                            ScheduledType = LMSScheduledType.Immediate,
                            SendTime = DateTime.Now,
                            CallBack = defaultInfo.CallBack,
                            DestCount = 1,
                            DestInfo = $"AA^{item.PhoneNum}",
                            ContentCount = 0,
                            ContentData = "",
                            MsgType = LMSMessageType.Text,
                            Reserved1 = item.Site,
                            Reserved2 = "",
                            Reserved3 = "",
                            Reserved4 = "2",
                            Reserved5 = ""
                        });

                        //--CS 메모 추가
                        barshopContext.CUSTOM_ORDER_ADMIN_MENT.Add(new CUSTOM_ORDER_ADMIN_MENT
                        {
                            ISWOrder = "1",
                            MENT = "LMS전송 : " + msg,
                            ORDER_SEQ = item.OrderSeq,
                            PCHECK = null,
                            STATUS = 0,
                            ADMIN_ID = "worker",
                            REG_DATE = DateTime.Now,
                            isJumun = "1",
                            intype = 4,
                            sgubun = item.Site,
                            stype = "기타",
                            Mtype = "AM01",
                            Category = "AMC0701"
                        }); ;

                        totalCnt++;
                    }

                    var successMMS = await _mms.SendMMSAsync(lmsList, cancellationToken);
                    if (successMMS)
                    {
                        await barshopContext.SaveChangesAsync(cancellationToken);
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

        private class WeddingComePass
        {
            public bool IsCome { get; set; }
            public string PhoneNum { get; set; }
            public string Site { get; set; }
            public int OrderSeq { get; set; }
        }
    }
}
