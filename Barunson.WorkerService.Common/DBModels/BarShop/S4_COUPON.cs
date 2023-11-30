using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    public partial class S4_COUPON
    {
        [Key]
        public int SEQ { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string coupon_code { get; set; }
        public int company_seq { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string uid { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime reg_date { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string discount_type { get; set; } = null!;
        public int discount_value { get; set; }
        public int? limit_price { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isYN { get; set; }
        [StringLength(200)]
        [Unicode(false)]
        public string coupon_desc { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isRecycle { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isWeddingCoupon { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isJehu { get; set; }
        public int? id { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? end_date { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string cardbrand { get; set; }
        [StringLength(2)]
        [Unicode(false)]
        public string item_type { get; set; }
        [StringLength(6)]
        [Unicode(false)]
        public string COUPON_TYPE_CODE { get; set; }
        public int? limit_order_count { get; set; }
        [StringLength(6)]
        [Unicode(false)]
        public string DEVICE_TYPE_CODE { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string dup_ind { get; set; }
    }
}
