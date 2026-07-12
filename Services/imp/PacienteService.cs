using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Pacientes;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.Services.imp;

public class PacienteService : IPacienteService
{
    private readonly ApplicationDbContext _context;

    public PacienteService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PacienteResponseDto>> ObtenerTodosAsync()
    {
        var pacientes = await _context.Set<Paciente>()
            .Include(x => x.HistorialClinico)
            .Include(x => x.FamiliaresDirectos)
            .Include(x => x.Doctor)        
            .Include(x => x.RecepcionadoPor)  
            .AsNoTracking()
            .ToListAsync();

        return pacientes.Select(MapearPaciente);
    }

    public async Task<PacienteResponseDto?> ObtenerPorIdAsync(Guid id)
    {
        var paciente = await _context.Set<Paciente>()
            .Include(x => x.HistorialClinico)
            .Include(x => x.FamiliaresDirectos)
            .Include(x => x.Doctor)          
            .Include(x => x.RecepcionadoPor) 
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (paciente == null) return null;
        return MapearPaciente(paciente);
    }

    public async Task<PacienteResponseDto?> ObtenerPorDniAsync(string dni)
    {
        if (string.IsNullOrWhiteSpace(dni)) return null;

        var paciente = await _context.Set<Paciente>()
            .Include(x => x.HistorialClinico)
            .Include(x => x.FamiliaresDirectos)
            .Include(x => x.Doctor)        
            .Include(x => x.RecepcionadoPor) 
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.DNI == dni.Trim());

