using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Examen_PROAGRO.Models;

public partial class ProagrodbContext : DbContext
{
    public ProagrodbContext()
    {
    }

    public ProagrodbContext(DbContextOptions<ProagrodbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Restaurante> Restaurantes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Restaurante>(entity =>
        {
            entity.HasKey(e => e.FsqId).HasName("PK__Restaura__335F75E05EF4F5CD");

            entity.Property(e => e.FsqId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("fsq_id");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FormattedAddress)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("formatted_address");
            entity.Property(e => e.Foto)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("foto");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Rating)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("rating");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
