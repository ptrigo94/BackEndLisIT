using Autorizacion.Models;
using AyudasSociales.Models;
using Microsoft.EntityFrameworkCore;

namespace Autorizacion.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext()
        {

        }
        public AuthDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AyudaSocial> AyudasSociales {get; set;}
        public DbSet<AyudaUsuario> AyudasUsuarios { get; set; }

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
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Ciudad)
                .WithMany(c => c.Usuarios)
                .HasForeignKey(u => u.CiudadId);

            //Ayudas para usuarios
            modelBuilder.Entity<AyudaUsuario>()
                .HasOne(a => a.Ayuda )
                .WithMany(au => au.AyudaUsuario)
                .HasForeignKey(u => u.AyudaSocialId);
            modelBuilder.Entity<AyudaUsuario>()
                .HasOne(a => a.User)
                .WithMany(u => u.AyudaUsuario)
                .HasForeignKey(u => u.UserId);

            //Ayudas para Comunas
            modelBuilder.Entity<AyudaComuna>()
                .HasOne(a => a.Ayuda)
                .WithMany(au => au.AyudaComunas)
                .HasForeignKey(u => u.AyudaSocialId);
            modelBuilder.Entity<AyudaComuna>()
                .HasOne(a => a.Ciudad)
                .WithMany(u => u.AyudaComunas)
                .HasForeignKey(u => u.CiudadId);

            //Ayudas para Regiones
            modelBuilder.Entity<AyudaRegion>()
                .HasOne(a => a.Ayuda)
                .WithMany(au => au.AyudaRegiones)
                .HasForeignKey(u => u.AyudaSocialId);

            modelBuilder.Entity<AyudaRegion>()
                .HasOne(a => a.Region)
                .WithMany(u => u.AyudaRegiones)
                .HasForeignKey(u => u.RegionId);


        }


        public DbSet<AyudasSociales.Models.AyudaComuna> AyudaComuna { get; set; }


        public DbSet<AyudasSociales.Models.AyudaRegion> AyudaRegion { get; set; }



    }
}
