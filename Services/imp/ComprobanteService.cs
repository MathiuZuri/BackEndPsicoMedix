using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Comprobantes;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;
using psicomedixMonolito.Models.ATENCIONES;
using psicomedixMonolito.Services.Interfacespdf;

namespace psicomedixMonolito.Services.imp;

public class ComprobanteService : IComprobanteService
{
    private decimal TasaIgvActiva => (decimal)TasaImpuesto.IGV_18;
    private readonly ApplicationDbContext _context;
    private readonly IComprobantePdfService _pdfService; // 🚀 Inyección del generador de PDF nativo

    public ComprobanteService(ApplicationDbContext context, IComprobantePdfService pdfService)
    {
        _context = context;
        _pdfService = pdfService;
    }
    
    public async Task<ArchivoDescargaDto> GenerarPdfBoletaPagoAsync(Guid comprobanteId)
    {
        var comprobante = await _context.Comprobantes.FindAsync(comprobanteId)
            ?? throw new KeyNotFoundException("Comprobante de pago no encontrado.");

        // Reutilizamos la lógica del preview con los IDs reales guardados en el registro
        var previewDto = await PreviewBoletaPagoAsync(comprobante.PagoId ?? Guid.Empty);
        previewDto.CodigoComprobante = comprobante.CodigoComprobante; // Reemplaza "PREVIEW" por el código real (Ej: B001-000005)
        previewDto.FechaEmision = comprobante.FechaEmision;

        var pdfBytes = _pdfService.GenerarBoletaPagoPdf(previewDto);

        return new ArchivoDescargaDto
        {
            Archivo = pdfBytes,
            ContentType = "application/pdf",
            NombreArchivo = $"Boleta_{comprobante.CodigoComprobante}.pdf"
        };
    }

    public async Task<ArchivoDescargaDto> GenerarPdfConstanciaCitaAsync(Guid comprobanteId)
    {
        var comprobante = await _context.Comprobantes.FindAsync(comprobanteId)
            ?? throw new KeyNotFoundException("Comprobante de constancia de cita no encontrado.");

        var previewDto = await PreviewConstanciaCitaAsync(comprobante.CitaId ?? Guid.Empty);
        previewDto.CodigoComprobante = comprobante.CodigoComprobante;
        previewDto.FechaEmision = comprobante.FechaEmision;

        var pdfBytes = _pdfService.GenerarConstanciaCitaPdf(previewDto);

        return new ArchivoDescargaDto
        {
            Archivo = pdfBytes,
            ContentType = "application/pdf",
            NombreArchivo = $"Constancia_Cita_{comprobante.CodigoComprobante}.pdf"
        };
    }

    public async Task<ArchivoDescargaDto> GenerarPdfResumenAtencionAsync(Guid comprobanteId)
    {
        var comprobante = await _context.Comprobantes.FindAsync(comprobanteId)
            ?? throw new KeyNotFoundException("Comprobante de resumen de atención no encontrado.");

        var previewDto = await PreviewResumenAtencionAsync(comprobante.AtencionId ?? Guid.Empty);
        previewDto.CodigoComprobante = comprobante.CodigoComprobante;
        previewDto.FechaEmision = comprobante.FechaEmision;

        var pdfBytes = _pdfService.GenerarResumenAtencionPdf(previewDto);

        return new ArchivoDescargaDto
        {
            Archivo = pdfBytes,
            ContentType = "application/pdf",
            NombreArchivo = $"Resumen_Atencion_{comprobante.CodigoComprobante}.pdf"
        };
    }

    public async Task<ArchivoDescargaDto> GenerarPdfEstadoCuentaPacienteAsync(Guid comprobanteId)
    {
        var comprobante = await _context.Comprobantes.FindAsync(comprobanteId)
            ?? throw new KeyNotFoundException("Comprobante de estado de cuenta no encontrado.");

        var previewDto = await PreviewEstadoCuentaPacienteAsync(comprobante.PacienteId);
        previewDto.CodigoComprobante = comprobante.CodigoComprobante;
        previewDto.FechaEmision = comprobante.FechaEmision;

        var pdfBytes = _pdfService.GenerarEstadoCuentaPacientePdf(previewDto);

        return new ArchivoDescargaDto
        {
            Archivo = pdfBytes,
            ContentType = "application/pdf",
            NombreArchivo = $"Estado_Cuenta_{comprobante.CodigoComprobante}.pdf"
        };
    }

