using LaptopManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LaptopManagement.Data
{
    public class LaptopManagementContext : DbContext
    {
        public LaptopManagementContext(DbContextOptions<LaptopManagementContext> options) : base(options) { }

        // DbSet properties
        public DbSet<Laptop> Laptops { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<LaptopImage> LaptopImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình mối quan hệ giữa Laptop và Category
            modelBuilder.Entity<Laptop>()
                .HasOne(l => l.Category)
                .WithMany(c => c.Laptops)
                .HasForeignKey(l => l.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            // Cấu hình mối quan hệ giữa LaptopImage và Laptop
            modelBuilder.Entity<LaptopImage>()
                .HasOne(li => li.Laptop)
                .WithMany(l => l.Images)
                .HasForeignKey(li => li.LaptopId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình mối quan hệ giữa OrderDetail và Order
            modelBuilder.Entity<OrderDetail>()
    .HasOne(od => od.Order)
    .WithMany(o => o.OrderDetails)
    .HasForeignKey(od => od.OrderId)
    .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình mối quan hệ giữa OrderDetail và Laptop
            modelBuilder.Entity<OrderDetail>()
    .HasOne(od => od.Order)
    .WithMany(o => o.OrderDetails)
    .HasForeignKey(od => od.OrderId)
    .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình mối quan hệ giữa CartItem và Laptop
            modelBuilder.Entity<CartItem>()
               .HasOne(ci => ci.Laptop)
               .WithMany()
               .HasForeignKey(ci => ci.LaptopId)
               .OnDelete(DeleteBehavior.Cascade);


            // Cấu hình các thuộc tính decimal để đảm bảo độ chính xác
            modelBuilder.Entity<Laptop>()
                .Property(l => l.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.UnitPrice)
                .HasColumnType("decimal(18,2)");

            // Cấu hình các thuộc tính khác
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.FullName)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Laptop>()
                .Property(l => l.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Laptop>()
                .Property(l => l.Brand)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Order>()
                .Property(o => o.CustomerName)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Order>()
                .Property(o => o.CustomerEmail)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Order>()
                .Property(o => o.CustomerPhone)
                .HasMaxLength(15)
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Cấu hình kết nối cơ sở dữ liệu của bạn ở đây nếu cần
            }

            // Bỏ qua cảnh báo về thay đổi mô hình chưa được áp dụng
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
    }
}
