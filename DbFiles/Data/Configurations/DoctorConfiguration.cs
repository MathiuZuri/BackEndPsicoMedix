using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.DbFiles.Data.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("Doctores");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CodigoDoctor)
            .IsRequired()
            .HasMaxLength(30);

        builder.HasIndex(x => x.CodigoDoctor)
            .IsUnique();

        builder.Property(x => x.CMP)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(x => x.CMP)
            .IsUnique();

        builder.Property(x => x.Nombres)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Apellidos)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Especialidad)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(x => x.Celular)
            .HasMaxLength(15);

        builder.Property(x => x.Correo)
            .HasMaxLength(150);

        builder.Property(x => x.FechaInicioContrato)
            .IsRequired();

        builder.Property(x => x.Estado)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(30);

        builder.HasOne(x => x.Usuario)
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Horarios)
            .WithOne(x => x.Doctor)
            .HasForeignKey(x => x.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Citas)
            .WithOne(x => x.Doctor)
            .HasForeignKey(x => x.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Atenciones)
            .WithOne(x => x.Doctor)
            .HasForeignKey(x => x.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}