    // ==========================================================
    // 1. BOLETA DE PAGO (Preview, Emisión)
    // ==========================================================
    public async Task<ComprobantePagoPreviewDto> PreviewBoletaPagoAsync(Guid pagoId, decimal? tasaImpuesto = null)
    {
        if (pagoId == Guid.Empty)
            throw new InvalidOperationException("El identificador del pago es obligatorio.");

        var tasaFinal = tasaImpuesto ?? TasaIgvActiva;
        var pago = await ObtenerPagoConDetallePorIdAsync(pagoId);

        var subtotal = CalcularSubtotalDesdeTotal(pago.MontoPagado, tasaFinal);
        var impuesto = pago.MontoPagado - subtotal;

        return new ComprobantePagoPreviewDto
        {
            CodigoComprobante = "PREVIEW",
            PagoId = pago.Id,
            CodigoPago = pago.CodigoPago,
            PacienteId = pago.PacienteId,
            Paciente = pago.Paciente == null ? "" : $"{pago.Paciente.Nombres} {pago.Paciente.Apellidos}",
            DniPaciente = pago.Paciente?.DNI ?? "",
            AtencionId = pago.AtencionId,
            CodigoAtencion = pago.Atencion?.CodigoAtencion,
            CitaId = pago.CitaId,
            CodigoCita = pago.Cita?.CodigoCita,
            Servicio = pago.ServicioClinico?.Nombre ?? "Servicio clínico",
            MontoPagado = pago.MontoPagado,
            Subtotal = subtotal,
            TasaImpuesto = tasaFinal,
            MontoImpuesto = impuesto,
            Total = pago.MontoPagado,
            MetodoPago = pago.MetodoPago.ToString(),
            EstadoPago = pago.Estado.ToString(),
            FechaPago = pago.FechaPago,
            FechaEmision = DateTime.UtcNow,
            Observacion = pago.Observacion,
            Detalles = new List<ComprobanteDetalleDto>
            {
                new()
                {
                    CodigoServicio = pago.ServicioClinico?.CodigoServicio ?? "",
                    Descripcion = pago.ServicioClinico?.Nombre ?? "Servicio clínico",
                    Cantidad = 1,
                    PrecioUnitarioFinal = pago.MontoPagado,
                    Subtotal = subtotal,
                    TasaImpuesto = tasaFinal,
                    MontoImpuesto = impuesto,
                    Total = pago.MontoPagado
                }
            }
        };
    }

    public async Task<Guid> EmitirBoletaPagoAsync(EmitirComprobantePagoDto dto, Guid usuarioId)
    {
        if (dto.PagoId == Guid.Empty && string.IsNullOrWhiteSpace(dto.CodigoPago))
            throw new InvalidOperationException("Debe enviar el identificador del pago o el código de pago.");

        Pago pago;
        if (!string.IsNullOrWhiteSpace(dto.CodigoPago))
        {
            pago = await _context.Set<Pago>()
                       .Include(x => x.Paciente)
                       .Include(x => x.Atencion)
                       .Include(x => x.ServicioClinico)
                       .FirstOrDefaultAsync(x => x.CodigoPago == dto.CodigoPago.Trim())
                   ?? throw new KeyNotFoundException("Pago no encontrado.");
        }
        else
        {
            pago = await ObtenerPagoConDetallePorIdAsync(dto.PagoId);
        }

        var serie = ObtenerSerie(TipoComprobante.BoletaPago);
        var ultimoNumero = await _context.Comprobantes
            .Where(x => x.Serie == serie)
            .MaxAsync(x => (int?)x.Numero) ?? 0;
        var numero = ultimoNumero + 1;

        var subtotal = CalcularSubtotalDesdeTotal(pago.MontoPagado, TasaIgvActiva);
        var impuesto = pago.MontoPagado - subtotal;

        var comprobante = new Comprobante
        {
            Id = Guid.NewGuid(),
            CodigoComprobante = $"{serie}-{numero:000000}",
            Serie = serie,
            Numero = numero,
            TipoComprobante = TipoComprobante.BoletaPago,
            Estado = EstadoComprobante.Emitido,
            FormatoImpresion = TipoFormatoImpresion.A4,
            PacienteId = pago.PacienteId,
            PagoId = pago.Id,
            CitaId = pago.CitaId,
            AtencionId = pago.AtencionId,
            HistorialClinicoId = pago.Atencion?.HistorialClinicoId,
            TipoDocumentoPaciente = TipoDocumentoComprobante.DNI,
            NumeroDocumentoPaciente = pago.Paciente?.DNI ?? "",
            NombrePaciente = pago.Paciente == null ? "" : $"{pago.Paciente.Nombres} {pago.Paciente.Apellidos}",
            DireccionPaciente = pago.Paciente?.Direccion,
            Subtotal = subtotal,
            TasaImpuesto = TasaIgvActiva,
            MontoImpuesto = impuesto,
            Total = pago.MontoPagado,
            FechaEmision = DateTime.UtcNow,
            UsuarioEmisionId = usuarioId,
            Observacion = dto.Observacion?.Trim(),
            DatosSnapshotJson = JsonSerializer.Serialize(new
            {
                Tipo = "Boleta de pago",
                PagoId = pago.Id,
                CodigoPago = pago.CodigoPago,
                PacienteId = pago.PacienteId,
                Paciente = pago.Paciente == null ? "" : $"{pago.Paciente.Nombres} {pago.Paciente.Apellidos}",
                DniPaciente = pago.Paciente?.DNI ?? "",
                Servicio = pago.ServicioClinico?.Nombre ?? "Servicio clínico",
                MontoTotal = pago.MontoTotal,
                MontoPagado = pago.MontoPagado,
                SaldoPendiente = pago.SaldoPendiente,
                MetodoPago = pago.MetodoPago.ToString(),
                EstadoPago = pago.Estado.ToString(),
                FechaPago = pago.FechaPago,
                TasaImpuesto = TasaIgvActiva,
                Subtotal = subtotal,
                MontoImpuesto = impuesto,
                Total = pago.MontoPagado
            })
        };

        comprobante.Detalles.Add(new ComprobanteDetalle
        {
            Id = Guid.NewGuid(),
            ComprobanteId = comprobante.Id,
            CodigoServicio = pago.ServicioClinico?.CodigoServicio ?? "",
            Descripcion = pago.ServicioClinico?.Nombre ?? "Servicio clínico",
            Cantidad = 1,
            PrecioUnitarioFinal = pago.MontoPagado,
            Subtotal = subtotal,
            TasaImpuesto = TasaIgvActiva,
            MontoImpuesto = impuesto,
            Total = pago.MontoPagado
        });

        await _context.Comprobantes.AddAsync(comprobante);
        await _context.SaveChangesAsync();
        return comprobante.Id;
    }

