using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Barunson.WorkerService.Common.Services
{
    public class SuccessfulDependencyFilter : ITelemetryProcessor
    {
        private ITelemetryProcessor Next { get; set; }

        // next will point to the next TelemetryProcessor in the chain.
        public SuccessfulDependencyFilter(ITelemetryProcessor next)
        {
            this.Next = next;
        }

        public void Process(ITelemetry item)
        {
            // To filter out an item, return without calling the next processor.
            if (!OKtoSend(item)) { return; }

            this.Next.Process(item);
        }

        // Example: replace with your own criteria.
        private bool OKtoSend(ITelemetry item)
        {
            var dependency = item as DependencyTelemetry;
            if (dependency == null) return true;

            return dependency.Success != true;
        }
    }

    /// <summary>
    /// SQL DB 호출에서 정상은 기록하지 않도록 필터 설정
    /// </summary>
    public class NoSQLDependencies : ITelemetryProcessor
    {
        private ITelemetryProcessor Next { get; set; }

        // next will point to the next TelemetryProcessor in the chain.
        public NoSQLDependencies(ITelemetryProcessor next)
        {
            this.Next = next;
        }

        public void Process(ITelemetry item)
        {
            if (IsSQLDependency(item)) { return; }

            this.Next.Process(item);
        }
        
        private bool IsSQLDependency(ITelemetry item)
        {
            var dependency = item as DependencyTelemetry;
            if (dependency == null) return false;
            if (dependency.Type == "SQL" && dependency.Success == true)
            {
                return true;
            }
            return false;
        }
    }
}
