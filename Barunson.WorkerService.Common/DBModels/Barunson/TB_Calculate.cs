using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    public partial class TB_Calculate
    {
        /// <summary>
        /// 정산_ID
        /// </summary>
        [Key]
        public int Calculate_ID { get; set; }
        /// <summary>
        /// 송금_ID
        /// </summary>
        public int? Remit_ID { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Calculate_Type_Code { get; set; }
        /// <summary>
        /// 송금_금액
        /// </summary>
        public int? Remit_Price { get; set; }
        /// <summary>
        /// 송금_은행_코드
        /// </summary>
        [StringLength(3)]
        [Unicode(false)]
        public string Remit_Bank_Code { get; set; }
        /// <summary>
        /// 송금_계좌_번호
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Remit_Account_Number { get; set; }
        /// <summary>
        /// 송금_내용
        /// </summary>
        [StringLength(200)]
        [Unicode(false)]
        public string Remit_Content { get; set; }
        /// <summary>
        /// 거래_번호
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string Trading_Number { get; set; }
        /// <summary>
        /// 고유_번호
        /// </summary>
        public int? Unique_Number { get; set; }
        /// <summary>
        /// 요청_일시
        /// </summary>
        [StringLength(14)]
        [Unicode(false)]
        public string Request_DateTime { get; set; }
        /// <summary>
        /// 요청_일자
        /// </summary>
        [StringLength(8)]
        [Unicode(false)]
        public string Request_Date { get; set; }
        /// <summary>
        /// 상태_코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Status_Code { get; set; }
        /// <summary>
        /// 오류_코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Error_Code { get; set; }
        [StringLength(1000)]
        [Unicode(false)]
        public string Error_Message { get; set; }
        /// <summary>
        /// 정산_일시
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Calculate_DateTime { get; set; }

       
    }
}
