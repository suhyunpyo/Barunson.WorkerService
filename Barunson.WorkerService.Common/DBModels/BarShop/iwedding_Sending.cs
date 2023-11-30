using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 제휴사 iwedding 데이터
    /// </summary>
    public partial class iwedding_Sending
    {
        /// <summary>
        /// 아이웨딩에 발송완료 주문정보 넘겨진 주문번호
        /// </summary>
        [Key]
        public int order_seq { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime reg_date { get; set; }
    }
}
