using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Atenciones;
using psicomedixMonolito.DTOs.Atenciones.Modulos;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;
using psicomedixMonolito.Models.ATENCIONES;

namespace psicomedixMonolito.Services.imp;

public class AtencionService : IAtencionService
{
    private readonly ApplicationDbContext _context;

    // ✅ Solo depende del DbContext, igual que UsuarioService
    public AtencionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AtencionResponseDto>> ObtenerTodasAsync()
    {
        var atenciones = await _context.Atenciones
            .Include(a => a.Paciente)
            .Include(a => a.Doctor)
                .ThenInclude(d => d.Usuario)
            .Include(a => a.ServicioClinico)
            .Include(a => a.Pagos)
            .AsNoTracking()
            .ToListAsync();

        return atenciones.Select(MapearAtencion);
    }

    public async Task<IEnumerable<AtencionResponseDto>> ObtenerPorPacienteAsync(Guid pacienteId)
    {
        var atenciones = await _context.Atenciones
            .Include(x => x.Paciente)
            .Include(x => x.Doctor)
                .ThenInclude(d => d.Usuario)
            .Include(x => x.ServicioClinico)
            .Include(x => x.Cita)
            .Include(x => x.Anamnesis)
            .Include(x => x.ImpresionDiagnostica)
            .Where(x => x.PacienteId == pacienteId)
            .OrderByDescending(x => x.FechaInicio)
            .ToListAsync();

        return atenciones.Select(MapearAtencion);
    }

    public async Task<AtencionResponseDto?> ObtenerPorIdAsync(Guid id)
    {
        var atencion = await _context.Atenciones
            .Include(x => x.Paciente)
            .Include(x => x.Doctor)
                .ThenInclude(d => d.Usuario)
            .Include(x => x.ServicioClinico)
            .Include(x => x.Cita)
            .Include(x => x.Pagos)
            .Include(x => x.Anamnesis)
            .Include(x => x.ExamenesFisicos)
            .Include(x => x.TactosVaginales)
            .Include(x => x.Ecografias)
            .Include(x => x.ImpresionDiagnostica)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (atencion == null) return null;
        return MapearAtencion(atencion);
    }