        if (paciente == null) return null;
        return MapearPaciente(paciente);
    }

    public async Task<Guid> CrearAsync(CrearPacienteDto dto, Guid usuarioId)
    {
        if (!string.IsNullOrWhiteSpace(dto.DNI))
        {
            var existe = await _context.Set<Paciente>().AnyAsync(x => x.DNI == dto.DNI.Trim());
            if (existe) throw new InvalidOperationException("Ya existe un paciente registrado con ese DNI.");
        }

        var dniLimpio = dto.DNI?.Trim();

        // 🚀 Lógica de Evaluación de IMC: Si el doctor lo digitó se guarda, sino se calcula automáticamente
        decimal? imcFinal = dto.IMC;
        if ((imcFinal == null || imcFinal == 0) && dto.PesoKg.HasValue && dto.TallaMetros.HasValue && dto.TallaMetros.Value > 0)
        {
            imcFinal = Math.Round(dto.PesoKg.Value / (dto.TallaMetros.Value * dto.TallaMetros.Value), 2);
        }

        var paciente = new Paciente
        {
            Id = Guid.NewGuid(),
            CodigoPaciente = $"PCT-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString("N")[..5].ToUpper()}",
            DNI = dniLimpio,
            FechaAtencion = dto.FechaAtencion,
            
            TieneInformante = dto.TieneInformante,
            InformanteNombre = dto.InformanteNombre,
            InformanteParentesco = dto.InformanteParentesco,
            InformanteOcupacion = dto.InformanteOcupacion,
            InformanteEstadoCivil = dto.InformanteEstadoCivil,
            InformanteEdad = dto.InformanteEdad,
            InformanteCelular = dto.InformanteCelular,

            PrimeraAtencionPsicologia = dto.PrimeraAtencionPsicologia,
            Nombres = dto.Nombres,
            Apellidos = dto.Apellidos,
            Genero = dto.Genero,
            EdadAnos = dto.EdadAnos,
            EdadMeses = dto.EdadMeses,
            Celular = dto.Celular,
            FechaNacimiento = dto.FechaNacimiento,
            EstadoCivil = dto.EstadoCivil,
            LugarNacimiento = dto.LugarNacimiento,
            Ocupacion = dto.Ocupacion,
            GradoInstruccion = dto.GradoInstruccion,
            GradoSemestreCiclo = dto.GradoSemestreCiclo,
            Carrera = dto.Carrera,

            TieneMasHermanosHijos = dto.TieneMasHermanosHijos,
            TieneMediosHermanosPadrastros = dto.TieneMediosHermanosPadrastros,
            IntegrantesFamilia = dto.IntegrantesFamilia,
            Religion = dto.Religion,
            PesoKg = dto.PesoKg,
            TallaMetros = dto.TallaMetros,
            
            // Asignación de valores evaluados
            IMC = imcFinal,

            // Mapeo corregido directo a la infraestructura de Doctores
            DoctorId = dto.DoctorId,
            OtrasEspecialidadesRaw = dto.OtrasEspecialidadesRequeridas != null ? string.Join(",", dto.OtrasEspecialidadesRequeridas) : null,
            ConsultaAsuntosLegales = dto.ConsultaAsuntosLegales,
            MotivoConsulta = dto.MotivoConsulta,
            RecepcionadoPorId = usuarioId,

            UsuarioId = usuarioId,
            FechaRegistro = DateTime.UtcNow, // Corrección de tipografía de inicialización
            Estado = EstadoPaciente.Activo
        };

        // Guardado de la grilla de familiares directos
        if (dto.Familiares != null && dto.Familiares.Any())
        {
            foreach (var f in dto.Familiares)
            {
                paciente.FamiliaresDirectos.Add(new PacienteFamiliar
                {
                    Id = Guid.NewGuid(),
                    PacienteId = paciente.Id,
                    Parentesco = f.Parentesco,
                    Nombres = f.Nombres,
                    Edad = f.Edad,
                    Ocupacion = f.Ocupacion,
                    VivenJuntos = f.VivenJuntos
                });
            }
        }

        var codigoHistorial = await GenerarCodigoHistorialAsync();
        var historial = new HistorialClinico
        {
            Id = Guid.NewGuid(),
            CodigoHistorial = codigoHistorial,
            PacienteId = paciente.Id,
            FechaApertura = DateTime.UtcNow,
            Estado = EstadoHistorialClinico.Activo
        };

        await _context.Set<Paciente>().AddAsync(paciente);
        await _context.Set<HistorialClinico>().AddAsync(historial);
        await _context.SaveChangesAsync();

        return paciente.Id;
    }

    public async Task ActualizarContactoAsync(Guid id, ActualizarContactoPacienteDto dto)
    {
        var paciente = await _context.Set<Paciente>().FindAsync(id)
            ?? throw new KeyNotFoundException("Paciente no encontrado.");

        paciente.Celular = dto.Celular;
        await _context.SaveChangesAsync();
    }

    public async Task CambiarEstadoAsync(Guid id, CambiarEstadoPacienteDto dto)
    {
        var paciente = await _context.Set<Paciente>().FindAsync(id)
            ?? throw new KeyNotFoundException("Paciente no encontrado.");

        paciente.Estado = dto.Estado;
        await _context.SaveChangesAsync();
    }

    private static PacienteResponseDto MapearPaciente(Paciente x)
    {
        return new PacienteResponseDto
        {
            Id = x.Id,
            CodigoPaciente = x.CodigoPaciente,
            DNI = x.DNI,
            
            // SECCIÓN 0
            FechaAtencion = x.FechaAtencion,

            // SECCIÓN I
            TieneInformante = x.TieneInformante,
            InformanteNombre = x.InformanteNombre,
            InformanteParentesco = x.InformanteParentesco,
            InformanteOcupacion = x.InformanteOcupacion,
            InformanteEstadoCivil = x.InformanteEstadoCivil?.ToString(),
            InformanteEdad = x.InformanteEdad,
            InformanteCelular = x.InformanteCelular,

            // SECCIÓN II
            PrimeraAtencionPsicologia = x.PrimeraAtencionPsicologia,
            Nombres = x.Nombres,
            Apellidos = x.Apellidos,
            NombreCompleto = string.IsNullOrWhiteSpace(x.Nombres) ? "No registrado" : $"{x.Nombres} {x.Apellidos}".Trim(),
            Genero = x.Genero?.ToString(),
            EdadAnos = x.EdadAnos,
            EdadMeses = x.EdadMeses,
            Celular = x.Celular,
            FechaNacimiento = x.FechaNacimiento,
            EstadoCivil = x.EstadoCivil?.ToString(),
            LugarNacimiento = x.LugarNacimiento,
            Ocupacion = x.Ocupacion,
            GradoInstruccion = x.GradoInstruccion?.ToString(),
            GradoSemestreCiclo = x.GradoSemestreCiclo,
            Carrera = x.Carrera,

            // SECCIÓN III
            TieneMasHermanosHijos = x.TieneMasHermanosHijos,
            TieneMediosHermanosPadrastros = x.TieneMediosHermanosPadrastros,
            IntegrantesFamilia = x.IntegrantesFamilia,
            Religion = x.Religion,
            PesoKg = x.PesoKg,
            TallaMetros = x.TallaMetros,
            IMC = x.IMC,

            // Mapeo dinámico de la sub-colección de Familiares Directos
            FamiliaresDirectos = x.FamiliaresDirectos?.Select(f => new FamiliarDto
            {
                Parentesco = f.Parentesco,
                Nombres = f.Nombres,
                Edad = f.Edad,
                Ocupacion = f.Ocupacion,
                VivenJuntos = f.VivenJuntos
            }).ToList() ?? new List<FamiliarDto>(),

            // SECCIÓN IV
            DoctorId = x.DoctorId,
            DoctorNombre = x.Doctor != null 
                ? $"{x.Doctor.Nombres} {x.Doctor.Apellidos}".Trim() 
                : "No asignado",
            
            OtrasEspecialidadesRequeridas = !string.IsNullOrEmpty(x.OtrasEspecialidadesRaw)
                ? x.OtrasEspecialidadesRaw.Split(',').ToList()
                : new List<string>(),
            ConsultaAsuntosLegales = x.ConsultaAsuntosLegales,
            MotivoConsulta = x.MotivoConsulta,
        
            RecepcionadoPorId = x.RecepcionadoPorId,
            RecepcionadoPorNombre = x.RecepcionadoPor != null 
                ? $"{x.RecepcionadoPor.Nombres} {x.RecepcionadoPor.Apellidos}".Trim() 
                : "Sistema / Admin",

            // Infraestructura base
            Estado = x.Estado.ToString(),
            CodigoHistorial = x.HistorialClinico?.CodigoHistorial,
            FechaRegistro = x.FechaRegistro
        };
    }

    private async Task<string> GenerarCodigoHistorialAsync()
    {
        var prefijo = $"FIL-{DateTime.UtcNow.Year}-";
        var ultimo = await _context.Set<HistorialClinico>()
            .Where(h => h.CodigoHistorial != null && h.CodigoHistorial.StartsWith(prefijo))
            .OrderByDescending(h => h.CodigoHistorial)
            .FirstOrDefaultAsync();

        if (ultimo == null) return $"{prefijo}0001";
        var partes = ultimo.CodigoHistorial.Split('-');
        if (partes.Length == 3 && int.TryParse(partes[2], out int correlativo))
        {
            return $"{prefijo}{(correlativo + 1):D4}";
        }
        return $"{prefijo}0001";
    }
}