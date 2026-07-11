using psicomedixMonolito.DTOs.Horarios;

namespace psicomedixMonolito.Services;

public interface IHorarioDoctorService
{
    Task<IEnumerable<HorarioDoctorResponseDto>> ObtenerTodosAsync();
    Task<IEnumerable<HorarioDoctorResponseDto>> ObtenerPorDoctorAsync(Guid doctorId);
    Task<Guid> CrearAsync(CrearHorarioDoctorDto dto);
    Task ActualizarAsync(Guid id, EditarHorarioDoctorDto dto);
    Task<MatrizSemanalDto> ObtenerMatrizSemanalAsync(Guid doctorId, DateOnly fechaReferencia);
}