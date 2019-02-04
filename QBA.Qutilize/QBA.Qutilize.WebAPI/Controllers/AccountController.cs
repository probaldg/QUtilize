using Newtonsoft.Json.Linq;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using User = QBA.Qutilize.Models.User;
using QBA.Qutilize.DataAccess.DataModel;
using Microsoft.Ajax.Utilities;

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
            var dbUser = _dbContext.Users.Include("Projects").SingleOrDefault(x => x.UserName == user.UserName
                        && x.Password == user.Password && x.IsActive);

           // var dbUser = _dbContext.Users.Include("Projects").Where(x => x.UserName == user.UserName && x.IsActive).FirstOrDefault<User>();

            if (dbUser != null)
            {

                User userModel = new User
                {
                    ID = dbUser.Id,
                    Name = dbUser.Name,
                    UserName = dbUser.UserName,
                    IsActive = dbUser.IsActive,
                    CreateDate = dbUser.CreateDate,
                    CreatedBy = dbUser.CreatedBy
                };

                //Getting all the project for this user
                foreach (var item in dbUser.Projects)
                {
                    QBA.Qutilize.Models.Project project = new QBA.Qutilize.Models.Project();
                    project.ProjectName = item.Name;
                    project.ProjectID= item.Id;
                    project.Description = item.Description;
                    project.ParentProjectID = item.ParentProjectId;
                    project.CreatedBy = item.CreatedBy;
                    project.CreateDate = item.CreateDate;
                    project.IsActive = item.IsActive;

                    userModel.Projects.Add(project);
                }

                
                return userModel;
            }
            else
                return null;

        }

       
    }
}
