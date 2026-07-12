using System.ComponentModel.DataAnnotations;
using psicomedixMonolito.DTOs.Atenciones.Modulos;

namespace psicomedixMonolito.DTOs.Atenciones;

public class CerrarAtencionDto
{
    // El cierre exige el guardado del bloque diagnóstico conclusivo
    public PsicoDiagnosticoCierreDto DiagnosticoCierre { get; set; } = new();
    public string? ObservacionesFinales { get; set; }
}