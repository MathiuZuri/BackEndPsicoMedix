using psicomedixMonolito.DTOs.PDFsDto;
using psicomedixMonolito.Services.Interfacespdf;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace psicomedixMonolito.Services.Documents.Comprobantes.Pdfservicios;

public class ResumenPartoPdfService : IResumenPartoPdfService
{
    private readonly string ColorPrincipal = "#4DB6D2";
    private readonly string ColorSecundario = "#F089A8";
    private readonly string ColorTexto = Colors.Grey.Darken3;

    public byte[] GeneratePdf(ResumenPartoPdfDto dto)
    {
        // Ruta del logo relativa al directorio base de ejecución del monolito
        var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RECURSOS", "LOGO.jpeg");
        byte[] logoBytes = File.Exists(logoPath) ? File.ReadAllBytes(logoPath) : null;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(35);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Arial).FontColor(ColorTexto));

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
                column.Item().AlignRight().Text("RESUMEN DE PARTO").Bold().FontSize(20).FontColor(ColorPrincipal);
                column.Item().AlignRight().Text("Partograma y Control del Trabajo de Parto").FontSize(10).FontColor(Colors.Grey.Medium);
            });
        });
    }

    private void ConstruirContenido(IContainer container, ResumenPartoPdfDto dto)
    {
        container.PaddingTop(10).Column(column =>
        {
            column.Spacing(15);

            // --- Datos Generales del Parto ---
            column.Item().Element(c => CrearSeccion(c, "DATOS GENERALES DEL PARTO", ColorPrincipal, inner =>
            {
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span("Paciente: ").Bold(); t.Span(dto.PacienteNombre); });
                    row.RelativeItem().Text(t => { t.Span("DNI: ").Bold(); t.Span(dto.Dni); });
                });
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span("Fecha: ").Bold(); t.Span(dto.FechaParto.ToString("dd/MM/yyyy")); });
                    row.RelativeItem().Text(t => { t.Span("Hora: ").Bold(); t.Span(dto.HoraParto.ToString()); });
                });
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span("Condición: ").Bold(); t.Span(dto.CondicionParto); });
                    row.RelativeItem().Text(t => { t.Span("Atendido por: ").Bold(); t.Span(dto.AtendidoPor); });
                });
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span("Forma de terminación: ").Bold(); t.Span(dto.FormaTerminacion); });
                    row.RelativeItem().Text(t => { t.Span("Medicación expulsivo: ").Bold(); t.Span(dto.MedicacionExpulsivo); });
                });
            }));

            // --- Detalles del Parto ---
            column.Item().Element(c => CrearSeccion(c, "DETALLES DEL PARTO", ColorSecundario, inner =>
            {
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span("Episiotomía: ").Bold(); t.Span(dto.Episiotomia); });
                    row.RelativeItem().Text(t => { t.Span("Desgarros: ").Bold(); t.Span(dto.Desgarros); });
                });
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span("Alumbramiento: ").Bold(); t.Span(dto.Alumbramiento); });
                    row.RelativeItem().Text(t => { t.Span("Modalidad Placenta: ").Bold(); t.Span(dto.ModalidadPlacenta); });
                });
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span("Peso Placenta: ").Bold(); t.Span(dto.PesoPlacenta + " grs."); });
                    row.RelativeItem().Text(t => { t.Span("Líquido amniótico: ").Bold(); t.Span(dto.LiquidoAmniotico + " cc."); });
                });
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span("Color líquido: ").Bold(); t.Span(dto.ColorLiquido); });
                    row.RelativeItem().Text(t => { t.Span("Longitud cordón: ").Bold(); t.Span(dto.LongitudCordon + " cm."); });
                });
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span("Pérdida sanguínea: ").Bold(); t.Span(dto.PerdidaSanguinea + " cc."); });
                    row.RelativeItem().Text("");
                });
                inner.PaddingTop(5).Text(t => { t.Span("Observaciones de la madre: ").Bold(); t.Span(dto.ObservacionesMadre); });
            }));

            // --- Recién Nacido ---
            column.Item().Element(c => CrearSeccion(c, "RECIÉN NACIDO", ColorPrincipal, inner =>
            {
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span("Vivo/Muerto: ").Bold(); t.Span(dto.RnVivoMuerto); });
                    row.RelativeItem().Text(t => { t.Span("Sexo: ").Bold(); t.Span(dto.SexoRN); });
                });
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span("Apgar 1 min: ").Bold(); t.Span(dto.Apgar1Min); });
                    row.RelativeItem().Text(t => { t.Span("Apgar 5 min: ").Bold(); t.Span(dto.Apgar5Min); });
                });
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span("Peso: ").Bold(); t.Span(dto.PesoRN + " grs."); });
                    row.RelativeItem().Text(t => { t.Span("Talla: ").Bold(); t.Span(dto.TallaRN + " cm."); });
                });
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span("PC: ").Bold(); t.Span(dto.PC + " cm."); });
                    row.RelativeItem().Text(t => { t.Span("PT: ").Bold(); t.Span(dto.PT + " cm."); });
                });
                inner.PaddingTop(5).Text(t => { t.Span("Observaciones del RN: ").Bold(); t.Span(dto.ObservacionesRN); });
            }));

            // --- Diagnóstico Post-Parto ---
            column.Item().Element(c => CrearSeccion(c, "DIAGNÓSTICO POST-PARTO", ColorSecundario, inner =>
            {
                inner.Text(dto.DiagnosticoPostParto);
            }));

            // --- Funciones Vitales (tabla) ---
            if (dto.ControlesVitales != null && dto.ControlesVitales.Any())
            {
                column.Item().PaddingTop(10).Text("CONTROLES DE FUNCIONES VITALES").Bold().FontSize(12).FontColor(ColorPrincipal);
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns => {
                        columns.RelativeColumn(); // Hora
                        columns.RelativeColumn(); // PA
                        columns.RelativeColumn(); // Pulso
                        columns.RelativeColumn(); // Temperatura
                        columns.RelativeColumn(); // Respiración
                    });
                    table.Header(header => {
                        header.Cell().Element(c => CeldaCabecera(c, ColorPrincipal)).Text("Hora");
                        header.Cell().Element(c => CeldaCabecera(c, ColorPrincipal)).Text("P/A");
                        header.Cell().Element(c => CeldaCabecera(c, ColorPrincipal)).Text("Pulso");
                        header.Cell().Element(c => CeldaCabecera(c, ColorPrincipal)).Text("Temp");
                        header.Cell().Element(c => CeldaCabecera(c, ColorPrincipal)).Text("Resp");
                    });
                    foreach (var vital in dto.ControlesVitales)
                    {
                        table.Cell().Element(CeldaContenido).Text(vital.Hora.ToString());
                        table.Cell().Element(CeldaContenido).Text(vital.PA);
                        table.Cell().Element(CeldaContenido).Text(vital.Pulso);
                        table.Cell().Element(CeldaContenido).Text(vital.Temperatura);
                        table.Cell().Element(CeldaContenido).Text(vital.Respiracion);
                    }
                });
            }

            // --- Partograma (cuadrícula simplificada) ---
            if (dto.RegistrosPartograma != null && dto.RegistrosPartograma.Any())
            {
                column.Item().PaddingTop(10).Text("PARTOGRAMA (registro horario)").Bold().FontSize(12).FontColor(ColorPrincipal);
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns => {
                        columns.ConstantColumn(40); // Hora
                        columns.RelativeColumn(); // Dilatación
                        columns.RelativeColumn(); // Altura presentación
                        columns.RelativeColumn(); // Dinámica uterina
                        columns.RelativeColumn(); // FCF
                        columns.RelativeColumn(); // Oxitocina
                        columns.RelativeColumn(); // Medicamentos
                    });
                    table.Header(header => {
                        header.Cell().Element(c => CeldaCabecera(c, ColorPrincipal)).Text("Hora");
                        header.Cell().Element(c => CeldaCabecera(c, ColorPrincipal)).Text("Dilat.");
                        header.Cell().Element(c => CeldaCabecera(c, ColorPrincipal)).Text("Alt. Pres.");
                        header.Cell().Element(c => CeldaCabecera(c, ColorPrincipal)).Text("Din. Uter.");
                        header.Cell().Element(c => CeldaCabecera(c, ColorPrincipal)).Text("FCF");
                        header.Cell().Element(c => CeldaCabecera(c, ColorPrincipal)).Text("Oxit.");
                        header.Cell().Element(c => CeldaCabecera(c, ColorPrincipal)).Text("Medic.");
                    });
                    foreach (var reg in dto.RegistrosPartograma)
                    {
                        table.Cell().Element(CeldaContenido).Text(reg.Hora.ToString());
                        table.Cell().Element(CeldaContenido).Text(reg.Dilatacion);
                        table.Cell().Element(CeldaContenido).Text(reg.AlturaPresentacion);
                        table.Cell().Element(CeldaContenido).Text(reg.DinamicaUterina);
                        table.Cell().Element(CeldaContenido).Text(reg.FrecuenciaCardiacaFetal);
                        table.Cell().Element(CeldaContenido).Text(reg.Oxitocina);
                        table.Cell().Element(CeldaContenido).Text(reg.Medicamentos);
                    }
                });
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

    private IContainer CeldaContenido(IContainer container)
        => container.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten3);

    private void ConstruirPiePagina(IContainer container)
    {
        container.PaddingTop(15).Row(row =>
        {
            row.RelativeItem().Text(text =>
            {
                // ✅ REBRANDEANDO: Ajustado para Psicomedix
                text.Span("Documento generado por el Sistema - Psicomedix ").FontSize(8).FontColor(Colors.Grey.Medium);
            });
            row.RelativeItem().AlignRight().Text(text =>
            {
                text.Span($"Fecha de impresión: {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(8).FontColor(Colors.Grey.Medium);
            });
        });
    }
}