    // ==========================================================
    // 2. CONSTANCIA DE CITA (Preview, Emisión)
    // ==========================================================
    public async Task<ComprobanteCitaPreviewDto> PreviewConstanciaCitaAsync(Guid citaId)
    {
        if (citaId == Guid.Empty)
            throw new InvalidOperationException("El identificador de la cita es obligatorio.");

        var cita = await _context.Set<Cita>()
                       .Include(x => x.Paciente)
                       .Include(x => x.Doctor)
                       .Include(x => x.ServicioClinico)
                       .FirstOrDefaultAsync(x => x.Id == citaId)
                   ?? throw new KeyNotFoundException("Cita no encontrada.");

        return new ComprobanteCitaPreviewDto
        {
            ComprobanteId = Guid.Empty,
            CodigoComprobante = "PREVIEW",
            CitaId = cita.Id,
            CodigoCita = cita.CodigoCita,
            PacienteId = cita.PacienteId,
            Paciente = $"{cita.Paciente.Nombres} {cita.Paciente.Apellidos}",
            DniPaciente = cita.Paciente.DNI,
            DireccionPaciente = cita.Paciente.Direccion,
            DoctorId = cita.DoctorId,
            Doctor = $"{cita.Doctor.Nombres} {cita.Doctor.Apellidos}",
            Especialidad = cita.Doctor.Especialidad,
            ServicioClinicoId = cita.ServicioClinicoId,
            Servicio = cita.ServicioClinico.Nombre,
            FechaCita = cita.Fecha,
            HoraInicio = cita.HoraInicio,
            HoraFin = cita.HoraFin,
            EstadoCita = cita.Estado.ToString(),
            Motivo = cita.Motivo,
            FechaEmision = DateTime.UtcNow,
            Observacion = "Vista previa"
        };
    }

