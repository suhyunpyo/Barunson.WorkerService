using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.DBModels.BarShop;
using Barunson.WorkerService.Common.Services;
using Barunson.WorkerService.LMSBatchJob.Jobs;
using Barunson.WorkerService.LMSBatchJob.Models;
using Cronos;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Barunson.WorkerService.LMSBatchJob
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IServiceProvider _serviceProvider;
        private TelemetryClient _telemetryClient;
        private readonly ILMSSendService _mms;
        private readonly IMailSendService _mail;

        private string WorkerName { get => "LMSBatchJob"; }


        public Worker(ILogger<Worker> logger, TelemetryClient tc, IHostApplicationLifetime hostApplicationLifetime, IServiceProvider services, ILMSSendService mms, IMailSendService mail)
        {
            _logger = logger;
            _telemetryClient = tc;
            _hostApplicationLifetime = hostApplicationLifetime;
            _serviceProvider = services;
            _mms = mms;
            _mail = mail;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{WorkerName} running at: {time}", WorkerName, DateTimeOffset.Now);
            
            using (var operation = _telemetryClient.StartOperation<RequestTelemetry>(WorkerName))
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var TaskContext = scope.ServiceProvider.GetRequiredService<BarShopContext>();

                        //매일
                        // 답례품 관련 LMS 발송, 매일 오전 11:00
                        await new SendForGift(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, _mms, WorkerName).Excute(cancellationToken);
                        //답례품 업체 주문건수 SMS, 매일 오전 8:00
                        await new SendOrderGift(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, _mms, WorkerName).Excute(cancellationToken);
                        //구매후기 독려 알림톡 , 매일 오후 5:00
                        await new SendOrderReview(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, _mms, WorkerName).Excute(cancellationToken);
                        //바른손 샘플 주문 프페청첩장 제안, 매일 오후 2:00
                        await new SendSampleOrderMMS(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, _mms, WorkerName).Excute(cancellationToken);
                        //선결제 주문자(초안확정및인쇄요청) SMS 발송, 매일 오전 10:00
                        await new SendPreSettleSMS(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, _mms, WorkerName).Excute(cancellationToken);
                        //식전영상 쿠폰 잔여확인문자, 매일 오전 9:30, 기존 Sp 호출 유지
                        await new SendCouponOutOfStockSMS(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, _mms, WorkerName).Excute(cancellationToken);
                        //입고 알림문자, 매일 09~19시 10분  3시간 간격
                        await new SendRestockSMS(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, _mms, WorkerName).Excute(cancellationToken);
                        //초안확정유도 알림톡, 매일 오전 11:10
                        await new SendChoanConfirmBizTalk(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, _mms, WorkerName).Excute(cancellationToken);
                        //바른손 얼리버드 구매독려 LMS 발송, 매일 16:50, 쿠폰발행으로 기존 SP 호출 유지
                        await new SendEarlybirdMMS(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, _mms, WorkerName).Excute(cancellationToken);
                        //바른손카드 경상도 고객 감사장 구매유도 LMS 발송, 매일 12:30
                        await new SendBarunsonCardThankCardMMS(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, _mms, WorkerName).Excute(cancellationToken);
                        //바른손카드 샘플후기 독려 알림톡 , 매일 오후 5:10
                        await new SendSampleOrderReview(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, _mms, WorkerName).Excute(cancellationToken);
                        //주문모니터링, 매 1시간
                        await new OrderMonitoring(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, _mms, WorkerName).Excute(cancellationToken);
                        //광고, 정보 문자 발송, 매 10분
                        await new SendSMSMaster(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, _mms, WorkerName).Excute(cancellationToken);
                        // 예식일 임박/경과 고객 LMS 발송, 매일 오전 10:10      
                        await new SendLMSComeNPassWeddingDay(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, _mms, WorkerName).Excute(cancellationToken);
                                                                  
                    }
                    operation.Telemetry.Success = true;
                }
                catch (Exception ex)
                {
                    operation.Telemetry.Success = false;

                    _logger.LogError(ex, "{time:yyyy-MM-dd HH:mm:ss} {WorkerName} has error.", DateTime.Now, WorkerName);
                }
                
            }
            await _telemetryClient.FlushAsync(cancellationToken);
            _logger.LogInformation("{WorkerName} stop at: {time}", WorkerName, DateTimeOffset.Now);
            _hostApplicationLifetime.StopApplication();
        }
              

    }
}
