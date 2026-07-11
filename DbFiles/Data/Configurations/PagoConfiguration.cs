using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.DbFiles.Data.Configurations;

public class PagoConfiguration : IEntityTypeConfiguration<Pago>
{
    public void Configure(EntityTypeBuilder<Pago> builder)
    {
        builder.ToTable("Pagos");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CodigoPago)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.CodigoPago)
            .IsUnique();

        builder.Property(x => x.MontoTotal)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(x => x.MontoPagado)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(x => x.SaldoPendiente)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(x => x.MontoAdelanto)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(x => x.MetodoPago)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.Estado)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.Observacion)
            .HasMaxLength(500);

        builder.Property(x => x.FechaPago)
            .IsRequired();

        builder.HasIndex(x => x.PacienteId);
        builder.HasIndex(x => x.CitaId);
        builder.HasIndex(x => x.AtencionId);
        builder.HasIndex(x => x.FechaPago);

        builder.HasOne(x => x.Paciente)
            .WithMany(x => x.Pagos)
            .HasForeignKey(x => x.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ServicioClinico)
            .WithMany(x => x.Pagos)
            .HasForeignKey(x => x.ServicioClinicoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Cita)
            .WithMany(x => x.Pagos)
            .HasForeignKey(x => x.CitaId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Atencion)
            .WithMany(x => x.Pagos)
            .HasForeignKey(x => x.AtencionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.UsuarioRegistro)
            .WithMany()
            .HasForeignKey(x => x.UsuarioRegistroId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}