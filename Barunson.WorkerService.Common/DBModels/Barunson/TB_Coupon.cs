using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    /// <summary>
    /// 쿠폰
    /// </summary>
    public partial class TB_Coupon
    {

        /// <summary>
        /// 쿠폰_ID
        /// </summary>
        [Key]
        public int Coupon_ID { get; set; }
        /// <summary>
        /// 쿠폰_명
        /// </summary>
        [StringLength(100)]
        public string Coupon_Name { get; set; } = null!;
        /// <summary>
        /// 발급_방식_코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Publish_Method_Code { get; set; } = null!;
        /// <summary>
        /// 발급_대상_코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Publish_Target_Code { get; set; } = null!;
        /// <summary>
        /// 사용_가능_기준_코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Use_Available_Standard_Code { get; set; } = null!;
        /// <summary>
        /// 기준_구매_금액
        /// </summary>
        public int? Standard_Purchase_Price { get; set; }
        /// <summary>
        /// 할인_방식_코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Discount_Method_Code { get; set; }
        /// <summary>
        /// 할인_율
        /// </summary>
        public double? Discount_Rate { get; set; }
        /// <summary>
        /// 할인_금액
        /// </summary>
        public int? Discount_Price { get; set; }
        /// <summary>
        /// 기간_방식_코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Period_Method_Code { get; set; }
        /// <summary>
        /// 발행_시작_일자
        /// </summary>
        [StringLength(10)]
        [Unicode(false)]
        public string Publish_Start_Date { get; set; }
        /// <summary>
        /// 발행_종료_일자
        /// </summary>
        [StringLength(10)]
        [Unicode(false)]
        public string Publish_End_Date { get; set; }
        /// <summary>
        /// 발행_기간_코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Publish_Period_Code { get; set; }
        /// <summary>
        /// 설명
        /// </summary>
        [StringLength(1000)]
        [Unicode(false)]
        public string Description { get; set; }
        /// <summary>
        /// 쿠폰_이미지_URL
        /// </summary>
        [StringLength(1000)]
        [Unicode(false)]
        public string Coupon_Image_URL { get; set; }
        /// <summary>
        /// 사용_여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string Use_YN { get; set; }
        /// <summary>
        /// 등록_일시
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Regist_DateTime { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Regist_User_ID { get; set; }
        [StringLength(15)]
        [Unicode(false)]
        public string Regist_IP { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Update_User_ID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Update_DateTime { get; set; }
        [StringLength(15)]
        [Unicode(false)]
        public string Update_IP { get; set; }
        /// <summary>
        /// 쿠폰_적용_코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Coupon_Apply_Code { get; set; }
        /// <summary>
        /// 쿠폰_적용_상품_ID
        /// </summary>
        public int? Coupon_Apply_Product_ID { get; set; }

    }
}
