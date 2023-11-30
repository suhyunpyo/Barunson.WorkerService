using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    /// <summary>
    /// 아이템_리소스
    /// </summary>
    public partial class TB_Item_Resource
    {

        /// <summary>
        /// 리소스_ID
        /// </summary>
        [Key]
        public int Resource_ID { get; set; }
        /// <summary>
        /// 신랑명 &amp; 신부명
        /// 
        /// </summary>
        [StringLength(1000)]
        [Unicode(false)]
        public string CharacterSet { get; set; }
        /// <summary>
        /// 문자_크기
        /// </summary>
        public double? Character_Size { get; set; }
        /// <summary>
        /// 색상
        /// </summary>
        [StringLength(25)]
        [Unicode(false)]
        public string Color { get; set; }
        /// <summary>
        /// 배경_색상
        /// </summary>
        [StringLength(25)]
        [Unicode(false)]
        public string Background_Color { get; set; }
        /// <summary>
        /// 굵게_여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string Bold_YN { get; set; }
        /// <summary>
        /// 이탤릭체_여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string Italic_YN { get; set; }
        /// <summary>
        /// 밑줄_여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string Underline_YN { get; set; }
        /// <summary>
        /// 자간
        /// </summary>
        public double? BetweenText { get; set; }
        /// <summary>
        /// 행간
        /// </summary>
        public double? BetweenLine { get; set; }
        /// <summary>
        /// 수직_정렬
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string Vertical_Alignment { get; set; }
        /// <summary>
        /// 수평_정렬
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string Horizontal_Alignment { get; set; }
        /// <summary>
        /// 순서
        /// </summary>
        public int? Sort { get; set; }
        /// <summary>
        /// 폰트
        /// </summary>
        [StringLength(100)]
        [Unicode(false)]
        public string Font { get; set; }
        /// <summary>
        /// 리소스_URL
        /// </summary>
        [StringLength(1000)]
        [Unicode(false)]
        public string Resource_URL { get; set; }
        /// <summary>
        /// 리소스_높이
        /// </summary>
        public double? Resource_Height { get; set; }
        /// <summary>
        /// 리소스_너비
        /// </summary>
        public double? Resource_Width { get; set; }
        /// <summary>
        /// I : 이미지
        /// M : 동영상
        /// T : 텍스트
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Resource_Type_Code { get; set; }
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
        [StringLength(15)]
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
        [StringLength(15)]
        [Unicode(false)]
        public string Update_IP { get; set; }
        public long? FileSize { get; set; }

    }
}
