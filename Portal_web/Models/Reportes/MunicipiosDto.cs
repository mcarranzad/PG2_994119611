using System.ComponentModel.DataAnnotations;

namespace sgc.ml.Dto.Reportes;

public class MunicipiosDto
{
    
    [Key] 
    public int id { get; set; }
    
    public string  municipio { get; set; }
}