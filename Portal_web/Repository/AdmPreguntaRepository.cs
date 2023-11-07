using sgc.ml.Models;
using sgc.ml.Repository.Interfaces;
using sgc.ml.Resources;

namespace sgc.ml.Repository;

public class AdmPreguntaRepository : IAdmPreguntaRepository
{
    private readonly DataContext dbContext;

    public AdmPreguntaRepository(DataContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public List<AdmPregunta> GetPreguntas()
    {
        try
        {
            return dbContext.preguntas.ToList();
        }
        catch (Exception ex)
        {
            return new List<AdmPregunta>();
        }
    }
}