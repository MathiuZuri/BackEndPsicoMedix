using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.DbFiles.Data.Configurations;

public class PacienteConfiguration : IEntityTypeConfiguration<Paciente>
{
    public void Configure(EntityTypeBuilder<Paciente> builder)
    {
        builder.ToTable("Pacientes");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CodigoPaciente).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.CodigoPaciente).IsUnique();

        builder.Property(x => x.DNI).IsRequired(false).HasMaxLength(20);
        builder.Property(x => x.Nombres).IsRequired(false).HasMaxLength(150);
        builder.Property(x => x.Apellidos).IsRequired(false).HasMaxLength(150);
        builder.Property(x => x.Celular).IsRequired(false).HasMaxLength(20);

        // Registro de la nueva columna de IMC persistente
        builder.Property(x => x.IMC).IsRequired(false);

        builder.Property(x => x.Genero).HasConversion<string>().HasMaxLength(30).IsRequired(false);
        builder.Property(x => x.EstadoCivil).HasConversion<string>().HasMaxLength(30).IsRequired(false);
        builder.Property(x => x.InformanteEstadoCivil).HasConversion<string>().HasMaxLength(30).IsRequired(false);
        builder.Property(x => x.GradoInstruccion).HasConversion<string>().HasMaxLength(50).IsRequired(false);
        builder.Property(x => x.Estado).HasConversion<string>().HasMaxLength(30).IsRequired(true);

        // Relación unificada directa con el módulo de Doctores existente
        builder.HasOne(x => x.Doctor)
            .WithMany()
            .HasForeignKey(x => x.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.RecepcionadoPor)
            .WithMany()
            .HasForeignKey(x => x.RecepcionadoPorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.FamiliaresDirectos)
            .WithOne(f => f.Paciente)
            .HasForeignKey(f => f.PacienteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}