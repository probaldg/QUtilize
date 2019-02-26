using QBA.Qutilize.WebApp.Helper;
using QBA.Qutilize.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBA.Qutilize.WebApp.Controllers
{
    public class AdminController : Controller
    {
        UserModel um = new UserModel();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageUsers(int ID = 0)
        {
            UserModel obj = new UserModel();
            if (ID > 0)
            {
                try
                {
                    DataTable dt = new DataTable();
                    dt = obj.GetUsersByID(ID);
                    obj.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                    obj.Name = dt.Rows[0]["Name"].ToString();
                    obj.UserName= dt.Rows[0]["UserName"].ToString();
                    //obj.Password = dt.Rows[0]["Password"].ToString();

                    obj.EmailId = dt.Rows[0]["EmailId"].ToString();

                    obj.IsActive = Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString());
                }
                catch
                {

                }
            }

            return View(obj);
        }

        [HttpPost]
        public ActionResult ManageUsers(UserModel model)
        {
            try
            {
                if (model.ID > 0)
                {
                    DataTable dt = new DataTable();
                    dt = um.GetUsersByID(model.ID);
                    model.EditedBy = System.Web.HttpContext.Current.Session["ID"].ToString();
                    model.EditedDate = DateTime.Now;

                    um.Update_UserDetails(model);
                    TempData["ErrStatus"] = model.ISErr.ToString();
                }
                else
                {
                    int id;
                    model.CreatedBy = (System.Web.HttpContext.Current.Session["ID"].ToString());
                    model.CreateDate = DateTime.Now;
                    string password = model.Password;
                    password = EncryptionHelper.ConvertStringToMD5(password);
                    model.IsActive = true;
                    model.Password = password;

                    um.InsertUserdata(model, out id);
                    if (id > 0)
                    {

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction("ManageUsers", "Admin");
        }
        public ActionResult LoadUsersData()
        {
            UserModel obj = new UserModel();

            string strUserData = string.Empty;

            int i = 0;

            DataTable dt = obj.GetAllUsers();


            foreach (DataRow dr in dt.Rows)
            {
                strUserData += "<tr><td class='text-center'>" + dr["Id"].ToString() + "</td><td class='text-center'>" + dr["UserName"].ToString() + "</td>" + "<td class='text-center'>" + dr["Name"].ToString() + "</td>" + "<td class='text-center'>" + dr["EmailId"] + "</td>" +
                                 "<td class='text-center'><a href = 'ManageUsers?ID=" + dr["ID"].ToString() + "'>Edit</a></td></tr>";
                i++;
            }
            return Content(strUserData);
        }
    }
}