using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Comunes;
using psicomedixMonolito.DTOs.Finanzas;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.Services.imp;

public class FinanzasService : IFinanzasService
{
    private readonly ApplicationDbContext _context;
    
    public FinanzasService(ApplicationDbContext context)
    {
        _context = context;
    }

    // ==========================================================
    // MÉTODOS DE PAGINACIÓN INTERNA
    // ==========================================================
    private static PaginacionResponseDto<T> Paginar<T>(IEnumerable<T> source, PaginacionRequestDto request)
    {
        var total = source.Count();
        var datos = source
            .Skip((request.Pagina - 1) * request.CantidadPorPagina)
            .Take(request.CantidadPorPagina)
            .ToList();

        return new PaginacionResponseDto<T>
        {
            Pagina = request.Pagina,
            CantidadPorPagina = request.CantidadPorPagina,
            TotalRegistros = total,
            Datos = datos
        };
    }

    public async Task<PaginacionResponseDto<PagoFinanzasDto>> ObtenerPagosPendientesPaginadosAsync(PaginacionRequestDto request)
    {
        var pagos = await ObtenerPagosValidosAsync();
        var pendientes = pagos
            .Where(x => x.Estado == EstadoPago.Pendiente || x.SaldoPendiente > 0)
            .OrderByDescending(x => x.FechaPago)
            .Select(MapearPagoFinanzas);

        return Paginar(pendientes, request);
    }
    
    public async Task<EstadoPagoAtencionDto> ObtenerEstadoPagoAtencionDetalladoAsync(Guid atencionId)
    {
        if (atencionId == Guid.Empty)
            throw new InvalidOperationException("El identificador de la atención es obligatorio.");

        var pagos = await ObtenerPagosValidosAsync();
        var pagosAtencion = pagos
            .Where(x => x.AtencionId == atencionId)
            .ToList();

        if (!pagosAtencion.Any())
            throw new KeyNotFoundException("No se encontraron pagos asociados a la atención.");

        var estado = pagosAtencion
            .GroupBy(x => x.AtencionId!.Value)
            .Select(MapearEstadoPagoAtencion)
            .First();

        var ultimoPago = pagosAtencion.OrderByDescending(x => x.FechaPago).First();
        estado.TipoUltimoPago = ultimoPago.SaldoPendiente == 0 ? "Completo" : "Parcial";

        return estado;
    }
    
    // ==========================================================
    // AJUSTES FINANCIEROS / JUSTIFICACIONES
    // ==========================================================

    // 🚩 Ahora recibe usuarioId desde el controlador
    public async Task<Guid> RegistrarAjusteFinancieroAsync(RegistrarAjusteFinancieroDto dto, Guid usuarioId)
    {
        if (dto.PagoId == Guid.Empty)
            throw new InvalidOperationException("El identificador del pago es obligatorio.");

        if (dto.MontoAjuste <= 0)
            throw new InvalidOperationException("El monto del ajuste debe ser mayor a 0.");

        if (string.IsNullOrWhiteSpace(dto.Motivo))
            throw new InvalidOperationException("El motivo del ajuste financiero es obligatorio.");

        var pago = await _context.Set<Pago>().FindAsync(dto.PagoId)
                   ?? throw new KeyNotFoundException("Pago no encontrado.");

        var motivoNormalizado = dto.Motivo.Trim().ToUpper();
        var existeDuplicado = await _context.Set<AjusteFinanciero>()
            .AnyAsync(x =>
                x.PagoId == pago.Id &&
                x.TipoAjuste == dto.TipoAjuste &&
                x.MontoAjuste == dto.MontoAjuste &&
                x.Motivo.Trim().ToUpper() == motivoNormalizado);

        if (existeDuplicado)
            throw new InvalidOperationException("Ya existe un ajuste financiero similar registrado para este pago.");

        var ajuste = new AjusteFinanciero
        {
            Id = Guid.NewGuid(),
            PagoId = pago.Id,
            AtencionId = pago.AtencionId,
            PacienteId = pago.PacienteId,
            TipoAjuste = dto.TipoAjuste,
            MontoAjuste = dto.MontoAjuste,
            Motivo = dto.Motivo.Trim(),
            Observacion = dto.Observacion?.Trim(),
            UsuarioRegistroId = usuarioId,
            FechaRegistro = DateTime.UtcNow
        };

        await _context.Set<AjusteFinanciero>().AddAsync(ajuste);
        await _context.SaveChangesAsync();

        return ajuste.Id;
    }
    
