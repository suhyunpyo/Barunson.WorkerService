using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.CommonBatchJob.Jobs
{
    /// <summary>
    /// 비회원 주문후 회원가입시 주문에 memeberid 업데이트
    /// 매시 40분에 실행
    /// </summary>
    internal class CheckSessionMemeberId : BaseJob
    {

        public CheckSessionMemeberId(ILogger<Worker> logger, IServiceProvider services, BarShopContext barShopContext,
            TelemetryClient tc, IMailSendService mail, string workerName)
            : base(logger, services, barShopContext, tc, mail, workerName, "CheckSessionMemeberId", "40 * * * *")
        { }
        public override async Task Excute(CancellationToken cancellationToken)
        {
            try
            {
                if (!await IsExecute(cancellationToken))
                    return;
                _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is working.");
                var Now = DateTime.Now;

                using (var fncScope = _serviceProvider.CreateScope())
                {
                    var targetDate = Now.AddDays(-1);
                    var barshopContext = fncScope.ServiceProvider.GetRequiredService<BarShopContext>();
                    using (var trans = await barshopContext.Database.BeginTransactionAsync(cancellationToken))
                    {
                        var testEmails = new List<string>
                        {
                            "developer@barunn.net",
                            "test@gmail.com",
                            "test@naver.com",
                            "a@naver.com",
                            "A@naver.com"
                        };
                        //비회원 주문 목록
                        var orderQ = from m in barshopContext.custom_order
                                     where m.member_id == "" && m.order_date > targetDate
                                     && !testEmails.Contains(m.order_email)
                                     select m;
                        var orderItems = await orderQ.ToListAsync(cancellationToken);

                        //회원 목록 email
                        var emails = orderItems.Select(x => x.order_email).Distinct().ToList();
                        var memberQ = from m in barshopContext.S2_UserInfo_TheCard
                                      where emails.Contains(m.umail)
                                      select new { m.uid, m.umail };
                        var memberItems = await memberQ.ToListAsync(cancellationToken);

                        var existsMembers = from o in orderItems
                                            join m in memberItems on o.order_email equals m.umail
                                            select new { o, m };

                        foreach (var orderItem in existsMembers)
                        {
                            orderItem.o.member_id = orderItem.m.uid;
                            var cmdText = $"insert into chk_session_log (member_id, order_seq, created_tmstmp) values ('{orderItem.o.member_id}' ,{orderItem.o.order_seq}, getdate())";
                            await barshopContext.Database.ExecuteSqlRawAsync(cmdText);
                        }
                        await barshopContext.SaveChangesAsync(cancellationToken);
                        await trans.CommitAsync(cancellationToken);
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
