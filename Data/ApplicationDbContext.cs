using Evoltis.Models;
using Microsoft.EntityFrameworkCore;

namespace Evoltis.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Domicilio> Domicilios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.ID);

                entity.Property(u => u.Nombre)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(u => u.FechaCreacion)
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Domicilio>(entity =>
            {
                entity.HasKey(d => d.ID);

                entity.Property(d => d.Calle)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(d => d.Numero)
                      .HasMaxLength(10);

                entity.Property(d => d.Provincia)
                      .HasMaxLength(50);

                entity.Property(d => d.Ciudad)
                      .HasMaxLength(50);

                entity.Property(d => d.FechaCreacion)
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Relación 1 a 1
                entity.HasOne(d => d.Usuario)
                      .WithOne(u => u.Domicilio)
                      .HasForeignKey<Domicilio>(d => d.UsuarioID)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
