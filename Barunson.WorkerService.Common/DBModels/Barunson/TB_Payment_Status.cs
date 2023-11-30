using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    /// <summary>
    /// 결제_수단_일별
    /// </summary>
    [Index("Date", Name = "IX_TB_Payment_Status_Day_Date", IsUnique = true)]
    public partial class TB_Payment_Status_Day
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
        /// 카드_결제_금액
        /// </summary>
        public int? Card_Payment_Price { get; set; }
        /// <summary>
        /// 계좌_이체_금액
        /// </summary>
        public int? Account_Transfer_Price { get; set; }
        /// <summary>
        /// 가상_계좌_금액
        /// </summary>
        public int? Virtual_Account_Price { get; set; }
        /// <summary>
        /// 기타_금액
        /// </summary>
        public int? Etc_Price { get; set; }
        /// <summary>
        /// 합계_금액
        /// </summary>
        public int? Total_Price { get; set; }
        /// <summary>
        /// 취소_환불_금액
        /// </summary>
        public int? Cancel_Refund_Price { get; set; }
        /// <summary>
        /// 순매출_금액
        /// </summary>
        public int? Profit_Price { get; set; }
    }

    /// <summary>
    /// 결제_수단_월별
    /// </summary>
    [Index("Date", Name = "IX_TB_Payment_Status_Month_Date", IsUnique = true)]
    public partial class TB_Payment_Status_Month
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
        /// 카드_결제_금액
        /// </summary>
        public int? Card_Payment_Price { get; set; }
        /// <summary>
        /// 계좌_이체_금액
        /// </summary>
        public int? Account_Transfer_Price { get; set; }
        /// <summary>
        /// 가상_계좌_금액
        /// </summary>
        public int? Virtual_Account_Price { get; set; }
        /// <summary>
        /// 기타_금액
        /// </summary>
        public int? Etc_Price { get; set; }
        /// <summary>
        /// 합계_금액
        /// </summary>
        public int? Total_Price { get; set; }
        /// <summary>
        /// 취소_환불_금액
        /// </summary>
        public int? Cancel_Refund_Price { get; set; }
        /// <summary>
        /// 순매출_금액
        /// </summary>
        public int? Profit_Price { get; set; }
    }
}
