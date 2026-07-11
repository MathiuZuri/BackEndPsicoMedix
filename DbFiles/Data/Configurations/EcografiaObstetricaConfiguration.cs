using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models.ATENCIONES;

namespace psicomedixMonolito.DbFiles.Data.Configurations;

public class EcografiaObstetricaConfiguration : IEntityTypeConfiguration<EcografiaObstetrica>
{
    public void Configure(EntityTypeBuilder<EcografiaObstetrica> builder)
    {
        builder.ToTable("EcografiasObstetricas");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PlacentaLocalizacion).HasMaxLength(100);
        builder.Property(x => x.PlacentaGranum).HasMaxLength(20); // 0, I, II, III
        builder.Property(x => x.Conclusiones).HasMaxLength(1000);
        
        // Optimización para decimales en PostgreSQL
        builder.Property(x => x.IndiceLiquidoAmniotico).HasColumnType("decimal(5,2)");
    }
}