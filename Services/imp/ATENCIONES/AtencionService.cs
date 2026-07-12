using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Atenciones;
using psicomedixMonolito.DTOs.Atenciones.Modulos;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;
using psicomedixMonolito.Models.ATENCIONES;
using psicomedixMonolito.Models.ATENCIONES.AtencionBase;
using psicomedixMonolito.Services.ATENCIONES;

namespace psicomedixMonolito.Services.imp.ATENCIONES;

public class AtencionService : IAtencionService
{
    private readonly ApplicationDbContext _context;

    public AtencionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AtencionResponseDto>> ObtenerTodasAsync()
    {
        var atenciones = await _context.Atenciones
            .Include(a => a.Paciente)
            .Include(a => a.Doctor)
            .Include(a => a.ServicioClinico)
            .Include(a => a.Pagos)
            .Include(a => a.AnamnesisHistoria)
            .Include(a => a.SomaticoVegetativo)
            .Include(a => a.EscalasAnimo)
            .Include(a => a.DesarrolloPsicosocial)
            .Include(a => a.EvaluacionCognitiva)
            .Include(a => a.DiagnosticoCierre)
            .AsNoTracking()
            .ToListAsync();

        return atenciones.Select(MapearAtencion);
    }

    public async Task<IEnumerable<AtencionResponseDto>> ObtenerPorPacienteAsync(Guid pacienteId)
    {
        var atenciones = await _context.Atenciones
            .Include(x => x.Paciente)
            .Include(x => x.Doctor)
            .Include(x => x.ServicioClinico)
            .Include(x => x.Pagos) // <- Esto faltaba y por eso salía costo 0
            .Include(x => x.AnamnesisHistoria)
            .Include(x => x.SomaticoVegetativo) // <- Esto faltaba y por eso salía null
            .Include(x => x.EscalasAnimo)        // <- Esto faltaba y por eso salía null
            .Include(x => x.DesarrolloPsicosocial) // <- Esto faltaba y por eso salía null
            .Include(x => x.EvaluacionCognitiva)   // <- Esto faltaba y por eso salía null
            .Include(x => x.DiagnosticoCierre)
            .Where(x => x.PacienteId == pacienteId)
            .OrderByDescending(x => x.FechaInicio)
            .AsNoTracking()
            .ToListAsync();

        return atenciones.Select(MapearAtencion);
    }

    public async Task<AtencionResponseDto?> ObtenerPorIdAsync(Guid id)
    {
        // El GET mantiene la carga de todas las sub-tablas para cumplir con el principio de Documento Único
        var atencion = await _context.Atenciones
            .Include(x => x.Paciente)
            .Include(x => x.Doctor)
            .Include(x => x.ServicioClinico)
            .Include(x => x.Pagos)
            .Include(x => x.AnamnesisHistoria)
            .Include(x => x.SomaticoVegetativo)
            .Include(x => x.EscalasAnimo)
            .Include(x => x.DesarrolloPsicosocial)
            .Include(x => x.EvaluacionCognitiva)
            .Include(x => x.DiagnosticoCierre)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (atencion == null) return null;
        return MapearAtencion(atencion);
    }

