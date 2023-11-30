using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 비즈톡 발송
    /// </summary>
    [Index("msg_status", "date_client_req", Name = "idx_ata_mmt_tran_1")]
    [Index("recipient_num", Name = "idx_ata_mmt_tran_2")]
    [Index("ata_id", Name = "idx_ata_mmt_tran_3")]
    [Index("sender_key", "template_code", Name = "idx_ata_mmt_tran_4")]
    public partial class ata_mmt_tran
    {
        [Key]
        public int mt_pr { get; set; }
        /// <summary>
        /// 부서 코드 (참조용 필드)
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string mt_refkey { get; set; }
        /// <summary>
        /// 메시지 우선 순위
        /// </summary>
        [StringLength(2)]
        [Unicode(false)]
        public string priority { get; set; } = null!;
        /// <summary>
        /// 전송 예약 시간
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime date_client_req { get; set; }
        /// <summary>
        /// 메시지 제목
        /// </summary>
        [StringLength(40)]
        [Unicode(false)]
        public string subject { get; set; } = null!;
        /// <summary>
        /// 전송 메시지
        /// </summary>
        [StringLength(4000)]
        [Unicode(false)]
        public string content { get; set; } = null!;
        /// <summary>
        /// 발신자 전화 번호
        /// </summary>
        [StringLength(25)]
        [Unicode(false)]
        public string callback { get; set; } = null!;
        /// <summary>
        /// 메시지 상태 (1-전송대기, 2-결과대기, 3-완료)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string msg_status { get; set; } = null!;
        /// <summary>
        /// 수신자 전화 번호
        /// </summary>
        [StringLength(25)]
        [Unicode(false)]
        public string recipient_num { get; set; }
        /// <summary>
        /// Biz talk G/W 접수 시간
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? date_mt_sent { get; set; }
        /// <summary>
        /// 단말기 도착 시간
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? date_rslt { get; set; }
        /// <summary>
        /// Biz talk 으로부터 결과 수신한 시간
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? date_mt_report { get; set; }
        /// <summary>
        /// 전송 결과(1000-성공, 기타-실패)
        /// </summary>
        [StringLength(4)]
        [Unicode(false)]
        public string report_code { get; set; }
        /// <summary>
        /// 전송된 Biz talk G/W 정보
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string rs_id { get; set; }
        /// <summary>
        /// 국가 코드
        /// </summary>
        [StringLength(8)]
        [Unicode(false)]
        public string country_code { get; set; } = null!;
        /// <summary>
        /// 메시지 종류(1008-알림톡, 1009-친구톡)
        /// </summary>
        public int msg_type { get; set; }
        /// <summary>
        /// 암호화 사용 유무
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string crypto_yn { get; set; }
        /// <summary>
        /// ATA 이중화시 사용되는 ID
        /// </summary>
        [StringLength(2)]
        [Unicode(false)]
        public string ata_id { get; set; }
        /// <summary>
        /// 데이터 등록일자
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? reg_date { get; set; }
        /// <summary>
        /// 발신 프로필 키
        /// </summary>
        [StringLength(40)]
        [Unicode(false)]
        public string sender_key { get; set; } = null!;
        /// <summary>
        /// 메시지 유형 템플릿 코드
        /// </summary>
        [StringLength(30)]
        [Unicode(false)]
        public string template_code { get; set; }
        /// <summary>
        /// 메시지 발송 방식
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string response_method { get; set; } = null!;
        /// <summary>
        /// 카카오톡 친구톡 발송시 광고성메시지
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string ad_flag { get; set; } = null!;
        /// <summary>
        /// 카카오톡 전송방식 1-format string 2-JSON 3-XML
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string kko_btn_type { get; set; }
        /// <summary>
        /// 버튼템플릿 전송시 버튼 정보
        /// </summary>
        [StringLength(4000)]
        [Unicode(false)]
        public string kko_btn_info { get; set; }
        /// <summary>
        /// 친구톡 이미지 URL
        /// </summary>
        [StringLength(200)]
        [Unicode(false)]
        public string img_url { get; set; }
        /// <summary>
        /// 친구톡 이미지 클릭시 이동할 URL
        /// </summary>
        [StringLength(100)]
        [Unicode(false)]
        public string img_link { get; set; }
        /// <summary>
        /// 유저 기타필드)sales_gubun
        /// </summary>
        [StringLength(100)]
        [Unicode(false)]
        public string etc_text_1 { get; set; }
        /// <summary>
        /// 유저 기타필드)호출프로시저
        /// </summary>
        [StringLength(100)]
        [Unicode(false)]
        public string etc_text_2 { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string etc_text_3 { get; set; }
        /// <summary>
        /// 유저 기타필드)company_seq
        /// </summary>
        public int? etc_num_1 { get; set; }
        public int? etc_num_2 { get; set; }
        public int? etc_num_3 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? etc_date_1 { get; set; }
    }
}
