using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.DBModels.BarShop;
using Barunson.WorkerService.Common.Services;
using Cronos;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Barunson.WorkerService.Common.Jobs
{
    /// <summary>
    /// 배치 작업 기본 클레스
    /// </summary>
    public abstract class BaseJob
    {
        private BarShopContext TaskContext { get; set; }

        protected readonly ILogger _logger;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IMailSendService _mail;
        protected readonly TelemetryClient _telemetryClient;

        protected string WorkerName { get; set; }
        protected string funcName { get; set; }
        protected string Cron { get; set; }

        public BaseJob(ILogger logger, IServiceProvider services, BarShopContext barShopContext, TelemetryClient tc, IMailSendService mail, 
            string worker, string func, string cron)
        {
            _logger = logger;
            _serviceProvider = services;
            TaskContext = barShopContext;
            _telemetryClient = tc;
            _mail = mail;
            WorkerName = worker;
            funcName = func;
            Cron = cron;
        }

        /// <summary>
        /// 실행 함수
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task Excute(CancellationToken cancellationToken);

        #region Task Item data
        /// <summary>
        /// 실행 여부
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task<bool> IsExecute(CancellationToken cancellationToken)
        {
            var taskQuery = from m in TaskContext.BarunWorkerTask
                            where m.WorkerName == WorkerName && m.FunctionName == funcName
                            select m;
            var taskItem = await taskQuery.FirstOrDefaultAsync(cancellationToken);
            var now = DateTime.Now;
            //실행시간 1시간 경과되어도 실행되지 않을 경우 메일 경고
            //매일 지속발송 방지를 위하여 2시간이 경과되면 발송하지 않음.
            if (taskItem != null && taskItem.NextExcuteTime.HasValue 
                && taskItem.NextExcuteTime <= now.AddHours(-1) && !(taskItem.NextExcuteTime <= now.AddHours(-2)))
            {
                
                var mailSubject = $"[Worker] {funcName} 작업 점검 요청";

                var mailBody = new StringBuilder();
                mailBody.AppendLine("<table cellpadding=\"0\" cellspacing =\"0\" width=\"100%\">");
                mailBody.AppendLine("<tr>");
                mailBody.AppendLine($"<td>작업 함수명: {WorkerName} - {funcName}</td>");
                mailBody.AppendLine($"<td>실행할 시간: {taskItem.NextExcuteTime}</td>");
                mailBody.AppendLine($"<td>마지막 실행: {taskItem.LastExcuteTime}</td>");
                mailBody.AppendLine($"<td>요청 시간: {now}</td>");
                mailBody.AppendLine("</tr>");

                await _mail.SendAsync(mailSubject, mailBody.ToString());
            }

            return (taskItem != null && taskItem.NextExcuteTime.HasValue && taskItem.NextExcuteTime <= now);
        }
        /// <summary>
        /// 다음 실행 시간 업데이트
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task SetNextTimeTaskItemAsync(CancellationToken cancellationToken)
        {
            var taskQuery = from m in TaskContext.BarunWorkerTask
                            where m.WorkerName == WorkerName && m.FunctionName == funcName
                            select m;
            var taskItem = await taskQuery.FirstAsync(cancellationToken);

            taskItem.LastExcuteTime = DateTime.Now;
            taskItem.NextExcuteTime = GetNextTime(Cron);
            await TaskContext.SaveChangesAsync(cancellationToken);
        }

        private static DateTime? GetNextTime(string cron)
        {
            var cronexpression = CronExpression.Parse(cron);
            var next = cronexpression.GetNextOccurrence(DateTimeOffset.Now, TimeZoneInfo.Local);
            return next?.DateTime;
        }

        protected async Task<BarunWorkerTask> GetCurruntWorkTask()
        {
            var taskQuery = from m in TaskContext.BarunWorkerTask
                            where m.WorkerName == WorkerName && m.FunctionName == funcName
                            select m;
            return await taskQuery.FirstOrDefaultAsync();
        }
        #endregion
    }
}
