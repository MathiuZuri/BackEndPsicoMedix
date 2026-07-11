using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models;
using psicomedixMonolito.Models.ATENCIONES;

namespace psicomedixMonolito.DbFiles.Data.Configurations;

public class CitaConfiguration : IEntityTypeConfiguration<Cita>
{
    public void Configure(EntityTypeBuilder<Cita> builder)
    {
        builder.ToTable("Citas");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CodigoCita)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.CodigoCita)
            .IsUnique();

        builder.Property(x => x.Fecha)
            .IsRequired();

        builder.Property(x => x.HoraInicio)
            .IsRequired();

        builder.Property(x => x.HoraFin)
            .IsRequired();

        builder.Property(x => x.Motivo)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.Observaciones)
            .HasMaxLength(500);

        builder.Property(x => x.Estado)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.FechaRegistro)
            .IsRequired();

        builder.HasIndex(x => new { x.DoctorId, x.Fecha, x.HoraInicio, x.HoraFin });
        builder.HasIndex(x => new { x.PacienteId, x.Fecha });

        builder.HasOne(x => x.Paciente)
            .WithMany(x => x.Citas)
            .HasForeignKey(x => x.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Doctor)
            .WithMany(x => x.Citas)
            .HasForeignKey(x => x.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ServicioClinico)
            .WithMany(x => x.Citas)
            .HasForeignKey(x => x.ServicioClinicoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.HorarioDoctor)
            .WithMany(x => x.Citas)
            .HasForeignKey(x => x.HorarioDoctorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.UsuarioRegistro)
            .WithMany()
            .HasForeignKey(x => x.UsuarioRegistroId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Atencion)
            .WithOne(x => x.Cita)
            .HasForeignKey<Atencion>(x => x.CitaId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}