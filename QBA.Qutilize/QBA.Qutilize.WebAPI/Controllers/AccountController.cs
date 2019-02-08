using Newtonsoft.Json.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using User = QBA.Qutilize.Models.User;
using QBA.Qutilize.DataAccess.DAL;
using System.Linq;
using QBA.Qutilize.Models;

namespace QBA.Qutilize.WebAPI.Controllers
{
    public class AccountController : ApiController
    {

        QUtilizeDBCon _dbContext;
        public AccountController()
        {
            _dbContext = new QUtilizeDBCon();
        }

        [HttpPost()]
        [Route("api/Account/Login/")]
        [ResponseType(typeof(JObject))]
        public User Login(User user)
        {
            if (user == null)
                return null;

            var dbUser = _dbContext.USPUsers_Get(user.UserName, user.Password).ToList();

            if (dbUser != null)
            {
                return CreateUser(dbUser);
            }
            else
                return null;

        }

        private User CreateUser(System.Collections.Generic.List<USPUsers_Get_Result> dbUser)
        {
            User userModel = new User();
            if (dbUser.Count > 0)
            {
                userModel.ID = dbUser.ElementAt(0).Id;
                userModel.Name = dbUser.ElementAt(0).Name;
                userModel.UserName = dbUser.ElementAt(0).UserName;
                userModel.CreateDate = dbUser.ElementAt(0).CreateDate;
                userModel.CreatedBy = dbUser.ElementAt(0).CreatedBy;

                foreach (var item in dbUser)
                {
                    userModel.Projects.Add(new Qutilize.Models.Project()
                    {
                        ProjectName = item.ProjectName,
                        ProjectID = item.ProjectID,
                        ParentProjectID = item.ParentProjectId,
                        Description = item.ProjectDescription,

                    });
                    userModel.Roles.Add(new Roles()
                    {
                        Id = item.RoleID,
                        Name = item.RoleName
                    });
                }
            }
            return userModel;
        }
    }
}
