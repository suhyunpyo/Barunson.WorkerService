using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    /// <summary>
    /// 송금_정보
    /// </summary>
    [Index("Invitation_ID", Name = "NonClusteredIndex-20211213-154548")]
    public partial class TB_Remit
    {

        /// <summary>
        /// 송금_ID
        /// </summary>
        [Key]
        public int Remit_ID { get; set; }
        /// <summary>
        /// 계좌_ID
        /// </summary>
        public int? Account_ID { get; set; }
        /// <summary>
        /// 쿠폰_주문_ID
        /// </summary>
        public int? Coupon_Order_ID { get; set; }
        /// <summary>
        /// 초대장_ID
        /// </summary>
        public int? Invitation_ID { get; set; }
        /// <summary>
        /// 유니크인덱스 설정 필요
        /// 
        /// [년월일] + [제로필 일련번호5자리]
        /// 2021123100000
        /// </summary>
        [StringLength(100)]
        [Unicode(false)]
        public string Partner_Order_ID { get; set; }
        /// <summary>
        /// 더즌에서 받는 정보
        /// 
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string Transaction_ID { get; set; }
        /// <summary>
        /// 더즌에서 받는 정보
        /// 
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string Transaction_Detail_ID { get; set; }
        /// <summary>
        /// 혼주 예금주로 대응
        /// 
        /// </summary>
        [StringLength(50)]
        public string Item_Name { get; set; }
        /// <summary>
        /// 전체_금액
        /// </summary>
        public int? Total_Price { get; set; }
        /// <summary>
        /// 더즌에서 할당받은 카카오페이 계좌번호
        /// 
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Account_Number { get; set; }
        /// <summary>
        /// 더즌에서 할당 받은 카카오페이 결제 은행코드
        /// 
        /// </summary>
        [StringLength(3)]
        [Unicode(false)]
        public string Bank_Code { get; set; }
        /// <summary>
        /// 송금자_명
        /// </summary>
        [StringLength(50)]
        public string Remitter_Name { get; set; }
        /// <summary>
        /// 결제_토큰
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string Payment_Token { get; set; }
        /// <summary>
        /// R0 : 준비요청
        /// R1 : 준비완료
        /// P2 : 승인요청
        /// P3 : 승인완료
        /// 
        /// RC : 준비취소
        /// RF : 준비실패
        /// PF : 승인실패
        /// 
        /// C0 : 정산 완료
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Result_Code { get; set; }
        /// <summary>
        /// 전송_상태
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string Send_Status { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Status_Code { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Error_Code { get; set; }
        [StringLength(1000)]
        [Unicode(false)]
        public string Error_Message { get; set; }
        /// <summary>
        /// 등록_일시
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Regist_DateTime { get; set; }
        /// <summary>
        /// 준비_일시
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Ready_DateTime { get; set; }
        /// <summary>
        /// 요청_일시
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Request_DateTime { get; set; }
        /// <summary>
        /// 완료_일시
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Complete_DateTime { get; set; }
        [StringLength(8)]
        [Unicode(false)]
        public string Complete_Date { get; set; }

    }
}
