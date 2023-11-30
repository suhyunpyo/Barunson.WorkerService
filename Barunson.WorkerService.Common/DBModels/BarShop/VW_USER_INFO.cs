using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    [Keyless]
    public partial class VW_USER_INFO
    {
        [StringLength(50)]
        [Unicode(false)]
        public string uid { get; set; } = null!;
        [StringLength(200)]
        [Unicode(false)]
        public string pwd { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string uname { get; set; } = null!;
        [StringLength(100)]
        [Unicode(false)]
        public string umail { get; set; } = null!;
        [StringLength(10)]
        [Unicode(false)]
        public string BIRTH_DATE { get; set; } = null!;
        [StringLength(1)]
        [Unicode(false)]
        public string BIRTH_DATE_TYPE { get; set; } = null!;
        [StringLength(64)]
        [Unicode(false)]
        public string DupInfo { get; set; }
        [StringLength(88)]
        [Unicode(false)]
        public string ConnInfo { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string AuthType { get; set; }
        [StringLength(8)]
        [Unicode(false)]
        public string ORIGINAL_BIRTH_DATE { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string Gender { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string NATIONAL_INFO { get; set; }
        [StringLength(4)]
        [Unicode(false)]
        public string WEDD_YEAR { get; set; } = null!;
        [StringLength(3)]
        [Unicode(false)]
        public string WEDD_MONTH { get; set; }
        [StringLength(3)]
        [Unicode(false)]
        public string WEDD_DAY { get; set; }
        [StringLength(12)]
        [Unicode(false)]
        public string WEDDING_DAY { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string WEDDING_HALL { get; set; }
        [StringLength(2)]
        [Unicode(false)]
        public string site_div { get; set; } = null!;
        [StringLength(15)]
        [Unicode(false)]
        public string SITE_DIV_NAME { get; set; } = null!;
        [StringLength(1)]
        [Unicode(false)]
        public string chk_sms { get; set; } = null!;
        [StringLength(1)]
        [Unicode(false)]
        public string chk_mailservice { get; set; } = null!;
        [StringLength(14)]
        [Unicode(false)]
        public string HPHONE { get; set; }
        [StringLength(14)]
        [Unicode(false)]
        public string PHONE { get; set; }
        [StringLength(8000)]
        [Unicode(false)]
        public string ZIPCODE { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isJehu { get; set; }
        [StringLength(3)]
        [Unicode(false)]
        public string zip1 { get; set; } = null!;
        [StringLength(3)]
        [Unicode(false)]
        public string zip2 { get; set; } = null!;
        [StringLength(150)]
        [Unicode(false)]
        public string address { get; set; } = null!;
        [StringLength(100)]
        [Unicode(false)]
        public string addr_detail { get; set; } = null!;
        [StringLength(1)]
        [Unicode(false)]
        public string mkt_chk_flag { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string CHOICE_AGREEMENT_FOR_SAMSUNG_MEMBERSHIP { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string CHOICE_AGREEMENT_FOR_SAMSUNG_CHOICE_PERSONAL_DATA { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string CHOICE_AGREEMENT_FOR_SAMSUNG_THIRDPARTY { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? smembership_reg_date { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string INTEGRATION_MEMBER_YORN { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? INTERGRATION_DATE { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string INTERGRATION_BEFORE_ID { get; set; }
        [StringLength(2)]
        [Unicode(false)]
        public string REFERER_SALES_GUBUN { get; set; }
        [StringLength(2)]
        [Unicode(false)]
        public string SELECT_SALES_GUBUN { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string SELECT_USER_ID { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string USE_YORN { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime reg_date { get; set; }
        public int? company_seq { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string CHK_MYOMEE { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? MYOMEE_REG_DATE { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isMCardAble { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string inflow_route { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string chk_iloommembership { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? iloommembership_reg_date { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string chk_lgmembership { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? lgmembership_reg_date { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string chk_cuckoosmembership { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? cuckoosship_reg_Date { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string chk_casamiamembership { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? casamiaship_reg_Date { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string chk_ktmembership { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ktmembership_reg_Date { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string chk_hyundaimembership { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? hyundaimembership_reg_Date { get; set; }
        [StringLength(200)]
        public string wedd_name { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string smembership_period { get; set; }
    }
}
