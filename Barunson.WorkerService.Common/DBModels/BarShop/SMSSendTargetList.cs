using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    [PrimaryKey("Id", "Seq")]
    public partial class SMSSendTargetList
    {
        [Key]
        public int Id { get; set; }
        [Key]
        public int Seq { get; set; }
        /// <summary>
        /// 발송대상번호
        /// </summary>
        [StringLength(15)]
        [Unicode(false)]
        public string PhoneNo { get; set; } = null!;

        /// <summary>
        /// 이름
        /// </summary>
        [StringLength(30)]
        public string Name { get; set; } = null!;

        [StringLength(100)]
        public string? A { get; set; }

        [StringLength(100)]
        public string? B { get; set; }

        [StringLength(100)]
        public string? C { get; set; }

        [StringLength(100)]
        public string? D { get; set; }

        /// <summary>
        /// 40: 실패, 100. 발송대기, 105. 발송중, 110.발송완료,
        /// </summary>
        public byte Status { get; set; }

        [StringLength(100)] 
        public string? StatusText { get; set; }
        /// <summary>
        /// 발송시간
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? SendTime { get; set; }

        [ForeignKey("Id")]
        [InverseProperty("SMSSendTargetList")]
        public virtual SMSSendMaster IdNavigation { get; set; } = null!;
    }
}
