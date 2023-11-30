using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    public partial class CJ_ONEDAYTOKEN
    {
        [Key]
        [StringLength(50)]
        [Unicode(false)]
        public string TOKEN_NUM { get; set; }

        [StringLength(20)]
        [Unicode(false)]
        public string TOKEN_EXPRTN_DTM { get; set; }

        [Column(TypeName = "datetime")] 
        public DateTime? REG_DATE { get; set; }

    }
}