    public async Task<IEnumerable<PagoFinanzasDto>> ObtenerLibroDiarioAsync(DateOnly fecha)
    {
        var pagos = await ObtenerPagosValidosAsync();

        return pagos
            .Where(x => DateOnly.FromDateTime(x.FechaPago) == fecha)
            .OrderByDescending(x => x.FechaPago)
            .Select(MapearPagoFinanzas)
            .ToList();
    }

    public async Task<ResumenFinancieroMensualCompletoDto> ObtenerResumenFinancieroMensualCompletoAsync(int anio, int mes)
    {
        ValidarAnioMes(anio, mes);

        var pagos = await ObtenerPagosValidosAsync();

        var pagosDelMes = pagos
            .Where(x => x.FechaPago.Year == anio && x.FechaPago.Month == mes)
            .ToList();

        var estadosPorAtencion = pagosDelMes
            .Where(x => x.AtencionId.HasValue)
            .GroupBy(x => x.AtencionId!.Value)
            .Select(MapearEstadoPagoAtencion)
            .ToList();

        // Consulta directa de ajustes sustituyendo el antiguo repositorio
        var ajustes = await _context.Set<AjusteFinanciero>()
            .Include(x => x.Pago)
            .Include(x => x.Paciente)
            .Include(x => x.Atencion)
            .Include(x => x.UsuarioRegistro)
            .OrderByDescending(x => x.FechaRegistro)
            .AsNoTracking()
            .ToListAsync();

        var ajustesDelMes = ajustes
            .Where(x => x.FechaRegistro.Year == anio && x.FechaRegistro.Month == mes)
            .OrderByDescending(x => x.FechaRegistro)
            .Select(MapearAjusteFinanciero)
            .ToList();

        return new ResumenFinancieroMensualCompletoDto
        {
            Anio = anio,
            Mes = mes,
            ResumenCaja = new ResumenCajaDto
            {
                TotalIngresos = pagosDelMes.Sum(x => x.MontoPagado),
                CantidadMovimientos = pagosDelMes.Count,
                TotalEfectivo = pagosDelMes.Where(x => x.MetodoPago == MetodoPago.Efectivo).Sum(x => x.MontoPagado),
                TotalYape = pagosDelMes.Where(x => x.MetodoPago == MetodoPago.Yape).Sum(x => x.MontoPagado),
                TotalPlin = pagosDelMes.Where(x => x.MetodoPago == MetodoPago.Plin).Sum(x => x.MontoPagado),
                TotalTransferencia = pagosDelMes.Where(x => x.MetodoPago == MetodoPago.Transferencia).Sum(x => x.MontoPagado),
                TotalTarjeta = pagosDelMes.Where(x => x.MetodoPago == MetodoPago.Tarjeta).Sum(x => x.MontoPagado),
                TotalOtro = pagosDelMes.Where(x => x.MetodoPago == MetodoPago.Otro).Sum(x => x.MontoPagado),
                MetodosPago = pagosDelMes
                    .GroupBy(x => x.MetodoPago)
                    .Select(g => new ResumenMetodoPagoDto
                    {
                        MetodoPago = g.Key.ToString(),
                        Total = g.Sum(x => x.MontoPagado),
                        CantidadMovimientos = g.Count()
                    })
                    .OrderByDescending(x => x.Total)
                    .ToList(),
                Movimientos = pagosDelMes.OrderByDescending(x => x.FechaPago).Select(MapearPagoFinanzas).ToList()
            },
            ResumenRealAtenciones = new ResumenRealAtencionesDto
            {
                TotalFacturadoReal = estadosPorAtencion.Sum(x => x.MontoTotal),
                TotalPagadoReal = estadosPorAtencion.Sum(x => x.TotalPagado),
                TotalDeudaReal = estadosPorAtencion.Sum(x => x.SaldoReal),
                TotalSobrepagos = estadosPorAtencion.Sum(x => x.Sobrepago),
                AtencionesPagadas = estadosPorAtencion.Count(x => x.EstadoFinanciero == "Pagado"),
                AtencionesParciales = estadosPorAtencion.Count(x => x.EstadoFinanciero == "Parcial"),
                AtencionesPendientes = estadosPorAtencion.Count(x => x.EstadoFinanciero == "Pendiente"),
                AtencionesSobrepagadas = estadosPorAtencion.Count(x => x.EstadoFinanciero == "Sobrepagado"),
                EstadosAtenciones = estadosPorAtencion
            },
            AjustesFinancieros = ajustesDelMes
        };
    }

