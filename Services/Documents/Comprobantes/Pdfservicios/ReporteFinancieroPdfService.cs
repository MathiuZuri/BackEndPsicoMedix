using psicomedixMonolito.DTOs.PDFsDto;
using psicomedixMonolito.Services.Interfacespdf;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

// Mapeo a tus DTOs financieros planos

namespace psicomedixMonolito.Services.Documents.Comprobantes.Pdfservicios;

public class ReporteFinancieroPdfService : IReporteFinancieroPdfService
{
    private readonly string ColorPrincipal = "#4DB6D2";
    private readonly string ColorSecundario = "#F089A8";
    private readonly string ColorTexto = Colors.Grey.Darken3;

    public byte[] GeneratePdf(ReporteDiarioDto dto)
    {
        var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RECURSOS", "LOGO.jpeg");
        byte[] logoBytes = File.Exists(logoPath) ? File.ReadAllBytes(logoPath) : null;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(35);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x
                    .FontSize(10)
                    .FontFamily(Fonts.Arial)
                    .FontColor(ColorTexto)
                    .LineHeight(1.5f)
                );

                page.Header().Element(c => ConstruirEncabezado(c, logoBytes));
                page.Content().Element(c => ConstruirContenido(c, dto));
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
                column.Item().AlignRight().Text("REPORTE FINANCIERO").Bold().FontSize(20).FontColor(ColorPrincipal);
                column.Item().AlignRight().Text("Resumen Diario de Ingresos").FontSize(10).FontColor(Colors.Grey.Medium);
                // ✅ REBRANDEANDO: Ajustado con una nomenclatura profesional genérica para Psicomedix
                column.Item().AlignRight().Text("PSICOMEDIX | Jr. Los Andes 123, Juliaca").FontSize(8).FontColor(Colors.Grey.Darken2);
            });
        });
    }

    private void ConstruirContenido(IContainer container, ReporteDiarioDto dto)
    {
        container.PaddingTop(10).Column(column =>
        {
            column.Spacing(15);

            column.Item().Element(c => CrearSeccion(c, "INFORMACIÓN DEL REPORTE", ColorPrincipal, inner =>
            {
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span("Fecha: ").Bold(); t.Span(dto.Fecha.ToString("dd/MM/yyyy")); });
                    row.RelativeItem().Text(t => { t.Span("Total de pagos: ").Bold(); t.Span(dto.CantidadPagos.ToString()); });
                });
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span("Cierre realizado por: ").Bold(); t.Span(dto.CierrePor); });
                    row.RelativeItem().Text(t => { t.Span("Fecha de cierre: ").Bold(); t.Span(dto.FechaCierre?.ToString("dd/MM/yyyy HH:mm") ?? "---"); });
                });
            }));

            column.Item().Element(c => CrearSeccion(c, "RESUMEN EJECUTIVO", ColorSecundario, inner =>
            {
                var total = dto.TotalEfectivo + dto.TotalYape + dto.TotalPlin + dto.TotalTransferencia + dto.TotalTarjeta + dto.TotalOtro;
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span("Total Ingresos: ").Bold(); t.Span($"S/ {dto.TotalIngresos:N2}"); });
                    row.RelativeItem().Text(t => { t.Span("N° Movimientos: ").Bold(); t.Span(dto.CantidadPagos.ToString()); });
                });
                inner.PaddingTop(5).Row(row => {
                    row.RelativeItem().Text(t => { t.Span($"Efectivo: S/ {dto.TotalEfectivo:N2} ({CalcularPorcentaje(dto.TotalEfectivo, total):P0})"); });
                    row.RelativeItem().Text(t => { t.Span($"Yape: S/ {dto.TotalYape:N2} ({CalcularPorcentaje(dto.TotalYape, total):P0})"); });
                });
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span($"Plin: S/ {dto.TotalPlin:N2} ({CalcularPorcentaje(dto.TotalPlin, total):P0})"); });
                    row.RelativeItem().Text(t => { t.Span($"Transferencia: S/ {dto.TotalTransferencia:N2} ({CalcularPorcentaje(dto.TotalTransferencia, total):P0})"); });
                });
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span($"Tarjeta: S/ {dto.TotalTarjeta:N2} ({CalcularPorcentaje(dto.TotalTarjeta, total):P0})"); });
                    row.RelativeItem().Text(t => { t.Span($"Otro: S/ {dto.TotalOtro:N2} ({CalcularPorcentaje(dto.TotalOtro, total):P0})"); });
                });
            }));

            if (dto.Movimientos != null && dto.Movimientos.Any())
            {
                column.Item().PaddingTop(10).Text("MOVIMIENTOS DEL DÍA").Bold().FontSize(12).FontColor(ColorPrincipal);
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns => {
                        columns.RelativeColumn();
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });
                    table.Header(header => {
                        header.Cell().Element(c => CeldaCabecera(c, ColorPrincipal)).Text("Código");
                        header.Cell().Element(c => CeldaCabecera(c, ColorPrincipal)).Text("Paciente");
                        header.Cell().Element(c => CeldaCabecera(c, ColorPrincipal)).Text("Servicio");
                        header.Cell().Element(c => CeldaCabecera(c, ColorPrincipal)).AlignRight().Text("Monto");
                        header.Cell().Element(c => CeldaCabecera(c, ColorPrincipal)).Text("Método");
                        header.Cell().Element(c => CeldaCabecera(c, ColorPrincipal)).Text("Hora");
                    });

                    bool alternar = false;
                    foreach (var mov in dto.Movimientos)
                    {
                        var colorFondo = alternar ? Colors.Grey.Lighten4 : Colors.White;
                        table.Cell().Element(c => CeldaContenido(c, colorFondo)).Text(mov.CodigoPago);
                        table.Cell().Element(c => CeldaContenido(c, colorFondo)).Text(mov.Paciente);
                        table.Cell().Element(c => CeldaContenido(c, colorFondo)).Text(mov.Servicio);
                        table.Cell().Element(c => CeldaContenido(c, colorFondo)).AlignRight().Text($"S/ {mov.Monto:N2}");
                        table.Cell().Element(c => CeldaContenido(c, colorFondo)).Text(mov.MetodoPago);
                        table.Cell().Element(c => CeldaContenido(c, colorFondo)).Text(mov.FechaPago.ToString("HH:mm"));
                        alternar = !alternar;
                    }

                    table.Cell().Element(c => CeldaTotal(c, ColorPrincipal)).Text("TOTAL");
                    table.Cell().Element(c => CeldaTotal(c, ColorPrincipal)).Text("");
                    table.Cell().Element(c => CeldaTotal(c, ColorPrincipal)).Text("");
                    table.Cell().Element(c => CeldaTotal(c, ColorPrincipal)).AlignRight().Text($"S/ {dto.Movimientos.Sum(m => m.Monto):N2}");
                    table.Cell().Element(c => CeldaTotal(c, ColorPrincipal)).Text("");
                    table.Cell().Element(c => CeldaTotal(c, ColorPrincipal)).Text("");
                });
            }

            if (!string.IsNullOrWhiteSpace(dto.Observaciones))
            {
                column.Item().Element(c => CrearSeccion(c, "OBSERVACIONES", ColorPrincipal, inner =>
                {
                    inner.Text(dto.Observaciones);
                }));
            }
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

    private IContainer CeldaCabecera(IContainer container, string colorFondo)
        => container.Background(colorFondo).Padding(5).DefaultTextStyle(x => x.FontColor(Colors.White).Bold());

    private IContainer CeldaContenido(IContainer container, string colorFondo)
        => container.Background(colorFondo).Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten3);

    private IContainer CeldaTotal(IContainer container, string colorFondo)
        => container.Background(colorFondo).Padding(5).DefaultTextStyle(x => x.FontColor(Colors.White).Bold());

    private decimal CalcularPorcentaje(decimal valor, decimal total)
    {
        if (total == 0) return 0;
        return valor / total;
    }

    private void ConstruirPiePagina(IContainer container)
    {
        container.PaddingTop(15).Row(row =>
        {
            row.RelativeItem().Text(text =>
            {
                // ✅ REBRANDEANDO: Ajustado para la marca unificada Psicomedix
                text.Span("Documento generado por el Sistema - Psicomedix").FontSize(8).FontColor(Colors.Grey.Medium);
            });
            row.RelativeItem().AlignRight().Text(text =>
            {
                text.Span($"Fecha de impresión: {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(8).FontColor(Colors.Grey.Medium);
            });
        });
    }
}