    public async Task<Guid> EmitirConstanciaCitaAsync(EmitirComprobanteCitaDto dto, Guid usuarioId)
    {
        if (dto.CitaId == Guid.Empty)
            throw new InvalidOperationException("El identificador de la cita es obligatorio.");

        var cita = await _context.Set<Cita>()
                       .Include(x => x.Paciente)
                            .ThenInclude(p => p.HistorialClinico)
                       .FirstOrDefaultAsync(x => x.Id == dto.CitaId)
                   ?? throw new KeyNotFoundException("Cita no encontrada.");

        var serie = ObtenerSerie(TipoComprobante.ConstanciaCita);
        var ultimoNumero = await _context.Comprobantes
            .Where(x => x.Serie == serie)
            .MaxAsync(x => (int?)x.Numero) ?? 0;
        var numero = ultimoNumero + 1;

        var comprobante = new Comprobante
        {
            Id = Guid.NewGuid(),
            CodigoComprobante = $"{serie}-{numero:000000}",
            Serie = serie,
            Numero = numero,
            TipoComprobante = TipoComprobante.ConstanciaCita,
            Estado = EstadoComprobante.Emitido,
            FormatoImpresion = dto.FormatoImpresion,
            PacienteId = cita.PacienteId,
            CitaId = cita.Id,
            HistorialClinicoId = cita.Paciente.HistorialClinico?.Id,
            TipoDocumentoPaciente = TipoDocumentoComprobante.DNI,
            NumeroDocumentoPaciente = cita.Paciente.DNI,
            NombrePaciente = $"{cita.Paciente.Nombres} {cita.Paciente.Apellidos}",
            DireccionPaciente = cita.Paciente.Direccion,
            Subtotal = 0,
            TasaImpuesto = 0,
            MontoImpuesto = 0,
            Total = 0,
            FechaEmision = DateTime.UtcNow,
            UsuarioEmisionId = usuarioId,
            Observacion = dto.Observacion?.Trim(),
            DatosSnapshotJson = JsonSerializer.Serialize(new
            {
                Tipo = "Constancia de cita",
                CitaId = cita.Id,
                CodigoCita = cita.CodigoCita,
                PacienteId = cita.PacienteId,
                Paciente = $"{cita.Paciente.Nombres} {cita.Paciente.Apellidos}",
                DniPaciente = cita.Paciente.DNI,
                DoctorId = cita.DoctorId,
                Doctor = cita.Doctor == null ? "" : $"{cita.Doctor.Nombres} {cita.Doctor.Apellidos}",
                Servicio = cita.ServicioClinico?.Nombre ?? "",
                Fecha = cita.Fecha,
                HoraInicio = cita.HoraInicio,
                HoraFin = cita.HoraFin,
                Motivo = cita.Motivo,
                EstadoCita = cita.Estado.ToString()
            })
        };

        await _context.Comprobantes.AddAsync(comprobante);
        await _context.SaveChangesAsync();
        return comprobante.Id;
    }

    // ==========================================================
    // 3. RESUMEN DE ATENCIÓN (Preview, Emisión)
    // ==========================================================
    public async Task<ComprobanteAtencionPreviewDto> PreviewResumenAtencionAsync(Guid atencionId)
    {
        if (atencionId == Guid.Empty)
            throw new InvalidOperationException("El identificador de la atención es obligatorio.");

        var atencion = await ObtenerAtencionDetalleCompletoAsync(atencionId);
        return MapearAtencionPreview(atencion, "PREVIEW");
    }

    public async Task<Guid> EmitirResumenAtencionAsync(EmitirComprobanteAtencionDto dto, Guid usuarioId)
    {
        if (dto.AtencionId == Guid.Empty)
            throw new InvalidOperationException("El identificador de la atención es obligatorio.");

        var atencion = await ObtenerAtencionDetalleCompletoAsync(dto.AtencionId);

        var costoFinal = atencion.Pagos?.Sum(p => p.MontoTotal) ?? 0;
        var montoPagado = atencion.Pagos?.Sum(p => p.MontoPagado) ?? 0;
        var saldoPendiente = atencion.Pagos?.Sum(p => p.SaldoPendiente) ?? 0;
        var subtotal = CalcularSubtotalDesdeTotal(costoFinal, TasaIgvActiva);
        var impuesto = costoFinal - subtotal;

        var serie = ObtenerSerie(TipoComprobante.ResumenAtencion);
        var ultimoNumero = await _context.Comprobantes
            .Where(x => x.Serie == serie)
            .MaxAsync(x => (int?)x.Numero) ?? 0;
        var numero = ultimoNumero + 1;

        var comprobante = new Comprobante
        {
            Id = Guid.NewGuid(),
            CodigoComprobante = $"{serie}-{numero:000000}",
            Serie = serie,
            Numero = numero,
            TipoComprobante = TipoComprobante.ResumenAtencion,
            Estado = EstadoComprobante.Emitido,
            FormatoImpresion = dto.FormatoImpresion,
            PacienteId = atencion.PacienteId,
            AtencionId = atencion.Id,
            HistorialClinicoId = atencion.HistorialClinicoId,
            TipoDocumentoPaciente = TipoDocumentoComprobante.DNI,
            NumeroDocumentoPaciente = atencion.Paciente.DNI,
            NombrePaciente = $"{atencion.Paciente.Nombres} {atencion.Paciente.Apellidos}",
            DireccionPaciente = atencion.Paciente.Direccion,
            Subtotal = subtotal,
            TasaImpuesto = TasaIgvActiva,
            MontoImpuesto = impuesto,
            Total = costoFinal,
            FechaEmision = DateTime.UtcNow,
            UsuarioEmisionId = usuarioId,
            Observacion = dto.Observacion?.Trim(),
            DatosSnapshotJson = JsonSerializer.Serialize(new
            {
                Tipo = "Resumen de atención",
                AtencionId = atencion.Id,
                CodigoAtencion = atencion.CodigoAtencion,
                PacienteId = atencion.PacienteId,
                Paciente = $"{atencion.Paciente.Nombres} {atencion.Paciente.Apellidos}",
                Doctor = atencion.Doctor == null ? "" : $"{atencion.Doctor.Nombres} {atencion.Doctor.Apellidos}",
                Servicio = atencion.ServicioClinico?.Nombre ?? "",
                MotivoConsulta = atencion.Anamnesis?.MotivoConsulta,
                DiagnosticoPrincipal = atencion.ImpresionDiagnostica?.DiagnosticoPrincipal,
                FechaInicio = atencion.FechaInicio,
                FechaCierre = atencion.FechaCierre,
                CostoFinal = costoFinal,
                MontoPagado = montoPagado,
                SaldoPendiente = saldoPendiente
            })
        };

        comprobante.Detalles.Add(new ComprobanteDetalle
        {
            Id = Guid.NewGuid(),
            ComprobanteId = comprobante.Id,
            CodigoServicio = atencion.ServicioClinico?.CodigoServicio ?? "",
            Descripcion = atencion.ServicioClinico?.Nombre ?? "Servicio clínico",
            Cantidad = 1,
            PrecioUnitarioFinal = costoFinal,
            Subtotal = subtotal,
            TasaImpuesto = TasaIgvActiva,
            MontoImpuesto = impuesto,
            Total = costoFinal
        });

        await _context.Comprobantes.AddAsync(comprobante);
        await _context.SaveChangesAsync();
        return comprobante.Id;
    }

