using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Models;

public class Libro
{
    [Display(Name = "ID")]
    public int LibroId { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(200, ErrorMessage = "El título no puede exceder los 200 caracteres.")]
    [Display(Name = "Título")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El ISBN es obligatorio.")]
    [StringLength(20, ErrorMessage = "El ISBN no puede exceder los 20 caracteres.")]
    [Display(Name = "ISBN")]
    public string ISBN { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe seleccionar un autor.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un autor válido.")]
    [Display(Name = "Autor")]
    public int AutorId { get; set; }

    [Display(Name = "Autor")]
    public string? AutorNombre { get; set; }

    [Required(ErrorMessage = "El número de ejemplares es obligatorio.")]
    [Range(0, 10000, ErrorMessage = "El número de ejemplares debe ser mayor o igual a 0.")]
    [Display(Name = "Ejemplares")]
    public int Ejemplares { get; set; }

    [Display(Name = "Estado")]
    public bool Activo { get; set; } = true;
}
