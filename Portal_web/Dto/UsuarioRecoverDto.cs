using System.ComponentModel.DataAnnotations;

namespace sgc.ml.Dto;

public class UsuarioRecoverDto
{
    [Required(ErrorMessage = "El campo usuario tiene un valor invalido")]
    public string usuario { get; set; }
    
    [Required(ErrorMessage = "El campo pregunta tiene un valor invalido")]
    [Range(0, int.MaxValue, ErrorMessage = "El valor del campo pregunta no es valido")]
    public int pregunta { get; set; }
    
    [Required(ErrorMessage = "El campo respuesta tiene un valor invalido")]
    public string respuesta { get; set; }
}