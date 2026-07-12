using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models.ATENCIONES.AtencionBase;

namespace psicomedixMonolito.DbFiles.Data.Configurations.ATENCIONES;

public class PsicoAnamnesisHistoriaConfiguration : IEntityTypeConfiguration<PsicoAnamnesisHistoria>
{
    public void Configure(EntityTypeBuilder<PsicoAnamnesisHistoria> builder)
    {
        builder.ToTable("PsicoAnamnesisHistoriales");
        builder.HasKey(x => x.Id);

        // Optimizamos los campos que no requieren texto ilimitado
        builder.Property(x => x.CigarrillosVape).HasMaxLength(200);
        builder.Property(x => x.Suplementos).HasMaxLength(500);
    }
}