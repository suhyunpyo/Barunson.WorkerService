using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 고객 쿠폰 발급 내역
    /// </summary>
    [Keyless]
    public partial class S4_MyCoupon
    {
        [StringLength(50)]
        [Unicode(false)]
        public string coupon_code { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string uid { get; set; }
        public int company_seq { get; set; }
        public int id { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isMyYN { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? end_date { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? reg_date { get; set; }
    }
}
