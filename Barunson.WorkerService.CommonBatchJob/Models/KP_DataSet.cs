namespace Barunson.WorkerService.CommonBatchJob.Models
{
    public class KP_DataSet
    {
        // 정상 전문
        public string Success { set; get; }

        // 오류 전문
        public string Fail { set; get; }

        public string Error { set; get; }

    }
}
