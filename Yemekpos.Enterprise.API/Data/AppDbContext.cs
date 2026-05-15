using Microsoft.EntityFrameworkCore;
using Yemekpos.Enterprise.API.Models;

namespace Yemekpos.Enterprise.API.Data
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        
        // YENİ EKLENENLER
        public DbSet<ModifierGroup> ModifierGroups { get; set; }
        public DbSet<Modifier> Modifiers { get; set; }
        public DbSet<ProductModifierGroup> ProductModifierGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Çoka-çok ilişkiyi kuruyoruz
            modelBuilder.Entity<ProductModifierGroup>()
                .HasKey(pmg => new { pmg.ProductId, pmg.ModifierGroupId });

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("numeric(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("numeric(18,2)");
                
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasColumnType("numeric(18,2)");

            // Yeni eklenen fiyat kolonunun hassasiyeti
            modelBuilder.Entity<Modifier>()
                .Property(m => m.ExtraPrice)
                .HasColumnType("numeric(18,2)");
        }
    }
}