using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models.ATENCIONES.AtencionBase;

namespace psicomedixMonolito.DbFiles.Data.Configurations.ATENCIONES;

public class PsicoSomaticoVegetativoConfiguration : IEntityTypeConfiguration<PsicoSomaticoVegetativo>
{
    public void Configure(EntityTypeBuilder<PsicoSomaticoVegetativo> builder)
    {
        builder.ToTable("PsicoSomaticoVegetativos");
        builder.HasKey(x => x.Id);

        // Campos cortos indexados u optimizados
        builder.Property(x => x.SuenoDuracionInicio).HasMaxLength(50);
        builder.Property(x => x.SuenoDuracionFin).HasMaxLength(50);
        builder.Property(x => x.Peso).HasMaxLength(20);
        builder.Property(x => x.Apetito).HasMaxLength(200);
        builder.Property(x => x.ReflejoPupilar).HasMaxLength(100);
        
        builder.Property(x => x.SaturacionOxigeno).HasColumnType("decimal(5,2)");
    }
}