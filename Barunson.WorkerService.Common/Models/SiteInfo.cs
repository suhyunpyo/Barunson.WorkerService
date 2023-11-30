namespace Barunson.WorkerService.Common.Models
{
    public class SiteInfo
    {
        public string Brand { get; set; }
        public string CallBack { get; set; }
        //public string EventUrl { get; set; }
        public int CompaySeq { get; set; }
        public string Site { get; set; }
        public static Dictionary<string, SiteInfo> GetSiteInfos()
        {
            return new Dictionary<string, SiteInfo>
                {
                    {"SB", new SiteInfo { Brand = "바른손카드", CallBack = "1644-0708", CompaySeq = 5001, Site="www.barunsoncard.com" } },
                    {"SA", new SiteInfo { Brand = "비핸즈카드", CallBack = "1644-9713", CompaySeq = 5006 , Site=""} },
                    {"ST", new SiteInfo { Brand = "더카드", CallBack = "1644-7998",  CompaySeq = 5007, Site="" } },
                    {"B", new SiteInfo { Brand = "바른손몰", CallBack = "1644-7413", CompaySeq = 5000, Site="www.barunsonmall.com" } },
                    {"H", new SiteInfo { Brand = "바른손몰", CallBack = "1644-7413", CompaySeq = 5000, Site="www.barunsonmall.com"  } },
                    {"C", new SiteInfo { Brand = "바른손몰", CallBack = "1644-7413", CompaySeq = 5000, Site="www.barunsonmall.com"  } },
                    {"BM", new SiteInfo { Brand = "바른손M카드", CallBack = "1644-0708", CompaySeq = 8029, Site=""  } },
                    {"SS", new SiteInfo { Brand = "프리미어페이퍼", CallBack = "1644-8796", CompaySeq = 5003, Site="www.premierpaper.co.kr"} },
                    {"SD", new SiteInfo { Brand = "디얼디어", CallBack = "1661-2646", CompaySeq = 7717, Site="deardeer.kr"} },
                    {"GS", new SiteInfo { Brand = "바른손G샵", CallBack = "1644-0708", CompaySeq = 8030, Site=""} }

                };
        }
    }
}