    public async Task<IEnumerable<AjusteFinancieroDto>> ObtenerAjustesFinancierosAsync()
    {
        var ajustes = await _context.Set<AjusteFinanciero>()
            .Include(x => x.Pago)
            .Include(x => x.Paciente)
            .Include(x => x.Atencion)
            .Include(x => x.UsuarioRegistro)
            .OrderByDescending(x => x.FechaRegistro)
            .AsNoTracking()
            .ToListAsync();

        return ajustes.Select(MapearAjusteFinanciero).ToList();
    }

    public async Task<IEnumerable<AjusteFinancieroDto>> ObtenerAjustesPorAtencionAsync(Guid atencionId)
    {
        if (atencionId == Guid.Empty)
            throw new InvalidOperationException("El identificador de la atención es obligatorio.");

        var ajustes = await _context.Set<AjusteFinanciero>()
            .Include(x => x.Pago)
            .Include(x => x.Paciente)
            .Include(x => x.Atencion)
            .Include(x => x.UsuarioRegistro)
            .Where(x => x.AtencionId == atencionId)
            .OrderByDescending(x => x.FechaRegistro)
            .AsNoTracking()
            .ToListAsync();

        return ajustes.Select(MapearAjusteFinanciero).ToList();
    }

    public async Task<IEnumerable<AjusteFinancieroDto>> ObtenerAjustesPorPagoAsync(Guid pagoId)
    {
        if (pagoId == Guid.Empty)
            throw new InvalidOperationException("El identificador del pago es obligatorio.");

        var ajustes = await _context.Set<AjusteFinanciero>()
            .Include(x => x.Pago)
            .Include(x => x.Paciente)
            .Include(x => x.Atencion)
            .Include(x => x.UsuarioRegistro)
            .Where(x => x.PagoId == pagoId)
            .OrderByDescending(x => x.FechaRegistro)
            .AsNoTracking()
            .ToListAsync();

        return ajustes.Select(MapearAjusteFinanciero).ToList();
    }

    // ==========================================================
    // DEUDAS REALES AGRUPADAS POR ATENCIÓN
    // ==========================================================

    public async Task<IEnumerable<EstadoPagoAtencionDto>> ObtenerDeudasRealesAsync()
    {
        var pagos = await ObtenerPagosValidosAsync();

        return pagos
            .Where(x => x.AtencionId.HasValue)
            .GroupBy(x => x.AtencionId!.Value)
            .Select(MapearEstadoPagoAtencion)
            .Where(x => x.TieneDeuda)
            .OrderByDescending(x => x.SaldoReal)
            .ThenBy(x => x.Paciente)
            .ToList();
    }

    public async Task<IEnumerable<EstadoPagoAtencionDto>> ObtenerDeudasRealesPacienteAsync(Guid pacienteId)
    {
        if (pacienteId == Guid.Empty)
            throw new InvalidOperationException("El identificador del paciente es obligatorio.");

        var paciente = await _context.Set<Paciente>().FindAsync(pacienteId)
            ?? throw new KeyNotFoundException("Paciente no encontrado.");

        var pagos = await ObtenerPagosValidosAsync();

        return pagos
            .Where(x => x.PacienteId == paciente.Id && x.AtencionId.HasValue)
            .GroupBy(x => x.AtencionId!.Value)
            .Select(MapearEstadoPagoAtencion)
            .Where(x => x.TieneDeuda)
            .OrderByDescending(x => x.SaldoReal)
            .ToList();
    }

