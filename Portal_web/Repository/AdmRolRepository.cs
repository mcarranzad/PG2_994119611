using sgc.ml.Models;
using sgc.ml.Repository.Interfaces;
using sgc.ml.Resources;

namespace sgc.ml.Repository;

public class AdmRolRepository : IAdmRolRepository
{
    private readonly DataContext dbContext;

    public AdmRolRepository(DataContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public List<AdmRol> GetRol()
    {
        try
        {
            return dbContext.rols.ToList();
        }
        catch (Exception ex)
        {
            return new List<AdmRol>();
        }
    }
}