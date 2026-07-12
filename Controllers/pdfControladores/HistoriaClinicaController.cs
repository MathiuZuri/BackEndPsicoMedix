using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.PDFsDto;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;
using psicomedixMonolito.Models.ATENCIONES;
using psicomedixMonolito.Services.Interfacespdf;
using psicomedixMonolito.Utils.Authorization;

namespace psicomedixMonolito.Controllers.pdfControladores;

[ApiController]
[Route("api/[controller]")]
[Tags("Documentos PDF - Historia Clínica")]
public class HistoriaClinicaController : ControllerBase
{
    private readonly IHistoriaClinicaPdfService _pdfService;
    private readonly ApplicationDbContext _context;

    public HistoriaClinicaController(IHistoriaClinicaPdfService pdfService, ApplicationDbContext context)
    {
        _pdfService = pdfService;
        _context = context;
    }

    /// <summary>
    /// Genera y descarga la Historia Clínica completa de un paciente en PDF.
    /// </summary>
    [HttpGet("paciente/{pacienteId:guid}")]
    [Authorize(Policy = PermisosPolicies.HistorialImprimir)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DescargarHistoriaClinica(Guid pacienteId)
    {
        var paciente = await _context.Set<Paciente>()
            .Include(p => p.HistorialClinico)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == pacienteId);

        if (paciente == null)
            return NotFound(ApiResponse<object>.Error("Paciente no encontrado", 404));

        var historial = paciente.HistorialClinico;
        if (historial == null)
            return NotFound(ApiResponse<object>.Error("El paciente no tiene historial clínico", 404));

        // Actualizado: Carga de forma hambrienta todas las nuevas sub-tablas del formulario psicológico
        var atencionesOrdenadas = await _context.Set<Atencion>()
            .Include(a => a.ServicioClinico)
            .Include(a => a.Doctor)
            .Include(a => a.AnamnesisHistoria)
            .Include(a => a.SomaticoVegetativo)
            .Include(a => a.EscalasAnimo)
            .Include(a => a.DesarrolloPsicosocial)
            .Include(a => a.EvaluacionCognitiva)
            .Include(a => a.DiagnosticoCierre)
            .Where(a => a.PacienteId == pacienteId)
            .OrderByDescending(a => a.FechaInicio)
            .AsNoTracking()
            .ToListAsync();

        var ultimaAtencion = atencionesOrdenadas.FirstOrDefault();
        var cierre = ultimaAtencion?.DiagnosticoCierre;

        var nombreFormateado = string.IsNullOrWhiteSpace(paciente.Nombres) 
            ? "No registrado" 
            : $"{paciente.Nombres} {paciente.Apellidos}".Trim();

        var sexoFormateado = paciente.Genero == GeneroEvaluado.Masculino ? "MASCULINO" : "FEMENINO";

        var dto = new HistoriaClinicaPdfDto
        {
            NombresApellidos = nombreFormateado,
            Dni = paciente.DNI ?? "Pendiente",
            FechaNacimiento = paciente.FechaNacimiento ?? DateTime.MinValue,
            Sexo = sexoFormateado,
            LugarNacimiento = paciente.LugarNacimiento ?? "",
            Direccion = "Ficha de Afiliación",
            Correo = "",
            Celular = paciente.Celular ?? "",
            Ocupacion = paciente.Ocupacion ?? "",
            NumeroHistoria = historial.CodigoHistorial,
            FechaRegistro = paciente.FechaRegistro,
            
            // Mapeo adaptado de las notas de la evolución y diagnóstico de cierre
            MotivoConsulta = ultimaAtencion?.ObservacionesIniciales ?? "",
            Indicaciones = cierre?.Recomendaciones ?? "",
            Peso = paciente.PesoKg?.ToString() ?? "",
            Talla = paciente.TallaMetros?.ToString() ?? "",
            
            // Campos obstétricos obsoletos quemados con valores base neutrales para preservar compatibilidad
            Gesta = 0,
            Partos = 0,
            Abortos = 0,
            HijosVivos = 0,
            FUR = null,
            FPP = null,
            AlturaUterina = "",
            Situacion = "",
            Presentacion = "",
            LatidosCardiacosFetales = "",
            Edemas = "",
            
            // Historial resumido de las últimas 5 consultas externas del paciente
            Atenciones = atencionesOrdenadas.Take(5).Select(a => new AtencionResumenDto
            {
                Fecha = a.FechaInicio,
                Servicio = a.ServicioClinico?.Nombre ?? "",
                Doctor = a.Doctor != null ? $"{a.Doctor.Nombres} {a.Doctor.Apellidos}" : "",
                Diagnostico = a.DiagnosticoCierre?.ImpresionDiagnostica ?? "Evolución en progreso"
            }).ToList()
        };

        byte[] pdfBytes;
        try
        {
            pdfBytes = _pdfService.GeneratePdf(dto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.Error($"Error al generar el PDF: {ex.Message}", 500));
        }

        var fileName = $"HistoriaClinica_{paciente.DNI ?? "PCT"}_{DateTime.Now:yyyyMMddHHmm}.pdf";
        return File(pdfBytes, "application/pdf", fileName);
    }
}