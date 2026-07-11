using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.DbFiles.Data.Configurations;

public class AjusteFinancieroConfiguration : IEntityTypeConfiguration<AjusteFinanciero>
{
    public void Configure(EntityTypeBuilder<AjusteFinanciero> builder)
    {
        builder.ToTable("AjustesFinancieros");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TipoAjuste)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.MontoAjuste)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(x => x.Motivo)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Observacion)
            .HasMaxLength(500);

        builder.Property(x => x.FechaRegistro)
            .IsRequired();

        builder.HasIndex(x => x.PagoId);
        builder.HasIndex(x => x.AtencionId);
        builder.HasIndex(x => x.PacienteId);
        builder.HasIndex(x => x.TipoAjuste);
        builder.HasIndex(x => x.FechaRegistro);

        builder.HasOne(x => x.Pago)
            .WithMany(x => x.AjustesFinancieros)
            .HasForeignKey(x => x.PagoId)
            .OnDelete(DeleteBehavior.Restrict);

        // CORRECCIÓN AQUÍ: WithMany() vacío para Atencion
        builder.HasOne(x => x.Atencion)
            .WithMany() 
            .HasForeignKey(x => x.AtencionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Paciente)
            .WithMany()
            .HasForeignKey(x => x.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.UsuarioRegistro)
            .WithMany()
            .HasForeignKey(x => x.UsuarioRegistroId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}