using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.PDFsDto;
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
    private readonly ApplicationDbContext _context; // 🚀 Único punto de contacto con la persistencia en el monolito

    public HistoriaClinicaController(
        IHistoriaClinicaPdfService pdfService,
        ApplicationDbContext context)
    {
        _pdfService = pdfService;
        _context = context;
    }

    /// <summary>
    /// Genera y descarga la Historia Clínica completa de un paciente en PDF.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite obtener un documento formal con la ficha de identificación,
    /// antecedentes, funciones vitales, examen obstétrico y las últimas atenciones.
    /// Ideal para entregar al paciente o para expedientes médicos.
    /// **Permiso requerido:** Aludido en la directiva de políticas de acceso.
    /// </remarks>
    [HttpGet("paciente/{pacienteId:guid}")]
    [Authorize(Policy = PermisosPolicies.HistorialImprimir)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DescargarHistoriaClinica(Guid pacienteId)
    {
        // 🚀 Consulta Directa 1: Extraemos los datos del paciente y su relación uno a uno con el historial
        var paciente = await _context.Set<Paciente>()
            .Include(p => p.HistorialClinico)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == pacienteId);

        if (paciente == null)
            return NotFound(ApiResponse<object>.Error("Paciente no encontrado", 404));

        var historial = paciente.HistorialClinico;
        if (historial == null)
            return NotFound(ApiResponse<object>.Error("El paciente no tiene historial clínico", 404));

        // 🚀 Consulta Directa 2: Traemos las atenciones del paciente resolviendo todo el árbol clínico en un solo viaje
        var atencionesOrdenadas = await _context.Set<Atencion>()
            .Include(a => a.ServicioClinico)
            .Include(a => a.Doctor)
            .Include(a => a.Anamnesis)
            .Include(a => a.ExamenesFisicos)
            .Include(a => a.ImpresionDiagnostica)
            .Where(a => a.PacienteId == pacienteId)
            .OrderByDescending(a => a.FechaInicio)
            .AsNoTracking()
            .ToListAsync();

        var ultimaAtencion = atencionesOrdenadas.FirstOrDefault();
        var anamnesis = ultimaAtencion?.Anamnesis;
        
        // Obtenemos el examen físico más reciente de la última atención registrada
        var examenFisico = ultimaAtencion?.ExamenesFisicos?
            .OrderByDescending(e => e.FechaHoraExamen)
            .FirstOrDefault();
            
        var diagnostico = ultimaAtencion?.ImpresionDiagnostica;

        // Mapeo limpio directo al DTO plano de QuestPDF
        var dto = new HistoriaClinicaPdfDto
        {
            NombresApellidos = $"{paciente.Nombres} {paciente.Apellidos}",
            Dni = paciente.DNI,
            FechaNacimiento = paciente.FechaNacimiento,
            Sexo = paciente.Sexo == "M" ? "MASCULINO" : "FEMENINO",
            LugarNacimiento = paciente.LugarNacimiento ?? "",
            Direccion = paciente.Direccion ?? "",
            Correo = paciente.Correo ?? "",
            Celular = paciente.Celular ?? "",
            Ocupacion = paciente.Ocupacion ?? "",
            MotivoConsulta = anamnesis?.MotivoConsulta ?? "",
            NumeroHistoria = historial.CodigoHistorial,
            FechaRegistro = paciente.FechaRegistro,
            Menarquia = "",
            RitmoCatamenial = "",
            Gesta = anamnesis?.Gestaciones ?? 0,
            Partos = anamnesis?.PartosATermino ?? 0,
            Abortos = anamnesis?.Abortos ?? 0,
            HijosVivos = anamnesis?.HijosVivos ?? 0,
            HijosMuertos = 0,
            FUR = anamnesis?.FechaUltimaRegla,
            FPP = anamnesis?.FechaProbableParto,
            PI = "",
            MetodoAnticonceptivo = "",
            PA = "",
            Pulso = "",
            Temperatura = "",
            Respiracion = "",
            SO2 = "",
            Peso = "",
            Talla = "",
            AlturaUterina = examenFisico?.AlturaUterina?.ToString() ?? "",
            Situacion = examenFisico?.SituacionPosicionPresentacion ?? "",
            Presentacion = examenFisico?.SituacionPosicionPresentacion ?? "",
            LatidosCardiacosFetales = examenFisico?.LatidosCardiacosFetales?.ToString() ?? "",
            Edemas = examenFisico?.Edemas ?? "",
            Indicaciones = diagnostico?.IndicacionesReceta ?? "",
            Atenciones = atencionesOrdenadas.Take(5).Select(a => new AtencionResumenDto
            {
                Fecha = a.FechaInicio,
                Servicio = a.ServicioClinico?.Nombre ?? "",
                Doctor = a.Doctor != null ? $"{a.Doctor.Nombres} {a.Doctor.Apellidos}" : "",
                Diagnostico = a.ImpresionDiagnostica?.DiagnosticoPrincipal ?? ""
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

        var fileName = $"HistoriaClinica_{paciente.DNI}_{DateTime.Now:yyyyMMddHHmm}.pdf";
        return File(pdfBytes, "application/pdf", fileName);
    }
}