using QBA.Qutilize.DataAccess.DataModel;
using QBA.Qutilize.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QBA.Qutilize.WebAPI.Controllers
{
    public class UserProjectController : ApiController
    {
        QutilizeModel _dbContext;
        public UserProjectController()
        {
            _dbContext = new QutilizeModel();
        }
        public IEnumerable<Project> GetUserProjectsByUserID(int userID)
        {
        //    var UserInRole = db.UserProfiles.
        //Join(db.UsersInRoles, u => u.UserId, uir => uir.UserId,
        //(u, uir) => new { u, uir }).
        //Join(db.Roles, r => r.uir.RoleId, ro => ro.RoleId, (r, ro) => new { r, ro })
        //.Select(m => new AddUserToRole
        //{
        //    UserName = m.r.u.UserName,
        //    RoleName = m.ro.RoleName
        //});

                var ProjectsForUser=  _dbContext.Projects.Join(_dbContext.Users )
        }
    }
}
