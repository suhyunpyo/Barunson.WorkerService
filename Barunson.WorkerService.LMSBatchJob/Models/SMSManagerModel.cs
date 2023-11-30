using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barunson.WorkerService.LMSBatchJob.Models
{
    #region 코드 Enum 선언

    /// <summary>
    /// 문자발송 상태 코드
    /// 0.작성중,40: 등록실패, 50.취소, 90. 테스트발송대기, 99. 테스트 발송완료, 100. 발송대기, 105. 발송중, 110.발송완료,  
    /// </summary>
    public enum SMSManagerStatus : byte
    {
        /// <summary>
        /// 작성중
        /// </summary>
        Writing = 0,
        /// <summary>
        /// 등록실패
        /// </summary>
        Fail = 40,

        /// <summary>
        /// 테스트발송대기
        /// </summary>
        WaitingTest = 90,
        /// <summary>
        ///  테스트 발송완료
        /// </summary>
        ComplateTest = 99,
        /// <summary>
        /// 발송대기
        /// </summary>
        WaitingSend = 100,
        /// <summary>
        /// 발송중
        /// </summary>
        Sending = 105,
        /// <summary>
        /// 발송완료
        /// </summary>
        Complated = 110,
        /// <summary>
        /// 취소
        /// </summary>
        Cancel = 150,
    }

    /// <summary>
    /// 문자 발송유형,
    /// 1.즉시발송, 2.예약발송
    /// </summary>
    public enum SMSManagerSendType : byte
    {
        /// <summary>
        /// 즉시발송
        /// </summary>
        Immediately = 1,
        /// <summary>
        /// 예약발송
        /// </summary>
        Reservation = 2,
    }
    /// <summary>
    /// 발송 구분
    /// </summary>
    public enum SMSManagerClass : byte
    {
        /// <summary>
        /// 광고
        /// </summary>
        AD = 1,
        /// <summary>
        /// 정보
        /// </summary>
        Info = 2,
    }
    #endregion
}
