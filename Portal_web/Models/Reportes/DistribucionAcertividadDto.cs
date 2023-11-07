using System.ComponentModel.DataAnnotations;

namespace sgc.ml.Dto.Reportes;

public class DistribucionAcertividadDto
{
    [Key]
    public int id { get; set; }
    
    public string prediccion { get; set; }
    
    public int cantidad { get; set; }
    
    public double acertividad { get; set; }
   
}