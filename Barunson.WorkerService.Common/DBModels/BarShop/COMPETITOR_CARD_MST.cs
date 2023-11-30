using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 타사이트 카드정보
    /// </summary>
    public partial class COMPETITOR_CARD_MST
    {
        [Key]
        public int SEQ { get; set; }

        [StringLength(100)]
        [Unicode(false)]
        public string? SITE_NAME { get; set; }

        public int? CARD_SEQ { get; set; }

        [StringLength(100)]
        [Unicode(false)]
        public string? CARD_CODE { get; set; }

        [StringLength(100)]
        [Unicode(false)]
        public string? CARD_NAME { get; set; }

        [Column(TypeName = "numeric(18, 2)")]
        public decimal? CARD_PRICE { get; set; }

        [Column(TypeName = "numeric(18, 2)")]
        public decimal? DISCOUNT_RATE { get; set; }

        [StringLength(100)]
        [Unicode(false)]
        public string? CARD_IMAGE { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? REG_DATE { get; set; }
    }
}
