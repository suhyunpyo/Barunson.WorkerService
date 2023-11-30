using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    public partial class CJ_API_LOG
    {
        [Key]
        public int logseq { get; set; }

        [Unicode(false)]
        public string ORDER_SEQ { get; set; }
        [Unicode(false)]
        public string KIND { get; set; }
        [Unicode(false)]
        public string RESULT_CODE { get; set; }
        public string RESULT_MSG { get; set; }
        public DateTime? REG_DATE { get; set; }
    }
}
