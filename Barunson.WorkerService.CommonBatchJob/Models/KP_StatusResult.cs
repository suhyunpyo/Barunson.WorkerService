using System.Text.Json.Serialization;

namespace Barunson.WorkerService.CommonBatchJob.Models
{
    public class KP_StatusResult
    {
        public KP_StatusResult()
        {
            account = new KP_FirmAccount();
        }
        [JsonPropertyName("status")]
        public int status { set; get; }
        [JsonPropertyName("send_status")]
        public string send_status { set; get; }
        [JsonPropertyName("cid")]
        public string cid { set; get; }
        [JsonPropertyName("tid")]
        public string tid { set; get; }
        [JsonPropertyName("partner_order_id")]
        public string partner_order_id { set; get; }
        [JsonPropertyName("partner_user_id")]
        public string partner_user_id { set; get; }
        [JsonPropertyName("sender_name")]
        public string sender_name { set; get; }
        [JsonPropertyName("total_amount")]
        public int total_amount { set; get; }
        [JsonPropertyName("created_at")]
        public string created_at { set; get; }
        [JsonPropertyName("approved_at")]
        public string approved_at { set; get; }
        [JsonPropertyName("account")]
        public KP_FirmAccount account { set; get; }
    }
}
