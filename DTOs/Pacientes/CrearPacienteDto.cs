using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using psicomedixMonolito.Enums;

namespace psicomedixMonolito.DTOs.Pacientes;

public class CrearPacienteDto
{
    [Required(ErrorMessage = "El número de documento (DNI/Pasaporte) es obligatorio.")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "El documento debe tener entre 8 y 20 caracteres.")]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "El documento solo puede contener caracteres alfanuméricos.")]
    public string? DNI { get; set; }

    [Required(ErrorMessage = "La fecha de atención inicial es obligatoria.")]
    public DateTime? FechaAtencion { get; set; }

    // Sección I: Informante
    public bool? TieneInformante { get; set; }

    [StringLength(150, ErrorMessage = "El nombre del informante no debe superar los 150 caracteres.")]
    public string? InformanteNombre { get; set; }

    [StringLength(50, ErrorMessage = "El parentesco del informante no debe superar los 50 caracteres.")]
    public string? InformanteParentesco { get; set; }

    [StringLength(100, ErrorMessage = "La ocupación del informante no debe superar los 100 caracteres.")]
    public string? InformanteOcupacion { get; set; }

    public EstadoCivil? InformanteEstadoCivil { get; set; }

    [Range(0, 120, ErrorMessage = "La edad del informante debe ser un rango válido entre 0 y 120 años.")]
    public int? InformanteEdad { get; set; }

    [StringLength(20, ErrorMessage = "El celular del informante no debe superar los 20 dígitos.")]
    [Phone(ErrorMessage = "El formato del celular del informante no es válido.")]
    public string? InformanteCelular { get; set; }

    // Sección II: Evaluado
    public bool? PrimeraAtencionPsicologia { get; set; }

    [Required(ErrorMessage = "Los nombres del evaluado son obligatorios.")]
    [StringLength(100, ErrorMessage = "Los nombres no deben superar los 100 caracteres.")]
    public string? Nombres { get; set; }

    [Required(ErrorMessage = "Los apellidos del evaluado son obligatorios.")]
    [StringLength(100, ErrorMessage = "Los apellidos no deben superar los 100 caracteres.")]
    public string? Apellidos { get; set; }

    [Required(ErrorMessage = "El género del evaluado es obligatorio.")]
    public GeneroEvaluado? Genero { get; set; }

    [Required(ErrorMessage = "La edad en años es obligatoria.")]
    [Range(0, 120, ErrorMessage = "La edad debe estar entre 0 y 120 años.")]
    public int? EdadAnos { get; set; }

    [Range(0, 11, ErrorMessage = "Los meses de edad deben estar entre 0 y 11.")]
    public int? EdadMeses { get; set; }

    [Required(ErrorMessage = "El número celular de contacto es obligatorio.")]
    [StringLength(20, ErrorMessage = "El celular no debe superar los 20 dígitos.")]
    [Phone(ErrorMessage = "El formato del número celular no es válido.")]
    public string? Celular { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    public DateTime? FechaNacimiento { get; set; }

    public EstadoCivil? EstadoCivil { get; set; }

    [StringLength(150, ErrorMessage = "El lugar de nacimiento no debe superar los 150 caracteres.")]
    public string? LugarNacimiento { get; set; }

    [StringLength(150, ErrorMessage = "La ocupación no debe superar los 150 caracteres.")]
    public string? Ocupacion { get; set; }

    public GradoInstruccion? GradoInstruccion { get; set; }

    [StringLength(50, ErrorMessage = "El grado, semestre o ciclo no debe superar los 50 caracteres.")]
    public string? GradoSemestreCiclo { get; set; }

    [StringLength(150, ErrorMessage = "La carrera profesional no debe superar los 150 caracteres.")]
    public string? Carrera { get; set; }

    // Sección III: Entorno Fijo
    public List<FamiliarDto> Familiares { get; set; } = new();
    public bool? TieneMasHermanosHijos { get; set; }
    public bool? TieneMediosHermanosPadrastros { get; set; }

    [Range(1, 30, ErrorMessage = "El número de integrantes familiares debe estar entre 1 y 30.")]
    public int? IntegrantesFamilia { get; set; }

    [StringLength(100, ErrorMessage = "La religión no debe superar los 100 caracteres.")]
    public string? Religion { get; set; }

    [Range(0.1, 500.0, ErrorMessage = "El peso en Kg debe ser un valor físico real.")]
    public decimal? PesoKg { get; set; }

    [Range(0.1, 3.0, ErrorMessage = "La talla en metros debe ser un valor físico real (Ej: 1.65).")]
    public decimal? TallaMetros { get; set; }

    [Range(5.0, 100.0, ErrorMessage = "El IMC calculado se encuentra fuera de los rangos clínicos tolerados.")]
    public decimal? IMC { get; set; }

    // Sección IV: Asignación
    [Required(ErrorMessage = "Debe asignar un psicólogo/doctor responsable a este expediente.")]
    public Guid? DoctorId { get; set; }

    public List<string> OtrasEspecialidadesRequeridas { get; set; } = new();
    public bool? ConsultaAsuntosLegales { get; set; }

    [Required(ErrorMessage = "El motivo de consulta inicial es obligatorio para abrir la ficha.")]
    [StringLength(3000, ErrorMessage = "El motivo de consulta no debe exceder los 3000 caracteres.")]
    public string? MotivoConsulta { get; set; }
}