using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Atenciones;
using psicomedixMonolito.DTOs.Atenciones.Modulos;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models.ATENCIONES.AtencionBase;
using psicomedixMonolito.Services.ATENCIONES;

namespace psicomedixMonolito.Services.imp.ATENCIONES;

public class PsicoFormularioService : IPsicoFormularioService
{
    private readonly ApplicationDbContext _context;

    public PsicoFormularioService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task ActualizarFormularioPsicologicoAsync(Guid atencionId, RegistrarAtencionDto dto)
    {
        var atencion = await _context.Atenciones
            .Include(x => x.AnamnesisHistoria)
            .Include(x => x.SomaticoVegetativo)
            .Include(x => x.EscalasAnimo)
            .Include(x => x.DesarrolloPsicosocial)
            .Include(x => x.EvaluacionCognitiva)
            .Include(x => x.DiagnosticoCierre)
            .FirstOrDefaultAsync(x => x.Id == atencionId)
            ?? throw new KeyNotFoundException("La sesión de atención clínica no existe.");

        if (atencion.Estado == EstadoAtencion.Cerrada)
            throw new InvalidOperationException("No se pueden modificar las secciones clínicas de una evaluación ya cerrada.");

        // Guardado de observaciones
        if (dto.ObservacionesIniciales != null)
            atencion.ObservacionesIniciales = dto.ObservacionesIniciales;

        // ✅ 1. SECCIÓN: ANAMNESIS E HISTORIA
        if (dto.AnamnesisHistoria != null)
        {
            if (atencion.AnamnesisHistoria == null)
            {
                var nueva = MapearDtoAEntidad(dto.AnamnesisHistoria, atencion.Id);
                atencion.AnamnesisHistoria = nueva;
                await _context.PsicoAnamnesisHistoriales.AddAsync(nueva);
            }
            else
                ActualizarCamposEntidad(atencion.AnamnesisHistoria, dto.AnamnesisHistoria);
        }

        // ✅ 2. SECCIÓN: SOMÁTICO VEGETATIVO
        if (dto.SomaticoVegetativo != null)
        {
            if (atencion.SomaticoVegetativo == null)
            {
                var nueva = MapearDtoAEntidad(dto.SomaticoVegetativo, atencion.Id);
                atencion.SomaticoVegetativo = nueva;
                await _context.PsicoSomaticoVegetativos.AddAsync(nueva);
            }
            else
                ActualizarCamposEntidad(atencion.SomaticoVegetativo, dto.SomaticoVegetativo);
        }

        // ✅ 3. SECCIÓN: ESCALAS DE ÁNIMO
        if (dto.EscalasAnimo != null)
        {
            if (atencion.EscalasAnimo == null)
            {
                var nueva = MapearDtoAEntidad(dto.EscalasAnimo, atencion.Id);
                atencion.EscalasAnimo = nueva;
                await _context.PsicoEscalasAnimos.AddAsync(nueva);
            }
            else
                ActualizarCamposEntidad(atencion.EscalasAnimo, dto.EscalasAnimo);
        }

        // ✅ 4. SECCIÓN: DESARROLLO PSICOSOCIAL
        if (dto.DesarrolloPsicosocial != null)
        {
            if (atencion.DesarrolloPsicosocial == null)
            {
                var nueva = MapearDtoAEntidad(dto.DesarrolloPsicosocial, atencion.Id);
                atencion.DesarrolloPsicosocial = nueva;
                await _context.PsicoDesarrollosPsicosociales.AddAsync(nueva);
            }
            else
                ActualizarCamposEntidad(atencion.DesarrolloPsicosocial, dto.DesarrolloPsicosocial);
        }

        // ✅ 5. SECCIÓN: EVALUACIÓN COGNITIVA
        if (dto.EvaluacionCognitiva != null)
        {
            if (atencion.EvaluacionCognitiva == null)
            {
                var nueva = MapearDtoAEntidad(dto.EvaluacionCognitiva, atencion.Id);
                atencion.EvaluacionCognitiva = nueva;
                await _context.PsicoEvaluacionesCognitivas.AddAsync(nueva);
            }
            else
                ActualizarCamposEntidad(atencion.EvaluacionCognitiva, dto.EvaluacionCognitiva);
        }

        // ✅ 6. SECCIÓN: DIAGNÓSTICO Y CIERRE
        if (dto.DiagnosticoCierre != null)
        {
            if (atencion.DiagnosticoCierre == null)
            {
                var nueva = MapearDtoAEntidad(dto.DiagnosticoCierre, atencion.Id);
                atencion.DiagnosticoCierre = nueva;
                await _context.PsicoDiagnosticosCierres.AddAsync(nueva);
            }
            else
                ActualizarCamposEntidad(atencion.DiagnosticoCierre, dto.DiagnosticoCierre);
        }

        await _context.SaveChangesAsync();
    }

