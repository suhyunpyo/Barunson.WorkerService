using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barunson.WorkerService.CommonBatchJob.Models
{
    public class DbCalculateData
    {
        public int RemitID { get; set; }
        public int AccountID { get; set; }
        public int? TotalPrice { get; set; }
        public string RemitterName { get; set; }
        public int Tax { get; set; }
        public string KakaoBankCode { get; set; }
        public string KakaoAccountNumber { get; set; }
        public string BankCode { get; set; }
        public string AccountNumber { get; set; }
        public string DepositorName { get; set; }
        public string TransactionId { get; set; }

    }

    public class DbFeeCalculateData
    {
        public int Tax { get; set; }
        public string KakaoBankCode { get; set; }
        public string KakaoAccountNumber { get; set; }
        public string BankCode { get; set; }
        public string AccountNumber { get; set; }

    }

}
