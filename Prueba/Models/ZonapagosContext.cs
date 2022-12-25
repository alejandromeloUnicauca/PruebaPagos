using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Prueba.Models
{
    public partial class ZonapagosContext : DbContext
    {
        public ZonapagosContext()
        {
        }

        public ZonapagosContext(DbContextOptions<ZonapagosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comercio> Comercio { get; set; }
        public virtual DbSet<Trans> Trans { get; set; }
        public virtual DbSet<TransEstado> TransEstado { get; set; }
        public virtual DbSet<TransMedioPago> TransMedioPago { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings[0].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comercio>(entity =>
            {
                entity.HasKey(e => e.ComercioCodigo);

                entity.Property(e => e.ComercioCodigo)
                    .HasColumnName("comercio_codigo")
                    .ValueGeneratedNever();

                entity.Property(e => e.ComercioDireccion)
                    .HasColumnName("comercio_direccion")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ComercioNit)
                    .IsRequired()
                    .HasColumnName("comercio_nit")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ComercioNombre)
                    .IsRequired()
                    .HasColumnName("comercio_nombre")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ComercioPassword)
                    .HasColumnName("comercio_password")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Trans>(entity =>
            {
                entity.HasKey(e => e.TransCodigo);

                entity.Property(e => e.TransCodigo)
                    .HasColumnName("trans_codigo")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ComercioCodigo).HasColumnName("comercio_codigo");

                entity.Property(e => e.TransConcepto)
                    .HasColumnName("trans_concepto")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TransEstadoId).HasColumnName("trans_estado_id");

                entity.Property(e => e.TransFecha)
                    .IsRequired()
                    .HasColumnName("trans_fecha")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransMediopId).HasColumnName("trans_mediop_id");

                entity.Property(e => e.TransTotal)
                    .HasColumnName("trans_total")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.UsuarioIdentificacion)
                    .IsRequired()
                    .HasColumnName("usuario_identificacion")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.ComercioCodigoNavigation)
                    .WithMany(p => p.Trans)
                    .HasForeignKey(d => d.ComercioCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trans_Comercio");

                entity.HasOne(d => d.TransEstado)
                    .WithMany(p => p.Trans)
                    .HasForeignKey(d => d.TransEstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trans_Trans_Estado");

                entity.HasOne(d => d.TransMediop)
                    .WithMany(p => p.Trans)
                    .HasForeignKey(d => d.TransMediopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trans_Trans_Medio_Pago");

                entity.HasOne(d => d.UsuarioIdentificacionNavigation)
                    .WithMany(p => p.Trans)
                    .HasForeignKey(d => d.UsuarioIdentificacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trans_Usuario");
            });

            modelBuilder.Entity<TransEstado>(entity =>
            {
                entity.ToTable("Trans_Estado");

                entity.Property(e => e.TransEstadoId)
                    .HasColumnName("trans_estado_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.TransEstadoNombre)
                    .HasColumnName("trans_estado_nombre")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TransMedioPago>(entity =>
            {
                entity.HasKey(e => e.TransMediopId);

                entity.ToTable("Trans_Medio_Pago");

                entity.Property(e => e.TransMediopId)
                    .HasColumnName("trans_mediop_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.TransMediopNombre)
                    .HasColumnName("trans_mediop_nombre")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.UsuarioIdentificacion);

                entity.Property(e => e.UsuarioIdentificacion)
                    .HasColumnName("usuario_identificacion")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioEmail)
                    .IsRequired()
                    .HasColumnName("usuario_email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioNombre)
                    .IsRequired()
                    .HasColumnName("usuario_nombre")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioPassword)
                    .HasColumnName("usuario_password")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
