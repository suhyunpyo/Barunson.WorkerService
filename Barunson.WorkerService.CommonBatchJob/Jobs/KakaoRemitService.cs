using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.DBModels.Barunson;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Services;
using Barunson.WorkerService.CommonBatchJob.Config;
using Barunson.WorkerService.CommonBatchJob.Models;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Barunson.WorkerService.CommonBatchJob.Jobs
{
    /// <summary>
    /// 카카오 송금 정산, 10 분간격
    /// </summary>
    internal class KakaoRemitService : BaseJob
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly KakaoBankConfig _kakaoBankConfig;


        public KakaoRemitService(ILogger<Worker> logger, IServiceProvider services, BarShopContext barShopContext,
            TelemetryClient tc, IMailSendService mail, string workerName,
            IHttpClientFactory clientFactory, KakaoBankConfig kakaoBankConfig)
            : base(logger, services, barShopContext, tc, mail, workerName, "KakaoRemitService", "*/10 * * * *")
        {
            _clientFactory = clientFactory;
            _kakaoBankConfig = kakaoBankConfig;
        }

        public override async Task Excute(CancellationToken cancellationToken)
        {
            try
            {
                if (!await IsExecute(cancellationToken))
                    return;
                _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is working.");
                var Now = DateTime.Now;

                var custCalc = new CustomerCalculate(_logger, _telemetryClient, _mail, _clientFactory, _kakaoBankConfig, _serviceProvider);
                await custCalc.ExecuteAsyn(cancellationToken);

                var now = DateTime.Now;
                if (now.Day == 10 && now.Hour >= 7 && now.Hour < 8)
                {
                    var feeCale = new FeeCalculate(_logger, _telemetryClient, _mail, _clientFactory, _kakaoBankConfig, _serviceProvider);
                    await feeCale.ExecuteAsyn(cancellationToken);
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

    public class BaseCalculate
    {
        protected readonly ILogger _logger;
        protected TelemetryClient _telemetryClient;
        protected readonly IMailSendService _mail;
        protected readonly IHttpClientFactory _clientFactory;
        protected readonly KakaoBankConfig _kakaoBankConfig;
        protected readonly IServiceProvider _serviceProvider;

        public BaseCalculate(ILogger logger, TelemetryClient telemetryClient, IMailSendService mail, IHttpClientFactory clientFactory, KakaoBankConfig kakaoBankConfig, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _telemetryClient = telemetryClient;
            _mail = mail;
            _clientFactory = clientFactory;
            _kakaoBankConfig = kakaoBankConfig;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 일자별 유니크 숫자 생성
        /// </summary>
        /// <param name="date"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task<int> GetTelegramNoAsync(string date, BarunsonContext context, CancellationToken cancellationToken)
        {
            var uniquNum = -1;

            using (var tran = await context.Database.BeginTransactionAsync(cancellationToken))
            {
                var q = from a in context.TB_Daily_Unique
                        where a.Request_Date == date
                        select a.Unique_Number;
                var item = await q.MaxAsync(m => (int?)m, cancellationToken);
                if (!item.HasValue)
                    item = 0;

                uniquNum = item.Value + 1;

                var addItem = new TB_Daily_Unique { Request_Date = date, Unique_Number = uniquNum };
                context.TB_Daily_Unique.Add(addItem);
                await context.SaveChangesAsync(cancellationToken);

                await tran.CommitAsync(cancellationToken);
            }
            return uniquNum;
        }


        #region Kakao API

        protected async Task<KP_DataSet> PostKakaoApiAsync<T>(Uri apiUri, T postBody, CancellationToken cancellationToken)
        {
            KP_DataSet data = new KP_DataSet();
            var client = _clientFactory.CreateClient();

            try
            {
                var bodystr = JsonSerializer.Serialize<T>(postBody);

                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Post;
                    request.RequestUri = apiUri;
                    request.Content = new StringContent(bodystr, Encoding.UTF8, "application/json");

                    var response = await client.SendAsync(request, cancellationToken);
                    response.EnsureSuccessStatusCode();

                    data.Success = await response.Content.ReadAsStringAsync();

                    _telemetryClient.TrackTrace($"Kakao Api Call: {apiUri}, Success: {data.Success}");
                }
            }
            catch (WebException we)
            {
                if (we.Response != null)
                {
                    using (var reader = new StreamReader(we.Response.GetResponseStream()))
                    {
                        data.Fail = reader.ReadToEnd();
                        _telemetryClient.TrackTrace($"Kakao Api Call: {apiUri}, Fail: {data.Fail}");
                    }
                }
                else
                {
                    data.Error = we.StackTrace;
                    _telemetryClient.TrackException(we);
                }
            }
            catch (Exception ex)
            {
                data.Error = ex.StackTrace;
                _telemetryClient.TrackException(ex);
            }

            return data;
        }
        #endregion
    }

    public class CustomerCalculate : BaseCalculate
    {
        private readonly int failCount = 5;

        public CustomerCalculate(ILogger logger, TelemetryClient telemetryClient, IMailSendService mail, IHttpClientFactory clientFactory, KakaoBankConfig kakaoBankConfig, IServiceProvider serviceProvider)
            : base(logger, telemetryClient, mail, clientFactory, kakaoBankConfig, serviceProvider)
        {
        }


        public async Task ExecuteAsyn(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BarunsonContext>();
                var items = await GetRetryCalculateListAsync(context, cancellationToken);

                foreach (var item in items)
                {
                    // 카카오 페이 송금 상태 API 확인 (송금 콜백 대용)
                    var status = new KP_Status();
                    status.ApiKey = _kakaoBankConfig.MainApiKey;
                    status.Cid = _kakaoBankConfig.MainCid;
                    status.Tid = item.TransactionId;

                    var apiUri = new Uri(_kakaoBankConfig.MainHost, _kakaoBankConfig.StatusUri);
                    KP_DataSet checkTransfer = await PostKakaoApiAsync<KP_Status>(apiUri, status, cancellationToken);

                    if (!string.IsNullOrWhiteSpace(checkTransfer.Success))
                    {
                        var statusResult = JsonSerializer.Deserialize<KP_StatusResult>(checkTransfer.Success);
                        //송금 결과 확인 성공이 아니면 중지, 다음 송금 확인,
                        if (statusResult.send_status != "SUCCESS_PAYMENT")
                            continue;
                    }
                    else
                    {
                        continue;
                    }

                    // 정상 송금 및 정상 정산 회수 확인(송금 콜백 대응 DB로 체크)
                    var sendStatus = await GetSendStatusAsync(item.RemitID, context, cancellationToken);
                    // 정상 송금 상태이며 정상 정산이 있으면 다음 프로세스 스킵
                    if (sendStatus.Item1 == "SUCCESS_PAYMENT" && sendStatus.Item2 == 0)
                    {
                        await ProcessingCalcuAsync(item, context, cancellationToken);
                    }

                }
            }
        }
        private async Task<List<DbCalculateData>> GetRetryCalculateListAsync(BarunsonContext context, CancellationToken cancellationToken)
        {
            var items = new List<DbCalculateData>();

            //카카오 뱅크 계좌 정보
            var kakaoAccount = await (from m in context.TB_Account_Setting
                                      orderby m.Account_Setting_ID descending
                                      select m).FirstAsync(cancellationToken);


            //정산 날짜 범위
            var date = DateTime.Now;
            var fromDateTime = date.AddDays(-10);
            var toDateTime = date;

            //정산 대상 쿼리
            var remitQuery = from Remit in context.TB_Remit
                             join InvitationTax in context.TB_Invitation_Tax on Remit.Invitation_ID equals InvitationTax.Invitation_ID
                             join Tax in context.TB_Tax on InvitationTax.Tax_ID equals Tax.Tax_ID
                             join Account in context.TB_Account on Remit.Account_ID equals Account.Account_ID
                             where Remit.Result_Code == "RC005"
                             && Remit.Complete_DateTime >= fromDateTime
                             && Remit.Complete_DateTime < toDateTime
                             && !context.TB_Calculate.Any(c => c.Remit_ID == Remit.Remit_ID
                                && c.Calculate_Type_Code == "CTC02"
                                && (c.Status_Code == "100" || c.Status_Code == "200"))
                             select new DbCalculateData
                             {
                                 RemitID = Remit.Remit_ID,
                                 AccountID = Account.Account_ID,
                                 TotalPrice = Remit.Total_Price,
                                 RemitterName = Remit.Remitter_Name,
                                 TransactionId = Remit.Transaction_ID,
                                 Tax = Tax.Tax,
                                 BankCode = Account.Bank_Code,
                                 AccountNumber = Account.Account_Number,
                                 DepositorName = Account.Depositor_Name
                             };
            var remitItems = await remitQuery.ToListAsync(cancellationToken);

            foreach (var remitItem in remitItems)
            {
                //정산 실패 카운터
                var calcFailCount = await (from c in context.TB_Calculate where c.Remit_ID == remitItem.RemitID select c).CountAsync(cancellationToken);
                if (calcFailCount <= failCount)
                {
                    remitItem.KakaoBankCode = kakaoAccount.Kakao_Bank_Code;
                    remitItem.KakaoAccountNumber = kakaoAccount.Kakao_Account_Number;

                    items.Add(remitItem);
                }
            }

            return items;
        }

        /// <summary>
        /// 정상 송금 및 정상 정산 회수 확인(송금 콜백 대응 DB로 체크)
        /// </summary>
        /// <param name="remitId"></param>
        /// <returns></returns>
        private async Task<(string, int)> GetSendStatusAsync(int remitId, BarunsonContext context, CancellationToken cancellationToken)
        {
            var sendStatus = await (from a in context.TB_Remit
                                    where a.Remit_ID == remitId
                                    select a.Send_Status
                        ).FirstOrDefaultAsync(cancellationToken);
            var count = await (from a in context.TB_Calculate
                               where a.Remit_ID == remitId
                               && (a.Status_Code == "200" || a.Status_Code == "100")
                               select a.Calculate_ID
                               ).CountAsync(cancellationToken);

            return (sendStatus, count);
        }


        /// <summary>
        /// 정산작업 진행
        /// </summary>
        /// <param name="item"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task ProcessingCalcuAsync(DbCalculateData item, BarunsonContext context, CancellationToken cancellationToken)
        {
            // 이체처리 결과 조회
            // 해당 송금건 마지막 이체 조회 (VTIM, 0011) 확인
            // VTIM, 0011 : 정산 계좌 은행 장애 건
            // 장애건 계좌 입금 확인 후 정상 송금 됬으면 입금이 되도록 처리 요청 확인
            var calcitems = await (from a in context.TB_Calculate
                                   where a.Remit_ID == item.RemitID
                                   orderby a.Calculate_ID descending
                                   select new
                                   {
                                       a.Calculate_ID,
                                       a.Remit_ID,
                                       a.Unique_Number,
                                       a.Request_Date,
                                       a.Status_Code,
                                       a.Error_Code
                                   }).ToListAsync(cancellationToken);

            //정산 요청건이 있거나 완료된 건이 있으면 정산 요청 진행 스킵
            if (calcitems.Any(m => m.Status_Code == "200" || m.Status_Code == "100"))
                return;
            //20건 이상 오류 발생 시 중지
            if (calcitems.Count > 20)
                return;

            var calcitem = calcitems.FirstOrDefault();

            KP_DataSet kakaoApiResult = null;
            TB_Calculate calculate = null;
            // 이체처리 결과 조회에서 이체가 안된경우 재정산
            if (calcitem != null && (calcitem.Error_Code == "VTIM" || calcitem.Error_Code == "0011"))
            {
                calculate = await (from a in context.TB_Calculate
                                   where a.Calculate_ID == calcitem.Calculate_ID
                                   select a).FirstAsync(cancellationToken);

                KP_Check check = new KP_Check();
                check.ApiKey = _kakaoBankConfig.BankingApiKey;
                check.OrgCode = _kakaoBankConfig.OrgCode;
                check.OrgTelegramNo = calculate.Unique_Number.Value.ToString();
                check.TrDt = calculate.Request_Date;

                var apiUri = new Uri(_kakaoBankConfig.BankingHost, _kakaoBankConfig.TransferCheckUri);
                kakaoApiResult = await PostKakaoApiAsync<KP_Check>(apiUri, check, cancellationToken);
            }
            else
            {
                string NowDate = DateTime.Now.ToString("yyyyMMdd");
                int UniqueNumber = await GetTelegramNoAsync(NowDate, context, cancellationToken);

                var FirmTransfer = new KP_FirmTransfer
                {
                    ApiKey = _kakaoBankConfig.BankingApiKey,
                    OrgCode = _kakaoBankConfig.OrgCode,
                    TelegramNo = UniqueNumber,
                    RvAccountCntn = item.RemitterName + "입금",
                    Amount = (int)(item.TotalPrice - item.Tax), // 수수료 빼고 입금
                    RvBankCode = item.BankCode,
                    RvAccount = item.AccountNumber
                };
                calculate = new TB_Calculate
                {
                    Unique_Number = UniqueNumber,
                    Calculate_Type_Code = "CTC02",
                    Request_Date = NowDate,
                    Calculate_DateTime = DateTime.Now,
                    Status_Code = "100",
                    Remit_ID = item.RemitID,
                    Remit_Price = FirmTransfer.Amount,
                    Remit_Content = FirmTransfer.RvAccountCntn,
                    Remit_Bank_Code = FirmTransfer.RvBankCode,
                    Remit_Account_Number = FirmTransfer.RvAccount
                };
                context.TB_Calculate.Add(calculate);

                var apiUri = new Uri(_kakaoBankConfig.BankingHost, _kakaoBankConfig.TransferUri);
                kakaoApiResult = await PostKakaoApiAsync<KP_FirmTransfer>(apiUri, FirmTransfer, cancellationToken);
            }

            if (kakaoApiResult.Success != null)
            {
                KP_FirmTransferResult FirmTransferResult = JsonSerializer.Deserialize<KP_FirmTransferResult>(kakaoApiResult.Success);
                if (FirmTransferResult.Status == 200)
                {
                    // 정상 처리 저장
                    calculate.Status_Code = FirmTransferResult.Status.ToString();
                    calculate.Trading_Number = FirmTransferResult.NatvTrNo;
                    calculate.Request_DateTime = FirmTransferResult.RequestAt;
                    calculate.Remit_Price = FirmTransferResult.Amount;
                }
                else
                {
                    // 통신 이상 정보 저장
                    calculate.Status_Code = FirmTransferResult.Status.ToString();
                    calculate.Error_Code = FirmTransferResult.ErrorCode;
                    calculate.Error_Message = FirmTransferResult.ErrorMessage;
                }
                await context.SaveChangesAsync(cancellationToken);
            }
            else if (kakaoApiResult.Fail != null)
            {
                KP_FirmTransferResult FirmTransferResult = JsonSerializer.Deserialize<KP_FirmTransferResult>(kakaoApiResult.Fail);

                calculate.Status_Code = FirmTransferResult.Status.ToString();
                calculate.Error_Code = FirmTransferResult.ErrorCode;
                calculate.Error_Message = FirmTransferResult.ErrorMessage;

                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }

    public class FeeCalculate : BaseCalculate
    {
        public FeeCalculate(ILogger logger, TelemetryClient telemetryClient, IMailSendService mail, IHttpClientFactory clientFactory, KakaoBankConfig kakaoBankConfig, IServiceProvider serviceProvider)
            : base(logger, telemetryClient, mail, clientFactory, kakaoBankConfig, serviceProvider)
        {
        }

        public async Task ExecuteAsyn(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BarunsonContext>();

                if (await CheckFreeCalulateAsync(context, cancellationToken))
                    return;

                var feeCalculateData = await GetFreeCalculateListAsync(context, cancellationToken);

                await ProcessingCalcuAsync(feeCalculateData, context, cancellationToken);

            }
        }
        /// <summary>
        /// 월정산 완료 여부 확인
        /// True 완료, Fales 실패
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<bool> CheckFreeCalulateAsync(BarunsonContext context, CancellationToken cancellationToken)
        {
            var date = DateTime.Today;
            var thisMon = new DateTime(date.Year, date.Month, 1);

            var query = from c in context.TB_Calculate
                        where c.Remit_ID == null
                        && c.Calculate_DateTime >= thisMon
                        && c.Calculate_Type_Code == "CTC01"
                        && c.Status_Code == "200"
                        select c;

            return await query.AnyAsync(cancellationToken);
        }

        private async Task<DbFeeCalculateData> GetFreeCalculateListAsync(BarunsonContext context, CancellationToken cancellationToken)
        {
            //정산 날짜 범위
            var date = DateTime.Today.AddMonths(-1);
            DateTime fromMonth = new DateTime(date.Year, date.Month, 1);
            DateTime toMonth = fromMonth.AddMonths(1);
            //카카오 뱅크 계좌 정보
            var kakaoAccount = await (from m in context.TB_Account_Setting
                                      orderby m.Account_Setting_ID descending
                                      select m).FirstAsync(cancellationToken);

            //정산 대상 쿼리
            var remitQuery = from Remit in context.TB_Remit
                             join InvitationTax in context.TB_Invitation_Tax on Remit.Invitation_ID equals InvitationTax.Invitation_ID
                             join Tax in context.TB_Tax on InvitationTax.Tax_ID equals Tax.Tax_ID
                             where Remit.Result_Code == "RC005"
                             && Remit.Complete_DateTime >= fromMonth
                             && Remit.Complete_DateTime < toMonth
                             && !context.TB_Calculate.Any(c => c.Remit_ID == Remit.Remit_ID
                                && c.Calculate_Type_Code == "CTC01"
                                && c.Status_Code == "200")
                             select Tax.Tax;

            var tax = await remitQuery.SumAsync(cancellationToken);

            return new DbFeeCalculateData
            {
                Tax = tax,
                KakaoBankCode = kakaoAccount.Kakao_Bank_Code,
                KakaoAccountNumber = kakaoAccount.Kakao_Account_Number,
                BankCode = kakaoAccount.Barunn_Bank_Code,
                AccountNumber = kakaoAccount.Barunn_Account_Number
            };
        }

        private async Task ProcessingCalcuAsync(DbFeeCalculateData item, BarunsonContext context, CancellationToken cancellationToken)
        {
            var date = DateTime.Today.AddMonths(-1);
            DateTime fromMonth = new DateTime(date.Year, date.Month, 1);
            DateTime toMonth = fromMonth.AddMonths(1);

            var NowDate = DateTime.Now.ToString("yyyyMMdd");
            int UniqueNumber = await GetTelegramNoAsync(NowDate, context, cancellationToken);

            // 정산 전송 데이터 설정
            KP_FirmTransfer FirmTransfer = new KP_FirmTransfer
            {
                ApiKey = _kakaoBankConfig.BankingApiKey,
                OrgCode = _kakaoBankConfig.OrgCode,
                TelegramNo = UniqueNumber,

                RvAccountCntn = "수수료정산",
                Amount = item.Tax,

                RvBankCode = item.BankCode,
                RvAccount = item.AccountNumber
            };
            var apiUri = new Uri(_kakaoBankConfig.BankingHost, _kakaoBankConfig.TransferUri);
            var kakaoApiResult = await PostKakaoApiAsync<KP_FirmTransfer>(apiUri, FirmTransfer, cancellationToken);


            using (var tran = await context.Database.BeginTransactionAsync(cancellationToken))
            {
                // 정산 정보 사전 요청 전 저장
                var calculate = new TB_Calculate
                {
                    Calculate_Type_Code = "CTC01",
                    Remit_Price = FirmTransfer.Amount,
                    Remit_Content = FirmTransfer.RvAccountCntn,
                    Remit_Bank_Code = FirmTransfer.RvBankCode,
                    Remit_Account_Number = FirmTransfer.RvAccount,
                    Unique_Number = UniqueNumber,
                    Request_Date = NowDate,
                    Calculate_DateTime = DateTime.Now
                };

                context.TB_Calculate.Add(calculate);

                if (kakaoApiResult.Success != null)
                {
                    KP_FirmTransferResult FirmTransferResult = JsonSerializer.Deserialize<KP_FirmTransferResult>(kakaoApiResult.Success);
                    // 정산 상태 저장
                    if (FirmTransferResult.Status == 200)
                    {
                        // 정상 처리 저장
                        calculate.Status_Code = FirmTransferResult.Status.ToString();
                        calculate.Trading_Number = FirmTransferResult.NatvTrNo;
                        calculate.Request_DateTime = FirmTransferResult.RequestAt;
                        calculate.Remit_Price = FirmTransferResult.Amount;

                        var remitItemsQuery = from r in context.TB_Remit
                                              join it in context.TB_Invitation_Tax on r.Invitation_ID equals it.Invitation_ID
                                              join t in context.TB_Tax on it.Tax_ID equals t.Tax_ID
                                              where r.Result_Code == "RC005"
                                              && r.Complete_DateTime >= fromMonth
                                              && r.Complete_DateTime < toMonth
                                              && !context.TB_Calculate.Any(c => c.Remit_ID == r.Remit_ID
                                                    && c.Calculate_Type_Code == "CTC01"
                                                    && c.Status_Code == "200")
                                              select new
                                              {
                                                  r.Remit_ID,
                                                  t.Tax
                                              };
                        var remitItems = await remitItemsQuery.ToListAsync(cancellationToken);

                        foreach (var targetItem in remitItems)
                        {
                            var newcalc = new TB_Calculate
                            {
                                Remit_ID = targetItem.Remit_ID,
                                Calculate_Type_Code = "CTC01",
                                Remit_Price = targetItem.Tax,
                                Remit_Bank_Code = item.BankCode,
                                Remit_Account_Number = item.AccountNumber,
                                Remit_Content = "수수료정산",
                                Trading_Number = FirmTransferResult.NatvTrNo,
                                Unique_Number = UniqueNumber,
                                Request_DateTime = FirmTransferResult.RequestAt,
                                Request_Date = NowDate,
                                Status_Code = FirmTransferResult.Status.ToString(),
                                Calculate_DateTime = calculate.Calculate_DateTime
                            };
                            context.TB_Calculate.Add(newcalc);
                        }
                    }
                    else
                    {
                        // 통신 이상 정보 저장
                        calculate.Status_Code = FirmTransferResult.Status.ToString();
                        calculate.Error_Code = FirmTransferResult.ErrorCode;
                        calculate.Error_Message = FirmTransferResult.ErrorMessage;

                    }
                    await context.SaveChangesAsync(cancellationToken);
                    await tran.CommitAsync(cancellationToken);
                }
                else if (kakaoApiResult.Fail != null)
                {
                    KP_FirmTransferResult FirmTransferResult = JsonSerializer.Deserialize<KP_FirmTransferResult>(kakaoApiResult.Fail);

                    calculate.Status_Code = FirmTransferResult.Status.ToString();
                    calculate.Error_Code = FirmTransferResult.ErrorCode;
                    calculate.Error_Message = FirmTransferResult.ErrorMessage;

                    await context.SaveChangesAsync(cancellationToken);
                    await tran.CommitAsync(cancellationToken);
                }
                else
                {
                    await tran.RollbackAsync(cancellationToken);
                }
            }
        }
    }
}
