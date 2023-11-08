using AyudasSociales.Models;
using Microsoft.EntityFrameworkCore;

namespace AyudasSociales.Data
{
    public class LocalizacionDbContext : DbContext
    {
        public LocalizacionDbContext (DbContextOptions options) : base(options) { 
            
        }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Region> Regiones { get; set; }
        public DbSet<Ciudad> Ciudades { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Region>()
                .HasOne(r => r.Pais)
                .WithMany(p => p.Regiones)
                .HasForeignKey(r => r.PaisId);
            modelBuilder.Entity<Ciudad>()
                .HasOne(c => c.Region)
                .WithMany(r => r.Ciudades)
                .HasForeignKey(c => c.RegionId);
        }
    }
}