    // 🚩 usuarioId ahora viene del controlador
    public async Task<Guid> RegistrarAtencionAsync(RegistrarAtencionDto dto, Guid usuarioId)
    {
        var paciente = await _context.Set<Paciente>().FindAsync(dto.PacienteId) 
            ?? throw new KeyNotFoundException("Paciente no encontrado.");

        var servicio = await _context.Set<ServicioClinico>().FindAsync(dto.ServicioClinicoId)
            ?? throw new KeyNotFoundException("Servicio no encontrado.");

        if (!dto.HistorialClinicoId.HasValue || dto.HistorialClinicoId.Value == Guid.Empty)
            throw new InvalidOperationException("El identificador del historial clínico es obligatorio.");

        Guid historialIdReal = dto.HistorialClinicoId.Value;

        var atencion = new Atencion
        {
            Id = Guid.NewGuid(),
            CodigoAtencion = GenerarCodigoAtencion(servicio.CodigoServicio),
            PacienteId = dto.PacienteId,
            DoctorId = dto.DoctorId,
            ServicioClinicoId = dto.ServicioClinicoId,
            CitaId = dto.CitaId,
            HistorialClinicoId = historialIdReal,
            FechaInicio = DateTime.UtcNow,
            Estado = EstadoAtencion.Abierta
        };

        // --- Mapeo de módulos clínicos ---
        if (dto.Anamnesis != null)
        {
            atencion.Anamnesis = new Anamnesis
            {
                Id = Guid.NewGuid(),
                MotivoConsulta = dto.Anamnesis.MotivoConsulta,
                Gestaciones = dto.Anamnesis.Gestaciones,
                HijosVivos = dto.Anamnesis.HijosVivos,
                Abortos = dto.Anamnesis.Abortos,
                PartosPretermino = dto.Anamnesis.PartosPretermino,
                PartosATermino = dto.Anamnesis.PartosATermino,
                FechaUltimaRegla = ToUtc(dto.Anamnesis.FechaUltimaRegla),
                FechaProbableParto = ToUtc(dto.Anamnesis.FechaProbableParto),
                EdadGestacional = dto.Anamnesis.EdadGestacional,
                Alergias = dto.Anamnesis.Alergias,
                EnfermedadesCronicas = dto.Anamnesis.EnfermedadesCronicas,
                CirugiasPrevias = dto.Anamnesis.CirugiasPrevias,
                AntecedentesAdicionales = dto.Anamnesis.AntecedentesAdicionales
            };
        }

        atencion.ExamenesFisicos = dto.ExamenesFisicos?.Select(e => new ExamenFisico
        {
            Id = Guid.NewGuid(),
            FechaHoraExamen = DateTime.SpecifyKind(e.FechaHoraExamen, DateTimeKind.Utc),
            Lotep = e.Lotep,
            EstadoGeneral = e.EstadoGeneral,
            EstadoHidratacion = e.EstadoHidratacion,
            EstadoNutricion = e.EstadoNutricion,
            EscalaGlasgow = e.EscalaGlasgow,
            UteroGravido = e.UteroGravido,
            AlturaUterina = e.AlturaUterina,
            SituacionPosicionPresentacion = e.SituacionPosicionPresentacion,
            LatidosCardiacosFetales = e.LatidosCardiacosFetales,
            MovimientosFetales = e.MovimientosFetales,
            TonoUterino = e.TonoUterino,
            DinamicaUterina = e.DinamicaUterina,
            SangradoTv = e.SangradoTv,
            PerdidaLiquidoAmniotico = e.PerdidaLiquidoAmniotico,
            ColorLiquidoAmniotico = e.ColorLiquidoAmniotico,
            TaponMucoso = e.TaponMucoso,
            FlujoVaginal = e.FlujoVaginal,
            PunoPercusionLumbar = e.PunoPercusionLumbar,
            Edemas = e.Edemas,
            ReflejosOsteotendinosos = e.ReflejosOsteotendinosos
        }).ToList() ?? new List<ExamenFisico>();

        atencion.TactosVaginales = dto.TactosVaginales?.Select(t => new TactoVaginal
        {
            Id = Guid.NewGuid(),
            FechaHora = DateTime.SpecifyKind(t.FechaHora, DateTimeKind.Utc),
            Dilatacion = t.Dilatacion,
            Borramiento = t.Borramiento,
            AlturaPresentacion = t.AlturaPresentacion,
            MembranasOvulares = t.MembranasOvulares,
            ColorLiquido = t.ColorLiquido,
            Pelvis = t.Pelvis,
            VariedadPresentacion = t.VariedadPresentacion
        }).ToList() ?? new List<TactoVaginal>();

        atencion.Ecografias = dto.Ecografias?.Select(e => new EcografiaObstetrica
        {
            Id = Guid.NewGuid(),
            FechaHora = DateTime.SpecifyKind(e.FechaHora, DateTimeKind.Utc),
            DiametroBiparietal = e.DiametroBiparietal,
            CircunferenciaCefalica = e.CircunferenciaCefalica,
            CircunferenciaAbdominal = e.CircunferenciaAbdominal,
            LongitudFemur = e.LongitudFemur,
            PesoFetalEstimado = e.PesoFetalEstimado,
            IndiceLiquidoAmniotico = e.IndiceLiquidoAmniotico,
            PlacentaLocalizacion = e.PlacentaLocalizacion,
            PlacentaGranum = e.PlacentaGranum,
            CircularCordon = e.CircularCordon,
            Conclusiones = e.Conclusiones
        }).ToList() ?? new List<EcografiaObstetrica>();

        if (dto.ImpresionDiagnostica != null)
        {
            atencion.ImpresionDiagnostica = new ImpresionDiagnostica
            {
                Id = Guid.NewGuid(),
                DiagnosticoPrincipal = dto.ImpresionDiagnostica.DiagnosticoPrincipal,
                DiagnosticosSecundarios = dto.ImpresionDiagnostica.DiagnosticosSecundarios,
                IndicacionesReceta = dto.ImpresionDiagnostica.IndicacionesReceta,
                FechaProximaCita = ToUtc(dto.ImpresionDiagnostica.FechaProximaCita),
                MotivoProximaCita = dto.ImpresionDiagnostica.MotivoProximaCita
            };
        }

        // Creación del pago asociado
        var pago = new Pago
        {
            Id = Guid.NewGuid(),
            CodigoPago = GenerarCodigo("PAG", paciente.DNI),
            AtencionId = atencion.Id,
            PacienteId = dto.PacienteId,
            ServicioClinicoId = dto.ServicioClinicoId,
            MontoTotal = dto.CostoFinal,
            MontoPagado = 0,
            SaldoPendiente = dto.CostoFinal,
            Estado = EstadoPago.Pendiente,
            FechaPago = DateTime.UtcNow,
            UsuarioRegistroId = usuarioId
        };

        // Registro en el historial clínico
        var detalle = new HistorialDetalle
        {
            Id = Guid.NewGuid(),
            CodigoDetalle = GenerarCodigoDetalle(servicio.CodigoServicio),
            HistorialClinicoId = historialIdReal,
            AtencionId = atencion.Id,
            TipoMovimiento = TipoMovimientoHistorial.AtencionRegistrada,
            Titulo = "Apertura de Consulta Externa",
            Descripcion = dto.Anamnesis != null ? $"Motivo de consulta: {dto.Anamnesis.MotivoConsulta}" : "Atención aperturada sin anamnesis inicial.",
            FechaRegistro = DateTime.UtcNow,
            UsuarioId = usuarioId
        };

        // Actualización de cita si existe
        if (dto.CitaId.HasValue)
        {
            var cita = await _context.Set<Cita>().FindAsync(dto.CitaId.Value);
            if (cita != null)
            {
                cita.Estado = EstadoCita.EnProgreso;
            }
        }

        await _context.Set<Atencion>().AddAsync(atencion);
        await _context.Set<Pago>().AddAsync(pago);
        await _context.Set<HistorialDetalle>().AddAsync(detalle);
        await _context.SaveChangesAsync();

        return atencion.Id;
    }

