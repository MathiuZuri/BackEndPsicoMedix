using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.DbFiles.Data.Configurations;

public class HistorialClinicoConfiguration : IEntityTypeConfiguration<HistorialClinico>
{
    public void Configure(EntityTypeBuilder<HistorialClinico> builder)
    {
        builder.ToTable("HistorialesClinicos");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CodigoHistorial)
            .IsRequired()
            .HasMaxLength(40);

        builder.HasIndex(x => x.CodigoHistorial)
            .IsUnique();

        builder.HasIndex(x => x.PacienteId)
            .IsUnique();

        builder.Property(x => x.FechaApertura)
            .IsRequired();

        builder.Property(x => x.Estado)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(30);

        builder.HasOne(x => x.Paciente)
            .WithOne(x => x.HistorialClinico)
            .HasForeignKey<HistorialClinico>(x => x.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Detalles)
            .WithOne(x => x.HistorialClinico)
            .HasForeignKey(x => x.HistorialClinicoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}