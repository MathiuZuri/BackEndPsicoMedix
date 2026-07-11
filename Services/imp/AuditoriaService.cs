using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Auditoria;
using psicomedixMonolito.DTOs.Comunes;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.Services.imp;

public class AuditoriaService : IAuditoriaService
{
    private readonly ApplicationDbContext _context;

    // En el monolito solo inyectamos el DbContext de la aplicación
    public AuditoriaService(ApplicationDbContext context)
    {
        _context = context;
    }

    // El Selector de expresiones se mantiene intacto. Es excelente para proyecciones directas a SQL.
    private static Expression<Func<Auditoria, AuditoriaResponseDto>> SelectorDto => x => new AuditoriaResponseDto
    {
        Id = x.Id,
        UsuarioId = x.UsuarioId,
        UsuarioNombre = x.Usuario == null ? null : $"{x.Usuario.Nombres} {x.Usuario.Apellidos}",
        TipoAccion = x.TipoAccion,
        Modulo = x.Modulo,
        EntidadAfectada = x.EntidadAfectada,
        EntidadId = x.EntidadId,
        Descripcion = x.Descripcion,
        ValorAnterior = x.ValorAnterior,
        ValorNuevo = x.ValorNuevo,
        IpAddress = x.IpAddress,
        UserAgent = x.UserAgent,
        FueExitoso = x.FueExitoso,
        DetalleError = x.DetalleError,
        Nivel = x.Nivel,
        FechaHora = x.FechaHora,
        EsConsulta = x.EsConsulta
    };

    // ========== MÉTODOS ORIGINALES REFACTOREADOS (SIN REPOSITORIO) ==========
    
    public async Task<IEnumerable<AuditoriaResponseDto>> ObtenerTodosAsync()
    {
        // Consultamos directo al DbContext usando proyecciones óptimas a la base de datos
        return await _context.Auditorias
            .Include(x => x.Usuario)
            .OrderByDescending(x => x.FechaHora)
            .AsNoTracking()
            .Select(SelectorDto) 
            .ToListAsync();
    }

    public async Task<IEnumerable<AuditoriaResponseDto>> ObtenerPorUsuarioAsync(Guid usuarioId)
    {
        return await _context.Auditorias
            .Include(x => x.Usuario)
            .Where(x => x.UsuarioId == usuarioId)
            .OrderByDescending(x => x.FechaHora)
            .AsNoTracking()
            .Select(SelectorDto)
            .ToListAsync();
    }

    // ========== MÉTODOS CON PAGINACIÓN Y FILTROS DIRECTOS ==========

    public async Task<PaginacionResponseDto<AuditoriaResponseDto>> ObtenerTodosPaginadosAsync(
        PaginacionRequestDto request,
        TipoAccionAuditoria? tipoAccion = null,
        bool? soloConsultas = null)
    {
        // 1. Declaración limpia directo del DbContext
        IQueryable<Auditoria> query = _context.Auditorias
            .Include(x => x.Usuario)
            .AsNoTracking();

        // 2. Filtros dinámicos
        if (tipoAccion.HasValue)
            query = query.Where(x => x.TipoAccion == tipoAccion.Value);

        if (soloConsultas.HasValue)
            query = query.Where(x => x.EsConsulta == soloConsultas.Value);

        // 3. Ordenamiento
        query = query.OrderByDescending(x => x.FechaHora);

        var total = await query.CountAsync();

        // 4. Ejecución física de la paginación
        var items = await query
            .Skip((request.Pagina - 1) * request.CantidadPorPagina)
            .Take(request.CantidadPorPagina)
            .Select(SelectorDto)
            .ToListAsync();

        return new PaginacionResponseDto<AuditoriaResponseDto>
        {
            Pagina = request.Pagina,
            CantidadPorPagina = request.CantidadPorPagina,
            TotalRegistros = total,
            Datos = items
        };
    }

    public async Task<PaginacionResponseDto<AuditoriaResponseDto>> ObtenerPorUsuarioPaginadosAsync(
        Guid usuarioId,
        PaginacionRequestDto request,
        TipoAccionAuditoria? tipoAccion = null,
        bool? soloConsultas = null)
    {
        // 1. Consulta directa filtrando por el ID de usuario principal
        IQueryable<Auditoria> query = _context.Auditorias
            .Include(x => x.Usuario)
            .Where(x => x.UsuarioId == usuarioId)
            .AsNoTracking();

        // 2. Filtros adicionales
        if (tipoAccion.HasValue)
            query = query.Where(x => x.TipoAccion == tipoAccion.Value);

        if (soloConsultas.HasValue)
            query = query.Where(x => x.EsConsulta == soloConsultas.Value);

        // 3. Ordenamiento
        query = query.OrderByDescending(x => x.FechaHora);

        var total = await query.CountAsync();

        // 4. Mapeo y Paginación
        var items = await query
            .Skip((request.Pagina - 1) * request.CantidadPorPagina)
            .Take(request.CantidadPorPagina)
            .Select(SelectorDto)
            .ToListAsync();

        return new PaginacionResponseDto<AuditoriaResponseDto>
        {
            Pagina = request.Pagina,
            CantidadPorPagina = request.CantidadPorPagina,
            TotalRegistros = total,
            Datos = items
        };
    }
}