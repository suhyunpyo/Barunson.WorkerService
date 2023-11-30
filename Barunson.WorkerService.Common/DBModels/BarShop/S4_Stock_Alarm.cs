using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 품절카드 입고 문자 관리
    /// </summary>
    public partial class S4_Stock_Alarm
    {
        [Key]
        public int seq { get; set; }
        public int company_seq { get; set; }
        public int card_seq { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string uid { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string uname { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string umail { get; set; }
        [StringLength(3)]
        [Unicode(false)]
        public string hand_phone1 { get; set; } = null!;
        [StringLength(4)]
        [Unicode(false)]
        public string hand_phone2 { get; set; } = null!;
        [StringLength(4)]
        [Unicode(false)]
        public string hand_phone3 { get; set; } = null!;
        /// <summary>
        /// 발송 여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isAlarm_send { get; set; } = null!;
        /// <summary>
        /// 발송일
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? send_date { get; set; }
        /// <summary>
        /// 등록일
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime reg_date { get; set; }
    }
}
