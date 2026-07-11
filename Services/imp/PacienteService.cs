using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Pacientes;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;
using psicomedixMonolito.Utils.Helpers;

namespace psicomedixMonolito.Services.imp;

public class PacienteService : IPacienteService
{
    private readonly ApplicationDbContext _context;

    // En el monolito purificado, la persistencia de datos se apoya únicamente en el DbContext unificado
    public PacienteService(ApplicationDbContext context)
    {
        _context = context;
    }

    // ==========================================================
    // CONSULTAS DIRECTAS CON EF CORE (SQL OPTIMIZADO)
    // ==========================================================

    public async Task<IEnumerable<PacienteResponseDto>> ObtenerTodosAsync()
    {
        var pacientes = await _context.Set<Paciente>()
            .Include(x => x.HistorialClinico)
            .AsNoTracking()
            .ToListAsync();

        return pacientes.Select(x => new PacienteResponseDto
        {
            Id = x.Id,
            CodigoPaciente = x.CodigoPaciente,
            DNI = x.DNI,
            Nombres = x.Nombres,
            Apellidos = x.Apellidos,
            FechaNacimiento = x.FechaNacimiento,
            Sexo = x.Sexo,
            Celular = x.Celular,
            Correo = x.Correo,
            Direccion = x.Direccion,
            LugarNacimiento = x.LugarNacimiento,
            GradoInstruccion = x.GradoInstruccion,
            Ocupacion = x.Ocupacion,
            Religion = x.Religion,
            EstadoCivil = x.EstadoCivil,
            NombrePareja = x.NombrePareja,
            CelularPareja = x.CelularPareja,
            Estado = x.Estado,
            FechaRegistro = x.FechaRegistro,
            CodigoHistorial = x.HistorialClinico?.CodigoHistorial,
            HistorialClinicoId = x.HistorialClinico?.Id ?? Guid.Empty
        });
    }

    public async Task<PacienteResponseDto?> ObtenerPorIdAsync(Guid id)
    {
        var paciente = await _context.Set<Paciente>()
            .Include(x => x.HistorialClinico)
                .ThenInclude(x => x!.Detalles)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (paciente == null) return null;
        return MapearPaciente(paciente);
    }

    public async Task<PacienteResponseDto?> ObtenerPorDniAsync(string dni)
    {
        var paciente = await _context.Set<Paciente>()
            .Include(x => x.HistorialClinico)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.DNI == dni);

