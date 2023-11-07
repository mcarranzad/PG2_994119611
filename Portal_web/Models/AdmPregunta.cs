using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sgc.ml.Models;

[Table(name:"adm_pregunta")]
public class AdmPregunta
{
    [Key]
    public int pregunta_id { get; set; }

    public string nom_pregunta { get; set; }

    public DateTime date_created { get; set; }
}
