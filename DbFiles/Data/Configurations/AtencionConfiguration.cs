using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models.ATENCIONES;
using psicomedixMonolito.Models.ATENCIONES.AtencionBase;

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
            .HasConversion<string>() 
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.ObservacionesIniciales)
            .HasColumnType("text");

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
        // 🚀 NUEVAS RELACIONES MODULARES PSICOLÓGICAS (1 a 1)
        // ========================================================
        builder.HasOne(a => a.AnamnesisHistoria)
            .WithOne(ah => ah.Atencion)
            .HasForeignKey<PsicoAnamnesisHistoria>(ah => ah.AtencionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.SomaticoVegetativo)
            .WithOne(sv => sv.Atencion)
            .HasForeignKey<PsicoSomaticoVegetativo>(sv => sv.AtencionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.EscalasAnimo)
            .WithOne(ea => ea.Atencion)
            .HasForeignKey<PsicoEscalasAnimo>(ea => ea.AtencionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.DesarrolloPsicosocial)
            .WithOne(dp => dp.Atencion)
            .HasForeignKey<PsicoDesarrolloPsicosocial>(dp => dp.AtencionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.EvaluacionCognitiva)
            .WithOne(ec => ec.Atencion)
            .HasForeignKey<PsicoEvaluacionCognitiva>(ec => ec.AtencionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.DiagnosticoCierre)
            .WithOne(dc => dc.Atencion)
            .HasForeignKey<PsicoDiagnosticoCierre>(dc => dc.AtencionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}