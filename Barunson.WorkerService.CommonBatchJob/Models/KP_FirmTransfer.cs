using System.Text.Json.Serialization;

namespace Barunson.WorkerService.CommonBatchJob.Models
{
    public class KP_FirmTransfer : KP_Firm
    {
        [JsonPropertyName("drw_account_cntn")]
        public string DrwAccountCntn { set; get; }
        [JsonPropertyName("drw_bank_code")]
        public string DrwBankCode { set; get; }
        [JsonPropertyName("drw_account")]
        public string DrwAccount { set; get; }
        [JsonPropertyName("rv_bank_code")]
        public string RvBankCode { set; get; }
        [JsonPropertyName("rv_account")]
        public string RvAccount { set; get; }
        [JsonPropertyName("rv_account_cntn")]
        public string RvAccountCntn { set; get; }
        [JsonPropertyName("amount")]
        public int Amount { set; get; }
    }
}
