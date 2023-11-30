using System.Text.Json.Serialization;

namespace Barunson.WorkerService.CommonBatchJob.Models
{
    public class KP_FirmInquireDepositor : KP_Firm
    {
        [JsonPropertyName("bank_code")]
        public string BankCode { set; get; }
        [JsonPropertyName("account")]
        public string Account { set; get; }
    }
}
