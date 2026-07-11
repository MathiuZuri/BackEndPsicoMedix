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

        builder.Property(x => x.CodigoPaciente)
            .IsRequired()
            .HasMaxLength(30);

        builder.HasIndex(x => x.CodigoPaciente)
            .IsUnique();

        builder.Property(x => x.DNI)
            .IsRequired()
            .HasMaxLength(11);

        builder.HasIndex(x => x.DNI)
            .IsUnique();

        builder.Property(x => x.Nombres)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Apellidos)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Sexo)
            .IsRequired()
            .HasMaxLength(1);

        builder.Property(x => x.Celular)
            .HasMaxLength(15);

        builder.Property(x => x.Correo)
            .HasMaxLength(150);

        builder.Property(x => x.Direccion)
            .HasMaxLength(250);

        // --- Configuración de Filiación ---
        builder.Property(x => x.LugarNacimiento).HasMaxLength(150);
        builder.Property(x => x.GradoInstruccion).HasMaxLength(100);
        builder.Property(x => x.Ocupacion).HasMaxLength(150);
        builder.Property(x => x.Religion).HasMaxLength(50);
        builder.Property(x => x.EstadoCivil).HasMaxLength(50);
        builder.Property(x => x.NombrePareja).HasMaxLength(150);
        builder.Property(x => x.CelularPareja).HasMaxLength(15);

        builder.Property(x => x.Estado)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.FechaRegistro)
            .IsRequired();

        builder.HasOne(x => x.Usuario)
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.HistorialClinico)
            .WithOne(x => x.Paciente)
            .HasForeignKey<HistorialClinico>(x => x.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Citas)
            .WithOne(x => x.Paciente)
            .HasForeignKey(x => x.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Atenciones)
            .WithOne(x => x.Paciente)
            .HasForeignKey(x => x.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Pagos)
            .WithOne(x => x.Paciente)
            .HasForeignKey(x => x.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}