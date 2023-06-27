using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace HDU_AppXetTuyen.Models
{
    public partial class DbConnecttion : DbContext
    {
        public DbConnecttion()
            : base("name=DbConnecttion")
        {
        }

        public virtual DbSet<AdminAccount> AdminAccounts { get; set; }
        public virtual DbSet<DangKyXetTuyen> DangKyXetTuyens { get; set; }
        public virtual DbSet<DangKyXetTuyen_DiemTS> DangKyXetTuyen_DiemTS { get; set; }
        public virtual DbSet<DoiTuong> DoiTuongs { get; set; }
        public virtual DbSet<DotXetTuyen> DotXetTuyens { get; set; }
        public virtual DbSet<Huyen> Huyens { get; set; }
        public virtual DbSet<KhoiNganh> KhoiNganhs { get; set; }
        public virtual DbSet<KhuVuc> KhuVucs { get; set; }
        public virtual DbSet<LePhiXetTuyen> LePhiXetTuyens { get; set; }
        public virtual DbSet<MonHoc> MonHocs { get; set; }
        public virtual DbSet<NamHoc> NamHocs { get; set; }
        public virtual DbSet<Ngành> Ngành { get; set; }
        public virtual DbSet<PhuongThucXetTuyen> PhuongThucXetTuyens { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<ThiSinhDangKy> ThiSinhDangKies { get; set; }
        public virtual DbSet<Tinh> Tinhs { get; set; }
        public virtual DbSet<ToHopMon> ToHopMons { get; set; }
        public virtual DbSet<TruongCapBa> TruongCapBas { get; set; }
        public virtual DbSet<Xa> Xas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DotXetTuyen>()
                .HasMany(e => e.DangKyXetTuyens)
                .WithOptional(e => e.DotXetTuyen)
                .HasForeignKey(e => e.DotXT_ID);

            modelBuilder.Entity<DotXetTuyen>()
                .HasMany(e => e.ThiSinhDangKies)
                .WithOptional(e => e.DotXetTuyen)
                .HasForeignKey(e => e.DotXT_ID);

            modelBuilder.Entity<MonHoc>()
                .HasMany(e => e.ToHopMons)
                .WithOptional(e => e.MonHoc)
                .HasForeignKey(e => e.Thm_mon1_ID);

            modelBuilder.Entity<MonHoc>()
                .HasMany(e => e.ToHopMons1)
                .WithOptional(e => e.MonHoc1)
                .HasForeignKey(e => e.Thm_mon2_ID);

            modelBuilder.Entity<MonHoc>()
                .HasMany(e => e.ToHopMons2)
                .WithOptional(e => e.MonHoc2)
                .HasForeignKey(e => e.Thm_mon3_ID);
        }
    }
}
