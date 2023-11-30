using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 혜택배너
    /// </summary>
    public partial class BENEFIT_BANNER
    {
        [Key]
        public int SEQ { get; set; }
        public int? COMPANY_SEQ { get; set; }
        /// <summary>
        /// 타입(L:대메뉴 M:중메뉴 S:소메뉴)별 위치
        /// </summary>
        [StringLength(10)]
        [Unicode(false)]
        public string B_TYPE { get; set; } = null!;
        /// <summary>
        /// 1:진행 2:대기 3:대체
        /// </summary>
        public int B_TYPE_NO { get; set; }
        /// <summary>
        /// 전시유무(Y:전시 N:비전시)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string DISPLAY_YN { get; set; }
        /// <summary>
        /// 이벤트 시작일
        /// </summary>
        [StringLength(10)]
        [Unicode(false)]
        public string EVENT_S_DT { get; set; }
        /// <summary>
        /// 이벤트 종료일
        /// </summary>
        [StringLength(10)]
        [Unicode(false)]
        public string EVENT_E_DT { get; set; }
        /// <summary>
        /// 메인 타이틀(제목)
        /// </summary>
        [StringLength(100)]
        public string MAIN_TITLE { get; set; }
        /// <summary>
        /// 서브 타이틀(내용)
        /// </summary>
        [StringLength(100)]
        public string SUB_TITLE { get; set; }
        /// <summary>
        /// 페이지 연결URL
        /// </summary>
        [StringLength(100)]
        public string PAGE_URL { get; set; }
        /// <summary>
        /// 배너 이미지 경로
        /// </summary>
        [StringLength(255)]
        [Unicode(false)]
        public string B_IMG { get; set; }
        /// <summary>
        /// 배경 컬러코드
        /// </summary>
        [StringLength(10)]
        [Unicode(false)]
        public string B_BACK_COLOR { get; set; }
        /// <summary>
        /// 윙배너 이미지 경로
        /// </summary>
        [StringLength(255)]
        [Unicode(false)]
        public string WING_IMG { get; set; }
        /// <summary>
        /// 윙배너노출
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string WING_YN { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string BAND_YN { get; set; }
        /// <summary>
        /// 새창띄움
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string NEW_BLANK_YN { get; set; }
        /// <summary>
        /// 제휴배너
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string JEHU_YN { get; set; }
        /// <summary>
        /// 기간이 남은 리스트를 삭제한 경우Y
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string DELETE_YN { get; set; }
        /// <summary>
        /// 종료유무(Y:종료 N:미종료)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string END_YN { get; set; }
        /// <summary>
        /// 대체유무(Y:대체 N:미대체)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string REPLACE_YN { get; set; }
        /// <summary>
        /// 상시노출(Y:상시 N)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string ALWAYS_YN { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CREATED_DATE { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string CREATED_UID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UPDATED_DATE { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string UPDATED_UID { get; set; }
    }
}
