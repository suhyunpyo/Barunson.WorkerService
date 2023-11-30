using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    /// <summary>
    /// 초대장_상세
    /// </summary>
    [Index("Invitation_URL", Name = "NCIDX_Invitation_Detail_URL", IsUnique = true)]
    public partial class TB_Invitation_Detail
    {
        /// <summary>
        /// 초대장_ID
        /// </summary>
        [Key]
        public int Invitation_ID { get; set; }
        /// <summary>
        /// 초대장_URL
        /// </summary>
        [StringLength(100)]
        public string Invitation_URL { get; set; } = null!;
        /// <summary>
        /// 초대장_제목
        /// </summary>
        [StringLength(200)]
        [Unicode(false)]
        public string Invitation_Title { get; set; } = null!;
        /// <summary>
        /// 인사말
        /// </summary>
        [StringLength(1000)]
        public string Greetings { get; set; }
        /// <summary>
        /// 보내는이
        /// </summary>
        [StringLength(100)]
        public string Sender { get; set; }
        /// <summary>
        /// 대표_이미지_URL
        /// </summary>
        [StringLength(1000)]
        [Unicode(false)]
        public string Delegate_Image_URL { get; set; }
        /// <summary>
        /// SNS_이미지_URL
        /// </summary>
        [StringLength(1000)]
        [Unicode(false)]
        public string SNS_Image_URL { get; set; }
        /// <summary>
        /// 방명록_사용_여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string GuestBook_Use_YN { get; set; } = null!;
        /// <summary>
        /// 신랑_명
        /// </summary>
        [StringLength(100)]
        public string Groom_Name { get; set; } = null!;
        /// <summary>
        /// 신랑_영문명
        /// </summary>
        [StringLength(100)]
        public string Groom_EngName { get; set; }
        /// <summary>
        /// 신랑_국제_전화_여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string Groom_Global_Phone_YN { get; set; }
        /// <summary>
        /// 신랑_국제_전화
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string Groom_Global_Phone_Number { get; set; }
        /// <summary>
        /// 신랑_전화
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string Groom_Phone { get; set; }
        /// <summary>
        /// 신부_명
        /// </summary>
        [StringLength(100)]
        public string Bride_Name { get; set; } = null!;
        /// <summary>
        /// 신부_영문명
        /// </summary>
        [StringLength(100)]
        public string Bride_EngName { get; set; }
        /// <summary>
        /// 신부_국제_전화_여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string Bride_Global_Phone_YN { get; set; }
        /// <summary>
        /// 신부_국제_전화_번호
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string Bride_Global_Phone_Number { get; set; }
        /// <summary>
        /// 신부_전화
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string Bride_Phone { get; set; }
        /// <summary>
        /// 혼주_정보_사용_여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string Parents_Information_Use_YN { get; set; }
        /// <summary>
        /// 신랑_혼주1_호칭
        /// </summary>
        [StringLength(50)]
        public string Groom_Parents1_Title { get; set; }
        /// <summary>
        /// 신랑_혼주1_명칭
        /// </summary>
        [StringLength(100)]
        public string Groom_Parents1_Name { get; set; }
        /// <summary>
        /// 신랑_혼주1_국제_전화_번호_여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string Groom_Parents1_Global_Phone_Number_YN { get; set; }
        /// <summary>
        /// 신랑_혼주1_국제_전화_번호
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string Groom_Parents1_Global_Phone_Number { get; set; }
        /// <summary>
        /// 신랑_혼주1_전화
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string Groom_Parents1_Phone { get; set; }
        /// <summary>
        /// 신랑_혼주2_호칭
        /// </summary>
        [StringLength(50)]
        public string Groom_Parents2_Title { get; set; }
        /// <summary>
        /// 신랑_혼주2_명칭
        /// </summary>
        [StringLength(100)]
        public string Groom_Parents2_Name { get; set; }
        /// <summary>
        /// 신랑_혼주2_국제_전화_번호_여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string Groom_Parents2_Global_Phone_Number_YN { get; set; }
        /// <summary>
        /// 신랑_혼주2_국제_전화_번호
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string Groom_Parents2_Global_Phone_Number { get; set; }
        /// <summary>
        /// 신랑_혼주2_전화
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string Groom_Parents2_Phone { get; set; }
        /// <summary>
        /// 신부_혼주1_호칭
        /// </summary>
        [StringLength(50)]
        public string Bride_Parents1_Title { get; set; }
        /// <summary>
        /// 신부_혼주1_명칭
        /// </summary>
        [StringLength(100)]
        public string Bride_Parents1_Name { get; set; }
        /// <summary>
        /// 신부_혼주1_국제_전화_번호_여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string Bride_Parents1_Global_Phone_Number_YN { get; set; }
        /// <summary>
        /// 신부_혼주1_국제_전화_번호
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string Bride_Parents1_Global_Phone_Number { get; set; }
        /// <summary>
        /// 신부_혼주1_전화
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string Bride_Parents1_Phone { get; set; }
        /// <summary>
        /// 신부_혼주2_호칭
        /// </summary>
        [StringLength(50)]
        public string Bride_Parents2_Title { get; set; }
        /// <summary>
        /// 신부_혼주2_명칭
        /// </summary>
        [StringLength(100)]
        public string Bride_Parents2_Name { get; set; }
        /// <summary>
        /// 신부_혼주2_국제_전화_번호_여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string Bride_Parents2_Global_Phone_Number_YN { get; set; }
        /// <summary>
        /// 신부_혼주2_국제_전화_번호
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string Bride_Parents2_Global_Phone_Number { get; set; }
        /// <summary>
        /// 신부_혼주2_전화
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string Bride_Parents2_Phone { get; set; }
        /// <summary>
        /// 예식일자
        /// </summary>
        [StringLength(10)]
        [Unicode(false)]
        public string WeddingDate { get; set; } = null!;
        /// <summary>
        /// 예식시분
        /// </summary>
        [StringLength(4)]
        [Unicode(false)]
        public string WeddingHHmm { get; set; } = null!;
        /// <summary>
        /// 시간_구분_코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Time_Type_Code { get; set; } = null!;
        /// <summary>
        /// 시간_구분_영문_여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string Time_Type_Eng_YN { get; set; }
        /// <summary>
        /// 예식년
        /// </summary>
        [StringLength(4)]
        [Unicode(false)]
        public string WeddingYY { get; set; }
        /// <summary>
        /// 예식월
        /// </summary>
        [StringLength(2)]
        [Unicode(false)]
        public string WeddingMM { get; set; }
        /// <summary>
        /// 예식일
        /// </summary>
        [StringLength(2)]
        [Unicode(false)]
        public string WeddingDD { get; set; }
        /// <summary>
        /// 예식요일
        /// </summary>
        [StringLength(100)]
        public string WeddingWeek { get; set; }
        /// <summary>
        /// 예식요일_영어_여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string WeddingWeek_Eng_YN { get; set; }
        /// <summary>
        /// 예식분
        /// </summary>
        [StringLength(2)]
        [Unicode(false)]
        public string WeddingMin { get; set; }
        /// <summary>
        /// 예식장_명
        /// </summary>
        [StringLength(100)]
        public string Weddinghall_Name { get; set; } = null!;
        /// <summary>
        /// 층홀실
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string WeddingHallDetail { get; set; }
        /// <summary>
        /// 예식장주소
        /// </summary>
        [StringLength(500)]
        public string Weddinghall_Address { get; set; }
        /// <summary>
        /// 예식장_연락처
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string Weddinghall_PhoneNumber { get; set; }
        /// <summary>
        /// 약도_구분_코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Outline_Type_Code { get; set; } = null!;
        /// <summary>
        /// 약도_이미지_URL
        /// </summary>
        [StringLength(1000)]
        [Unicode(false)]
        public string Outline_Image_URL { get; set; }
        /// <summary>
        /// 좌표_LAT
        /// </summary>
        public double? Location_LAT { get; set; }
        /// <summary>
        /// 좌표_LOT
        /// </summary>
        public double? Location_LOT { get; set; }
        /// <summary>
        /// 기타_정보_사용_여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string Etc_Information_Use_YN { get; set; } = null!;
        /// <summary>
        /// 초대_영상_사용_여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string Invitation_Video_Use_YN { get; set; } = null!;
        /// <summary>
        /// 초대_영상_유형_코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Invitation_Video_Type_Code { get; set; }
        /// <summary>
        /// 초대_영상_URL
        /// </summary>
        [StringLength(1000)]
        [Unicode(false)]
        public string Invitation_Video_URL { get; set; }
        /// <summary>
        /// 갤러리_사용_여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string Gallery_Use_YN { get; set; } = null!;
        /// <summary>
        /// 갤러리_유형_코드
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Gallery_Type_Code { get; set; }
        /// <summary>
        /// 축의금_송금_사용_여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string MoneyGift_Remit_Use_YN { get; set; }
        /// <summary>
        /// 등록_사용자_ID
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Regist_User_ID { get; set; }
        /// <summary>
        /// 등록_일시
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Regist_DateTime { get; set; }
        /// <summary>
        /// 등록_IP
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Regist_IP { get; set; }
        /// <summary>
        /// 수정_사용자_ID
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Update_User_ID { get; set; }
        /// <summary>
        /// 수정_일시
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Update_DateTime { get; set; }
        /// <summary>
        /// 수정_IP
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Update_IP { get; set; }
        [StringLength(2)]
        [Unicode(false)]
        public string WeddingHour { get; set; }
        public double? Delegate_Image_Height { get; set; }
        public double? Delegate_Image_Width { get; set; }
        public double? SNS_Image_Height { get; set; }
        public double? SNS_Image_Width { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string MMS_Send_YN { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string Invitation_Display_YN { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string MoneyAccount_Remit_Use_YN { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string MoneyAccount_Div_Use_YN { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string DetailNewLineYN { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string Conf_KaKaoPay_YN { get; set; } = null!;
        [StringLength(1)]
        [Unicode(false)]
        public string Conf_Remit_YN { get; set; } = null!;
        [StringLength(1)]
        [Unicode(false)]
        public string Flower_gift_YN { get; set; } = null!;
        public string ExtendData { get; set; }

    }
}
