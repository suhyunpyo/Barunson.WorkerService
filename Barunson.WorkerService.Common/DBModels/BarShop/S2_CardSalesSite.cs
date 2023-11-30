using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 사이트별 카드 판매정보	
    /// </summary>
    public partial class S2_CardSalesSite
    {
        [Key]
        public int card_seq { get; set; }
        [Key]
        public int Company_Seq { get; set; }
        public int? CardDiscount_Seq { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string IsDisplay { get; set; }
        /// <summary>
        /// 판매여부(1:판매가능,0:판매불가,2:원주문결제/추가주문가능,3:원주문결제만 가능,4:추가주문만 가능
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsJumun { get; set; }
        /// <summary>
        /// 신상품 여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsNew { get; set; }
        /// <summary>
        /// 베스트 상품
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsBest { get; set; }
        /// <summary>
        /// 초특가제품
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsExtra { get; set; }
        /// <summary>
        /// 제휴카드 여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsJehyu { get; set; }
        public short? Ranking { get; set; }
        /// <summary>
        /// 주 랭킹
        /// </summary>
        public short? Ranking_w { get; set; }
        /// <summary>
        /// 월 랭킹
        /// </summary>
        public short? Ranking_m { get; set; }
        /// <summary>
        /// 등록일
        /// </summary>
        [StringLength(10)]
        [Unicode(false)]
        public string input_date { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string IsSale { get; set; }
        public short? SampRankNo { get; set; }
        public short? PostRankNo { get; set; }
        public short? ZzimRankNo { get; set; }
        /// <summary>
        /// 추천샘플 제품(고객 샘플 주문시 함께 발송카드)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string AppSample { get; set; }
        /// <summary>
        /// 쿠폰적용 불가 상품
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isNotCoupon { get; set; }
        /// <summary>
        /// 베스트 아이콘(임시)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsExtra2 { get; set; }
        public int? isRecommend { get; set; }
        public int? isSSPre { get; set; }
        /// <summary>
        /// 설명
        /// </summary>
        [StringLength(1000)]
        [Unicode(false)]
        public string isSummary { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string isBgcolor { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isDigitalCard { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Display_Date { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string IsInProduct { get; set; }
        [StringLength(1000)]
        [Unicode(false)]
        public string MovieURL { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string DisplayLabel { get; set; }
        [StringLength(1000)]
        [Unicode(false)]
        public string Tip { get; set; }
        public int? sealingsticker_seq { get; set; }
        public int? sealingsticker_groupseq { get; set; }
        public int? ribbon_seq { get; set; }
        public int? ribbon_groupseq { get; set; }
        public int? papercover_seq { get; set; }
        public int? papercover_groupseq { get; set; }
        public int? Flower_seq { get; set; }
        public int? Flower_GroupSeq { get; set; }
        public int? pocket_seq { get; set; }
        public int? pocket_groupseq { get; set; }
    }
}
