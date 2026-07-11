using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.DbFiles.Data.Configurations;

public class ServicioClinicoConfiguration : IEntityTypeConfiguration<ServicioClinico>
{
    public void Configure(EntityTypeBuilder<ServicioClinico> builder)
    {
        builder.ToTable("ServiciosClinicos");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CodigoServicio)
            .IsRequired()
            .HasMaxLength(30);

        builder.HasIndex(x => x.CodigoServicio)
            .IsUnique();

        builder.Property(x => x.Nombre)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(x => x.Descripcion)
            .HasMaxLength(300);

        builder.Property(x => x.CostoBase)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(x => x.DuracionMinutos)
            .IsRequired();

        builder.Property(x => x.RequiereCita)
            .IsRequired();

        builder.Property(x => x.GeneraHistorial)
            .IsRequired();

        builder.Property(x => x.Estado)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(30);

        builder.HasMany(x => x.Citas)
            .WithOne(x => x.ServicioClinico)
            .HasForeignKey(x => x.ServicioClinicoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Atenciones)
            .WithOne(x => x.ServicioClinico)
            .HasForeignKey(x => x.ServicioClinicoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Pagos)
            .WithOne(x => x.ServicioClinico)
            .HasForeignKey(x => x.ServicioClinicoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}