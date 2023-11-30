using System.Text.Json.Serialization;

namespace Barunson.WorkerService.CommonBatchJob.Models
{
    public class KP_Firm
    {
        [JsonPropertyName("api_key")]
        public string ApiKey { set; get; }
        [JsonPropertyName("org_code")]
        public string OrgCode { set; get; }
        [JsonPropertyName("telegram_no")]
        public int? TelegramNo { set; get; }
    }
}
