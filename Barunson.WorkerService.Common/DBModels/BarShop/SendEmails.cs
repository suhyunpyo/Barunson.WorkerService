using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    [Index("SendYn", Name = "IX_SendEmailContent_SendYn")]
    public partial class SendEmailContent
    {
        [Key]
        public int ContentId { get; set; }

        /// <summary>
        /// 패턴정의: 영문 대문자 1자리+숫자 2자리코드  ex)C01-&gt; Customer 관련 첫번재 메일폼
        /// </summary>
        [StringLength(3)]
        public string EmailFormCode { get; set; } = null!;

        /// <summary>
        /// 기본값: false, 내용 수정시 false 변경
        /// </summary>
        public bool SendYn { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? SendDate { get; set; }

        /// <summary>
        /// 추가 수신 대상이 있을 경우
        /// </summary>
        [StringLength(30)]
        public string? ToName { get; set; }

        /// <summary>
        /// 추가 수신 대상이 있을 경우
        /// </summary>
        [StringLength(100)]
        public string? ToEmailAddress { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime RegDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime ModDate { get; set; }

        [InverseProperty("Content")]
        public virtual ICollection<SendEmailContentItem> SendEmailContentItem { get; } = new List<SendEmailContentItem>();
    }

    [PrimaryKey("ContentId", "ContentKey")]
    public partial class SendEmailContentItem
    {
        [Key]
        public int ContentId { get; set; }

        [Key]
        [StringLength(100)]
        public string ContentKey { get; set; } = null!;

        public string? ContentValue { get; set; }

        [ForeignKey("ContentId")]
        [InverseProperty("SendEmailContentItem")]
        public virtual SendEmailContent Content { get; set; } = null!;
    }
    public partial class SendEmailMaster
    {
        /// <summary>
        /// 패턴정의: 영문 대문자 1자리+숫자 2자리코드  ex)C01-&gt; Customer 관련 첫번재 메일폼
        /// </summary>
        [Key]
        [StringLength(3)]
        public string EmailFormCode { get; set; } = null!;

        [StringLength(30)]
        public string SenderName { get; set; } = null!;

        [StringLength(100)]
        public string SenderEmailAddress { get; set; } = null!;

        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        public bool? UseYn { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime RegDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime ModDate { get; set; }

        /// <summary>
        /// ([이메일] - 신청자 메일 주소, [문의사항] - 문의사항내용 ) 등 콘텐트에 치환될 내용 설명(참고용)
        /// </summary>
        [StringLength(2000)]
        public string? ContentDescription { get; set; }

        /// <summary>
        /// 메일 본문 또는 본문 페이지 URL
        /// </summary>
        public string Contents { get; set; } = null!;

        [InverseProperty("EmailFormCodeNavigation")]
        public virtual ICollection<SendEmailRecipient> SendEmailRecipient { get; } = new List<SendEmailRecipient>();
    }
    [PrimaryKey("EmailFormCode", "Sort")]
    public partial class SendEmailRecipient
    {
        /// <summary>
        /// 패턴정의: 영문 대문자 1자리+숫자 2자리코드  ex)C01-&gt; Customer 관련 첫번재 메일폼
        /// </summary>
        [Key]
        [StringLength(3)]
        public string EmailFormCode { get; set; } = null!;

        [Key]
        public int Sort { get; set; }

        [StringLength(30)]
        public string ToName { get; set; } = null!;

        [StringLength(100)]
        public string ToEmailAddress { get; set; } = null!;

        [Required]
        public bool? UserYn { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime RegDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime ModDate { get; set; }

        [ForeignKey("EmailFormCode")]
        [InverseProperty("SendEmailRecipient")]
        public virtual SendEmailMaster EmailFormCodeNavigation { get; set; } = null!;
    }
}
