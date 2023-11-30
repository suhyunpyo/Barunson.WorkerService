using System.Text.Json.Serialization;

namespace Barunson.WorkerService.CommonBatchJob.Models
{
    public class KP_FirmInquireDepositorResult : KP_Result
    {
        [JsonPropertyName("natv_tr_no")]
        public string NatvTrNo { set; get; }
        [JsonPropertyName("request_at")]
        public string RequestAt { set; get; }
        [JsonPropertyName("depositor")]
        public string Depositor { set; get; }
    }
}
