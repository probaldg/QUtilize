using QBA.Qutilize.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBA.Qutilize.WebApp.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            try
            {
                UserViewModel userViewModel = new UserViewModel();
                userViewModel.GetUsers();

                return View(userViewModel);
            }
            catch (Exception)
            {

                throw;
            }
          
        }
    }
}