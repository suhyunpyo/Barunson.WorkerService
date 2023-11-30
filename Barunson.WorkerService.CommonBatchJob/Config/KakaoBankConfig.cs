namespace Barunson.WorkerService.CommonBatchJob.Config
{
    public class KakaoBankConfig
    {
        public Uri BankingHost { get; set; }

        public string InquireDepositorUri { get; set; }
        public string TransferUri { get; set; }
        public string TransferCheckUri { get; set; }

        public string BankingApiKey { get; set; }
        public string OrgCode { get; set; }

        public Uri MainHost { get; set; }
        public string StatusUri { get; set; }

        public string MainApiKey { get; set; }
        public string MainCid { get; set; }

    }
}
