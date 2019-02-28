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

        #region User managment region

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
                    obj.UserName = dt.Rows[0]["UserName"].ToString();
                    //obj.Password = dt.Rows[0]["Password"].ToString();

                    obj.EmailId = dt.Rows[0]["EmailId"].ToString();

                    obj.IsActive = Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString());

                    um.Update_UserDetails(obj);
                    TempData["ErrStatus"] = obj.ISErr.ToString();
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
                    model.EditedBy = System.Web.HttpContext.Current.Session["ID"]?.ToString();
                    model.EditedDate = DateTime.Now;


                    um.Update_UserDetails(model);
                    TempData["ErrStatus"] = model.ISErr.ToString();
                }
                else
                {
                    int id;
                    if (System.Web.HttpContext.Current.Session["ID"] != null)
                    {
                        model.CreatedBy = (System.Web.HttpContext.Current.Session["ID"].ToString());
                    }

                    model.CreateDate = DateTime.Now;
                    string password = model.Password;
                    password = EncryptionHelper.ConvertStringToMD5(password);
                    model.IsActive = model.IsActive;
                    model.Password = password;

                    um.InsertUserdata(model, out id);
                    if (id > 0)
                    {

                    }
                    TempData["ErrStatus"] = model.ISErr.ToString();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction("ManageUsers", "Admin");
        }

        public ActionResult DeleteUser(int ID)
        {
            UserModel obj = new UserModel();
            if (ID > 0)
            {
                try
                {

                    obj.ID = Convert.ToInt32(ID);
                    obj.EditedBy = "";
                    obj.EditedDate = DateTime.Now;

                }
                catch
                {

                }
            }

            return View(obj);
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
                                 "<td class='text-center'><a href = 'ManageUsers?ID=" + dr["ID"].ToString() + "'>Edit </a> </td></tr>";
                i++;
            }
            return Content(strUserData);
        }

        public ActionResult Checkemail(string email)
        {
            UserModel _um = new UserModel();
            DataTable dt = _um.checkemail(email);
            string message = string.Empty;
            if (dt.Rows.Count > 0)
            {
                message = "Duplicate";
            }
            else
            {
                message = "Ok";
            }
            return Json(message);
        }

        public ActionResult UpdatePassword(int id, string Password)
        {
            string password = Password;
            int editedBy = 0;
            password = EncryptionHelper.ConvertStringToMD5(password);
            if (System.Web.HttpContext.Current.Session["ID"] != null)
            {
                editedBy = Convert.ToInt32(System.Web.HttpContext.Current.Session["ID"]);
            }
            DateTime editedTS = DateTime.Now;

            string status = string.Empty;

            try
            {
                um.updatePassword(id, password, editedBy, editedTS);
                status = "Updated Successfully";
            }
            catch (Exception)
            {

                status = "Something Went wrong. Please try later";
            }


            return Json(status);
        }
        #endregion


        #region Project managment region"
        //
        public ActionResult LoadProjectData()
        {
            ProjectModel obj = new ProjectModel();

            string strUserData = string.Empty;

            int i = 0;

            DataTable dt = obj.GetAllProjects();


            foreach (DataRow dr in dt.Rows)
            {
                //strUserData += "<tr><td class='text-center'>" + dr["Id"].ToString() + "</td><td class='text-center'>" + dr["Name"].ToString() + "</td>" + "<td class='text-center'>" + dr["Description"].ToString() + "</td>" + "<td class='text-center'>" + dr["ParentProjectName"] + "</td>" +
                //                 "<td class='text-center'><a href = 'ManageProject?ID=" + dr["ID"].ToString() + "'>Edit </a> </td></tr>";

                strUserData += "<tr><td class='text-center'>" + dr["Id"].ToString() + "</td><td class='text-center'>" + dr["Name"].ToString() + "</td>" + "<td class='text-center'>" + dr["Description"].ToString() +
                 "<td class='text-center'><a href = 'ManageProject?ID=" + dr["ID"].ToString() + "'>Edit </a> </td></tr>";
                i++;
            }
            return Content(strUserData);
        }
        public ActionResult ManageProject(int ID = 0)
        {
            ProjectModel obj = new ProjectModel();
            List<ProjectModel> objRole = new List<ProjectModel>();

            if (ID > 0)
            {
                try
                {
                    DataTable dt = new DataTable();
                    dt = obj.GetProjectByID(ID);
                    obj.ProjectID = Convert.ToInt32(dt.Rows[0]["ID"]);
                    obj.ProjectName = dt.Rows[0]["Name"].ToString();
                    obj.Description = dt.Rows[0]["Description"].ToString();
                    obj.IsActive = Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString());

                    obj.Update_ProjectDetails(obj);
                    TempData["ErrStatus"] = obj.ISErr.ToString();
                }
                catch
                {

                }
            }
            else
            {
                //TODO populate all project in dropdown list
            }

            return View(obj);
        }

        [HttpPost]
        public ActionResult ManageProject(ProjectModel model)
        {
            ProjectModel obj = new ProjectModel();
            if (model.ProjectID > 0)
            {
                try
                {
                    obj = model;
                    obj.EditedBy = System.Web.HttpContext.Current.Session["ID"]?.ToString();
                    obj.EditedDate = DateTime.Now;
                    obj.IsActive = model.IsActive;

                    obj.Update_ProjectDetails(obj);

                    TempData["ErrStatus"] = obj.ISErr.ToString();
                }
                catch
                {

                }
            }
            else
            {

                model.CreatedBy = System.Web.HttpContext.Current.Session["ID"]?.ToString();
                model.CreateDate = DateTime.Now;

                model.IsActive = model.IsActive;

                obj.InsertProjectdata(model, out int id);
                if (id > 0)
                {

                }
                TempData["ErrStatus"] = model.ISErr.ToString();
            }
            return RedirectToAction("ManageProject", "Admin");

        }
        #endregion



        #region User Project Mapping region
        public ActionResult UserProjectMapping()
        {
            //UserProjectMappingModel userProject = new UserProjectMappingModel();

            //SelectList objListOfUserToBind = new SelectList(userProject.GetAllUsers(), "ID", "Name", 0);
            //userProject.UsersSelectList = objListOfUserToBind;


            return View();
        }


        public ActionResult LoadALLUserMapped()
        {
            string strProjectMapped = string.Empty;
            UserProjectMappingModel USM = new UserProjectMappingModel();
            DataTable dt = USM.GetAllUsers();

            foreach (DataRow dr in dt.Rows)
            {
                int i = 0;

                strProjectMapped += "<tr>";
                strProjectMapped += "<td align='center'>" + dr["ID"].ToString() + "</td><td align='center'>" + dr["Name"].ToString() + "</td>";
                strProjectMapped += "<td align='center'><button data-toggle='modal' data-target='#myModalForModule' onclick='ShowPermission(" + dr["ID"].ToString() + ")'>Map</button></td>";
                //strProjectMapped += "<td align='center'><button data-toggle='modal' data-target='#myModalForModule'>Map</button></td>";

                strProjectMapped += "</td>";
                i++;
            }

            return Content(strProjectMapped);
        }

        public ActionResult LoadAllModules()
        {
            UserProjectMappingModel obj = new UserProjectMappingModel();
            DataTable dt = obj.GetAllProjects();
            string strModules = string.Empty;

            foreach (DataRow dr in dt.Rows)
            {
                strModules += "<ul class='module' style='list-style: none;'>";
                strModules += "<li class='limodule' style='list-style: none;'>";
                strModules += "<input type='checkbox' class='check' name='modules' value='" + dr["Id"].ToString() + "'>" +
                                dr["Name"].ToString();

                strModules += "</li>";
                strModules += "</ul>";

            }
            return Content(strModules);
        }


        [HttpPost]
        public ActionResult SaveProjectMapping(UserProjectMappingModel[] itemlist)
        {
            UserProjectMappingModel UPM = new UserProjectMappingModel();
            int UserID = itemlist[0].UserId;
            try
            {

           
            DataTable dt = UPM.GetAllProjectByUserID(itemlist[0].UserId); // TODO crate a proc to get all the project mapped to the user

            if (dt.Rows.Count > 0)
            {
                UPM.DeleteAllExistingMapping(itemlist[0].UserId); // TODO crate a proc to delete all the project mapped to the user
            }

            foreach (UserProjectMappingModel i in itemlist)   //loop through the array and insert value into database.
            {
                UserProjectMappingModel mm = new UserProjectMappingModel();

                mm.UserId = i.UserId;
                mm.ProjectId = i.ProjectId;
                mm.InsertUserProjectMappingdata(mm);
            }
            }
            catch (Exception)
            {

                throw;
            }
            return Json(true);
        }
        #endregion
    }
}