using System.ComponentModel.DataAnnotations;

namespace sgc.ml.Dto
{
    public class DtoLogin
    {
        [Required(ErrorMessage = "Campo usuario requerido"), MaxLength(50, ErrorMessage = "Maximo 50 caracteres")]
        public string username { get; set; }
        [Required(ErrorMessage = "Campo contraseña requerido"), MaxLength(50, ErrorMessage = "Maximo 50 caracteres")]
        public string pwd { get; set; }

    }
}
