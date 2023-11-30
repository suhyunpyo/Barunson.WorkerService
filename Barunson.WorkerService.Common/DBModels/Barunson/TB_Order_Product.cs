using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    /// <summary>
    /// 주문_상품
    /// </summary>
    public partial class TB_Order_Product
    {
        /// <summary>
        /// 주문_ID
        /// </summary>
        [Key]
        public int Order_ID { get; set; }
        /// <summary>
        /// 상품_ID
        /// </summary>
        [Key]
        public int Product_ID { get; set; }
        /// <summary>
        /// 상품_구분_코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Product_Type_Code { get; set; }
        /// <summary>
        /// 아이템_가격
        /// </summary>
        public int? Item_Price { get; set; }
        /// <summary>
        /// 아이템_수량
        /// </summary>
        public int? Item_Count { get; set; }
        /// <summary>
        /// 전체_가격
        /// </summary>
        public int? Total_Price { get; set; }
        /// <summary>
        /// 등록_일시
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Regist_DateTime { get; set; }
        /// <summary>
        /// 등록_사용자_ID
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Regist_User_ID { get; set; }
        /// <summary>
        /// 등록_IP
        /// </summary>
        [StringLength(15)]
        [Unicode(false)]
        public string Regist_IP { get; set; }
        /// <summary>
        /// 수정_사용자_ID
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Update_User_ID { get; set; }
        /// <summary>
        /// 수정_일시
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Update_DateTime { get; set; }
        /// <summary>
        /// 수정_IP
        /// </summary>
        [StringLength(15)]
        [Unicode(false)]
        public string Update_IP { get; set; }

    }
}
