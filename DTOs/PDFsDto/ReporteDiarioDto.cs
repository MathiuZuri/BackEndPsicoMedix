// Domain/PDFsDto/ReporteDiarioDto.cs
namespace psicomedixMonolito.DTOs.PDFsDto;

public class ReporteDiarioDto
{
    public DateOnly Fecha { get; set; }
    public decimal TotalIngresos { get; set; }
    public decimal TotalEfectivo { get; set; }
    public decimal TotalYape { get; set; }
    public decimal TotalPlin { get; set; }
    public decimal TotalTransferencia { get; set; }
    public decimal TotalTarjeta { get; set; }
    public decimal TotalOtro { get; set; }
    public int CantidadPagos { get; set; }

    // ✅ NUEVO: Datos del cierre
    public string CierrePor { get; set; } = "";
    public DateTime? FechaCierre { get; set; }

    // ✅ NUEVO: Observaciones del reporte
    public string Observaciones { get; set; } = "";

    public List<MovimientoReporteDto> Movimientos { get; set; } = new();
}

public class MovimientoReporteDto
{
    public string CodigoPago { get; set; } = "";
    public string Paciente { get; set; } = "";
    public string Servicio { get; set; } = "";
    public decimal Monto { get; set; }
    public string MetodoPago { get; set; } = "";
    public DateTime FechaPago { get; set; }
}