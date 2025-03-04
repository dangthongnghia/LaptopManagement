using LaptopManagement.Models;
using Microsoft.EntityFrameworkCore;

public class LaptopManagementContext : DbContext
{
    public LaptopManagementContext(DbContextOptions<LaptopManagementContext> options) : base(options) { }

    public DbSet<Laptop> Laptops { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<LaptopImage> LaptopImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
         .HasOne(o => o.Laptop)
         .WithMany(l => l.Orders) // Liên kết với Orders trong Laptop
         .HasForeignKey(o => o.LaptopId)
         .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<LaptopImage>()
            .HasOne(li => li.Laptop)
            .WithMany(l => l.Images)
            .HasForeignKey(li => li.LaptopId)
            .OnDelete(DeleteBehavior.Cascade);
 

        modelBuilder.Entity<Laptop>()
            .Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Laptop>()
        .Property(l => l.Price)
        .HasPrecision(18, 2);

        modelBuilder.Entity<Order>()
            .Property(o => o.CustomerName)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Order>()
            .Property(o => o.CustomerEmail)
            .IsRequired()
            .HasMaxLength(255);

        modelBuilder.Entity<Order>()
            .Property(o => o.CustomerPhone)
            .IsRequired()
            .HasMaxLength(15);

        modelBuilder.Entity<Order>()
            .Property(o => o.Status)
            .IsRequired()
            .HasMaxLength(50);
    }
}