using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models.ATENCIONES.AtencionBase;

namespace psicomedixMonolito.DbFiles.Data.Configurations.ATENCIONES;

public class PsicoEscalasAnimoConfiguration : IEntityTypeConfiguration<PsicoEscalasAnimo>
{
    public void Configure(EntityTypeBuilder<PsicoEscalasAnimo> builder)
    {
        builder.ToTable("PsicoEscalasAnimos");
        builder.HasKey(x => x.Id);
        
        // Las escalas numéricas se quedan como integers nullables por defecto
    }
}