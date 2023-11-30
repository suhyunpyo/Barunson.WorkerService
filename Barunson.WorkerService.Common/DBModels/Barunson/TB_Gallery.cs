using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.Barunson
{
    /// <summary>
    /// 갤러리
    /// </summary>
    [Index("Invitation_ID", Name = "IX_TB_Gallery_ID")]
    public partial class TB_Gallery
    {
        /// <summary>
        /// 갤러리_ID
        /// </summary>
        [Key]
        public int Gallery_ID { get; set; }
        /// <summary>
        /// 초대장_ID
        /// </summary>
        public int? Invitation_ID { get; set; }
        /// <summary>
        /// 순서
        /// </summary>
        public int? Sort { get; set; }
        /// <summary>
        /// 이미지_URL
        /// </summary>
        [StringLength(1000)]
        [Unicode(false)]
        public string Image_URL { get; set; }
        /// <summary>
        /// 이미지_높이
        /// </summary>
        public int? Image_Height { get; set; }
        /// <summary>
        /// 이미지_너비
        /// </summary>
        public int? Image_Width { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Regist_User_ID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Regist_DateTime { get; set; }
        [StringLength(15)]
        [Unicode(false)]
        public string Regist_IP { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Update_User_ID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Update_DateTime { get; set; }
        [StringLength(15)]
        [Unicode(false)]
        public string Update_IP { get; set; }
        public long? FileSize { get; set; }

        [StringLength(1000)]
        public string SmallImage_URL { get; set; }
    }
}
