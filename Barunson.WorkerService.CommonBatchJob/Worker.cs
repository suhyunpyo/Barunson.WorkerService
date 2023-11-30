using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.Models;
using Barunson.WorkerService.Common.Services;
using Barunson.WorkerService.CommonBatchJob.Config;
using Barunson.WorkerService.CommonBatchJob.Jobs;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Barunson.WorkerService.CommonBatchJob
{
    public class Worker : BackgroundService
    {
        private readonly IConfiguration _appconfig;
        private readonly ILogger<Worker> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IServiceProvider _serviceProvider;
        private TelemetryClient _telemetryClient;
        private readonly SMBUser _sMBUser;
        private readonly CJLogisticsAPIConfig _cjConfig;
        private readonly IMailSendService _mail;
        private readonly IHttpClientFactory _clientFactory;
        private readonly List<PgMertInfo> _pgInfos;
        private readonly KakaoBankConfig _kakaoBankConfig;
        private readonly ILMSSendService _mms;

        private string WorkerName { get => "BarunBatchJob"; }

        public Worker(ILogger<Worker> logger, TelemetryClient tc, IConfiguration appconfig, IHostApplicationLifetime hostApplicationLifetime, IServiceProvider services, 
            SMBUser sMBUser, IMailSendService mail, IHttpClientFactory clientFactory, CJLogisticsAPIConfig cjConfig, List<PgMertInfo> pgInfos,
            KakaoBankConfig kakaoBankConfig, ILMSSendService mms)
        {
            _appconfig = appconfig;
            _logger = logger;
            _telemetryClient = tc;
            _hostApplicationLifetime = hostApplicationLifetime;
            _serviceProvider = services;
            _sMBUser = sMBUser;
            _cjConfig = cjConfig;
            _mail = mail;
            _clientFactory = clientFactory;
            _pgInfos = pgInfos;
            _kakaoBankConfig = kakaoBankConfig;
            _mms = mms;
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

                        //매시간, 분
                        // 바비더프몰 결제완료 / 초안컨펌완료 주문건에 한해서 쿠폰 발급, 10분간격
                        // 모바일 청첩장을 구매
                        await new MobileOrderCouponPublish(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName).Excute(cancellationToken);
                        // 회원전환_식전영상쿠폰발급, 4 시간 간격
                        await new FeelMakerCoupon(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName).Excute(cancellationToken);
                        // 주문세션확인 비회원 주문후 회원가입시 주문에 memeberid 업데이트, 매시 40분
                        await new CheckSessionMemeberId(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName).Excute(cancellationToken);
                        // DearDeer 주문 오류, 매시 20분 간격
                        await new DearDeerOrderFailCheck(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName).Excute(cancellationToken);
                        // 메일 발송 작업, 매 10분
                        await new SendMail(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName, _clientFactory).Excute(cancellationToken);
                        // 화환 선물, 매 10 분
                        await new FlaSystem(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName, _clientFactory).Excute(cancellationToken);
                        // CJ 배송정보 업데이트 3시간(30분) 간격
                        await new CJLogistics(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName, _clientFactory, _cjConfig).Excute(cancellationToken);
                        // 카카오 송금 정산, 10 분간격
                        await new KakaoRemitService(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName, _clientFactory, _kakaoBankConfig).Excute(cancellationToken);
                        // ASP Error Log 파일 취합, 매시 10분
                        await new AspErrorLogGathering(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName, _sMBUser).Excute(cancellationToken);
                        // ERP-XERP, 스마트 재고 실시간 연동,  10분간격 
                        await new S2CardERPStock(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName).Excute(cancellationToken);

                        //매일
                        // [메인] 혜택배너, 매일 오전 0:30 
                        await new MainBenefitBanner(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName).Excute(cancellationToken);
                        // 바른손 베스트, 매일 오전 0:40
                        await new CardBestRanking(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName).Excute(cancellationToken);
                        // 바른손 M 카드 송금, 주문 통계, 매일 오전 0:50
                        await new MobileCardStatistice(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName).Excute(cancellationToken);
                        // 바른손 M 무통장입금대기자동취소, 매일 오전 0:10
                        await new MobileCardDepositWaitingOrder(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName).Excute(cancellationToken);
                        // ERP-XERP, 바른손 Shop, 매일 오전 1:30
                        await new BarunsonErp(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName).Excute(cancellationToken);
                        // 대기 초안 취소 - 매일 오전 1:00
                        await new ChoanCancel(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName).Excute(cancellationToken);
                        // Escrow 정보 업데이트 매일 23: 30
                        await new EscrowPgUpdate(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName, _clientFactory, _pgInfos).Excute(cancellationToken);

                        // casamia 멤버십 생성 및 전송, 매일 5:30 
                        await new CasamiaMember(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName, _clientFactory, _mms).Excute(cancellationToken);
                        // IWedding 멤버십 생성 및 전송, 매일 6:10 
                        await new IWeddingMember(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName, _clientFactory).Excute(cancellationToken);
                        // Memplus 맴버십, 매일 4:20
                        await new MemPlusMember(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName, _appconfig).Excute(cancellationToken);

                        // 셈플 주문 통계 계산, 매일 오전 1:30
                        await new SampleOrderStatistics(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName).Excute(cancellationToken);
                        // 경쟁사 사이트 카드 정보, 매일 오전 11:50
                        await new CompetitorSiteCheck(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName, _clientFactory).Excute(cancellationToken);

                        // 주문 결제 업데이트 실패건 처리, 매일 7,8시 30분
                        await new TossPaymentCheck(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName, _clientFactory, _pgInfos).Excute(cancellationToken);

                        //특정 날짜
                        // LMS 발송 데이터 백업, 매월 첫 일요일 오전 3시
                        await new LMSDataBackup(_logger, _serviceProvider, TaskContext, _telemetryClient, _mail, WorkerName).Excute(cancellationToken);
                       
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