    // =========================================================================
    // FÁBRICAS INTERNAS: MAPEO DTO -> ENTIDAD
    // =========================================================================
    private static PsicoAnamnesisHistoria MapearDtoAEntidad(PsicoAnamnesisHistoriaDto d, Guid atId) => new()
    {
        Id = Guid.NewGuid(),
        AtencionId = atId,
        SustanciasNotasGenerales = d.SustanciasNotasGenerales,
        SustanciasLegales = d.SustanciasLegales,
        ConsumoOH = d.ConsumoOH,
        CigarrillosVape = d.CigarrillosVape,
        SustanciasNoLegales = d.SustanciasNoLegales,
        Medicamentos = d.Medicamentos,
        Suplementos = d.Suplementos,
        EnfermedadesAccidentesNotasGenerales = d.EnfermedadesAccidentesNotasGenerales,
        Enfermedades = d.Enfermedades,
        Accidentes = d.Accidentes,
        Cirugias = d.Cirugias,
        Hospitalizacion = d.Hospitalizacion,
        FamiliaresAntecedentesRelacionados = d.FamiliaresAntecedentesRelacionados
    };

    private static PsicoSomaticoVegetativo MapearDtoAEntidad(PsicoSomaticoVegetativoDto d, Guid atId) => new()
    {
        Id = Guid.NewGuid(),
        AtencionId = atId,
        SuenoNotasGenerales = d.SuenoNotasGenerales,
        SuenoDuracionInicio = d.SuenoDuracionInicio,
        SuenoDuracionFin = d.SuenoDuracionFin,
        Ensonaciones = d.Ensonaciones,
        Pesadillas = d.Pesadillas,
        ApneaSueno = d.ApneaSueno,
        Sobresaltos = d.Sobresaltos,
        ParalisisSueno = d.ParalisisSueno,
        SuenoOtros = d.SuenoOtros,
        AlimentacionNotasGenerales = d.AlimentacionNotasGenerales,
        Peso = d.Peso,
        AspectoFisicoActividadFisica = d.AspectoFisicoActividadFisica,
        Apetito = d.Apetito,
        AntecedentesAlteracionesClinicas = d.AntecedentesAlteracionesClinicas,
        SomatizacionesNotasGenerales = d.SomatizacionesNotasGenerales,
        Cefalea = d.Cefalea,
        Adormecimientos = d.Adormecimientos,
        Sudoracion = d.Sudoracion,
        Rubefaccion = d.Rubefaccion,
        SomatizacionesOtros = d.SomatizacionesOtros,
        SignosVitalesNotasGenerales = d.SignosVitalesNotasGenerales,
        SaturacionOxigeno = d.SaturacionOxigeno,
        ReflejoPupilar = d.ReflejoPupilar,
        FrecuenciaCardiaca = d.FrecuenciaCardiaca,
        SignosVitalesOtros = d.SignosVitalesOtros
    };

