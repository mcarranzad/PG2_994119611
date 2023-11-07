using sgc.ml.Dto;

namespace sgc.ml.Services.Interfaces
{
    public interface IUserService
    {

        /// <summary>
        /// Function para obtener los usuarios y verificar si existe
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool checkLogin(string username, string password);


    }
}
