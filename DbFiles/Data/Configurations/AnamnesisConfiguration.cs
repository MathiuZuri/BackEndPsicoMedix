using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models.ATENCIONES;

namespace psicomedixMonolito.DbFiles.Data.Configurations;

public class AnamnesisConfiguration : IEntityTypeConfiguration<Anamnesis>
{
    public void Configure(EntityTypeBuilder<Anamnesis> builder)
    {
        builder.ToTable("Anamnesis");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.MotivoConsulta).IsRequired().HasMaxLength(1500);
        builder.Property(x => x.EdadGestacional).HasMaxLength(50);
        
        builder.Property(x => x.Alergias).HasMaxLength(500);
        builder.Property(x => x.EnfermedadesCronicas).HasMaxLength(500);
        builder.Property(x => x.CirugiasPrevias).HasMaxLength(500);
        builder.Property(x => x.AntecedentesAdicionales).HasMaxLength(1500);
    }
}