    public async Task<EstadoPagoAtencionDto> ObtenerEstadoPagoAtencionAsync(Guid atencionId)
    {
        if (atencionId == Guid.Empty)
            throw new InvalidOperationException("El identificador de la atención es obligatorio.");

        var pagos = await ObtenerPagosValidosAsync();

        var pagosAtencion = pagos
            .Where(x => x.AtencionId == atencionId)
            .ToList();

        if (!pagosAtencion.Any())
            throw new KeyNotFoundException("No se encontraron pagos asociados a la atención.");

        return pagosAtencion
            .GroupBy(x => x.AtencionId!.Value)
            .Select(MapearEstadoPagoAtencion)
            .First();
    }

    // ==========================================================
    // RESÚMENES DE CAJA / MOVIMIENTOS INDIVIDUALES
    // ==========================================================

    public async Task<ResumenDiarioFinanzasDto> ObtenerResumenDiarioAsync(DateOnly fecha)
    {
        var pagos = await ObtenerPagosValidosAsync();

        var pagosDelDia = pagos
            .Where(x => DateOnly.FromDateTime(x.FechaPago) == fecha)
            .ToList();

        return new ResumenDiarioFinanzasDto
        {
            Fecha = fecha,
            TotalIngresos = pagosDelDia.Sum(x => x.MontoPagado),
            TotalPendiente = pagosDelDia.Sum(x => x.SaldoPendiente),
            TotalDeuda = pagosDelDia.Where(x => x.SaldoPendiente > 0).Sum(x => x.SaldoPendiente),
            CantidadPagos = pagosDelDia.Count,
            PagosCompletados = pagosDelDia.Count(x => x.Estado == EstadoPago.Pagado),
            PagosParciales = pagosDelDia.Count(x => x.Estado == EstadoPago.Parcial),
            PagosPendientes = pagosDelDia.Count(x => x.Estado == EstadoPago.Pendiente || x.SaldoPendiente > 0),
            Pagos = pagosDelDia.OrderByDescending(x => x.FechaPago).Select(MapearPagoFinanzas).ToList()
        };
    }

    public async Task<ResumenMensualFinanzasDto> ObtenerResumenMensualAsync(int anio, int mes)
    {
        ValidarAnioMes(anio, mes);

        var pagos = await ObtenerPagosValidosAsync();

        var pagosDelMes = pagos
            .Where(x => x.FechaPago.Year == anio && x.FechaPago.Month == mes)
            .ToList();

        var dias = pagosDelMes
            .GroupBy(x => DateOnly.FromDateTime(x.FechaPago))
            .OrderBy(x => x.Key)
            .Select(g => new ResumenDiarioFinanzasDto
            {
                Fecha = g.Key,
                TotalIngresos = g.Sum(x => x.MontoPagado),
                TotalPendiente = g.Sum(x => x.SaldoPendiente),
                TotalDeuda = g.Where(x => x.SaldoPendiente > 0).Sum(x => x.SaldoPendiente),
                CantidadPagos = g.Count(),
                PagosCompletados = g.Count(x => x.Estado == EstadoPago.Pagado),
                PagosParciales = g.Count(x => x.Estado == EstadoPago.Parcial),
                PagosPendientes = g.Count(x => x.Estado == EstadoPago.Pendiente || x.SaldoPendiente > 0),
                Pagos = g.OrderByDescending(x => x.FechaPago).Select(MapearPagoFinanzas).ToList()
            })
            .ToList();

        return new ResumenMensualFinanzasDto
        {
            Anio = anio,
            Mes = mes,
            TotalIngresos = pagosDelMes.Sum(x => x.MontoPagado),
            TotalPendiente = pagosDelMes.Sum(x => x.SaldoPendiente),
            TotalDeuda = pagosDelMes.Where(x => x.SaldoPendiente > 0).Sum(x => x.SaldoPendiente),
            CantidadPagos = pagosDelMes.Count,
            PagosCompletados = pagosDelMes.Count(x => x.Estado == EstadoPago.Pagado),
            PagosParciales = pagosDelMes.Count(x => x.Estado == EstadoPago.Parcial),
            PagosPendientes = pagosDelMes.Count(x => x.Estado == EstadoPago.Pendiente || x.SaldoPendiente > 0),
            Dias = dias
        };
    }