    private static PsicoEscalasAnimo MapearDtoAEntidad(PsicoEscalasAnimoDto d, Guid atId) => new()
    {
        Id = Guid.NewGuid(),
        AtencionId = atId,
        EscalaIrritabilidad = d.EscalaIrritabilidad,
        EscalaTristeza = d.EscalaTristeza,
        EscalaAnsiedad = d.EscalaAnsiedad,
        EscalaPreocupacion = d.EscalaPreocupacion,
        EscalaImpulsividad = d.EscalaImpulsividad,
        EscalaEstres = d.EscalaEstres,
        EscalaFatiga = d.EscalaFatiga,
        EscalaDesmotivacion = d.EscalaDesmotivacion
    };

    private static PsicoDesarrolloPsicosocial MapearDtoAEntidad(PsicoDesarrolloPsicosocialDto d, Guid atId) => new()
    {
        Id = Guid.NewGuid(),
        AtencionId = atId,
        AutoestimaAutocuidado = d.AutoestimaAutocuidado,
        AcademicoLaboral = d.AcademicoLaboral,
        SocializacionFamilia = d.SocializacionFamilia,
        PersonalidadAutoexpresion = d.PersonalidadAutoexpresion
    };

    private static PsicoEvaluacionCognitiva MapearDtoAEntidad(PsicoEvaluacionCognitivaDto d, Guid atId) => new()
    {
        Id = Guid.NewGuid(),
        AtencionId = atId,
        BeckPersonal = d.BeckPersonal,
        BeckMundoExterior = d.BeckMundoExterior,
        BeckFuturo = d.BeckFuturo,
        BeckAutolesiones = d.BeckAutolesiones,
        BeckAutolisis = d.BeckAutolisis,
        BeckOtros = d.BeckOtros,
        FcPensamiento = d.FcPensamiento,
        FcAtencion = d.FcAtencion,
        FcConcentracion = d.FcConcentracion,
        FcLenguaje = d.FcLenguaje,
        FcPercepcion = d.FcPercepcion,
        FcJuicio = d.FcJuicio,
        FcAbstraccion = d.FcAbstraccion,
        FcAprendizaje = d.FcAprendizaje,
        FcMemoria = d.FcMemoria,
        FcMotivacion = d.FcMotivacion,
        FcEmocion = d.FcEmocion,
        FcCalculo = d.FcCalculo,
        FcCoordinacionMotoraFina = d.FcCoordinacionMotoraFina,
        FcCoordinacionMotoraGruesa = d.FcCoordinacionMotoraGruesa
    };

    private static PsicoDiagnosticoCierre MapearDtoAEntidad(PsicoDiagnosticoCierreDto d, Guid atId) => new()
    {
        Id = Guid.NewGuid(),
        AtencionId = atId,
        DiagnosticoDiferencial1 = d.DiagnosticoDiferencial1,
        DiagnosticoDiferencial2 = d.DiagnosticoDiferencial2,
        DiagnosticoDiferencial3 = d.DiagnosticoDiferencial3,
        ImpresionDiagnostica = d.ImpresionDiagnostica,
        Recomendaciones = d.Recomendaciones
    };

    // =========================================================================
    // ACTUALIZADORES MUTABLES
    // =========================================================================
    private static void ActualizarCamposEntidad(PsicoAnamnesisHistoria e, PsicoAnamnesisHistoriaDto d)
    {
        e.SustanciasNotasGenerales = d.SustanciasNotasGenerales ?? e.SustanciasNotasGenerales;
        e.SustanciasLegales = d.SustanciasLegales ?? e.SustanciasLegales;
        e.ConsumoOH = d.ConsumoOH ?? e.ConsumoOH;
        e.CigarrillosVape = d.CigarrillosVape ?? e.CigarrillosVape;
        e.SustanciasNoLegales = d.SustanciasNoLegales ?? e.SustanciasNoLegales;
        e.Medicamentos = d.Medicamentos ?? e.Medicamentos;
        e.Suplementos = d.Suplementos ?? e.Suplementos;
        e.EnfermedadesAccidentesNotasGenerales = d.EnfermedadesAccidentesNotasGenerales ?? e.EnfermedadesAccidentesNotasGenerales;
        e.Enfermedades = d.Enfermedades ?? e.Enfermedades;
        e.Accidentes = d.Accidentes ?? e.Accidentes;
        e.Cirugias = d.Cirugias ?? e.Cirugias;
        e.Hospitalizacion = d.Hospitalizacion ?? e.Hospitalizacion;
        e.FamiliaresAntecedentesRelacionados = d.FamiliaresAntecedentesRelacionados ?? e.FamiliaresAntecedentesRelacionados;
    }

