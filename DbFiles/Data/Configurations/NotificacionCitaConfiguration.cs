using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.DbFiles.Data.Configurations;

public class NotificacionCitaConfiguration : IEntityTypeConfiguration<NotificacionCita>
{
    public void Configure(EntityTypeBuilder<NotificacionCita> builder)
    {
        builder.ToTable("NotificacionesCitas");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TelefonoDestino)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Mensaje)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Canal)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.Estado)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.Intentos)
            .IsRequired();

        builder.Property(x => x.Error)
            .HasMaxLength(1000);

        builder.Property(x => x.FechaCreacion)
            .IsRequired();

        builder.HasOne(x => x.Cita)
            .WithMany(x => x.Notificaciones)
            .HasForeignKey(x => x.CitaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Enlace bidireccional armónico con la colección de la entidad Paciente
        builder.HasOne(x => x.Paciente)
            .WithMany(x => x.NotificacionesCita)
            .HasForeignKey(x => x.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.CitaId);
        builder.HasIndex(x => x.PacienteId);
        builder.HasIndex(x => new { x.Estado, x.FechaProgramadaEnvio });
    }
}