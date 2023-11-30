using System;
using Barunson.WorkerService.Common.DBModels.MoSvr;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Barunson.WorkerService.Common.DBContext
{
    public partial class MoSvrContext : DbContext
    {
        public MoSvrContext()
        {
        }

        public MoSvrContext(DbContextOptions<MoSvrContext> options)
            : base(options)
        {
        }

        public virtual DbSet<SDK_MMS_SEND> SDK_MMS_SEND { get; set; } = null!;

        public virtual DbSet<SDK_SMS_SEND> SDK_SMS_SEND { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<SDK_MMS_SEND>(entity =>
            {
                entity.HasKey(e => e.MSG_ID)
                    .HasName("PK__SDK_MMS___825DA51C5B44FCDD");
            });

            modelBuilder.Entity<SDK_SMS_SEND>(entity =>
            {
                entity.HasKey(e => e.MSG_ID)
                    .HasName("PK__SDK_SMS___825DA51C29D675D1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
