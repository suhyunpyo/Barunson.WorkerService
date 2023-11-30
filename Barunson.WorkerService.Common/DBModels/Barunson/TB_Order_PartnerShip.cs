using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    /// <summary>
    /// 파트너 연동 주문 정보
    /// </summary>
    [Index("P_OrderDate", "P_Id", Name = "IX_TB_Order_PartnerShip_OrderDatae")]
    [Index("Order_ID", Name = "IX_TB_Order_PartnerShip_Order_ID")]
    [Index("LastUpdateTime", "P_Id", Name = "IX_TB_Order_PartnerShip_UpdateTime")]
    public partial class TB_Order_PartnerShip
    {
        /// <summary>
        /// 파트너사 주문번호
        /// </summary>
        [Key]
        [StringLength(30)]
        public string P_OrderCode { get; set; } = null!;
        /// <summary>
        /// 파트너사 고유ID
        /// </summary>
        [Key]
        [StringLength(10)]
        public string P_Id { get; set; } = null!;
        /// <summary>
        /// 바른손 주문_ID
        /// </summary>
        public int Order_ID { get; set; }
        /// <summary>
        /// 주문일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime P_OrderDate { get; set; }
        
        public DateTime LastUpdateTime { get; set; }
        /// <summary>
        /// 주문 상태코드
        /// </summary>
        [StringLength(10)]
        public string P_OrderState { get; set; }
        /// <summary>
        /// 상품코드
        /// </summary>
        [StringLength(50)]
        public string P_ProductCode { get; set; }
        /// <summary>
        /// 상품명
        /// </summary>
        [StringLength(100)]
        public string P_ProductName { get; set; } = null!;
        /// <summary>
        /// 주문자명
        /// </summary>
        [StringLength(30)]
        public string P_Order_Name { get; set; } = null!;
        /// <summary>
        /// 주문자연락처
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string P_Order_Phone { get; set; }
        /// <summary>
        /// 결제_금액
        /// </summary>
        public int Payment_Price { get; set; }
        /// <summary>
        /// 결제_상태_코드
        /// </summary>
        [StringLength(10)]
        [Unicode(false)]
        public string Payment_Status_Code { get; set; } = null!;
        /// <summary>
        /// 결제_방법_코드
        /// </summary>
        [StringLength(10)]
        [Unicode(false)]
        public string Payment_Method_Code { get; set; } = null!;
        [Column(TypeName = "smalldatetime")]
        public DateTime? Payment_DateTime { get; set; }
        /// <summary>
        /// 최소 여부
        /// </summary>
        public bool Is_Refund { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? Refund_DateTime { get; set; }
        /// <summary>
        /// 파트너사 확장 데이터
        /// </summary>
        public string P_ExtendData { get; set; }
    }
}
