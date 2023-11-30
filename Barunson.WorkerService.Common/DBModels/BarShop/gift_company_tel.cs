using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    [Keyless]
    public partial class gift_company_tel
    {
        [StringLength(5)]
        [Unicode(false)]
        public string code { get; set; } = null!;
        [StringLength(40)]
        [Unicode(false)]
        public string company_name { get; set; } = null!;
        [StringLength(40)]
        [Unicode(false)]
        public string company_info { get; set; }
        [StringLength(13)]
        [Unicode(false)]
        public string company_tel { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isYN { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime created_tmstmp { get; set; }
    }
}
