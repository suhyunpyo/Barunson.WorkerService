using System.Text.Json.Serialization;

namespace Barunson.WorkerService.CommonBatchJob.Models
{
    class KP_Check
    {
        [JsonPropertyName("api_key")]
        public string ApiKey { set; get; }
        [JsonPropertyName("org_code")]
        public string OrgCode { set; get; }
        [JsonPropertyName("org_telegram_no")]
        public string OrgTelegramNo { set; get; }
        [JsonPropertyName("tr_dt")]
        public string TrDt { set; get; }
        [JsonPropertyName("drw_bank_code")]
        public string DrwBankCode { set; get; }
    }
}
