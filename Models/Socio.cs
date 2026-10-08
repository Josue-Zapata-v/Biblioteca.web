using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Models;

public class Socio
{
    [Display(Name = "ID")]
    public int SocioId { get; set; }

    [Required(ErrorMessage = "El DNI es obligatorio.")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener exactamente 8 caracteres.")]
    [RegularExpression(@"^[0-9]{8}$", ErrorMessage = "El DNI debe tener 8 dígitos numéricos.")]
    [Display(Name = "DNI")]
    public string DNI { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(120, ErrorMessage = "El nombre no puede exceder los 120 caracteres.")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(120, ErrorMessage = "El correo no puede exceder los 120 caracteres.")]
    [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido.")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Correo Electrónico")]
    public string? Email { get; set; }

    [Display(Name = "Activo")]
    public bool Activo { get; set; } = true;
}