    public async Task CerrarAtencionAsync(Guid id, CerrarAtencionDto dto)
    {
        var atencion = await _context.Atenciones
            .Include(x => x.ImpresionDiagnostica)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (atencion == null) throw new KeyNotFoundException("Atención no encontrada.");
        if (atencion.Estado == EstadoAtencion.Cerrada) throw new InvalidOperationException("La atención ya está cerrada.");

        atencion.FechaCierre = DateTime.UtcNow;
        atencion.Estado = EstadoAtencion.Cerrada;

        if (atencion.ImpresionDiagnostica == null)
        {
            atencion.ImpresionDiagnostica = new ImpresionDiagnostica
            {
                Id = Guid.NewGuid(),
                AtencionId = atencion.Id,
                DiagnosticoPrincipal = dto.ImpresionDiagnostica.DiagnosticoPrincipal,
                DiagnosticosSecundarios = dto.ImpresionDiagnostica.DiagnosticosSecundarios,
                IndicacionesReceta = dto.ImpresionDiagnostica.IndicacionesReceta,
                FechaProximaCita = ToUtc(dto.ImpresionDiagnostica.FechaProximaCita),
                MotivoProximaCita = dto.ImpresionDiagnostica.MotivoProximaCita
            };
        }
        else
        {
            atencion.ImpresionDiagnostica.DiagnosticoPrincipal = dto.ImpresionDiagnostica.DiagnosticoPrincipal;
            atencion.ImpresionDiagnostica.DiagnosticosSecundarios = dto.ImpresionDiagnostica.DiagnosticosSecundarios;
            atencion.ImpresionDiagnostica.IndicacionesReceta = dto.ImpresionDiagnostica.IndicacionesReceta;
            atencion.ImpresionDiagnostica.FechaProximaCita = ToUtc(dto.ImpresionDiagnostica.FechaProximaCita);
            atencion.ImpresionDiagnostica.MotivoProximaCita = dto.ImpresionDiagnostica.MotivoProximaCita;
        }

        if (!string.IsNullOrEmpty(dto.ObservacionesFinales))
        {
            atencion.ImpresionDiagnostica.DiagnosticosSecundarios += $"\nOBSERVACIONES: {dto.ObservacionesFinales}";
        }

        if (atencion.CitaId.HasValue)
        {
            var cita = await _context.Set<Cita>().FindAsync(atencion.CitaId.Value);
            if (cita != null)
            {
                cita.Estado = EstadoCita.Atendida;
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task AnularAtencionAsync(Guid id, string motivo)
    {
        var atencion = await _context.Set<Atencion>().FindAsync(id);
        if (atencion == null) throw new KeyNotFoundException("Atención no encontrada.");
        if (atencion.Estado == EstadoAtencion.Cerrada) throw new InvalidOperationException("No se puede anular una atención cerrada.");

        atencion.Estado = EstadoAtencion.Anulada;

        if (atencion.CitaId.HasValue)
        {
            var cita = await _context.Set<Cita>().FindAsync(atencion.CitaId.Value);
            if (cita != null)
            {
                cita.Estado = EstadoCita.Cancelada;
            }
        }

        await _context.SaveChangesAsync();
    }

    // ===================== MAPEOS Y GENERADORES =====================
    private static AtencionResponseDto MapearAtencion(Atencion atencion)
    {
        return new AtencionResponseDto
        {
            Id = atencion.Id,
            CodigoAtencion = atencion.CodigoAtencion,
            PacienteId = atencion.PacienteId,
            PacienteNombre = $"{atencion.Paciente?.Nombres} {atencion.Paciente?.Apellidos}".Trim(),
            DoctorId = atencion.DoctorId,
            DoctorNombre = $"{atencion.Doctor?.Usuario?.Nombres} {atencion.Doctor?.Usuario?.Apellidos}".Trim(),
            ServicioClinicoId = atencion.ServicioClinicoId,
            ServicioNombre = atencion.ServicioClinico?.Nombre ?? string.Empty,
            CitaId = atencion.CitaId,
            HistorialClinicoId = atencion.HistorialClinicoId ?? Guid.Empty,
            FechaInicio = atencion.FechaInicio,
            FechaCierre = atencion.FechaCierre,
            Estado = atencion.Estado,

            CostoFinal = atencion.Pagos?.Sum(p => p.MontoTotal) ?? 0,
            MontoPagado = atencion.Pagos?.Sum(p => p.MontoPagado) ?? 0,
            SaldoPendiente = atencion.Pagos?.Sum(p => p.SaldoPendiente) ?? 0,

            Anamnesis = atencion.Anamnesis != null ? MapearAnamnesis(atencion.Anamnesis) : null,
            ExamenesFisicos = atencion.ExamenesFisicos?.Select(MapearExamenFisico).ToList() ?? new List<ExamenFisicoDto>(),
            TactosVaginales = atencion.TactosVaginales?.Select(MapearTactoVaginal).ToList() ?? new List<TactoVaginalDto>(),
            Ecografias = atencion.Ecografias?.Select(MapearEcografia).ToList() ?? new List<EcografiaObstetricaDto>(),
            ImpresionDiagnostica = atencion.ImpresionDiagnostica != null ? MapearImpresionDiagnostica(atencion.ImpresionDiagnostica) : null
        };
    }

    private static AnamnesisDto MapearAnamnesis(Anamnesis an) => new()
    {
        MotivoConsulta = an.MotivoConsulta,
        Gestaciones = an.Gestaciones,
        HijosVivos = an.HijosVivos,
        Abortos = an.Abortos,
        PartosPretermino = an.PartosPretermino,
        PartosATermino = an.PartosATermino,
        FechaUltimaRegla = an.FechaUltimaRegla,
        FechaProbableParto = an.FechaProbableParto,
        EdadGestacional = an.EdadGestacional,
        Alergias = an.Alergias,
        EnfermedadesCronicas = an.EnfermedadesCronicas,
        CirugiasPrevias = an.CirugiasPrevias,
        AntecedentesAdicionales = an.AntecedentesAdicionales
    };

    private static ExamenFisicoDto MapearExamenFisico(ExamenFisico e) => new()
    {
        FechaHoraExamen = e.FechaHoraExamen,
        Lotep = e.Lotep,
        EstadoGeneral = e.EstadoGeneral,
        EstadoHidratacion = e.EstadoHidratacion,
        EstadoNutricion = e.EstadoNutricion,
        EscalaGlasgow = e.EscalaGlasgow,
        UteroGravido = e.UteroGravido,
        AlturaUterina = e.AlturaUterina,
        SituacionPosicionPresentacion = e.SituacionPosicionPresentacion,
        LatidosCardiacosFetales = e.LatidosCardiacosFetales,
        MovimientosFetales = e.MovimientosFetales,
        TonoUterino = e.TonoUterino,
        DinamicaUterina = e.DinamicaUterina,
        SangradoTv = e.SangradoTv,
        PerdidaLiquidoAmniotico = e.PerdidaLiquidoAmniotico,
        ColorLiquidoAmniotico = e.ColorLiquidoAmniotico,
        TaponMucoso = e.TaponMucoso,
        FlujoVaginal = e.FlujoVaginal,
        PunoPercusionLumbar = e.PunoPercusionLumbar,
        Edemas = e.Edemas,
        ReflejosOsteotendinosos = e.ReflejosOsteotendinosos
    };

    private static TactoVaginalDto MapearTactoVaginal(TactoVaginal t) => new()
    {
        FechaHora = t.FechaHora,
        Dilatacion = t.Dilatacion,
        Borramiento = t.Borramiento,
        AlturaPresentacion = t.AlturaPresentacion,
        MembranasOvulares = t.MembranasOvulares,
        ColorLiquido = t.ColorLiquido,
        Pelvis = t.Pelvis,
        VariedadPresentacion = t.VariedadPresentacion
    };

    private static EcografiaObstetricaDto MapearEcografia(EcografiaObstetrica e) => new()
    {
        FechaHora = e.FechaHora,
        DiametroBiparietal = e.DiametroBiparietal,
        CircunferenciaCefalica = e.CircunferenciaCefalica,
        CircunferenciaAbdominal = e.CircunferenciaAbdominal,
        LongitudFemur = e.LongitudFemur,
        PesoFetalEstimado = e.PesoFetalEstimado,
        IndiceLiquidoAmniotico = e.IndiceLiquidoAmniotico,
        PlacentaLocalizacion = e.PlacentaLocalizacion,
        PlacentaGranum = e.PlacentaGranum,
        CircularCordon = e.CircularCordon,
        Conclusiones = e.Conclusiones
    };

    private static ImpresionDiagnosticaDto MapearImpresionDiagnostica(ImpresionDiagnostica id) => new()
    {
        DiagnosticoPrincipal = id.DiagnosticoPrincipal,
        DiagnosticosSecundarios = id.DiagnosticosSecundarios,
        IndicacionesReceta = id.IndicacionesReceta,
        FechaProximaCita = id.FechaProximaCita,
        MotivoProximaCita = id.MotivoProximaCita
    };

    private static string GenerarCodigoAtencion(string codigoServicio) =>
        $"ATN-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString("N")[..5].ToUpper()}-{codigoServicio.ToUpper()}";

    private static string GenerarCodigoDetalle(string codigoServicio) =>
        $"{Guid.NewGuid().ToString("N")[..5].ToUpper()}-{codigoServicio}-{DateTime.UtcNow:yyyy}";

    private static string GenerarCodigo(string prefijo, string dni) =>
        $"{Guid.NewGuid().ToString("N")[..5].ToUpper()}-{prefijo}-{DateTime.UtcNow:yyyy}-{dni}";
    
    private static DateTime? ToUtc(DateTime? dateTime) =>
        dateTime.HasValue ? DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc) : null;
}