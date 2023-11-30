using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// CS 처리 상세 (관리메모)
    /// </summary>
    public partial class CUSTOM_ORDER_ADMIN_MENT
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 1:청첩장관련,0:식권 또는 샘플
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string ISWOrder { get; set; }
        /// <summary>
        /// 메모
        /// </summary>
        [StringLength(2000)]
        [Unicode(false)]
        public string MENT { get; set; }
        public int ORDER_SEQ { get; set; }
        /// <summary>
        /// 유형(0:일반,1:포장지시,2:사고,3/5:취소)
        /// </summary>
        public byte? PCHECK { get; set; }
        /// <summary>
        /// 처리여부(0:등록,9:처리완료)
        /// </summary>
        public byte? STATUS { get; set; }
        /// <summary>
        /// 등록 어드민
        /// </summary>
        [StringLength(15)]
        [Unicode(false)]
        public string ADMIN_ID { get; set; }
        /// <summary>
        /// 등록일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? REG_DATE { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isJumun { get; set; }
        public byte? intype { get; set; }
        [StringLength(2)]
        [Unicode(false)]
        public string sgubun { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string stype { get; set; }
        [StringLength(12)]
        [Unicode(false)]
        public string Mtype { get; set; }
        [StringLength(12)]
        [Unicode(false)]
        public string Category { get; set; }
    }
}
