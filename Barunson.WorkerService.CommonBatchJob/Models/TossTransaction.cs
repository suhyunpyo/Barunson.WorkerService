using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Barunson.WorkerService.CommonBatchJob.Models
{
    internal class TossTransaction
    {
        [JsonPropertyName("mId")]
        public string mId { get; set; }
        
        [JsonPropertyName("transactionKey")]
        public string transactionKey { get; set; }

        [JsonPropertyName("paymentKey")]
        public string paymentKey { get; set; }

        [JsonPropertyName("orderId")]
        public string orderId { get; set; }

        [JsonPropertyName("method")]
        public string method { get; set; }

        [JsonPropertyName("customerKey")]
        public string customerKey { get; set; }

        [JsonPropertyName("useEscrow")]
        public Boolean useEscrow { get; set; }

        [JsonPropertyName("receiptUrl")]
        public string receiptUrl { get; set; }

        [JsonPropertyName("status")]
        public string status { get; set; }

        [JsonPropertyName("transactionAt")]
        public string transactionAt { get; set; }

        [JsonPropertyName("currency")]
        public string currency { get; set; }

        [JsonPropertyName("amount")]
        public int amount { get; set; }
    }
}
