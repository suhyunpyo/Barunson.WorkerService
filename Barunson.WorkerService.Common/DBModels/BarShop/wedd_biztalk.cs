using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 비즈톡 템플릿
    /// </summary>
    public partial class wedd_biztalk
    {
        [Key]
        public int ID { get; set; }
        [StringLength(2)]
        [Unicode(false)]
        public string sales_gubun { get; set; }
        public int? company_seq { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string div { get; set; }
        [StringLength(4000)]
        [Unicode(false)]
        public string content { get; set; }
        public int? msg_type { get; set; }
        [StringLength(40)]
        [Unicode(false)]
        public string sender_key { get; set; }
        [StringLength(30)]
        [Unicode(false)]
        public string template_code { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string kko_btn_type { get; set; }
        [StringLength(4000)]
        [Unicode(false)]
        public string kko_btn_info { get; set; }
        [StringLength(200)]
        [Unicode(false)]
        public string lms_subject { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string template_name { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string USE_YORN { get; set; }
        [StringLength(15)]
        [Unicode(false)]
        public string callback { get; set; } = null!;
    }
}
