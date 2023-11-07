using System.ComponentModel.DataAnnotations;

namespace sgc.ml.Dto.Reportes;

public class MunicipiosConsultasDto
{
    [Key] 
    public int id { get; set; }
    
    public string  municipio { get; set; }
    
    public double  verificacion_a { get; set; }
    
    public double  verificacion_b { get; set; }
    
    public double  recuperacion_a { get; set; }
    
    public double  recuperacion_b { get; set; }
}