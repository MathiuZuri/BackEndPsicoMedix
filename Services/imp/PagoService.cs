using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Pagos;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.Services.imp;

public class PagoService : IPagoService
{
    private readonly ApplicationDbContext _context;

    // ✅ Solo DbContext, igual que UsuarioService
    public PagoService(ApplicationDbContext context)
    {
        _context = context;
    }

    // ==========================================================
    // CONSULTAS DIRECTAS CON EF CORE
    // ==========================================================

    public async Task<IEnumerable<PagoResponseDto>> ObtenerPorPacienteAsync(Guid pacienteId)
    {
        var pagos = await _context.Set<Pago>()
            .AsNoTracking()
            .Include(x => x.Paciente)
            .Include(x => x.ServicioClinico)
            .Include(x => x.Cita)
            .Include(x => x.Atencion)
            .Where(x => x.PacienteId == pacienteId)
            .OrderByDescending(x => x.FechaPago)
            .ToListAsync();

        return pagos.Select(MapearPago);
    }

    public async Task<IEnumerable<PagoResponseDto>> ObtenerPorCitaAsync(Guid citaId)
    {
        var pagos = await _context.Set<Pago>()
            .AsNoTracking()
            .Include(x => x.Paciente)
            .Include(x => x.ServicioClinico)
            .Include(x => x.Cita)
            .Include(x => x.Atencion)
            .Where(x => x.CitaId == citaId)
            .OrderByDescending(x => x.FechaPago)
            .ToListAsync();

        return pagos.Select(MapearPago);
    }

    public async Task<IEnumerable<PagoResponseDto>> ObtenerPorAtencionAsync(Guid atencionId)
    {
        var pagos = await _context.Set<Pago>()
            .AsNoTracking()
            .Include(x => x.Paciente)
            .Include(x => x.ServicioClinico)
            .Include(x => x.Cita)
            .Include(x => x.Atencion)
            .Where(x => x.AtencionId == atencionId)
            .OrderByDescending(x => x.FechaPago)
            .ToListAsync();

        return pagos.Select(MapearPago);
    }

    // ==========================================================
    // OPERACIONES DE ESCRITURA Y CONTROL TRANSACCIONAL
    // ==========================================================

    public async Task CambiarEstadoAsync(Guid id, CambiarEstadoPagoDto dto)
    {
        var pago = await _context.Set<Pago>().FindAsync(id)
            ?? throw new KeyNotFoundException("Pago no encontrado.");

        if (pago.Estado == EstadoPago.Eliminado)
            throw new InvalidOperationException("No se puede modificar un pago eliminado.");

        if (dto.Estado == EstadoPago.Eliminado && pago.SaldoPendiente > 0)
            throw new InvalidOperationException("No se puede eliminar un pago con saldo pendiente. Primero regularice la deuda.");

        pago.Estado = dto.Estado;
        await _context.SaveChangesAsync();
    }

    // 🚩 Ahora recibe el usuarioId desde el controlador
    public async Task<Guid> RegistrarAsync(RegistrarPagoDto dto, Guid usuarioId)
    {
        if (dto.MontoPagado > dto.MontoTotal)
            throw new InvalidOperationException("El monto pagado no puede ser mayor al monto total.");

        if (dto.MontoAdelanto > dto.MontoTotal)
            throw new InvalidOperationException("El monto de adelanto no puede ser mayor al monto total.");

        var paciente = await _context.Set<Paciente>().FindAsync(dto.PacienteId)
            ?? throw new KeyNotFoundException("Paciente no encontrado.");

        var servicio = await _context.Set<ServicioClinico>().FindAsync(dto.ServicioClinicoId)
            ?? throw new KeyNotFoundException("Servicio no encontrado.");

        var saldo = dto.MontoTotal - dto.MontoPagado;

        var pago = new Pago
        {
            Id = Guid.NewGuid(),
            CodigoPago = GenerarCodigo("PAG", paciente.DNI),
            PacienteId = dto.PacienteId,
            ServicioClinicoId = dto.ServicioClinicoId,
            CitaId = dto.CitaId,
            AtencionId = dto.AtencionId,
            MontoTotal = dto.MontoTotal,
            MontoPagado = dto.MontoPagado,
            SaldoPendiente = saldo,
            MontoAdelanto = dto.MontoAdelanto,
            MetodoPago = dto.MetodoPago,
            Estado = saldo == 0 ? EstadoPago.Pagado : (dto.MontoPagado > 0 ? EstadoPago.Parcial : EstadoPago.Pendiente),
            Observacion = dto.Observacion,
            FechaPago = DateTime.UtcNow,
            UsuarioRegistroId = usuarioId
        };

        await _context.Set<Pago>().AddAsync(pago);

        // Registro en historial clínico
        var historial = await _context.Set<HistorialClinico>()
            .FirstOrDefaultAsync(x => x.PacienteId == dto.PacienteId);

        if (historial != null)
        {
            var detalle = new HistorialDetalle
            {
                Id = Guid.NewGuid(),
                CodigoDetalle = GenerarCodigo(servicio.CodigoServicio, paciente.DNI),
                HistorialClinicoId = historial.Id,
                PagoId = pago.Id,
                TipoMovimiento = TipoMovimientoHistorial.PagoRegistrado, // Asegúrate de que el enum exista
                Titulo = "Pago registrado",
                Descripcion = $"Se registró pago de S/ {dto.MontoPagado} por {servicio.Nombre}. Método: {dto.MetodoPago}.",
                FechaRegistro = DateTime.UtcNow,
                UsuarioId = usuarioId
            };
            await _context.Set<HistorialDetalle>().AddAsync(detalle);
        }

        await _context.SaveChangesAsync();
        return pago.Id;
    }

    // ==========================================================
    // HELPERS PRIVADOS
    // ==========================================================

    private static PagoResponseDto MapearPago(Pago x) => new()
    {
        Id = x.Id,
        CodigoPago = x.CodigoPago,
        PacienteId = x.PacienteId,
        PacienteNombre = x.Paciente == null ? "" : $"{x.Paciente.Nombres} {x.Paciente.Apellidos}",
        ServicioClinicoId = x.ServicioClinicoId,
        ServicioNombre = x.ServicioClinico?.Nombre ?? "",
        CitaId = x.CitaId,
        AtencionId = x.AtencionId,
        MontoTotal = x.MontoTotal,
        MontoPagado = x.MontoPagado,
        SaldoPendiente = x.SaldoPendiente,
        MontoAdelanto = x.MontoAdelanto,
        MetodoPago = x.MetodoPago,
        Estado = x.Estado,
        Observacion = x.Observacion,
        FechaPago = x.FechaPago
    };

    private static string GenerarCodigo(string prefijo, string dni) =>
        $"{Guid.NewGuid().ToString("N")[..5].ToUpper()}-{prefijo}-{DateTime.UtcNow:yyyy}-{dni}";
}