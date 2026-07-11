using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Comunes;
using psicomedixMonolito.DTOs.Doctores;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;
using psicomedixMonolito.Utils.Helpers;

namespace psicomedixMonolito.Services.imp;

public class DoctorService : IDoctorService
{
    private readonly ApplicationDbContext _context;

    // Inyectamos únicamente el DbContext centralizado de la aplicación
    public DoctorService(ApplicationDbContext context)
    {
        _context = context;
    }

    // ==========================================================
    // CONSULTAS GENERALES DIRECTAS A LAS TABLAS
    // ==========================================================

    public async Task<IEnumerable<DoctorResponseDto>> ObtenerTodosAsync()
    {
        var doctores = await _context.Set<Doctor>()
            .AsNoTracking()
            .ToListAsync();

        return doctores.Select(MapearDoctor);
    }

    public async Task<IEnumerable<DoctorResponseDto>> ObtenerActivosAsync()
    {
        var doctores = await _context.Set<Doctor>()
            .Where(x => x.Estado == EstadoDoctor.Activo)
            .AsNoTracking()
            .ToListAsync();

        return doctores.Select(MapearDoctor);
    }

    public async Task<DoctorResponseDto?> ObtenerPorIdAsync(Guid id)
    {
        var doctor = await _context.Set<Doctor>().FindAsync(id);
        if (doctor == null) return null;

        return MapearDoctor(doctor);
    }

    // ==========================================================
    // OPERACIONES DE ESCRITURA Y MANTENIMIENTO
    // ==========================================================

    public async Task<Guid> CrearAsync(CrearDoctorDto dto, Guid usuarioId)
    {
        // Validación directa contra la tabla Doctores
        var existe = await _context.Set<Doctor>()
            .FirstOrDefaultAsync(x => x.CMP == dto.CMP);

        if (existe != null)
            throw new InvalidOperationException("Ya existe un doctor registrado con ese CMP.");

        if (dto.FechaFinContrato.HasValue && dto.FechaFinContrato.Value < dto.FechaInicioContrato)
            throw new InvalidOperationException("La fecha de fin de contrato no puede ser menor que la fecha de inicio.");

        var doctor = new Doctor
        {
            Id = Guid.NewGuid(),
            CodigoDoctor = GenerarCodigoDoctor(dto.CMP),
            CMP = dto.CMP,
            Nombres = dto.Nombres,
            Apellidos = dto.Apellidos,
            Especialidad = dto.Especialidad,
            Celular = dto.Celular,
            Correo = dto.Correo,
            FechaInicioContrato = FechaHelper.ToUtc(dto.FechaInicioContrato),
            FechaFinContrato = FechaHelper.ToUtc(dto.FechaFinContrato),
            UsuarioId = usuarioId, // Recibido directamente por parámetro
            Estado = EstadoDoctor.Activo
        };

        await _context.Set<Doctor>().AddAsync(doctor);
        await _context.SaveChangesAsync();

        return doctor.Id;
    }

    public async Task ActualizarAsync(Guid id, EditarDoctorDto dto)
    {
        var doctor = await _context.Set<Doctor>().FindAsync(id)
            ?? throw new KeyNotFoundException("Doctor no encontrado.");

        if (dto.FechaFinContrato.HasValue && dto.FechaFinContrato.Value < dto.FechaInicioContrato)
            throw new InvalidOperationException("La fecha de fin de contrato no puede ser menor que la fecha de inicio.");

        doctor.CMP = dto.CMP;
        doctor.Nombres = dto.Nombres;
        doctor.Apellidos = dto.Apellidos;
        doctor.Especialidad = dto.Especialidad;
        doctor.Celular = dto.Celular;
        doctor.Correo = dto.Correo;
        doctor.FechaInicioContrato = FechaHelper.ToUtc(dto.FechaInicioContrato);
        doctor.FechaFinContrato = FechaHelper.ToUtc(dto.FechaFinContrato);
        doctor.Estado = dto.Estado;

        await _context.SaveChangesAsync();
    }

    // ==========================================================
    // PROCESO COMPLEJO: CONTRATACIÓN DE MÉDICO Y CREACIÓN DE USUARIO
    // ==========================================================
    public async Task<Guid> ContratarAsync(ContratarDoctorDto dto)
    {
        // 1. Validaciones directas usando el DbContext unificado
        var existeDoctor = await _context.Set<Doctor>().AnyAsync(x => x.CMP == dto.CMP);
        if (existeDoctor)
            throw new InvalidOperationException("Ya existe un doctor con ese CMP.");

        var existeUsuario = await _context.Set<Usuario>().AnyAsync(x => x.UserName == dto.UserName);
        if (existeUsuario)
            throw new InvalidOperationException("El nombre de usuario ya está en uso.");

        var existeCorreo = await _context.Set<Usuario>().AnyAsync(x => x.Correo == dto.CorreoUsuario);
        if (existeCorreo)
            throw new InvalidOperationException("El correo ya está registrado.");

        // 2. Instanciación del objeto Usuario
        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            CodigoUsuario = GenerarCodigoUsuario(),
            Nombres = dto.Nombres,
            Apellidos = dto.Apellidos,
            UserName = dto.UserName,
            Correo = dto.CorreoUsuario,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            FechaRegistro = DateTime.UtcNow,
            Estado = EstadoUsuario.Activo,
            DebeCambiarContrasena = true
        };

        await _context.Set<Usuario>().AddAsync(usuario);

