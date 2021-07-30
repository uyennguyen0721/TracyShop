using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TracyShop.Models
{
    public partial class TracyShopContext : DbContext
    {
        public TracyShopContext() 
        { 

        }
        public TracyShopContext(DbContextOptions<TracyShopContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Address> Addresses { set; get; }
        public virtual DbSet<Category> Categories { set; get; }
        public virtual DbSet<Image> Images { set; get; }
        public virtual DbSet<Order> Orders { set; get; }
        public virtual DbSet<OrderDetail> OrderDetails { set; get; }
        public virtual DbSet<PaymentMenthod> PaymentMenthods { set; get; }
        public virtual DbSet<Product> Products { set; get; }
        public virtual DbSet<Promotion> Promotions { set; get; }
        public virtual DbSet<Reviews> Reviews { set; get; }
        public virtual DbSet<Size> Sizes { set; get; }
        public virtual DbSet<StockReceived> StockReceiveds { set; get; }
        public virtual DbSet<StockReceivedDetail> StockReceivedDetails { set; get; }
        public virtual DbSet<Trandemark> Trandemarks { set; get; }
        public virtual DbSet<UserRole> UserRoles { set; get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=uyennguyen\\SQLEXPRESS;Database=tracyshop;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.Street).HasMaxLength(50);
                entity.Property(e => e.Ward).HasMaxLength(25);
                entity.Property(e => e.District).HasMaxLength(25);
                entity.Property(e => e.City).HasMaxLength(25);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.Property(e => e.Path).HasMaxLength(255);
            });

            modelBuilder.Entity<PaymentMenthod>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(255);
            });

            modelBuilder.Entity<Reviews>(entity =>
            {
                entity.Property(e => e.Image).HasMaxLength(100);
                entity.Property(e => e.Content).HasMaxLength(255);
            });

            modelBuilder.Entity<Size>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(10);
                entity.Property(e => e.Description).HasMaxLength(255);
            });

            modelBuilder.Entity<Trandemark>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Origin).HasMaxLength(50);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(15);
            });
        }


    }
}
