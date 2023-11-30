using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 샘플주문 상세정보
    /// </summary>
    [Index("SAMPLE_ORDER_SEQ", Name = "NCI_CUSTOM_SAMSPLE_ORDER_SEQ")]
    public partial class CUSTOM_SAMPLE_ORDER_ITEM
    {
        [Key]
        public int CARD_SEQ { get; set; }
        [Key]
        public int SAMPLE_ORDER_SEQ { get; set; }
        /// <summary>
        /// 샘플 판매가
        /// </summary>
        public int CARD_PRICE { get; set; }
        /// <summary>
        /// 주문일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? REG_DATE { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isChu { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string md_recommend { get; set; }
        public int? sort { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string price_info { get; set; }

        [ForeignKey("SAMPLE_ORDER_SEQ")]
        [InverseProperty("CUSTOM_SAMPLE_ORDER_ITEM")]
        public virtual CUSTOM_SAMPLE_ORDER SAMPLE_ORDER_SEQNavigation { get; set; } = null!;
    }
}
