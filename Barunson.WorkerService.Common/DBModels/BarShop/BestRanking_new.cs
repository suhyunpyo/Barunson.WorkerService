using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    public partial class BestRanking_new
    {
        [Key]
        public int id { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string sales_Gubun { get; set; }
        public int? company_seq { get; set; }
        public short Rank { get; set; }
        public int Card_Seq { get; set; }
        public int Cnt { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime RegDate { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string Gubun { get; set; } = null!;
        [StringLength(10)]
        [Unicode(false)]
        public string Gubun_data { get; set; }
        [StringLength(5)]
        [Unicode(false)]
        public string RankChangeGubun { get; set; }
        [StringLength(2)]
        [Unicode(false)]
        public string RankChangeNo { get; set; }
    }
}
