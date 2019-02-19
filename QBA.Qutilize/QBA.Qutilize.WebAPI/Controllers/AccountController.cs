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
            try
            {
                var dbUser = _dbContext.USPUsers_Get(user.UserName, user.Password).ToList();

                if (dbUser != null)
                {
                    return CreateUser(dbUser);
                }
                else
                    return null;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private User CreateUser(System.Collections.Generic.List<USPUsers_Get_Result> dbUser)
        {
            User userModel = new User();
            try
            {
                if (dbUser.Count > 0)
                {
                    userModel.ID = dbUser.ElementAt(0).Id;
                    userModel.Password = dbUser.ElementAt(0).Password;
                    userModel.Name = dbUser.ElementAt(0).Name;
                    userModel.UserName = dbUser.ElementAt(0).UserName;
                    userModel.CreateDate = dbUser.ElementAt(0).CreateDate;
                    userModel.CreatedBy = dbUser.ElementAt(0).CreatedBy;

                    foreach (var item in dbUser)
                    {
                        if(item.ProjectID != null)
                        {
                            userModel.Projects.Add(new Qutilize.Models.Project()
                            {
                                ProjectName = item.ProjectName,
                                ProjectID = item.ProjectID,
                                ParentProjectID = item.ParentProjectId,
                                Description = item.ProjectDescription,
                            });
                        }
                       
                        if(item.RoleID != null)
                        {
                            userModel.Roles.Add(new Roles()
                            {
                                Id = item.RoleID,
                                Name = item.RoleName
                            });
                        }
                        
                    }
                }

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return userModel;
        }
    }
}
