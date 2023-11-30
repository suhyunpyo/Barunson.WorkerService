using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    public partial class CASAMIA_DAILY_INFO
    {
        [Key]
        public int seq { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string ConnInfo { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string uid { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string uname { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string gender { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string birth_div { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string Birth_date { get; set; }
        [StringLength(14)]
        [Unicode(false)]
        public string phone { get; set; }
        [StringLength(14)]
        [Unicode(false)]
        public string hand_phone { get; set; }
        [StringLength(6)]
        [Unicode(false)]
        public string zipcode { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string address { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string addr_detail { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string umail { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string wedding_day { get; set; }
        [StringLength(4)]
        [Unicode(false)]
        public string barun_reg_site { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? barun_reg_Date { get; set; }
        [StringLength(4)]
        [Unicode(false)]
        public string casamia_rst_cd { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? casamia_send_date { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? create_date { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? mms_send_Date { get; set; }
    }
}
