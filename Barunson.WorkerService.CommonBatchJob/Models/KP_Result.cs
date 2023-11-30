using System.Text.Json.Serialization;

namespace Barunson.WorkerService.CommonBatchJob.Models
{
    public class KP_Result
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }
        [JsonPropertyName("error_code")]
        public string ErrorCode { set; get; }
        [JsonPropertyName("error_message")]
        public string ErrorMessage { set; get; }
    }
}
