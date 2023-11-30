using Barunson.WorkerService.Common.DBModels.BarShop;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBContext
{
    public partial class BarShopContext : DbContext
    {
        public BarShopContext()
        {
        }

        public BarShopContext(DbContextOptions<BarShopContext> options)
            : base(options)
        {
        }

        #region 프로퍼티
        public virtual DbSet<CASAMIA_DAILY_INFO> CASAMIA_DAILY_INFO { get; set; }
        public virtual DbSet<S2_UserInfo> S2_UserInfo { get; set; }
        public virtual DbSet<VW_USER_INFO> VW_USER_INFO { get; set; }
        public virtual DbSet<custom_order> custom_order { get; set; } = null!;
        public virtual DbSet<Custom_order_Group> Custom_order_Group { get; set; } = null!;
        public virtual DbSet<custom_order_WeddInfo> custom_order_WeddInfo { get; set; } = null!;
        public virtual DbSet<BarunWorkerLog> BarunWorkerLog { get; set; } = null!;
        public virtual DbSet<BarunWorkerTask> BarunWorkerTask { get; set; } = null!;
        public virtual DbSet<BENEFIT_BANNER> BENEFIT_BANNER { get; set; } = null!;
        public virtual DbSet<BestRanking_new> BestRanking_new { get; set; } = null!;
        public virtual DbSet<S2_CardSalesSite> S2_CardSalesSite { get; set; } = null!;
        public virtual DbSet<CUSTOM_SAMPLE_ORDER> CUSTOM_SAMPLE_ORDER { get; set; } = null!;
        public virtual DbSet<Custom_Sample_Order_Statistics> Custom_Sample_Order_Statistics { get; set; }
        public virtual DbSet<CUSTOM_SAMPLE_ORDER_ITEM> CUSTOM_SAMPLE_ORDER_ITEM { get; set; } = null!;
        public virtual DbSet<S2_UserComment> S2_UserComment { get; set; } = null!;
        public virtual DbSet<S2_UserInfo_TheCard> S2_UserInfo_TheCard { get; set; } = null!;
        public virtual DbSet<custom_order_item> custom_order_item { get; set; } = null!;
        public virtual DbSet<S4_COUPON> S4_COUPON { get; set; } = null!;
        public virtual DbSet<S4_MyCoupon> S4_MyCoupon { get; set; } = null!;
        public virtual DbSet<COUPON_DETAIL> COUPON_DETAIL { get; set; } = null!;
        public virtual DbSet<COUPON_ISSUE> COUPON_ISSUE { get; set; } = null!;
        public virtual DbSet<CUSTOM_ETC_ORDER> CUSTOM_ETC_ORDER { get; set; } = null!;
        public virtual DbSet<CUSTOM_ETC_ORDER_ITEM> CUSTOM_ETC_ORDER_ITEM { get; set; } = null!;
        public virtual DbSet<S2_Card> S2_Card { get; set; } = null!;
        public virtual DbSet<manage_code> manage_code { get; set; } = null!;
        public virtual DbSet<gift_company_tel> gift_company_tel { get; set; } = null!;
        public virtual DbSet<ata_mmt_tran> ata_mmt_tran { get; set; } = null!;
        public virtual DbSet<wedd_biztalk> wedd_biztalk { get; set; } = null!;
        public virtual DbSet<CUSTOM_ORDER_ADMIN_MENT> CUSTOM_ORDER_ADMIN_MENT { get; set; } = null!;
        public virtual DbSet<S4_Stock_Alarm> S4_Stock_Alarm { get; set; } = null!;
        public virtual DbSet<KT_DAILY_INFO> KT_DAILY_INFO { get; set; } = null!;
        public virtual DbSet<KT_DAILY_INFO_CANCEL> KT_DAILY_INFO_CANCEL { get; set; } = null!;
        public virtual DbSet<iwedding_Sending> iwedding_Sending { get; set; } = null!;
        public virtual DbSet<S2_CardView> S2_CardView { get; set; } = null!;
        public virtual DbSet<S2_CardOption> S2_CardOption { get; set; } = null!;

        public virtual DbSet<VW_DELIVERY_MST> VW_DELIVERY_MST { get; set; } = null!;
        public virtual DbSet<COMPANY> COMPANY { get; set; } = null!;
        public virtual DbSet<DELIVERY_INFO_DELCODE> DELIVERY_INFO_DELCODE { get; set; } = null!;
        public virtual DbSet<CJ_ONEDAYTOKEN> CJ_ONEDAYTOKEN { get; set; }
        public virtual DbSet<CJ_API_LOG> CJ_API_LOG { get; set; }
        public virtual DbSet<CJ_DELCODE> CJ_DELCODE { get; set; } = null!;

        public virtual DbSet<SendEmailContent> SendEmailContent { get; set; }

        public virtual DbSet<SendEmailContentItem> SendEmailContentItem { get; set; }

        public virtual DbSet<SendEmailMaster> SendEmailMaster { get; set; }

        public virtual DbSet<SendEmailRecipient> SendEmailRecipient { get; set; }

        public virtual DbSet<SMSSendMaster> SMSSendMaster { get; set; }

        public virtual DbSet<SMSSendTargetList> SMSSendTargetList { get; set; }
        public virtual DbSet<COMPETITOR_CARD_MST> COMPETITOR_CARD_MST { get; set; }

        #endregion

        #region Create Model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Korean_Wansung_CI_AS");

            modelBuilder.Entity<CJ_DELCODE>(entity =>
            {
                entity.Property(e => e.DELCODE_SEQ)
                    .ValueGeneratedNever()
                    .HasComment("");

                entity.Property(e => e.CODE).HasComment("송장번호");

                entity.Property(e => e.CODESEQ).HasComment("");

                entity.Property(e => e.ISUSE)
                    .IsFixedLength()
                    .HasComment("사용유무 (0:사용안함, 1:사용완료)");

                entity.Property(e => e.IS_USE).HasComment("");
            });

            modelBuilder.Entity<VW_DELIVERY_MST>(entity =>
            {
                entity.ToView("VW_DELIVERY_MST");

                entity.Property(e => e.ISHJ).IsFixedLength();
            });
            modelBuilder.Entity<COMPANY>(entity =>
            {
                entity.HasKey(e => e.COMPANY_SEQ)
                    .HasName("PK__COMPANY__07220AB2");

                
                entity.HasIndex(e => e.COMPANY_NAME, "IDX_company__name")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.SALES_GUBUN, "NCI_COMPANY_SALES_GUBUN")
                    .HasFillFactor(90);

                entity.Property(e => e.ACCOUNT_NO).HasComment("정산은행 계좌번호");

                entity.Property(e => e.ACC_E_MAIL)
                    .HasDefaultValueSql("('')")
                    .HasComment("정산담당자 이메일");

                entity.Property(e => e.ACC_HP_NO)
                    .HasDefaultValueSql("('Y')")
                    .HasComment("정산담당자 핸드폰 번호");

                entity.Property(e => e.ACC_NM)
                    .HasDefaultValueSql("('')")
                    .HasComment("정산담당자 이름");

                entity.Property(e => e.ACC_TEL_NO)
                    .HasDefaultValueSql("('')")
                    .HasComment("정산담당자 연락처");

                entity.Property(e => e.BACK_ADDR)
                    .HasDefaultValueSql("('')")
                    .HasComment("주소 뒷부분");

                entity.Property(e => e.BANK_NM).HasComment("정산은행이름");

                entity.Property(e => e.BOSS_NM)
                    .HasDefaultValueSql("('')")
                    .HasComment("대표자 이름");

                entity.Property(e => e.BOSS_TEL_NO)
                    .HasDefaultValueSql("('')")
                    .HasComment("대표 번호");

                entity.Property(e => e.CHG_DT)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("최종 변경일");

                entity.Property(e => e.CHG_ID)
                    .HasDefaultValueSql("('')")
                    .HasComment("변경 관리자 아이디");

                entity.Property(e => e.COMPANY_NAME).HasComment("업체명");

                entity.Property(e => e.COMPANY_NUM).HasComment("사업자 번호");

                entity.Property(e => e.CORP_EXP).HasDefaultValueSql("('')");

                entity.Property(e => e.END_DATE).HasComment("마감일");

                entity.Property(e => e.ERP_Dept)
                    .IsFixedLength()
                    .HasComment("1: 영업1본부, 2: 영업2본부");

                entity.Property(e => e.ERP_PGcheck)
                    .IsFixedLength()
                    .HasComment("");

                entity.Property(e => e.ERP_PayLater)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength()
                    .HasComment("Y:후불정산업체");

                entity.Property(e => e.ERP_PriceLevel)
                    .IsFixedLength()
                    .HasComment("1:대리점가,2:출고가,3:소비자가");

                entity.Property(e => e.ERP_TaxType)
                    .IsFixedLength()
                    .HasComment("10 : 일반과세, 22: 매출영수증");

                entity.Property(e => e.E_MAIL)
                    .HasDefaultValueSql("('')")
                    .HasComment("이메일");

                entity.Property(e => e.FAX_NO)
                    .HasDefaultValueSql("('')")
                    .HasComment("업체 팩스번호");

                entity.Property(e => e.FIRST_ALARM).IsFixedLength();

                entity.Property(e => e.FRONT_ADDR)
                    .HasDefaultValueSql("('')")
                    .HasComment("주소 앞부분");

                entity.Property(e => e.IMG_DIR).HasComment("사용안함");

                entity.Property(e => e.INFO_TMP).HasComment("대리점의 경우 업체 URL");

                entity.Property(e => e.INFO_TMP3).HasComment("메인 이미지");

                entity.Property(e => e.INFO_TMP4).HasComment("1일 경우 무료식권 제공");

                entity.Property(e => e.JAEHU_KIND)
                    .HasDefaultValueSql("('W')")
                    .HasComment("W:웹,D:대리점,O:오프영업,M:EC대리점,C:EC 커스터마이징,B:EC B2B,E:e청첩장");

                entity.Property(e => e.JUMUN_TYPE)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("0:기본값(사이트링크),1:결제안함(제휴사매출),2:결제함(바른손매출)");

                entity.Property(e => e.KIND).HasDefaultValueSql("('')");

                entity.Property(e => e.LOGIN_ID)
                    .HasDefaultValueSql("('')")
                    .HasComment("로그인 아이디");

                entity.Property(e => e.MNG_E_MAIL)
                    .HasDefaultValueSql("('')")
                    .HasComment("관리자 이메일");

                entity.Property(e => e.MNG_HP_NO).HasDefaultValueSql("('')");

                entity.Property(e => e.MNG_NM)
                    .HasDefaultValueSql("('')")
                    .HasComment("관리자 이름");

                entity.Property(e => e.MNG_TEL_NO)
                    .HasDefaultValueSql("('')")
                    .HasComment("관리자 연락처");

                entity.Property(e => e.ONOFF)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength()
                    .HasComment("온/오프 제휴");

                entity.Property(e => e.PASSWD)
                    .HasDefaultValueSql("('')")
                    .HasComment("비밀번호");

                entity.Property(e => e.REGIST_DATE)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("등록일");

                entity.Property(e => e.REG_ID)
                    .HasDefaultValueSql("('')")
                    .HasComment("등록 관리자 아이디");

                entity.Property(e => e.SALES_GUBUN)
                    .HasDefaultValueSql("('W')")
                    .HasComment("W:웹,D:대리점,O:오프영업");

                entity.Property(e => e.SASIK_GUBUN).IsFixedLength();

                entity.Property(e => e.START_DATE).HasComment("시작일");

                entity.Property(e => e.STATUS).HasComment("상태 (S1:대기,S2:진행,S3:삭제)");

                entity.Property(e => e.SUPPLY_DISRATE)
                    .HasDefaultValueSql("((0))")
                    .HasComment("기본 할인율");

                entity.Property(e => e.UP_TAE)
                    .HasDefaultValueSql("((22))")
                    .HasComment("업태 (ERP 연동시 tax 타입으로 사용,22:매출영수증,10:매출일반과세)");

                entity.Property(e => e.ZIP_CODE)
                    .HasDefaultValueSql("('')")
                    .HasComment("우편번호");

                entity.Property(e => e.ewed_val).HasDefaultValueSql("((0))");

                entity.Property(e => e.mypage_url).HasComment("고객에게 발송되는 메일에서 링크될 mypage주소");
            });
            modelBuilder.Entity<DELIVERY_INFO_DELCODE>(entity =>
            {
                entity.HasKey(e => new { e.delivery_id, e.delivery_code_num });

                entity.Property(e => e.delivery_code_num).HasComment("송장코드");

                entity.Property(e => e.DELCODE_REG_DATE).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.delivery_com).HasComment("택배사 코드(HJ:한진택배,CJ:CJ택배)");

                entity.Property(e => e.id).ValueGeneratedOnAdd();

                entity.Property(e => e.isHJ)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength()
                    .HasComment("한진택배 전송 여부");
            });
            modelBuilder.Entity<CJ_ONEDAYTOKEN>(entity =>
            {
                entity.HasKey(e => e.TOKEN_NUM);

            });
            modelBuilder.Entity<CJ_API_LOG>(entity =>
            {
                entity.HasKey(e => e.logseq);

            });

            modelBuilder.Entity<BarunWorkerLog>(entity =>
            {
                entity.HasKey(e => e.LogSeq);
            });


            modelBuilder.Entity<BarunWorkerTask>(entity =>
            {
                entity.HasKey(e => new { e.FunctionName, e.WorkerName });
            });

            modelBuilder.Entity<CASAMIA_DAILY_INFO>(entity =>
            {
                entity.HasKey(e => e.seq)
                    .HasName("PK__CASAMIA___DDDFBCBE03B2DCE5");
            });

            modelBuilder.Entity<S2_UserInfo>(entity =>
            {
                entity.HasKey(e => new { e.uid, e.site_div });

                entity.HasIndex(e => e.reg_date, "IDX_S2_UserInfo_Reg_Date")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.hand_phone1, "IDX_hand_phone1")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.hand_phone1, e.hand_phone2, e.hand_phone3 }, "IDX_hand_phone123")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.hand_phone2, "IDX_hand_phone2")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.hand_phone3, "IDX_hand_phone3")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.phone1, "IDX_phone1")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.phone1, e.phone2, e.phone3 }, "IDX_phone123")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.phone2, "IDX_phone2")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.phone3, "IDX_phone3")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.ConnInfo, "NCI_CONNINFO")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.DupInfo, "NCI_DUPINFO")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.REFERER_SALES_GUBUN, "NCI_REFERER_SALES_GUBUN")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.reg_date, "NCI_REG_DATE")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.site_div, "NCI_SITE_DIV")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.uid, e.DupInfo }, "NCI_USERINFO_UID_DUPINFO")
                    .HasFillFactor(90);

                entity.Property(e => e.AuthType).IsFixedLength();

                entity.Property(e => e.DupInfo).IsFixedLength();

                entity.Property(e => e.Gender).IsFixedLength();

                entity.Property(e => e.INTEGRATION_MEMBER_YORN).IsFixedLength();

                entity.Property(e => e.NationalInfo).IsFixedLength();

                entity.Property(e => e.USE_YORN).IsFixedLength();

                entity.Property(e => e.addr_flag).HasDefaultValueSql("((0))");

                entity.Property(e => e.birth_div).IsFixedLength();

                entity.Property(e => e.chk_DM)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength();

                entity.Property(e => e.chk_DormancyAccount).IsFixedLength();

                entity.Property(e => e.chk_iloommembership).IsFixedLength();

                entity.Property(e => e.chk_lgmembership).IsFixedLength();

                entity.Property(e => e.chk_mail_input).IsFixedLength();

                entity.Property(e => e.chk_mailservice).IsFixedLength();

                entity.Property(e => e.chk_myomee).IsFixedLength();

                entity.Property(e => e.chk_smembership)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.chk_smembership_coop).IsFixedLength();

                entity.Property(e => e.chk_smembership_leave).IsFixedLength();

                entity.Property(e => e.chk_smembership_per).IsFixedLength();

                entity.Property(e => e.chk_sms).IsFixedLength();

                entity.Property(e => e.isJehu)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.isMCardAble)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength();

                entity.Property(e => e.is_appSample)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.mkt_chk_flag).IsFixedLength();

                entity.Property(e => e.mod_date).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.reg_date).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.smembership_chk_flag).IsFixedLength();

                entity.Property(e => e.wedd_pgubun).IsFixedLength();
            });

            modelBuilder.Entity<VW_USER_INFO>(entity =>
            {
                entity.ToView("VW_USER_INFO");

                entity.Property(e => e.AuthType).IsFixedLength();

                entity.Property(e => e.BIRTH_DATE_TYPE).IsFixedLength();

                entity.Property(e => e.CHK_MYOMEE).IsFixedLength();

                entity.Property(e => e.CHOICE_AGREEMENT_FOR_SAMSUNG_CHOICE_PERSONAL_DATA).IsFixedLength();

                entity.Property(e => e.CHOICE_AGREEMENT_FOR_SAMSUNG_MEMBERSHIP).IsFixedLength();

                entity.Property(e => e.CHOICE_AGREEMENT_FOR_SAMSUNG_THIRDPARTY).IsFixedLength();

                entity.Property(e => e.DupInfo).IsFixedLength();

                entity.Property(e => e.Gender).IsFixedLength();

                entity.Property(e => e.INTEGRATION_MEMBER_YORN).IsFixedLength();

                entity.Property(e => e.NATIONAL_INFO).IsFixedLength();

                entity.Property(e => e.USE_YORN).IsFixedLength();

                entity.Property(e => e.WEDDING_HALL).IsFixedLength();

                entity.Property(e => e.chk_iloommembership).IsFixedLength();

                entity.Property(e => e.chk_lgmembership).IsFixedLength();

                entity.Property(e => e.chk_mailservice).IsFixedLength();

                entity.Property(e => e.chk_sms).IsFixedLength();

                entity.Property(e => e.isJehu).IsFixedLength();

                entity.Property(e => e.isMCardAble).IsFixedLength();

                entity.Property(e => e.mkt_chk_flag).IsFixedLength();
            });

            modelBuilder.Entity<custom_order>(entity =>
            {
                entity.HasKey(e => e.order_seq)
                    .HasName("PK_custom_order_1")
                    .IsClustered(false);

                entity.HasIndex(e => e.company_seq, "IDX_corder__company_seq")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.member_id, "IDX_corder__member_id")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.src_ap_date, "IDX_corder__order_apdate")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.order_email, "IDX_corder__order_email")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.order_hphone, "IDX_corder__order_hphone")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.order_name, "IDX_corder__order_name")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.order_phone, "IDX_corder__order_phone")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.sales_Gubun, "IDX_corder__sales_gubun")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.settle_date, "IDX_corder__settle_date")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.src_CloseCopy_date, "IDX_corder__src_closecopy_date")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.src_compose_date, "IDX_corder__src_compose_date")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.src_compose_mod_date, "IDX_corder__src_compose_mod_date")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.src_confirm_date, "IDX_corder__src_confirm_date")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.src_jebon_date, "IDX_corder__src_jebon_date")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.src_packing_date, "IDX_corder__src_packing_date")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.src_print_commit_date, "IDX_corder__src_print_commit_date")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.src_print_date, "IDX_corder__src_print_date")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.src_printW_date, "IDX_corder__src_printw_date")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.src_send_date, "IDX_corder__src_send_date")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.up_order_seq, "IDX_corder__up_order_seq")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.order_g_seq, "IDX_orger_g_seq")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.order_count, "NCI_ORDER_COUNT")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.order_type, "NCI_ORDER_TYPE")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.order_date, "clu_order_date")
                    .IsClustered()
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.pg_caldate, e.src_compose_admin_id }, "custom_order60")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.pg_tid, "nind_pg_tid")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.status_seq, e.settle_status }, "nind_status_seq_settle_status")
                    .HasFillFactor(90);

                entity.Property(e => e.order_seq).ValueGeneratedNever();

                entity.Property(e => e.AUTO_CHOAN_STATUS_CODE).HasDefaultValueSql("('138001')");

                entity.Property(e => e.AUTO_CHOAN_UPLOAD_CODE).HasDefaultValueSql("('140001')");

                entity.Property(e => e.EnvPremium_price).HasDefaultValueSql("((0))");

                entity.Property(e => e.EnvSpecial_Price).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsThanksCard)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.LiningJaebon_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("라이닝제본비용");

                entity.Property(e => e.Mask_Price).HasDefaultValueSql("((0))");

                entity.Property(e => e.MaskingTape_price).HasDefaultValueSql("((0))");

                entity.Property(e => e.MemoryBook_Price).HasDefaultValueSql("((0))");

                entity.Property(e => e.PACKING_EXPECTED_CHECK).IsFixedLength();

                entity.Property(e => e.PB_Coupon).HasComment("포토북 쿠폰");

                entity.Property(e => e.PosFlag)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength()
                    .HasComment("포스 전송 여부");

                entity.Property(e => e.ProcLevel)
                    .HasDefaultValueSql("((1))")
                    .HasComment("우선처리도.숫자 높을수록 우선처리.");

                entity.Property(e => e.WisaFlag).IsFixedLength();

                entity.Property(e => e.addition_couponseq).HasComment("중복쿠폰번호");

                entity.Property(e => e.addition_reduce_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("중복쿠폰할인액");

                entity.Property(e => e.cancel_type).HasDefaultValueSql("((0))");

                entity.Property(e => e.card_nointyn).IsFixedLength();

                entity.Property(e => e.card_opt)
                    .HasDefaultValueSql("('')")
                    .HasComment("staff몰에서 주문시 직원 이름");

                entity.Property(e => e.company_seq).HasDefaultValueSql("((1))");

                entity.Property(e => e.cont_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("칼라내지 비용");

                entity.Property(e => e.coop_orderid).HasComment("제휴사측의 주문번호");

                entity.Property(e => e.coop_sale_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("사용 안함");

                entity.Property(e => e.couponseq).HasComment("사용쿠폰번호");

                entity.Property(e => e.delivery_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("배송비");

                entity.Property(e => e.discount_in_advance).IsFixedLength();

                entity.Property(e => e.discount_rate)
                    .HasDefaultValueSql("((0))")
                    .HasComment("할인율");

                entity.Property(e => e.embo_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("엠보인쇄 비용");

                entity.Property(e => e.envInsert_price).HasDefaultValueSql("((0))");

                entity.Property(e => e.env_price).HasDefaultValueSql("((0))");

                entity.Property(e => e.etc_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("기타비용");

                entity.Property(e => e.etc_price_ment).HasComment("기타금액변동 사유");

                entity.Property(e => e.flower_price).HasDefaultValueSql("((0))");

                entity.Property(e => e.fticket_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("식권비용");

                entity.Property(e => e.guestbook_price).HasDefaultValueSql("((0))");

                entity.Property(e => e.isAscrow)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength()
                    .HasComment("에스크로 신청여부");

                entity.Property(e => e.isBaesongRisk)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("1:급초안처리요망");

                entity.Property(e => e.isCCG).IsFixedLength();

                entity.Property(e => e.isChoanRisk)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isColorInpaper)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("1:고급내지 옵션선택");

                entity.Property(e => e.isColorPrint)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("0:일반인쇄 / 1:칼라 일반인쇄 / 2:한면 칼라인쇄 /3:양면 칼라인쇄");

                entity.Property(e => e.isColorPrt_card)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("내지 칼라인쇄여부");

                entity.Property(e => e.isColorPrt_env)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("봉투 칼라인쇄여부");

                entity.Property(e => e.isCompose)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("코렐 작성 여부");

                entity.Property(e => e.isContAdd)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("1:급매송처리요망");

                entity.Property(e => e.isCorel)
                    .HasDefaultValueSql("((1))")
                    .IsFixedLength()
                    .HasComment("사용 안함.");

                entity.Property(e => e.isEmbo)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isEnvAdd)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("1:카드인쇄판 추가");

                entity.Property(e => e.isEnvCharge)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength();

                entity.Property(e => e.isEnvInsert)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("1:봉투인쇄판추가");

                entity.Property(e => e.isEnvSpecial)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength()
                    .HasComment("고급봉투 신청여부");

                entity.Property(e => e.isFTicket)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("1:식권주문");

                entity.Property(e => e.isLanguage).HasComment("0:사용안함, KOR:한글, ENG:영문");

                entity.Property(e => e.isLiningJaebon)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("봉투라이닝 (0:없음,1:유료,2:무료)");

                entity.Property(e => e.isMiniCard)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isMoneyEnv)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isPDF)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("포토내지 청첩장 PDF작성 여부(사용안함)");

                entity.Property(e => e.isPerfume)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isPrintCopy)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("지시서 검증 여부(아직 사용안함)");

                entity.Property(e => e.isReceipt)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("1:현금영수증 발행");

                entity.Property(e => e.isRibon)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isSpecial)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("1:초특급 서비스 요청");

                entity.Property(e => e.isStoreRequisit)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isVar)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength()
                    .HasComment("기타 관리(현재는 T-map 신청여부)");

                entity.Property(e => e.isWMovie)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength()
                    .HasComment("동영상 신청여부(프리미어비핸즈)");

                entity.Property(e => e.ishandmade)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("1:핸드메이드서비스 가능");

                entity.Property(e => e.ishandmade2).HasDefaultValueSql("((0))");

                entity.Property(e => e.isinpaper)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("1:속지부착 서비스 가능");

                entity.Property(e => e.jebon_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("제본비");

                entity.Property(e => e.label_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("라벨비용");

                entity.Property(e => e.laser_price).HasDefaultValueSql("((0))");

                entity.Property(e => e.last_total_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("최종ERP전송금액");

                entity.Property(e => e.member_id).HasComment("주문 회원 아이디");

                entity.Property(e => e.mini_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("미니청첩장");

                entity.Property(e => e.mini_status_seq)
                    .HasDefaultValueSql("((0))")
                    .HasComment("미니청첩장 인쇄 진행 상태");

                entity.Property(e => e.moneyenv_price).HasDefaultValueSql("((0))");

                entity.Property(e => e.option_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("인쇄판추가비용");

                entity.Property(e => e.order_add_flag)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("0:수정사항 없는 추가주문,1:수정추가주문");

                entity.Property(e => e.order_add_type)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("0:셋트주문,2:봉투주문,3:백봉투주문");

                entity.Property(e => e.order_bname).HasComment("신부 이름");

                entity.Property(e => e.order_count).HasComment("주문수량");

                entity.Property(e => e.order_date)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("주문일");

                entity.Property(e => e.order_email).HasComment("주문자 이메일");

                entity.Property(e => e.order_etc_comment).HasComment("주문 요구사항");

                entity.Property(e => e.order_faxphone).HasComment("주문자팩스번호");

                entity.Property(e => e.order_gname).HasComment("신랑 이름");

                entity.Property(e => e.order_hphone).HasComment("주문자 핸드폰 번호");

                entity.Property(e => e.order_name).HasComment("주문자 이름");

                entity.Property(e => e.order_phone).HasComment("주문자 전화번호");

                entity.Property(e => e.order_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("카드정가합계");

                entity.Property(e => e.order_total_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("카드할인가합계(카드 할인가 + env_price)");

                entity.Property(e => e.order_type)
                    .HasDefaultValueSql("((1))")
                    .HasComment("주문타입 (1:청첩장 2:감사장 3:초대장 4,시즌카드 5:미니청첩장 6:포토/디지탈 7:이니셜 8:포토미니)");

                entity.Property(e => e.paperCover_price).HasDefaultValueSql("((0))");

                entity.Property(e => e.pay_Type)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("0:pg 결제,1:주문영업결제,2:제휴사 후불,4:사고건");

                entity.Property(e => e.perfume_price).HasDefaultValueSql("((0))");

                entity.Property(e => e.pg_caldate).HasComment("PG환불정산일");

                entity.Property(e => e.pg_fee)
                    .HasDefaultValueSql("((0))")
                    .HasComment("PG수수료");

                entity.Property(e => e.pg_paydate).HasComment("PG정산일");

                entity.Property(e => e.pg_payprice).HasComment("사용안함");

                entity.Property(e => e.pg_recaldate)
                    .HasDefaultValueSql("((0))")
                    .HasComment("PG환불수금일");

                entity.Property(e => e.pg_receipt_tid).HasComment("현금영수증 이니시스 TID");

                entity.Property(e => e.pg_repaydate).HasComment("PG수금일");

                entity.Property(e => e.pg_resultinfo).HasComment("PG결제메세지");

                entity.Property(e => e.pg_resultinfo2)
                    .HasDefaultValueSql("('')")
                    .HasComment("가상계좌일 경우 입금자 이름");

                entity.Property(e => e.pg_shopid)
                    .HasDefaultValueSql("('barunson2')")
                    .HasComment("PG아이디");

                entity.Property(e => e.pg_status)
                    .HasDefaultValueSql("((0))")
                    .HasComment("사용안함");

                entity.Property(e => e.pg_tid).HasComment("이니시스 TID");

                entity.Property(e => e.point_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("적립금");

                entity.Property(e => e.printW_status)
                    .HasDefaultValueSql("((0))")
                    .HasComment("인쇄 대기 상태");

                entity.Property(e => e.print_color).HasComment("칼라내지 인쇄 색상");

                entity.Property(e => e.print_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("인쇄비");

                entity.Property(e => e.print_type)
                    .HasDefaultValueSql("((0))")
                    .HasComment("이니셜 청첩장의 인쇄타입");

                entity.Property(e => e.reduce_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("쿠폰할인금액");

                entity.Property(e => e.ribbon_price).HasDefaultValueSql("((0))");

                entity.Property(e => e.sales_Gubun)
                    .HasDefaultValueSql("('W')")
                    .HasComment("B:제휴,H:프페 제휴, SA:비핸즈, SS:프페,SB: 바른손, ST:더카드,D:대리점 , P:아웃바운드, Q:지역대리점");

                entity.Property(e => e.sasik_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("사식비");

                entity.Property(e => e.sealing_sticker_price).HasDefaultValueSql("((0))");

                entity.Property(e => e.settle_cancel_date).HasComment("결제취소일");

                entity.Property(e => e.settle_date).HasComment("결제일");

                entity.Property(e => e.settle_method)
                    .IsFixedLength()
                    .HasComment("결제방법(1:계좌이체,3:무통장,2,6:카드, 8:카카오페이)");

                entity.Property(e => e.settle_price).HasComment("결제금액");

                entity.Property(e => e.settle_status)
                    .HasDefaultValueSql("((0))")
                    .HasComment("결제상태 (0:결제이전/ 1:가상계좌입금전/ 2:결제완료/ 3,5:결제취소)");

                entity.Property(e => e.site_gubun)
                    .IsFixedLength()
                    .HasComment("(0:원래의 사이트,1:제휴사,3:제휴사 커스터마이징,4:사고건)");

                entity.Property(e => e.src_CloseCopy_date).HasComment("원고마감 처리일");

                entity.Property(e => e.src_PrintCopy_admin_id).HasComment("원고출력자");

                entity.Property(e => e.src_ap_admin_id).HasComment("주문승인 처리자");

                entity.Property(e => e.src_cancel_admin_id).HasComment("주문 취소자");

                entity.Property(e => e.src_cancel_date).HasComment("주문 취소일");

                entity.Property(e => e.src_compose_admin_id).HasComment("초안작성자");

                entity.Property(e => e.src_compose_date).HasComment("초안작성일");

                entity.Property(e => e.src_compose_mod_admin_id).HasComment("초안수정자");

                entity.Property(e => e.src_compose_mod_date).HasComment("초안 최종 수정일");

                entity.Property(e => e.src_confirm_date).HasComment("초안 확정일");

                entity.Property(e => e.src_erp_date).HasComment("ERP전송일");

                entity.Property(e => e.src_jebon_commit_date).HasComment("제본 완료일");

                entity.Property(e => e.src_jebon_date).HasComment("제본시작일");

                entity.Property(e => e.src_mini_cut_date).HasComment("사용 안함");

                entity.Property(e => e.src_mini_packing_date).HasComment("사용 안함");

                entity.Property(e => e.src_mini_print_date).HasComment("사용 안함");

                entity.Property(e => e.src_modRequest_date).HasComment("초안 최종 수정 요청일");

                entity.Property(e => e.src_packing_date).HasComment("포장 처리일");

                entity.Property(e => e.src_printCopy_date).HasComment("원고출력 처리일");

                entity.Property(e => e.src_printW_date).HasComment("인쇄대기 처리일");

                entity.Property(e => e.src_print_admin_id)
                    .HasDefaultValueSql("('')")
                    .HasComment("인쇄처리자");

                entity.Property(e => e.src_print_commit_date).HasComment("인쇄 완료일");

                entity.Property(e => e.src_print_date).HasComment("인쇄 처리일");

                entity.Property(e => e.src_printer_seq).HasComment("인쇄소(0:내부,1:내부-구분,2:내부-3층,3:학술,4:성원,5:대리점)");

                entity.Property(e => e.src_sendW_date).HasComment("배송대기 처리일(현재 사용안함)");

                entity.Property(e => e.src_send_date).HasComment("배송 처리일");

                entity.Property(e => e.status_seq).HasComment("1:주문 삭제 / 0:주문진행중 / 1:주문완료/ 3:주문취소/ 5:결제취소/ 6:초안수정요청/ 7:초안등록/ 8:초안수정등록/ 9:컨펌완료/ 10:인쇄대기/ 11:인쇄중/ 12:인쇄완료/ 13:제본/ 14:포장/ 15:발송");

                entity.Property(e => e.sticker_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("유료스티커");

                entity.Property(e => e.tmap_price).HasDefaultValueSql("((0))");

                entity.Property(e => e.trouble_type)
                    .HasDefaultValueSql("((0))")
                    .HasComment("사고유형");

                entity.Property(e => e.unicef_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("유니세스 기부금");

                entity.Property(e => e.up_order_seq).HasComment("추가주문일 경우 원 주문번호");

                entity.Property(e => e.weddinfo_id)
                    .HasDefaultValueSql("((0))")
                    .HasComment(".order_seq추가주문이거나 사고건인 경우 원주문의 order_Seq가 된다.");
            });

            modelBuilder.Entity<Custom_order_Group>(entity =>
            {

                entity.HasIndex(e => e.company_seq, "NCI_CUSTOM_ORDER_GROUP_COMPANY_SEQ")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.member_id, "NCI_CUSTOM_ORDER_GROUP_MEMBER_ID")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.member_id, e.company_seq }, "NCI_CUSTOM_ORDER_GROUP_MEMBER_ID_COMPANY_SEQ")
                    .HasFillFactor(90);

                entity.Property(e => e.delivery_price).HasDefaultValueSql("((0))");

                entity.Property(e => e.etc_price).HasDefaultValueSql("((0))");

                entity.Property(e => e.isAscrow).IsFixedLength();

                entity.Property(e => e.isReceipt)
                    .HasDefaultValueSql("('1')")
                    .IsFixedLength();

                entity.Property(e => e.order_date).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.order_price).HasDefaultValueSql("((0))");

                entity.Property(e => e.order_total_price).HasDefaultValueSql("((0))");

                entity.Property(e => e.pay_Type)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.pg_fee).HasDefaultValueSql("((0))");

                entity.Property(e => e.point_price).HasDefaultValueSql("((0))");

                entity.Property(e => e.settle_method).IsFixedLength();

                entity.Property(e => e.settle_price).HasDefaultValueSql("((0))");

                entity.Property(e => e.settle_status).HasDefaultValueSql("((0))");

                entity.Property(e => e.site_gubun)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.status_seq).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<BENEFIT_BANNER>(entity =>
            {
                entity.Property(e => e.ALWAYS_YN)
                    .HasDefaultValueSql("('N')")
                    .HasComment("상시노출(Y:상시 N)");

                entity.Property(e => e.B_BACK_COLOR).HasComment("배경 컬러코드");

                entity.Property(e => e.B_IMG).HasComment("배너 이미지 경로");

                entity.Property(e => e.B_TYPE).HasComment("타입(L:대메뉴 M:중메뉴 S:소메뉴)별 위치");

                entity.Property(e => e.B_TYPE_NO).HasComment("1:진행 2:대기 3:대체");

                entity.Property(e => e.CREATED_DATE).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DELETE_YN)
                    .HasDefaultValueSql("('N')")
                    .HasComment("기간이 남은 리스트를 삭제한 경우Y");

                entity.Property(e => e.DISPLAY_YN)
                    .HasDefaultValueSql("('N')")
                    .HasComment("전시유무(Y:전시 N:비전시)");

                entity.Property(e => e.END_YN)
                    .HasDefaultValueSql("('N')")
                    .HasComment("종료유무(Y:종료 N:미종료)");

                entity.Property(e => e.EVENT_E_DT).HasComment("이벤트 종료일");

                entity.Property(e => e.EVENT_S_DT).HasComment("이벤트 시작일");

                entity.Property(e => e.JEHU_YN).HasComment("제휴배너");

                entity.Property(e => e.MAIN_TITLE).HasComment("메인 타이틀(제목)");

                entity.Property(e => e.NEW_BLANK_YN).HasComment("새창띄움");

                entity.Property(e => e.PAGE_URL).HasComment("페이지 연결URL");

                entity.Property(e => e.REPLACE_YN)
                    .HasDefaultValueSql("('N')")
                    .HasComment("대체유무(Y:대체 N:미대체)");

                entity.Property(e => e.SUB_TITLE).HasComment("서브 타이틀(내용)");

                entity.Property(e => e.WING_IMG).HasComment("윙배너 이미지 경로");

                entity.Property(e => e.WING_YN).HasComment("윙배너노출");
            });

            modelBuilder.Entity<BestRanking_new>(entity =>
            {
                entity.Property(e => e.Gubun).IsFixedLength();

                entity.Property(e => e.RegDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.sales_Gubun).IsFixedLength();
            });

            modelBuilder.Entity<S2_CardSalesSite>(entity =>
            {
                entity.HasKey(e => new { e.card_seq, e.Company_Seq });

                                entity.HasIndex(e => e.Company_Seq, "IDX__company_seq")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.IsDisplay, "IDX__isdisplay")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.Ranking, "IDX__ranking")
                    .HasFillFactor(90);

                entity.Property(e => e.AppSample)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength()
                    .HasComment("추천샘플 제품(고객 샘플 주문시 함께 발송카드)");

                entity.Property(e => e.CardDiscount_Seq).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsBest)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength()
                    .HasComment("베스트 상품");

                entity.Property(e => e.IsDisplay)
                    .HasDefaultValueSql("((1))")
                    .IsFixedLength();

                entity.Property(e => e.IsExtra)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength()
                    .HasComment("초특가제품");

                entity.Property(e => e.IsExtra2)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength()
                    .HasComment("베스트 아이콘(임시)");

                entity.Property(e => e.IsInProduct).IsFixedLength();

                entity.Property(e => e.IsJehyu)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength()
                    .HasComment("제휴카드 여부");

                entity.Property(e => e.IsJumun)
                    .HasDefaultValueSql("((1))")
                    .IsFixedLength()
                    .HasComment("판매여부(1:판매가능,0:판매불가,2:원주문결제/추가주문가능,3:원주문결제만 가능,4:추가주문만 가능");

                entity.Property(e => e.IsNew)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("신상품 여부");

                entity.Property(e => e.IsSale)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength();

                entity.Property(e => e.Ranking_m)
                    .HasDefaultValueSql("((1000))")
                    .HasComment("월 랭킹");

                entity.Property(e => e.Ranking_w)
                    .HasDefaultValueSql("((1000))")
                    .HasComment("주 랭킹");

                entity.Property(e => e.input_date).HasComment("등록일");

                entity.Property(e => e.isDigitalCard).IsFixedLength();

                entity.Property(e => e.isNotCoupon)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength()
                    .HasComment("쿠폰적용 불가 상품");

                entity.Property(e => e.isRecommend).HasDefaultValueSql("((0))");

                entity.Property(e => e.isSSPre).HasDefaultValueSql("((0))");

                entity.Property(e => e.isSummary).HasComment("설명");

                entity.Property(e => e.papercover_groupseq).HasDefaultValueSql("((0))");

                entity.Property(e => e.papercover_seq).HasDefaultValueSql("((0))");

                entity.Property(e => e.pocket_groupseq).HasDefaultValueSql("((0))");

                entity.Property(e => e.pocket_seq).HasDefaultValueSql("((0))");

                entity.Property(e => e.ribbon_groupseq).HasDefaultValueSql("((0))");

                entity.Property(e => e.ribbon_seq).HasDefaultValueSql("((0))");

                entity.Property(e => e.sealingsticker_groupseq).HasDefaultValueSql("((0))");

                entity.Property(e => e.sealingsticker_seq).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<CUSTOM_SAMPLE_ORDER>(entity =>
            {
                entity.HasKey(e => e.sample_order_seq)
                    .HasName("PK__CUSTOM_SAMPLE_OR__1EF99443")
                    .IsClustered(false);

               

                entity.HasIndex(e => new { e.COMPANY_SEQ, e.sample_order_seq, e.STATUS_SEQ }, "CUSTOM_SAMPLE_ORDER67")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.MEMBER_EMAIL, "MEMBER_EMAIL")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.MEMBER_ID, "MEMBER_ID")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.REQUEST_DATE, "NCI_REQUEST_DATE")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.SETTLE_DATE, "clu")
                    .IsClustered()
                    .HasFillFactor(90);

                entity.Property(e => e.sample_order_seq).ValueGeneratedNever();

                entity.Property(e => e.BUY_CONF)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.CANCEL_DATE).HasComment("주문취소일");

                entity.Property(e => e.CANCEL_REASON).HasComment("주문취소 사유");

                entity.Property(e => e.COMPANY_SEQ)
                    .HasDefaultValueSql("((1))")
                    .HasComment("제휴업체");

                entity.Property(e => e.DELIVERY_CHANGO)
                    .HasDefaultValueSql("((1))")
                    .HasComment("배송창고 1:본사출고");

                entity.Property(e => e.DELIVERY_CODE_NUM).HasComment("배송 송장번호");

                entity.Property(e => e.DELIVERY_COM)
                    .IsFixedLength()
                    .HasComment("택배사 코드(CJ:CJ택배)");

                entity.Property(e => e.DELIVERY_DATE).HasComment("배송처리일");

                entity.Property(e => e.DELIVERY_METHOD)
                    .HasDefaultValueSql("((1))")
                    .IsFixedLength()
                    .HasComment("1:택배");

                entity.Property(e => e.DELIVERY_PRICE)
                    .HasDefaultValueSql("((2000))")
                    .HasComment("배송비");

                entity.Property(e => e.DSP_PRINT_YORN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.INVOICE_PRINT_YORN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.ISDACOM)
                    .HasDefaultValueSql("((1))")
                    .IsFixedLength()
                    .HasComment("영수증 발행여부");

                entity.Property(e => e.JOB_ORDER_PRINT_YORN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.MEMBER_ADDRESS).HasComment("수취인 주소");

                entity.Property(e => e.MEMBER_ADDRESS_DETAIL).HasComment("수취인 상세주소");

                entity.Property(e => e.MEMBER_EMAIL).HasComment("주문자 이메일");

                entity.Property(e => e.MEMBER_FAX).HasComment("사용안함");

                entity.Property(e => e.MEMBER_HPHONE).HasComment("주문자 핸드폰번호");

                entity.Property(e => e.MEMBER_ID).HasComment("회원ID");

                entity.Property(e => e.MEMBER_NAME).HasComment("주문자 이름");

                entity.Property(e => e.MEMBER_PHONE).HasComment("주문자 전화번호");

                entity.Property(e => e.MEMBER_ZIP).HasComment("수취인 우편번호");

                entity.Property(e => e.MEMO).HasComment("배송메모");

                entity.Property(e => e.MULTI_PACK_REG_DATE).HasComment("묶음배송 등록일");

                entity.Property(e => e.MULTI_PACK_SEQ).HasComment("묶음배송 seq");

                entity.Property(e => e.MULTI_PACK_SUB_SEQ).HasComment("묶음배송 건수 (1,2)");

                entity.Property(e => e.PG_CALDATE).HasComment("PG수금일");

                entity.Property(e => e.PG_FEE)
                    .HasDefaultValueSql("((0))")
                    .HasComment("PG수수료");

                entity.Property(e => e.PG_MERTID).HasComment("PG상점 아이디");

                entity.Property(e => e.PG_PAYDATE).HasComment("PG정산일");

                entity.Property(e => e.PG_RECALDATE).HasComment("PG환불수금일");

                entity.Property(e => e.PG_REFEE).HasComment("pG환불 수수료");

                entity.Property(e => e.PG_REPAYDATE).HasComment("PG환불정산일");

                entity.Property(e => e.PG_RESULTINFO).HasComment("PG결제결과");

                entity.Property(e => e.PG_RESULTINFO2).HasComment("PG결제결과 (입금자 이름)");

                entity.Property(e => e.PG_TID).HasComment("이니시스 TID");

                entity.Property(e => e.PREPARE_DATE).HasComment("카드준비일");

                entity.Property(e => e.REDUCE_PRICE).HasDefaultValueSql("((0))");

                entity.Property(e => e.REQUEST_DATE).HasComment("주문일");

                entity.Property(e => e.SALES_GUBUN)
                    .HasDefaultValueSql("('W')")
                    .HasComment("manage_code.sales_gubun");

                entity.Property(e => e.SETTLE_CANCEL)
                    .IsFixedLength()
                    .HasComment("결제취소여부(사용안함)");

                entity.Property(e => e.SETTLE_DATE).HasComment("결제일");

                entity.Property(e => e.SETTLE_HPHONE).HasComment("결제 핸드폰");

                entity.Property(e => e.SETTLE_METHOD)
                    .IsFixedLength()
                    .HasComment("결제방법");

                entity.Property(e => e.SETTLE_MOBILID)
                    .IsFixedLength()
                    .HasComment("사용안함");

                entity.Property(e => e.SETTLE_PRICE).HasComment("결제금액");

                entity.Property(e => e.SRC_ERP_DATE).HasComment("ERP전송일");

                entity.Property(e => e.STATUS_SEQ).HasComment("주문상태 1:주문완료(결제전),3:결제완료,10:카드준비중,12:발송완료");

                entity.Property(e => e.WEDD_DATE).HasComment("예식일");

                entity.Property(e => e.WisaFlag).IsFixedLength();

                entity.Property(e => e.card_nointyn).IsFixedLength();

                entity.Property(e => e.isAscrow)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength()
                    .HasComment("에스크로 거래건 여부");

                entity.Property(e => e.isHJ)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength()
                    .HasComment("한진택배 EDI 전송여부");

                entity.Property(e => e.isOneClickSample).IsFixedLength();

                entity.Property(e => e.isVar)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength();

                entity.Property(e => e.join_division)
                    .HasDefaultValueSql("('Web')")
                    .HasComment("mobile,Web 구분");

                entity.Property(e => e.order_g_seq).HasComment("더카드 그룹 seq");
            });

            modelBuilder.Entity<CUSTOM_SAMPLE_ORDER_ITEM>(entity =>
            {
                entity.HasKey(e => new { e.SAMPLE_ORDER_SEQ, e.CARD_SEQ })
                    .HasName("PK__CUSTOM_SAMPLE_OR__540C7B00");



                entity.Property(e => e.CARD_PRICE).HasComment("샘플 판매가");

                entity.Property(e => e.REG_DATE)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("주문일");

                entity.Property(e => e.isChu)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength();

                entity.Property(e => e.md_recommend)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.HasOne(d => d.SAMPLE_ORDER_SEQNavigation)
                    .WithMany(p => p.CUSTOM_SAMPLE_ORDER_ITEM)
                    .HasForeignKey(d => d.SAMPLE_ORDER_SEQ)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CUSTOM_SA__SAMPL__668030F6");
            });

            modelBuilder.Entity<S2_UserComment>(entity =>
            {
                entity.HasKey(e => e.seq)
                    .HasName("PK_CIDX_S2_UserComment__seq");


                entity.HasIndex(e => e.card_code, "IDX__card_code")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.card_seq, "IDX__card_seq")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.company_seq, "IDX__company_seq")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.order_seq, "IDX__order_seq")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.sales_gubun, "IDX__sales_gubun")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.reg_date, "NCI_REG_DATE")
                    .HasFillFactor(90);

                entity.Property(e => e.EVENT_STATUS_CODE)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.comm_div).IsFixedLength();

                entity.Property(e => e.edit_date).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.isBest)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength()
                    .HasComment(" 0:심사중 , 1:당첨축하 , 3:승인보류");

                entity.Property(e => e.isDP)
                    .HasDefaultValueSql("('1')")
                    .IsFixedLength()
                    .HasComment("전시여부");

                entity.Property(e => e.reg_date).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.score).HasComment("별점");
            });

            modelBuilder.Entity<custom_order_WeddInfo>(entity =>
            {
                entity.HasKey(e => e.iid)
                    .IsClustered(false);


                entity.HasIndex(e => e.weddimg_idx, "NCI-WEDDIMG_IDX")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.wedd_idx, "NCI-WEDD_IDX")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.wedd_date, "NCI_WEDD_DATE")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.order_seq, "custom_order_WeddInfo3")
                    .HasFillFactor(90);

                entity.Property(e => e.MiniCard_Contents).HasDefaultValueSql("('')");

                entity.Property(e => e.MiniCard_Opt).IsFixedLength();

                entity.Property(e => e.bride_Fname_eng).HasComment("미니청첩장용 영문 성");

                entity.Property(e => e.bride_father).HasComment("신부 아버지");

                entity.Property(e => e.bride_father_fname).HasComment("신부 아버지 성");

                entity.Property(e => e.bride_father_header).HasComment("신부 아버지 故");

                entity.Property(e => e.bride_father_tail).HasComment("신부 아버지 세례명");

                entity.Property(e => e.bride_fname).HasComment("신부 성");

                entity.Property(e => e.bride_initial)
                    .IsFixedLength()
                    .HasComment("스토리러브 청첩장중, 이니셜 신부 이니셜");

                entity.Property(e => e.bride_initial1).IsFixedLength();

                entity.Property(e => e.bride_mother).HasComment("신부 어머니");

                entity.Property(e => e.bride_mother_fname).HasComment("신부 어머니 성");

                entity.Property(e => e.bride_mother_header).HasComment("신부 어머니 故");

                entity.Property(e => e.bride_mother_tail).HasComment("신부 어머니 세례명");

                entity.Property(e => e.bride_name).HasComment("신부이름");

                entity.Property(e => e.bride_name_eng).HasComment("미니청첩장용 영문 이름");

                entity.Property(e => e.bride_rank).HasComment("신부 관계");

                entity.Property(e => e.bride_star).IsFixedLength();

                entity.Property(e => e.bride_tail).HasComment("신부 세례명");

                entity.Property(e => e.etc_comment).HasComment("기타 요청사항");

                entity.Property(e => e.etc_file).HasComment("기타 요청사항 첨부파일");

                entity.Property(e => e.event_Day).HasComment("예식 일");

                entity.Property(e => e.event_ampm).HasComment("예식 오전/오후/낮 표기");

                entity.Property(e => e.event_hour).HasComment("예식 시");

                entity.Property(e => e.event_minute).HasComment("예식 분");

                entity.Property(e => e.event_month).HasComment("예식 월");

                entity.Property(e => e.event_weekname).HasComment("예식 요일");

                entity.Property(e => e.event_year).HasComment("예식 년");

                entity.Property(e => e.fetype)
                    .HasDefaultValueSql("(0)")
                    .IsFixedLength()
                    .HasComment("봉투 폰트타입");

                entity.Property(e => e.ftype)
                    .HasDefaultValueSql("(0)")
                    .IsFixedLength()
                    .HasComment("폰트타입(0:A type,1:B type,2:C type,3:E type) ,한지카드의 경우 2:가로형,3:세로형");

                entity.Property(e => e.greeting_content).HasComment("인사말 내용");

                entity.Property(e => e.greeting_content2).HasDefaultValueSql("('')");

                entity.Property(e => e.groom_Fname_eng).HasComment("미니청첩장용 영문 성");

                entity.Property(e => e.groom_father).HasComment("신랑 아버지");

                entity.Property(e => e.groom_father_fname).HasComment("신랑 아버지 성");

                entity.Property(e => e.groom_father_header).HasComment("신랑 아버지 故");

                entity.Property(e => e.groom_father_tail).HasComment("신랑 아버지 세례명");

                entity.Property(e => e.groom_fname).HasComment("신랑 성");

                entity.Property(e => e.groom_initial)
                    .IsFixedLength()
                    .HasComment("스토리러브 청첩장중, 이니셜 신랑 이니셜");

                entity.Property(e => e.groom_initial1).IsFixedLength();

                entity.Property(e => e.groom_mother).HasComment("신랑 어머지");

                entity.Property(e => e.groom_mother_fname).HasComment("신랑 어머니 성");

                entity.Property(e => e.groom_mother_header).HasComment("신랑 어머니 故");

                entity.Property(e => e.groom_mother_tail).HasComment("신랑 어머니 세례명");

                entity.Property(e => e.groom_name).HasComment("신랑이름");

                entity.Property(e => e.groom_name_eng).HasComment("미니청첩장용 영문 이름");

                entity.Property(e => e.groom_rank).HasComment("신랑 관계");

                entity.Property(e => e.groom_star).IsFixedLength();

                entity.Property(e => e.groom_tail).HasComment("신랑 세례명");

                entity.Property(e => e.invite_name).HasComment("초대장일 경우 초대인 이름");

                entity.Property(e => e.isNotMapPrint)
                    .HasDefaultValueSql("(0)")
                    .IsFixedLength()
                    .HasComment("1:약도인쇄안함");

                entity.Property(e => e.isbride_tail)
                    .IsFixedLength()
                    .HasComment("신부 세례명 표기 여부");

                entity.Property(e => e.isgroom_tail)
                    .IsFixedLength()
                    .HasComment("신랑 세례명 표기 여부");

                entity.Property(e => e.keyimg).HasComment("현재는 티맵 키워드 저장");

                entity.Property(e => e.lunar_event_Date).HasComment("음력일");

                entity.Property(e => e.lunar_yes_or_no).HasComment("음력표기 여부");

                entity.Property(e => e.map_id).HasComment("자동초안주문인 경우 사용 약도ID");

                entity.Property(e => e.map_info).HasComment("예식장 추가정보");

                entity.Property(e => e.map_trans_method)
                    .HasDefaultValueSql("(0)")
                    .IsFixedLength()
                    .HasComment("약도 전송 방법");

                entity.Property(e => e.map_uploadfile).HasComment("사용안함");

                entity.Property(e => e.picture1).HasComment("포토청첩장일 경우 사용자 이미지");

                entity.Property(e => e.picture2).HasComment("포토청첩장일 경우 사용자 이미지");

                entity.Property(e => e.picture3).HasComment("포토청첩장일 경우 사용자 이미지");

                entity.Property(e => e.traffic_id).HasComment("자동초안주문인 경우 사용 교통편ID");

                entity.Property(e => e.wedd_addr).HasComment("예식장 주소");

                entity.Property(e => e.wedd_date).HasComment("예식일(주문단에서 입력받은 행사일 정보 조합)");

                entity.Property(e => e.wedd_idx).HasComment("바른손 예식장 키값");

                entity.Property(e => e.wedd_name).HasComment("예식장 이름");

                entity.Property(e => e.wedd_phone).HasComment("예식장 전화번호");

                entity.Property(e => e.wedd_place).HasComment("예식장소");

                entity.Property(e => e.weddimg_idx).HasComment("바른손 약도 키값");
            });

            modelBuilder.Entity<custom_order_item>(entity =>
            {
                entity.HasIndex(e => e.card_seq, "IDX_iorder__card_seq")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.item_count, "IDX_iorder__item_count")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.item_type, "IDX_iorder__item_type")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.order_seq, "IDX_iorder__order_seq")
                    .HasFillFactor(90);

                entity.Property(e => e.REG_DATE).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.addnum_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("추가수량비용(셋트 이외의 수량에 대한 합계비용)");

                entity.Property(e => e.discount_rate).HasComment("할인율");

                entity.Property(e => e.item_count).HasComment("주문수량");

                entity.Property(e => e.item_price).HasComment("소비자가");

                entity.Property(e => e.item_sale_price).HasComment("판매가");

                entity.Property(e => e.item_type)
                    .HasDefaultValueSql("('C')")
                    .IsFixedLength()
                    .HasComment("manage_code.itemt_type");

                entity.Property(e => e.memo1).HasComment("기타정보(미니청첩장의경우 인쇄색상등)");
            });

            modelBuilder.Entity<S4_COUPON>(entity =>
            {
                entity.HasIndex(e => e.coupon_code, "NCI_COUPON_CODE")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.company_seq, "NCI_S4_COUPON_COMPANY_SEQ")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.uid, "NCI_S4_COUPON_UID")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.uid, e.company_seq }, "NCI_S4_COUPON_UID_COMPANY_SEQ")
                    .HasFillFactor(90);

                entity.Property(e => e.COUPON_TYPE_CODE).HasDefaultValueSql("('114001')");

                entity.Property(e => e.cardbrand).IsFixedLength();

                entity.Property(e => e.discount_type)
                    .HasDefaultValueSql("('R')")
                    .IsFixedLength();

                entity.Property(e => e.isJehu)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.isRecycle)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength();

                entity.Property(e => e.isWeddingCoupon)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.isYN)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength();

                entity.Property(e => e.item_type).HasDefaultValueSql("('W1')");

                entity.Property(e => e.reg_date).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<S4_MyCoupon>(entity =>
            {

                entity.HasIndex(e => e.id, "CI_S4_MYCOUPON")
                    .IsClustered()
                    .HasFillFactor(90);

                entity.HasIndex(e => e.coupon_code, "NCI_COUPON_CODE")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.company_seq, "NCI_S4_MYCOUPON_COMPANY_SEQ")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.uid, "NCI_S4_MYCOUPON_UID")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.uid, e.company_seq }, "NCI_S4_MYCOUPON_UID_COMPANY_SEQ")
                    .HasFillFactor(90);

                entity.Property(e => e.id).ValueGeneratedOnAdd();

                entity.Property(e => e.isMyYN)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength();

                entity.Property(e => e.reg_date).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<COUPON_DETAIL>(entity =>
            {

                entity.HasIndex(e => e.COUPON_CODE, "NCI_COUPON_CODE")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.COUPON_MST_SEQ, "NCI_COUPON_MST_SEQ")
                    .HasFillFactor(90);

                entity.Property(e => e.COUPON_DETAIL_SEQ).HasComment("SEQ");

                entity.Property(e => e.COUPON_CODE).HasComment("쿠폰코드");

                entity.Property(e => e.COUPON_MST_SEQ).HasComment("COUPON_MST.COUPON_MST_SEQ");

                entity.Property(e => e.DOWNLOAD_ACTIVE_YN)
                    .HasDefaultValueSql("('Y')")
                    .HasComment("다운로드가능여부(Y/N)");

            });

            modelBuilder.Entity<COUPON_ISSUE>(entity =>
            {

                entity.HasIndex(e => e.ACTIVE_YN, "NCI_ACTIVE_YN")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.COUPON_DETAIL_SEQ, "NCI_COUPON_DETAIL_SEQ")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.UID, "NCI_UID")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.UID, e.ACTIVE_YN }, "NCI_UID_ACTIVE_YN")
                    .HasFillFactor(90);

                entity.Property(e => e.COUPON_ISSUE_SEQ).HasComment("SEQ");

                entity.Property(e => e.ACTIVE_YN)
                    .HasDefaultValueSql("('Y')")
                    .HasComment("사용가능여부(Y/N)");

                entity.Property(e => e.COMPANY_SEQ).HasComment("COMPANY_SEQ");

                entity.Property(e => e.COUPON_DETAIL_SEQ).HasComment("COUPON_DETAIL.COUPON_DETAIL_SEQ");

                entity.Property(e => e.END_DATE).HasComment("종료일");

                entity.Property(e => e.REG_DATE).HasComment("등록일");

                entity.Property(e => e.SALES_GUBUN).HasComment("사이트구분");

                entity.Property(e => e.UID).HasComment("유저ID");

            });

            modelBuilder.Entity<S2_UserInfo_TheCard>(entity =>
            {
                entity.HasKey(e => new { e.uid, e.jumin })
                    .HasName("PK_CIDX_S2_UserInfo_TheCard__uid");

                entity.HasIndex(e => e.reg_date, "IDX_S2_UserInfo_TheCard_Reg_Date")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.hand_phone1, "IDX_hand_phone1")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.hand_phone1, e.hand_phone2, e.hand_phone3 }, "IDX_hand_phone123")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.hand_phone2, "IDX_hand_phone2")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.hand_phone3, "IDX_hand_phone3")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.phone1, "IDX_phone1")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.phone1, e.phone2, e.phone3 }, "IDX_phone123")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.phone2, "IDX_phone2")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.phone3, "IDX_phone3")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.ConnInfo, "NCI_CONNINFO")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.DupInfo, "NCI_DUPINFO")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.INTERGRATION_DATE, "NCI_INTERGRATION_DATE")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.INTEGRATION_MEMBER_YORN, "NCI_INTERGRATION_MEMBER_YORN")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.REFERER_SALES_GUBUN, "NCI_REFERER_SALES_GUBUN")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.SELECT_SALES_GUBUN, "NCI_SELECT_SALES_GUBUN")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.site_div, "NCI_SITE_DIV")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.uid, e.DupInfo }, "NCI_USERINFO_THECARD_UID_DUPINFO")
                    .HasFillFactor(90);

                entity.Property(e => e.AuthType).IsFixedLength();

                entity.Property(e => e.DupInfo).IsFixedLength();

                entity.Property(e => e.Gender).IsFixedLength();

                entity.Property(e => e.INTEGRATION_MEMBER_YORN).IsFixedLength();

                entity.Property(e => e.NationalInfo).IsFixedLength();

                entity.Property(e => e.USE_YORN).IsFixedLength();

                entity.Property(e => e.addr_flag).HasDefaultValueSql("((0))");

                entity.Property(e => e.birth_div).IsFixedLength();

                entity.Property(e => e.chk_DM)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength();

                entity.Property(e => e.chk_DormancyAccount).IsFixedLength();

                entity.Property(e => e.chk_iloommembership).IsFixedLength();

                entity.Property(e => e.chk_lgmembership).IsFixedLength();

                entity.Property(e => e.chk_mail_input).IsFixedLength();

                entity.Property(e => e.chk_mailservice).IsFixedLength();

                entity.Property(e => e.chk_myomee).IsFixedLength();

                entity.Property(e => e.chk_smembership)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.chk_smembership_coop).IsFixedLength();

                entity.Property(e => e.chk_smembership_leave).IsFixedLength();

                entity.Property(e => e.chk_smembership_per).IsFixedLength();

                entity.Property(e => e.chk_sms).IsFixedLength();

                entity.Property(e => e.isJehu)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.isMCardAble)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength();

                entity.Property(e => e.is_appSample)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.mkt_chk_flag).IsFixedLength();

                entity.Property(e => e.mod_date).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.reg_date).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.smembership_chk_flag).IsFixedLength();

                entity.Property(e => e.wedd_pgubun).IsFixedLength();
            });

            modelBuilder.Entity<CUSTOM_ETC_ORDER>(entity =>
            {
                entity.HasKey(e => e.order_seq)
                    .HasName("PK_CUSTOM_ETC_ORDER_1");


                entity.HasIndex(e => e.order_g_seq, "IDX_CUSTOM_ETC_ORDER_G_SEQ")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.member_id, e.order_type }, "custom_etc_order_ccg")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.member_id, "custom_etc_order_memid")
                    .HasFillFactor(90);

                entity.Property(e => e.order_seq).ValueGeneratedNever();

                entity.Property(e => e.WisaFlag).IsFixedLength();

                entity.Property(e => e.card_nointyn).IsFixedLength();

                entity.Property(e => e.company_Seq).HasDefaultValueSql("((1))");

                entity.Property(e => e.coupon_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("쿠폰차감금액");

                entity.Property(e => e.couponseq).HasComment("쿠폰번호");

                entity.Property(e => e.delivery_code).HasComment("송장코드");

                entity.Property(e => e.delivery_com)
                    .IsFixedLength()
                    .HasComment("배송업체");

                entity.Property(e => e.delivery_date).HasComment("배송일자");

                entity.Property(e => e.delivery_method)
                    .IsFixedLength()
                    .HasComment("배송방법");

                entity.Property(e => e.delivery_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("배송비");

                entity.Property(e => e.isAscrow)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isHJ)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isReceipt)
                    .IsFixedLength()
                    .HasComment("영수증발급여부");

                entity.Property(e => e.option_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("옵션선택비용");

                entity.Property(e => e.order_date).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.order_email).HasComment("주문자 이메일");

                entity.Property(e => e.order_g_seq).HasComment("더카드전용 통합SEQ");

                entity.Property(e => e.order_hphone).HasComment("주문자 핸드폰번호");

                entity.Property(e => e.order_name).HasComment("주문자 이름");

                entity.Property(e => e.order_phone).HasComment("주문자 전화번호");

                entity.Property(e => e.order_type).HasComment("manage_code.code (code_type =etcprod)");

                entity.Property(e => e.pg_Fee).HasDefaultValueSql("((0))");

                entity.Property(e => e.pg_receipt_tid).HasComment("영수증발급ID");

                entity.Property(e => e.pg_refee).HasDefaultValueSql("((0))");

                entity.Property(e => e.pg_resultinfo).HasComment("결제결과-결제정보");

                entity.Property(e => e.pg_resultinfo2).HasComment("결제결과-결제자명");

                entity.Property(e => e.pg_shopid)
                    .HasDefaultValueSql("('bhands_b')")
                    .HasComment("데이콤 PG아이디");

                entity.Property(e => e.pg_tid).HasComment("데이콤 주문번호 or 이니시스 연동 TID");

                entity.Property(e => e.prepare_date).HasComment("준비일자");

                entity.Property(e => e.recv_address).HasComment("수취인 주소");

                entity.Property(e => e.recv_address_detail).HasComment("수취인 상세주소");

                entity.Property(e => e.recv_hphone).HasComment("수취인 핸드폰 번호");

                entity.Property(e => e.recv_msg).HasComment("배송 메시지");

                entity.Property(e => e.recv_name).HasComment("수취인 인름");

                entity.Property(e => e.recv_phone).HasComment("수취인 전화번호");

                entity.Property(e => e.recv_zip).HasComment("수취인 우편번호");

                entity.Property(e => e.sales_gubun)
                    .HasDefaultValueSql("('W')")
                    .HasComment("판매사이트구분");

                entity.Property(e => e.settle_Cancel_Date).HasComment("결제취소일");

                entity.Property(e => e.settle_date).HasComment("결제일");

                entity.Property(e => e.settle_method)
                    .IsFixedLength()
                    .HasComment("결제방법");

                entity.Property(e => e.settle_price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("결제금액");

                entity.Property(e => e.site_gubun)
                    .HasDefaultValueSql("('W')")
                    .IsFixedLength();

                entity.Property(e => e.status_seq)
                    .HasDefaultValueSql("((0))")
                    .HasComment("주문상태");
            });

            modelBuilder.Entity<CUSTOM_ETC_ORDER_ITEM>(entity =>
            {
                entity.HasKey(e => new { e.order_seq, e.seq });


                entity.Property(e => e.card_opt).HasComment("제품선택옵션");

                entity.Property(e => e.card_price).HasComment("상품소비자가격");

                entity.Property(e => e.card_sale_price).HasComment("상품할인구매가");

                entity.Property(e => e.isChange)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("시즌카드의 경우 카드교체여부");

                entity.Property(e => e.order_count).HasComment("주문수량");

                entity.Property(e => e.order_tbl)
                    .HasDefaultValueSql("('E')")
                    .IsFixedLength()
                    .HasComment("W:청첩장 테이블,E:etc 테이블");
            });

            modelBuilder.Entity<S2_Card>(entity =>
            {

                entity.HasIndex(e => e.Card_Code, "IDX__card_code")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.Card_ERPCode, "IDX__card_erpcode")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.Card_Name, "IDX__card_name")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.Card_Price, "IDX__card_price")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.CardSet_Price, "IDX__cardset_price")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.Card_Seq, "IX_S2_Card")
                    .HasFillFactor(90);

                entity.Property(e => e.CARD_GROUP)
                    .HasDefaultValueSql("('I')")
                    .IsFixedLength()
                    .HasComment("I:초대장, X:시즌카드");

                entity.Property(e => e.CardBrand)
                    .IsFixedLength()
                    .HasComment("B:바른손카드, N:비핸즈,W:위시메이드,S:프페 Z:기타");

                entity.Property(e => e.CardFactory_Price).HasDefaultValueSql("((0))");

                entity.Property(e => e.CardSet_Price).HasComment("제품 셋트가(카드에만 적용)");

                entity.Property(e => e.Card_Div)
                    .IsFixedLength()
                    .HasComment("A01:카드,A02:내지,A03:인사말카드,A04:약도카드 B01:봉투,B02:봉투라이닝 C01:신랑봉투,C02:신부봉투,C03:미니,C04:스티커,C05:사은품,C06:식권셋트");

                entity.Property(e => e.Card_ERPCode).HasComment("ERP연동코드");

                entity.Property(e => e.Card_HSize).HasComment("제품 세로 사이즈");

                entity.Property(e => e.Card_Image).HasComment("제품썸네일이미지(주문단에 사용)");

                entity.Property(e => e.Card_Price).HasComment("단품가격 (추후 추가주문 등 단품 판매 기준가격)");

                entity.Property(e => e.DISPLAY_YORN)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength();

                entity.Property(e => e.ERP_EXPECTED_ARRIVAL_DATE).HasComment("사용안함");

                entity.Property(e => e.ERP_EXPECTED_ARRIVAL_DATE_USE_YORN).HasDefaultValueSql("('N')");

                entity.Property(e => e.ERP_MIN_STOCK_QTY).HasDefaultValueSql("((3000))");

                entity.Property(e => e.ERP_MIN_STOCK_QTY_USE_YORN).HasDefaultValueSql("('Y')");

                entity.Property(e => e.Explain).HasComment("내용");

                entity.Property(e => e.FPRINT_YORN).IsFixedLength();

                entity.Property(e => e.Option_Name).HasComment("부가상품_옵션명");

                entity.Property(e => e.REGIST_DATES).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RegDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Tip).HasComment("구매팁");

                entity.Property(e => e.Unit).HasComment("단위");

                entity.Property(e => e.Unit_Value).HasComment("단위수량");

                entity.Property(e => e.WisaFlag).IsFixedLength();

                entity.Property(e => e.admin_id).HasComment("수정자");

                entity.Property(e => e.t_env_code).HasComment("봉투코드");
            });

            modelBuilder.Entity<manage_code>(entity =>
            {
                entity.HasKey(e => e.code_id)
                    .HasName("PK_manage_code_1");


                entity.Property(e => e.code).HasComment("코드");

                entity.Property(e => e.code_type).HasComment("코드타입");

                entity.Property(e => e.code_value).HasComment("코드명");

                entity.Property(e => e.parent_id)
                    .HasDefaultValueSql("((0))")
                    .HasComment("상위id");

                entity.Property(e => e.seq).HasDefaultValueSql("((0))");

                entity.Property(e => e.use_yorn)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength()
                    .HasComment("사용여부");
            });

            modelBuilder.Entity<gift_company_tel>(entity =>
            {
                entity.Property(e => e.created_tmstmp).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.isYN).HasDefaultValueSql("('Y')");
            });

            modelBuilder.Entity<ata_mmt_tran>(entity =>
            {
                entity.HasKey(e => e.mt_pr)
                    .HasName("PK__ata_mmt___910A22E1257DEFA8");


                entity.Property(e => e.ad_flag)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength()
                    .HasComment("카카오톡 친구톡 발송시 광고성메시지")
                    .UseCollation("Korean_Wansung_CS_AS");

                entity.Property(e => e.ata_id)
                    .HasDefaultValueSql("(' ')")
                    .IsFixedLength()
                    .HasComment("ATA 이중화시 사용되는 ID")
                    .UseCollation("Korean_Wansung_CS_AS");

                entity.Property(e => e.callback)
                    .HasComment("발신자 전화 번호")
                    .UseCollation("Korean_Wansung_CS_AS");

                entity.Property(e => e.content)
                    .HasComment("전송 메시지")
                    .UseCollation("Korean_Wansung_CS_AS");

                entity.Property(e => e.country_code)
                    .HasDefaultValueSql("('82')")
                    .HasComment("국가 코드")
                    .UseCollation("Korean_Wansung_CS_AS");

                entity.Property(e => e.crypto_yn)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength()
                    .HasComment("암호화 사용 유무")
                    .UseCollation("Korean_Wansung_CS_AS");

                entity.Property(e => e.date_client_req)
                    .HasDefaultValueSql("('1970-01-01 00:00:00')")
                    .HasComment("전송 예약 시간");

                entity.Property(e => e.date_mt_report).HasComment("Biz talk 으로부터 결과 수신한 시간");

                entity.Property(e => e.date_mt_sent).HasComment("Biz talk G/W 접수 시간");

                entity.Property(e => e.date_rslt).HasComment("단말기 도착 시간");

                entity.Property(e => e.etc_num_1).HasComment("유저 기타필드)company_seq");

                entity.Property(e => e.etc_text_1)
                    .HasComment("유저 기타필드)sales_gubun")
                    .UseCollation("Korean_Wansung_CS_AS");

                entity.Property(e => e.etc_text_2)
                    .HasComment("유저 기타필드)호출프로시저")
                    .UseCollation("Korean_Wansung_CS_AS");

                entity.Property(e => e.etc_text_3).UseCollation("Korean_Wansung_CS_AS");

                entity.Property(e => e.img_link)
                    .HasComment("친구톡 이미지 클릭시 이동할 URL")
                    .UseCollation("Korean_Wansung_CS_AS");

                entity.Property(e => e.img_url)
                    .HasComment("친구톡 이미지 URL")
                    .UseCollation("Korean_Wansung_CS_AS");

                entity.Property(e => e.kko_btn_info)
                    .HasComment("버튼템플릿 전송시 버튼 정보")
                    .UseCollation("Korean_Wansung_CS_AS");

                entity.Property(e => e.kko_btn_type)
                    .IsFixedLength()
                    .HasComment("카카오톡 전송방식 1-format string 2-JSON 3-XML")
                    .UseCollation("Korean_Wansung_CS_AS");

                entity.Property(e => e.msg_status)
                    .HasDefaultValueSql("('1')")
                    .IsFixedLength()
                    .HasComment("메시지 상태 (1-전송대기, 2-결과대기, 3-완료)")
                    .UseCollation("Korean_Wansung_CS_AS");

                entity.Property(e => e.msg_type)
                    .HasDefaultValueSql("('1008')")
                    .HasComment("메시지 종류(1008-알림톡, 1009-친구톡)");

                entity.Property(e => e.mt_refkey)
                    .HasComment("부서 코드 (참조용 필드)")
                    .UseCollation("Korean_Wansung_CS_AS");

                entity.Property(e => e.priority)
                    .HasDefaultValueSql("('S')")
                    .IsFixedLength()
                    .HasComment("메시지 우선 순위")
                    .UseCollation("Korean_Wansung_CS_AS");

                entity.Property(e => e.recipient_num)
                    .HasComment("수신자 전화 번호")
                    .UseCollation("Korean_Wansung_CS_AS");

                entity.Property(e => e.reg_date)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("데이터 등록일자");

                entity.Property(e => e.report_code)
                    .IsFixedLength()
                    .HasComment("전송 결과(1000-성공, 기타-실패)")
                    .UseCollation("Korean_Wansung_CS_AS");

                entity.Property(e => e.response_method)
                    .HasDefaultValueSql("('push')")
                    .HasComment("메시지 발송 방식")
                    .UseCollation("Korean_Wansung_CS_AS");

                entity.Property(e => e.rs_id)
                    .HasComment("전송된 Biz talk G/W 정보")
                    .UseCollation("Korean_Wansung_CS_AS");

                entity.Property(e => e.sender_key)
                    .HasComment("발신 프로필 키")
                    .UseCollation("Korean_Wansung_CS_AS");

                entity.Property(e => e.subject)
                    .HasDefaultValueSql("(' ')")
                    .HasComment("메시지 제목")
                    .UseCollation("Korean_Wansung_CS_AS");

                entity.Property(e => e.template_code)
                    .HasComment("메시지 유형 템플릿 코드")
                    .UseCollation("Korean_Wansung_CS_AS");
            });

            modelBuilder.Entity<wedd_biztalk>(entity =>
            {

                entity.Property(e => e.callback).HasDefaultValueSql("('1644-9713')");
            });

            modelBuilder.Entity<CUSTOM_ORDER_ADMIN_MENT>(entity =>
            {
                entity.HasKey(e => e.ID)
                    .IsClustered(false);

                entity.HasIndex(e => e.REG_DATE, "clu_reg_date")
                    .IsClustered()
                    .HasFillFactor(90);

                entity.HasIndex(e => e.ORDER_SEQ, "nind_corder_seq")
                    .HasFillFactor(90);

                entity.Property(e => e.ADMIN_ID).HasComment("등록 어드민");

                entity.Property(e => e.ISWOrder)
                    .HasDefaultValueSql("(1)")
                    .IsFixedLength()
                    .HasComment("1:청첩장관련,0:식권 또는 샘플");

                entity.Property(e => e.MENT).HasComment("메모");

                entity.Property(e => e.PCHECK).HasComment("유형(0:일반,1:포장지시,2:사고,3/5:취소)");

                entity.Property(e => e.REG_DATE)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("등록일");

                entity.Property(e => e.STATUS)
                    .HasDefaultValueSql("(0)")
                    .HasComment("처리여부(0:등록,9:처리완료)");

                entity.Property(e => e.intype).HasDefaultValueSql("((0))");

                entity.Property(e => e.isJumun)
                    .HasDefaultValueSql("('1')")
                    .IsFixedLength();

                entity.Property(e => e.sgubun).HasDefaultValueSql("('')");

                entity.Property(e => e.stype).HasDefaultValueSql("('기타')");
            });

            modelBuilder.Entity<S4_Stock_Alarm>(entity =>
            {
                entity.HasKey(e => e.seq)
                    .HasName("PK_CIDX_S4_Stock_Alarm__seq");


                entity.Property(e => e.isAlarm_send)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength()
                    .HasComment("발송 여부");

                entity.Property(e => e.reg_date)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("등록일");

                entity.Property(e => e.send_date).HasComment("발송일");
            });

            modelBuilder.Entity<KT_DAILY_INFO>(entity =>
            {
                entity.HasKey(e => e.seq)
                    .HasName("PK__KT_DAILY__DDDFBCBED0ED308F");
            });

            modelBuilder.Entity<iwedding_Sending>(entity =>
            {

                entity.Property(e => e.order_seq)
                    .ValueGeneratedNever()
                    .HasComment("아이웨딩에 발송완료 주문정보 넘겨진 주문번호");

                entity.Property(e => e.reg_date).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<S2_CardView>(entity =>
            {
                entity.ToView("S2_CardView");

                entity.Property(e => e.card_div).IsFixedLength();
            });

            modelBuilder.Entity<SendEmailContent>(entity =>
            {
                entity.Property(e => e.EmailFormCode).HasComment("패턴정의: 영문 대문자 1자리+숫자 2자리코드  ex)C01-> Customer 관련 첫번재 메일폼");
                entity.Property(e => e.ModDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.RegDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.SendYn).HasComment("기본값: false, 내용 수정시 false 변경");
                entity.Property(e => e.ToEmailAddress).HasComment("추가 수신 대상이 있을 경우");
                entity.Property(e => e.ToName).HasComment("추가 수신 대상이 있을 경우");
            });

            modelBuilder.Entity<SendEmailContentItem>(entity =>
            {
                entity.HasOne(d => d.Content).WithMany(p => p.SendEmailContentItem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SendEmailContentItem_SendEmailContent");
            });

            modelBuilder.Entity<SendEmailMaster>(entity =>
            {
                entity.Property(e => e.EmailFormCode).HasComment("패턴정의: 영문 대문자 1자리+숫자 2자리코드  ex)C01-> Customer 관련 첫번재 메일폼");
                entity.Property(e => e.ContentDescription).HasComment("([이메일] - 신청자 메일 주소, [문의사항] - 문의사항내용 ) 등 콘텐트에 치환될 내용 설명(참고용)");
                entity.Property(e => e.Contents).HasComment("메일 본문 또는 본문 페이지 URL");
                entity.Property(e => e.ModDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.RegDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UseYn).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<SendEmailRecipient>(entity =>
            {
                entity.Property(e => e.EmailFormCode).HasComment("패턴정의: 영문 대문자 1자리+숫자 2자리코드  ex)C01-> Customer 관련 첫번재 메일폼");
                entity.Property(e => e.ModDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.RegDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UserYn).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.EmailFormCodeNavigation).WithMany(p => p.SendEmailRecipient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SendEmailRecipient_SendEmailMaster");
            });

            modelBuilder.Entity<S2_CardOption>(entity =>
            {
                entity.HasComment("카드옵션정보");

                entity.Property(e => e.Card_Seq).ValueGeneratedNever();

                entity.Property(e => e.DigitalColor).HasComment("디지털 인쇄 칼라색상 종류");

                entity.Property(e => e.IsAdd)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("추가주문 (0:불가,1:가능)");

                entity.Property(e => e.IsColorInpaper)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.IsColorPrint)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("칼라인쇄 (0:없음,1:유료,2:무료)");

                entity.Property(e => e.IsEmbo)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("엠보인쇄 (0:없음,1:유료,2:무료)");

                entity.Property(e => e.IsEmboColor)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("엠보인쇄칼라(1:기본,2:진회색,3:은색,4:갈색,5:짙은청색,6:자주색)");

                entity.Property(e => e.IsEnvInsert)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("봉투삽입 (0:없음,1:유료,2:무료)");

                entity.Property(e => e.IsFChoice)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.IsHandmade)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("부속품제본 (0:없음,1:유료,2:무료)");

                entity.Property(e => e.IsHandmade2).IsFixedLength();

                entity.Property(e => e.IsHanji)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("한지여부(1:일반한지,2:가로형고급한지,3:세로형고급한지)");

                entity.Property(e => e.IsInPaper)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.IsJaebon)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("내지 끼우기 (0:없음,1:유료,2:무료)");

                entity.Property(e => e.IsLiningJaebon)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("봉투라이닝 (0:없음,1:유료,2:무료)");

                entity.Property(e => e.IsOutsideInitial)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("겉면인쇄여부");

                entity.Property(e => e.IsQuick)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("초특급 가능 여부");

                entity.Property(e => e.IsSample)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("샘플주문 (0:불가,1:가능)");

                entity.Property(e => e.IsSampleEnd)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength();

                entity.Property(e => e.IsSensInpaper)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("감성내지 (0:없음,1:있음)");

                entity.Property(e => e.IsSticker)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("스티커제공 (0:없음,1:유료,2:무료)");

                entity.Property(e => e.IsUsrComment)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("주문시 사용자 멘트(0:없음,1:필요)");

                entity.Property(e => e.IsUsrImg1)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("주문시 사용자 이미지업로드(0:없음,1:필요)");

                entity.Property(e => e.IsUsrImg2)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("주문시 사용자 이미지업로드(0:없음,1:필요)");

                entity.Property(e => e.IsUsrImg3)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("주문시 사용자 이미지업로드(0:없음,1:필요)");

                entity.Property(e => e.IsUsrImg4)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.Master_2Color)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.PrintMethod).HasComment("인쇄방법(XXX 세자리 캐릭터값이 다음과 같이 주어진다) G:금박,S:은박, B:먹박,0:박없음, 1:유광,0:무광, 1:형압,0:압없음");

                entity.Property(e => e.SpecialAccYN).IsFixedLength();

                entity.Property(e => e.embo_print).HasComment("엠보인쇄되는 항목(C:카드,P:약도카드,I:내지 등)");

                entity.Property(e => e.isColorMaster).IsFixedLength();

                entity.Property(e => e.isCustomDColor)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isDesigner).HasComment("카드 디자이너");

                entity.Property(e => e.isDigitalColor)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("디지털 인쇄 여부");

                entity.Property(e => e.isEngWedding).IsFixedLength();

                entity.Property(e => e.isEnvDesignType).IsFixedLength();

                entity.Property(e => e.isEnvPremium).HasDefaultValueSql("((0))");

                entity.Property(e => e.isEnvSpecial)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("고급봉투 가능여부");

                entity.Property(e => e.isEnvSpecialPrint).IsFixedLength();

                entity.Property(e => e.isFSC)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isGreeting).IsFixedLength();

                entity.Property(e => e.isGroomBrideYN)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isHappyPrice)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isHoneyMoon).IsFixedLength();

                entity.Property(e => e.isInternalDigital)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isJigunamu)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isLInitial)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isLanguage)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("0:사용안함, 1:한글만, 2:영문만, 3:한/영선택");

                entity.Property(e => e.isLaser)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isLaserCard)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isLetterPress)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isMasterDigital)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isMasterPrintColor).IsFixedLength();

                entity.Property(e => e.isMiniCard)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isMoneyEnv)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isNewEvent)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isNewGubun)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isPhrase)
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength();

                entity.Property(e => e.isPutGiveCard)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isRepinart)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isSelfEditor)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength()
                    .HasComment("셀프초안주문 (0:불가,1:가능)");

                entity.Property(e => e.isSpringYN)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isStarcard)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isTechnic).HasComment("테크닉가공 정보");

                entity.Property(e => e.isUsrImg_info)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isWongoYN)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.isstickerspecial)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.outsourcing_print).HasComment("외부업체인쇄되는 항목(C:카드,P:약도카드,I:내지 등)");
            });

            modelBuilder.Entity<COMPETITOR_CARD_MST>(entity =>
            {
                entity.ToTable(tb => tb.HasComment("타사이트 카드정보"));

                entity.HasIndex(e => e.REG_DATE, "NCI_REG_DATE").HasFillFactor(90);
            });


        }

        #endregion
    }
}
