using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.PDFsDto;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;
using psicomedixMonolito.Services.Interfacespdf;
using psicomedixMonolito.Utils.Authorization;

namespace psicomedixMonolito.Controllers.pdfControladores;

[ApiController]
[Route("api/[controller]")]
[Tags("Documentos PDF - Reportes Financieros")]
public class ReportesFinancierosController : ControllerBase
{
    private readonly IReporteFinancieroPdfService _pdfService;
    private readonly ApplicationDbContext _context; // 🚀 Único punto de contacto con los datos en el monolito

    public ReportesFinancierosController(
        IReporteFinancieroPdfService pdfService, 
        ApplicationDbContext context)
    {
        _pdfService = pdfService;
        _context = context;
    }

    /// <summary>
    /// Genera y descarga el reporte financiero diario en PDF.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite obtener un resumen de los ingresos del día, desglosado por método de pago,
    /// con el detalle de cada movimiento. Útil para el cierre de caja y conciliación contable.
    /// **Permiso requerido:** <see cref="PermisosPolicies.FinanzasVer"/>.
    /// </remarks>
    [HttpGet("diario")]
    [Authorize(Policy = PermisosPolicies.FinanzasVer)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DescargarReporteDiario([FromQuery] DateOnly fecha)
    {
        // 🚀 Optimización de Rendimiento SQL-Side: 
        // Calculamos el rango exacto de tiempo para que la base de datos use índices y no procese filas de más.
        var fechaInicio = fecha.ToDateTime(TimeOnly.MinValue);
        var fechaFin = fechaInicio.AddDays(1);

        var pagosDia = await _context.Set<Pago>()
            .Include(p => p.Paciente)
            .Include(p => p.ServicioClinico)
            .Where(p => p.FechaPago >= fechaInicio && 
                        p.FechaPago < fechaFin && 
                        p.Estado != EstadoPago.Anulado &&
                        p.Estado != EstadoPago.Eliminado) // Filtro de exclusión de estados inválidos
            .AsNoTracking()
            .ToListAsync();

        // Construcción limpia del DTO consolidado para QuestPDF
        var dto = new ReporteDiarioDto
        {
            Fecha = fecha,
            CantidadPagos = pagosDia.Count,
            TotalEfectivo = pagosDia.Where(p => p.MetodoPago == MetodoPago.Efectivo).Sum(p => p.MontoPagado),
            TotalYape = pagosDia.Where(p => p.MetodoPago == MetodoPago.Yape).Sum(p => p.MontoPagado),
            TotalPlin = pagosDia.Where(p => p.MetodoPago == MetodoPago.Plin).Sum(p => p.MontoPagado),
            TotalTransferencia = pagosDia.Where(p => p.MetodoPago == MetodoPago.Transferencia).Sum(p => p.MontoPagado),
            TotalTarjeta = pagosDia.Where(p => p.MetodoPago == MetodoPago.Tarjeta).Sum(p => p.MontoPagado),
            TotalOtro = pagosDia.Where(p => p.MetodoPago == MetodoPago.Otro).Sum(p => p.MontoPagado),
            TotalIngresos = pagosDia.Sum(p => p.MontoPagado),
            Movimientos = pagosDia.Select(p => new MovimientoReporteDto
            {
                CodigoPago = p.CodigoPago,
                Paciente = p.Paciente != null ? $"{p.Paciente.Nombres} {p.Paciente.Apellidos}" : "Paciente no registrado",
                Servicio = p.ServicioClinico?.Nombre ?? "Servicio General",
                Monto = p.MontoPagado,
                MetodoPago = p.MetodoPago.ToString(),
                FechaPago = p.FechaPago
            }).ToList()
        };

        // Renderización binaria limpia mediante el servicio utilitario
        var pdfBytes = _pdfService.GeneratePdf(dto);
        
        return File(pdfBytes, "application/pdf", $"ReporteDiario_{fecha:yyyyMMdd}.pdf");
    }
}