using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.DbFiles.Data.Configurations;

public class HorarioDoctorConfiguration : IEntityTypeConfiguration<HorarioDoctor>
{
    public void Configure(EntityTypeBuilder<HorarioDoctor> builder)
    {
        builder.ToTable("HorariosDoctor");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.DiaSemana)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.HoraInicio)
            .IsRequired();

        builder.Property(x => x.HoraFin)
            .IsRequired();

        builder.Property(x => x.FechaInicioVigencia)
            .IsRequired();

        builder.Property(x => x.Activo)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.DoctorId,
            x.DiaSemana,
            x.HoraInicio,
            x.HoraFin,
            x.FechaInicioVigencia
        });

        builder.HasOne(x => x.Doctor)
            .WithMany(x => x.Horarios)
            .HasForeignKey(x => x.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Citas)
            .WithOne(x => x.HorarioDoctor)
            .HasForeignKey(x => x.HorarioDoctorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}