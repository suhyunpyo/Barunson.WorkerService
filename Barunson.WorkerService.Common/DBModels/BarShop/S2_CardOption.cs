using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Barunson.WorkerService.Common.DBModels.BarShop
{
    /// <summary>
    /// 카드옵션정보
    /// </summary>
    public partial class S2_CardOption
    {
        [Key]
        public int Card_Seq { get; set; }
        /// <summary>
        /// 엠보인쇄 (0:없음,1:유료,2:무료)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsEmbo { get; set; }
        /// <summary>
        /// 엠보인쇄칼라(1:기본,2:진회색,3:은색,4:갈색,5:짙은청색,6:자주색)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsEmboColor { get; set; }
        /// <summary>
        /// 엠보인쇄되는 항목(C:카드,P:약도카드,I:내지 등)
        /// </summary>
        [StringLength(10)]
        [Unicode(false)]
        public string embo_print { get; set; }
        /// <summary>
        /// 초특급 가능 여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsQuick { get; set; }
        /// <summary>
        /// 칼라인쇄 (0:없음,1:유료,2:무료)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsColorPrint { get; set; }
        /// <summary>
        /// 부속품제본 (0:없음,1:유료,2:무료)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsHandmade { get; set; }
        /// <summary>
        /// 한지여부(1:일반한지,2:가로형고급한지,3:세로형고급한지)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsHanji { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string IsInPaper { get; set; }
        /// <summary>
        /// 내지 끼우기 (0:없음,1:유료,2:무료)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsJaebon { get; set; }
        /// <summary>
        /// 봉투라이닝 (0:없음,1:유료,2:무료)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsLiningJaebon { get; set; }
        /// <summary>
        /// 봉투삽입 (0:없음,1:유료,2:무료)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsEnvInsert { get; set; }
        /// <summary>
        /// 샘플주문 (0:불가,1:가능)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsSample { get; set; }
        /// <summary>
        /// 겉면인쇄여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsOutsideInitial { get; set; }
        /// <summary>
        /// 인쇄방법(XXX 세자리 캐릭터값이 다음과 같이 주어진다) G:금박,S:은박, B:먹박,0:박없음, 1:유광,0:무광, 1:형압,0:압없음
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string PrintMethod { get; set; }
        /// <summary>
        /// 추가주문 (0:불가,1:가능)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsAdd { get; set; }
        /// <summary>
        /// 주문시 사용자 이미지업로드(0:없음,1:필요)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsUsrImg1 { get; set; }
        /// <summary>
        /// 주문시 사용자 이미지업로드(0:없음,1:필요)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsUsrImg2 { get; set; }
        /// <summary>
        /// 주문시 사용자 이미지업로드(0:없음,1:필요)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsUsrImg3 { get; set; }
        /// <summary>
        /// 주문시 사용자 멘트(0:없음,1:필요)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsUsrComment { get; set; }
        /// <summary>
        /// 스티커제공 (0:없음,1:유료,2:무료)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsSticker { get; set; }
        /// <summary>
        /// 감성내지 (0:없음,1:있음)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string IsSensInpaper { get; set; }
        /// <summary>
        /// 외부업체인쇄되는 항목(C:카드,P:약도카드,I:내지 등)
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string outsourcing_print { get; set; }
        /// <summary>
        /// 셀프초안주문 (0:불가,1:가능)
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isSelfEditor { get; set; }
        /// <summary>
        /// 디지털 인쇄 여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isDigitalColor { get; set; }
        /// <summary>
        /// 디지털 인쇄 칼라색상 종류
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string DigitalColor { get; set; }
        /// <summary>
        /// 고급봉투 가능여부
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isEnvSpecial { get; set; }
        /// <summary>
        /// 카드 디자이너
        /// </summary>
        [StringLength(20)]
        public string isDesigner { get; set; }
        /// <summary>
        /// 테크닉가공 정보
        /// </summary>
        [StringLength(80)]
        public string isTechnic { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isLInitial { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string option_img1 { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string option_img2 { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string IsColorInpaper { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string IsFChoice { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isCustomDColor { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string Master_2Color { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string isFonttype { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isUsrImg_info { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string IsUsrImg4 { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isMoneyEnv { get; set; }
        /// <summary>
        /// 0:사용안함, 1:한글만, 2:영문만, 3:한/영선택
        /// </summary>
        [StringLength(1)]
        [Unicode(false)]
        public string isLanguage { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isLaser { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isFSC { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isJigunamu { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isNewEvent { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isRepinart { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isHappyPrice { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isWongoYN { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isSpringYN { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isStarcard { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isLetterPress { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isNewGubun { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isGroomBrideYN { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isMasterDigital { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isInternalDigital { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isLaserCard { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isstickerspecial { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isPutGiveCard { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isEngWedding { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isHoneyMoon { get; set; }
        [StringLength(200)]
        [Unicode(false)]
        public string isCardOptionColor { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isEnvSpecialPrint { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isEnvDesignType { get; set; }
        [StringLength(200)]
        [Unicode(false)]
        public string isColorOptionCards { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isColorMaster { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isMasterPrintColor { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isGreeting { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isPhrase { get; set; } = null!;
        [StringLength(1)]
        [Unicode(false)]
        public string SpecialAccYN { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string IsSampleEnd { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isMiniCard { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string IsHandmade2 { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string isEnvPremium { get; set; }
    }
}
