using psicomedixMonolito.DTOs.Comprobantes;
using psicomedixMonolito.Services.Interfacespdf;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace psicomedixMonolito.Services.Documents.Comprobantes.Services;

public class ComprobantePdfService : IComprobantePdfService
{
    public byte[] GenerarBoletaPagoPdf(ComprobantePagoPreviewDto dto)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                ConfigurarPaginaBase(page);

                page.Header().Element(c => ConstruirEncabezado(c, "BOLETA DE PAGO", dto.CodigoComprobante));

                page.Content().PaddingVertical(15).Column(column =>
                {
                    column.Spacing(12);

                    column.Item().Element(c => ConstruirDatosPaciente(
                        c,
                        dto.Paciente,
                        dto.DniPaciente,
                        dto.FechaEmision
                    ));

                    column.Item().Element(c => ConstruirTablaPago(c, dto));

                    column.Item().Element(c => ConstruirResumenMontos(
                        c,
                        dto.Subtotal,
                        dto.MontoImpuesto,
                        dto.Total,
                        dto.TasaImpuesto
                    ));

                    if (!string.IsNullOrWhiteSpace(dto.Observacion))
                    {
                        column.Item().Element(c => ConstruirObservacion(c, dto.Observacion));
                    }
                });

                page.Footer().Element(ConstruirPiePagina);
            });
        }).GeneratePdf();
    }

    public byte[] GenerarConstanciaCitaPdf(ComprobanteCitaPreviewDto dto)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                ConfigurarPaginaBase(page);

                page.Header().Element(c => ConstruirEncabezado(c, "CONSTANCIA DE CITA", dto.CodigoComprobante));

                page.Content().PaddingVertical(15).Column(column =>
                {
                    column.Spacing(12);

                    column.Item().Element(c => ConstruirDatosPaciente(
                        c,
                        dto.Paciente,
                        dto.DniPaciente,
                        dto.FechaEmision
                    ));

                    column.Item().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(info =>
                    {
                        info.Spacing(6);
                        info.Item().Text("Datos de la cita").Bold().FontSize(13);
                        info.Item().Text($"Código de cita: {dto.CodigoCita}");
                        info.Item().Text($"Servicio: {dto.Servicio}");
                        info.Item().Text($"Doctor: {dto.Doctor}");
                        info.Item().Text($"Fecha: {dto.FechaCita:dd/MM/yyyy}");
                        info.Item().Text($"Hora: {dto.HoraInicio} - {dto.HoraFin}");
                        info.Item().Text($"Estado: {dto.EstadoCita}");
                        info.Item().Text($"Motivo: {dto.Motivo}");
                    });

                    if (!string.IsNullOrWhiteSpace(dto.Observacion))
                    {
                        column.Item().Element(c => ConstruirObservacion(c, dto.Observacion));
                    }
                });

                page.Footer().Element(ConstruirPiePagina);
            });
        }).GeneratePdf();
    }

    public byte[] GenerarResumenAtencionPdf(ComprobanteAtencionPreviewDto dto)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                ConfigurarPaginaBase(page);

                page.Header().Element(c => ConstruirEncabezado(c, "RESUMEN DE ATENCIÓN", dto.CodigoComprobante));

                page.Content().PaddingVertical(15).Column(column =>
                {
                    column.Spacing(12);

                    column.Item().Element(c => ConstruirDatosPaciente(
                        c,
                        dto.Paciente,
                        dto.DniPaciente,
                        dto.FechaEmision
                    ));

                    column.Item().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(info =>
                    {
                        info.Spacing(6);
                        info.Item().Text("Datos de la atención").Bold().FontSize(13);
                        info.Item().Text($"Código de atención: {dto.CodigoAtencion}");
                        info.Item().Text($"Servicio: {dto.Servicio}");
                        info.Item().Text($"Doctor: {dto.Doctor}");
                        info.Item().Text($"Fecha de inicio: {dto.FechaInicio:dd/MM/yyyy HH:mm}");
                        info.Item().Text($"Estado: {dto.EstadoAtencion}");
                    });

                    column.Item().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(info =>
                    {
                        info.Spacing(6);
                        info.Item().Text("Resumen clínico").Bold().FontSize(13);
                        info.Item().Text($"Motivo de consulta: {dto.MotivoConsulta}");
                        info.Item().Text($"Diagnóstico: {dto.DiagnosticoResumen}");
                        info.Item().Text($"Indicaciones: {dto.Indicaciones}");
                        info.Item().Text($"Tratamiento: {dto.Tratamiento}");
                        info.Item().Text($"Observaciones: {dto.Observaciones}");
                    });
                });

                page.Footer().Element(ConstruirPiePagina);
            });
        }).GeneratePdf();
    }

    public byte[] GenerarEstadoCuentaPacientePdf(ComprobanteEstadoCuentaPreviewDto dto)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                ConfigurarPaginaBase(page);

                page.Header().Element(c => ConstruirEncabezado(c, "ESTADO DE CUENTA DEL PACIENTE", dto.CodigoComprobante));

                page.Content().PaddingVertical(15).Column(column =>
                {
                    column.Spacing(12);

                    column.Item().Element(c => ConstruirDatosPaciente(
                        c,
                        dto.Paciente,
                        dto.DniPaciente,
                        dto.FechaEmision
                    ));

                    column.Item().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(info =>
                    {
                        info.Spacing(6);
                        info.Item().Text("Resumen financiero").Bold().FontSize(13);
                        info.Item().Text($"Total facturado: S/ {dto.TotalFacturado:N2}");
                        info.Item().Text($"Total pagado: S/ {dto.TotalPagado:N2}");
                        info.Item().Text($"Total pendiente: S/ {dto.TotalPendiente:N2}");
                    });

                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CeldaCabecera).Text("Fecha");
                            header.Cell().Element(CeldaCabecera).Text("Servicio");
                            header.Cell().Element(CeldaCabecera).AlignRight().Text("Total");
                            header.Cell().Element(CeldaCabecera).AlignRight().Text("Pagado");
                            header.Cell().Element(CeldaCabecera).AlignRight().Text("Saldo");
                        });

                        foreach (var item in dto.Detalles)
                        {
                            table.Cell().Element(CeldaContenido).Text(item.FechaPago.ToString("dd/MM/yyyy"));
                            table.Cell().Element(CeldaContenido).Text(item.Servicio);
                            table.Cell().Element(CeldaContenido).AlignRight().Text($"S/ {item.MontoTotal:N2}");
                            table.Cell().Element(CeldaContenido).AlignRight().Text($"S/ {item.MontoPagado:N2}");
                            table.Cell().Element(CeldaContenido).AlignRight().Text($"S/ {item.SaldoPendiente:N2}");
                        }
                    });
                });

                page.Footer().Element(ConstruirPiePagina);
            });
        }).GeneratePdf();
    }

    private static void ConfigurarPaginaBase(PageDescriptor page)
    {
        page.Size(PageSizes.A4);
        page.Margin(35);
        page.DefaultTextStyle(x => x.FontSize(10));
    }

    private static void ConstruirEncabezado(IContainer container, string titulo, string codigo)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text("PSICOMEDIX").Bold().FontSize(16).BackgroundColor(Colors.DeepPurple.Darken3);
                column.Item().Text("Sistema Integral de Gestión Clínica").FontSize(9);
                column.Item().Text("Juliaca - Perú").FontSize(9);
            });

            row.ConstantItem(190).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(8).Column(column =>
            {
                column.Item().AlignCenter().Text(titulo).Bold().FontSize(13);
                column.Item().AlignCenter().Text(codigo).FontSize(10);
            });
        });
    }

    private static void ConstruirDatosPaciente(IContainer container, string paciente, string dni, DateTime fechaEmision)
    {
        container.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(column =>
        {
            column.Spacing(5);
            column.Item().Text("Datos del paciente").Bold().FontSize(13);
            column.Item().Text($"Paciente: {paciente}");
            column.Item().Text($"DNI: {dni}");
            column.Item().Text($"Fecha de emisión: {fechaEmision:dd/MM/yyyy HH:mm}");
        });
    }

    private static void ConstruirTablaPago(IContainer container, ComprobantePagoPreviewDto dto)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(2);
                columns.RelativeColumn(4);
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });

            table.Header(header =>
            {
                header.Cell().Element(CeldaCabecera).Text("Código");
                header.Cell().Element(CeldaCabecera).Text("Descripción");
                header.Cell().Element(CeldaCabecera).AlignRight().Text("Cant.");
                header.Cell().Element(CeldaCabecera).AlignRight().Text("P. Unit.");
                header.Cell().Element(CeldaCabecera).AlignRight().Text("Total");
            });

            foreach (var detalle in dto.Detalles)
            {
                table.Cell().Element(CeldaContenido).Text(detalle.CodigoServicio);
                table.Cell().Element(CeldaContenido).Text(detalle.Descripcion);
                table.Cell().Element(CeldaContenido).AlignRight().Text(detalle.Cantidad.ToString());
                table.Cell().Element(CeldaContenido).AlignRight().Text($"S/ {detalle.PrecioUnitarioFinal:N2}");
                table.Cell().Element(CeldaContenido).AlignRight().Text($"S/ {detalle.Total:N2}");
            }
        });
    }

    private static void ConstruirResumenMontos(
        IContainer container,
        decimal subtotal,
        decimal impuesto,
        decimal total,
        decimal tasaImpuesto)
    {
        container.AlignRight().Width(230).Column(column =>
        {
            column.Spacing(4);
            column.Item().Row(row =>
            {
                row.RelativeItem().Text("Subtotal:");
                row.ConstantItem(90).AlignRight().Text($"S/ {subtotal:N2}");
            });

            column.Item().Row(row =>
            {
                row.RelativeItem().Text($"IGV incluido ({tasaImpuesto:N2}%):");
                row.ConstantItem(90).AlignRight().Text($"S/ {impuesto:N2}");
            });

            column.Item().LineHorizontal(1);

            column.Item().Row(row =>
            {
                row.RelativeItem().Text("Total:");
                row.ConstantItem(90).AlignRight().Text($"S/ {total:N2}").Bold();
            });
        });
    }

    private static void ConstruirObservacion(IContainer container, string? observacion)
    {
        container.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(column =>
        {
            column.Item().Text("Observación").Bold();
            column.Item().Text(observacion ?? "");
        });
    }

    private static void ConstruirPiePagina(IContainer container)
    {
        container.AlignCenter().Text(text =>
        {
            // ✅ CAMBIO DE IDENTIDAD: Ajustado para el nuevo sistema
            text.Span("Documento generado por SYS Psicomedix - ");
            text.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
        });
    }

    private static IContainer CeldaCabecera(IContainer container)
    {
        return container
            .Background(Colors.Grey.Lighten3)
            .Border(1)
            .BorderColor(Colors.Grey.Lighten1)
            .Padding(5);
    }

    private static IContainer CeldaContenido(IContainer container)
    {
        return container
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten3)
            .Padding(5);
    }
}