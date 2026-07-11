using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.DbFiles.Data.Configurations;

public class AuditoriaConfiguration : IEntityTypeConfiguration<Auditoria>
{
    public void Configure(EntityTypeBuilder<Auditoria> builder)
    {
        builder.ToTable("Auditorias");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TipoAccion)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Modulo)
            .IsRequired()
            .HasMaxLength(80);

        builder.Property(x => x.EntidadAfectada)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Descripcion)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.ValorAnterior)
            .HasColumnType("text");

        builder.Property(x => x.ValorNuevo)
            .HasColumnType("text");

        builder.Property(x => x.IpAddress)
            .HasMaxLength(60);

        builder.Property(x => x.UserAgent)
            .HasMaxLength(300);

        builder.Property(x => x.FueExitoso)
            .IsRequired();

        builder.Property(x => x.DetalleError)
            .HasColumnType("text");

        builder.Property(x => x.FechaHora)
            .IsRequired();
        
        builder.Property(x => x.EsConsulta)
            .IsRequired();

        builder.HasIndex(x => x.UsuarioId);
        builder.HasIndex(x => x.FechaHora);
        builder.HasIndex(x => x.TipoAccion);

        builder.HasOne(x => x.Usuario)
            .WithMany(x => x.Auditorias)
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}