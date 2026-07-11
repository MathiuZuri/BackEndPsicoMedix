namespace psicomedixMonolito.DTOs.PDFsDto;

public class ResumenPartoPdfDto
{
    public string PacienteNombre { get; set; } = "";
    public string Dni { get; set; } = "";
    public DateTime FechaParto { get; set; }
    public TimeOnly HoraParto { get; set; }
    public string CondicionParto { get; set; } = "";
    public string AtendidoPor { get; set; } = "";
    public string FormaTerminacion { get; set; } = "";
    public string MedicacionExpulsivo { get; set; } = "";
    public string Episiotomia { get; set; } = "";
    public string Desgarros { get; set; } = "";
    public string Alumbramiento { get; set; } = "";
    public string ModalidadPlacenta { get; set; } = "";
    public string PesoPlacenta { get; set; } = "";
    public string LiquidoAmniotico { get; set; } = "";
    public string ColorLiquido { get; set; } = "";
    public string LongitudCordon { get; set; } = "";
    public string PerdidaSanguinea { get; set; } = "";
    public string ObservacionesMadre { get; set; } = "";
    public string RnVivoMuerto { get; set; } = "";
    public string SexoRN { get; set; } = "";
    public string Apgar1Min { get; set; } = "";
    public string Apgar5Min { get; set; } = "";
    public string PesoRN { get; set; } = "";
    public string TallaRN { get; set; } = "";
    public string PC { get; set; } = "";
    public string PT { get; set; } = "";
    public string ObservacionesRN { get; set; } = "";
    public string DiagnosticoPostParto { get; set; } = "";
    public List<ControlVitalDto> ControlesVitales { get; set; } = new();
    public List<PartogramaRegistroDto> RegistrosPartograma { get; set; } = new();
}

public class ControlVitalDto
{
    public TimeOnly Hora { get; set; }
    public string PA { get; set; } = "";
    public string Pulso { get; set; } = "";
    public string Temperatura { get; set; } = "";
    public string Respiracion { get; set; } = "";
}

public class PartogramaRegistroDto
{
    public int Hora { get; set; }
    public string Dilatacion { get; set; } = "";
    public string AlturaPresentacion { get; set; } = "";
    public string DinamicaUterina { get; set; } = "";
    public string FrecuenciaCardiacaFetal { get; set; } = "";
    public string Oxitocina { get; set; } = "";
    public string Medicamentos { get; set; } = "";
    public string Pulso { get; set; } = "";
    public string Temperatura { get; set; } = "";
    public string Orina { get; set; } = "";
}