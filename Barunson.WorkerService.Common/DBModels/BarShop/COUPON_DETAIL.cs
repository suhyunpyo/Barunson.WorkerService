using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 쿠폰코드정보
    /// </summary>
    public partial class COUPON_DETAIL
    {

        /// <summary>
        /// SEQ
        /// </summary>
        [Key]
        public int COUPON_DETAIL_SEQ { get; set; }
        /// <summary>
        /// COUPON_MST.COUPON_MST_SEQ
        /// </summary>
        public int COUPON_MST_SEQ { get; set; }
        /// <summary>
        /// 쿠폰코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string COUPON_CODE { get; set; } = null!;
        /// <summary>
        /// 다운로드가능여부(Y/N)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string DOWNLOAD_ACTIVE_YN { get; set; }

    }
}
