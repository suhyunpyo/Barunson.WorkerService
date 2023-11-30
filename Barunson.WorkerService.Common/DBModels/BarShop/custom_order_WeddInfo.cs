using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 카드인쇄정보
    /// </summary>
    public partial class custom_order_WeddInfo
    {
        public int? id { get; set; }
        [Key]
        public int iid { get; set; }
        public int order_seq { get; set; }
        /// <summary>
        /// 폰트타입(0:A type,1:B type,2:C type,3:E type) ,한지카드의 경우 2:가로형,3:세로형
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string ftype { get; set; }
        /// <summary>
        /// 봉투 폰트타입
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string fetype { get; set; }
        /// <summary>
        /// 예식 년
        /// </summary>
        [StringLength(4)]
        [Unicode(false)]
        public string event_year { get; set; }
        /// <summary>
        /// 예식 월
        /// </summary>
        [StringLength(2)]
        [Unicode(false)]
        public string event_month { get; set; }
        /// <summary>
        /// 예식 일
        /// </summary>
        [StringLength(2)]
        [Unicode(false)]
        public string event_Day { get; set; }
        /// <summary>
        /// 예식 요일
        /// </summary>
        [StringLength(50)]
        public string event_weekname { get; set; }
        /// <summary>
        /// 음력표기 여부
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string lunar_yes_or_no { get; set; }
        /// <summary>
        /// 음력일
        /// </summary>
        [StringLength(50)]
        public string lunar_event_Date { get; set; }
        /// <summary>
        /// 예식 오전/오후/낮 표기
        /// </summary>
        [StringLength(20)]
        public string event_ampm { get; set; }
        /// <summary>
        /// 예식 시
        /// </summary>
        [StringLength(2)]
        [Unicode(false)]
        public string event_hour { get; set; }
        /// <summary>
        /// 예식 분
        /// </summary>
        [StringLength(2)]
        [Unicode(false)]
        public string event_minute { get; set; }
        /// <summary>
        /// 예식장 이름
        /// </summary>
        [StringLength(200)]
        public string wedd_name { get; set; }
        /// <summary>
        /// 예식장소
        /// </summary>
        [StringLength(200)]
        public string wedd_place { get; set; }
        /// <summary>
        /// 예식장 주소
        /// </summary>
        [StringLength(1000)]
        public string wedd_addr { get; set; }
        /// <summary>
        /// 예식장 전화번호
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string wedd_phone { get; set; }
        /// <summary>
        /// 약도 전송 방법
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string map_trans_method { get; set; }
        /// <summary>
        /// 바른손 예식장 키값
        /// </summary>
        public int? wedd_idx { get; set; }
        /// <summary>
        /// 바른손 약도 키값
        /// </summary>
        public int? weddimg_idx { get; set; }
        /// <summary>
        /// 사용안함
        /// </summary>
        [StringLength(200)]
        [Unicode(false)]
        public string map_uploadfile { get; set; }
        /// <summary>
        /// 예식장 추가정보
        /// </summary>
        [StringLength(4000)]
        public string map_info { get; set; }
        /// <summary>
        /// 1:약도인쇄안함
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isNotMapPrint { get; set; }
        /// <summary>
        /// 인사말 내용
        /// </summary>
        [StringLength(4000)]
        public string greeting_content { get; set; }
        /// <summary>
        /// 신랑이름
        /// </summary>
        [StringLength(100)]
        public string groom_name { get; set; }
        /// <summary>
        /// 신부이름
        /// </summary>
        [StringLength(100)]
        public string bride_name { get; set; }
        /// <summary>
        /// 스토리러브 청첩장중, 이니셜 신랑 이니셜
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string groom_initial { get; set; }
        /// <summary>
        /// 스토리러브 청첩장중, 이니셜 신부 이니셜
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string bride_initial { get; set; }
        /// <summary>
        /// 미니청첩장용 영문 이름
        /// </summary>
        [StringLength(100)]
        public string groom_name_eng { get; set; }
        /// <summary>
        /// 미니청첩장용 영문 이름
        /// </summary>
        [StringLength(100)]
        public string bride_name_eng { get; set; }
        /// <summary>
        /// 미니청첩장용 영문 성
        /// </summary>
        [StringLength(100)]
        public string groom_Fname_eng { get; set; }
        /// <summary>
        /// 미니청첩장용 영문 성
        /// </summary>
        [StringLength(100)]
        public string bride_Fname_eng { get; set; }
        /// <summary>
        /// 신랑 세례명
        /// </summary>
        [StringLength(50)]
        public string groom_tail { get; set; }
        /// <summary>
        /// 신부 세례명
        /// </summary>
        [StringLength(50)]
        public string bride_tail { get; set; }
        /// <summary>
        /// 신랑 아버지
        /// </summary>
        [StringLength(100)]
        public string groom_father { get; set; }
        /// <summary>
        /// 신랑 어머지
        /// </summary>
        [StringLength(100)]
        public string groom_mother { get; set; }
        /// <summary>
        /// 신랑 관계
        /// </summary>
        [StringLength(50)]
        public string groom_rank { get; set; }
        /// <summary>
        /// 신부 아버지
        /// </summary>
        [StringLength(100)]
        public string bride_father { get; set; }
        /// <summary>
        /// 신부 어머니
        /// </summary>
        [StringLength(100)]
        public string bride_mother { get; set; }
        /// <summary>
        /// 신부 관계
        /// </summary>
        [StringLength(50)]
        public string bride_rank { get; set; }
        /// <summary>
        /// 신랑 성
        /// </summary>
        [StringLength(50)]
        public string groom_fname { get; set; }
        /// <summary>
        /// 신부 성
        /// </summary>
        [StringLength(50)]
        public string bride_fname { get; set; }
        /// <summary>
        /// 신랑 아버지 성
        /// </summary>
        [StringLength(50)]
        public string groom_father_fname { get; set; }
        /// <summary>
        /// 신랑 어머니 성
        /// </summary>
        [StringLength(50)]
        public string groom_mother_fname { get; set; }
        /// <summary>
        /// 신부 아버지 성
        /// </summary>
        [StringLength(50)]
        public string bride_father_fname { get; set; }
        /// <summary>
        /// 신부 어머니 성
        /// </summary>
        [StringLength(50)]
        public string bride_mother_fname { get; set; }
        /// <summary>
        /// 신랑 아버지 세례명
        /// </summary>
        [StringLength(50)]
        public string groom_father_tail { get; set; }
        /// <summary>
        /// 신랑 어머니 세례명
        /// </summary>
        [StringLength(50)]
        public string groom_mother_tail { get; set; }
        /// <summary>
        /// 신부 아버지 세례명
        /// </summary>
        [StringLength(50)]
        public string bride_father_tail { get; set; }
        /// <summary>
        /// 신부 어머니 세례명
        /// </summary>
        [StringLength(50)]
        public string bride_mother_tail { get; set; }
        /// <summary>
        /// 신랑 세례명 표기 여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isgroom_tail { get; set; }
        /// <summary>
        /// 신부 세례명 표기 여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isbride_tail { get; set; }
        /// <summary>
        /// 신랑 아버지 故
        /// </summary>
        [StringLength(50)]
        public string groom_father_header { get; set; }
        /// <summary>
        /// 신랑 어머니 故
        /// </summary>
        [StringLength(50)]
        public string groom_mother_header { get; set; }
        /// <summary>
        /// 신부 아버지 故
        /// </summary>
        [StringLength(50)]
        public string bride_father_header { get; set; }
        /// <summary>
        /// 신부 어머니 故
        /// </summary>
        [StringLength(50)]
        public string bride_mother_header { get; set; }
        /// <summary>
        /// 초대장일 경우 초대인 이름
        /// </summary>
        [StringLength(1000)]
        public string invite_name { get; set; }
        /// <summary>
        /// 기타 요청사항
        /// </summary>
        [StringLength(4000)]
        public string etc_comment { get; set; }
        /// <summary>
        /// 기타 요청사항 첨부파일
        /// </summary>
        [StringLength(500)]
        public string etc_file { get; set; }
        /// <summary>
        /// 포토청첩장일 경우 사용자 이미지
        /// </summary>
        [StringLength(500)]
        public string picture1 { get; set; }
        /// <summary>
        /// 포토청첩장일 경우 사용자 이미지
        /// </summary>
        [StringLength(500)]
        public string picture2 { get; set; }
        /// <summary>
        /// 포토청첩장일 경우 사용자 이미지
        /// </summary>
        [StringLength(500)]
        public string picture3 { get; set; }
        [StringLength(400)]
        public string msg1 { get; set; }
        /// <summary>
        /// 현재는 티맵 키워드 저장
        /// </summary>
        [StringLength(200)]
        public string keyimg { get; set; }
        /// <summary>
        /// 예식일(주문단에서 입력받은 행사일 정보 조합)
        /// </summary>
        [StringLength(100)]
        public string wedd_date { get; set; }
        /// <summary>
        /// 자동초안주문인 경우 사용 약도ID
        /// </summary>
        public int? map_id { get; set; }
        /// <summary>
        /// 자동초안주문인 경우 사용 교통편ID
        /// </summary>
        public int? traffic_id { get; set; }
        [StringLength(400)]
        public string wedd_ename { get; set; }
        [StringLength(500)]
        public string picture4 { get; set; }
        [StringLength(500)]
        public string picture5 { get; set; }
        [StringLength(500)]
        public string picture6 { get; set; }
        [StringLength(500)]
        public string picture7 { get; set; }
        [StringLength(500)]
        public string picture8 { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string groom_initial1 { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string bride_initial1 { get; set; }
        [StringLength(100)]
        public string groom_name_eng1 { get; set; }
        [StringLength(100)]
        public string bride_name_eng1 { get; set; }
        [StringLength(100)]
        public string groom_Fname_eng1 { get; set; }
        [StringLength(100)]
        public string bride_Fname_eng1 { get; set; }
        [StringLength(2)]
        [Unicode(false)]
        public string groom_star { get; set; }
        [StringLength(2)]
        [Unicode(false)]
        public string bride_star { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isNotPlacePrint { get; set; }
        [StringLength(1000)]
        public string wedd_road_Addr { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string addr_gb { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string AddrDirectInd { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string groom_Illustration { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string bride_Illustration { get; set; }
        [StringLength(200)]
        public string worship_title { get; set; }
        [StringLength(4)]
        public string worship_header { get; set; }
        [StringLength(200)]
        public string worship_name { get; set; }
        [StringLength(4000)]
        public string worship_content { get; set; }
        [StringLength(200)]
        public string hymn_title1 { get; set; }
        [StringLength(4000)]
        public string hymn_content1 { get; set; }
        [StringLength(200)]
        public string hymn_title2 { get; set; }
        [StringLength(4000)]
        public string hymn_content2 { get; set; }
        [StringLength(200)]
        public string bible_title { get; set; }
        [StringLength(4000)]
        public string bible_content { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string MiniCard_Opt { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string MiniCard_Contents { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string MiniCard_Contents2 { get; set; }
        [StringLength(4000)]
        public string greeting_content2 { get; set; }
        [StringLength(1000)]
        public string Account_Number { get; set; }
    }
}
