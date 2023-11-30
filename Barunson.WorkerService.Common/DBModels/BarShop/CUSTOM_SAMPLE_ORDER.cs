using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 샘플주문정보
    /// </summary>
    [Index("MEMBER_ID", "STATUS_SEQ", Name = "NCI_CUSTOM_SAMPLE_ORDER_MEMBER_STATUS")]
    [Index("DELIVERY_DATE", Name = "idx_custom_sample_order_delivery_date")]
    public partial class CUSTOM_SAMPLE_ORDER
    {
        public CUSTOM_SAMPLE_ORDER()
        {
            CUSTOM_SAMPLE_ORDER_ITEM = new HashSet<CUSTOM_SAMPLE_ORDER_ITEM>();
        }

        [Key]
        public int sample_order_seq { get; set; }
        /// <summary>
        /// manage_code.sales_gubun
        /// </summary>
        [StringLength(2)]
        [Unicode(false)]
        public string SALES_GUBUN { get; set; } = null!;
        /// <summary>
        /// 제휴업체
        /// </summary>
        public int COMPANY_SEQ { get; set; }
        /// <summary>
        /// 회원ID
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string MEMBER_ID { get; set; }
        /// <summary>
        /// 주문자 이름
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string MEMBER_NAME { get; set; } = null!;
        /// <summary>
        /// 주문자 이메일
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string MEMBER_EMAIL { get; set; }
        /// <summary>
        /// 사용안함
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string MEMBER_FAX { get; set; }
        /// <summary>
        /// 주문자 전화번호
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string MEMBER_PHONE { get; set; }
        /// <summary>
        /// 수취인 우편번호
        /// </summary>
        [StringLength(6)]
        [Unicode(false)]
        public string MEMBER_ZIP { get; set; } = null!;
        /// <summary>
        /// 수취인 주소
        /// </summary>
        [StringLength(255)]
        [Unicode(false)]
        public string MEMBER_ADDRESS { get; set; } = null!;
        /// <summary>
        /// 수취인 상세주소
        /// </summary>
        [StringLength(100)]
        [Unicode(false)]
        public string MEMBER_ADDRESS_DETAIL { get; set; }
        /// <summary>
        /// 주문일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime REQUEST_DATE { get; set; }
        /// <summary>
        /// 카드준비일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? PREPARE_DATE { get; set; }
        /// <summary>
        /// 배송처리일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? DELIVERY_DATE { get; set; }
        /// <summary>
        /// 배송 송장번호
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string DELIVERY_CODE_NUM { get; set; }
        /// <summary>
        /// 1:택배
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string DELIVERY_METHOD { get; set; } = null!;
        /// <summary>
        /// 택배사 코드(CJ:CJ택배)
        /// </summary>
        [StringLength(2)]
        [Unicode(false)]
        public string DELIVERY_COM { get; set; }
        /// <summary>
        /// 배송메모
        /// </summary>
        [StringLength(500)]
        [Unicode(false)]
        public string MEMO { get; set; }
        /// <summary>
        /// 배송창고 1:본사출고
        /// </summary>
        public byte? DELIVERY_CHANGO { get; set; }
        /// <summary>
        /// 주문상태 1:주문완료(결제전),3:결제완료,10:카드준비중,12:발송완료
        /// </summary>
        public int STATUS_SEQ { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string INVOICE_PRINT_YORN { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string JOB_ORDER_PRINT_YORN { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string DSP_PRINT_YORN { get; set; }
        /// <summary>
        /// 사용안함
        /// </summary>
        [StringLength(30)]
        [Unicode(false)]
        public string SETTLE_MOBILID { get; set; }
        /// <summary>
        /// 결제일
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? SETTLE_DATE { get; set; }
        /// <summary>
        /// 결제 핸드폰
        /// </summary>
        [StringLength(13)]
        [Unicode(false)]
        public string SETTLE_HPHONE { get; set; }
        public short? CARD_PRICE { get; set; }
        public short? REDUCE_PRICE { get; set; }
        /// <summary>
        /// 배송비
        /// </summary>
        public short? DELIVERY_PRICE { get; set; }
        /// <summary>
        /// 결제금액
        /// </summary>
        public int? SETTLE_PRICE { get; set; }
        /// <summary>
        /// 결제취소여부(사용안함)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string SETTLE_CANCEL { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string BUY_CONF { get; set; }
        [StringLength(25)]
        [Unicode(false)]
        public string ADMIN_ID { get; set; }
        /// <summary>
        /// 결제방법
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string SETTLE_METHOD { get; set; }
        /// <summary>
        /// 주문자 핸드폰번호
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string MEMBER_HPHONE { get; set; }
        /// <summary>
        /// 영수증 발행여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string ISDACOM { get; set; }
        /// <summary>
        /// PG상점 아이디
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string PG_MERTID { get; set; }
        /// <summary>
        /// 이니시스 TID
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string PG_TID { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string DACOM_TID { get; set; }
        /// <summary>
        /// PG결제결과
        /// </summary>
        [StringLength(255)]
        [Unicode(false)]
        public string PG_RESULTINFO { get; set; }
        /// <summary>
        /// PG결제결과 (입금자 이름)
        /// </summary>
        [StringLength(500)]
        [Unicode(false)]
        public string PG_RESULTINFO2 { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string card_installmonth { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string card_nointyn { get; set; }
        /// <summary>
        /// 주문취소일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? CANCEL_DATE { get; set; }
        /// <summary>
        /// 주문취소 사유
        /// </summary>
        [StringLength(100)]
        [Unicode(false)]
        public string CANCEL_REASON { get; set; }
        /// <summary>
        /// PG수수료
        /// </summary>
        public double? PG_FEE { get; set; }
        /// <summary>
        /// pG환불 수수료
        /// </summary>
        public double? PG_REFEE { get; set; }
        /// <summary>
        /// PG정산일
        /// </summary>
        [StringLength(10)]
        [Unicode(false)]
        public string PG_PAYDATE { get; set; }
        /// <summary>
        /// PG수금일
        /// </summary>
        [StringLength(10)]
        [Unicode(false)]
        public string PG_CALDATE { get; set; }
        /// <summary>
        /// PG환불정산일
        /// </summary>
        [StringLength(10)]
        [Unicode(false)]
        public string PG_REPAYDATE { get; set; }
        /// <summary>
        /// PG환불수금일
        /// </summary>
        [StringLength(10)]
        [Unicode(false)]
        public string PG_RECALDATE { get; set; }
        /// <summary>
        /// ERP전송일
        /// </summary>
        [StringLength(10)]
        [Unicode(false)]
        public string SRC_ERP_DATE { get; set; }
        /// <summary>
        /// 에스크로 거래건 여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isAscrow { get; set; }
        /// <summary>
        /// 한진택배 EDI 전송여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isHJ { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isVar { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string call_admin_id { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string etc_info_s { get; set; }
        /// <summary>
        /// mobile,Web 구분
        /// </summary>
        [StringLength(10)]
        [Unicode(false)]
        public string join_division { get; set; }
        /// <summary>
        /// 더카드 그룹 seq
        /// </summary>
        public int? order_g_seq { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isOneClickSample { get; set; }
        /// <summary>
        /// 묶음배송 seq
        /// </summary>
        public int? MULTI_PACK_SEQ { get; set; }
        /// <summary>
        /// 묶음배송 건수 (1,2)
        /// </summary>
        public int? MULTI_PACK_SUB_SEQ { get; set; }
        /// <summary>
        /// 묶음배송 등록일
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? MULTI_PACK_REG_DATE { get; set; }
        /// <summary>
        /// 예식일
        /// </summary>
        [StringLength(10)]
        [Unicode(false)]
        public string WEDD_DATE { get; set; }
        [StringLength(10)]
        public string OPT_GUBUN { get; set; }
        [StringLength(4)]
        public string OPT_VALUES { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string WisaFlag { get; set; }

        [InverseProperty("SAMPLE_ORDER_SEQNavigation")]
        public virtual ICollection<CUSTOM_SAMPLE_ORDER_ITEM> CUSTOM_SAMPLE_ORDER_ITEM { get; set; }
    }
}
