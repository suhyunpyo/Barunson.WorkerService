using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    /// <summary>
    /// 상품
    /// </summary>
    public partial class TB_Product
    {

        /// <summary>
        /// 상품_ID
        /// </summary>
        [Key]
        public int Product_ID { get; set; }
        /// <summary>
        /// 템플릿_ID
        /// </summary>
        public int? Template_ID { get; set; }
        /// <summary>
        /// 상품_코드
        /// </summary>
        [StringLength(6)]
        [Unicode(false)]
        public string Product_Code { get; set; } = null!;
        /// <summary>
        /// 청첩장
        /// 감사장
        /// 포토형
        /// 
        /// 답례품
        /// 
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Product_Category_Code { get; set; } = null!;
        /// <summary>
        /// 상품_브랜드_코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Product_Brand_Code { get; set; } = null!;
        /// <summary>
        /// 상품_명
        /// </summary>
        [StringLength(100)]
        public string Product_Name { get; set; } = null!;
        /// <summary>
        /// 상품_설명
        /// </summary>
        [StringLength(200)]
        [Unicode(false)]
        public string Product_Description { get; set; }
        /// <summary>
        /// 가격
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// 진열_여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string Display_YN { get; set; } = null!;
        /// <summary>
        /// 대표_이미지_URL
        /// </summary>
        [StringLength(1000)]
        [Unicode(false)]
        public string Main_Image_URL { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Regist_User_ID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Regist_DateTime { get; set; }
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
        [StringLength(6)]
        [Unicode(false)]
        public string Original_Product_Code { get; set; } = null!;
        [StringLength(1000)]
        [Unicode(false)]
        public string Preview_Image_URL { get; set; }
        [StringLength(1000)]
        [Unicode(false)]
        public string SetCard_URL { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string SetCard_Display_YN { get; set; } = null!;
        [StringLength(1000)]
        [Unicode(false)]
        public string SetCard_Mobile_URL { get; set; }

    }
}