    private static void ActualizarCamposEntidad(PsicoSomaticoVegetativo e, PsicoSomaticoVegetativoDto d)
    {
        e.SuenoNotasGenerales = d.SuenoNotasGenerales ?? e.SuenoNotasGenerales;
        e.SuenoDuracionInicio = d.SuenoDuracionInicio ?? e.SuenoDuracionInicio;
        e.SuenoDuracionFin = d.SuenoDuracionFin ?? e.SuenoDuracionFin;
        e.Ensonaciones = d.Ensonaciones ?? e.Ensonaciones;
        e.Pesadillas = d.Pesadillas ?? e.Pesadillas;
        e.ApneaSueno = d.ApneaSueno ?? e.ApneaSueno;
        e.Sobresaltos = d.Sobresaltos ?? e.Sobresaltos;
        e.ParalisisSueno = d.ParalisisSueno ?? e.ParalisisSueno;
        e.SuenoOtros = d.SuenoOtros ?? e.SuenoOtros;
        e.AlimentacionNotasGenerales = d.AlimentacionNotasGenerales ?? e.AlimentacionNotasGenerales;
        e.Peso = d.Peso ?? e.Peso;
        e.AspectoFisicoActividadFisica = d.AspectoFisicoActividadFisica ?? e.AspectoFisicoActividadFisica;
        e.Apetito = d.Apetito ?? e.Apetito;
        e.AntecedentesAlteracionesClinicas = d.AntecedentesAlteracionesClinicas ?? e.AntecedentesAlteracionesClinicas;
        e.SomatizacionesNotasGenerales = d.SomatizacionesNotasGenerales ?? e.SomatizacionesNotasGenerales;
        e.Cefalea = d.Cefalea ?? e.Cefalea;
        e.Adormecimientos = d.Adormecimientos ?? e.Adormecimientos;
        e.Sudoracion = d.Sudoracion ?? e.Sudoracion;
        e.Rubefaccion = d.Rubefaccion ?? e.Rubefaccion;
        e.SomatizacionesOtros = d.SomatizacionesOtros ?? e.SomatizacionesOtros;
        e.SignosVitalesNotasGenerales = d.SignosVitalesNotasGenerales ?? e.SignosVitalesNotasGenerales;
        e.SaturacionOxigeno = d.SaturacionOxigeno ?? e.SaturacionOxigeno;
        e.ReflejoPupilar = d.ReflejoPupilar ?? e.ReflejoPupilar;
        e.FrecuenciaCardiaca = d.FrecuenciaCardiaca ?? e.FrecuenciaCardiaca;
        e.SignosVitalesOtros = d.SignosVitalesOtros ?? e.SignosVitalesOtros;
    }

