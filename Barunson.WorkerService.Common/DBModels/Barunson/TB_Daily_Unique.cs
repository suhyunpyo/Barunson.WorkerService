using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    /// <summary>
    /// 날짜_고유_번호
    /// </summary>
    public partial class TB_Daily_Unique
    {
        /// <summary>
        /// 요청_일자
        /// </summary>
        [Key]
        [StringLength(8)]
        [Unicode(false)]
        public string Request_Date { get; set; } = null!;
        /// <summary>
        /// 고유_번호
        /// </summary>
        [Key]
        public int Unique_Number { get; set; }
    }
}