        if (paciente == null) return null;
        return MapearPaciente(paciente);
    }

    // ==========================================================
    // PROCESO ATÓMICO: CREACIÓN DE PACIENTE Y APERTURA DE HISTORIAL
    // ==========================================================
    public async Task<Guid> CrearAsync(CrearPacienteDto dto, Guid usuarioId)
    {
        var existe = await _context.Set<Paciente>().AnyAsync(x => x.DNI == dto.DNI);
        if (existe)
            throw new InvalidOperationException("Ya existe un paciente registrado con ese DNI.");

        var paciente = new Paciente
        {
            Id = Guid.NewGuid(),
            CodigoPaciente = GenerarCodigoPaciente(dto.DNI),
            DNI = dto.DNI,
            Nombres = dto.Nombres,
            Apellidos = dto.Apellidos,
            FechaNacimiento = FechaHelper.ToUtc(dto.FechaNacimiento),
            Sexo = dto.Sexo,
            Celular = dto.Celular,
            Correo = dto.Correo,
            Direccion = dto.Direccion,
            LugarNacimiento = dto.LugarNacimiento,
            GradoInstruccion = dto.GradoInstruccion,
            Ocupacion = dto.Ocupacion,
            Religion = dto.Religion,
            EstadoCivil = dto.EstadoCivil,
            NombrePareja = dto.NombrePareja,
            CelularPareja = dto.CelularPareja,
            UsuarioId = usuarioId, // Inyectado directamente por parámetro
            FechaRegistro = DateTime.UtcNow,
            Estado = EstadoPaciente.Activo
        };

        // Generación de código correlativo eficiente en el servidor SQL
        var codigoHistorial = await GenerarCodigoHistorialAsync();

        var historial = new HistorialClinico
        {
            Id = Guid.NewGuid(),
            CodigoHistorial = codigoHistorial,
            PacienteId = paciente.Id,
            FechaApertura = DateTime.UtcNow,
            Estado = EstadoHistorialClinico.Activo
        };

        var detalle = new HistorialDetalle
        {
            Id = Guid.NewGuid(),
            CodigoDetalle = GenerarCodigoDetalle("REG", dto.DNI),
            HistorialClinicoId = historial.Id,
            TipoMovimiento = TipoMovimientoHistorial.AperturaHistorial,
            Titulo = "Apertura de historial clínico",
            Descripcion = "Se registró al paciente y se aperturó su historial clínico.",
            FechaRegistro = DateTime.UtcNow,
            UsuarioId = usuarioId // Inyectado directamente por parámetro
        };

        // Enlace transaccional explícito al DbContext ejecutable
        await _context.Set<Paciente>().AddAsync(paciente);
        await _context.Set<HistorialClinico>().AddAsync(historial);
        await _context.Set<HistorialDetalle>().AddAsync(detalle);

        // Operación atómica de confirmación en la base de datos
        await _context.SaveChangesAsync();

        return paciente.Id;
    }

    public async Task ActualizarContactoAsync(Guid id, ActualizarContactoPacienteDto dto)
    {
        var paciente = await _context.Set<Paciente>().FindAsync(id)
            ?? throw new KeyNotFoundException("Paciente no encontrado.");

        paciente.Celular = dto.Celular;
        paciente.Correo = dto.Correo;
        paciente.Direccion = dto.Direccion;

        await _context.SaveChangesAsync();
    }
    
    public async Task CambiarEstadoAsync(Guid id, CambiarEstadoPacienteDto dto)
    {
        var paciente = await _context.Set<Paciente>().FindAsync(id)
            ?? throw new KeyNotFoundException("Paciente no encontrado.");
        
        if (paciente.Estado == EstadoPaciente.Eliminado)
            throw new InvalidOperationException("No se puede cambiar el estado de un paciente eliminado.");

        paciente.Estado = dto.Estado;
        await _context.SaveChangesAsync();
    }

    // ==========================================================
    // HELPERS PRIVADOS Y MAPEADORES INDEPENDIENTES
    // ==========================================================

    private static PacienteResponseDto MapearPaciente(Paciente paciente)
    {
        return new PacienteResponseDto
        {
            Id = paciente.Id,
            CodigoPaciente = paciente.CodigoPaciente,
            DNI = paciente.DNI,
            Nombres = paciente.Nombres,
            Apellidos = paciente.Apellidos,
            FechaNacimiento = paciente.FechaNacimiento,
            Sexo = paciente.Sexo,
            Celular = paciente.Celular,
            Correo = paciente.Correo,
            Direccion = paciente.Direccion,
            LugarNacimiento = paciente.LugarNacimiento,
            GradoInstruccion = paciente.GradoInstruccion,
            Ocupacion = paciente.Ocupacion,
            Religion = paciente.Religion,
            EstadoCivil = paciente.EstadoCivil,
            NombrePareja = paciente.NombrePareja,
            CelularPareja = paciente.CelularPareja,
            Estado = paciente.Estado,
            FechaRegistro = paciente.FechaRegistro,
            CodigoHistorial = paciente.HistorialClinico?.CodigoHistorial,
            HistorialClinicoId = paciente.HistorialClinico?.Id ?? Guid.Empty
        };
    }

    private static string GenerarCodigoPaciente(string dni) =>
        $"PCT-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString("N")[..5].ToUpper()}-{dni}";

    private async Task<string> GenerarCodigoHistorialAsync()
    {
        var anioActual = DateTime.UtcNow.Year.ToString();
        var prefijo = $"FIL-{anioActual}-";

        var ultimoHistorial = await _context.Set<HistorialClinico>()
            .Where(h => h.CodigoHistorial != null && h.CodigoHistorial.StartsWith(prefijo))
            .OrderByDescending(h => h.CodigoHistorial)
            .FirstOrDefaultAsync();

        if (ultimoHistorial == null)
        {
            return $"{prefijo}0001";
        }

        var partes = ultimoHistorial.CodigoHistorial.Split('-');
        if (partes.Length == 3 && int.TryParse(partes[2], out int ultimoCorrelativo))
        {
            int nuevoCorrelativo = ultimoCorrelativo + 1;
            return $"{prefijo}{nuevoCorrelativo:D4}";
        }

        return $"{prefijo}0001";
    }

    private static string GenerarCodigoDetalle(string codigoServicio, string dni) =>
        $"{Guid.NewGuid().ToString("N")[..5].ToUpper()}-{codigoServicio}-{DateTime.UtcNow:yyyy}-{dni}";
}