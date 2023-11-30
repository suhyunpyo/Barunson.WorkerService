using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    /// <summary>
    /// 계좌_정보
    /// </summary>
    public partial class TB_Account
    {
        /// <summary>
        /// 모바일초대장에 매핑할 키
        /// 
        /// </summary>
        [Key]
        public int Account_ID { get; set; }
        public int Invitation_ID { get; set; }
        /// <summary>
        /// 사용자_ID
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string User_ID { get; set; }
        /// <summary>
        /// 신랑
        /// 신부
        /// 신랑혼주
        /// 신부혼주
        /// 
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Account_Type_Code { get; set; }
        /// <summary>
        /// 금융관리원에서 표준으로 잡는 은행 코드
        /// 
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
        /// 예금주_명
        /// </summary>
        [StringLength(100)]
        public string Depositor_Name { get; set; }
        public int? Sort { get; set; }
        /// <summary>
        /// 등록_일시
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Regist_DateTime { get; set; }

    }
}
