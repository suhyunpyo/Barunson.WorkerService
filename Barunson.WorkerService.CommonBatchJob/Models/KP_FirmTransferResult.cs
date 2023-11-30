using System.Text.Json.Serialization;

namespace Barunson.WorkerService.CommonBatchJob.Models
{
    public class KP_FirmTransferResult : KP_Result
    {
        [JsonPropertyName("natv_tr_no")]
        public string NatvTrNo { set; get; }
        [JsonPropertyName("request_at")]
        public string RequestAt { set; get; }
        [JsonPropertyName("amount")]
        public int Amount { set; get; }
    }
}