    // ==========================================================
    // 4. ESTADO DE CUENTA DEL PACIENTE (Preview, Emisión)
    // ==========================================================
    public async Task<ComprobanteEstadoCuentaPreviewDto> PreviewEstadoCuentaPacienteAsync(Guid pacienteId)
    {
        if (pacienteId == Guid.Empty)
            throw new InvalidOperationException("El identificador del paciente es obligatorio.");

        var paciente = await _context.Set<Paciente>().FindAsync(pacienteId)
            ?? throw new KeyNotFoundException("Paciente no encontrado.");

        var pagos = await _context.Set<Pago>()
            .Include(x => x.ServicioClinico)
            .Where(x => x.PacienteId == pacienteId)
            .ToListAsync();

        var pagosValidos = pagos
            .Where(x => x.Estado != EstadoPago.Anulado && x.Estado != EstadoPago.Eliminado)
            .OrderByDescending(x => x.FechaPago)
            .ToList();

        var totalFacturado = pagosValidos.Sum(x => x.MontoTotal);
        var totalPagado = pagosValidos.Sum(x => x.MontoPagado);
        var totalPendiente = Math.Max(totalFacturado - totalPagado, 0);

        return new ComprobanteEstadoCuentaPreviewDto
        {
            ComprobanteId = Guid.Empty,
            CodigoComprobante = "PREVIEW",
            PacienteId = paciente.Id,
            Paciente = $"{paciente.Nombres} {paciente.Apellidos}",
            DniPaciente = paciente.DNI,
            DireccionPaciente = paciente.Direccion,
            TotalFacturado = totalFacturado,
            TotalPagado = totalPagado,
            TotalPendiente = totalPendiente,
            FechaEmision = DateTime.UtcNow,
            Detalles = pagosValidos.Select(x => new DetalleEstadoCuentaComprobanteDto
            {
                PagoId = x.Id,
                CodigoPago = x.CodigoPago,
                Servicio = x.ServicioClinico?.Nombre ?? "",
                FechaPago = x.FechaPago,
                MontoTotal = x.MontoTotal,
                MontoPagado = x.MontoPagado,
                SaldoPendiente = x.SaldoPendiente,
                EstadoPago = x.Estado.ToString()
            }).ToList()
        };
    }

