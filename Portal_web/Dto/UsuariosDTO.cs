using System.ComponentModel.DataAnnotations;

namespace sgc.ml.Dto;

public class UsuariosDTO
{
    [Required(ErrorMessage = "El campo nombres es invalido")]
    public string nombres { get; set; }

    [Required(ErrorMessage = "El campo apellidos es invalido")]
    public string apellidos { get; set; }

    [Required(ErrorMessage = "El correo electronico es invalido")]
    public string email { get; set; }

    [Required(ErrorMessage = "El campo contraseña tiene un valor invalido")]
    public string pwd { get; set; }

    [Required(ErrorMessage = "El campo usuario tiene un valor invalido")]
    public string usuario { get; set; }

    [Required(ErrorMessage = "El campo rol tiene un valor invalido")]
    [Range(0, int.MaxValue, ErrorMessage = "El valor del campo rol no es valido")]
    public int rol { get; set; }
    
    [Required(ErrorMessage = "El campo pregunta tiene un valor invalido")]
    [Range(0, int.MaxValue, ErrorMessage = "El valor del campo pregunta no es valido")]
    public int pregunta { get; set; }
    
    [Required(ErrorMessage = "El campo respuesta tiene un valor invalido")]
    public string respuesta { get; set; }
    
}