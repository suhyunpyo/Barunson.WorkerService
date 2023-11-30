using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    /// <summary>
    /// 초대장_수수료
    /// </summary>
    public partial class TB_Invitation_Tax
    {
        /// <summary>
        /// 초대장_ID
        /// </summary>
        [Key]
        public int Invitation_ID { get; set; }
        /// <summary>
        /// 수수료_ID
        /// </summary>
        public int? Tax_ID { get; set; }
        /// <summary>
        /// 등록_일시
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Regist_DateTime { get; set; }

    }
}