    public async Task<Guid> EmitirEstadoCuentaPacienteAsync(EmitirComprobanteEstadoCuentaDto dto, Guid usuarioId)
    {
        if (dto.PacienteId == Guid.Empty)
            throw new InvalidOperationException("El identificador del paciente es obligatorio.");

        var paciente = await _context.Set<Paciente>()
            .Include(p => p.HistorialClinico)
            .FirstOrDefaultAsync(x => x.Id == dto.PacienteId)
            ?? throw new KeyNotFoundException("Paciente no encontrado.");

        var pagos = await _context.Set<Pago>()
            .Include(x => x.ServicioClinico)
            .Where(x => x.PacienteId == dto.PacienteId)
            .ToListAsync();

        var pagosValidos = pagos
            .Where(x => x.Estado != EstadoPago.Anulado && x.Estado != EstadoPago.Eliminado)
            .OrderByDescending(x => x.FechaPago)
            .ToList();

        var totalFacturado = pagosValidos.Sum(x => x.MontoTotal);
        var totalPagado = pagosValidos.Sum(x => x.MontoPagado);
        var totalPendiente = Math.Max(totalFacturado - totalPagado, 0);

        var serie = ObtenerSerie(TipoComprobante.EstadoCuenta);
        var ultimoNumero = await _context.Comprobantes
            .Where(x => x.Serie == serie)
            .MaxAsync(x => (int?)x.Numero) ?? 0;
        var numero = ultimoNumero + 1;

        var comprobante = new Comprobante
        {
            Id = Guid.NewGuid(),
            CodigoComprobante = $"{serie}-{numero:000000}",
            Serie = serie,
            Numero = numero,
            TipoComprobante = TipoComprobante.EstadoCuenta,
            Estado = EstadoComprobante.Emitido,
            FormatoImpresion = dto.FormatoImpresion,
            PacienteId = paciente.Id,
            HistorialClinicoId = paciente.HistorialClinico?.Id,
            TipoDocumentoPaciente = TipoDocumentoComprobante.DNI,
            NumeroDocumentoPaciente = paciente.DNI,
            NombrePaciente = $"{paciente.Nombres} {paciente.Apellidos}",
            DireccionPaciente = paciente.Direccion,
            Subtotal = totalFacturado,
            TasaImpuesto = 0,
            MontoImpuesto = 0,
            Total = totalFacturado,
            FechaEmision = DateTime.UtcNow,
            UsuarioEmisionId = usuarioId,
            Observacion = dto.Observacion?.Trim(),
            DatosSnapshotJson = JsonSerializer.Serialize(new
            {
                Tipo = "Estado de cuenta",
                PacienteId = paciente.Id,
                Paciente = $"{paciente.Nombres} {paciente.Apellidos}",
                DniPaciente = paciente.DNI,
                TotalFacturado = totalFacturado,
                TotalPagado = totalPagado,
                TotalPendiente = totalPendiente,
                FechaEmision = DateTime.UtcNow
            })
        };

        foreach (var pago in pagosValidos)
        {
            comprobante.Detalles.Add(new ComprobanteDetalle
            {
                Id = Guid.NewGuid(),
                ComprobanteId = comprobante.Id,
                CodigoServicio = pago.ServicioClinico?.CodigoServicio ?? "",
                Descripcion = $"{pago.ServicioClinico?.Nombre ?? "Servicio"} - {pago.CodigoPago}",
                Cantidad = 1,
                PrecioUnitarioFinal = pago.MontoTotal,
                Subtotal = pago.MontoTotal,
                TasaImpuesto = 0,
                MontoImpuesto = 0,
                Total = pago.MontoTotal
            });
        }

        await _context.Comprobantes.AddAsync(comprobante);
        await _context.SaveChangesAsync();
        return comprobante.Id;
    }

    // ==========================================================
    // 5. CONSULTAS Y ANULACIÓN
    // ==========================================================
    public async Task<ComprobanteDto> ObtenerPorIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new InvalidOperationException("El identificador del comprobante es obligatorio.");

