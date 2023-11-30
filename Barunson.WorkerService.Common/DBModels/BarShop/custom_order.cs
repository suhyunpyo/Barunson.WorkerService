using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 주문관련정보
    /// </summary>
    [Index("status_seq", "src_send_date", Name = "IDX_corder__send_date_status_seq")]
    [Index("weddinfo_id", Name = "IDX_corder__weddinfo_id")]
    [Index("status_seq", "member_id", Name = "NIC_ORDER_MEMBER_STATUS")]
    public partial class custom_order
    {
        [Key]
        public int order_seq { get; set; }
        /// <summary>
        /// 추가주문일 경우 원 주문번호
        /// </summary>
        public int? up_order_seq { get; set; }
        /// <summary>
        /// 주문타입 (1:청첩장 2:감사장 3:초대장 4,시즌카드 5:미니청첩장 6:포토/디지탈 7:이니셜 8:포토미니)
        /// </summary>
        [StringLength(2)]
        [Unicode(false)]
        public string order_type { get; set; }
        /// <summary>
        /// B:제휴,H:프페 제휴, SA:비핸즈, SS:프페,SB: 바른손, ST:더카드,D:대리점 , P:아웃바운드, Q:지역대리점
        /// </summary>
        [StringLength(2)]
        [Unicode(false)]
        public string sales_Gubun { get; set; }
        /// <summary>
        /// (0:원래의 사이트,1:제휴사,3:제휴사 커스터마이징,4:사고건)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string site_gubun { get; set; }
        /// <summary>
        /// 0:pg 결제,1:주문영업결제,2:제휴사 후불,4:사고건
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string pay_Type { get; set; }
        /// <summary>
        /// 이니셜 청첩장의 인쇄타입
        /// </summary>
        [StringLength(3)]
        [Unicode(false)]
        public string print_type { get; set; }
        /// <summary>
        /// 사고유형
        /// </summary>
        [StringLength(4)]
        [Unicode(false)]
        public string trouble_type { get; set; }
        public int? company_seq { get; set; }
        /// <summary>
        /// 1:주문 삭제 / 0:주문진행중 / 1:주문완료/ 3:주문취소/ 5:결제취소/ 6:초안수정요청/ 7:초안등록/ 8:초안수정등록/ 9:컨펌완료/ 10:인쇄대기/ 11:인쇄중/ 12:인쇄완료/ 13:제본/ 14:포장/ 15:발송
        /// </summary>
        public int status_seq { get; set; }
        /// <summary>
        /// 결제상태 (0:결제이전/ 1:가상계좌입금전/ 2:결제완료/ 3,5:결제취소)
        /// </summary>
        public byte? settle_status { get; set; }
        /// <summary>
        /// 인쇄 대기 상태
        /// </summary>
        public byte? printW_status { get; set; }
        /// <summary>
        /// 미니청첩장 인쇄 진행 상태
        /// </summary>
        public byte? mini_status_seq { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isStoreRequisit { get; set; }
        /// <summary>
        /// 제휴사측의 주문번호
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string coop_orderid { get; set; }
        /// <summary>
        /// 주문일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? order_date { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? src_ap_date { get; set; }
        /// <summary>
        /// 초안작성일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? src_compose_date { get; set; }
        /// <summary>
        /// 초안 최종 수정 요청일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? src_modRequest_date { get; set; }
        /// <summary>
        /// 초안 최종 수정일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? src_compose_mod_date { get; set; }
        /// <summary>
        /// 초안 확정일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? src_confirm_date { get; set; }
        /// <summary>
        /// 원고출력 처리일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? src_printCopy_date { get; set; }
        /// <summary>
        /// 원고마감 처리일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? src_CloseCopy_date { get; set; }
        /// <summary>
        /// 인쇄대기 처리일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? src_printW_date { get; set; }
        /// <summary>
        /// 인쇄 처리일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? src_print_date { get; set; }
        /// <summary>
        /// 인쇄 완료일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? src_print_commit_date { get; set; }
        /// <summary>
        /// 제본시작일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? src_jebon_date { get; set; }
        /// <summary>
        /// 제본 완료일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? src_jebon_commit_date { get; set; }
        /// <summary>
        /// 포장 처리일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? src_packing_date { get; set; }
        /// <summary>
        /// 배송대기 처리일(현재 사용안함)
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? src_sendW_date { get; set; }
        /// <summary>
        /// 배송 처리일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? src_send_date { get; set; }
        /// <summary>
        /// 주문 취소일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? src_cancel_date { get; set; }
        /// <summary>
        /// 사용 안함
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? src_mini_print_date { get; set; }
        /// <summary>
        /// 사용 안함
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? src_mini_cut_date { get; set; }
        /// <summary>
        /// 사용 안함
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? src_mini_packing_date { get; set; }
        /// <summary>
        /// ERP전송일
        /// </summary>
        [StringLength(10)]
        [Unicode(false)]
        public string src_erp_date { get; set; }
        /// <summary>
        /// 주문승인 처리자
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string src_ap_admin_id { get; set; }
        /// <summary>
        /// 초안작성자
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string src_compose_admin_id { get; set; }
        /// <summary>
        /// 초안수정자
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string src_compose_mod_admin_id { get; set; }
        /// <summary>
        /// 원고출력자
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string src_PrintCopy_admin_id { get; set; }
        /// <summary>
        /// 인쇄처리자
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string src_print_admin_id { get; set; }
        /// <summary>
        /// 인쇄소(0:내부,1:내부-구분,2:내부-3층,3:학술,4:성원,5:대리점)
        /// </summary>
        public short? src_printer_seq { get; set; }
        /// <summary>
        /// 주문 취소자
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string src_cancel_admin_id { get; set; }
        /// <summary>
        /// 주문 회원 아이디
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string member_id { get; set; }
        /// <summary>
        /// 주문자 이름
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string order_name { get; set; }
        /// <summary>
        /// 주문자 이메일
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string order_email { get; set; }
        /// <summary>
        /// 주문자 전화번호
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string order_phone { get; set; }
        /// <summary>
        /// 주문자 핸드폰 번호
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string order_hphone { get; set; }
        /// <summary>
        /// 주문자팩스번호
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string order_faxphone { get; set; }
        /// <summary>
        /// 주문 요구사항
        /// </summary>
        [StringLength(1000)]
        [Unicode(false)]
        public string order_etc_comment { get; set; }
        /// <summary>
        /// 신랑 이름
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string order_gname { get; set; }
        /// <summary>
        /// 신부 이름
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string order_bname { get; set; }
        public int? card_seq { get; set; }
        /// <summary>
        /// 주문수량
        /// </summary>
        public int? order_count { get; set; }
        /// <summary>
        /// staff몰에서 주문시 직원 이름
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string card_opt { get; set; }
        /// <summary>
        /// 0:수정사항 없는 추가주문,1:수정추가주문
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string order_add_flag { get; set; }
        /// <summary>
        /// 0:셋트주문,2:봉투주문,3:백봉투주문
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string order_add_type { get; set; }
        /// <summary>
        /// 봉투라이닝 (0:없음,1:유료,2:무료)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isLiningJaebon { get; set; }
        /// <summary>
        /// 1:속지부착 서비스 가능
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isinpaper { get; set; }
        /// <summary>
        /// 1:핸드메이드서비스 가능
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string ishandmade { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isRibon { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isEmbo { get; set; }
        /// <summary>
        /// 우선처리도.숫자 높을수록 우선처리.
        /// </summary>
        public byte? ProcLevel { get; set; }
        /// <summary>
        /// 코렐 작성 여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isCompose { get; set; }
        /// <summary>
        /// 지시서 검증 여부(아직 사용안함)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isPrintCopy { get; set; }
        /// <summary>
        /// 포토내지 청첩장 PDF작성 여부(사용안함)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isPDF { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isChoanRisk { get; set; }
        /// <summary>
        /// 1:급초안처리요망
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isBaesongRisk { get; set; }
        /// <summary>
        /// 1:급매송처리요망
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isContAdd { get; set; }
        /// <summary>
        /// 1:카드인쇄판 추가
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isEnvAdd { get; set; }
        /// <summary>
        /// 1:봉투인쇄판추가
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isEnvInsert { get; set; }
        /// <summary>
        /// 1:식권주문
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isFTicket { get; set; }
        /// <summary>
        /// 1:고급내지 옵션선택
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isColorInpaper { get; set; }
        /// <summary>
        /// 0:일반인쇄 / 1:칼라 일반인쇄 / 2:한면 칼라인쇄 /3:양면 칼라인쇄
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isColorPrint { get; set; }
        /// <summary>
        /// 1:현금영수증 발행
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isReceipt { get; set; }
        /// <summary>
        /// 사용 안함.
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isCorel { get; set; }
        /// <summary>
        /// 1:초특급 서비스 요청
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isSpecial { get; set; }
        /// <summary>
        /// 사용쿠폰번호
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string couponseq { get; set; }
        /// <summary>
        /// 기타금액변동 사유
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string etc_price_ment { get; set; }
        /// <summary>
        /// 카드정가합계
        /// </summary>
        public int? order_price { get; set; }
        /// <summary>
        /// 할인율
        /// </summary>
        public double? discount_rate { get; set; }
        /// <summary>
        /// 카드할인가합계(카드 할인가 + env_price)
        /// </summary>
        public int? order_total_price { get; set; }
        /// <summary>
        /// 배송비
        /// </summary>
        public int? delivery_price { get; set; }
        /// <summary>
        /// 제본비
        /// </summary>
        public int? jebon_price { get; set; }
        /// <summary>
        /// 유료스티커
        /// </summary>
        public int? sticker_price { get; set; }
        /// <summary>
        /// 미니청첩장
        /// </summary>
        public int? mini_price { get; set; }
        /// <summary>
        /// 엠보인쇄 비용
        /// </summary>
        public int? embo_price { get; set; }
        /// <summary>
        /// 기타비용
        /// </summary>
        public int? etc_price { get; set; }
        public int? env_price { get; set; }
        public int? guestbook_price { get; set; }
        /// <summary>
        /// 칼라내지 비용
        /// </summary>
        public int? cont_price { get; set; }
        /// <summary>
        /// 인쇄판추가비용
        /// </summary>
        public int? option_price { get; set; }
        /// <summary>
        /// 쿠폰할인금액
        /// </summary>
        public int? reduce_price { get; set; }
        /// <summary>
        /// 식권비용
        /// </summary>
        public int? fticket_price { get; set; }
        /// <summary>
        /// 인쇄비
        /// </summary>
        public int? print_price { get; set; }
        /// <summary>
        /// 사식비
        /// </summary>
        public int? sasik_price { get; set; }
        /// <summary>
        /// 라벨비용
        /// </summary>
        public int? label_price { get; set; }
        public int? envInsert_price { get; set; }
        /// <summary>
        /// 사용 안함
        /// </summary>
        public int? coop_sale_price { get; set; }
        /// <summary>
        /// 최종ERP전송금액
        /// </summary>
        public int? last_total_price { get; set; }
        /// <summary>
        /// 결제일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? settle_date { get; set; }
        /// <summary>
        /// 결제취소일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? settle_cancel_date { get; set; }
        /// <summary>
        /// 결제방법(1:계좌이체,3:무통장,2,6:카드, 8:카카오페이)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string settle_method { get; set; }
        /// <summary>
        /// 결제금액
        /// </summary>
        public int? settle_price { get; set; }
        /// <summary>
        /// PG아이디
        /// </summary>
        [StringLength(500)]
        [Unicode(false)]
        public string pg_shopid { get; set; }
        /// <summary>
        /// 이니시스 TID
        /// </summary>
        [StringLength(500)]
        [Unicode(false)]
        public string pg_tid { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string dacom_tid { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string card_installmonth { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string card_nointyn { get; set; }
        /// <summary>
        /// 현금영수증 이니시스 TID
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string pg_receipt_tid { get; set; }
        /// <summary>
        /// PG결제메세지
        /// </summary>
        [StringLength(1000)]
        [Unicode(false)]
        public string pg_resultinfo { get; set; }
        /// <summary>
        /// 가상계좌일 경우 입금자 이름
        /// </summary>
        [StringLength(1000)]
        [Unicode(false)]
        public string pg_resultinfo2 { get; set; }
        /// <summary>
        /// 사용안함
        /// </summary>
        public byte? pg_status { get; set; }
        /// <summary>
        /// 사용안함
        /// </summary>
        public int? pg_payprice { get; set; }
        /// <summary>
        /// PG수수료
        /// </summary>
        public double? pg_fee { get; set; }
        /// <summary>
        /// .order_seq추가주문이거나 사고건인 경우 원주문의 order_Seq가 된다.
        /// </summary>
        public int? weddinfo_id { get; set; }
        /// <summary>
        /// PG정산일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? pg_paydate { get; set; }
        /// <summary>
        /// PG수금일
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? pg_repaydate { get; set; }
        /// <summary>
        /// PG환불정산일
        /// </summary>
        [StringLength(10)]
        [Unicode(false)]
        public string pg_caldate { get; set; }
        /// <summary>
        /// PG환불수금일
        /// </summary>
        [StringLength(10)]
        [Unicode(false)]
        public string pg_recaldate { get; set; }
        /// <summary>
        /// 포스 전송 여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string PosFlag { get; set; }
        /// <summary>
        /// 포토북 쿠폰
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string PB_Coupon { get; set; }
        /// <summary>
        /// 내지 칼라인쇄여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isColorPrt_card { get; set; }
        /// <summary>
        /// 봉투 칼라인쇄여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isColorPrt_env { get; set; }
        /// <summary>
        /// 유니세스 기부금
        /// </summary>
        public int? unicef_price { get; set; }
        /// <summary>
        /// 에스크로 신청여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isAscrow { get; set; }
        /// <summary>
        /// 칼라내지 인쇄 색상
        /// </summary>
        [StringLength(2)]
        [Unicode(false)]
        public string print_color { get; set; }
        /// <summary>
        /// 기타 관리(현재는 T-map 신청여부)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isVar { get; set; }
        /// <summary>
        /// 동영상 신청여부(프리미어비핸즈)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isWMovie { get; set; }
        /// <summary>
        /// 적립금
        /// </summary>
        public int? point_price { get; set; }
        /// <summary>
        /// 고급봉투 신청여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isEnvSpecial { get; set; }
        /// <summary>
        /// 라이닝제본비용
        /// </summary>
        public int? LiningJaebon_price { get; set; }
        public int? tmap_price { get; set; }
        public int? cancel_type { get; set; }
        [StringLength(200)]
        public string cancel_type_comment { get; set; }
        public int? cancel_user_type { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string discount_in_advance { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? discount_in_advance_reg_date { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? discount_in_advance_cancel_date { get; set; }
        public int? EventIdx { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string inflow_route { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string OUTSOURCING_TYPE { get; set; }
        public int? order_g_seq { get; set; }
        public int? moneyenv_price { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isEnvCharge { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isMoneyEnv { get; set; }
        /// <summary>
        /// 0:사용안함, KOR:한글, ENG:영문
        /// </summary>
        [StringLength(3)]
        public string isLanguage { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? OUTSOURCING_RECEIPT_DATE { get; set; }
        public int? laser_price { get; set; }
        [StringLength(6)]
        [Unicode(false)]
        public string OUTSOURCING_MERGE_TYPE_CODE { get; set; }
        [StringLength(6)]
        [Unicode(false)]
        public string OUTSOURCING_PRINTING_HOUSE_TYPE_CODE { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string inflow_route_settle { get; set; }
        /// <summary>
        /// 중복쿠폰할인액
        /// </summary>
        public int? addition_reduce_price { get; set; }
        /// <summary>
        /// 중복쿠폰번호
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string addition_couponseq { get; set; }
        [StringLength(6)]
        [Unicode(false)]
        public string AUTO_CHOAN_STATUS_CODE { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AUTO_CHOAN_REG_DATE { get; set; }
        [StringLength(6)]
        [Unicode(false)]
        public string AUTO_CHOAN_UPLOAD_CODE { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AUTO_CHOAN_UPLOAD_REG_DATE { get; set; }
        [StringLength(1000)]
        [Unicode(false)]
        public string Trouble_Comment { get; set; }
        public int? MemoryBook_Price { get; set; }
        public int? EnvSpecial_Price { get; set; }
        public int? unit_price { get; set; }
        public int? flower_price { get; set; }
        public int? sealing_sticker_price { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isPerfume { get; set; }
        public int? perfume_price { get; set; }
        public int? AddPrice { get; set; }
        public int? ribbon_price { get; set; }
        public int? paperCover_price { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string WisaFlag { get; set; }
        public int? Mask_Price { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isMiniCard { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isCCG { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string ishandmade2 { get; set; } = null!;
        public int jebon2_price { get; set; }
        public int Pocket_price { get; set; }
        public int? EnvPremium_price { get; set; }
        public int? MaskingTape_price { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string trouble_version { get; set; }
        [StringLength(12)]
        [Unicode(false)]
        public string trouble_type_new { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string trouble_worker { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string PACKING_EXPECTED_DATE { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string PACKING_EXPECTED_CHECK { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string IsThanksCard { get; set; }
    }
}