    public async Task<Guid> RegistrarAtencionAsync(RegistrarAtencionDto dto, Guid usuarioId)
    {
        var paciente = await _context.Set<Paciente>().FindAsync(dto.PacienteId) 
            ?? throw new KeyNotFoundException("Paciente no encontrado.");

        var servicio = await _context.Set<ServicioClinico>().FindAsync(dto.ServicioClinicoId)
            ?? throw new KeyNotFoundException("Servicio no encontrado.");

        if (!dto.HistorialClinicoId.HasValue || dto.HistorialClinicoId.Value == Guid.Empty)
            throw new InvalidOperationException("El identificador del historial clínico es obligatorio.");

        // Apertura Core limpia y purificada de lógica de especialidades
        var atencion = new Atencion
        {
            Id = Guid.NewGuid(),
            CodigoAtencion = $"ATN-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString("N")[..5].ToUpper()}-{servicio.CodigoServicio.ToUpper()}",
            PacienteId = dto.PacienteId,
            DoctorId = dto.DoctorId,
            ServicioClinicoId = dto.ServicioClinicoId,
            CitaId = dto.CitaId,
            HistorialClinicoId = dto.HistorialClinicoId.Value,
            FechaInicio = DateTime.UtcNow,
            ObservacionesIniciales = dto.ObservacionesIniciales,
            Estado = EstadoAtencion.Abierta
        };

        // Registro de caja base financiero obligatorio
        var pago = new Pago
        {
            Id = Guid.NewGuid(),
            CodigoPago = $"{Guid.NewGuid().ToString("N")[..5].ToUpper()}-PAG-{DateTime.UtcNow.Year}",
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

        var detalleHistorial = new HistorialDetalle
        {
            Id = Guid.NewGuid(),
            CodigoDetalle = $"{Guid.NewGuid().ToString("N")[..5].ToUpper()}-EVO-{DateTime.UtcNow.Year}",
            HistorialClinicoId = dto.HistorialClinicoId.Value,
            AtencionId = atencion.Id,
            TipoMovimiento = TipoMovimientoHistorial.AtencionRegistrada,
            Titulo = "Apertura de Evolución Clínica",
            Descripcion = dto.ObservacionesIniciales ?? "Sesión terapéutica base aperturada.",
            FechaRegistro = DateTime.UtcNow,
            UsuarioId = usuarioId
        };

        if (dto.CitaId.HasValue)
        {
            var cita = await _context.Set<Cita>().FindAsync(dto.CitaId.Value);
            if (cita != null) cita.Estado = EstadoCita.EnProgreso;
        }

        await _context.Set<Atencion>().AddAsync(atencion);
        await _context.Set<Pago>().AddAsync(pago);
        await _context.Set<HistorialDetalle>().AddAsync(detalleHistorial);
        await _context.SaveChangesAsync();

        return atencion.Id;
    }

    public async Task CerrarAtencionAsync(Guid id, CerrarAtencionDto dto)
    {
        var atencion = await _context.Atenciones
            .Include(x => x.DiagnosticoCierre)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (atencion == null) throw new KeyNotFoundException("Atención no encontrada.");
        if (atencion.Estado == EstadoAtencion.Cerrada) throw new InvalidOperationException("La atención ya está cerrada.");

        atencion.FechaCierre = DateTime.UtcNow;
        atencion.Estado = EstadoAtencion.Cerrada;

        // El cierre inyecta el bloque de diagnóstico conclusivo
        if (atencion.DiagnosticoCierre == null)
        {
            atencion.DiagnosticoCierre = new PsicoDiagnosticoCierre
            {
                Id = Guid.NewGuid(),
                AtencionId = atencion.Id,
                DiagnosticoDiferencial1 = dto.DiagnosticoCierre.DiagnosticoDiferencial1,
                DiagnosticoDiferencial2 = dto.DiagnosticoCierre.DiagnosticoDiferencial2,
                DiagnosticoDiferencial3 = dto.DiagnosticoCierre.DiagnosticoDiferencial3,
                ImpresionDiagnostica = dto.DiagnosticoCierre.ImpresionDiagnostica,
                Recomendaciones = dto.DiagnosticoCierre.Recomendaciones
            };
        }
        else
        {
            atencion.DiagnosticoCierre.DiagnosticoDiferencial1 = dto.DiagnosticoCierre.DiagnosticoDiferencial1;
            atencion.DiagnosticoCierre.DiagnosticoDiferencial2 = dto.DiagnosticoCierre.DiagnosticoDiferencial2;
            atencion.DiagnosticoCierre.DiagnosticoDiferencial3 = dto.DiagnosticoCierre.DiagnosticoDiferencial3;
            atencion.DiagnosticoCierre.ImpresionDiagnostica = dto.DiagnosticoCierre.ImpresionDiagnostica;
            atencion.DiagnosticoCierre.Recomendaciones = dto.DiagnosticoCierre.Recomendaciones;
        }

        if (!string.IsNullOrEmpty(dto.ObservacionesFinales))
        {
            atencion.DiagnosticoCierre.ImpresionDiagnostica += $"\nOBSERVACIONES FINALES: {dto.ObservacionesFinales}";
        }

        if (atencion.CitaId.HasValue)
        {
            var cita = await _context.Set<Cita>().FindAsync(atencion.CitaId.Value);
            if (cita != null) cita.Estado = EstadoCita.Atendida;
        }

        await _context.SaveChangesAsync();
    }

    public async Task AnularAtencionAsync(Guid id, string motivo)
    {
        var atencion = await _context.Set<Atencion>().FindAsync(id);
        if (atencion == null) throw new KeyNotFoundException("Atención no encontrada.");
        if (atencion.Estado == EstadoAtencion.Cerrada) throw new InvalidOperationException("No se puede anular una sesión cerrada.");

        atencion.Estado = EstadoAtencion.Anulada;
        await _context.SaveChangesAsync();
    }

    // =========================================================================
    // 🖨️ MAPEADOR DE CONSULTA DE EXPEDIENTES (Documento Unificado de Salida)
    // =========================================================================
    private static AtencionResponseDto MapearAtencion(Atencion a)
    {
        return new AtencionResponseDto
        {
            Id = a.Id,
            CodigoAtencion = a.CodigoAtencion,
            PacienteId = a.PacienteId,
            PacienteNombre = $"{a.Paciente?.Nombres} {a.Paciente?.Apellidos}".Trim(),
            DoctorId = a.DoctorId,
            DoctorNombre = $"{a.Doctor?.Nombres} {a.Doctor?.Apellidos}".Trim(),
            ServicioClinicoId = a.ServicioClinicoId,
            ServicioNombre = a.ServicioClinico?.Nombre ?? string.Empty,
            CitaId = a.CitaId,
            HistorialClinicoId = a.HistorialClinicoId ?? Guid.Empty,
            FechaInicio = a.FechaInicio,
            FechaCierre = a.FechaCierre,
            Estado = a.Estado.ToString(),
            ObservacionesIniciales = a.ObservacionesIniciales,

            CostoFinal = a.Pagos?.Sum(p => p.MontoTotal) ?? 0,
            MontoPagado = a.Pagos?.Sum(p => p.MontoPagado) ?? 0,
            SaldoPendiente = a.Pagos?.Sum(p => p.SaldoPendiente) ?? 0,

            AnamnesisHistoria = a.AnamnesisHistoria != null ? new PsicoAnamnesisHistoriaDto
            {
                SustanciasNotasGenerales = a.AnamnesisHistoria.SustanciasNotasGenerales, SustanciasLegales = a.AnamnesisHistoria.SustanciasLegales, ConsumoOH = a.AnamnesisHistoria.ConsumoOH, CigarrillosVape = a.AnamnesisHistoria.CigarrillosVape, SustanciasNoLegales = a.AnamnesisHistoria.SustanciasNoLegales, Medicamentos = a.AnamnesisHistoria.Medicamentos, Suplementos = a.AnamnesisHistoria.Suplementos,
                EnfermedadesAccidentesNotasGenerales = a.AnamnesisHistoria.EnfermedadesAccidentesNotasGenerales, Enfermedades = a.AnamnesisHistoria.Enfermedades, Accidentes = a.AnamnesisHistoria.Accidentes, Cirugias = a.AnamnesisHistoria.Cirugias, Hospitalizacion = a.AnamnesisHistoria.Hospitalizacion, FamiliaresAntecedentesRelacionados = a.AnamnesisHistoria.FamiliaresAntecedentesRelacionados
            } : null,

            SomaticoVegetativo = a.SomaticoVegetativo != null ? new PsicoSomaticoVegetativoDto
            {
                SuenoNotasGenerales = a.SomaticoVegetativo.SuenoNotasGenerales, SuenoDuracionInicio = a.SomaticoVegetativo.SuenoDuracionInicio, SuenoDuracionFin = a.SomaticoVegetativo.SuenoDuracionFin, Ensonaciones = a.SomaticoVegetativo.Ensonaciones, Pesadillas = a.SomaticoVegetativo.Pesadillas, ApneaSueno = a.SomaticoVegetativo.ApneaSueno, Sobresaltos = a.SomaticoVegetativo.Sobresaltos, ParalisisSueno = a.SomaticoVegetativo.ParalisisSueno, SuenoOtros = a.SomaticoVegetativo.SuenoOtros,
                AlimentacionNotasGenerales = a.SomaticoVegetativo.AlimentacionNotasGenerales, Peso = a.SomaticoVegetativo.Peso, AspectoFisicoActividadFisica = a.SomaticoVegetativo.AspectoFisicoActividadFisica, Apetito = a.SomaticoVegetativo.Apetito, AntecedentesAlteracionesClinicas = a.SomaticoVegetativo.AntecedentesAlteracionesClinicas,
                SomatizacionesNotasGenerales = a.SomaticoVegetativo.SomatizacionesNotasGenerales, Cefalea = a.SomaticoVegetativo.Cefalea, Adormecimientos = a.SomaticoVegetativo.Adormecimientos, Sudoracion = a.SomaticoVegetativo.Sudoracion, Rubefaccion = a.SomaticoVegetativo.Rubefaccion, SomatizacionesOtros = a.SomaticoVegetativo.SomatizacionesOtros,
                SignosVitalesNotasGenerales = a.SomaticoVegetativo.SignosVitalesNotasGenerales, SaturacionOxigeno = a.SomaticoVegetativo.SaturacionOxigeno, ReflejoPupilar = a.SomaticoVegetativo.ReflejoPupilar, FrecuenciaCardiaca = a.SomaticoVegetativo.FrecuenciaCardiaca, SignosVitalesOtros = a.SomaticoVegetativo.SignosVitalesOtros
            } : null,

            EscalasAnimo = a.EscalasAnimo != null ? new PsicoEscalasAnimoDto
            {
                EscalaIrritabilidad = a.EscalasAnimo.EscalaIrritabilidad, EscalaTristeza = a.EscalasAnimo.EscalaTristeza, EscalaAnsiedad = a.EscalasAnimo.EscalaAnsiedad, EscalaPreocupacion = a.EscalasAnimo.EscalaPreocupacion, EscalaImpulsividad = a.EscalasAnimo.EscalaImpulsividad, EscalaEstres = a.EscalasAnimo.EscalaEstres, EscalaFatiga = a.EscalasAnimo.EscalaFatiga, EscalaDesmotivacion = a.EscalasAnimo.EscalaDesmotivacion
            } : null,

            DesarrolloPsicosocial = a.DesarrolloPsicosocial != null ? new PsicoDesarrolloPsicosocialDto
            {
                AutoestimaAutocuidado = a.DesarrolloPsicosocial.AutoestimaAutocuidado, AcademicoLaboral = a.DesarrolloPsicosocial.AcademicoLaboral, SocializacionFamilia = a.DesarrolloPsicosocial.SocializacionFamilia, PersonalidadAutoexpresion = a.DesarrolloPsicosocial.PersonalidadAutoexpresion
            } : null,

            EvaluacionCognitiva = a.EvaluacionCognitiva != null ? new PsicoEvaluacionCognitivaDto
            {
                BeckPersonal = a.EvaluacionCognitiva.BeckPersonal, BeckMundoExterior = a.EvaluacionCognitiva.BeckMundoExterior, BeckFuturo = a.EvaluacionCognitiva.BeckFuturo, BeckAutolesiones = a.EvaluacionCognitiva.BeckAutolesiones, BeckAutolisis = a.EvaluacionCognitiva.BeckAutolisis, BeckOtros = a.EvaluacionCognitiva.BeckOtros,
                FcPensamiento = a.EvaluacionCognitiva.FcPensamiento, FcAtencion = a.EvaluacionCognitiva.FcAtencion, FcConcentracion = a.EvaluacionCognitiva.FcConcentracion, FcLenguaje = a.EvaluacionCognitiva.FcLenguaje, FcPercepcion = a.EvaluacionCognitiva.FcPercepcion, FcJuicio = a.EvaluacionCognitiva.FcJuicio, FcAbstraccion = a.EvaluacionCognitiva.FcAbstraccion, FcAprendizaje = a.EvaluacionCognitiva.FcAprendizaje, FcMemoria = a.EvaluacionCognitiva.FcMemoria, FcMotivacion = a.EvaluacionCognitiva.FcMotivacion, FcEmocion = a.EvaluacionCognitiva.FcEmocion, FcCalculo = a.EvaluacionCognitiva.FcCalculo, FcCoordinacionMotoraFina = a.EvaluacionCognitiva.FcCoordinacionMotoraFina, FcCoordinacionMotoraGruesa = a.EvaluacionCognitiva.FcCoordinacionMotoraGruesa
            } : null,

            DiagnosticoCierre = a.DiagnosticoCierre != null ? new PsicoDiagnosticoCierreDto
            {
                DiagnosticoDiferencial1 = a.DiagnosticoCierre.DiagnosticoDiferencial1, DiagnosticoDiferencial2 = a.DiagnosticoCierre.DiagnosticoDiferencial2, DiagnosticoDiferencial3 = a.DiagnosticoCierre.DiagnosticoDiferencial3, ImpresionDiagnostica = a.DiagnosticoCierre.ImpresionDiagnostica, Recomendaciones = a.DiagnosticoCierre.Recomendaciones
            } : null
        };
    }
}