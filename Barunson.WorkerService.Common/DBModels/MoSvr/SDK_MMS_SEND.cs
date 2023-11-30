using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.MoSvr
{
    [Index("SEND_STATUS", "SCHEDULE_TYPE", "SEND_DATE", Name = "IDX_SDK_M_S_0")]
    public partial class SDK_MMS_SEND
    {
        [Key]
        public long MSG_ID { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string USER_ID { get; set; } = null!;
        public int SCHEDULE_TYPE { get; set; }
        [StringLength(64)]
        [Unicode(false)]
        public string SUBJECT { get; set; }
        [StringLength(16)]
        [Unicode(false)]
        public string NOW_DATE { get; set; } = null!;
        [StringLength(16)]
        [Unicode(false)]
        public string SEND_DATE { get; set; } = null!;
        [StringLength(20)]
        [Unicode(false)]
        public string CALLBACK { get; set; } = null!;
        public int DEST_COUNT { get; set; }
        [Column(TypeName = "text")]
        public string DEST_INFO { get; set; } = null!;
        [Column(TypeName = "text")]
        public string MMS_MSG { get; set; }
        public int CONTENT_COUNT { get; set; }
        [StringLength(250)]
        [Unicode(false)]
        public string CONTENT_DATA { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string KT_OFFICE_CODE { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string CDR_ID { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string RESERVED1 { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string RESERVED2 { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string RESERVED3 { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string RESERVED4 { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string RESERVED5 { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string RESERVED6 { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string RESERVED7 { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string RESERVED8 { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string RESERVED9 { get; set; }
        public int SEND_STATUS { get; set; }
        public int SEND_COUNT { get; set; }
        public int SEND_RESULT { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string SEND_PROC_TIME { get; set; }
        public int MSG_TYPE { get; set; }
        public long? STD_ID { get; set; }
    }
}
