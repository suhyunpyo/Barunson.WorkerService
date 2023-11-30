using Barunson.WorkerService.Common.DBModels.DearDeer;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.Common.DBContext
{
    public partial class DearDeerContext : DbContext
    {
        public DearDeerContext()
        {
        }

        public DearDeerContext(DbContextOptions<DearDeerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<orders> orders { get; set; }
        public virtual DbSet<sample_orders> sample_orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<orders>(entity =>
            {
                
                entity.Property(e => e.accident_image).HasComment("사고건 이미지");

                entity.Property(e => e.barunson_order_flag)
                    .HasDefaultValueSql("'F'")
                    .IsFixedLength(true)
                    .HasComment("바른손에 주문성공: T, 실패: F");

                entity.Property(e => e.barunson_order_type)
                    .HasDefaultValueSql("'base'")
                    .HasComment("바른손 주문타입: base: 인쇄제품, etc: 완제품");

                entity.Property(e => e.barunson_status_seq).HasComment("바른손 주문상태값");

                entity.Property(e => e.base_printing_company)
                    .HasDefaultValueSql("'barunson'")
                    .HasComment("red, barunson");

                entity.Property(e => e.box_unit)
                    .HasDefaultValueSql("'1'")
                    .HasComment("박스단위");

                entity.Property(e => e.charge_unit)
                    .HasDefaultValueSql("'선불'")
                    .HasComment("요금단위");

                entity.Property(e => e.delivery_price).HasComment("배송비");

                entity.Property(e => e.discount_money).HasComment("할인 금액");

                entity.Property(e => e.draft_state).HasDefaultValueSql("'B'");

                entity.Property(e => e.is_accident)
                    .HasDefaultValueSql("'F'")
                    .IsFixedLength(true);

                entity.Property(e => e.is_create_file)
                    .HasDefaultValueSql("'F'")
                    .IsFixedLength(true);

                entity.Property(e => e.is_printing)
                    .HasDefaultValueSql("'F'")
                    .IsFixedLength(true);

                entity.Property(e => e.locker_no).HasComment("사물함번호");

                entity.Property(e => e.order_base)
                    .HasDefaultValueSql("'wcard'")
                    .HasComment("wcard, goods, mcard");

                entity.Property(e => e.order_state).HasDefaultValueSql("'B'");

                entity.Property(e => e.order_type)
                    .HasDefaultValueSql("'D'")
                    .IsFixedLength(true)
                    .HasComment("주문 타입 : D=일반, Q:빠른");

                entity.Property(e => e.original_amount).HasComment("원금 합계");

                entity.Property(e => e.packing_at).HasComment("패킹시작시간");

                entity.Property(e => e.packing_state)
                    .HasDefaultValueSql("'B'")
                    .IsFixedLength(true)
                    .HasComment("패킹상태 : B:시작전,  S:시작,  E:완료");

                entity.Property(e => e.partner_shop_id).HasComment("partner_shop.id");

                entity.Property(e => e.print_degree).HasComment("인쇄차수");

                entity.Property(e => e.printing_state).HasDefaultValueSql("'B'");

                entity.Property(e => e.red_api_state).HasDefaultValueSql("'F'");

                entity.Property(e => e.red_order_no).HasComment("레드주문번호");

                entity.Property(e => e.refund_money).HasComment("환불금액");

                entity.Property(e => e.refund_type)
                    .IsFixedLength(true)
                    .HasComment("환불타입 - A: 부분환불, B:전체환불");

                entity.Property(e => e.refunded_at).HasComment("환불일자");

                entity.Property(e => e.shipping_due_date).HasComment("배송예정일");

                entity.Property(e => e.shipping_state).HasDefaultValueSql("'B'");

                entity.Property(e => e.shipping_type)
                    .HasDefaultValueSql("'P'")
                    .IsFixedLength(true)
                    .HasComment("배송타입 : P=택배, D=직접, Q:퀵, ");

                entity.Property(e => e.status_red).HasDefaultValueSql("'PGS'");

                entity.Property(e => e.use_coupon)
                    .HasDefaultValueSql("'F'")
                    .IsFixedLength(true);

                entity.Property(e => e.volumne_unit)
                    .HasDefaultValueSql("'60'")
                    .HasComment("부피단위");
            });

            modelBuilder.Entity<sample_orders>(entity =>
            {
                entity.Property(e => e.addresses_id).HasComment("addresses.id");

                entity.Property(e => e.admin_memo).HasComment("관리자 메모");

                entity.Property(e => e.bank_info).HasComment("은행명");

                entity.Property(e => e.bank_name).HasComment("은행명");

                entity.Property(e => e.barunson_order_seq).HasComment("custom_samlpe_order.samlpq_order_seq");

                entity.Property(e => e.bride_fname).HasComment("신부 성");

                entity.Property(e => e.bride_fname_eng).HasComment("신부 영문 이름");

                entity.Property(e => e.bride_name).HasComment("신부 이름");

                entity.Property(e => e.bride_name_eng).HasComment("신부 영문성");

                entity.Property(e => e.cash).HasComment("무통장입금");

                entity.Property(e => e.groom_fname).HasComment("신랑 성");

                entity.Property(e => e.groom_fname_eng).HasComment("신랑 영문 이름");

                entity.Property(e => e.groom_name).HasComment("신랑 이름");

                entity.Property(e => e.groom_name_eng).HasComment("신랑 영문성");

                entity.Property(e => e.is_create_file)
                    .HasDefaultValueSql("'F'")
                    .IsFixedLength(true)
                    .HasComment("F U+ 주문취소가능 / T 취소불가 ");

                entity.Property(e => e.memo).HasComment("메모");

                entity.Property(e => e.order_ip).HasComment("주문자 ip");

                entity.Property(e => e.order_state)
                    .HasDefaultValueSql("'B'")
                    .HasComment("주문상태");

                entity.Property(e => e.order_step)
                    .HasDefaultValueSql("'payment'")
                    .HasComment("payment||fail");

                entity.Property(e => e.order_useragent).HasComment("주문자 useragent");

                entity.Property(e => e.paid_list_id).HasComment("lguplus_paid_list.id");

                entity.Property(e => e.paid_money).HasComment("전체금액");

                entity.Property(e => e.partner_shop_id).HasComment("partner_shop.id");

                entity.Property(e => e.pay_date).HasComment("결제 일자");

                entity.Property(e => e.pay_day).HasComment("결제 일");

                entity.Property(e => e.pay_month).HasComment("결제 월");

                entity.Property(e => e.pay_type).HasComment("결제 타입");

                entity.Property(e => e.pay_year).HasComment("결제 년");

                entity.Property(e => e.pg_app_no).HasComment("결제 앱 번호");

                entity.Property(e => e.pg_name).HasComment("결제회사명");

                entity.Property(e => e.pg_tno).HasComment("결제 회사 tid");

                entity.Property(e => e.printing_state)
                    .HasDefaultValueSql("'B'")
                    .HasComment("제작상태");

                entity.Property(e => e.refund_money).HasComment("환불금액");

                entity.Property(e => e.refund_type)
                    .IsFixedLength(true)
                    .HasComment("환불타입 - A: 부분환불, B:전체환불");

                entity.Property(e => e.refunded_at).HasComment("환불일자");

                entity.Property(e => e.sample_order_no).HasComment("주문번호");

                entity.Property(e => e.shipping_at).HasComment("샘플발송일");

                entity.Property(e => e.shipping_company).HasComment("배송사");

                entity.Property(e => e.shipping_num).HasComment("송장번호");

                entity.Property(e => e.shipping_state)
                    .HasDefaultValueSql("'B'")
                    .HasComment("배송상태");

                entity.Property(e => e.total_money).HasComment("전체금액");

                entity.Property(e => e.user_id).HasComment("users.id");

                entity.Property(e => e.wedd_date).HasComment("예식일");
            });
        }
    }
}
