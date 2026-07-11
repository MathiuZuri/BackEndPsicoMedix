using psicomedixMonolito.DTOs.Auth;

namespace psicomedixMonolito.Services;

public interface IAuthService
{
    Task<RespuestaInicioSesionDto> IniciarSesionAsync(IniciarSesionDto dto);
    
    // Pasamos el usuarioId directamente desde el controlador API
    Task CambiarContrasenaAsync(CambiarContrasenaDto dto, Guid usuarioId); 
}