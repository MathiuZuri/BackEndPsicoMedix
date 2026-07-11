using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.DbFiles.Data.Configurations;

public class ComprobanteDetalleConfiguration : IEntityTypeConfiguration<ComprobanteDetalle>
{
    public void Configure(EntityTypeBuilder<ComprobanteDetalle> builder)
    {
        builder.ToTable("ComprobanteDetalles");

        builder.HasKey(x => x.Id);

        // ==========================================================
        // DATOS DEL SERVICIO
        // ==========================================================

        builder.Property(x => x.CodigoServicio)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Descripcion)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.Cantidad)
            .IsRequired();

        // ==========================================================
        // MONTOS
        // ==========================================================

        builder.Property(x => x.PrecioUnitarioFinal)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(x => x.Subtotal)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(x => x.TasaImpuesto)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(x => x.MontoImpuesto)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(x => x.Total)
            .HasPrecision(10, 2)
            .IsRequired();

        // ==========================================================
        // RELACIÓN CON COMPROBANTE
        // ==========================================================

        builder.HasIndex(x => x.ComprobanteId);

        builder.HasOne(x => x.Comprobante)
            .WithMany(x => x.Detalles)
            .HasForeignKey(x => x.ComprobanteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}