using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barunson.WorkerService.LMSBatchJob.Jobs
{
    internal abstract class LMSBaseJob : BaseJob
    {
        protected readonly ILMSSendService _mms;

        public LMSBaseJob(ILogger logger, IServiceProvider services, BarShopContext taskContext, TelemetryClient tc, IMailSendService mail, ILMSSendService mms
            , string worker, string func, string cron) 
            : base(logger, services, taskContext, tc, mail, worker, func, cron)
        {
            _mms = mms;
        }

        public abstract override Task Excute(CancellationToken cancellationToken);
        
    }
}
