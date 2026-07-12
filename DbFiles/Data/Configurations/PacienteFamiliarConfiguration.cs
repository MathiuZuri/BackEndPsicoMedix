using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.DbFiles.Data.Configurations;

public class PacienteFamiliarConfiguration : IEntityTypeConfiguration<PacienteFamiliar>
{
    public void Configure(EntityTypeBuilder<PacienteFamiliar> builder)
    {
        builder.ToTable("PacienteFamiliares");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nombres).IsRequired(false).HasMaxLength(150);
        builder.Property(x => x.Ocupacion).IsRequired(false).HasMaxLength(150);
        builder.Property(x => x.Parentesco).HasConversion<string>().HasMaxLength(30).IsRequired(false);
    }
}