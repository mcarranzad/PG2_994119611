using sgc.ml.Models;

namespace sgc.ml.Services.Interfaces;

public interface INeuronalService
{
    
    /// <summary>
    /// Este metodo se usa para devolver la informacion de la red neuronal
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<EvaluationResponse> GetResponse(EvaluationItems model);
    
}