using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 더카드 회원테이블
    /// </summary>
    public partial class S2_UserInfo_TheCard
    {
        [Key]
        [StringLength(50)]
        [Unicode(false)]
        public string uid { get; set; } = null!;
        [StringLength(16)]
        [Unicode(false)]
        public string PWD_BACKUP { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string uname { get; set; } = null!;
        [StringLength(100)]
        [Unicode(false)]
        public string umail { get; set; } = null!;
        [Key]
        [StringLength(13)]
        [Unicode(false)]
        public string jumin { get; set; } = null!;
        [StringLength(10)]
        [Unicode(false)]
        public string birth { get; set; } = null!;
        [StringLength(1)]
        [Unicode(false)]
        public string birth_div { get; set; } = null!;
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
        [StringLength(4)]
        [Unicode(false)]
        public string phone1 { get; set; }
        [StringLength(4)]
        [Unicode(false)]
        public string phone2 { get; set; }
        [StringLength(4)]
        [Unicode(false)]
        public string phone3 { get; set; }
        [StringLength(4)]
        [Unicode(false)]
        public string hand_phone1 { get; set; }
        [StringLength(4)]
        [Unicode(false)]
        public string hand_phone2 { get; set; }
        [StringLength(4)]
        [Unicode(false)]
        public string hand_phone3 { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string chk_mail_input { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string chk_sms { get; set; } = null!;
        [StringLength(1)]
        [Unicode(false)]
        public string chk_mailservice { get; set; } = null!;
        [StringLength(2)]
        [Unicode(false)]
        public string site_div { get; set; } = null!;
        [StringLength(1)]
        [Unicode(false)]
        public string isJehu { get; set; }
        public int? company_seq { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? login_date { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? login_date_lastest { get; set; }
        public int login_count { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string is_appSample { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime reg_date { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string var1 { get; set; }
        [StringLength(2)]
        [Unicode(false)]
        public string site_div_lastest { get; set; }
        [StringLength(30)]
        [Unicode(false)]
        public string RequestNumber { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string AuthType { get; set; }
        [StringLength(64)]
        [Unicode(false)]
        public string DupInfo { get; set; }
        [StringLength(88)]
        [Unicode(false)]
        public string ConnInfo { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string Gender { get; set; }
        [StringLength(8)]
        [Unicode(false)]
        public string BirthDate { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string NationalInfo { get; set; }
        [StringLength(4)]
        [Unicode(false)]
        public string wedd_year { get; set; }
        [StringLength(2)]
        [Unicode(false)]
        public string wedd_month { get; set; }
        [StringLength(2)]
        [Unicode(false)]
        public string wedd_day { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isMCardAble { get; set; }
        [StringLength(2)]
        [Unicode(false)]
        public string ugubun { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string chk_DM { get; set; }
        [StringLength(2)]
        [Unicode(false)]
        public string wedd_hour { get; set; }
        [StringLength(2)]
        [Unicode(false)]
        public string wedd_minute { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string wedd_pgubun { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? mod_date { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string chk_smembership { get; set; }
        public int? addr_flag { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? smembership_reg_date { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? smembership_leave_date { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string chk_smembership_leave { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string chk_smembership_per { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string chk_smembership_coop { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string inflow_route { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string smembership_inflow_route { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string smembership_chk_flag { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string mkt_chk_flag { get; set; }
        [StringLength(3)]
        [Unicode(false)]
        public string zip1_R { get; set; }
        [StringLength(3)]
        [Unicode(false)]
        public string zip2_R { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string address_R { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string addr_detail_R { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string chk_DormancyAccount { get; set; }
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
        [StringLength(16)]
        [Unicode(false)]
        public string BACKUP_UID { get; set; }
        [StringLength(200)]
        [Unicode(false)]
        public string PWD { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string chk_myomee { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? myomee_reg_date { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? iloommembership_reg_date { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string chk_iloommembership { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string chk_lgmembership { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? lgmembership_reg_date { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? lgmembership_leave_date { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string chk_cuckoosmembership { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? cuckoosship_reg_Date { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? cuckoosship_leave_date { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string chk_casamiamembership { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? casamiaship_reg_Date { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? casamiaship_leave_date { get; set; }
        [StringLength(200)]
        public string wedd_name { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string smembership_period { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string chk_ktmembership { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ktmembership_reg_Date { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ktmembership_leave_date { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string chk_hyundaimembership { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? hyundaimembership_reg_Date { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? hyundaimembership_leave_date { get; set; }
    }
}