    private static void ActualizarCamposEntidad(PsicoEscalasAnimo e, PsicoEscalasAnimoDto d)
    {
        e.EscalaIrritabilidad = d.EscalaIrritabilidad ?? e.EscalaIrritabilidad;
        e.EscalaTristeza = d.EscalaTristeza ?? e.EscalaTristeza;
        e.EscalaAnsiedad = d.EscalaAnsiedad ?? e.EscalaAnsiedad;
        e.EscalaPreocupacion = d.EscalaPreocupacion ?? e.EscalaPreocupacion;
        e.EscalaImpulsividad = d.EscalaImpulsividad ?? e.EscalaImpulsividad;
        e.EscalaEstres = d.EscalaEstres ?? e.EscalaEstres;
        e.EscalaFatiga = d.EscalaFatiga ?? e.EscalaFatiga;
        e.EscalaDesmotivacion = d.EscalaDesmotivacion ?? e.EscalaDesmotivacion;
    }

    private static void ActualizarCamposEntidad(PsicoDesarrolloPsicosocial e, PsicoDesarrolloPsicosocialDto d)
    {
        e.AutoestimaAutocuidado = d.AutoestimaAutocuidado ?? e.AutoestimaAutocuidado;
        e.AcademicoLaboral = d.AcademicoLaboral ?? e.AcademicoLaboral;
        e.SocializacionFamilia = d.SocializacionFamilia ?? e.SocializacionFamilia;
        e.PersonalidadAutoexpresion = d.PersonalidadAutoexpresion ?? e.PersonalidadAutoexpresion;
    }

    private static void ActualizarCamposEntidad(PsicoEvaluacionCognitiva e, PsicoEvaluacionCognitivaDto d)
    {
        e.BeckPersonal = d.BeckPersonal ?? e.BeckPersonal;
        e.BeckMundoExterior = d.BeckMundoExterior ?? e.BeckMundoExterior;
        e.BeckFuturo = d.BeckFuturo ?? e.BeckFuturo;
        e.BeckAutolesiones = d.BeckAutolesiones ?? e.BeckAutolesiones;
        e.BeckAutolisis = d.BeckAutolisis ?? e.BeckAutolisis;
        e.BeckOtros = d.BeckOtros ?? e.BeckOtros;
        e.FcPensamiento = d.FcPensamiento ?? e.FcPensamiento;
        e.FcAtencion = d.FcAtencion ?? e.FcAtencion;
        e.FcConcentracion = d.FcConcentracion ?? e.FcConcentracion;
        e.FcLenguaje = d.FcLenguaje ?? e.FcLenguaje;
        e.FcPercepcion = d.FcPercepcion ?? e.FcPercepcion;
        e.FcJuicio = d.FcJuicio ?? e.FcJuicio;
        e.FcAbstraccion = d.FcAbstraccion ?? e.FcAbstraccion;
        e.FcAprendizaje = d.FcAprendizaje ?? e.FcAprendizaje;
        e.FcMemoria = d.FcMemoria ?? e.FcMemoria;
        e.FcMotivacion = d.FcMotivacion ?? e.FcMotivacion;
        e.FcEmocion = d.FcEmocion ?? e.FcEmocion;
        e.FcCalculo = d.FcCalculo ?? e.FcCalculo;
        e.FcCoordinacionMotoraFina = d.FcCoordinacionMotoraFina ?? e.FcCoordinacionMotoraFina;
        e.FcCoordinacionMotoraGruesa = d.FcCoordinacionMotoraGruesa ?? e.FcCoordinacionMotoraGruesa;
    }

    private static void ActualizarCamposEntidad(PsicoDiagnosticoCierre e, PsicoDiagnosticoCierreDto d)
    {
        e.DiagnosticoDiferencial1 = d.DiagnosticoDiferencial1 ?? e.DiagnosticoDiferencial1;
        e.DiagnosticoDiferencial2 = d.DiagnosticoDiferencial2 ?? e.DiagnosticoDiferencial2;
        e.DiagnosticoDiferencial3 = d.DiagnosticoDiferencial3 ?? e.DiagnosticoDiferencial3;
        e.ImpresionDiagnostica = d.ImpresionDiagnostica ?? e.ImpresionDiagnostica;
        e.Recomendaciones = d.Recomendaciones ?? e.Recomendaciones;
    }
}