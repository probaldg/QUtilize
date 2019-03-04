using QBA.Qutilize.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QBA.Qutilize.WebApp.DAL;
using System.Data;
using QBA.Qutilize.WebApp.Helper;
using System.Configuration;

namespace QBA.Qutilize.WebApp.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult LoadSideMenu()
        {
            AccountViewModels obj = new Models.AccountViewModels();
            string strMenu = string.Empty;
            DataTable dt = obj.GetDashBoardMenu(Convert.ToInt32(Session["sessUser"]));
            foreach (DataRow dr in dt.Rows)
            {
                strMenu += "<div class='row lien'>" +
                           "<div class='col-md-12'>" +
                           "<a href='" + dr["URL"] + "' class='fa " + dr["DisplayCSS"].ToString() + "'></a> &nbsp;" +
                           "<a href='" + ConfigurationSettings.AppSettings["SiteAddress"]+ dr["URL"] + "'>" + dr["DisplayName"].ToString() + "</a>" +
                           "<hr>" +
                           "</div>" +
                           "</div>";
            }
            return Content(strMenu);
        }

        public ActionResult MyAccount()
        {

            if (System.Web.HttpContext.Current.Session["sessUser"] != null)
            {
                UserModel user = new UserModel();
                try
                {
                    DataTable dt = user.GetMyAccountData(int.Parse(System.Web.HttpContext.Current.Session["sessUser"].ToString()));
                    if (dt.Rows.Count > 0)
                    {
                        user.ID = int.Parse(dt.Rows[0]["Id"].ToString());
                        user.Name = dt.Rows[0]["Name"].ToString();
                       
                        user.EmailId = dt.Rows[0]["EmailId"].ToString();
                      

                        ViewBag.ID = int.Parse(dt.Rows[0]["ID"].ToString());
                        ViewBag.Name = dt.Rows[0]["Name"].ToString();
                        ViewBag.Email = dt.Rows[0]["EmailId"].ToString();
                        //  ViewBag.OrgName = dt.Rows[0]["orgname"].ToString();
                        // ViewBag.Designation = dt.Rows[0]["Designation"].ToString();
                    }
                    return View(user);
                }
                catch (Exception)
                {

                    throw;
                }
               
            }
            else
            {
                return Redirect("/Home/Index");
            }
        }


        public JsonResult UpdatePassword(int id, string password)
        {
            UserModel user = new UserModel();
            try
            {
                password = EncryptionHelper.ConvertStringToMD5(password);

                int editedBy = int.Parse(Session["sessUser"].ToString());
                DateTime editedTS = DateTime.Now;

                DataTable dt = user.updatePassword(id, password, editedBy, editedTS);
            }
            catch (Exception)
            {

                //throw;
            }
           
            return Json("updated");
        }

    }
}