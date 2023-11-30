using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Barunson.WorkerService.Common.DBModels.DearDeer
{
    [Index(nameof(sample_order_no), Name = "sample_order_no")]
    [Index(nameof(user_id), Name = "user_id")]
    public partial class sample_orders
    {
        [Key]
        [Column(TypeName = "int(10) unsigned")]
        public uint id { get; set; }
        [Required]
        [StringLength(30)]
        public string sample_order_no { get; set; }
        [Column(TypeName = "int(11)")]
        public int user_id { get; set; }
        [Column(TypeName = "int(11)")]
        public int addresses_id { get; set; }
        [Column(TypeName = "int(11)")]
        public int partner_shop_id { get; set; }
        [StringLength(20)]
        public string groom_fname { get; set; }
        [StringLength(20)]
        public string groom_name { get; set; }
        [StringLength(20)]
        public string groom_fname_eng { get; set; }
        [StringLength(20)]
        public string groom_name_eng { get; set; }
        [StringLength(20)]
        public string bride_fname { get; set; }
        [StringLength(20)]
        public string bride_name { get; set; }
        [StringLength(20)]
        public string bride_fname_eng { get; set; }
        [StringLength(20)]
        public string bride_name_eng { get; set; }
        [StringLength(50)]
        public string wedd_date { get; set; }
        [Required]
        [StringLength(2)]
        public string shipping_state { get; set; }
        [Required]
        [StringLength(2)]
        public string printing_state { get; set; }
        [Required]
        [StringLength(2)]
        public string order_state { get; set; }
        [StringLength(50)]
        public string shipping_num { get; set; }
        [StringLength(50)]
        public string shipping_company { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime? shipping_at { get; set; }
        [Column(TypeName = "int(11)")]
        public int? barunson_order_seq { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime? created_at { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime? updated_at { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime? deleted_at { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime? cancel_at { get; set; }
        [Column(TypeName = "text")]
        public string memo { get; set; }
        [Column(TypeName = "int(11)")]
        public int? total_money { get; set; }
        [Column(TypeName = "int(11)")]
        public int? paid_money { get; set; }
        [Column(TypeName = "int(11)")]
        public int? cash_request { get; set; }
        [Column(TypeName = "int(11)")]
        public int? cash { get; set; }
        [StringLength(255)]
        public string pay_type { get; set; }
        [StringLength(255)]
        public string pg_name { get; set; }
        [StringLength(255)]
        public string pg_tno { get; set; }
        [StringLength(255)]
        public string pg_app_no { get; set; }
        [StringLength(255)]
        public string bank_info { get; set; }
        [StringLength(255)]
        public string bank_name { get; set; }
        [Column(TypeName = "text")]
        public string admin_memo { get; set; }
        [StringLength(14)]
        public string pay_date { get; set; }
        [Column(TypeName = "int(11)")]
        public int? pay_year { get; set; }
        [Column(TypeName = "int(11)")]
        public int? pay_month { get; set; }
        [Column(TypeName = "int(11)")]
        public int? pay_day { get; set; }
        [StringLength(20)]
        public string order_ip { get; set; }
        [StringLength(255)]
        public string order_useragent { get; set; }
        [StringLength(4000)]
        public string service_memo { get; set; }
        [Required]
        [StringLength(1)]
        public string is_create_file { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime? refunded_at { get; set; }
        [StringLength(1)]
        public string refund_type { get; set; }
        [Column(TypeName = "int(11)")]
        public int? refund_money { get; set; }
        [Required]
        [StringLength(20)]
        public string order_step { get; set; }
        [Column(TypeName = "int(11)")]
        public int? paid_list_id { get; set; }
    }
}
