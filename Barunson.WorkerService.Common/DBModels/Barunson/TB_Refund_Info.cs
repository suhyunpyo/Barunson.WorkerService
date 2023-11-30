using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    /// <summary>
    /// 환불_정보
    /// </summary>
    public partial class TB_Refund_Info
    {
        /// <summary>
        /// 환불_ID
        /// </summary>
        [Key]
        public int Refund_ID { get; set; }
        /// <summary>
        /// 주문_ID
        /// </summary>
        public int Order_ID { get; set; }
        /// <summary>
        /// 환불_유형_코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Refund_Type_Code { get; set; }
        /// <summary>
        /// 환불_금액
        /// </summary>
        public int? Refund_Price { get; set; }
        /// <summary>
        /// 은행_구분_코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Bank_Type_Code { get; set; }
        /// <summary>
        /// 계좌번호
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string AccountNumber { get; set; }
        /// <summary>
        /// 환불_상태_코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Refund_Status_Code { get; set; }
        /// <summary>
        /// 예금주_명
        /// </summary>
        [StringLength(50)]
        public string Depositor_Name { get; set; }
        /// <summary>
        /// 환불_내용
        /// </summary>
        [StringLength(1000)]
        [Unicode(false)]
        public string Refund_Content { get; set; }
        /// <summary>
        /// 등록_일시
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Regist_DateTime { get; set; }
        /// <summary>
        /// 환불_일시
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Refund_DateTime { get; set; }
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
