using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 쿠폰발급리스트
    /// </summary>
    [Index("COUPON_DETAIL_SEQ", "ACTIVE_YN", Name = "NCI_COUPON_DETAIL_SEQ_ACTIVE_YN")]
    public partial class COUPON_ISSUE
    {
        /// <summary>
        /// SEQ
        /// </summary>
        [Key]
        public int COUPON_ISSUE_SEQ { get; set; }
        /// <summary>
        /// COUPON_DETAIL.COUPON_DETAIL_SEQ
        /// </summary>
        public int COUPON_DETAIL_SEQ { get; set; }
        /// <summary>
        /// 유저ID
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string UID { get; set; } = null!;
        /// <summary>
        /// 사용가능여부(Y/N)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string ACTIVE_YN { get; set; } = null!;
        /// <summary>
        /// COMPANY_SEQ
        /// </summary>
        public int COMPANY_SEQ { get; set; }
        /// <summary>
        /// 사이트구분
        /// </summary>
        [StringLength(10)]
        [Unicode(false)]
        public string SALES_GUBUN { get; set; } = null!;
        /// <summary>
        /// 종료일
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? END_DATE { get; set; }
        /// <summary>
        /// 등록일
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? REG_DATE { get; set; }

    }
}
