using Newtonsoft.Json.Linq;
using QBA.Qutilize.DataAccess.DataModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace QBA.Qutilize.WebAPI.Controllers
{
    public class UserProjectController : ApiController
    {
        QutilizeModel _dbContext;
        public UserProjectController()
        {
            _dbContext = new QutilizeModel();
        }

        [HttpGet()]
        [Route("api/UserProject/GetUserProjectsByUserID/")]
        [ResponseType(typeof(JObject))]
        public ICollection<Project> GetUserProjectsByUserID(int id)
        {
            List<Project> projects = new List<Project>();
            try
            {
                var User = _dbContext.Users.Include("Projects").Where(x => x.Id == id && x.IsActive).FirstOrDefault<User>();
                foreach (var item in User.Projects)
                {
                    projects.Add(new Project
                    {
                        Name = item.Name,
                        Description = item.Description,
                        ParentProjectId = item.ParentProjectId,
                        IsActive=item.IsActive,
                        CreateDate= item.CreateDate,
                        CreatedBy=item.CreatedBy,
                        Id= item.Id
                    });
                }

            }
            catch (System.Exception)
            {
                throw;
            }
           
            return projects;
        }
    }
}
