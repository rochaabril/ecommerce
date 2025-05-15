using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EcommerceApi.Data
{
    public partial class tienda_mayoristaContext : DbContext
    {
        public tienda_mayoristaContext()
        {
        }

        public tienda_mayoristaContext(DbContextOptions<tienda_mayoristaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Carrito> Carritos { get; set; } = null!;
        public virtual DbSet<Categoria> Categorias { get; set; } = null!;
        public virtual DbSet<ItemCarrito> ItemCarritos { get; set; } = null!;
        public virtual DbSet<ItemOrdene> ItemOrdenes { get; set; } = null!;
        public virtual DbSet<Ordene> Ordenes { get; set; } = null!;
        public virtual DbSet<Producto> Productos { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=CSJG-ICC;Database=tienda_mayorista;User Id=sa;Password=102401;Trust Server Certificate=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Carrito>(entity =>
            {
                entity.ToTable("Carrito");

                entity.Property(e => e.UsuarioId).HasColumnName("Usuario_Id");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Carritos)
                    .HasForeignKey(d => d.UsuarioId)
                    .HasConstraintName("FK__Carrito__Usuario__15502E78");
            });

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.Property(e => e.Nombre).HasMaxLength(100);
            });

            modelBuilder.Entity<ItemCarrito>(entity =>
            {
                entity.ToTable("Item_Carrito");

                entity.Property(e => e.CarritoId).HasColumnName("Carrito_Id");

                entity.Property(e => e.ProductoId).HasColumnName("Producto_Id");

                entity.HasOne(d => d.Carrito)
                    .WithMany(p => p.ItemCarritos)
                    .HasForeignKey(d => d.CarritoId)
                    .HasConstraintName("FK__Item_Carr__Carri__1FCDBCEB");

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.ItemCarritos)
                    .HasForeignKey(d => d.ProductoId)
                    .HasConstraintName("FK__Item_Carr__Produ__20C1E124");
            });

            modelBuilder.Entity<ItemOrdene>(entity =>
            {
                entity.ToTable("Item_Ordenes");

                entity.Property(e => e.OrdenId).HasColumnName("Orden_Id");

                entity.Property(e => e.ProductoId).HasColumnName("Producto_Id");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Orden)
                    .WithMany(p => p.ItemOrdenes)
                    .HasForeignKey(d => d.OrdenId)
                    .HasConstraintName("FK__Item_Orde__Orden__25869641");

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.ItemOrdenes)
                    .HasForeignKey(d => d.ProductoId)
                    .HasConstraintName("FK__Item_Orde__Produ__267ABA7A");
            });

            modelBuilder.Entity<Ordene>(entity =>
            {
                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UsuarioId).HasColumnName("Usuario_Id");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Ordenes)
                    .HasForeignKey(d => d.UsuarioId)
                    .HasConstraintName("FK__Ordenes__Usuario__1B0907CE");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.Property(e => e.ImagenUrl).HasMaxLength(255);

                entity.Property(e => e.Nombre).HasMaxLength(100);

                entity.Property(e => e.Observaciones).HasMaxLength(255);

                entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Categoria)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.CategoriaId)
                    .HasConstraintName("FK__Productos__Categ__08EA5793");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Nombre).HasMaxLength(50);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D105340EA330E9")
                    .IsUnique();

                entity.Property(e => e.Direccion).HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.EmailVerificado).HasColumnName("Email_Verificado");

                entity.Property(e => e.Nombre).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(255);

                entity.Property(e => e.RememberToken)
                    .HasMaxLength(100)
                    .HasColumnName("Remember_Token");

                entity.Property(e => e.Whatsapp).HasMaxLength(15);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Usuarios__RoleId__108B795B");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