    public async Task<ResumenAnualFinanzasDto> ObtenerResumenAnualAsync(int anio)
    {
        if (anio < 2000 || anio > DateTime.UtcNow.Year + 1)
            throw new InvalidOperationException("El año ingresado no es válido.");

        var pagos = await ObtenerPagosValidosAsync();

        var pagosDelAnio = pagos
            .Where(x => x.FechaPago.Year == anio)
            .ToList();

        var meses = Enumerable.Range(1, 12)
            .Select(mes =>
            {
                var pagosMes = pagosDelAnio.Where(x => x.FechaPago.Month == mes).ToList();

                return new ResumenMensualFinanzasDto
                {
                    Anio = anio,
                    Mes = mes,
                    TotalIngresos = pagosMes.Sum(x => x.MontoPagado),
                    TotalPendiente = pagosMes.Sum(x => x.SaldoPendiente),
                    TotalDeuda = pagosMes.Where(x => x.SaldoPendiente > 0).Sum(x => x.SaldoPendiente),
                    CantidadPagos = pagosMes.Count,
                    PagosCompletados = pagosMes.Count(x => x.Estado == EstadoPago.Pagado),
                    PagosParciales = pagosMes.Count(x => x.Estado == EstadoPago.Parcial),
                    PagosPendientes = pagosMes.Count(x => x.Estado == EstadoPago.Pendiente || x.SaldoPendiente > 0)
                };
            })
            .ToList();

        return new ResumenAnualFinanzasDto
        {
            Anio = anio,
            TotalIngresos = pagosDelAnio.Sum(x => x.MontoPagado),
            TotalPendiente = pagosDelAnio.Sum(x => x.SaldoPendiente),
            TotalDeuda = pagosDelAnio.Where(x => x.SaldoPendiente > 0).Sum(x => x.SaldoPendiente),
            CantidadPagos = pagosDelAnio.Count,
            PagosCompletados = pagosDelAnio.Count(x => x.Estado == EstadoPago.Pagado),
            PagosParciales = pagosDelAnio.Count(x => x.Estado == EstadoPago.Parcial),
            PagosPendientes = pagosDelAnio.Count(x => x.Estado == EstadoPago.Pendiente || x.SaldoPendiente > 0),
            Meses = meses
        };
    }

    public async Task<IEnumerable<PagoFinanzasDto>> ObtenerPagosPendientesAsync()
    {
        var pagos = await ObtenerPagosValidosAsync();

        return pagos
            .Where(x => x.Estado == EstadoPago.Pendiente || x.SaldoPendiente > 0)
            .OrderByDescending(x => x.FechaPago)
            .Select(MapearPagoFinanzas)
            .ToList();
    }

    public async Task<IEnumerable<PagoFinanzasDto>> ObtenerPagosPagadosAsync()
    {
        var pagos = await ObtenerPagosValidosAsync();

        return pagos
            .Where(x => x.Estado == EstadoPago.Pagado && x.SaldoPendiente <= 0)
            .OrderByDescending(x => x.FechaPago)
            .Select(MapearPagoFinanzas)
            .ToList();
    }

    public async Task<IEnumerable<PagoFinanzasDto>> ObtenerPagosParcialesAsync()
    {
        var pagos = await ObtenerPagosValidosAsync();

        return pagos
            .Where(x => x.Estado == EstadoPago.Parcial)
            .OrderByDescending(x => x.FechaPago)
            .Select(MapearPagoFinanzas)
            .ToList();
    }

