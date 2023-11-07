using System.Transactions;
using Microsoft.EntityFrameworkCore;
using sgc.ml.Models;
using sgc.ml.Repository.Interfaces;
using sgc.ml.Resources;
using sgc.ml.Util;

namespace sgc.ml.Repository;

public class AdmUsuarioRepository : IAdmUsuarioRepository
{
    private readonly DataContext dbContext;

    public AdmUsuarioRepository(DataContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public AdmUsuario? checkUsuarioAndPwd(string usuario, string password)
    {
        return dbContext.usuarios.FirstOrDefault(x => x.username.Equals(usuario) && x.pwd.Equals(password));
    }

    public AdmUsuario? checkIfUsuarioExist(string usuario)
    {
        return dbContext.usuarios.FirstOrDefault(x => x.username.Equals(usuario));
    }
    
    

    /// <summary>
    /// Este metodo persiste los historicos que vienen de eventix
    /// </summary>
    /// <param name="model"></param>
    public void Persist(AdmUsuario model)
    {
        model.usuario_id = 0;
        model.date_created = SgcFunctions.getNow();
        dbContext.Add(model);
        Save();
    }
    
    public void update(AdmUsuario model)
    {
        using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                dbContext.Entry(model).State = EntityState.Modified;
                Save();
                scope.Complete();
            }
            catch (Exception ex)
            {
                // logger.LogError(ex.InnerException.Message);
                // logger.LogError(ex.Message);
                scope.Dispose();
                throw;
            }
        }
    }


    public void Save()
    {
        try
        {
            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}