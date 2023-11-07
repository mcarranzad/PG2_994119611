using sgc.ml.Models;
using sgc.ml.Repository.Interfaces;
using sgc.ml.Resources;

namespace sgc.ml.Repository;

public class AdmClienteRepository : IAdmClienteRepository
{
    private readonly DataContext dbContext;


    public AdmClienteRepository(DataContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IEnumerable<AdmCliente> getAll()
    {
        try
        {
            return dbContext.clientes.ToList().Take(20);
        }
        catch (Exception ex)
        {
            return new List<AdmCliente>();
        }
    }

    public AdmCliente? getOne(long dpi)
    {
        try
        {
            return dbContext.clientes.FirstOrDefault(x => x.dpi == (dpi));
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}