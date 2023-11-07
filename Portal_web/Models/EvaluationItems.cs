namespace sgc.ml.Models;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Id
{
    public string dpi { get; set; }
}

public class EvaluationItems
{
    public List<Id> ids { get; set; }
}