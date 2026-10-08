using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Models;

public class Autor
{
    [Display(Name = "ID")]
    public int AutorId { get; set; }

    [Required(ErrorMessage = "El nombre del autor es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(60, ErrorMessage = "La nacionalidad no puede exceder los 60 caracteres.")]
    [Display(Name = "Nacionalidad")]
    public string? Nacionalidad { get; set; }

    [Display(Name = "Activo")]
    public bool Activo { get; set; } = true;
}
