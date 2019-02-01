using Newtonsoft.Json.Linq;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using User = QBA.Qutilize.Models.User;
using QBA.Qutilize.DataAccess.DataModel;

namespace QBA.Qutilize.WebAPI.Controllers
{
    public class AccountController : ApiController
    {
      
        QutilizeModel _dbContext;
        public AccountController()
        {
            _dbContext = new QutilizeModel();
        }

        [HttpPost()]
        [Route("api/Account/Login/")]
        [ResponseType(typeof(JObject))]
        public User Login(User user)
        {
            var dbUser = _dbContext.Users.SingleOrDefault(x => x.UserName == user.UserName
                        && x.Password == user.Password && x.IsActive);

            if (dbUser != null)
            {
                User userModel = new User
                {
                  Name   = dbUser.Name,
                    UserName = dbUser.UserName,
                    IsActive = dbUser.IsActive,
                };
                return userModel;
            }
            else
                return null;

        }

        //[HttpPost()]
        //[Route("api/Account/Login/")]
        //[ResponseType(typeof(JObject))]
        //public User Login(string userId, string password)
        //{
        //    var dbUser = _dbContext.Users.SingleOrDefault(x => x.UserID == userId
        //                && x.Password == password && x.Active.ToLower() == "y");

        //    if (dbUser != null)
        //    {
        //        User userModel = new User
        //        {
        //            UserID = dbUser.UserID,
        //            UserName = dbUser.UserName,
        //            Active = dbUser.Active,
        //        };
        //        return userModel;
        //    }
        //    else
        //        return null;

        //}
    }
}
