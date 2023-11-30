using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    /// <summary>
    /// 송금_통계
    /// </summary>
    public partial class TB_Remit_Statistics_Daily
    {
        /// <summary>
        /// 날짜
        /// </summary>
        [Key]
        [StringLength(8)]
        [Unicode(false)]
        public string Date { get; set; } = null!;
        /// <summary>
        /// 송금_금액
        /// </summary>
        public int? Remit_Price { get; set; }
        /// <summary>
        /// 수수료
        /// </summary>
        public int? Tax { get; set; }
        public int? Remit_Tax { get; set; }
        /// <summary>
        /// 업체_수수료
        /// </summary>
        public int? Calculate_Tax { get; set; }
        /// <summary>
        /// 조회_수수료
        /// </summary>
        public int? Hits_Tax { get; set; }
        /// <summary>
        /// 사용자_수
        /// </summary>
        public int? User_Count { get; set; }
        /// <summary>
        /// 계좌_수
        /// </summary>
        public int? Account_Count { get; set; }
        /// <summary>
        /// 송금_수
        /// </summary>
        public int? Remit_Count { get; set; }
    }

    /// <summary>
    /// 송금_통계
    /// </summary>
    public partial class TB_Remit_Statistics_Monthly
    {
        /// <summary>
        /// 날짜
        /// </summary>
        [Key]
        [StringLength(8)]
        [Unicode(false)]
        public string Date { get; set; } = null!;
        /// <summary>
        /// 송금_금액
        /// </summary>
        public int? Remit_Price { get; set; }
        /// <summary>
        /// 수수료
        /// </summary>
        public int? Tax { get; set; }
        public int? Remit_Tax { get; set; }
        /// <summary>
        /// 업체_수수료
        /// </summary>
        public int? Calculate_Tax { get; set; }
        /// <summary>
        /// 조회_수수료
        /// </summary>
        public int? Hits_Tax { get; set; }
        /// <summary>
        /// 사용자_수
        /// </summary>
        public int? User_Count { get; set; }
        /// <summary>
        /// 계좌_수
        /// </summary>
        public int? Account_Count { get; set; }
        /// <summary>
        /// 송금_수
        /// </summary>
        public int? Remit_Count { get; set; }
    }
}
