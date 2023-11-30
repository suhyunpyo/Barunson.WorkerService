using System.Text.Json.Serialization;

namespace Barunson.WorkerService.CommonBatchJob.Models
{
    public class KP_FirmAccount : KP_Firm
    {
        [JsonPropertyName("number")]
        public string Number { set; get; }
        [JsonPropertyName("bank_code")]
        public string BankCode { set; get; }
    }
}
