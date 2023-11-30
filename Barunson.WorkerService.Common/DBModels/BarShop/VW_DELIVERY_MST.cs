using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    [Keyless]
    public partial class VW_DELIVERY_MST
    {
        public int ORDER_SEQ { get; set; }
        [StringLength(2)]
        [Unicode(false)]
        public string SALES_GUBUN { get; set; }
        public int? COMPANY_SEQ { get; set; }
        [StringLength(128)]
        [Unicode(false)]
        public string COMPANY_NAME { get; set; } = null!;
        [StringLength(2)]
        [Unicode(false)]
        public string ORDER_TYPE { get; set; }
        [StringLength(19)]
        [Unicode(false)]
        public string ORDER_TABLE_NAME { get; set; } = null!;
        [StringLength(1)]
        [Unicode(false)]
        public string ISHJ { get; set; }
        public int? STATUS_SEQ { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string ERP_PARTCODE { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string DELIVERY_CODE { get; set; }
        [StringLength(6)]
        [Unicode(false)]
        public string RECV_ZIP { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string RECV_ADDR { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string RECV_ADDR_DETAIL { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string RECV_NAME { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string RECV_PHONE { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string RECV_HPHONE { get; set; }
        [StringLength(200)]
        [Unicode(false)]
        public string RECV_MSG { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? SEND_DATE { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string DELIVERY_MSG { get; set; } = null!;
        public int DELIVERY_SEQ { get; set; }
    }
}
