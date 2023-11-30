using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    /// <summary>
    /// 예금주_조회
    /// </summary>
    [Index("Request_Date", Name = "NonClusteredIndex_TB_Depositor_Hits_RegistDate")]
    public partial class TB_Depositor_Hits
    {
        /// <summary>
        /// 예금주_조회_ID
        /// </summary>
        [Key]
        public int Depositor_Hits_ID { get; set; }
        /// <summary>
        /// 사용자_ID
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string User_ID { get; set; }
        /// <summary>
        /// 고유_번호
        /// </summary>
        public int? Unique_Number { get; set; }
        /// <summary>
        /// 은행_코드
        /// </summary>
        [StringLength(3)]
        [Unicode(false)]
        public string Bank_Code { get; set; }
        /// <summary>
        /// 계좌_번호
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Account_Number { get; set; }
        /// <summary>
        /// 예금주
        /// </summary>
        [StringLength(50)]
        public string Depositor { get; set; }
        [StringLength(50)]
        public string Hits_Depositor { get; set; }
        /// <summary>
        /// 거래_번호
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string Trading_Number { get; set; }
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
        /// <summary>
        /// 오류_메세지
        /// </summary>
        [StringLength(1000)]
        [Unicode(false)]
        public string Error_Message { get; set; }
        /// <summary>
        /// 요청_일자
        /// </summary>
        [StringLength(8)]
        [Unicode(false)]
        public string Request_Date { get; set; }
        /// <summary>
        /// 요청_일시
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Request_DateTime { get; set; }
        /// <summary>
        /// 요청_결과_일시
        /// </summary>
        [StringLength(14)]
        [Unicode(false)]
        public string Request_Result_DateTime { get; set; }
    }
}
