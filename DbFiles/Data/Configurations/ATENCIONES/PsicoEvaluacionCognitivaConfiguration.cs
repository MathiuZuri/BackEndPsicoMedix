using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models.ATENCIONES.AtencionBase;

namespace psicomedixMonolito.DbFiles.Data.Configurations.ATENCIONES;

public class PsicoEvaluacionCognitivaConfiguration : IEntityTypeConfiguration<PsicoEvaluacionCognitiva>
{
    public void Configure(EntityTypeBuilder<PsicoEvaluacionCognitiva> builder)
    {
        builder.ToTable("PsicoEvaluacionesCognitivas");
        builder.HasKey(x => x.Id);
    }
}