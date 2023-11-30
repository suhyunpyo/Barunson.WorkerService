using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Barunson.WorkerService.LMSBatchJob.Models
{
    public class OrderFailSite
    {
        public static List<OrderFailConfig> DefaultSites => new List<OrderFailConfig>
        {
            new OrderFailConfig {
                SiteName = "바른손카드",
                ShortName = "바",
                CheckMinute = new Dictionary<string, short>
                {
                    {"청첩장", 0 },{"샘플", 0 }
                },
                UnCheckTimeFrom = new TimeOnly(1,00),
                UnCheckTimeTo = new TimeOnly(6,00),
                SmsTargets = new List<OrderFailConfigSmsTarget>()
            },
            new OrderFailConfig {
                SiteName = "프리미어페이퍼",
                ShortName = "프",
                CheckMinute = new Dictionary<string, short>
                {
                    {"청첩장", 0 },{"샘플", 0 }
                },
                UnCheckTimeFrom = new TimeOnly(1,00),
                UnCheckTimeTo = new TimeOnly(6,00),
                SmsTargets = new List<OrderFailConfigSmsTarget>()
            },
            new OrderFailConfig {
                SiteName = "디얼디어",
                ShortName = "디",
                CheckMinute = new Dictionary<string, short>
                {
                    {"청첩장", 0 },{"샘플", 0 }
                },
                UnCheckTimeFrom = new TimeOnly(1,00),
                UnCheckTimeTo = new TimeOnly(6,00),
                SmsTargets = new List<OrderFailConfigSmsTarget>()
            },
            new OrderFailConfig {
                SiteName = "바른손몰",
                ShortName = "몰",
                CheckMinute = new Dictionary<string, short>
                {
                    {"청첩장", 0 },{"샘플", 0 }
                },
                UnCheckTimeFrom = new TimeOnly(1,00),
                UnCheckTimeTo = new TimeOnly(6,00),
                SmsTargets = new List<OrderFailConfigSmsTarget>()
            },
            new OrderFailConfig {
                SiteName = "모바일초대장",
                ShortName = "모초",
                CheckMinute = new Dictionary<string, short>
                {
                    {"모초", 0 },{"모초화환", 0 }
                },
                UnCheckTimeFrom = new TimeOnly(1,00),
                UnCheckTimeTo = new TimeOnly(6,00),
                SmsTargets = new List<OrderFailConfigSmsTarget>()
            }
        };
    }
    /// <summary>
    /// 주문 장애 모티터링 체크 설정 모델
    /// </summary>
    public class OrderFailConfig
    {
        public string SiteName { get; set; }
        public string ShortName { get; set; }
        /// <summary>
        /// 주문 타겟 및 최대 경과 분 (청첩장 주문등 메인 주문 및 샘플 주문, 화환 주문 등 기타 주문)
        /// </summary>
        public Dictionary<string, short> CheckMinute { get; set; }
        
        /// <summary>
        /// 예외 시간 
        /// </summary>
        public TimeOnly UnCheckTimeFrom { get; set; }
        /// <summary>
        ///  예외 시간 
        /// </summary>
        public TimeOnly UnCheckTimeTo { get; set; }

        /// <summary>
        /// 발송 대상
        /// </summary>
        public List<OrderFailConfigSmsTarget> SmsTargets { get; set; }

    }

    public class OrderFailConfigSmsTarget
    {
        public int Sort { get; set; }
        public string Dept { get; set; }
        public string Name { get; set; }
        public string PhoneNum { get; set; }
    }

    /// <summary>
    /// 주문 실패 로그 상세
    /// </summary>
    public class OrderFailLog
    {
        /// <summary>
        /// 사이트
        /// </summary>
        public string SiteName { get; set; }
        /// <summary>
        /// 주문종류
        /// </summary>
        public string OrderName { get; set; }

        /// <summary>
        /// 마지막 주문 시간
        /// </summary>
        public DateTime? LastOrderTime { get; set; }
    }
}
