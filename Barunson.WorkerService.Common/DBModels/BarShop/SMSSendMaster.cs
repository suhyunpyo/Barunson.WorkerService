using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{

    /// <summary>
    /// 문자발송관리 마스터
    /// </summary>
    public partial class SMSSendMaster
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 제목
        /// </summary>
        [StringLength(300)]
        public string Title { get; set; } = null!;

        /// <summary>
        /// 분류: 1.광고, 2. 정보
        /// </summary>
        public byte Class { get; set; }

        /// <summary>
        /// 상태: 0.작성중, 90. 테스트발송대기, 99. 테스트 발송완료, 100. 발송대기, 105. 발송중, 110.발송완료, 150.취소
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 발송유형: 1.즉시발송, 2.예약발송
        /// </summary>
        public byte SendType { get; set; }

        /// <summary>
        /// 즉시발송시 발송 시작 시간(발송대기 업데이트), 예약발송시 예약 날짜시간
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? SendTime { get; set; }

        [StringLength(5)]
        [Unicode(false)] 
        public string SenderSiteCode { get; set; } = null!;

        /// <summary>
        /// 발신번호
        /// </summary>
        [StringLength(15)]
        public string SenderPhone { get; set; } = null!;

        [StringLength(4000)]
        public string? Message { get; set; }

        /// <summary>
        /// 등록일
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime RegDate { get; set; }

        /// <summary>
        /// 등록자ID
        /// </summary>
        [StringLength(100)]
        [Unicode(false)]
        public string RegUserId { get; set; } = null!;

        /// <summary>
        /// 등록자명
        /// </summary>
        [StringLength(100)]
        public string RegUserName { get; set; } = null!;

        /// <summary>
        /// 테스트발송대상: 이름^전화번호|이름^전화번호
        /// </summary>
        [StringLength(1000)]
        public string? TestSendTarget { get; set; }

        /// <summary>
        /// 발송대상 수
        /// </summary>
        public int TargetCount { get; set; }

        /// <summary>
        /// 발송성공 수
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 발송실패 수
        /// </summary>
        public int FailCount { get; set; }

        /// <summary>
        /// 취소일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? CancelTime { get; set; }

        /// <summary>
        /// MMS 파일 URL
        /// </summary>
        public string? FIleContent { get; set; }

        [InverseProperty("IdNavigation")]
        public virtual ICollection<SMSSendTargetList> SMSSendTargetList { get; } = new List<SMSSendTargetList>();
    }
}
