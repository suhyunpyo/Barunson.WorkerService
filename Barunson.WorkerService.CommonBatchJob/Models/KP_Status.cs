using System.Text.Json.Serialization;

namespace Barunson.WorkerService.CommonBatchJob.Models
{
    class KP_Status
    {
        [JsonPropertyName("api_key")]
        public string ApiKey { set; get; }
        [JsonPropertyName("cid")]
        public string Cid { set; get; }
        [JsonPropertyName("tid")]
        public string Tid { set; get; }
    }
}
