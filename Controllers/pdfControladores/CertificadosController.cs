using System.IO.Compression;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.PDFsDto;
using psicomedixMonolito.Models;
using psicomedixMonolito.Services.Interfacespdf;
using psicomedixMonolito.Utils.Authorization;

namespace psicomedixMonolito.Controllers.pdfControladores;

[ApiController]
[Route("api/[controller]")]
[Tags("Documentos PDF - Certificados")]
public class CertificadosController : ControllerBase
{
    private readonly ICertificadoTrabajoPdfService _pdfService;
    private readonly ApplicationDbContext _context; // 🚀 Único punto de contacto con los datos en el monolito

    public CertificadosController(
        ICertificadoTrabajoPdfService pdfService,
        ApplicationDbContext context)
    {
        _pdfService = pdfService;
        _context = context;
    }

    /// <summary>
    /// Descarga el certificado de trabajo del usuario autenticado.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite a un usuario obtener su propio certificado laboral en PDF.
    /// **Permiso requerido:** <see cref="PermisosPolicies.CertificadoGenerar"/>.
    /// </remarks>
    [HttpGet("mi-certificado")]
    [Authorize(Policy = PermisosPolicies.CertificadoGenerar)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DescargarMiCertificado()
    {
        // 🚀 Ajuste Monolítico: Extraemos el ID directamente desde el contexto de claims HTTP
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                      User.FindFirst("sub")?.Value ??
                      User.FindFirst("usuarioId")?.Value ??
                      User.FindFirst("UsuarioId")?.Value;

        if (string.IsNullOrEmpty(idClaim) || !Guid.TryParse(idClaim, out var usuarioId))
        {
            return Unauthorized(ApiResponse<object>.Error("Usuario no autenticado", 401));
        }

        var dto = await ConstruirDtoDesdeUsuario(usuarioId);
        if (dto == null)
            return NotFound(ApiResponse<object>.Error("Usuario no encontrado", 404));

        var pdfBytes = _pdfService.GeneratePdf(dto);
        var fileName = $"CertificadoTrabajo_{dto.CodigoUsuario}_{DateTime.Now:yyyyMMddHHmm}.pdf";
        return File(pdfBytes, "application/pdf", fileName);
    }

    /// <summary>
    /// Genera certificados de trabajo en bloque para múltiples usuarios y los devuelve en un ZIP.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite a administradores o directores generar certificados para un grupo de usuarios
    /// (por lista de IDs o por nombre de rol). Útil para procesos de entrega masiva.
    /// **Permiso requerido:** <see cref="PermisosPolicies.CertificadoBlock"/>.
    /// </remarks>
    [HttpPost("block")]
    [Authorize(Policy = PermisosPolicies.CertificadoBlock)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GenerarCertificadosEnBloque([FromBody] CertificadoBlockRequest request)
    {
        if (request == null || (request.UsuarioIds == null && string.IsNullOrWhiteSpace(request.Rol)))
            return BadRequest(ApiResponse<object>.Error("Debe especificar al menos un usuario o un rol", 400));

        List<Usuario> usuarios;

        // 🚀 Optimización Monolítica: Reemplazamos bucles repetitivos por consultas masivas agrupadas directo a SQL side
        if (request.UsuarioIds != null && request.UsuarioIds.Any())
        {
            usuarios = await _context.Set<Usuario>()
                .Include(x => x.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .Where(u => request.UsuarioIds.Contains(u.Id))
                .ToListAsync();
        }
        else
        {
            usuarios = await _context.Set<Usuario>()
                .Include(x => x.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .Where(u => u.UsuarioRoles.Any(ur => ur.Activo && ur.Rol.Nombre == request.Rol))
                .ToListAsync();
        }

        if (!usuarios.Any())
            return BadRequest(ApiResponse<object>.Error("No se encontraron usuarios para los criterios especificados", 400));

        var pdfs = new List<byte[]>();
        var codigos = new List<string>();
        
        foreach (var usuario in usuarios)
        {
            var dto = await ConstruirDtoDesdeUsuario(usuario.Id);
            if (dto != null)
            {
                pdfs.Add(_pdfService.GeneratePdf(dto));
                codigos.Add(dto.CodigoUsuario);
            }
        }

        if (!pdfs.Any())
            return BadRequest(ApiResponse<object>.Error("No se pudo generar ningún certificado", 400));

        var zipBytes = CrearZipConCertificados(pdfs, codigos);
        return File(zipBytes, "application/zip", $"Certificados_{DateTime.Now:yyyyMMddHHmm}.zip");
    }

    // =====================================================================
    // Helpers privados refactorizados a EF Core directo (Sin Repositorios)
    // =====================================================================
    private async Task<CertificadoTrabajoDto?> ConstruirDtoDesdeUsuario(Guid usuarioId)
    {
        // Consulta directa al DbSet con carga de relaciones requeridas
        var usuario = await _context.Set<Usuario>()
            .Include(x => x.UsuarioRoles)
                .ThenInclude(ur => ur.Rol)
            .FirstOrDefaultAsync(x => x.Id == usuarioId);

        if (usuario == null) return null;

        var roles = usuario.UsuarioRoles
            .Where(ur => ur.Activo)
            .Select(ur => ur.Rol.Nombre)
            .ToList();

        // Búsqueda directa del perfil médico sin recuperar colecciones completas a memoria
        var doctor = await _context.Set<Doctor>()
            .FirstOrDefaultAsync(d => d.UsuarioId == usuarioId);

        string area = roles.FirstOrDefault() ?? "General";
        string cargo = "";
        
        if (doctor != null)
        {
            area = doctor.Especialidad ?? area;
            cargo = $"Médico {doctor.Especialidad}";
        }
        else
        {
            cargo = roles.FirstOrDefault() ?? "Personal de Clínica";
        }

        var fechaInicio = usuario.FechaRegistro;
        var fechaFin = DateTime.UtcNow;
        string dni = "Pendiente"; // Lógica de DNI del modelo original mantenida
        string codigoCertificado = $"CERT-{DateTime.Now:yyyy}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        string observaciones = "Certificado emitido por el sistema de gestión clínica.";

        return new CertificadoTrabajoDto
        {
            NombreCompleto = $"{usuario.Nombres} {usuario.Apellidos}",
            Dni = dni,
            CodigoUsuario = usuario.CodigoUsuario,
            Correo = usuario.Correo,
            Roles = roles,
            Area = area,
            Cargo = cargo,
            FechaInicio = fechaInicio,
            FechaFin = fechaFin,
            NombreDirector = "Dr. Juan Pérez", 
            CargoDirector = "Director Médico",
            CodigoCertificado = codigoCertificado,
            Observaciones = observaciones
        };
    }

    private byte[] CrearZipConCertificados(List<byte[]> pdfs, List<string> codigos)
    {
        using var ms = new MemoryStream();
        using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
        {
            for (int i = 0; i < pdfs.Count; i++)
            {
                var entry = zip.CreateEntry($"Certificado_{codigos[i]}_{DateTime.Now:yyyyMMddHHmm}.pdf");
                using var entryStream = entry.Open();
                entryStream.Write(pdfs[i], 0, pdfs[i].Length);
            }
        }
        return ms.ToArray();
    }
}

// DTO para la solicitud en bloque (Mantenido)
public class CertificadoBlockRequest
{
    public List<Guid>? UsuarioIds { get; set; }
    public string? Rol { get; set; } 
}