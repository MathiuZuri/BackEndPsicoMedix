using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models.ATENCIONES.AtencionBase;

namespace psicomedixMonolito.DbFiles.Data.Configurations.ATENCIONES;

public class PsicoDesarrolloPsicosocialConfiguration : IEntityTypeConfiguration<PsicoDesarrolloPsicosocial>
{
    public void Configure(EntityTypeBuilder<PsicoDesarrolloPsicosocial> builder)
    {
        builder.ToTable("PsicoDesarrollosPsicosociales");
        builder.HasKey(x => x.Id);
    }
}