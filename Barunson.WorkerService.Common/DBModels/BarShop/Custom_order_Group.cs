using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 더카드전용 카드주문 그룹
    /// </summary>
    public partial class Custom_order_Group
    {
        [Key]
        public int order_g_seq { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string site_gubun { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string pay_Type { get; set; }
        public int? company_seq { get; set; }
        public int? status_seq { get; set; }
        public int? settle_status { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime order_date { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string src_cancel_admin_id { get; set; }
        [StringLength(16)]
        [Unicode(false)]
        public string member_id { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string order_name { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string order_email { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string order_phone { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string order_hphone { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string order_faxphone { get; set; }
        [StringLength(1000)]
        [Unicode(false)]
        public string order_etc_comment { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isReceipt { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string etc_price_ment { get; set; }
        public int? order_price { get; set; }
        public int? order_total_price { get; set; }
        public int? delivery_price { get; set; }
        public int? etc_price { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? settle_date { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? settle_cancel_date { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string settle_method { get; set; }
        public int? settle_price { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string pg_shopid { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string pg_tid { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string pg_receipt_tid { get; set; }
        [StringLength(1000)]
        [Unicode(false)]
        public string pg_resultinfo { get; set; }
        [StringLength(1000)]
        [Unicode(false)]
        public string pg_resultinfo2 { get; set; }
        public int? pg_status { get; set; }
        public double? pg_fee { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? pg_paydate { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? pg_repaydate { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string pg_caldate { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string pg_recaldate { get; set; }
        public int? point_price { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string delivery_name { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string temp_key { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string dacom_tid { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isAscrow { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? src_ap_date { get; set; }
    }
}
