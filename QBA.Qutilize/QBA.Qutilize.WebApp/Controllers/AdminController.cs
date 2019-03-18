using QBA.Qutilize.WebApp.Helper;
using QBA.Qutilize.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace QBA.Qutilize.WebApp.Controllers
{
    public class AdminController : Controller
    {
        readonly int loggedInUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
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

            UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);

            ViewBag.IsUserSysAdmin = userInfo.IsRoleSysAdmin;
            DataTable dtUsers = new DataTable();
            if (userInfo.IsRoleSysAdmin)
            {
                obj.OrganisationList.Clear();
                obj.OrganisationList = obj.GetAllOrgInList();



            }

            else
            {
                obj.OrganisationList.Clear();
                var organisation = obj.GetAllOrgInList().FirstOrDefault(x => x.id == userInfo.UserOrganisationID);
                obj.OrganisationList.Add(organisation);

                obj.UserOrgId = userInfo.UserOrganisationID;

                obj.UsersList.Clear();
                obj.UsersList = obj.GetAllUsersInList(userInfo.UserOrganisationID);
                obj.DepartmentList.Clear();
                obj.DepartmentList = obj.GetAllDepartmentInList(userInfo.UserOrganisationID);
            }


            if (ID > 0)
            {
                try
                {
                    DataTable dt = new DataTable();
                    dt = obj.GetUsersByID(ID);

                    obj.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                    obj.Name = dt.Rows[0]["Name"].ToString();
                    obj.UserName = dt.Rows[0]["UserName"].ToString();
                    obj.EmailId = dt.Rows[0]["EmailId"]?.ToString();
                    obj.Designation = dt.Rows[0]["Designation"].ToString();
                    if (dt.Rows[0]["ManagerId"] != DBNull.Value)
                    {
                        obj.ManagerId = Convert.ToInt32(dt.Rows[0]["ManagerId"]);
                    }

                    obj.ContactNo = dt.Rows[0]["PhoneNo"]?.ToString();
                    obj.AlterNetContactNo = dt.Rows[0]["AlternateConatctNo"]?.ToString();

                    if (dt.Rows[0]["BirthDate"] != DBNull.Value)
                    {
                        obj.BirthDate = Convert.ToDateTime(dt.Rows[0]["BirthDate"]).Date;
                    }
                    else
                        obj.BirthDate = null;

                    obj.Gender = dt.Rows[0]["Gender"]?.ToString();
                    obj.IsActive = Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString());

                    if (dt.Rows[0]["OrgID"] != DBNull.Value)
                    {
                        obj.UserOrgId = Convert.ToInt32(dt.Rows[0]["OrgID"]);
                    }


                    obj.UsersList.Clear();
                    obj.UsersList = obj.GetAllUsersInList(obj.UserOrgId);
                    obj.DepartmentList.Clear();
                    obj.DepartmentList = obj.GetAllDepartmentInList(obj.UserOrgId);

                    foreach (DataRow item in dt.Rows)
                    {
                        obj.DepartmentIds.Add(Convert.ToInt32(item["DeptID"]));
                        obj.DepartmentIdsInString += item["DeptID"].ToString() + ",";
                    }


                    obj.DepartmentIdsInString = obj.DepartmentIdsInString.TrimEnd(',');

                }
                catch (Exception ex)
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
                    if (ValidateRequest)
                    {
                        model.EditedBy = System.Web.HttpContext.Current.Session["sessUser"].ToString();
                        model.EditedDate = DateTime.Now;
                        um.Update_UserDetails(model);
                        TempData["ErrStatus"] = model.ISErr.ToString();
                    }

                }
                else
                {
                    if (System.Web.HttpContext.Current.Session["sessUser"] != null)
                    {
                        model.CreatedBy = System.Web.HttpContext.Current.Session["sessUser"].ToString();
                    }

                    model.CreateDate = DateTime.Now;
                    string password = model.Password;
                    password = EncryptionHelper.ConvertStringToMD5(password);
                    model.IsActive = model.IsActive;
                    model.Password = password;

                    um.InsertUserdata(model, out int id);
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
            UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);
            DataTable dtUsers = new DataTable();
            if (userInfo.IsRoleSysAdmin)
            {
                dtUsers = obj.GetAllUsers();
                foreach (DataRow dr in dtUsers.Rows)
                {

                    string status = Convert.ToBoolean(dr["IsActive"]) == true ? "Active" : "In Active";
                    strUserData += "<tr><td class='text-center'>" + dr["Id"].ToString() + "</td><td class='text-center'>" + dr["UserName"].ToString() + "</td>" + "<td class='text-center'>" + dr["Name"].ToString() + "</td>" + "<td class='text-center'>" + dr["EmailId"] + "</td>" +
                                    "<td class='text-center'>" + dr["Designation"] + "</td>" + "<td class='text-center'>" + dr["ManagerName"] + "</td>" + "<td class='text-center'>" + dr["orgname"].ToString() + "</td>" + "<td class='text-center'>" + status + "</td>" + "<td class='text-center'><a href = 'ManageUsers?ID=" + dr["ID"].ToString() + "'>Edit </a> </td></tr>";
                    i++;
                }
            }
            else
            {
                dtUsers = obj.GetAllUsers(userInfo.UserOrganisationID);
                foreach (DataRow dr in dtUsers.Rows)
                {
                    string status = Convert.ToBoolean(dr["IsActive"]) == true ? "Active" : "In Active";
                    strUserData += "<tr><td class='text-center'>" + dr["Id"].ToString() + "</td><td class='text-center'>" + dr["UserName"].ToString() + "</td>" + "<td class='text-center'>" + dr["Name"].ToString() + "</td>" + "<td class='text-center'>" + dr["EmailId"] + "</td>" +
                                    "<td class='text-center'>" + dr["Designation"] + "</td>" + "<td class='text-center'>" + dr["ManagerName"] + "</td>" + "<td class='text-center'>" + status + "</td>" + "<td class='text-center'><a href = 'ManageUsers?ID=" + dr["ID"].ToString() + "'>Edit </a> </td></tr>";
                    i++;
                }
            }

            return Content(strUserData);
        }

        public ActionResult GetManagers(int orgId)
        {
            UserModel user = new UserModel();
            string strUserData = string.Empty;
            try
            {
                var listUsers = user.GetAllUsersInList(orgId);
                strUserData += "<option value = 0>Please select</option>";
                foreach (UserModel item in listUsers)
                {
                    strUserData += "<option value=" + Convert.ToInt32(item.ID) + ">" + item.Name + "</option>";
                }
            }
            catch (Exception)
            {

                throw;
            }

            return Json(strUserData);
        }

        public ActionResult GetDepartments(int orgId)
        {
            UserModel user = new UserModel();
            string strDeptData = string.Empty;
            try
            {
                var listUsers = user.GetAllDepartmentInList(orgId);
                strDeptData += "<option value = 0>Please select</option>";
                foreach (DepartmentModel item in listUsers)
                {
                    strDeptData += "<option value=" + Convert.ToInt32(item.DepartmentID) + ">" + item.Name + "</option>";
                }
            }
            catch (Exception)
            {

                throw;
            }

            return Json(strDeptData);
        }

        public ActionResult Checkemail(string email, int orgId)
        {
            UserModel _um = new UserModel();
            DataTable dt = _um.checkemail(email, orgId);
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

            var loggedInUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
            UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);
            DataTable dt = new DataTable();

            if (userInfo.IsRoleSysAdmin)
            {
                dt = obj.GetAllProjects();
                int i = 0;
                foreach (DataRow item in dt.Rows)
                {

                    string status = Convert.ToBoolean(item["IsActive"]) == true ? "Active" : "In Active";
                    var departmentName = (item["DepartmentName"] == DBNull.Value) ? "" : item["DepartmentName"].ToString();
                    strUserData += "<tr><td class='text-center'>" + item["Id"].ToString() + "</td><td class='text-center'>" + item["Name"].ToString() + "</td>" + "<td class='text-center'>" + item["Description"].ToString() + "<td class='text-center'>" + item["DepartmentName"].ToString() +
                                   "<td class='text-center'>" + item["OrgName"].ToString() + "<td class='text-center'>" + status + " </ td > " +
                                    "<td class='text-center'><a href = 'ManageProject?ID=" + item["ID"].ToString() + "'>Edit </a> </td></tr>";
                    i++;

                }
            }
            else
            {
                int i = 0;
                dt = obj.GetAllProjects(userInfo.UserOrganisationID);
                foreach (DataRow dr in dt.Rows)
                {
                    string status = Convert.ToBoolean(dr["IsActive"]) == true ? "Active" : "In Active";
                    var departmentName = (dr["DepartmentName"] == DBNull.Value) ? "" : dr["DepartmentName"].ToString();

                    strUserData += "<tr><td class='text-center'>" + dr["Id"].ToString() + "</td><td class='text-center'>" + dr["Name"].ToString() + "</td>" + "<td class='text-center'>" + dr["Description"].ToString() + "</td>" + "<td class='text-center'>" + departmentName + "</td>" +

                                    "<td class='text-center'>" + dr["OrgName"].ToString() + "<td class='text-center'>" + status + " </td > " +
                                    "<td class='text-center'><a href = 'ManageProject?ID=" + dr["ID"].ToString() + "'>Edit </a> </td></tr>";
                    i++;
                }

            }

            return Content(strUserData);
        }
        public ActionResult ManageProject(int ID = 0)
        {
            ProjectModel obj = new ProjectModel();
            List<ProjectModel> objRole = new List<ProjectModel>();

            var loggedInUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
            UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);

            if (userInfo.IsRoleSysAdmin)
            {
                obj.DepartmentList = obj.GetDepartments();
            }
            else
            {
                obj.DepartmentList = obj.GetDepartments(userInfo.UserOrganisationID);
            }
            if (ID > 0)
            {
                try
                {
                    DataTable dt = new DataTable();
                    dt = obj.GetProjectByID(ID);
                    obj.ProjectID = Convert.ToInt32(dt.Rows[0]["ID"]);
                    obj.ProjectName = dt.Rows[0]["Name"].ToString();
                    obj.Description = dt.Rows[0]["Description"].ToString();
                    if (dt.Rows[0]["DepartmentID"] != null)
                    {
                        obj.DepartmentID = Convert.ToInt32(dt.Rows[0]["DepartmentID"]);

                    }
                    obj.MaxProjectTimeInHours = Convert.ToInt32(dt.Rows[0]["MaxProjectTimeInHours"]);
                    obj.IsActive = Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString());

                    //obj.Update_ProjectDetails(obj);
                    //TempData["ErrStatus"] = obj.ISErr.ToString();
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
            try
            {
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
            }
            catch (Exception ex)
            {
                TempData["ErrStatus"] = model.ISErr.ToString();
                throw;
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
            var loggedInUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
            UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);

            UserProjectMappingModel USM = new UserProjectMappingModel();
            DataTable dt = new DataTable();
            if (userInfo.IsRoleSysAdmin)
            {
                dt = USM.GetAllUsers();
            }
            else
            {
                dt = USM.GetAllUsers(userInfo.UserOrganisationID);
            }
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int i = 0; //

                    strProjectMapped += "<tr>";
                    strProjectMapped += "<td align='center'>" + dr["ID"].ToString() + "</td><td align='center'>" + dr["Name"].ToString() + "</td> " + "<td align='center'>" + dr["orgname"].ToString() + "</td> ";
                    strProjectMapped += "<td align='center'><button data-toggle='modal' data-target='#myModalForModule' onclick='ShowPermission(" + dr["ID"].ToString() + ")'>Map</button></td>";
                    strProjectMapped += "</td>";
                    i++;
                }
            }


            return Content(strProjectMapped);
        }

        //public ActionResult LoadAllModules()
        //{
        //    UserProjectMappingModel obj = new UserProjectMappingModel();
        //    var loggedInUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
        //    UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);

        //    DataTable dt = new DataTable();

        //    if (userInfo.IsRoleSysAdmin)
        //    {
        //        dt = obj.GetAllProjects();
        //    }
        //    else
        //    {
        //        dt = obj.GetAllProjects(userInfo.UserOrganisationID);
        //    }

        //    string strModules = @"<div id='divProjectList' class='row' style='margin:10px;'>";
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        strModules += @"<div style='float: left;width: 25%; padding: 5px;'>";
        //        strModules += "<input type='checkbox' class='check' style=' margin:5px;' name='modules' value='" + dr["Id"].ToString() + "'>" + dr["Name"].ToString();
        //        strModules += "</div>";
        //    }
        //    strModules += @"</div>";

        //    return Content(strModules);
        //}



        public ActionResult LoadAllModules(int Userid)
        {
            UserProjectMappingModel obj = new UserProjectMappingModel();
            //var loggedInUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
            //UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);

            UserInfoHelper userInfo = new UserInfoHelper(Userid);
            DataTable dt = new DataTable();

            if (userInfo.IsRoleSysAdmin)
            {
                dt = obj.GetAllProjects();
            }
            else
            {
                dt = obj.GetAllProjects(userInfo.UserOrganisationID);
            }


            if (Session["AllProjectList"] != null)
            {
                Session.Remove("AllProjectList");
                Session["AllProjectList"] = dt;
            }
            else
            {
                Session["AllProjectList"] = dt;
            }


            string strModules = @"<div id='divProjectList' class='row' style='margin:10px;'>";
            foreach (DataRow dr in dt.Rows)
            {
                strModules += @"<div style='float: left;width: 25%; padding: 5px;'>";
                strModules += "<input type='checkbox' class='check' style=' margin:5px;' name='modules' value='" + dr["Id"].ToString() + "'>" + dr["Name"].ToString();
                strModules += "</div>";
            }
            strModules += @"</div>";

            return Json(strModules);
        }


        public ActionResult SearchProject(string searchFilter)
        {
            //ViewBag.ProjectListDataTable
            string strModules = String.Empty;
            if (Session["AllProjectList"] != null)
            {
                DataTable dataTable = (DataTable)Session["AllProjectList"];

                if (dataTable.Rows.Count > 0)
                {

                    var datarow = dataTable.Select("Name like '" + searchFilter + "%'");

                    if (datarow.Length > 0)
                    {
                        DataTable dt = datarow.CopyToDataTable();
                        strModules = @"<div id='divProjectList' class='row' style='margin:10px;'>";
                        foreach (DataRow dr in dt.Rows)
                        {
                            strModules += @"<div style='float: left;width: 25%; padding: 5px;'>";
                            strModules += "<input type='checkbox' class='check' style=' margin:5px;' name='modules' value='" + dr["Id"].ToString() + "'>" + dr["Name"].ToString();
                            strModules += "</div>";
                        }
                        strModules += @"</div>";
                    }
                }
            }
            return Json(strModules);
        }
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

                if (Session["AllProjectList"] != null)
                {
                    Session.Remove("AllProjectList");
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
            obj.OrganisationList = obj.GetAllOrgInList();

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
                    obj.RolesOrgID = Convert.ToInt32(dt.Rows[0]["OrgId"]);

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
                    model.EditedBy = System.Web.HttpContext.Current.Session["sessUser"]?.ToString();
                    model.EditedDate = DateTime.Now;
                    rm.Update_RoleDetails(model);

                    TempData["ErrStatus"] = model.ISErr.ToString();
                }
                else
                {
                    int id;
                    if (System.Web.HttpContext.Current.Session["sessUser"] != null)
                    {
                        model.CreatedBy = (System.Web.HttpContext.Current.Session["sessUser"].ToString());
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
            DataTable dtRoles = new DataTable();
            string strUserData = string.Empty;
            try
            {
                UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);
                RoleModel obj = new RoleModel();
                if (userInfo.IsRoleSysAdmin)
                {
                    dtRoles = obj.GetAllRoles();
                }
                else
                {
                    dtRoles = obj.GetAllRoles(userInfo.UserOrganisationID);
                }

                int i = 0;

                foreach (DataRow dr in dtRoles.Rows)
                {
                    var Status = Convert.ToBoolean(dr["isActive"]) == true ? "Active" : "InActive";

                    strUserData += "<tr><td class='text-center'>" + dr["Id"].ToString() + "</td><td class='text-center'>" + dr["Name"].ToString() + "</td>" + "<td class='text-center'>" + dr["Description"].ToString() + "</td>" + "<td class='text-center'>" + dr["orgname"].ToString() + "</td>" +
                                     "<td class='text-center'>" + Status + "</td><td class='text-center'><a href = 'ManageRole?ID=" + dr["ID"].ToString() + "'>Edit </a> </td></tr>";
                    i++;
                }
            }
            catch (Exception ex)
            {

                throw;
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
            UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);
            DataTable dtUsers = new DataTable();
            try
            {
                if (userInfo.IsRoleSysAdmin)
                {
                    dtUsers = USM.GetAllUsers();
                }
                else
                {
                    dtUsers = USM.GetAllUsers(userInfo.UserOrganisationID);
                }

                foreach (DataRow dr in dtUsers.Rows)
                {
                    int i = 0;

                    strProjectMapped += "<tr>";
                    strProjectMapped += "<td align='center'>" + dr["ID"].ToString() + "</td><td align='center'>" + dr["Name"].ToString() + "</td>" + "</td><td align='center'>" + dr["orgname"].ToString() + "</td>";
                    strProjectMapped += "<td align='center'><button data-toggle='modal' data-target='#myModalForModule' onclick='ShowRoleMapped(" + dr["ID"].ToString() + ")'>Map</button></td>";

                    strProjectMapped += "</td>";
                    i++;
                }
            }
            catch (Exception)
            {

                throw;
            }


            return Content(strProjectMapped);
        }

        public ActionResult LoadAllRoles(int userID)
        {
            UserRoleMappingModel obj = new UserRoleMappingModel();

            UserInfoHelper userInfo = new UserInfoHelper(userID);

            DataTable dtRoles = new DataTable();

            if (userInfo.IsRoleSysAdmin)
            {
                dtRoles = obj.GetAllRoles();
            }
            else
            {
                dtRoles = obj.GetAllRoles(userInfo.UserOrganisationID);
            }


            string strModules = @"<div id='divProjectList' class='row' style='margin:10px;'>";
            foreach (DataRow dr in dtRoles.Rows)
            {
                strModules += @"<div style='float: left;width: 25%; padding: 5px;'>";
                strModules += "<input type='checkbox' class='check' style=' margin:5px;' name='modules' value='" + dr["Id"].ToString() + "'>" + dr["Name"].ToString();
                strModules += "</div>";
            }
            strModules += @"</div>";

            return Json(strModules);
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
                    org.createdBy = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
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
                    orgModel.editedBy = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
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
                    orgModel.createdBy = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
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

        #region Module
        public ActionResult ManageModule(int id = 0)
        {
            ModuleModel mm = new ModuleModel();
            try
            {

                DataTable dts = new DataTable();
                dts = mm.GetAllModules();

                List<ModuleModel> objModule = new List<ModuleModel>();
                int o = dts.Rows.Count;
                objModule.Add(new ModuleModel { ID = 0, Name = "Select" });
                for (int j = 0; j < o; j++)
                {
                    int d3 = Convert.ToInt32(dts.Rows[j]["ID"]);
                    string d4 = dts.Rows[j]["Name"].ToString();
                    objModule.Add(new ModuleModel { ID = d3, Name = d4 });
                }
                SelectList objListOfModuleToBind = new SelectList(objModule, "ID", "Name", 0);
                mm.ModuleList = objListOfModuleToBind;

                if (id > 0)
                {
                    DataTable dt = new DataTable();
                    dt = mm.GetModuleByID(id);
                    mm.Name = dt.Rows[0]["Name"].ToString();
                    mm.URL = dt.Rows[0]["URL"].ToString();



                    if (dt.Rows[0]["ParentID"].ToString() != "")
                    {
                        mm.ParentID = int.Parse(dt.Rows[0]["ParentID"].ToString());
                    }
                    else
                    {
                        mm.ParentID = 0;
                    }
                    mm.Description = dt.Rows[0]["Description"].ToString();
                    mm.DisplayCSS = dt.Rows[0]["DisplayCSS"].ToString();
                    mm.DisplayIcon = dt.Rows[0]["DisplayIcon"].ToString();
                    mm.DisplayName = dt.Rows[0]["DisplayName"].ToString();
                    mm.Rank = int.Parse(dt.Rows[0]["Rank"].ToString());
                    mm.isActive = Convert.ToBoolean(dt.Rows[0]["isActive"]);
                }
                else
                {
                    mm.Name = "";
                    mm.URL = "";
                    mm.ParentID = 0;
                    mm.Description = "";
                    mm.DisplayName = "";
                    mm.DisplayCSS = "";
                    mm.DisplayIcon = "";

                }
            }
            catch (Exception exx)
            {

            }

            return View(mm);
        }

        public ActionResult Modules()
        {
            ModuleModel mm = new ModuleModel();
            string strOrganisation = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                int i = 0;
                dt = mm.GetAllModules();

                Uri myuri = new Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
                string pathQuery = myuri.PathAndQuery;
                string hostName = myuri.ToString().Replace(pathQuery, "");

                List<OrganisationModel> viewModelList = new List<OrganisationModel>();
                foreach (DataRow dr in dt.Rows)
                {
                    strOrganisation += "<tr><td class='text-center'>" + dr["id"].ToString() + "</td><td class='text-center'>" + dr["Name"] + "</td>" + "<td class='text-center'>" + dr["URL"].ToString() + "</td>" +
                        "<td class='text-center'>" + dr["DisplayName"].ToString() + "</td>" + "<td class='text-center'><i class='" + dr["DisplayCSS"].ToString() + "'></i>  " + dr["DisplayCSS"].ToString() + "</td>" + "<td class='text-center'>" + dr["isActive"].ToString() + "</td>" +
                       "<td  class='text-center'><a href = 'ManageModule?ID=" + dr["ID"].ToString() + "'>Edit</a></td></tr>";
                    i++;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return Content(strOrganisation);

        }


        [HttpPost]
        public ActionResult ManageModule(ModuleModel model)
        {
            ModuleModel obj = new ModuleModel();
            if (model.ID > 0)
            {
                try
                {
                    obj = model;
                    obj.EditedBy = Convert.ToInt32(Session["sessUser"]);
                    obj.EditedTS = Convert.ToDateTime(DateTime.Now.ToString());
                    obj.isActive = Convert.ToBoolean(model.isActive);

                    obj.Update_ModuleDetails(obj);

                    TempData["ErrStatus"] = obj.ISErr.ToString();
                }
                catch
                {

                }
            }
            else
            {

                model.AddedBy = int.Parse(Session["sessUser"].ToString());
                model.AddedTS = DateTime.Now;

                model.isActive = Convert.ToBoolean(model.isActive);

                obj.InsertModuledata(model, out int id);
                if (id > 0)
                {

                }
                TempData["ErrStatus"] = model.ISErr.ToString();
            }
            return RedirectToAction("ManageModule", "Admin");

        }


        #endregion

        #region Department managment region
        public ActionResult ManageDepartment(int id = 0)
        {
            ManageDepartmentViewModel departmentVMModel = null;
            DepartmentModel departmentModel = new DepartmentModel();
            if (id > 0)
            {
                departmentVMModel = new ManageDepartmentViewModel(Convert.ToInt32(Session["sessUser"]));
                DataTable dt = new DataTable();
                dt = departmentModel.GetDepartmentByID(id);

                departmentVMModel.Department.DepartmentID = Convert.ToInt32(dt.Rows[0]["ID"]);
                departmentVMModel.Department.DepartmentCode = dt.Rows[0]["CODE"].ToString();
                departmentVMModel.Department.Name = dt.Rows[0]["Name"].ToString();
                if (dt.Rows[0]["DESCRIPTION"] != System.DBNull.Value)
                {
                    departmentVMModel.Department.Description = dt.Rows[0]["DESCRIPTION"].ToString();
                }
                if (dt.Rows[0]["DeptHeadID"] != System.DBNull.Value)
                {
                    departmentVMModel.Department.DepartmentHeadId = Convert.ToInt32(dt.Rows[0]["DeptHeadID"]);
                }

                departmentVMModel.Department.OrganisationID = Convert.ToInt32(dt.Rows[0]["ORGID"]);
                if (departmentVMModel.Department.OrganisationID > 0)
                {
                    DataTable dtUsers = ManageDepartmentViewModel.GetUsersByOrganisation(departmentVMModel.Department.OrganisationID);

                    if (dtUsers.Rows.Count > 0)
                    {
                        foreach (DataRow item in dtUsers.Rows)
                        {
                            departmentVMModel.Users.Add(new UserModel { ID = Convert.ToInt32(item["Id"]), Name = item["Name"].ToString() });
                        }
                    }

                }

                departmentVMModel.Department.IsActive = Convert.ToBoolean(dt.Rows[0]["isACTIVE"].ToString());
                departmentVMModel.Department.EditedBy = Convert.ToInt32(Session["sessUser"]);
                departmentVMModel.Department.EditedTS = DateTime.Now;

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
            DepartmentModel dm = new DepartmentModel();
            try
            {
                if (model.Department.DepartmentID > 0)
                {

                    model.Department.EditedBy = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
                    model.Department.EditedTS = DateTime.Now;
                    dm.UpdateDepartmentDetails(model.Department);

                    TempData["ErrStatus"] = model.ISErr.ToString();
                }
                else
                {
                    if (System.Web.HttpContext.Current.Session["sessUser"] != null)
                    {
                        model.Department.CreatedBy = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
                    }
                    model.Department.CreatedTS = DateTime.Now;
                    dm.InsertDepartmentdata(model.Department, out int id);

                    if (id > 0)
                    {

                    }
                    TempData["ErrStatus"] = model.Department.ISErr.ToString();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction("ManageDepartment", "Admin");
        }

        public ActionResult LoadDepartmentsData()
        {
            ManageDepartmentViewModel obj = new ManageDepartmentViewModel(Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]));
            string strUserData = string.Empty;
            int i = 0;
            DataTable dt = new DataTable();
            if (obj.IsRoleSysAdmin == true)
            {
                dt = obj.Department.GetAllDepartments();

            }
            else
            {
                dt = obj.Department.GetAllDepartments(obj.UserOrganisationID);

            }

            foreach (DataRow dr in dt.Rows)
            {
                var status = string.Empty;
                if (Convert.ToBoolean(dr["isACTIVE"]))
                    status = "Active";
                else
                    status = "InActive";

                strUserData += "<tr><td class='text-center'>" + dr["Id"].ToString() + "</td><td class='text-center'>" + dr["NAME"].ToString() + "</td>" + "<td class='text-center'>" + dr["DESCRIPTION"].ToString() + "</td>" + "<td class='text-center'>" + dr["DepartmentHead"] + "</td>" + "<td class='text-center'>" + dr["OrganisationName"] + "</td>" +
                                        "<td class='text-center'>" + status + "</td>" + "<td class='text-center'><a href = 'ManageDepartment?ID=" + dr["ID"].ToString() + "'>Edit </a> </td></tr>";
                i++;
            }
            return Content(strUserData);
        }

        public ActionResult GetUserByOrgId(int orgId)
        {
            ManageDepartmentViewModel obj = new ManageDepartmentViewModel();

            DataTable dt = ManageDepartmentViewModel.GetUsersByOrganisation(orgId);

            string strUserData = string.Empty;
            strUserData += "<option value = 0>Please select</option>";
            foreach (DataRow item in dt.Rows)
            {
                strUserData += "<option value=" + Convert.ToInt32(item["Id"]) + ">" + item["Name"].ToString() + "</option>";
            }
            return Json(strUserData);
        }
        #endregion

    }
}