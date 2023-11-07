using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sgc.ml.Models;

[Table(name:"adm_rol")]
public class AdmRol
{
    
    [Key]
    public int rol_id { get; set; }

    public string nom_rol { get; set; }

    public DateTime date_created { get; set; }
}