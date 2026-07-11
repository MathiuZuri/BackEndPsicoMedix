using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models.ATENCIONES;

namespace psicomedixMonolito.DbFiles.Data.Configurations;

public class ExamenFisicoConfiguration : IEntityTypeConfiguration<ExamenFisico>
{
    public void Configure(EntityTypeBuilder<ExamenFisico> builder)
    {
        builder.ToTable("ExamenesFisicos");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.EstadoGeneral).HasMaxLength(50);
        builder.Property(x => x.EstadoHidratacion).HasMaxLength(50);
        builder.Property(x => x.EstadoNutricion).HasMaxLength(50);
        
        builder.Property(x => x.SituacionPosicionPresentacion).HasMaxLength(100);
        builder.Property(x => x.MovimientosFetales).HasMaxLength(50);
        builder.Property(x => x.TonoUterino).HasMaxLength(50);
        builder.Property(x => x.DinamicaUterina).HasMaxLength(100);
        
        builder.Property(x => x.ColorLiquidoAmniotico).HasMaxLength(50);
        builder.Property(x => x.PunoPercusionLumbar).HasMaxLength(50);
        builder.Property(x => x.Edemas).HasMaxLength(50);
        builder.Property(x => x.ReflejosOsteotendinosos).HasMaxLength(50);
    }
}