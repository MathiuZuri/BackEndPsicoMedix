using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models.ATENCIONES;

namespace psicomedixMonolito.DbFiles.Data.Configurations;

public class AtencionConfiguration : IEntityTypeConfiguration<Atencion>
{
    public void Configure(EntityTypeBuilder<Atencion> builder)
    {
        builder.ToTable("Atenciones");

        builder.HasKey(x => x.Id);

        // ==========================================
        // PROPIEDADES BASE
        // ==========================================
        builder.Property(x => x.CodigoAtencion)
            .IsRequired()
            .HasMaxLength(30);

        builder.HasIndex(x => x.CodigoAtencion)
            .IsUnique();

        builder.Property(x => x.FechaInicio)
            .IsRequired();

        builder.Property(x => x.Estado)
            .HasConversion<string>() // Asume que EstadoAtencion es un Enum
            .IsRequired()
            .HasMaxLength(30);

        // ==========================================
        // RELACIONES CORE (Finanzas y Citas)
        // ==========================================
        builder.HasOne(x => x.Paciente)
            .WithMany(p => p.Atenciones)
            .HasForeignKey(x => x.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Doctor)
            .WithMany(d => d.Atenciones)
            .HasForeignKey(x => x.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasOne(x => x.ServicioClinico)
            .WithMany(s => s.Atenciones)
            .HasForeignKey(x => x.ServicioClinicoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Cita)
            .WithOne(c => c.Atencion)
            .HasForeignKey<Atencion>(x => x.CitaId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(x => x.Pagos)
            .WithOne(p => p.Atencion)
            .HasForeignKey(p => p.AtencionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Comprobantes)
            .WithOne(c => c.Atencion)
            .HasForeignKey(c => c.AtencionId)
            .OnDelete(DeleteBehavior.Restrict);

        // ========================================================
        // RELACIONES MODULARES (Independientes)
        // ========================================================
        
        // Módulos 1 a 1
        builder.HasOne(a => a.Anamnesis)
            .WithOne(an => an.Atencion)
            .HasForeignKey<Anamnesis>(an => an.AtencionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.ImpresionDiagnostica)
            .WithOne(id => id.Atencion)
            .HasForeignKey<ImpresionDiagnostica>(id => id.AtencionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Módulos 1 a N (Permite registrar múltiples durante una evolución)
        builder.HasMany(a => a.ExamenesFisicos)
            .WithOne(ef => ef.Atencion)
            .HasForeignKey(ef => ef.AtencionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.TactosVaginales)
            .WithOne(tv => tv.Atencion)
            .HasForeignKey(tv => tv.AtencionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Ecografias)
            .WithOne(eco => eco.Atencion)
            .HasForeignKey(eco => eco.AtencionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}