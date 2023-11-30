using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 주문상세내역
    /// </summary>
    public partial class custom_order_item
    {
        [Key]
        public int id { get; set; }
        public int order_seq { get; set; }
        public int card_seq { get; set; }
        /// <summary>
        /// manage_code.itemt_type
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string item_type { get; set; } = null!;
        /// <summary>
        /// 주문수량
        /// </summary>
        public int? item_count { get; set; }
        /// <summary>
        /// 소비자가
        /// </summary>
        public int? item_price { get; set; }
        /// <summary>
        /// 판매가
        /// </summary>
        public double? item_sale_price { get; set; }
        /// <summary>
        /// 할인율
        /// </summary>
        public double? discount_rate { get; set; }
        /// <summary>
        /// 기타정보(미니청첩장의경우 인쇄색상등)
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string memo1 { get; set; }
        /// <summary>
        /// 추가수량비용(셋트 이외의 수량에 대한 합계비용)
        /// </summary>
        public int? addnum_price { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? REG_DATE { get; set; }
    }
}
