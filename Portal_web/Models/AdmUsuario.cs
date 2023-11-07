using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sgc.ml.Models;

[Table(name: "adm_usuarios")]
public class AdmUsuario
{
    [Key] public int usuario_id { get; set; }

    public string nombres { get; set; }

    public string apellidos { get; set; }
    public string email { get; set; }
    public string pwd { get; set; }

    public string username { get; set; }

    public int rol { get; set; }
    public DateTime date_created { get; set; }

    public string respuesta { get; set; }

    public int pregunta { get; set; }
}