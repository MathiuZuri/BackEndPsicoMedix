using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.DbFiles.Data.Configurations;

public class ComprobanteConfiguration : IEntityTypeConfiguration<Comprobante>
{
    public void Configure(EntityTypeBuilder<Comprobante> builder)
    {
        builder.ToTable("Comprobantes");

        builder.HasKey(x => x.Id);

        // ==========================================================
        // IDENTIFICACIÓN DEL COMPROBANTE
        // ==========================================================

        builder.Property(x => x.CodigoComprobante)
            .IsRequired()
            .HasMaxLength(60);

        builder.HasIndex(x => x.CodigoComprobante)
            .IsUnique();

        builder.Property(x => x.Serie)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Numero)
            .IsRequired();

        builder.HasIndex(x => new { x.Serie, x.Numero, x.TipoComprobante })
            .IsUnique();

        // ==========================================================
        // ENUMS
        // ==========================================================

        builder.Property(x => x.TipoComprobante)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(40);

        builder.Property(x => x.Estado)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(40);

        builder.Property(x => x.FormatoImpresion)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(40);

        builder.Property(x => x.TipoDocumentoPaciente)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(30);

        // ==========================================================
        // DATOS DEL PACIENTE CONGELADOS EN EL COMPROBANTE
        // ==========================================================

        builder.Property(x => x.NumeroDocumentoPaciente)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.NombrePaciente)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.DireccionPaciente)
            .HasMaxLength(250);

        // ==========================================================
        // MONTOS
        // ==========================================================

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
        // FECHAS Y OBSERVACIONES
        // ==========================================================

        builder.Property(x => x.FechaEmision)
            .IsRequired();

        builder.Property(x => x.FechaAnulacion);

        builder.Property(x => x.MotivoAnulacion)
            .HasMaxLength(500);

        builder.Property(x => x.Observacion)
            .HasMaxLength(500);

        // ==========================================================
        // SNAPSHOT JSON
        // ==========================================================
        // PostgreSQL jsonb no debe recibir string vacío.
        // Por eso se configura como jsonb y con valor por defecto '{}'.

        builder.Property(x => x.DatosSnapshotJson)
            .IsRequired()
            .HasColumnType("jsonb")
            .HasDefaultValueSql("'{}'::jsonb");

        // ==========================================================
        // ÍNDICES
        // ==========================================================

        builder.HasIndex(x => x.PacienteId);
        builder.HasIndex(x => x.PagoId);
        builder.HasIndex(x => x.CitaId);
        builder.HasIndex(x => x.AtencionId);
        builder.HasIndex(x => x.HistorialClinicoId);
        builder.HasIndex(x => x.FechaEmision);
        builder.HasIndex(x => x.Estado);

        // ==========================================================
        // RELACIONES
        // ==========================================================

        builder.HasOne(x => x.Paciente)
            .WithMany(x => x.Comprobantes)
            .HasForeignKey(x => x.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Pago)
            .WithMany(x => x.Comprobantes)
            .HasForeignKey(x => x.PagoId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Cita)
            .WithMany(x => x.Comprobantes)
            .HasForeignKey(x => x.CitaId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Atencion)
            .WithMany(x => x.Comprobantes)
            .HasForeignKey(x => x.AtencionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.HistorialClinico)
            .WithMany(x => x.Comprobantes)
            .HasForeignKey(x => x.HistorialClinicoId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.UsuarioEmision)
            .WithMany(x => x.ComprobantesEmitidos)
            .HasForeignKey(x => x.UsuarioEmisionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.UsuarioAnulacion)
            .WithMany(x => x.ComprobantesAnulados)
            .HasForeignKey(x => x.UsuarioAnulacionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(x => x.Detalles)
            .WithOne(x => x.Comprobante)
            .HasForeignKey(x => x.ComprobanteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}