using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Barunson.WorkerService.CommonBatchJob.Models
{
    internal class TossPayment
    {
        [JsonPropertyName("mId")]
        public string mId { get; set; }

        [JsonPropertyName("lastTransactionKey")]
        public string lastTransactionKey { get; set; }

        [JsonPropertyName("paymentKey")]
        public string paymentKey { get; set; }

        [JsonPropertyName("orderId")]
        public string orderId { get; set; }

        [JsonPropertyName("orderName")]
        public string orderName { get; set; }

        [JsonPropertyName("taxExemptionAmount")]
        public int taxExemptionAmount { get; set; }

        [JsonPropertyName("status")]
        public string status { get; set; }

        [JsonPropertyName("requestedAt")]
        public string requestedAt { get; set; }

        [JsonPropertyName("approvedAt")]
        public string approvedAt { get; set; }

        [JsonPropertyName("useEscrow")]
        public Boolean useEscrow { get; set; }

        [JsonPropertyName("cultureExpense")]
        public Boolean cultureExpense { get; set; }

        [JsonPropertyName("card")]
        public Card card { get; set; }

        [JsonPropertyName("virtualAccount")]
        public VirtualAccount virtualAccount { get; set; }

        [JsonPropertyName("transfer")]
        public Transfer transfer { get; set; }

        [JsonPropertyName("mobilePhone")]
        public MobilePhone mobilePhone { get; set; }

        [JsonPropertyName("giftCertificate")]
        public GiftCertificate giftCertificate { get; set; }

        [JsonPropertyName("cashReceipt")]
        public CashReceipt cashReceipt { get; set; }

        [JsonPropertyName("cashReceipts")]
        public List<CashReceipts> cashReceipts { get; set; }

        [JsonPropertyName("discount")]
        public Discount discount { get; set; }

        [JsonPropertyName("cancels")]
        public List<Cancels> cancels { get; set; }

        [JsonPropertyName("secret")]
        public string secret { get; set; }

        [JsonPropertyName("type")]
        public string type { get; set; }

        [JsonPropertyName("easyPay")]
        public EasyPay easyPay { get; set; }

        [JsonPropertyName("country")]
        public string country { get; set; }

        [JsonPropertyName("failure")]
        public string failure { get; set; }

        [JsonPropertyName("isPartialCancelable")]
        public Boolean isPartialCancelable { get; set; }

        [JsonPropertyName("receipt")]
        public Url receipt { get; set; }

        [JsonPropertyName("checkout")]
        public Url checkout { get; set; }

        [JsonPropertyName("currency")]
        public string currency { get; set; }

        [JsonPropertyName("totalAmount")]
        public int totalAmount { get; set; }

        [JsonPropertyName("balanceAmount")]
        public int balanceAmount { get; set; }

        [JsonPropertyName("suppliedAmount")]
        public int suppliedAmount { get; set; }

        [JsonPropertyName("vat")]
        public int vat { get; set; }

        [JsonPropertyName("taxFreeAmount")]
        public int taxFreeAmount { get; set; }

        [JsonPropertyName("method")]
        public string method { get; set; }

        [JsonPropertyName("version")]
        public string version { get; set; }

        public string jsonStr { get; set; }

        public string jsonStrBeauty { get; set; }
    }

    public class Card
    {
        [JsonPropertyName("issuerCode")]
        public string issuerCode { get; set; }

        public string issuerCodeName { get; set; }

        [JsonPropertyName("acquirerCode")]
        public string acquirerCode { get; set; }

        [JsonPropertyName("number")]
        public string number { get; set; }

        [JsonPropertyName("installmentPlanMonths")]
        public int installmentPlanMonths { get; set; }

        [JsonPropertyName("isInterestFree")]
        public Boolean isInterestFree { get; set; }

        [JsonPropertyName("interestPayer")]
        public string interestPayer { get; set; }

        [JsonPropertyName("approveNo")]
        public string approveNo { get; set; }

        [JsonPropertyName("useCardPoint")]
        public Boolean useCardPoint { get; set; }

        [JsonPropertyName("cardType")]
        public string cardType { get; set; }

        [JsonPropertyName("ownerType")]
        public string ownerType { get; set; }

        [JsonPropertyName("acquireStatus")]
        public string acquireStatus { get; set; }

        [JsonPropertyName("amount")]
        public int amount { get; set; }
    }

    public class Url
    {
        [JsonPropertyName("url")]
        public string url { get; set; }
    }

    public class EasyPay
    {
        [JsonPropertyName("provider")]
        public string provider { get; set; }

        [JsonPropertyName("amount")]
        public int amount { get; set; }

        [JsonPropertyName("discountAmount")]
        public int discountAmount { get; set; }
    }

    public class CashReceipt
    {
        [JsonPropertyName("type")]
        public string type { get; set; }

        [JsonPropertyName("receiptKey")]
        public string receiptKey { get; set; }

        [JsonPropertyName("issueNumber")]
        public string issueNumber { get; set; }

        [JsonPropertyName("receiptUrl")]
        public string receiptUrl { get; set; }

        [JsonPropertyName("amount")]
        public int amount { get; set; }

        [JsonPropertyName("taxFreeAmount")]
        public int taxFreeAmount { get; set; }
    }

    public class CashReceipts
    {
        [JsonPropertyName("receiptKey")]
        public string receiptKey { get; set; }

        [JsonPropertyName("orderId")]
        public string orderId { get; set; }

        [JsonPropertyName("orderName")]
        public string orderName { get; set; }

        [JsonPropertyName("type")]
        public string type { get; set; }

        [JsonPropertyName("issueNumber")]
        public string issueNumber { get; set; }

        [JsonPropertyName("receiptUrl")]
        public string receiptUrl { get; set; }

        [JsonPropertyName("businessNumber")]
        public string businessNumber { get; set; }

        [JsonPropertyName("transactionType")]
        public string transactionType { get; set; }

        [JsonPropertyName("amount")]
        public int amount { get; set; }

        [JsonPropertyName("taxFreeAmount")]
        public int taxFreeAmount { get; set; }

        [JsonPropertyName("issueStatus")]
        public string issueStatus { get; set; }

        [JsonPropertyName("failure")]
        public Failure failure { get; set; }

        [JsonPropertyName("customerIdentityNumber")]
        public string customerIdentityNumber { get; set; }

        [JsonPropertyName("requestedAt")]
        public string requestedAt { get; set; }
    }

    public class Failure
    {
        [JsonPropertyName("code")]
        public string code { get; set; }

        [JsonPropertyName("message")]
        public string message { get; set; }
    }

    public class VirtualAccount
    {
        [JsonPropertyName("accountType")]
        public string accountType { get; set; }

        [JsonPropertyName("accountNumber")]
        public string accountNumber { get; set; }

        [JsonPropertyName("bankCode")]
        public string bankCode { get; set; }

        [JsonPropertyName("customerName")]
        public string customerName { get; set; }

        [JsonPropertyName("dueDate")]
        public string dueDate { get; set; }

        [JsonPropertyName("refundStatus")]
        public string refundStatus { get; set; }

        [JsonPropertyName("expired")]
        public string expired { get; set; }

        [JsonPropertyName("settlementStatus")]
        public string settlementStatus { get; set; }

        [JsonPropertyName("refundReceiveAccount")]
        public string refundReceiveAccount { get; set; }
    }

    public class MobilePhone
    {
        [JsonPropertyName("customerMobilePhone")]
        public string customerMobilePhone { get; set; }

        [JsonPropertyName("settlementStatus")]
        public string settlementStatus { get; set; }

        [JsonPropertyName("receiptUrl")]
        public string receiptUrl { get; set; }
    }

    public class GiftCertificate
    {
        [JsonPropertyName("approveNo")]
        public string approveNo { get; set; }

        [JsonPropertyName("settlementStatus")]
        public string settlementStatus { get; set; }
    }

    public class Transfer
    {
        [JsonPropertyName("bankCode")]
        public string bankCode { get; set; }

        [JsonPropertyName("settlementStatus")]
        public string settlementStatus { get; set; }
    }

    public class Cancels
    {
        [JsonPropertyName("cancelAmount")]
        public int cancelAmount { get; set; }

        [JsonPropertyName("cancelReason")]
        public string cancelReason { get; set; }

        [JsonPropertyName("taxFreeAmount")]
        public int taxFreeAmount { get; set; }

        [JsonPropertyName("taxExemptionAmount")]
        public int taxExemptionAmount { get; set; }

        [JsonPropertyName("refundableAmount")]
        public int refundableAmount { get; set; }

        [JsonPropertyName("easyPayDiscountAmount")]
        public int easyPayDiscountAmount { get; set; }

        [JsonPropertyName("canceledAt")]
        public string canceledAt { get; set; }

        [JsonPropertyName("transactionKey")]
        public string transactionKey { get; set; }

        [JsonPropertyName("receiptKey")]
        public string receiptKey { get; set; }
    }

    public class Discount
    {
        [JsonPropertyName("amount")]
        public int amount { get; set; }
    }
}