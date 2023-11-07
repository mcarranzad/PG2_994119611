using sgc.ml.Models;
using sgc.ml.Util;

namespace sgc.ml.Repository.Interfaces;

public interface IAdmUsuarioRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    AdmUsuario? checkUsuarioAndPwd(string email, string password);


    public AdmUsuario? checkIfUsuarioExist(string usuario);
    
    /// <summary>
    /// Este metodo persiste los historicos que vienen de eventix
    /// </summary>
    /// <param name="model"></param>
    void Persist(AdmUsuario model);
    
    void update(AdmUsuario model);
}