        // 3. Resolución y Validación del Rol asignado
        Guid rolId;
        if (dto.RolId.HasValue)
        {
            var rol = await _context.Set<Rol>().FindAsync(dto.RolId.Value)
                ?? throw new KeyNotFoundException("El rol especificado no existe.");
            rolId = rol.Id;
        }
        else
        {
            var rolDoctor = await _context.Set<Rol>().FirstOrDefaultAsync(x => x.Nombre == "Doctor");
            if (rolDoctor == null)
                throw new InvalidOperationException("El rol 'Doctor' no existe en el sistema. Ejecute el seeder.");
            rolId = rolDoctor.Id;
        }

        // Asignar el rol al usuario de manera directa en la tabla relacional
        var yaTieneRol = await _context.Set<UsuarioRol>()
            .AnyAsync(x => x.UsuarioId == usuario.Id && x.RolId == rolId);
            
        if (!yaTieneRol)
        {
            await _context.Set<UsuarioRol>().AddAsync(new UsuarioRol
            {
                UsuarioId = usuario.Id,
                RolId = rolId,
                FechaAsignacion = DateTime.UtcNow,
                Activo = true
            });
        }

        // 4. Asignación transaccional de permisos adicionales al Rol (si aplica)
        if (dto.PermisosIds != null && dto.PermisosIds.Any())
        {
            foreach (var permisoId in dto.PermisosIds.Distinct())
            {
                var permisoExiste = await _context.Set<Permiso>().AnyAsync(p => p.Id == permisoId);
                if (!permisoExiste) continue;

                var yaTienePermiso = await _context.Set<RolPermiso>()
                    .AnyAsync(x => x.RolId == rolId && x.PermisoId == permisoId);
                    
                if (!yaTienePermiso)
                {
                    await _context.Set<RolPermiso>().AddAsync(new RolPermiso
                    {
                        RolId = rolId,
                        PermisoId = permisoId,
                        FechaAsignacion = DateTime.UtcNow
                    });
                }
            }
        }

        // 5. Crear la entidad Doctor vinculada al Usuario generado
        var doctor = new Doctor
        {
            Id = Guid.NewGuid(),
            CodigoDoctor = GenerarCodigoDoctor(dto.CMP),
            CMP = dto.CMP,
            Nombres = dto.Nombres,
            Apellidos = dto.Apellidos,
            Especialidad = dto.Especialidad,
            Celular = dto.Celular,
            Correo = dto.Correo,
            FechaInicioContrato = FechaHelper.ToUtc(dto.FechaInicioContrato),
            FechaFinContrato = FechaHelper.ToUtc(dto.FechaFinContrato),
            UsuarioId = usuario.Id, // Se vincula directamente al Id del usuario recién creado
            Estado = EstadoDoctor.Activo
        };

        await _context.Set<Doctor>().AddAsync(doctor);
        
        // Un solo SaveChanges maneja todo de forma segura
        await _context.SaveChangesAsync();

        return doctor.Id;
    }

    // ==========================================================
    // BÚSQUEDA AVANZADA CON FILTROS E IQUERYABLE (SQL-SIDE OPTIMIZED)
    // ==========================================================
    public async Task<PaginacionResponseDto<DoctorResponseDto>> BuscarAsync(
        string? nombre,
        string? especialidad,
        EstadoDoctor? estado,
        PaginacionRequestDto request)
    {
        IQueryable<Doctor> query = _context.Set<Doctor>().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(nombre))
        {
            nombre = nombre.Trim().ToLower();
            query = query.Where(d =>
                d.Nombres.ToLower().Contains(nombre) ||
                d.Apellidos.ToLower().Contains(nombre));
        }

        if (!string.IsNullOrWhiteSpace(especialidad))
        {
            especialidad = especialidad.Trim().ToLower();
            query = query.Where(d => d.Especialidad.ToLower().Contains(especialidad));
        }

        if (estado.HasValue)
        {
            query = query.Where(d => d.Estado == estado.Value);
        }

        var total = await query.CountAsync();

        var items = await query
            .OrderBy(d => d.Nombres)
            .Skip((request.Pagina - 1) * request.CantidadPorPagina)
            .Take(request.CantidadPorPagina)
            .ToListAsync();

        return new PaginacionResponseDto<DoctorResponseDto>
        {
            Pagina = request.Pagina,
            CantidadPorPagina = request.CantidadPorPagina,
            TotalRegistros = total,
            Datos = items.Select(MapearDoctor).ToList()
        };
    }

    // ==========================================================
    // HELPERS PRIVADOS Y MAPEADORES
    // ==========================================================
    private static DoctorResponseDto MapearDoctor(Doctor doctor)
    {
        return new DoctorResponseDto
        {
            Id = doctor.Id,
            CodigoDoctor = doctor.CodigoDoctor,
            CMP = doctor.CMP,
            Nombres = doctor.Nombres,
            Apellidos = doctor.Apellidos,
            Especialidad = doctor.Especialidad,
            Celular = doctor.Celular,
            Correo = doctor.Correo,
            FechaInicioContrato = doctor.FechaInicioContrato,
            FechaFinContrato = doctor.FechaFinContrato,
            Estado = doctor.Estado
        };
    }

    private static string GenerarCodigoDoctor(string cmp) =>
        $"DOC-{Guid.NewGuid().ToString("N")[..5].ToUpper()}-{cmp}";

    private static string GenerarCodigoUsuario() =>
        $"USR-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString("N")[..5].ToUpper()}";
}