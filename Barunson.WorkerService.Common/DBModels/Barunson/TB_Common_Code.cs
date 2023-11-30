using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    /// <summary>
    /// 공통_코드
    /// </summary>
    public partial class TB_Common_Code
    {
        /// <summary>
        /// 코드_그룹
        /// </summary>
        [Key]
        [StringLength(100)]
        [Unicode(false)]
        public string Code_Group { get; set; } = null!;
        /// <summary>
        /// 코드
        /// </summary>
        [Key]
        [StringLength(50)]
        [Unicode(false)]
        public string Code { get; set; } = null!;
        /// <summary>
        /// 코드_명
        /// </summary>
        [StringLength(100)]
        public string Code_Name { get; set; }
        /// <summary>
        /// 순서
        /// </summary>
        public int? Sort { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Regist_User_ID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Regist_DateTime { get; set; }
        [StringLength(15)]
        [Unicode(false)]
        public string Regist_IP { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Update_User_ID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Update_DateTime { get; set; }
        [StringLength(15)]
        [Unicode(false)]
        public string Update_IP { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Extra_Code { get; set; }

    }
}
