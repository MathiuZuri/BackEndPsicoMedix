using psicomedixMonolito.DTOs.PDFsDto;
using psicomedixMonolito.Services.Interfacespdf;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

// Namespace unificado de tus DTOs

namespace psicomedixMonolito.Services.Documents.Comprobantes.Pdfservicios;

public class HistoriaClinicaPdfService : IHistoriaClinicaPdfService
{
    private readonly string ColorPrincipal = "#4DB6D2"; // Celeste corporativo
    private readonly string ColorSecundario = "#F089A8"; // Rosado corporativo
    private readonly string ColorTexto = Colors.Grey.Darken3;

    public HistoriaClinicaPdfService()
    {
    }

    public byte[] GeneratePdf(HistoriaClinicaPdfDto dto)
    {
        // Lectura física directa del recurso en el directorio de ejecución del monolito
        var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RECURSOS", "LOGO.jpeg");
        
        byte[] logoBytes = null;
        if (File.Exists(logoPath))
        {
            logoBytes = File.ReadAllBytes(logoPath);
        }

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
            {
                row.ConstantItem(120).Image(logo);
            }
            else
            {
                row.ConstantItem(120).Text("LOGO NO ENCONTRADO").FontColor(Colors.Red.Medium);
            }

            row.RelativeItem().Column(column =>
            {
                column.Item().AlignRight().Text("HISTORIA CLÍNICA").Bold().FontSize(20).FontColor(ColorPrincipal);
                column.Item().AlignRight().Text("Sistema Integral de Gestión Clínica").FontSize(10).FontColor(Colors.Grey.Medium);
            });
        });
    }

    private void ConstruirContenido(IContainer container, HistoriaClinicaPdfDto dto)
    {
        container.PaddingTop(10).Column(column =>
        {
            column.Spacing(15);

            // --- Ficha Identificación ---
            column.Item().Element(c => CrearSeccion(c, "FICHA DE IDENTIFICACIÓN", ColorPrincipal, inner =>
            {
                inner.Row(row =>
                {
                    row.RelativeItem().Column(col => {
                        col.Item().Text(t => { t.Span("Paciente: ").Bold(); t.Span(dto.NombresApellidos); });
                        col.Item().Text(t => { t.Span("DNI: ").Bold(); t.Span(dto.Dni); });
                        col.Item().Text(t => { t.Span("Fecha Nac.: ").Bold(); t.Span(dto.FechaNacimiento.ToString("dd/MM/yyyy")); });
                        col.Item().Text(t => { t.Span("Sexo: ").Bold(); t.Span(dto.Sexo); });
                        col.Item().Text(t => { t.Span("Dirección: ").Bold(); t.Span(dto.Direccion); });
                    });
                    row.RelativeItem().Column(col => {
                        col.Item().Text(t => { t.Span("N° de H.C: ").Bold(); t.Span(dto.NumeroHistoria).FontColor(Colors.Red.Medium); });
                        col.Item().Text(t => { t.Span("Fecha Registro: ").Bold(); t.Span(dto.FechaRegistro.ToString("dd/MM/yyyy")); });
                        col.Item().Text(t => { t.Span("Celular: ").Bold(); t.Span(dto.Celular); });
                        col.Item().Text(t => { t.Span("Ocupación: ").Bold(); t.Span(dto.Ocupacion); });
                    });
                });
                inner.PaddingTop(5).Text(t => { t.Span("Motivo de Consulta: ").Bold(); t.Span(dto.MotivoConsulta); });
            }));

            // --- Antecedentes Gineco-Obstétricos ---
            column.Item().Element(c => CrearSeccion(c, "ANTECEDENTES GINECO-OBSTÉTRICOS", ColorSecundario, inner =>
            {
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span("Menarquia: ").Bold(); t.Span(dto.Menarquia); });
                    row.RelativeItem().Text(t => { t.Span("R/C: ").Bold(); t.Span(dto.RitmoCatamenial); });
                    row.RelativeItem().Text(t => { t.Span("Gesta: ").Bold(); t.Span(dto.Gesta.ToString()); });
                    row.RelativeItem().Text(t => { t.Span("Partos: ").Bold(); t.Span(dto.Partos.ToString()); });
                });
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span("Abortos: ").Bold(); t.Span(dto.Abortos.ToString()); });
                    row.RelativeItem().Text(t => { t.Span("Hijos Vivos: ").Bold(); t.Span(dto.HijosVivos.ToString()); });
                    row.RelativeItem().Text(t => { t.Span("FUR: ").Bold(); t.Span(dto.FUR.HasValue ? dto.FUR.Value.ToString("dd/MM/yyyy") : "---"); });
                    row.RelativeItem().Text(t => { t.Span("FPP: ").Bold(); t.Span(dto.FPP.HasValue ? dto.FPP.Value.ToString("dd/MM/yyyy") : "---"); });
                });
                inner.PaddingTop(5).Text(t => { t.Span("Método Anticonceptivo: ").Bold(); t.Span(dto.MetodoAnticonceptivo); });
            }));

            // --- Funciones Vitales ---
            column.Item().Element(c => CrearSeccion(c, "FUNCIONES VITALES", ColorPrincipal, inner =>
            {
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span("P/A: ").Bold(); t.Span(dto.PA); });
                    row.RelativeItem().Text(t => { t.Span("Pulso: ").Bold(); t.Span(dto.Pulso); });
                    row.RelativeItem().Text(t => { t.Span("Temp: ").Bold(); t.Span(dto.Temperatura); });
                    row.RelativeItem().Text(t => { t.Span("Resp: ").Bold(); t.Span(dto.Respiracion); });
                });
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span("SO2: ").Bold(); t.Span(dto.SO2); });
                    row.RelativeItem().Text(t => { t.Span("Peso: ").Bold(); t.Span(dto.Peso); });
                    row.RelativeItem().Text(t => { t.Span("Talla: ").Bold(); t.Span(dto.Talla); });
                    row.RelativeItem().Text(""); 
                });
            }));

            // --- Examen Obstétrico ---
            column.Item().Element(c => CrearSeccion(c, "EXAMEN OBSTÉTRICO", ColorSecundario, inner =>
            {
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span("Altura Uterina: ").Bold(); t.Span(dto.AlturaUterina); });
                    row.RelativeItem().Text(t => { t.Span("Situación: ").Bold(); t.Span(dto.Situacion); });
                    row.RelativeItem().Text(t => { t.Span("Presentación: ").Bold(); t.Span(dto.Presentacion); });
                });
                inner.Row(row => {
                    row.RelativeItem().Text(t => { t.Span("Latidos Fetales: ").Bold(); t.Span(dto.LatidosCardiacosFetales); });
                    row.RelativeItem().Text(t => { t.Span("Edemas: ").Bold(); t.Span(dto.Edemas); });
                    row.RelativeItem().Text(""); 
                });
                inner.PaddingTop(5).Text(t => { t.Span("Indicaciones: ").Bold(); t.Span(dto.Indicaciones); });
            }));

            // --- Atenciones Recientes (Tabla) ---
            if (dto.Atenciones != null && dto.Atenciones.Any())
            {
                column.Item().PaddingTop(10).Text("RESUMEN DE ATENCIONES").Bold().FontSize(12).FontColor(ColorPrincipal);
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns => {
                        columns.RelativeColumn(1.5f);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(3);
                    });
                    
                    table.Header(header =>
                    {
                        header.Cell().Element(c => CeldaCabecera(c, ColorPrincipal)).Text("Fecha");
                        header.Cell().Element(c => CeldaCabecera(c, ColorPrincipal)).Text("Servicio");
                        header.Cell().Element(c => CeldaCabecera(c, ColorPrincipal)).Text("Doctor");
                        header.Cell().Element(c => CeldaCabecera(c, ColorPrincipal)).Text("Diagnóstico");
                    });
                    
                    foreach (var atencion in dto.Atenciones)
                    {
                        table.Cell().Element(CeldaContenido).Text(atencion.Fecha.ToString("dd/MM/yyyy"));
                        table.Cell().Element(CeldaContenido).Text(atencion.Servicio);
                        table.Cell().Element(CeldaContenido).Text(atencion.Doctor);
                        table.Cell().Element(CeldaContenido).Text(atencion.Diagnostico);
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
                // ✅ REBRANDEANDO: Identidad visual actualizada para Psicomedix
                text.Span("Documento generado por el Sistema - Psicomedix ").FontSize(8).FontColor(Colors.Grey.Medium);
            });
            row.RelativeItem().AlignRight().Text(text => 
            {
                text.Span($"Fecha de impresión: {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(8).FontColor(Colors.Grey.Medium);
            });
        });
    }
}