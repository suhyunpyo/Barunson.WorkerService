using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    /// <summary>
    /// 수수료
    /// </summary>
    public partial class TB_Tax
    {
        /// <summary>
        /// 수수료_ID
        /// </summary>
        [Key]
        public int Tax_ID { get; set; }
        /// <summary>
        /// 수수료
        /// </summary>
        public int Tax { get; set; }
        /// <summary>
        /// 이전_수수료
        /// </summary>
        public int? Previous_Tax { get; set; }
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