    public async Task<PagoFinanzasDto?> ObtenerPagoPorCodigoAsync(string codigoPago)
    {
        if (string.IsNullOrWhiteSpace(codigoPago))
            throw new InvalidOperationException("El código de pago es obligatorio.");

        // Consulta directa de pagos con inclusiones requeridas para el mapeo
        var pago = await _context.Set<Pago>()
            .Include(x => x.Paciente)
            .Include(x => x.ServicioClinico)
            .Include(x => x.UsuarioRegistro)
            .FirstOrDefaultAsync(x => x.CodigoPago == codigoPago.Trim());

        return pago == null ? null : MapearPagoFinanzas(pago);
    }

    // ==========================================================
    // ESTADO DE CUENTA DEL PACIENTE
    // ==========================================================

    public async Task<EstadoCuentaPacienteDto> ObtenerEstadoCuentaPacienteAsync(Guid pacienteId)
    {
        if (pacienteId == Guid.Empty)
            throw new InvalidOperationException("El identificador del paciente es obligatorio.");

        var paciente = await _context.Set<Paciente>().FindAsync(pacienteId)
            ?? throw new KeyNotFoundException("Paciente no encontrado.");

        var pagos = await _context.Set<Pago>()
            .Include(x => x.Paciente)
            .Include(x => x.ServicioClinico)
            .Include(x => x.UsuarioRegistro)
            .Where(x => x.PacienteId == pacienteId)
            .ToListAsync();

        var pagosValidos = pagos
            .Where(EsPagoValidoParaFinanzas)
            .OrderByDescending(x => x.FechaPago)
            .ToList();

        var estadosPorAtencion = pagosValidos
            .Where(x => x.AtencionId.HasValue)
            .GroupBy(x => x.AtencionId!.Value)
            .Select(MapearEstadoPagoAtencion)
            .ToList();

        return new EstadoCuentaPacienteDto
        {
            PacienteId = paciente.Id,
            Paciente = $"{paciente.Nombres} {paciente.Apellidos}",
            DniPaciente = paciente.DNI,
            TotalFacturado = estadosPorAtencion.Sum(x => x.MontoTotal),
            TotalPagado = estadosPorAtencion.Sum(x => x.TotalPagado),
            TotalPendiente = estadosPorAtencion.Sum(x => x.SaldoReal),
            CantidadPagos = pagosValidos.Count,
            PagosCompletados = estadosPorAtencion.Count(x => x.EstadoFinanciero == "Pagado"),
            PagosParciales = estadosPorAtencion.Count(x => x.EstadoFinanciero == "Parcial"),
            PagosPendientes = estadosPorAtencion.Count(x => x.EstadoFinanciero == "Pendiente" || x.TieneDeuda),
            Detalles = pagosValidos.Select(x => new DetalleEstadoCuentaDto
            {
                PagoId = x.Id,
                CodigoPago = x.CodigoPago,
                AtencionId = x.AtencionId,
                Servicio = x.ServicioClinico?.Nombre ?? "",
                MontoTotal = x.MontoTotal,
                MontoPagado = x.MontoPagado,
                SaldoPendiente = x.SaldoPendiente,
                EstadoPago = x.Estado.ToString(),
                FechaPago = x.FechaPago
            }).ToList()
        };
    }

    // ==========================================================
    // METODOS PRIVADOS Y ADAPTACIONES DE CONSULTA
    // ==========================================================

    private async Task<List<Pago>> ObtenerPagosValidosAsync()
    {
        var pagos = await _context.Set<Pago>()
            .Include(x => x.Paciente)
            .Include(x => x.ServicioClinico)
            .Include(x => x.UsuarioRegistro)
            .AsNoTracking()
            .ToListAsync();

        return pagos
            .Where(EsPagoValidoParaFinanzas)
            .ToList();
    }

    private static bool EsPagoValidoParaFinanzas(Pago pago)
    {
        return pago.Estado != EstadoPago.Anulado
               && pago.Estado != EstadoPago.Reembolsado
               && pago.Estado != EstadoPago.Eliminado;
    }

