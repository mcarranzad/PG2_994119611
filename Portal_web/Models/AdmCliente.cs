using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sgc.ml.Models;

[Table(name: "adm_cliente", Schema = "public")]
public class AdmCliente
{
    [Key] public int clienteId { get; set; }
    
    public string nombreCompleto { get; set; }
    public long dpi { get; set; }
    
    public string fechaNacimiento { get; set; }
    
    public string departamento { get; set; }
    
    public string municipio { get; set; }
    
}