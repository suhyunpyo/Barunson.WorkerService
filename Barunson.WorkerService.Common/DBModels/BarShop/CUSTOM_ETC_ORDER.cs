using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 부가상품주문내역
    /// </summary>
    public partial class CUSTOM_ETC_ORDER
    {
        [Key]
        public int order_seq { get; set; }
        /// <summary>
        /// manage_code.code (code_type =etcprod)
        /// </summary>
        [StringLength(2)]
        [Unicode(false)]
        public string order_type { get; set; }
        /// <summary>
        /// 판매사이트구분
        /// </summary>
        [StringLength(2)]
        [Unicode(false)]
        public string sales_gubun { get; set; } = null!;
        [StringLength(1)]
        [Unicode(false)]
        public string site_gubun { get; set; }
        public short company_Seq { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string member_id { get; set; }
        /// <summary>
        /// 주문자 이름
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string order_name { get; set; } = null!;
        /// <summary>
        /// 주문자 이메일
        /// </summary>
        [StringLength(100)]
        [Unicode(false)]
        public string order_email { get; set; } = null!;
        /// <summary>
        /// 주문자 전화번호
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string order_phone { get; set; } = null!;
        /// <summary>
        /// 주문자 핸드폰번호
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string order_hphone { get; set; }
        /// <summary>
        /// 수취인 인름
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string recv_name { get; set; } = null!;
        /// <summary>
        /// 수취인 전화번호
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string recv_phone { get; set; } = null!;
        /// <summary>
        /// 수취인 핸드폰 번호
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string recv_hphone { get; set; }
        /// <summary>
        /// 수취인 주소
        /// </summary>
        [StringLength(255)]
        [Unicode(false)]
        public string recv_address { get; set; }
        /// <summary>
        /// 수취인 상세주소
        /// </summary>
        [StringLength(100)]
        [Unicode(false)]
        public string recv_address_detail { get; set; }
        /// <summary>
        /// 수취인 우편번호
        /// </summary>
        [StringLength(6)]
        [Unicode(false)]
        public string recv_zip { get; set; }
        /// <summary>
        /// 배송 메시지
        /// </summary>
        [StringLength(200)]
        [Unicode(false)]
        public string recv_msg { get; set; }
        /// <summary>
        /// 주문상태
        /// </summary>
        public byte? status_seq { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? order_date { get; set; }
        /// <summary>
        /// 결제일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? settle_date { get; set; }
        /// <summary>
        /// 배송비
        /// </summary>
        public int? delivery_price { get; set; }
        /// <summary>
        /// 옵션선택비용
        /// </summary>
        public int? option_price { get; set; }
        /// <summary>
        /// 쿠폰번호
        /// </summary>
        [StringLength(40)]
        [Unicode(false)]
        public string couponseq { get; set; }
        /// <summary>
        /// 쿠폰차감금액
        /// </summary>
        public int? coupon_price { get; set; }
        /// <summary>
        /// 결제금액
        /// </summary>
        public int? settle_price { get; set; }
        /// <summary>
        /// 결제취소일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? settle_Cancel_Date { get; set; }
        /// <summary>
        /// 결제방법
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string settle_method { get; set; }
        /// <summary>
        /// 데이콤 PG아이디
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string pg_shopid { get; set; }
        /// <summary>
        /// 데이콤 주문번호 or 이니시스 연동 TID
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string pg_tid { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string dacom_tid { get; set; }
        /// <summary>
        /// 영수증발급ID
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string pg_receipt_tid { get; set; }
        /// <summary>
        /// 결제결과-결제정보
        /// </summary>
        [StringLength(255)]
        [Unicode(false)]
        public string pg_resultinfo { get; set; }
        /// <summary>
        /// 결제결과-결제자명
        /// </summary>
        [StringLength(500)]
        [Unicode(false)]
        public string pg_resultinfo2 { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string card_installmonth { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string card_nointyn { get; set; }
        /// <summary>
        /// 영수증발급여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isReceipt { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? compose_date { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? mod_request_date { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? mod_compose_date { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string compose_admin_id { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? confirm_date { get; set; }
        /// <summary>
        /// 준비일자
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? prepare_date { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? print_date { get; set; }
        /// <summary>
        /// 배송일자
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? delivery_date { get; set; }
        /// <summary>
        /// 송장코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string delivery_code { get; set; }
        /// <summary>
        /// 배송방법
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string delivery_method { get; set; }
        /// <summary>
        /// 배송업체
        /// </summary>
        [StringLength(2)]
        [Unicode(false)]
        public string delivery_com { get; set; }
        public double? pg_Fee { get; set; }
        public double? pg_refee { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string pg_paydate { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string pg_caldate { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string pg_repaydate { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string pg_recaldate { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string delivery_admin_id { get; set; }
        [StringLength(15)]
        [Unicode(false)]
        public string admin_id { get; set; }
        public int? etc_info_l { get; set; }
        [StringLength(200)]
        [Unicode(false)]
        public string etc_info_s { get; set; }
        [StringLength(1000)]
        [Unicode(false)]
        public string result_info { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string src_erp_date { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string admin_memo { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isAscrow { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isHJ { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string coupon_no { get; set; }
        /// <summary>
        /// 더카드전용 통합SEQ
        /// </summary>
        public int? order_g_seq { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string SampleBook_ID { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? Return_Limit_Date { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? Return_Request_Date { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? Return_Proceeding_Date { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? Return_Complete_Date { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? Stock_Date { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string WisaFlag { get; set; }
    }

    /// <summary>
    /// 부가상품 주문상세정보
    /// </summary>
    public partial class CUSTOM_ETC_ORDER_ITEM
    {
        public int card_seq { get; set; }
        [Key]
        public int order_seq { get; set; }
        [Key]
        public byte seq { get; set; }
        /// <summary>
        /// 주문수량
        /// </summary>
        public int order_count { get; set; }
        /// <summary>
        /// 상품소비자가격
        /// </summary>
        public int card_price { get; set; }
        /// <summary>
        /// 상품할인구매가
        /// </summary>
        public double card_sale_price { get; set; }
        /// <summary>
        /// W:청첩장 테이블,E:etc 테이블
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string order_tbl { get; set; } = null!;
        /// <summary>
        /// 시즌카드의 경우 카드교체여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isChange { get; set; } = null!;
        /// <summary>
        /// 제품선택옵션
        /// </summary>
        [StringLength(500)]
        [Unicode(false)]
        public string card_opt { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string SampleBook_ID { get; set; }
        public byte? SampleBook_Status { get; set; }
    }
}
