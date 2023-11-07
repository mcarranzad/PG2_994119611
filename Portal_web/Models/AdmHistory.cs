using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sgc.ml.Models;

[Table(name:"adm_history")]
public class AdmHistory
{
    [Key]
    public int history_id { get; set; }
    
    [Column(name:"dpi")]
    public long dpi { get; set; }
    
    [Column(name:"departamento")]
    public string departamento { get; set; }
    
    [Column(name:"municipio")]
    public string municipio { get; set; }
    
    [Column(name:"fecha_nacimiento")]
    public string fecha_nacimiento { get; set; }
    
    [Column(name:"precition")]
    public decimal precition { get; set; }
    
    [Column(name:"prediccion")]
    public string prediccion { get; set; }
    
    [Column(name:"date_created")]
    public DateTime date_created { get; set; }
    
    [Column(name:"resultado")]
    public byte resultado { get; set; }
    
}