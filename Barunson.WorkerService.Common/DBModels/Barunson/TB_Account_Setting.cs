using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    /// <summary>
    /// 계좌_설정
    /// </summary>
    public partial class TB_Account_Setting
    {
        /// <summary>
        /// 계좌_설정_ID
        /// </summary>
        [Key]
        public int Account_Setting_ID { get; set; }
        /// <summary>
        /// 바른_은행_코드
        /// </summary>
        [StringLength(3)]
        [Unicode(false)]
        public string Barunn_Bank_Code { get; set; }
        /// <summary>
        /// 바른_계좌_번호
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Barunn_Account_Number { get; set; }
        /// <summary>
        /// 카카오_은행_코드
        /// </summary>
        [StringLength(3)]
        [Unicode(false)]
        public string Kakao_Bank_Code { get; set; }
        /// <summary>
        /// 카카오_계좌_번호
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Kakao_Account_Number { get; set; }
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
    }
}