    private static PagoFinanzasDto MapearPagoFinanzas(Pago x)
    {
        return new PagoFinanzasDto
        {
            PagoId = x.Id,
            CodigoPago = x.CodigoPago,
            PacienteId = x.PacienteId,
            Paciente = x.Paciente == null ? "" : $"{x.Paciente.Nombres} {x.Paciente.Apellidos}",
            DniPaciente = x.Paciente?.DNI ?? "",
            AtencionId = x.AtencionId,
            Servicio = x.ServicioClinico?.Nombre ?? "",
            MontoTotal = x.MontoTotal,
            MontoPagado = x.MontoPagado,
            SaldoPendiente = x.SaldoPendiente,
            EstadoPago = x.Estado.ToString(),
            MetodoPago = x.MetodoPago.ToString(),
            FechaPago = x.FechaPago,
            RegistradoPor = x.UsuarioRegistro == null
                ? ""
                : $"{x.UsuarioRegistro.Nombres} {x.UsuarioRegistro.Apellidos}"
        };
    }

    private static EstadoPagoAtencionDto MapearEstadoPagoAtencion(IGrouping<Guid, Pago> grupo)
    {
        var pagos = grupo.OrderBy(x => x.FechaPago).ToList();
        var primerPago = pagos.First();
        var montoTotal = pagos.Max(x => x.MontoTotal);
        var totalPagado = pagos.Sum(x => x.MontoPagado);

        var saldoReal = Math.Max(montoTotal - totalPagado, 0);
        var sobrepago = Math.Max(totalPagado - montoTotal, 0);

        string estadoFinanciero = sobrepago > 0 ? "Sobrepagado" 
            : saldoReal == 0 ? "Pagado" 
            : totalPagado > 0 ? "Parcial" 
            : "Pendiente";

        return new EstadoPagoAtencionDto
        {
            AtencionId = grupo.Key,
            PacienteId = primerPago.PacienteId,
            Paciente = primerPago.Paciente == null ? "" : $"{primerPago.Paciente.Nombres} {primerPago.Paciente.Apellidos}",
            DniPaciente = primerPago.Paciente?.DNI ?? "",
            Servicio = primerPago.ServicioClinico?.Nombre ?? "",
            MontoTotal = montoTotal,
            TotalPagado = totalPagado,
            SaldoReal = saldoReal,
            Sobrepago = sobrepago,
            TieneDeuda = saldoReal > 0,
            TieneSobrepago = sobrepago > 0,
            EstadoFinanciero = estadoFinanciero,
            FechaPrimerPago = pagos.Min(x => x.FechaPago),
            FechaUltimoPago = pagos.Max(x => x.FechaPago),
            CantidadPagos = pagos.Count,
            Pagos = pagos.OrderByDescending(x => x.FechaPago).Select(MapearPagoFinanzas).ToList()
        };
    }

    private static AjusteFinancieroDto MapearAjusteFinanciero(AjusteFinanciero x)
    {
        return new AjusteFinancieroDto
        {
            Id = x.Id,
            PagoId = x.PagoId,
            CodigoPago = x.Pago?.CodigoPago ?? "",
            AtencionId = x.AtencionId,
            PacienteId = x.PacienteId,
            Paciente = x.Paciente == null ? "" : $"{x.Paciente.Nombres} {x.Paciente.Apellidos}",
            DniPaciente = x.Paciente?.DNI ?? "",
            TipoAjuste = x.TipoAjuste.ToString(),
            MontoAjuste = x.MontoAjuste,
            Motivo = x.Motivo,
            Observacion = x.Observacion,
            RegistradoPor = x.UsuarioRegistro == null ? "" : $"{x.UsuarioRegistro.Nombres} {x.UsuarioRegistro.Apellidos}",
            FechaRegistro = x.FechaRegistro
        };
    }

    private static void ValidarAnioMes(int anio, int mes)
    {
        if (anio < 2000 || anio > DateTime.UtcNow.Year + 1)
            throw new InvalidOperationException("El año ingresado no es válido.");

        if (mes < 1 || mes > 12)
            throw new InvalidOperationException("El mes ingresado no es válido.");
    }
}