using System.Globalization;
using psicomedixMonolito.DTOs.PDFsDto;
using psicomedixMonolito.Services.Interfacespdf;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

// Namespace de tus DTOs planos

namespace psicomedixMonolito.Services.Documents.Comprobantes.Pdfservicios;

public class CertificadoTrabajoPdfService : ICertificadoTrabajoPdfService
{
    private readonly string ColorPrincipal = "#4DB6D2";
    private readonly string ColorSecundario = "#F089A8";
    private readonly string ColorTexto = Colors.Grey.Darken3;

    public byte[] GeneratePdf(CertificadoTrabajoDto dto)
    {
        var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RECURSOS", "LOGO.jpeg");
        byte[] logoBytes = File.Exists(logoPath) ? File.ReadAllBytes(logoPath) : null;

        var selloPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RECURSOS", "SELLO.png");
        byte[] selloBytes = File.Exists(selloPath) ? File.ReadAllBytes(selloPath) : null;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(35);
                page.PageColor(Colors.White);
                
                // Interlineado y tipografía global
                page.DefaultTextStyle(x => x
                    .FontSize(10)
                    .FontFamily(Fonts.Arial)
                    .FontColor(ColorTexto)
                    .LineHeight(1.5f)
                );

                page.Header().Element(c => ConstruirEncabezado(c, logoBytes));
                page.Content().Element(c => ConstruirContenido(c, dto, selloBytes));
                page.Footer().Element(ConstruirPiePagina);
            });
        }).GeneratePdf();
    }

    private void ConstruirEncabezado(IContainer container, byte[] logo)
    {
        container.PaddingBottom(15).BorderBottom(2).BorderColor(ColorPrincipal).Row(row =>
        {
            if (logo != null)
                row.ConstantItem(120).Image(logo);
            else
                row.ConstantItem(120).Text("LOGO NO ENCONTRADO").FontColor(Colors.Red.Medium);

            row.RelativeItem().Column(column =>
            {
                column.Item().AlignRight().Text("CERTIFICADO DE TRABAJO").Bold().FontSize(20).FontColor(ColorPrincipal);
                // ✅ REBRANDEANDO: Identidad visual de Psicomedix
                column.Item().AlignRight().Text("PSICOMEDIX – Sistema Integral de Gestión Clínica").FontSize(10).FontColor(Colors.Grey.Medium);
                column.Item().AlignRight().Text("RUC: 20600123456 | Jr. Los Andes 123, Juliaca - Perú").FontSize(8).FontColor(Colors.Grey.Darken2);
            });
        });
    }

    private void ConstruirContenido(IContainer container, CertificadoTrabajoDto dto, byte[] sello)
    {
        container.PaddingTop(10).Column(column =>
        {
            column.Spacing(15);

            column.Item().Element(c => CrearSeccion(c, "CERTIFICADO", ColorSecundario, inner =>
            {
                inner.Column(col =>
                {
                    col.Spacing(8);

                    var cultura = new CultureInfo("es-PE");
                    string fechaInicio = dto.FechaInicio.ToString("dd 'de' MMMM 'de' yyyy", cultura);
                    string fechaFin = dto.FechaFin.ToString("dd 'de' MMMM 'de' yyyy", cultura);

                    col.Item().Text(t =>
                    {
                        t.Span("Se certifica que ").FontSize(11);
                        t.Span(dto.NombreCompleto).Bold().FontSize(12);
                        t.Span(", identificado(a) con ").FontSize(11);
                        t.Span(dto.Dni).Bold().FontSize(11);
                        t.Span(", ha desempeñado sus funciones en el área de ").FontSize(11);
                        t.Span(dto.Area).Bold().FontSize(11);
                        // ✅ REBRANDEANDO: Actualizado para el cuerpo del texto
                        t.Span(" de la institución Psicomedix, durante el período comprendido entre el ").FontSize(11);
                        t.Span(fechaInicio).Bold().FontSize(11);
                        t.Span(" y el ").FontSize(11);
                        t.Span(fechaFin).Bold().FontSize(11);
                        t.Span(".").FontSize(11);
                    });

                    col.Item().Text(t =>
                    {
                        t.Span("Durante su permanencia demostró responsabilidad, puntualidad y dedicación en las labores asignadas, cumpliendo satisfactoriamente con los objetivos institucionales.").FontSize(10);
                    });

                    if (!string.IsNullOrEmpty(dto.Cargo))
                    {
                        col.Item().Text(t =>
                        {
                            t.Span("Cargo desempeñado: ").Bold().FontSize(10);
                            t.Span(dto.Cargo).FontSize(10);
                        });
                    }

                    if (dto.Roles != null && dto.Roles.Any())
                    {
                        col.Item().PaddingTop(5).Text(t =>
                        {
                            t.Span("Roles asociados: ").Bold().FontSize(10);
                            t.Span(string.Join(", ", dto.Roles)).FontSize(10);
                        });
                    }

                    col.Item().PaddingTop(10).Text(t =>
                    {
                        t.Span("Código de validación: ").Bold().FontSize(9);
                        t.Span(dto.CodigoCertificado).FontColor(Colors.Grey.Darken2).FontSize(9);
                    });

                    if (!string.IsNullOrEmpty(dto.Observaciones))
                    {
                        col.Item().PaddingTop(5).Text(t =>
                        {
                            t.Span("Observaciones: ").Bold().FontSize(9);
                            t.Span(dto.Observaciones).FontSize(9);
                        });
                    }
                });
            }));

            // --- Firma y sello ---
            column.Item().PaddingTop(30).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("_____________________________").AlignCenter();
                    col.Item().Text(dto.NombreDirector).AlignCenter().Bold().FontSize(11);
                    col.Item().Text(dto.CargoDirector).AlignCenter().FontSize(9);
                    // ✅ REBRANDEANDO: Actualizado en la sección de firmas
                    col.Item().Text("Psicomedix").AlignCenter().FontSize(9);
                });

                if (sello != null)
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Height(80).AlignCenter().Image(sello).FitHeight();
                    });
                }
            });
        });
    }

    private void CrearSeccion(IContainer container, string titulo, string colorTitulo, Action<IContainer> content)
    {
        container.Border(1).BorderColor(Colors.Grey.Lighten2).Column(col =>
        {
            col.Item().Background(colorTitulo).PaddingVertical(4).PaddingHorizontal(8)
               .Text(titulo).Bold().FontSize(11).FontColor(Colors.White);
            col.Item().Padding(8).Element(content);
        });
    }

    private void ConstruirPiePagina(IContainer container)
    {
        container.PaddingTop(15).Row(row =>
        {
            row.RelativeItem().Text(text =>
            {
                // ✅ REBRANDEANDO: Marca de pie de página unificada
                text.Span("Documento generado por el Sistema – Psicomedix ").FontSize(8).FontColor(Colors.Grey.Medium);
                text.Span(" | Teléfono: (051) 123456").FontSize(8).FontColor(Colors.Grey.Medium);
            });
            row.RelativeItem().AlignRight().Text(text =>
            {
                text.Span($"Fecha de impresión: {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(8).FontColor(Colors.Grey.Medium);
            });
        });
    }
}