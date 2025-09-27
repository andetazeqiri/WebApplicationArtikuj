using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ArtikujManager.API.Models;

namespace ArtikujManager.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Artikuj> Artikujt { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            
            builder.Entity<Artikuj>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Emri).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Cmimi).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Njesia).HasMaxLength(50);
                entity.Property(e => e.Barkodi).HasMaxLength(100);
                entity.Property(e => e.Lloj).HasMaxLength(20);
                entity.Property(e => e.Tipi).HasMaxLength(20);
                entity.Property(e => e.DataKrijimit).HasDefaultValueSql("datetime('now')");
            });

            
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
            });
        }
    }
}