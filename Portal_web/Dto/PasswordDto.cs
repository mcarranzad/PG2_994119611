using System.ComponentModel.DataAnnotations;

namespace sgc.ml.Dto;

public class PasswordDto
{
    [Required(ErrorMessage = "El campo usuario tiene un valor invalido")]
    public string usuario { get; set; }
    
    [Required(ErrorMessage = "El campo password tiene un valor invalido")]
    public string password { get; set; }
    
    [Required(ErrorMessage = "El campo confirmar tiene un valor invalido")]
    public string confirmar { get; set; }
    
}