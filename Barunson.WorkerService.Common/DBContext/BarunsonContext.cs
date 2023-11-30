using System;
using Barunson.WorkerService.Common.DBModels.Barunson;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Barunson.WorkerService.Common.DBContext
{
    public partial class BarunsonContext : DbContext
    {
        #region 생성자
        public BarunsonContext()
        {
        }

        public BarunsonContext(DbContextOptions<BarunsonContext> options)
            : base(options)
        {
        }
        #endregion

        #region 프로퍼티
        public virtual DbSet<TB_Common_Code> TB_Common_Code { get; set; } = null!;
        public virtual DbSet<TB_Invitation> TB_Invitation { get; set; } = null!;
        public virtual DbSet<TB_Invitation_Detail> TB_Invitation_Detail { get; set; } = null!;
        public virtual DbSet<TB_Invitation_Item> TB_Invitation_Item { get; set; }
        public virtual DbSet<TB_Item_Resource> TB_Item_Resource { get; set; }
        public virtual DbSet<TB_Order> TB_Order { get; set; } = null!;
        public virtual DbSet<TB_Order_Product> TB_Order_Product { get; set; } = null!;
        public virtual DbSet<TB_Order_PartnerShip> TB_Order_PartnerShip { get; set; } = null!;
        public virtual DbSet<TB_Total_Statistic_Day> TB_Total_Statistic_Day { get; set; } = null!;
        public virtual DbSet<TB_Total_Statistic_Month> TB_Total_Statistic_Month { get; set; } = null!;
        public virtual DbSet<TB_Sales_Statistic_Day> TB_Sales_Statistic_Day { get; set; } = null!;
        public virtual DbSet<TB_Sales_Statistic_Month> TB_Sales_Statistic_Month { get; set; } = null!;
        public virtual DbSet<TB_Payment_Status_Day> TB_Payment_Status_Day { get; set; } = null!;
        public virtual DbSet<TB_Payment_Status_Month> TB_Payment_Status_Month { get; set; } = null!;
        public virtual DbSet<TB_Remit_Statistics_Daily> TB_Remit_Statistics_Daily { get; set; } = null!;
        public virtual DbSet<TB_Remit_Statistics_Monthly> TB_Remit_Statistics_Monthly { get; set; } = null!;
        public virtual DbSet<TB_Company_Tax> TB_Company_Tax { get; set; } = null!;
        public virtual DbSet<TB_Product> TB_Product { get; set; } = null!;
        public virtual DbSet<TB_Depositor_Hits> TB_Depositor_Hits { get; set; } = null!;
        public virtual DbSet<TB_Account> TB_Account { get; set; } = null!;
        public virtual DbSet<TB_Remit> TB_Remit { get; set; } = null!;
        public virtual DbSet<TB_Invitation_Tax> TB_Invitation_Tax { get; set; } = null!;
        public virtual DbSet<TB_Tax> TB_Tax { get; set; } = null!;
        public virtual DbSet<TB_Refund_Info> TB_Refund_Info { get; set; } = null!;
        public virtual DbSet<TB_Coupon> TB_Coupon { get; set; } = null!;
        public virtual DbSet<TB_Coupon_Publish> TB_Coupon_Publish { get; set; } = null!;
        public virtual DbSet<TB_Order_Coupon_Use> TB_Order_Coupon_Use { get; set; } = null!;
        public virtual DbSet<TB_Gallery> TB_Gallery { get; set; }
        public virtual DbSet<TB_Calculate> TB_Calculate { get; set; }
        public virtual DbSet<TB_Daily_Unique> TB_Daily_Unique { get; set; } = null!;
        public virtual DbSet<TB_Account_Setting> TB_Account_Setting { get; set; } = null!;
        #endregion

        #region Create Model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Korean_Wansung_CI_AS");

            modelBuilder.Entity<TB_Daily_Unique>(entity =>
            {
                entity.HasKey(e => new { e.Request_Date, e.Unique_Number });

                entity.Property(e => e.Request_Date).HasComment("요청_일자");

                entity.Property(e => e.Unique_Number)
                    .HasDefaultValueSql("((1))")
                    .HasComment("고유_번호");
            });

            modelBuilder.Entity<TB_Common_Code>(entity =>
            {
                entity.HasKey(e => new { e.Code_Group, e.Code });

                entity.Property(e => e.Code_Group).HasComment("코드_그룹");

                entity.Property(e => e.Code).HasComment("코드");

                entity.Property(e => e.Code_Name).HasComment("코드_명");

                entity.Property(e => e.Sort).HasComment("순서");

            });

            modelBuilder.Entity<TB_Invitation>(entity =>
            {
                entity.Property(e => e.Invitation_ID).HasComment("초대장_ID");

                entity.Property(e => e.Invitation_Display_YN)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength();

                entity.Property(e => e.Order_ID).HasComment("주문_ID");

                entity.Property(e => e.Regist_DateTime).HasComment("등록_일시");

                entity.Property(e => e.Regist_IP).HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID).HasComment("등록_사용자_ID");

                entity.Property(e => e.Template_ID).HasComment("템플릿_ID");

                entity.Property(e => e.Update_DateTime).HasComment("수정_일시");

                entity.Property(e => e.Update_IP).HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID).HasComment("수정_사용자_ID");

                entity.Property(e => e.User_ID).HasComment("사용자_ID");


            });

            modelBuilder.Entity<TB_Invitation_Detail>(entity =>
            {
                entity.Property(e => e.Invitation_ID)
                    .ValueGeneratedNever()
                    .HasComment("초대장_ID");

                entity.Property(e => e.Bride_EngName).HasComment("신부_영문명");

                entity.Property(e => e.Bride_Global_Phone_Number).HasComment("신부_국제_전화_번호");

                entity.Property(e => e.Bride_Global_Phone_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength()
                    .HasComment("신부_국제_전화_여부");

                entity.Property(e => e.Bride_Name).HasComment("신부_명");

                entity.Property(e => e.Bride_Parents1_Global_Phone_Number).HasComment("신부_혼주1_국제_전화_번호");

                entity.Property(e => e.Bride_Parents1_Global_Phone_Number_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength()
                    .HasComment("신부_혼주1_국제_전화_번호_여부");

                entity.Property(e => e.Bride_Parents1_Name).HasComment("신부_혼주1_명칭");

                entity.Property(e => e.Bride_Parents1_Phone).HasComment("신부_혼주1_전화");

                entity.Property(e => e.Bride_Parents1_Title).HasComment("신부_혼주1_호칭");

                entity.Property(e => e.Bride_Parents2_Global_Phone_Number).HasComment("신부_혼주2_국제_전화_번호");

                entity.Property(e => e.Bride_Parents2_Global_Phone_Number_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength()
                    .HasComment("신부_혼주2_국제_전화_번호_여부");

                entity.Property(e => e.Bride_Parents2_Name).HasComment("신부_혼주2_명칭");

                entity.Property(e => e.Bride_Parents2_Phone).HasComment("신부_혼주2_전화");

                entity.Property(e => e.Bride_Parents2_Title).HasComment("신부_혼주2_호칭");

                entity.Property(e => e.Bride_Phone).HasComment("신부_전화");

                entity.Property(e => e.Conf_KaKaoPay_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.Conf_Remit_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.Delegate_Image_URL).HasComment("대표_이미지_URL");

                entity.Property(e => e.DetailNewLineYN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.Etc_Information_Use_YN)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength()
                    .HasComment("기타_정보_사용_여부");

                entity.Property(e => e.Gallery_Type_Code).HasComment("갤러리_유형_코드");

                entity.Property(e => e.Gallery_Use_YN)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength()
                    .HasComment("갤러리_사용_여부");

                entity.Property(e => e.Greetings).HasComment("인사말");

                entity.Property(e => e.Groom_EngName).HasComment("신랑_영문명");

                entity.Property(e => e.Groom_Global_Phone_Number).HasComment("신랑_국제_전화");

                entity.Property(e => e.Groom_Global_Phone_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength()
                    .HasComment("신랑_국제_전화_여부");

                entity.Property(e => e.Groom_Name).HasComment("신랑_명");

                entity.Property(e => e.Groom_Parents1_Global_Phone_Number).HasComment("신랑_혼주1_국제_전화_번호");

                entity.Property(e => e.Groom_Parents1_Global_Phone_Number_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength()
                    .HasComment("신랑_혼주1_국제_전화_번호_여부");

                entity.Property(e => e.Groom_Parents1_Name).HasComment("신랑_혼주1_명칭");

                entity.Property(e => e.Groom_Parents1_Phone).HasComment("신랑_혼주1_전화");

                entity.Property(e => e.Groom_Parents1_Title).HasComment("신랑_혼주1_호칭");

                entity.Property(e => e.Groom_Parents2_Global_Phone_Number).HasComment("신랑_혼주2_국제_전화_번호");

                entity.Property(e => e.Groom_Parents2_Global_Phone_Number_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength()
                    .HasComment("신랑_혼주2_국제_전화_번호_여부");

                entity.Property(e => e.Groom_Parents2_Name).HasComment("신랑_혼주2_명칭");

                entity.Property(e => e.Groom_Parents2_Phone).HasComment("신랑_혼주2_전화");

                entity.Property(e => e.Groom_Parents2_Title).HasComment("신랑_혼주2_호칭");

                entity.Property(e => e.Groom_Phone).HasComment("신랑_전화");

                entity.Property(e => e.GuestBook_Use_YN)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength()
                    .HasComment("방명록_사용_여부");

                entity.Property(e => e.Invitation_Display_YN)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength();

                entity.Property(e => e.Invitation_Title).HasComment("초대장_제목");

                entity.Property(e => e.Invitation_URL).HasComment("초대장_URL");

                entity.Property(e => e.Invitation_Video_Type_Code).HasComment("초대_영상_유형_코드");

                entity.Property(e => e.Invitation_Video_URL).HasComment("초대_영상_URL");

                entity.Property(e => e.Invitation_Video_Use_YN)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength()
                    .HasComment("초대_영상_사용_여부");

                entity.Property(e => e.Location_LAT).HasComment("좌표_LAT");

                entity.Property(e => e.Location_LOT).HasComment("좌표_LOT");

                entity.Property(e => e.MMS_Send_YN).IsFixedLength();

                entity.Property(e => e.MoneyAccount_Div_Use_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.MoneyAccount_Remit_Use_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.MoneyGift_Remit_Use_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength()
                    .HasComment("축의금_송금_사용_여부");

                entity.Property(e => e.Outline_Image_URL).HasComment("약도_이미지_URL");

                entity.Property(e => e.Outline_Type_Code).HasComment("약도_구분_코드");

                entity.Property(e => e.Parents_Information_Use_YN)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength()
                    .HasComment("혼주_정보_사용_여부");

                entity.Property(e => e.Regist_DateTime).HasComment("등록_일시");

                entity.Property(e => e.Regist_IP).HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID).HasComment("등록_사용자_ID");

                entity.Property(e => e.SNS_Image_URL).HasComment("SNS_이미지_URL");

                entity.Property(e => e.Sender).HasComment("보내는이");

                entity.Property(e => e.Time_Type_Code).HasComment("시간_구분_코드");

                entity.Property(e => e.Time_Type_Eng_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength()
                    .HasComment("시간_구분_영문_여부");

                entity.Property(e => e.Update_DateTime).HasComment("수정_일시");

                entity.Property(e => e.Update_IP).HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID).HasComment("수정_사용자_ID");

                entity.Property(e => e.WeddingDD).HasComment("예식일");

                entity.Property(e => e.WeddingDate).HasComment("예식일자");

                entity.Property(e => e.WeddingHHmm).HasComment("예식시분");

                entity.Property(e => e.WeddingHallDetail).HasComment("층홀실");

                entity.Property(e => e.WeddingHour).IsFixedLength();

                entity.Property(e => e.WeddingMM).HasComment("예식월");

                entity.Property(e => e.WeddingMin).HasComment("예식분");

                entity.Property(e => e.WeddingWeek).HasComment("예식요일");

                entity.Property(e => e.WeddingWeek_Eng_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength()
                    .HasComment("예식요일_영어_여부");

                entity.Property(e => e.WeddingYY).HasComment("예식년");

                entity.Property(e => e.Weddinghall_Address).HasComment("예식장주소");

                entity.Property(e => e.Weddinghall_Name).HasComment("예식장_명");

                entity.Property(e => e.Weddinghall_PhoneNumber).HasComment("예식장_연락처");


            });

            modelBuilder.Entity<TB_Order>(entity =>
            {
                entity.Property(e => e.Order_ID).HasComment("주문_ID");

                entity.Property(e => e.Cancel_Time).HasDefaultValueSql("('00')");

                entity.Property(e => e.CashReceipt_Publish_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.CellPhone_Number).HasComment("휴대전화_번호");

                entity.Property(e => e.Coupon_Price).HasComment("쿠폰_금액");

                entity.Property(e => e.Email).HasComment("이메일");

                entity.Property(e => e.Escrow_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.Name).HasComment("이름");

                entity.Property(e => e.Noint_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.Order_Code).HasComment("주문_코드");

                entity.Property(e => e.Order_Price).HasComment("주문_금액");

                entity.Property(e => e.Order_Status_Code).HasComment("주문_상태_코드");

                entity.Property(e => e.PG_ID).HasComment("PG_ID");

                entity.Property(e => e.Payment_Method_Code).HasComment("결제_방법_코드");

                entity.Property(e => e.Payment_Price).HasComment("결제_금액");

                entity.Property(e => e.Payment_Status_Code).HasComment("결제_상태_코드");

                entity.Property(e => e.Previous_Order_ID).HasComment("이전_주문_ID");

                entity.Property(e => e.Regist_DateTime).HasComment("등록_일시");

                entity.Property(e => e.Regist_IP).HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID).HasComment("등록_사용자_ID");

                entity.Property(e => e.Update_DateTime).HasComment("수정_일시");

                entity.Property(e => e.Update_IP).HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID).HasComment("수정_사용자_ID");

                entity.Property(e => e.User_ID).HasComment("사용자_ID");

            });

            modelBuilder.Entity<TB_Order_Product>(entity =>
            {
                entity.HasKey(e => new { e.Order_ID, e.Product_ID });

                entity.Property(e => e.Order_ID).HasComment("주문_ID");

                entity.Property(e => e.Product_ID).HasComment("상품_ID");

                entity.Property(e => e.Item_Count).HasComment("아이템_수량");

                entity.Property(e => e.Item_Price).HasComment("아이템_가격");

                entity.Property(e => e.Product_Type_Code).HasComment("상품_구분_코드");

                entity.Property(e => e.Regist_DateTime).HasComment("등록_일시");

                entity.Property(e => e.Regist_IP).HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID).HasComment("등록_사용자_ID");

                entity.Property(e => e.Total_Price).HasComment("전체_가격");

                entity.Property(e => e.Update_DateTime).HasComment("수정_일시");

                entity.Property(e => e.Update_IP).HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID).HasComment("수정_사용자_ID");

            });

            modelBuilder.Entity<TB_Order_PartnerShip>(entity =>
            {
                entity.HasKey(e => new { e.P_OrderCode, e.P_Id });

                entity.Property(e => e.P_OrderCode).HasComment("파트너사 주문번호");

                entity.Property(e => e.P_Id).HasComment("파트너사 고유ID");

                entity.Property(e => e.Is_Refund).HasComment("최소 여부");

                entity.Property(e => e.Order_ID).HasComment("바른손 주문_ID");

                entity.Property(e => e.P_ExtendData).HasComment("파트너사 확장 데이터");

                entity.Property(e => e.P_OrderDate).HasComment("주문일");

                entity.Property(e => e.P_Order_Name).HasComment("주문자명");

                entity.Property(e => e.P_Order_Phone).HasComment("주문자연락처");

                entity.Property(e => e.P_ProductCode).HasComment("상품코드");

                entity.Property(e => e.P_ProductName).HasComment("상품명");

                entity.Property(e => e.Payment_Method_Code).HasComment("결제_방법_코드");

                entity.Property(e => e.Payment_Price).HasComment("결제_금액");

                entity.Property(e => e.Payment_Status_Code).HasComment("결제_상태_코드");
            });

            modelBuilder.Entity<TB_Total_Statistic_Day>(entity =>
            {
                entity.Property(e => e.ID).HasComment("ID");

                entity.Property(e => e.Cancel_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("취소_수");

                entity.Property(e => e.Cancel_Refund_Price).HasComment("취소_환불_금액");

                entity.Property(e => e.Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("유료_주문_수");

                entity.Property(e => e.Date).HasComment("날짜");

                entity.Property(e => e.Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("무료_주문_수");

                entity.Property(e => e.Memberjoin_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("회원가입_수");

                entity.Property(e => e.Payment_Price).HasComment("결제_금액");

                entity.Property(e => e.Profit_Price).HasComment("순매출_금액");
            });

            modelBuilder.Entity<TB_Total_Statistic_Month>(entity =>
            {
                entity.Property(e => e.ID).HasComment("ID");

                entity.Property(e => e.Cancel_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("취소_수");

                entity.Property(e => e.Cancel_Refund_Price).HasComment("취소_환불_금액");

                entity.Property(e => e.Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("유료_주문_수");

                entity.Property(e => e.Date).HasComment("날짜");

                entity.Property(e => e.Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("무료_주문_수");

                entity.Property(e => e.Memberjoin_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("회원가입_수");

                entity.Property(e => e.Payment_Price).HasComment("결제_금액");

                entity.Property(e => e.Profit_Price).HasComment("순매출_금액");
            });

            modelBuilder.Entity<TB_Sales_Statistic_Day>(entity =>
            {
                entity.Property(e => e.ID).HasComment("ID");

                entity.Property(e => e.Barunn_Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("바른손_유료_주문_수");

                entity.Property(e => e.Barunn_Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("바른손_무료_주문_수");

                entity.Property(e => e.Barunn_Sales_Price).HasComment("바른손_매출_금액");

                entity.Property(e => e.Bhands_Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("비핸즈_유료_주문 _수");

                entity.Property(e => e.Bhands_Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("비핸즈_무료_주문 _수");

                entity.Property(e => e.Bhands_Sales_Price).HasComment("비핸즈_매출_금액");

                entity.Property(e => e.Date).HasComment("날짜");

                entity.Property(e => e.Premier_Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("프리미어_유료_주문_수");

                entity.Property(e => e.Premier_Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("프리미어_무료_주문_수");

                entity.Property(e => e.Premier_Sales_Price).HasComment("프리미어_매출_금액 ");

                entity.Property(e => e.Thecard_Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("더카드_유료_주문_수 ");

                entity.Property(e => e.Thecard_Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("더카드_무료_주문_수 ");

                entity.Property(e => e.Thecard_Sales_Price).HasComment("더카드_매출_금액");

                entity.Property(e => e.Total_Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("합계_유료_주문_수");

                entity.Property(e => e.Total_Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("합계_무료_주문_수");

                entity.Property(e => e.Total_Sales_Price).HasComment("합계_매출_금액");
            });

            modelBuilder.Entity<TB_Sales_Statistic_Month>(entity =>
            {

                entity.Property(e => e.ID).HasComment("ID");

                entity.Property(e => e.Barunn_Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("바른손_유료_주문_수");

                entity.Property(e => e.Barunn_Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("바른손_무료_주문_수");

                entity.Property(e => e.Barunn_Sales_Price).HasComment("바른손_매출_금액");

                entity.Property(e => e.Bhands_Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("비핸즈_유료_주문 _수");

                entity.Property(e => e.Bhands_Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("비핸즈_무료_주문 _수");

                entity.Property(e => e.Bhands_Sales_Price).HasComment("비핸즈_매출_금액");

                entity.Property(e => e.Date).HasComment("날짜");

                entity.Property(e => e.Premier_Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("프리미어_유료_주문_수");

                entity.Property(e => e.Premier_Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("프리미어_무료_주문_수");

                entity.Property(e => e.Premier_Sales_Price).HasComment("프리미어_매출_금액 ");

                entity.Property(e => e.Thecard_Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("더카드_유료_주문_수 ");

                entity.Property(e => e.Thecard_Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("더카드_무료_주문_수 ");

                entity.Property(e => e.Thecard_Sales_Price).HasComment("더카드_매출_금액");

                entity.Property(e => e.Total_Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("합계_유료_주문_수");

                entity.Property(e => e.Total_Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("합계_무료_주문_수");

                entity.Property(e => e.Total_Sales_Price).HasComment("합계_매출_금액");
            });

            modelBuilder.Entity<TB_Payment_Status_Day>(entity =>
            {

                entity.Property(e => e.ID).HasComment("ID");

                entity.Property(e => e.Account_Transfer_Price).HasComment("계좌_이체_금액");

                entity.Property(e => e.Cancel_Refund_Price).HasComment("취소_환불_금액");

                entity.Property(e => e.Card_Payment_Price).HasComment("카드_결제_금액");

                entity.Property(e => e.Date).HasComment("날짜");

                entity.Property(e => e.Etc_Price).HasComment("기타_금액");

                entity.Property(e => e.Profit_Price).HasComment("순매출_금액");

                entity.Property(e => e.Total_Price).HasComment("합계_금액");

                entity.Property(e => e.Virtual_Account_Price).HasComment("가상_계좌_금액");
            });

            modelBuilder.Entity<TB_Payment_Status_Month>(entity =>
            {

                entity.Property(e => e.ID).HasComment("ID");

                entity.Property(e => e.Account_Transfer_Price).HasComment("계좌_이체_금액");

                entity.Property(e => e.Cancel_Refund_Price).HasComment("취소_환불_금액");

                entity.Property(e => e.Card_Payment_Price).HasComment("카드_결제_금액");

                entity.Property(e => e.Date).HasComment("날짜");

                entity.Property(e => e.Etc_Price).HasComment("기타_금액");

                entity.Property(e => e.Profit_Price).HasComment("순매출_금액");

                entity.Property(e => e.Total_Price).HasComment("합계_금액");

                entity.Property(e => e.Virtual_Account_Price).HasComment("가상_계좌_금액");
            });

            modelBuilder.Entity<TB_Product>(entity =>
            {

                entity.Property(e => e.Product_ID).HasComment("상품_ID");

                entity.Property(e => e.Display_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength()
                    .HasComment("진열_여부");

                entity.Property(e => e.Main_Image_URL).HasComment("대표_이미지_URL");

                entity.Property(e => e.Price).HasComment("가격");

                entity.Property(e => e.Product_Brand_Code).HasComment("상품_브랜드_코드");

                entity.Property(e => e.Product_Category_Code).HasComment("청첩장\r\n감사장\r\n포토형\r\n\r\n답례품\r\n");

                entity.Property(e => e.Product_Code).HasComment("상품_코드");

                entity.Property(e => e.Product_Description).HasComment("상품_설명");

                entity.Property(e => e.Product_Name).HasComment("상품_명");

                entity.Property(e => e.SetCard_Display_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.Template_ID).HasComment("템플릿_ID");

            });

            modelBuilder.Entity<TB_Remit_Statistics_Daily>(entity =>
            {

                entity.Property(e => e.Date).HasComment("날짜");

                entity.Property(e => e.Account_Count).HasComment("계좌_수");

                entity.Property(e => e.Calculate_Tax).HasComment("업체_수수료");

                entity.Property(e => e.Hits_Tax).HasComment("조회_수수료");

                entity.Property(e => e.Remit_Count).HasComment("송금_수");

                entity.Property(e => e.Remit_Price).HasComment("송금_금액");

                entity.Property(e => e.Tax).HasComment("수수료");

                entity.Property(e => e.User_Count).HasComment("사용자_수");
            });

            modelBuilder.Entity<TB_Remit_Statistics_Monthly>(entity =>
            {

                entity.Property(e => e.Date).HasComment("날짜");

                entity.Property(e => e.Account_Count).HasComment("계좌_수");

                entity.Property(e => e.Calculate_Tax).HasComment("업체_수수료");

                entity.Property(e => e.Hits_Tax).HasComment("조회_수수료");

                entity.Property(e => e.Remit_Count).HasComment("송금_수");

                entity.Property(e => e.Remit_Price).HasComment("송금_금액");

                entity.Property(e => e.Tax).HasComment("수수료");

                entity.Property(e => e.User_Count).HasComment("사용자_수");
            });

            modelBuilder.Entity<TB_Company_Tax>(entity =>
            {

                entity.Property(e => e.Company_Tax_ID).HasComment("업체_수수료_ID");

                entity.Property(e => e.Apply_Start_Date).HasComment("적용_시작_날짜");

                entity.Property(e => e.Regist_DateTime).HasComment("등록_일시");

                entity.Property(e => e.Regist_User_ID).HasComment("등록_사용자_ID");

                entity.Property(e => e.Remit_Tax).HasComment("수수료_비율");
            });

            modelBuilder.Entity<TB_Depositor_Hits>(entity =>
            {

                entity.Property(e => e.Depositor_Hits_ID).HasComment("예금주_조회_ID");

                entity.Property(e => e.Account_Number).HasComment("계좌_번호");

                entity.Property(e => e.Bank_Code).HasComment("은행_코드");

                entity.Property(e => e.Depositor).HasComment("예금주");

                entity.Property(e => e.Error_Code).HasComment("오류_코드");

                entity.Property(e => e.Error_Message).HasComment("오류_메세지");

                entity.Property(e => e.Request_Date).HasComment("요청_일자");

                entity.Property(e => e.Request_DateTime)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("요청_일시");

                entity.Property(e => e.Request_Result_DateTime).HasComment("요청_결과_일시");

                entity.Property(e => e.Status_Code).HasComment("상태_코드");

                entity.Property(e => e.Trading_Number).HasComment("거래_번호");

                entity.Property(e => e.Unique_Number).HasComment("고유_번호");

                entity.Property(e => e.User_ID).HasComment("사용자_ID");
            });

            modelBuilder.Entity<TB_Account>(entity =>
            {

                entity.Property(e => e.Account_ID).HasComment("모바일초대장에 매핑할 키\r\n");

                entity.Property(e => e.Account_Number).HasComment("계좌_번호");

                entity.Property(e => e.Account_Type_Code).HasComment("신랑\r\n신부\r\n신랑혼주\r\n신부혼주\r\n");

                entity.Property(e => e.Bank_Code).HasComment("금융관리원에서 표준으로 잡는 은행 코드\r\n");

                entity.Property(e => e.Depositor_Name).HasComment("예금주_명");

                entity.Property(e => e.Regist_DateTime).HasComment("등록_일시");

                entity.Property(e => e.User_ID).HasComment("사용자_ID");

            });

            modelBuilder.Entity<TB_Remit>(entity =>
            {

                entity.Property(e => e.Remit_ID).HasComment("송금_ID");

                entity.Property(e => e.Account_ID).HasComment("계좌_ID");

                entity.Property(e => e.Account_Number).HasComment("더즌에서 할당받은 카카오페이 계좌번호\r\n");

                entity.Property(e => e.Bank_Code).HasComment("더즌에서 할당 받은 카카오페이 결제 은행코드\r\n");

                entity.Property(e => e.Complete_DateTime).HasComment("완료_일시");

                entity.Property(e => e.Coupon_Order_ID).HasComment("쿠폰_주문_ID");

                entity.Property(e => e.Invitation_ID).HasComment("초대장_ID");

                entity.Property(e => e.Item_Name).HasComment("혼주 예금주로 대응\r\n");

                entity.Property(e => e.Partner_Order_ID).HasComment("유니크인덱스 설정 필요\r\n\r\n[년월일] + [제로필 일련번호5자리]\r\n2021123100000");

                entity.Property(e => e.Payment_Token).HasComment("결제_토큰");

                entity.Property(e => e.Ready_DateTime).HasComment("준비_일시");

                entity.Property(e => e.Regist_DateTime).HasComment("등록_일시");

                entity.Property(e => e.Remitter_Name).HasComment("송금자_명");

                entity.Property(e => e.Request_DateTime).HasComment("요청_일시");

                entity.Property(e => e.Result_Code).HasComment("R0 : 준비요청\r\nR1 : 준비완료\r\nP2 : 승인요청\r\nP3 : 승인완료\r\n\r\nRC : 준비취소\r\nRF : 준비실패\r\nPF : 승인실패\r\n\r\nC0 : 정산 완료");

                entity.Property(e => e.Send_Status).HasComment("전송_상태");

                entity.Property(e => e.Total_Price).HasComment("전체_금액");

                entity.Property(e => e.Transaction_Detail_ID).HasComment("더즌에서 받는 정보\r\n");

                entity.Property(e => e.Transaction_ID).HasComment("더즌에서 받는 정보\r\n");

            });

            modelBuilder.Entity<TB_Invitation_Tax>(entity =>
            {

                entity.Property(e => e.Invitation_ID)
                    .ValueGeneratedNever()
                    .HasComment("초대장_ID");

                entity.Property(e => e.Regist_DateTime).HasComment("등록_일시");

                entity.Property(e => e.Tax_ID).HasComment("수수료_ID");

            });

            modelBuilder.Entity<TB_Tax>(entity =>
            {

                entity.Property(e => e.Tax_ID).HasComment("수수료_ID");

                entity.Property(e => e.Previous_Tax).HasComment("이전_수수료");

                entity.Property(e => e.Regist_DateTime).HasComment("등록_일시");

                entity.Property(e => e.Regist_User_ID).HasComment("등록_사용자_ID");

                entity.Property(e => e.Tax).HasComment("수수료");
            });

            modelBuilder.Entity<TB_Refund_Info>(entity =>
            {

                entity.Property(e => e.Refund_ID).HasComment("환불_ID");

                entity.Property(e => e.AccountNumber).HasComment("계좌번호");

                entity.Property(e => e.Bank_Type_Code).HasComment("은행_구분_코드");

                entity.Property(e => e.Depositor_Name).HasComment("예금주_명");

                entity.Property(e => e.Order_ID).HasComment("주문_ID");

                entity.Property(e => e.Refund_Content).HasComment("환불_내용");

                entity.Property(e => e.Refund_DateTime).HasComment("환불_일시");

                entity.Property(e => e.Refund_Price).HasComment("환불_금액");

                entity.Property(e => e.Refund_Status_Code).HasComment("환불_상태_코드");

                entity.Property(e => e.Refund_Type_Code).HasComment("환불_유형_코드");

                entity.Property(e => e.Regist_DateTime).HasComment("등록_일시");

                entity.Property(e => e.Regist_IP).HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID).HasComment("등록_사용자_ID");

                entity.Property(e => e.Update_DateTime).HasComment("수정_일시");

                entity.Property(e => e.Update_IP).HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID).HasComment("수정_사용자_ID");

            });

            modelBuilder.Entity<TB_Coupon_Publish>(entity =>
            {

                entity.Property(e => e.Coupon_Publish_ID).HasComment("쿠폰_발행_ID");

                entity.Property(e => e.Coupon_ID).HasComment("쿠폰_ID");

                entity.Property(e => e.Expiration_Date).HasComment("만료_일자");

                entity.Property(e => e.Retrieve_DateTime).HasComment("회수_일시");

                entity.Property(e => e.Use_DateTime).HasComment("사용_일시");

                entity.Property(e => e.Use_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength()
                    .HasComment("사용_여부");

                entity.Property(e => e.User_ID).HasComment("사용자_ID");

            });

            modelBuilder.Entity<TB_Order_Coupon_Use>(entity =>
            {

                entity.Property(e => e.Order_ID)
                    .ValueGeneratedNever()
                    .HasComment("주문_ID");

                entity.Property(e => e.Coupon_Publish_ID).HasComment("쿠폰_발행_ID");

                entity.Property(e => e.Discount_Price).HasComment("할인_금액");

            });

            modelBuilder.Entity<TB_Coupon>(entity =>
            {
                entity.Property(e => e.Coupon_ID).HasComment("쿠폰_ID");

                entity.Property(e => e.Coupon_Apply_Code).HasComment("쿠폰_적용_코드");

                entity.Property(e => e.Coupon_Apply_Product_ID).HasComment("쿠폰_적용_상품_ID");

                entity.Property(e => e.Coupon_Image_URL).HasComment("쿠폰_이미지_URL");

                entity.Property(e => e.Coupon_Name).HasComment("쿠폰_명");

                entity.Property(e => e.Description).HasComment("설명");

                entity.Property(e => e.Discount_Method_Code).HasComment("할인_방식_코드");

                entity.Property(e => e.Discount_Price).HasComment("할인_금액");

                entity.Property(e => e.Discount_Rate).HasComment("할인_율");

                entity.Property(e => e.Period_Method_Code).HasComment("기간_방식_코드");

                entity.Property(e => e.Publish_End_Date).HasComment("발행_종료_일자");

                entity.Property(e => e.Publish_Method_Code).HasComment("발급_방식_코드");

                entity.Property(e => e.Publish_Period_Code).HasComment("발행_기간_코드");

                entity.Property(e => e.Publish_Start_Date).HasComment("발행_시작_일자");

                entity.Property(e => e.Publish_Target_Code).HasComment("발급_대상_코드");

                entity.Property(e => e.Regist_DateTime).HasComment("등록_일시");

                entity.Property(e => e.Standard_Purchase_Price).HasComment("기준_구매_금액");

                entity.Property(e => e.Use_Available_Standard_Code).HasComment("사용_가능_기준_코드");

                entity.Property(e => e.Use_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength()
                    .HasComment("사용_여부");
            });

            modelBuilder.Entity<TB_Gallery>(entity =>
            {

                entity.Property(e => e.Gallery_ID).HasComment("갤러리_ID");

                entity.Property(e => e.Image_Height).HasComment("이미지_높이");

                entity.Property(e => e.Image_URL).HasComment("이미지_URL");

                entity.Property(e => e.Image_Width).HasComment("이미지_너비");

                entity.Property(e => e.Invitation_ID).HasComment("초대장_ID");

                entity.Property(e => e.Sort).HasComment("순서");

            });

            modelBuilder.Entity<TB_Invitation_Item>(entity =>
            {

                entity.Property(e => e.Item_ID).HasComment("아이템_ID");

                entity.Property(e => e.Area_ID).HasComment("영역_ID");

                entity.Property(e => e.Invitation_ID).HasComment("초대장_ID");

                entity.Property(e => e.Item_Type_Code).HasComment("아이템_유형_코드");

                entity.Property(e => e.Location_Left).HasComment("위치_LEFT");

                entity.Property(e => e.Location_Top).HasComment("위치_TOP");

                entity.Property(e => e.Regist_DateTime).HasComment("등록_일시");

                entity.Property(e => e.Regist_IP).HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID).HasComment("등록_사용자_ID");

                entity.Property(e => e.Resource_ID).HasComment("리소스_ID");

                entity.Property(e => e.Size_Height).HasComment("사이즈_높이");

                entity.Property(e => e.Size_Width).HasComment("사이즈_너비");

                entity.Property(e => e.Update_DateTime).HasComment("수정_일시");

                entity.Property(e => e.Update_IP).HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID).HasComment("수정_사용자_ID");

            });

            modelBuilder.Entity<TB_Item_Resource>(entity =>
            {

                entity.Property(e => e.Resource_ID).HasComment("리소스_ID");

                entity.Property(e => e.Background_Color).HasComment("배경_색상");

                entity.Property(e => e.BetweenLine).HasComment("행간");

                entity.Property(e => e.BetweenText).HasComment("자간");

                entity.Property(e => e.Bold_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength()
                    .HasComment("굵게_여부");

                entity.Property(e => e.CharacterSet).HasComment("신랑명 & 신부명\r\n");

                entity.Property(e => e.Character_Size).HasComment("문자_크기");

                entity.Property(e => e.Color).HasComment("색상");

                entity.Property(e => e.Font).HasComment("폰트");

                entity.Property(e => e.Horizontal_Alignment)
                    .IsFixedLength()
                    .HasComment("수평_정렬");

                entity.Property(e => e.Italic_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength()
                    .HasComment("이탤릭체_여부");

                entity.Property(e => e.Regist_DateTime).HasComment("등록_일시");

                entity.Property(e => e.Regist_IP).HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID).HasComment("등록_사용자_ID");

                entity.Property(e => e.Resource_Height).HasComment("리소스_높이");

                entity.Property(e => e.Resource_Type_Code).HasComment("I : 이미지\r\nM : 동영상\r\nT : 텍스트");

                entity.Property(e => e.Resource_URL).HasComment("리소스_URL");

                entity.Property(e => e.Resource_Width).HasComment("리소스_너비");

                entity.Property(e => e.Sort).HasComment("순서");

                entity.Property(e => e.Underline_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength()
                    .HasComment("밑줄_여부");

                entity.Property(e => e.Update_DateTime).HasComment("수정_일시");

                entity.Property(e => e.Update_IP).HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID).HasComment("수정_사용자_ID");

                entity.Property(e => e.Vertical_Alignment)
                    .IsFixedLength()
                    .HasComment("수직_정렬");
            });

            modelBuilder.Entity<TB_Calculate>(entity =>
            {
                entity.Property(e => e.Calculate_ID).HasComment("정산_ID");

                entity.Property(e => e.Calculate_DateTime).HasComment("정산_일시");

                entity.Property(e => e.Error_Code).HasComment("오류_코드");

                entity.Property(e => e.Remit_Account_Number).HasComment("송금_계좌_번호");

                entity.Property(e => e.Remit_Bank_Code).HasComment("송금_은행_코드");

                entity.Property(e => e.Remit_Content).HasComment("송금_내용");

                entity.Property(e => e.Remit_ID).HasComment("송금_ID");

                entity.Property(e => e.Remit_Price).HasComment("송금_금액");

                entity.Property(e => e.Request_Date).HasComment("요청_일자");

                entity.Property(e => e.Request_DateTime).HasComment("요청_일시");

                entity.Property(e => e.Status_Code).HasComment("상태_코드");

                entity.Property(e => e.Trading_Number).HasComment("거래_번호");

                entity.Property(e => e.Unique_Number).HasComment("고유_번호");

            });

            modelBuilder.Entity<TB_Account_Setting>(entity =>
            {
                entity.Property(e => e.Account_Setting_ID).HasComment("계좌_설정_ID");

                entity.Property(e => e.Barunn_Account_Number).HasComment("바른_계좌_번호");

                entity.Property(e => e.Barunn_Bank_Code).HasComment("바른_은행_코드");

                entity.Property(e => e.Kakao_Account_Number).HasComment("카카오_계좌_번호");

                entity.Property(e => e.Kakao_Bank_Code).HasComment("카카오_은행_코드");

                entity.Property(e => e.Regist_DateTime).HasComment("등록_일시");

                entity.Property(e => e.Regist_User_ID).HasComment("등록_사용자_ID");
            });
        }
        #endregion
    }
}
