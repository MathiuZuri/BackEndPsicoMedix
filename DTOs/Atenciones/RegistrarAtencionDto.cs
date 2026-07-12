using System;
using System.ComponentModel.DataAnnotations;
using psicomedixMonolito.DTOs.Atenciones.Modulos;

namespace psicomedixMonolito.DTOs.Atenciones;

public class RegistrarAtencionDto
{
    [Required(ErrorMessage = "El identificador del paciente es obligatorio.")]
    public Guid PacienteId { get; set; }

    [Required(ErrorMessage = "El identificador del psicólogo es obligatorio.")]
    public Guid DoctorId { get; set; }

    [Required(ErrorMessage = "El servicio clínico es obligatorio.")]
    public Guid ServicioClinicoId { get; set; }

    public Guid? CitaId { get; set; }

    [Required(ErrorMessage = "El identificador del historial clínico es obligatorio.")]
    public Guid? HistorialClinicoId { get; set; }

    [Required(ErrorMessage = "El costo de la atención es obligatorio.")]
    [Range(0.00, 10000.00, ErrorMessage = "El costo final debe ser un monto válido mayor o igual a 0.")]
    public decimal CostoFinal { get; set; }

    [StringLength(2000, ErrorMessage = "Las observaciones iniciales de la sesión no deben superar los 2000 caracteres.")]
    public string? ObservacionesIniciales { get; set; }

    // Estructuras clínicas opcionales de Documento Único
    public PsicoAnamnesisHistoriaDto? AnamnesisHistoria { get; set; }
    public PsicoSomaticoVegetativoDto? SomaticoVegetativo { get; set; }
    public PsicoEscalasAnimoDto? EscalasAnimo { get; set; }
    public PsicoDesarrolloPsicosocialDto? DesarrolloPsicosocial { get; set; }
    public PsicoEvaluacionCognitivaDto? EvaluacionCognitiva { get; set; }
    public PsicoDiagnosticoCierreDto? DiagnosticoCierre { get; set; }
}