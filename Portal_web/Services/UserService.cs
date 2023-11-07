using sgc.ml.Dto;
using sgc.ml.Services.Interfaces;

namespace sgc.ml.Services
{

    public class UserService : IUserService
    {

        private IEnumerable<DtoLogin> usersFake;

        /// <summary>
        /// 
        /// </summary>
        public UserService()
        {

            usersFake = new List<DtoLogin>()
            {
                new DtoLogin(){  username="angela", pwd ="copodenieve"  },
                 new DtoLogin(){  username="admin", pwd ="admin"  }
            };

        }

        public bool checkLogin(string username, string password)
        {

            return usersFake.ToList().Find(x => x.username.Equals(username) && x.pwd.Equals(password)) != null ? true : false;
        }
    }
}
