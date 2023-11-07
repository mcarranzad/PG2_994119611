using Newtonsoft.Json;

namespace sgc.ml.Models;


public class Information1
{
    public long DPI { get; set; }
    [JsonProperty("Nombre Completo")] public string NombreCompleto { get; set; }
    public string departamento { get; set; }
    public string fechaNacimiento { get; set; }
    public object genero { get; set; }
    public string municipio { get; set; }
    public string prediccion { get; set; }
    
    public string precision { get; set; }
}


public class Resultado
{
    public List<Information1> Information_1 { get; set; }
}

public class EvaluationResponse
{
    public List<Resultado> Resultado { get; set; }

    public bool result { get; set; }
    
    public string msg { get; set; }
    
    
    
}