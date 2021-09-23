using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Models;

namespace TracyShop.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public AppDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.Entity<Address>().HasIndex(a => a.UserId).IsUnique();
            builder.Entity<ProductSize>().HasKey(ps => new { ps.ProductId, ps.SizeId });

            builder.Entity<ProductSize>()
                        .HasOne<Product>(ps => ps.Product)
                        .WithMany(p => p.ProductSizes)
                        .HasForeignKey(ps => ps.ProductId);


            builder.Entity<ProductSize>()
                        .HasOne<Size>(ps => ps.Size)
                        .WithMany(s => s.ProductSizes)
                        .HasForeignKey(ps => ps.SizeId);

            builder.Entity<Product>()
                        .HasOne<Category>(p => p.Category)
                        .WithMany(c => c.Products)
                        .HasForeignKey(p => p.CategoryId);

            builder.Entity<Product>()
                        .HasOne<Promotion>(p => p.Promotion)
                        .WithMany(pm => pm.Products)
                        .HasForeignKey(p => p.PromotionId);

            builder.Entity<Image>()
                        .HasOne<Product>(p => p.Product)
                        .WithMany(i => i.Images)
                        .HasForeignKey(p => p.ProductId);

            builder.Entity<District>()
                        .HasOne<City>(p => p.City)
                        .WithMany(i => i.Districts)
                        .HasForeignKey(p => p.CityId);

            builder.Entity<Address>()
                        .HasOne<District>(d => d.District)
                        .WithMany(a => a.Addresses)
                        .HasForeignKey(c => c.DistrictId);

            builder.Entity<Address>()
                        .HasOne<AppUser>(u => u.User)
                        .WithMany(a => a.Addresses)
                        .HasForeignKey(u => u.UserId);

            builder.Entity<Cart>()
                        .HasOne<Product>(p => p.Product)
                        .WithMany(c => c.Carts)
                        .HasForeignKey(p => p.ProductId);

            builder.Entity<Cart>()
                        .HasOne<AppUser>(p => p.User)
                        .WithMany(c => c.Carts)
                        .HasForeignKey(p => p.UserId);

            builder.Entity<Order>()
                        .HasOne<PaymentMenthod>(p => p.PaymentMenthod)
                        .WithMany(o => o.Orders)
                        .HasForeignKey(p => p.PaymentMenthodId);

            builder.Entity<OrderDetail>()
                        .HasOne<Product>(p => p.Product)
                        .WithMany(od => od.OrderDetails)
                        .HasForeignKey(p => p.ProductId);

            builder.Entity<OrderDetail>()
                        .HasOne<Order>(od => od.Order)
                        .WithMany(o => o.OrderDetails)
                        .HasForeignKey(od => od.OrderId);

            builder.Entity<Reviews>()
                        .HasOne<Product>(p => p.Product)
                        .WithMany(r => r.Reviews)
                        .HasForeignKey(p => p.ProductId);

            builder.Entity<Reviews>()
                        .HasOne<AppUser>(u => u.User)
                        .WithMany(r => r.Reviews)
                        .HasForeignKey(u => u.UserId);

            builder.Entity<StockReceived>()
                        .HasOne<AppUser>(u => u.User)
                        .WithMany(s => s.StockReceiveds)
                        .HasForeignKey(u => u.UserId);

            builder.Entity<StockReceivedDetail>()
                        .HasOne<Product>(p => p.Product)
                        .WithMany(s => s.StockReceivedDetails)
                        .HasForeignKey(p => p.ProductId);

            builder.Entity<StockReceivedDetail>()
                        .HasOne<StockReceived>(s => s.StockReceived)
                        .WithMany(r => r.StockReceivedDetails)
                        .HasForeignKey(s => s.StockReceivedId);


            // Bỏ tiền tố AspNet của các bảng: mặc định các bảng trong IdentityDbContext có
            // tên với tiền tố AspNet như: AspNetUserRoles, AspNetUser ...
            // Đoạn mã sau chạy khi khởi tạo DbContext, tạo database sẽ loại bỏ tiền tố đó
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
        }

        public DbSet<TracyShop.Models.Category> Category { get; set; }
        public DbSet<City> Cities { set; get; }
        public DbSet<District> Districts { set; get; }
        public DbSet<TracyShop.Models.Address> Address { set; get; }
        public DbSet<TracyShop.Models.Product> Product { get; set; }
        public DbSet<TracyShop.Models.Image> Image { get; set; }
        public DbSet<TracyShop.Models.Size> Sizes { set; get; }
        public DbSet<ProductSize> ProductSize { set; get; }
        public DbSet<TracyShop.Models.Promotion> Promotion { get; set; }
        public DbSet<Cart> Carts { set; get; }
        public DbSet<UserRole> UserRole { set; get; }

    }
}
