namespace psicomedixMonolito.DTOs.PDFsDto;

public class HistoriaClinicaPdfDto
{
    // ========== 1. Ficha de Identificación ==========
    public string NombresApellidos { get; set; } = "";
    public string Dni { get; set; } = "";
    public DateTime FechaNacimiento { get; set; }
    public string Sexo { get; set; } = "";
    public string LugarNacimiento { get; set; } = "";
    public string Direccion { get; set; } = "";
    public string Correo { get; set; } = "";
    public string Celular { get; set; } = "";
    public string Ocupacion { get; set; } = "";
    public string MotivoConsulta { get; set; } = "";
    public string NumeroHistoria { get; set; } = "";
    public DateTime FechaRegistro { get; set; }

    // ========== 2. Antecedentes Gineco-Obstétricos ==========
    public string Menarquia { get; set; } = "";
    public string RitmoCatamenial { get; set; } = "";
    public int Gesta { get; set; }
    public int Partos { get; set; }
    public int Abortos { get; set; }
    public int HijosVivos { get; set; }
    public int HijosMuertos { get; set; }
    public DateTime? FUR { get; set; }
    public DateTime? FPP { get; set; }
    public string? PI { get; set; }
    public string MetodoAnticonceptivo { get; set; } = "";

    // ========== 3. Funciones Vitales ==========
    public string PA { get; set; } = "";
    public string Pulso { get; set; } = "";
    public string Temperatura { get; set; } = "";
    public string Respiracion { get; set; } = "";
    public string SO2 { get; set; } = "";
    public string Peso { get; set; } = "";
    public string Talla { get; set; } = "";

    // ========== 4. Examen Obstétrico ==========
    public string AlturaUterina { get; set; } = "";
    public string Situacion { get; set; } = "";
    public string Presentacion { get; set; } = "";
    public string LatidosCardiacosFetales { get; set; } = "";
    public string Edemas { get; set; } = "";
    public string Indicaciones { get; set; } = "";

    // ========== 5. Resumen de Atenciones (opcional) ==========
    public List<AtencionResumenDto> Atenciones { get; set; } = new();
}

public class AtencionResumenDto
{
    public DateTime Fecha { get; set; }
    public string Servicio { get; set; } = "";
    public string Doctor { get; set; } = "";
    public string Diagnostico { get; set; } = "";
}