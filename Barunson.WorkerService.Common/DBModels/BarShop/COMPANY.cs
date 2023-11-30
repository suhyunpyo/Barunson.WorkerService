using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 제휴사 정보
    /// </summary>
    public partial class COMPANY
    {

        [Key]
        public int COMPANY_SEQ { get; set; }
        /// <summary>
        /// W:웹,D:대리점,O:오프영업
        /// </summary>
        [StringLength(2)]
        [Unicode(false)]
        public string SALES_GUBUN { get; set; } = null!;
        /// <summary>
        /// W:웹,D:대리점,O:오프영업,M:EC대리점,C:EC 커스터마이징,B:EC B2B,E:e청첩장
        /// </summary>
        [StringLength(2)]
        [Unicode(false)]
        public string JAEHU_KIND { get; set; }
        /// <summary>
        /// 0:기본값(사이트링크),1:결제안함(제휴사매출),2:결제함(바른손매출)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string JUMUN_TYPE { get; set; }
        public int COMPANY_UPPER_SEQ { get; set; }
        /// <summary>
        /// 업체명
        /// </summary>
        [StringLength(128)]
        [Unicode(false)]
        public string COMPANY_NAME { get; set; } = null!;
        /// <summary>
        /// 사업자 번호
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string COMPANY_NUM { get; set; }
        /// <summary>
        /// 등록일
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? REGIST_DATE { get; set; }
        /// <summary>
        /// 시작일
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? START_DATE { get; set; }
        /// <summary>
        /// 마감일
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? END_DATE { get; set; }
        /// <summary>
        /// 상태 (S1:대기,S2:진행,S3:삭제)
        /// </summary>
        [StringLength(2)]
        [Unicode(false)]
        public string STATUS { get; set; }
        /// <summary>
        /// 사용안함
        /// </summary>
        [StringLength(100)]
        [Unicode(false)]
        public string IMG_DIR { get; set; }
        /// <summary>
        /// 로그인 아이디
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string LOGIN_ID { get; set; } = null!;
        /// <summary>
        /// 비밀번호
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string PASSWD { get; set; } = null!;
        /// <summary>
        /// 대표자 이름
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string BOSS_NM { get; set; }
        /// <summary>
        /// 대표 번호
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string BOSS_TEL_NO { get; set; }
        /// <summary>
        /// 업태 (ERP 연동시 tax 타입으로 사용,22:매출영수증,10:매출일반과세)
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string UP_TAE { get; set; }
        /// <summary>
        /// 업체 팩스번호
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string FAX_NO { get; set; } = null!;
        [StringLength(20)]
        [Unicode(false)]
        public string KIND { get; set; }
        /// <summary>
        /// 이메일
        /// </summary>
        [StringLength(120)]
        [Unicode(false)]
        public string E_MAIL { get; set; } = null!;
        /// <summary>
        /// 우편번호
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string ZIP_CODE { get; set; } = null!;
        /// <summary>
        /// 주소 앞부분
        /// </summary>
        [StringLength(100)]
        [Unicode(false)]
        public string FRONT_ADDR { get; set; } = null!;
        /// <summary>
        /// 주소 뒷부분
        /// </summary>
        [StringLength(100)]
        [Unicode(false)]
        public string BACK_ADDR { get; set; } = null!;
        /// <summary>
        /// 관리자 이름
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string MNG_NM { get; set; } = null!;
        /// <summary>
        /// 관리자 이메일
        /// </summary>
        [StringLength(120)]
        [Unicode(false)]
        public string MNG_E_MAIL { get; set; } = null!;
        /// <summary>
        /// 관리자 연락처
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string MNG_TEL_NO { get; set; } = null!;
        [StringLength(20)]
        [Unicode(false)]
        public string MNG_HP_NO { get; set; } = null!;
        [StringLength(20)]
        [Unicode(false)]
        public string MNG_ZIP_CODE { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string MNG_ADDRESS { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string MNG_ADDR_DETAIL { get; set; }
        /// <summary>
        /// 정산담당자 이름
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string ACC_NM { get; set; } = null!;
        /// <summary>
        /// 정산담당자 이메일
        /// </summary>
        [StringLength(120)]
        [Unicode(false)]
        public string ACC_E_MAIL { get; set; } = null!;
        /// <summary>
        /// 정산담당자 연락처
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string ACC_TEL_NO { get; set; } = null!;
        /// <summary>
        /// 정산담당자 핸드폰 번호
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string ACC_HP_NO { get; set; } = null!;
        [StringLength(500)]
        [Unicode(false)]
        public string CORP_EXP { get; set; } = null!;
        /// <summary>
        /// 정산은행이름
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string BANK_NM { get; set; }
        /// <summary>
        /// 정산은행 계좌번호
        /// </summary>
        [StringLength(30)]
        [Unicode(false)]
        public string ACCOUNT_NO { get; set; }
        /// <summary>
        /// 기본 할인율
        /// </summary>
        public short? SUPPLY_DISRATE { get; set; }
        /// <summary>
        /// 등록 관리자 아이디
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string REG_ID { get; set; } = null!;
        /// <summary>
        /// 변경 관리자 아이디
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string CHG_ID { get; set; } = null!;
        /// <summary>
        /// 최종 변경일
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime CHG_DT { get; set; }
        /// <summary>
        /// 온/오프 제휴
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string ONOFF { get; set; } = null!;
        /// <summary>
        /// 대리점의 경우 업체 URL
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string INFO_TMP { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string INFO_TMP2 { get; set; }
        /// <summary>
        /// 메인 이미지
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string INFO_TMP3 { get; set; }
        /// <summary>
        /// 1일 경우 무료식권 제공
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string INFO_TMP4 { get; set; }
        /// <summary>
        /// 고객에게 발송되는 메일에서 링크될 mypage주소
        /// </summary>
        [StringLength(200)]
        [Unicode(false)]
        public string mypage_url { get; set; }
        public short? ewed_val { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string ERP_CODE { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string UP_TAE_STR { get; set; }
        public byte PRICE_GUBUN { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string SASIK_GUBUN { get; set; }
        /// <summary>
        /// 1: 영업1본부, 2: 영업2본부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string ERP_Dept { get; set; }
        /// <summary>
        /// 10 : 일반과세, 22: 매출영수증
        /// </summary>
        [StringLength(2)]
        [Unicode(false)]
        public string ERP_TaxType { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string ERP_PartCode { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string ERP_StaffCode { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string ERP_CostCode { get; set; }
        /// <summary>
        /// 1:대리점가,2:출고가,3:소비자가
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string ERP_PriceLevel { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string ERP_PGcheck { get; set; }
        /// <summary>
        /// Y:후불정산업체
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string ERP_PayLater { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string COMPANY_UPPER_YN { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string jehu_grade { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string feeType { get; set; }
        public double? feeRate { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string FIRST_ALARM { get; set; }
        [StringLength(20)]
        public string area { get; set; }
        [StringLength(20)]
        public string bank { get; set; }
        [StringLength(40)]
        public string bank_account_name { get; set; }
        [StringLength(40)]
        public string bank_account_no { get; set; }

    }
}
