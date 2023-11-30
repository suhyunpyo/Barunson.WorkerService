using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    [Keyless]
    public partial class S2_CardView
    {
        [StringLength(1)]
        [Unicode(false)]
        public string isS2 { get; set; } = null!;
        public int card_seq { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string card_code { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string old_code { get; set; } = null!;
        [StringLength(3)]
        [Unicode(false)]
        public string card_div { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string card_image { get; set; }
        public int? card_price { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string card_name { get; set; }
        [StringLength(152)]
        [Unicode(false)]
        public string card_code_str { get; set; }
        public short? brand { get; set; }
        public int? company_seq { get; set; }
        public int? inpaper_seq { get; set; }
        public int? env_seq { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string erp_code { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string embo_print { get; set; } = null!;
        [StringLength(20)]
        [Unicode(false)]
        public string outsourcing_print { get; set; } = null!;
        [StringLength(1)]
        [Unicode(false)]
        public string brand_code { get; set; } = null!;
        [StringLength(100)]
        [Unicode(false)]
        public string brand_name { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isMasterPrintColor { get; set; } = null!;
    }
}
