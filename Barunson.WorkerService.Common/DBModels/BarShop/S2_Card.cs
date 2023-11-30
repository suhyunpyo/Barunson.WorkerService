using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 카드정보
    /// </summary>
    public partial class S2_Card
    {
        [Key]
        public int Card_Seq { get; set; }
        /// <summary>
        /// B:바른손카드, N:비핸즈,W:위시메이드,S:프페 Z:기타
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string CardBrand { get; set; } = null!;
        [StringLength(30)]
        [Unicode(false)]
        public string Card_Code { get; set; } = null!;
        /// <summary>
        /// ERP연동코드
        /// </summary>
        [StringLength(30)]
        [Unicode(false)]
        public string Card_ERPCode { get; set; } = null!;
        /// <summary>
        /// A01:카드,A02:내지,A03:인사말카드,A04:약도카드 B01:봉투,B02:봉투라이닝 C01:신랑봉투,C02:신부봉투,C03:미니,C04:스티커,C05:사은품,C06:식권셋트
        /// </summary>
        [StringLength(3)]
        [Unicode(false)]
        public string Card_Div { get; set; } = null!;
        [StringLength(150)]
        [Unicode(false)]
        public string Card_Name { get; set; }
        /// <summary>
        /// 제품썸네일이미지(주문단에 사용)
        /// </summary>
        [StringLength(150)]
        [Unicode(false)]
        public string Card_Image { get; set; }
        /// <summary>
        /// 제품 셋트가(카드에만 적용)
        /// </summary>
        public int? CardSet_Price { get; set; }
        /// <summary>
        /// 단품가격 (추후 추가주문 등 단품 판매 기준가격)
        /// </summary>
        public int Card_Price { get; set; }
        /// <summary>
        /// 사용안함
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? ERP_EXPECTED_ARRIVAL_DATE { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string ERP_EXPECTED_ARRIVAL_DATE_USE_YORN { get; set; }
        public int? ERP_MIN_STOCK_QTY { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string ERP_MIN_STOCK_QTY_USE_YORN { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RegDate { get; set; }
        public int? Card_WSize { get; set; }
        /// <summary>
        /// 제품 세로 사이즈
        /// </summary>
        public int? Card_HSize { get; set; }
        [StringLength(30)]
        [Unicode(false)]
        public string Old_Code { get; set; }
        /// <summary>
        /// 봉투코드
        /// </summary>
        [StringLength(30)]
        [Unicode(false)]
        public string t_env_code { get; set; }
        [StringLength(30)]
        [Unicode(false)]
        public string t_inpaper_code { get; set; }
        /// <summary>
        /// 수정자
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string admin_id { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string new_code { get; set; }
        /// <summary>
        /// I:초대장, X:시즌카드
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string CARD_GROUP { get; set; }
        public int? CardFactory_Price { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? REGIST_DATES { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string DISPLAY_YORN { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DISPLAY_UPDATE_DATE { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string DISPLAY_UPDATE_UID { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string FPRINT_YORN { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string WisaFlag { get; set; }
        [Column(TypeName = "numeric(3, 0)")]
        public decimal? View_Discount_Percent { get; set; }
        public int? Cost_Price { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string Video_URL { get; set; }
        /// <summary>
        /// 구매팁
        /// </summary>
        [StringLength(2000)]
        public string Tip { get; set; }
        /// <summary>
        /// 내용
        /// </summary>
        public string Explain { get; set; }
        /// <summary>
        /// 단위
        /// </summary>
        [StringLength(10)]
        public string Unit { get; set; }
        /// <summary>
        /// 단위수량
        /// </summary>
        public int? Unit_Value { get; set; }
        /// <summary>
        /// 부가상품_옵션명
        /// </summary>
        [StringLength(50)]
        public string Option_Name { get; set; }
    }
}
