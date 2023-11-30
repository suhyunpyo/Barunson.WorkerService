using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    /// <summary>
    /// 업체_수수료
    /// </summary>
    public partial class TB_Company_Tax
    {
        /// <summary>
        /// 업체_수수료_ID
        /// </summary>
        [Key]
        public int Company_Tax_ID { get; set; }
        /// <summary>
        /// 수수료_비율
        /// </summary>
        public int? Remit_Tax { get; set; }
        public int? Calculate_Tax { get; set; }
        public int? Hits_Tax { get; set; }
        /// <summary>
        /// 적용_시작_날짜
        /// </summary>
        [StringLength(8)]
        [Unicode(false)]
        public string Apply_Start_Date { get; set; }
        /// <summary>
        /// 등록_일시
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Regist_DateTime { get; set; }
        /// <summary>
        /// 등록_사용자_ID
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Regist_User_ID { get; set; }
    }
}
