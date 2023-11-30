using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    /// <summary>
    /// 전체_현황_일별
    /// </summary>
    [Index("Date", Name = "IX_TB_Total_Statistic_Day_Date", IsUnique = true)]
    public partial class TB_Total_Statistic_Day
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 날짜
        /// </summary>
        [StringLength(8)]
        [Unicode(false)]
        public string Date { get; set; }
        /// <summary>
        /// 무료_주문_수
        /// </summary>
        public int? Free_Order_Count { get; set; }
        /// <summary>
        /// 유료_주문_수
        /// </summary>
        public int? Charge_Order_Count { get; set; }
        /// <summary>
        /// 취소_수
        /// </summary>
        public int? Cancel_Count { get; set; }
        /// <summary>
        /// 결제_금액
        /// </summary>
        public int? Payment_Price { get; set; }
        /// <summary>
        /// 취소_환불_금액
        /// </summary>
        public int? Cancel_Refund_Price { get; set; }
        /// <summary>
        /// 순매출_금액
        /// </summary>
        public int? Profit_Price { get; set; }
        /// <summary>
        /// 회원가입_수
        /// </summary>
        public int? Memberjoin_Count { get; set; }
    }

    /// <summary>
    /// 전체_현황_월별
    /// </summary>
    [Index("Date", Name = "IX_TB_Total_Statistic_Month_Date", IsUnique = true)]
    public partial class TB_Total_Statistic_Month
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 날짜
        /// </summary>
        [StringLength(6)]
        [Unicode(false)]
        public string Date { get; set; }
        /// <summary>
        /// 무료_주문_수
        /// </summary>
        public int? Free_Order_Count { get; set; }
        /// <summary>
        /// 유료_주문_수
        /// </summary>
        public int? Charge_Order_Count { get; set; }
        /// <summary>
        /// 취소_수
        /// </summary>
        public int? Cancel_Count { get; set; }
        /// <summary>
        /// 결제_금액
        /// </summary>
        public int? Payment_Price { get; set; }
        /// <summary>
        /// 취소_환불_금액
        /// </summary>
        public int? Cancel_Refund_Price { get; set; }
        /// <summary>
        /// 순매출_금액
        /// </summary>
        public int? Profit_Price { get; set; }
        /// <summary>
        /// 회원가입_수
        /// </summary>
        public int? Memberjoin_Count { get; set; }
    }
}
