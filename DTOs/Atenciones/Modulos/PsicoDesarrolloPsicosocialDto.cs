using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.DTOs.Atenciones.Modulos;

public class PsicoDesarrolloPsicosocialDto
{
    [StringLength(4000, ErrorMessage = "El análisis de Autoestima y Autocuidado no debe superar los 4000 caracteres.")]
    public string? AutoestimaAutocuidado { get; set; }

    [StringLength(4000, ErrorMessage = "El reporte del área Académica y Laboral no debe superar los 4000 caracteres.")]
    public string? AcademicoLaboral { get; set; }

    [StringLength(4000, ErrorMessage = "El reporte de Socialización y Familia no debe superar los 4000 caracteres.")]
    public string? SocializacionFamilia { get; set; }

    [StringLength(4000, ErrorMessage = "Las notas de Personalidad y Autoexpresión no deben superar los 4000 caracteres.")]
    public string? PersonalidadAutoexpresion { get; set; }
}