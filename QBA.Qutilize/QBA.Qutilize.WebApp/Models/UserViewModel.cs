using QBA.Qutilize.Models;
using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace QBA.Qutilize.WebApp.Models
{
    public class UserViewModel
    {
        public List<User> Users { get; set; }

        public UserViewModel()
        {
            Users = new List<User>();
        }

        public List<User> GetUsers()
        {
           var userDT= DataAccess.GetUsers();
            Users.Clear();

            if (userDT.Rows.Count>0)
            {
                for (int i = 0; i < userDT.Rows.Count-1; i++)
                {
                    User user = new User();
                    user.ID = (int)userDT.Rows[i]["Id"];
                    user.Name = userDT.Rows[i]["Name"].ToString();
                    user.UserName= userDT.Rows[i]["UserName"].ToString();
                    user.EmailId= userDT.Rows[i]["EmailId"]?.ToString();
                    Users.Add(user);
                }
                
            }
            return Users;
        }
    }
}