using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 샘플 주문 통계
    /// </summary>
    public partial class Custom_Sample_Order_Statistics
    {
        /// <summary>
        /// 샘플주문번호
        /// </summary>
        [Key]
        public int sample_order_seq { get; set; }

        /// <summary>
        /// 샘플주문 사이트, |으로 구분
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string? ActualSampleSites { get; set; }

        /// <summary>
        /// 실주문 사이트, |으로 구분
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string? ActualOrderSites { get; set; }

        /// <summary>
        /// 실주문 코드, |으로 구분
        /// </summary>
        [StringLength(1000)]
        [Unicode(false)]
        public string? ActualOrderSeqs { get; set; }
        
        /// <summary>
        /// 청첩장 주문 수량
        /// </summary>
        public int ActualOrderCount { get; set; }

        /// <summary>
        /// 청첩장 주문 카드, |으로 구분
        /// </summary>
        [StringLength(1000)]
        [Unicode(false)]
        public string? ActualOrderCardSeqs { get; set; }

        /// <summary>
        /// 주문일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? LatestOrderDate { get; set; }

        /// <summary>
        /// 결제일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? LatestSettleDate { get; set; }

        /// <summary>
        /// 배송일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? LatestSrcSendDate { get; set; }

        /// <summary>
        /// 레이저
        /// </summary>
        public bool HasLazer { get; set; }

        /// <summary>
        /// 디지털
        /// </summary>
        public bool HasDigital { get; set; }

        /// <summary>
        /// 형합
        /// </summary>
        public bool HasPressure { get; set; }

        /// <summary>
        /// 박
        /// </summary>
        public bool HasRolled { get; set; }
    }
}
