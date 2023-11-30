using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    /// <summary>
    /// 주문
    /// </summary>
    [Index("Email", Name = "IX_TB_Order_Email")]
    [Index("Order_DateTime", Name = "IX_TB_Order_Order_DateTime")]
    [Index("Payment_DateTime", Name = "IX_TB_Order_Payment_DateTime")]
    [Index("Regist_DateTime", Name = "IX_TB_Order_REegist_Date")]
    [Index("User_ID", Name = "IX_TB_Order_User_ID")]
    [Index("Order_Code", Name = "NCIDX_Order_Code", IsUnique = true)]
    public partial class TB_Order
    {
        public TB_Order()
        {
        }

        /// <summary>
        /// 주문_ID
        /// </summary>
        [Key]
        public int Order_ID { get; set; }
        /// <summary>
        /// 이전_주문_ID
        /// </summary>
        public int? Previous_Order_ID { get; set; }
        /// <summary>
        /// 주문_코드
        /// </summary>
        [StringLength(25)]
        [Unicode(false)]
        public string Order_Code { get; set; } = null!;
        /// <summary>
        /// 사용자_ID
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string User_ID { get; set; } = null!;
        /// <summary>
        /// 이름
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; } = null!;
        /// <summary>
        /// 휴대전화_번호
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string CellPhone_Number { get; set; } = null!;
        /// <summary>
        /// 이메일
        /// </summary>
        [StringLength(100)]
        [Unicode(false)]
        public string Email { get; set; } = null!;
        /// <summary>
        /// 주문_금액
        /// </summary>
        public int Order_Price { get; set; }
        /// <summary>
        /// 결제_방법_코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Payment_Method_Code { get; set; }
        /// <summary>
        /// PG_ID
        /// </summary>
        [StringLength(500)]
        [Unicode(false)]
        public string PG_ID { get; set; }
        /// <summary>
        /// 쿠폰_금액
        /// </summary>
        public int? Coupon_Price { get; set; }
        /// <summary>
        /// 결제_금액
        /// </summary>
        public int? Payment_Price { get; set; }
        /// <summary>
        /// 결제_상태_코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Payment_Status_Code { get; set; }
        /// <summary>
        /// 주문_상태_코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Order_Status_Code { get; set; }
        /// <summary>
        /// 등록_사용자_ID
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Regist_User_ID { get; set; }
        /// <summary>
        /// 등록_일시
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Regist_DateTime { get; set; }
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
        [StringLength(50)]
        [Unicode(false)]
        public string Card_Installment { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string CashReceipt_Publish_YN { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string Noint_YN { get; set; }
        [StringLength(100)]
        public string Finance_Auth_Number { get; set; }
        [StringLength(100)]
        public string Finance_Name { get; set; }
        [StringLength(50)]
        public string Payer_Name { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string Escrow_YN { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Account_Number { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string Trading_Number { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Order_DateTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Cancel_DateTime { get; set; }
        [StringLength(2)]
        [Unicode(false)]
        public string Cancel_Time { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Deposit_DeadLine_DateTime { get; set; }
        [StringLength(50)]
        public string Order_Path { get; set; }
        [StringLength(50)]
        public string Payment_Path { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Payment_DateTime { get; set; }
        
    }
}
