using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    public partial class BarunWorkerTask
    {
        [Key]
        [StringLength(50)]
        public string FunctionName { get; set; } = null!;
        [Key]
        [StringLength(50)]
        public string WorkerName { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime? LastExcuteTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? NextExcuteTime { get; set; }

        public string ConfigJson { get; set; } = null;
    }
}
