using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using psicomedixMonolito.Models.ATENCIONES.AtencionBase;

namespace psicomedixMonolito.DbFiles.Data.Configurations.ATENCIONES;

public class PsicoDiagnosticoCierreConfiguration : IEntityTypeConfiguration<PsicoDiagnosticoCierre>
{
    public void Configure(EntityTypeBuilder<PsicoDiagnosticoCierre> builder)
    {
        builder.ToTable("PsicoDiagnosticosCierres");
        builder.HasKey(x => x.Id);
    }
}