﻿using QBA.Qutilize.WebApp.Helper;
using QBA.Qutilize.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace QBA.Qutilize.WebApp.Controllers
{
    public class AdminController : Controller
    {
        ImageCompress generateThumbnail = new ImageCompress();
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
                strModules += "<ul class='module' style='list-style: none; margin:15px;'>";
                strModules += "<li class='limodule' style='list-style: none;margin:10px;'>";
                strModules += "<input type='checkbox' class='check' style=' margin:5px;' name='modules' value='" + dr["Id"].ToString() + "'>" +
                                dr["Name"].ToString();

                strModules += "</li>";
                strModules += "</ul>";

            }
            return Content(strModules);
        }
        //public ActionResult LoadAllModules()
        //{
        //    UserProjectMappingModel obj = new UserProjectMappingModel();
        //    DataTable dt = obj.GetAllProjects();
        //    string strModules = string.Empty;
        //    strModules += "<ul class='module' style='list-style: none; margin:15px;'>";
        //    strModules += "<tabel Border=1>";
        //    //foreach (DataRow dr in dt.Rows)
        //    //{
        //    //    //strModules += "<li class='limodule' style='list-style: none;margin:10px;'>";
        //    //strModules += "<input type='checkbox' class='check' style=' margin:5px;' name='modules' value='" + dr["Id"].ToString() + "'>" +
        //    //                dr["Name"].ToString();
        //    //strModules += "</li>";
        //    //}

        //    int columnCount = 3;
        //    int rowsCount = GetRowAndColumnCount(dt.Rows.Count, columnCount);
        //    strModules += "<tr>";
        //    for (int i = 0; i <= rowsCount; i++)
        //    {
        //        int dtRowCounter = 0;

        //        int col = 0;
        //        if (col <= 3 && col % 3 != 0 && col !=0)
        //        {
        //            strModules += "<td><li class='limodule' style='list-style: none;margin:10px;'>";

        //            strModules += "<input type='checkbox' class='check' style=' margin:5px;' name='modules' value='" + dt.Rows[dtRowCounter]["Id"].ToString() + "'>" +
        //                            dt.Rows[dtRowCounter]["Name"].ToString();
        //             strModules += "</li>";
        //            col++;
        //            dtRowCounter++;
        //        }
        //        else
        //        {
        //            strModules += "</tr >";
        //            if (col == 3)
        //            {
        //                col = 0;
        //                rowsCount++;
        //                strModules += "</tr>";
        //            }
        //        }


        //    }

        //    strModules += "</tabel>";
        //    strModules += "</ul>";
        //    return Content(strModules);
        //}
        public ActionResult GetProjectMappedToUser(int id)
        {
            UserProjectMappingModel upm = new UserProjectMappingModel();
            DataTable dt = upm.GetAllProjectByUserID(id);

            List<UserProjectMappingModel> viewModelList = new List<UserProjectMappingModel>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                UserProjectMappingModel um = new UserProjectMappingModel();
                um.UserId = int.Parse(dt.Rows[i]["UserId"].ToString());
                um.ProjectId = int.Parse(dt.Rows[i]["ProjectId"].ToString());
                //um.sysModuleID = int.Parse(dt.Rows[i]["sysModuleID"].ToString());

                viewModelList.Add(um);
            }
            return Json(viewModelList);
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

        #region Role managment region
        public ActionResult ManageRole(int ID = 0)
        {
            RoleModel obj = new RoleModel();
            List<ProjectModel> objRole = new List<ProjectModel>();
            if (!ModuleMappingHelper.IsUserMappedToModule(Convert.ToInt32(Session["sessUser"]), Request.Url.AbsoluteUri))
            {
                return RedirectToAction("DashBoard", "Home");
            }
            if (ID > 0)
            {
                try
                {
                    DataTable dt = new DataTable();
                    dt = obj.GetRolesByID(ID);
                    obj.Id = Convert.ToInt32(dt.Rows[0]["ID"]);
                    obj.Name = dt.Rows[0]["Name"].ToString();
                    obj.Description = dt.Rows[0]["Description"].ToString();
                    obj.IsActive = Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString());

                    obj.Update_RoleDetails(obj);
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
        public ActionResult ManageRole(RoleModel model)
        {
            RoleModel rm = new RoleModel();
            try
            {
                if (model.Id > 0)
                {
                    DataTable dt = new DataTable();
                    dt = um.GetUsersByID(model.Id);
                    model.EditedBy = System.Web.HttpContext.Current.Session["ID"]?.ToString();
                    model.EditedDate = DateTime.Now;
                    rm.Update_RoleDetails(model);

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

                    model.IsActive = model.IsActive;
                    rm.InsertRoletdata(model, out id);
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
            return RedirectToAction("ManageRole", "Admin");
        }
        public ActionResult LoadRoleData()
        {
            RoleModel obj = new RoleModel();

            string strUserData = string.Empty;

            int i = 0;

            DataTable dt = obj.GetAllRoles();


            foreach (DataRow dr in dt.Rows)
            {
                strUserData += "<tr><td class='text-center'>" + dr["Id"].ToString() + "</td><td class='text-center'>" + dr["Name"].ToString() + "</td>" + "<td class='text-center'>" + dr["Description"].ToString() + "</td>" +
                                 "<td class='text-center'>" + dr["isActive"].ToString() + "</td><td class='text-center'><a href = 'ManageRole?ID=" + dr["ID"].ToString() + "'>Edit </a> </td></tr>";
                i++;
            }
            return Content(strUserData);
        }
        #endregion

        #region User Role Mapping region
        public ActionResult UserRoleMapping()
        {

            return View();
        }

        public ActionResult LoadALLUserRoleToMapped()
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

        public ActionResult LoadAllRoles()
        {
            UserRoleMappingModel obj = new UserRoleMappingModel();
            DataTable dt = obj.GetAllRoles();
            string strModules = string.Empty;

            foreach (DataRow dr in dt.Rows)
            {
                strModules += "<ul class='module' style='list-style: none; margin:15px;'>";
                strModules += "<li class='limodule' style='list-style: none;margin:10px;'>";
                strModules += "<input type='checkbox' class='check' style=' margin:5px;' name='modules' value='" + dr["Id"].ToString() + "'>" +
                                dr["Name"].ToString();

                strModules += "</li>";
                strModules += "</ul>";

            }
            return Content(strModules);
        }

        public ActionResult SaveRoleMapping(UserRoleMappingModel[] itemlist)
        {
            UserRoleMappingModel UPM = new UserRoleMappingModel();
            int UserID = itemlist[0].UserId;
            try
            {


                DataTable dt = UPM.GetAllRolesByUserID(itemlist[0].UserId);

                if (dt.Rows.Count > 0)
                {
                    UPM.DeleteAllExistingMapping(itemlist[0].UserId);
                }

                foreach (UserRoleMappingModel i in itemlist)
                {
                    UserRoleMappingModel mm = new UserRoleMappingModel();

                    mm.UserId = i.UserId;
                    mm.RoleId = i.RoleId;
                    mm.InsertUserRoleMappingdata(mm);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Json(true);
        }

        public ActionResult GetRolesMappedToUser(int id)
        {
            UserRoleMappingModel upm = new UserRoleMappingModel();
            DataTable dt = upm.GetAllRolesByUserID(id);

            List<UserRoleMappingModel> viewModelList = new List<UserRoleMappingModel>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                UserRoleMappingModel um = new UserRoleMappingModel();
                um.UserId = int.Parse(dt.Rows[i]["UserId"].ToString());
                um.RoleId = int.Parse(dt.Rows[i]["RoleId"].ToString());

                viewModelList.Add(um);
            }
            return Json(viewModelList);
        }

        #endregion

        #region Role module Mapping region
        public ActionResult RoleModuleMapping()
        {
            if (!ModuleMappingHelper.IsUserMappedToModule(Convert.ToInt32(Session["sessUser"]), Request.Url.AbsoluteUri))
            {
                return RedirectToAction("DashBoard", "Home");
            }
            return View();
        }
        public ActionResult LoadALLRoleToBeMappedWithModule()
        {
            string strProjectMapped = string.Empty;
            RoleMouduleMappingModel USM = new RoleMouduleMappingModel();
            DataTable dt = USM.GetAllRoles();

            foreach (DataRow dr in dt.Rows)
            {
                int i = 0;

                strProjectMapped += "<tr>";
                strProjectMapped += "<td align='center'>" + dr["ID"].ToString() + "</td><td align='center'>" + dr["Name"].ToString() + "</td>";
                strProjectMapped += "<td align='center'><button data-toggle='modal' data-target='#myModalForModule' onclick='ShowPermission(" + dr["Id"].ToString() + ")'>Map</button></td>";
                strProjectMapped += "</td>";
                i++;
            }

            return Content(strProjectMapped);
        }
        public ActionResult LoadAllSysModules()
        {
            RoleMouduleMappingModel obj = new RoleMouduleMappingModel();
            DataTable dt = obj.GetAllModules();
            string strModules = string.Empty;

            foreach (DataRow dr in dt.Rows)
            {
                strModules += "<ul class='module' style='list-style: none; margin:15px;'>";
                strModules += "<li class='limodule' style='list-style: none;margin:10px;'>";
                strModules += "<input type='checkbox' class='check' style=' margin:5px;' name='modules' value='" + dr["ID"].ToString() + "'>" +
                                dr["Name"].ToString();

                strModules += "</li>";
                strModules += "</ul>";

            }
            return Content(strModules);
        }
        public ActionResult GetModuleMappedToRole(int id)
        {
            RoleMouduleMappingModel rmm = new RoleMouduleMappingModel();
            DataTable dt = rmm.GetAllModuleByRoleID(id);

            List<RoleMouduleMappingModel> viewModelList = new List<RoleMouduleMappingModel>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                RoleMouduleMappingModel um = new RoleMouduleMappingModel();
                um.SysModuleId = int.Parse(dt.Rows[i]["SYSModuleID"].ToString());
                um.RoleId = int.Parse(dt.Rows[i]["RoleId"].ToString());

                viewModelList.Add(um);
            }
            return Json(viewModelList);
        }

        public ActionResult SaveModuleMapping(RoleMouduleMappingModel[] itemlist)
        {
            RoleMouduleMappingModel UPM = new RoleMouduleMappingModel();

            try
            {
                if (itemlist == null)
                    return null;

                int RoleID = itemlist[0].RoleId;
                DataTable dt = UPM.GetAllModuleByRoleID(itemlist[0].RoleId);

                // var moduleListIdDatabase = dt.AsEnumerable().ToArray();

                if (dt.Rows.Count > 0)
                {
                    UPM.DeleteAllExistingMapping(itemlist[0].RoleId);
                }

                foreach (RoleMouduleMappingModel i in itemlist)
                {
                    RoleMouduleMappingModel mm = new RoleMouduleMappingModel();

                    mm.RoleId = i.RoleId;
                    mm.SysModuleId = i.SysModuleId;
                    mm.CreatedBy = System.Web.HttpContext.Current.Session["ID"]?.ToString();
                    mm.CreateDate = DateTime.Now;
                    mm.InsertRoleModuleMappingdata(mm);
                }


            }
            catch (Exception)
            {

                throw;
            }
            return Json(true);
        }
        #endregion


        private int GetRowAndColumnCount(int dtRowsCount, int columnCount)
        {
            var divisionResult = dtRowsCount % columnCount;
            var rowCount = dtRowsCount / columnCount;
            int rows;
            if (divisionResult == 0)
            {
                rows = rowCount;
            }
            else
            {
                rows = rowCount + 1;
            }

            return rows;
        }

        #region Organization 
        public ActionResult ManageOrganisation(int id = 0)
        {
            OrganisationModel org = new OrganisationModel();
            try
            {
                if (!ModuleMappingHelper.IsUserMappedToModule(Convert.ToInt32(Session["sessUser"]), Request.Url.AbsoluteUri))
                {
                    return RedirectToAction("DashBoard", "Home");
                }
                if (id > 0)
                {
                    DataTable dt = new DataTable();
                    dt = org.GetOrganisationDataByID(id);
                    org.orgname = dt.Rows[0]["orgname"].ToString();
                    org.url = dt.Rows[0]["url"].ToString();
                    org.address = dt.Rows[0]["address"].ToString();
                    org.contact_email_id = dt.Rows[0]["contact_email_id"].ToString();
                    org.logo = dt.Rows[0]["logo"].ToString();
                    org.isActive = Convert.ToBoolean(dt.Rows[0]["isActive"]);
                    org.createdBy = Convert.ToInt32(System.Web.HttpContext.Current.Session["ID"]);
                    org.createdTS = DateTime.Now;
                }
                else
                {
                    org.orgname = "";
                    org.wikiurl = "";
                    org.url = "";
                    org.logo = "";
                    org.isActive = false;
                }
            }
            catch (Exception exx) { }
            return View(org);
        }
        public ActionResult Organisations()
        {
            OrganisationModel Org = new OrganisationModel();
            string strOrganisation = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                int i = 0;
                dt = Org.GetALLOrganisationData();

                Uri myuri = new Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
                string pathQuery = myuri.PathAndQuery;
                string hostName = myuri.ToString().Replace(pathQuery, "");

                List<OrganisationModel> viewModelList = new List<OrganisationModel>();
                foreach (DataRow dr in dt.Rows)
                {
                    strOrganisation += "<tr><td class='text-center'>" + dr["id"].ToString() + "</td><td class='text-center'>" + dr["orgname"] + "</td>" + "<td class='text-center'>" + dr["address"].ToString() + "</td>" +
                        "<td class='text-center'>" + dr["contact_email_id"].ToString() + "</td>" + "<td class='text-center'>" + dr["isActive"].ToString() + "</td>" +
                       "<td  class='text-center'><a href = 'ManageOrganisation?ID=" + dr["ID"].ToString() + "'>Edit</a></td></tr>";
                    i++;
                }
            }
            catch (Exception exc) { }
            return Content(strOrganisation);

        }
        [HttpPost]
        public ActionResult ManageOrganisation(OrganisationModel orgModel)
        {
            try
            {
                if (orgModel.id > 0)
                {
                    var filelogo = Request.Files["logo"];
                    if (filelogo != null && filelogo.ContentLength > 0)
                    {


                        var logo = Path.GetFileName(filelogo.FileName);
                        logo = orgModel.orgname + "_logo_" + logo;
                        var path = Path.Combine(Server.MapPath("~/images/organisation_logo"), logo);
                        Stream strm = filelogo.InputStream;
                        generateThumbnail.GenerateThumbnails(0.3, strm, path);
                        //filelogo.SaveAs(path);
                        orgModel.logo = logo;
                    }
                    orgModel.wikiurl = Encrypt(orgModel.url);
                    orgModel.editedTS = DateTime.Now;
                    orgModel.editedBy = Convert.ToInt32(System.Web.HttpContext.Current.Session["ID"]);
                    orgModel.updateOrganisation(orgModel);
                }
                else
                {
                    int i;
                    var filelogo = Request.Files["logo"];
                    if (filelogo != null && filelogo.ContentLength > 0)
                    {


                        var logo = Path.GetFileName(filelogo.FileName);
                        logo = orgModel.orgname + "_logo_" + logo;
                        var path = Path.Combine(Server.MapPath("~/images/organisation_logo"), logo);
                        Stream strm = filelogo.InputStream;
                        generateThumbnail.GenerateThumbnails(0.3, strm, path);
                        //filelogo.SaveAs(path);
                        orgModel.logo = logo;
                    }
                    orgModel.wikiurl = Encrypt(orgModel.url);
                    orgModel.createdTS = DateTime.Now;
                    orgModel.createdBy = Convert.ToInt32(System.Web.HttpContext.Current.Session["ID"]);
                    orgModel.insert_OrganisationData(orgModel, out i);
                    //ViewData["ErrStatus"] = orgModel.ISErr.ToString();
                    //ModelState.AddModelError("MSG", orgModel.ErrString);
                }
            }
            catch (Exception exc) { }
            return RedirectToAction("ManageOrganisation");
        }
        private string Encrypt(string clearText)
        {
            try
            {
                string EncryptionKey = "MAKV2SPBNI99212";
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch (Exception exc) { }
            return clearText;
        }
        #endregion

        #region Department managment region
        public ActionResult ManageDepartment(int id = 0)
        {
            ManageDepartmentViewModel departmentVMModel= null;
            if(id> 0)
            {

            }
            else
            {
                if (System.Web.HttpContext.Current.Session["sessUser"] != null)
                {
                    departmentVMModel = new ManageDepartmentViewModel(Convert.ToInt32(Session["sessUser"]));
                }
                else
                    RedirectToAction("DashBoard", "Home");
            }

            return View(departmentVMModel);
        }

        [HttpPost]
        public ActionResult ManageDepartment(ManageDepartmentViewModel model)
        {
           return RedirectToAction("ManageDepartment", "Admin");
        }

        public ActionResult LoadDepartmentsData()
        {
            ManageDepartmentViewModel obj = new ManageDepartmentViewModel();
            string strUserData = string.Empty;
            int i = 0;
            DataTable dt = obj.Department.GetAllDepartments();
            foreach (DataRow dr in dt.Rows)
            {

                strUserData += "<tr><td class='text-center'>" + dr["Id"].ToString() + "</td><td class='text-center'>" + dr["NAME"].ToString() + "</td>" + "<td class='text-center'>" + dr["DESCRIPTION"].ToString() + "</td>" + "<td class='text-center'>" + dr["DepartmentHead"] + "</td>" + "<td class='text-center'>" + dr["OrganisationName"] + "</td>" +
                                     "<td class='text-center'><a href = 'ManageDepartment?ID=" + dr["ID"].ToString() + "'>Edit </a> </td></tr>";
                i++;
            }
            return Content(strUserData);
        }
        #endregion

    }
}