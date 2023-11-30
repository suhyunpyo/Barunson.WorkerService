using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 공통기준코드값
    /// </summary>
    public partial class manage_code
    {
        /// <summary>
        /// 코드타입
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string code_type { get; set; } = null!;
        /// <summary>
        /// 코드
        /// </summary>
        [StringLength(12)]
        [Unicode(false)]
        public string code { get; set; } = null!;
        /// <summary>
        /// 코드명
        /// </summary>
        [StringLength(100)]
        [Unicode(false)]
        public string code_value { get; set; } = null!;
        [StringLength(500)]
        [Unicode(false)]
        public string etc1 { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string etc2 { get; set; }
        public int? seq { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string etc3 { get; set; }
        /// <summary>
        /// 사용여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string use_yorn { get; set; }
        [Key]
        public int code_id { get; set; }
        /// <summary>
        /// 상위id
        /// </summary>
        public int? parent_id { get; set; }
    }
}