        var comprobante = await ObtenerComprobanteCompletoPorIdAsync(id);
        return MapearComprobante(comprobante);
    }

    public async Task<IEnumerable<ComprobanteDto>> ObtenerPorPacienteAsync(Guid pacienteId)
    {
        if (pacienteId == Guid.Empty)
            throw new InvalidOperationException("El identificador del paciente es obligatorio.");

        var comprobantes = await _context.Comprobantes
            .AsNoTracking()
            .Include(x => x.Paciente)
            .Include(x => x.UsuarioEmision)
            .Include(x => x.UsuarioAnulacion)
            .Include(x => x.Detalles)
            .Where(x => x.PacienteId == pacienteId)
            .OrderByDescending(x => x.FechaEmision)
            .ToListAsync();

        return comprobantes.Select(MapearComprobante).ToList();
    }

    public async Task<IEnumerable<ComprobanteDto>> ObtenerPorPagoAsync(Guid pagoId)
    {
        if (pagoId == Guid.Empty)
            throw new InvalidOperationException("El identificador del pago es obligatorio.");

        var comprobantes = await _context.Comprobantes
            .AsNoTracking()
            .Include(x => x.Paciente)
            .Include(x => x.Pago)
            .Include(x => x.UsuarioEmision)
            .Include(x => x.UsuarioAnulacion)
            .Include(x => x.Detalles)
            .Where(x => x.PagoId == pagoId)
            .OrderByDescending(x => x.FechaEmision)
            .ToListAsync();

        return comprobantes.Select(MapearComprobante).ToList();
    }

    public async Task<IEnumerable<ComprobanteDto>> ObtenerPorAtencionAsync(Guid atencionId)
    {
        if (atencionId == Guid.Empty)
            throw new InvalidOperationException("El identificador de la atención es obligatorio.");

        var comprobantes = await _context.Comprobantes
            .AsNoTracking()
            .Include(x => x.Paciente)
            .Include(x => x.Atencion)
            .Include(x => x.UsuarioEmision)
            .Include(x => x.UsuarioAnulacion)
            .Include(x => x.Detalles)
            .Where(x => x.AtencionId == atencionId)
            .OrderByDescending(x => x.FechaEmision)
            .ToListAsync();

        return comprobantes.Select(MapearComprobante).ToList();
    }

    public async Task<IEnumerable<ComprobanteDto>> ObtenerTodosAsync()
    {
        var comprobantes = await _context.Comprobantes
            .AsNoTracking()
            .Include(x => x.Paciente)
                .ThenInclude(p => p.HistorialClinico)
            .Include(x => x.Pago)
            .Include(x => x.Cita)
            .Include(x => x.Atencion)
            .Include(x => x.UsuarioEmision)
            .Include(x => x.UsuarioAnulacion)
            .Include(x => x.Detalles)
            .OrderByDescending(x => x.FechaEmision)
            .ToListAsync();

        return comprobantes.Select(MapearComprobante).ToList();
    }

    public async Task AnularComprobanteAsync(Guid comprobanteId, string motivo, Guid usuarioId)
    {
        if (comprobanteId == Guid.Empty)
            throw new InvalidOperationException("El identificador del comprobante es obligatorio.");

        if (string.IsNullOrWhiteSpace(motivo))
            throw new InvalidOperationException("El motivo de anulación es obligatorio.");

        var comprobante = await _context.Comprobantes.FindAsync(comprobanteId)
            ?? throw new KeyNotFoundException("Comprobante no encontrado.");

        if (comprobante.Estado == EstadoComprobante.Anulado)
            throw new InvalidOperationException("El comprobante ya se encuentra anulado.");

        comprobante.Estado = EstadoComprobante.Anulado;
        comprobante.FechaAnulacion = DateTime.UtcNow;
        comprobante.UsuarioAnulacionId = usuarioId;
        comprobante.MotivoAnulacion = motivo.Trim();

        await _context.SaveChangesAsync();
    }

    // ==========================================================
    // 6. MÉTODOS PRIVADOS / CONSULTAS INTERNAS
    // ==========================================================
    private async Task<Pago> ObtenerPagoConDetallePorIdAsync(Guid pagoId)
    {
        return await _context.Set<Pago>()
            .Include(x => x.Paciente)
            .Include(x => x.Atencion)
            .Include(x => x.Cita)
            .Include(x => x.ServicioClinico)
            .FirstOrDefaultAsync(x => x.Id == pagoId)
            ?? throw new KeyNotFoundException("Pago no encontrado.");
    }

    private async Task<Comprobante> ObtenerComprobanteCompletoPorIdAsync(Guid id)
    {
        return await _context.Comprobantes
            .Include(x => x.Paciente)
                .ThenInclude(p => p.HistorialClinico)
            .Include(x => x.Pago)
            .Include(x => x.Cita)
                .ThenInclude(c => c!.Doctor)
            .Include(x => x.Cita)
                .ThenInclude(c => c!.ServicioClinico)
            .Include(x => x.Atencion)
                .ThenInclude(a => a!.Doctor)
            .Include(x => x.Atencion)
                .ThenInclude(a => a!.ServicioClinico)
            .Include(x => x.UsuarioEmision)
            .Include(x => x.UsuarioAnulacion)
            .Include(x => x.Detalles)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new KeyNotFoundException("Comprobante no encontrado.");
    }

    private async Task<Atencion> ObtenerAtencionDetalleCompletoAsync(Guid atencionId)
    {
        return await _context.Set<Atencion>()
            .Include(x => x.Paciente)
            .Include(x => x.Doctor)
                .ThenInclude(d => d.Usuario)
            .Include(x => x.ServicioClinico)
            .Include(x => x.Cita)
            .Include(x => x.Pagos)
            .Include(x => x.Anamnesis)
            .Include(x => x.ImpresionDiagnostica)
            .FirstOrDefaultAsync(x => x.Id == atencionId)
            ?? throw new KeyNotFoundException("Atención no encontrada.");
    }

    private static string ObtenerSerie(TipoComprobante tipo)
    {
        return tipo switch
        {
            TipoComprobante.BoletaPago => "B001",
            TipoComprobante.ConstanciaCita => "C001",
            TipoComprobante.ResumenAtencion => "A001",
            TipoComprobante.EstadoCuenta => "E001",
            TipoComprobante.HistoriaClinica => "H001",
            _ => "D001"
        };
    }

    private static decimal CalcularSubtotalDesdeTotal(decimal total, decimal tasaImpuesto)
    {
        return Math.Round(total / (1 + tasaImpuesto / 100), 2);
    }

    private static ComprobanteAtencionPreviewDto MapearAtencionPreview(Atencion atencion, string codigoComprobante)
    {
        var costoFinal = atencion.Pagos?.Sum(p => p.MontoTotal) ?? 0;
        var montoPagado = atencion.Pagos?.Sum(p => p.MontoPagado) ?? 0;
        var saldoPendiente = atencion.Pagos?.Sum(p => p.SaldoPendiente) ?? 0;

        return new ComprobanteAtencionPreviewDto
        {
            ComprobanteId = Guid.Empty,
            CodigoComprobante = codigoComprobante,
            AtencionId = atencion.Id,
            CodigoAtencion = atencion.CodigoAtencion ?? "",
            PacienteId = atencion.PacienteId,
            Paciente = $"{atencion.Paciente.Nombres} {atencion.Paciente.Apellidos}",
            DniPaciente = atencion.Paciente.DNI,
            DireccionPaciente = atencion.Paciente.Direccion,
            DoctorId = atencion.DoctorId,
            Doctor = atencion.Doctor == null ? "" : $"{atencion.Doctor.Nombres} {atencion.Doctor.Apellidos}",
            Especialidad = atencion.Doctor?.Especialidad ?? "",
            ServicioClinicoId = atencion.ServicioClinicoId,
            Servicio = atencion.ServicioClinico?.Nombre ?? "Servicio clínico",
            FechaInicio = atencion.FechaInicio,
            FechaCierre = atencion.FechaCierre,
            MotivoConsulta = atencion.Anamnesis?.MotivoConsulta ?? "",
            DiagnosticoResumen = atencion.ImpresionDiagnostica?.DiagnosticoPrincipal,
            Indicaciones = atencion.ImpresionDiagnostica?.IndicacionesReceta,
            Tratamiento = atencion.ImpresionDiagnostica?.DiagnosticosSecundarios,
            Observaciones = atencion.ImpresionDiagnostica?.DiagnosticosSecundarios,
            EstadoAtencion = atencion.Estado.ToString(),
            CostoFinal = costoFinal,
            MontoPagado = montoPagado,
            SaldoPendiente = saldoPendiente,
            FechaEmision = DateTime.UtcNow
        };
    }

    private static ComprobanteDto MapearComprobante(Comprobante x)
    {
        return new ComprobanteDto
        {
            Id = x.Id,
            CodigoComprobante = x.CodigoComprobante,
            Serie = x.Serie,
            Numero = x.Numero,
            TipoComprobante = x.TipoComprobante.ToString(),
            Estado = x.Estado.ToString(),
            FormatoImpresion = x.FormatoImpresion.ToString(),
            PacienteId = x.PacienteId,
            Paciente = x.NombrePaciente,
            TipoDocumentoPaciente = x.TipoDocumentoPaciente.ToString(),
            NumeroDocumentoPaciente = x.NumeroDocumentoPaciente,
            DireccionPaciente = x.DireccionPaciente,
            PagoId = x.PagoId,
            CitaId = x.CitaId,
            AtencionId = x.AtencionId,
            HistorialClinicoId = x.HistorialClinicoId,
            Subtotal = x.Subtotal,
            TasaImpuesto = x.TasaImpuesto,
            MontoImpuesto = x.MontoImpuesto,
            Total = x.Total,
            FechaEmision = x.FechaEmision,
            UsuarioEmisionId = x.UsuarioEmisionId,
            UsuarioEmision = x.UsuarioEmision == null ? null : $"{x.UsuarioEmision.Nombres} {x.UsuarioEmision.Apellidos}",
            FechaAnulacion = x.FechaAnulacion,
            UsuarioAnulacionId = x.UsuarioAnulacionId,
            UsuarioAnulacion = x.UsuarioAnulacion == null ? null : $"{x.UsuarioAnulacion.Nombres} {x.UsuarioAnulacion.Apellidos}",
            Observacion = x.Observacion,
            MotivoAnulacion = x.MotivoAnulacion,
            Detalles = x.Detalles.Select(d => new ComprobanteDetalleDto
            {
                Id = d.Id,
                CodigoServicio = d.CodigoServicio,
                Descripcion = d.Descripcion,
                Cantidad = d.Cantidad,
                PrecioUnitarioFinal = d.PrecioUnitarioFinal,
                Subtotal = d.Subtotal,
                TasaImpuesto = d.TasaImpuesto,
                MontoImpuesto = d.MontoImpuesto,
                Total = d.Total
            }).ToList()
        };
    }
}