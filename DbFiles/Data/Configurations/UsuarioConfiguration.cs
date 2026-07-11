using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.DbFiles.Data.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CodigoUsuario)
            .IsRequired()
            .HasMaxLength(30);

        builder.HasIndex(x => x.CodigoUsuario)
            .IsUnique();

        builder.Property(x => x.Nombres)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Apellidos)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.UserName)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.UserName)
            .IsUnique();

        builder.Property(x => x.Correo)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(x => x.Correo)
            .IsUnique();

        builder.Property(x => x.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Estado)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.FechaRegistro)
            .IsRequired();

        builder.HasMany(x => x.UsuarioRoles)
            .WithOne(x => x.Usuario)
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Auditorias)
            .WithOne(x => x.Usuario)
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}