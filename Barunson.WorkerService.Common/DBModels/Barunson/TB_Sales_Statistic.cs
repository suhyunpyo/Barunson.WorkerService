using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    /// <summary>
    /// 매출_통계_일별
    /// </summary>
    [Index("Date", Name = "IX_TB_Sales_Statistic_Day_Date", IsUnique = true)]
    public partial class TB_Sales_Statistic_Day
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
        /// 바른손_매출_금액
        /// </summary>
        public int? Barunn_Sales_Price { get; set; }
        /// <summary>
        /// 바른손_무료_주문_수
        /// </summary>
        public int? Barunn_Free_Order_Count { get; set; }
        /// <summary>
        /// 바른손_유료_주문_수
        /// </summary>
        public int? Barunn_Charge_Order_Count { get; set; }
        /// <summary>
        /// 비핸즈_매출_금액
        /// </summary>
        public int? Bhands_Sales_Price { get; set; }
        /// <summary>
        /// 비핸즈_무료_주문 _수
        /// </summary>
        public int? Bhands_Free_Order_Count { get; set; }
        /// <summary>
        /// 비핸즈_유료_주문 _수
        /// </summary>
        public int? Bhands_Charge_Order_Count { get; set; }
        /// <summary>
        /// 더카드_매출_금액
        /// </summary>
        public int? Thecard_Sales_Price { get; set; }
        /// <summary>
        /// 더카드_무료_주문_수 
        /// </summary>
        public int? Thecard_Free_Order_Count { get; set; }
        /// <summary>
        /// 더카드_유료_주문_수 
        /// </summary>
        public int? Thecard_Charge_Order_Count { get; set; }
        /// <summary>
        /// 프리미어_매출_금액 
        /// </summary>
        public int? Premier_Sales_Price { get; set; }
        /// <summary>
        /// 프리미어_무료_주문_수
        /// </summary>
        public int? Premier_Free_Order_Count { get; set; }
        /// <summary>
        /// 프리미어_유료_주문_수
        /// </summary>
        public int? Premier_Charge_Order_Count { get; set; }
        /// <summary>
        /// 합계_매출_금액
        /// </summary>
        public int? Total_Sales_Price { get; set; }
        /// <summary>
        /// 합계_무료_주문_수
        /// </summary>
        public int? Total_Free_Order_Count { get; set; }
        /// <summary>
        /// 합계_유료_주문_수
        /// </summary>
        public int? Total_Charge_Order_Count { get; set; }
    }

    /// <summary>
    /// 매출_통계_월별
    /// </summary>
    [Index("Date", Name = "IX_TB_Sales_Statistic_Month_Date", IsUnique = true)]
    public partial class TB_Sales_Statistic_Month
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
        /// 바른손_매출_금액
        /// </summary>
        public int? Barunn_Sales_Price { get; set; }
        /// <summary>
        /// 바른손_무료_주문_수
        /// </summary>
        public int? Barunn_Free_Order_Count { get; set; }
        /// <summary>
        /// 바른손_유료_주문_수
        /// </summary>
        public int? Barunn_Charge_Order_Count { get; set; }
        /// <summary>
        /// 비핸즈_매출_금액
        /// </summary>
        public int? Bhands_Sales_Price { get; set; }
        /// <summary>
        /// 비핸즈_무료_주문 _수
        /// </summary>
        public int? Bhands_Free_Order_Count { get; set; }
        /// <summary>
        /// 비핸즈_유료_주문 _수
        /// </summary>
        public int? Bhands_Charge_Order_Count { get; set; }
        /// <summary>
        /// 더카드_매출_금액
        /// </summary>
        public int? Thecard_Sales_Price { get; set; }
        /// <summary>
        /// 더카드_무료_주문_수 
        /// </summary>
        public int? Thecard_Free_Order_Count { get; set; }
        /// <summary>
        /// 더카드_유료_주문_수 
        /// </summary>
        public int? Thecard_Charge_Order_Count { get; set; }
        /// <summary>
        /// 프리미어_매출_금액 
        /// </summary>
        public int? Premier_Sales_Price { get; set; }
        /// <summary>
        /// 프리미어_무료_주문_수
        /// </summary>
        public int? Premier_Free_Order_Count { get; set; }
        /// <summary>
        /// 프리미어_유료_주문_수
        /// </summary>
        public int? Premier_Charge_Order_Count { get; set; }
        /// <summary>
        /// 합계_매출_금액
        /// </summary>
        public int? Total_Sales_Price { get; set; }
        /// <summary>
        /// 합계_무료_주문_수
        /// </summary>
        public int? Total_Free_Order_Count { get; set; }
        /// <summary>
        /// 합계_유료_주문_수
        /// </summary>
        public int? Total_Charge_Order_Count { get; set; }
    }
}
