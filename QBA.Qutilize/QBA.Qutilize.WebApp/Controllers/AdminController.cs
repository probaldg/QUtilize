

using Newtonsoft.Json;
using OfficeOpenXml;
using QBA.Qutilize.WebApp.Helper;
using QBA.Qutilize.WebApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;



namespace QBA.Qutilize.WebApp.Controllers
{
    public class AdminController : Controller
    {
      
        readonly int loggedInUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
        ImageCompress generateThumbnail = new ImageCompress();
        UserModel um = new UserModel();
        string strTaskData = string.Empty;
        String strIssueData = string.Empty;
        string strStatusData = String.Empty;
        string strspace = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp";
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        #region User managment region

        public ActionResult ManageUsers(int ID = 0)
        {
            UserModel obj = new UserModel();
            MasterMaritalStatusModel maritalstatusModel = new MasterMaritalStatusModel();
            MasterFunctionalRoleModel functionalRoleModel = new MasterFunctionalRoleModel();
            
            UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);

            ViewBag.IsUserSysAdmin = userInfo.IsRoleSysAdmin;
            DataTable dtUsers = new DataTable();
            if (userInfo.IsRoleSysAdmin)
            {
                obj.OrganisationList.Clear();
                obj.OrganisationList = obj.GetAllOrgInList().Where(x => x.isActive == true).ToList();
            }

            else
            {
                obj.OrganisationList.Clear();
                var organisation = obj.GetAllOrgInList().FirstOrDefault(x => x.id == userInfo.UserOrganisationID && x.isActive == true);
                obj.OrganisationList.Add(organisation);

                obj.UserOrgId = userInfo.UserOrganisationID;

                obj.UsersList.Clear();
                obj.UsersList = obj.GetAllUsersInList(userInfo.UserOrganisationID).Where(x => x.IsActive == true).ToList();
                obj.UsersList_II.Clear();
                obj.UsersList_II = obj.GetAllUsersInList(userInfo.UserOrganisationID).Where(x => x.IsActive == true).ToList();
                obj.UsersList_III.Clear();
                obj.UsersList_III = obj.GetAllUsersInList(userInfo.UserOrganisationID).Where(x => x.IsActive == true).ToList();

                obj.DepartmentList.Clear();
                obj.DepartmentList = obj.GetAllDepartmentInList(userInfo.UserOrganisationID).Where(x => x.IsActive == true).ToList();

                obj.MaritalStatusList = maritalstatusModel.Get_MaritalStatus(userInfo.UserOrganisationID).Where(x => x.IsActive == true).ToList();
                obj.FunctionalRoleList= functionalRoleModel.Get_FunctinalRoleDetl(userInfo.UserOrganisationID).Where(x => x.IsActive == true).ToList();
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
                    if (dt.Rows[0]["UserCode"] != null)
                    {
                        obj.UserCode = dt.Rows[0]["UserCode"].ToString();
                    }

                    obj.EmailId = dt.Rows[0]["EmailId"]?.ToString();
                    obj.Designation = dt.Rows[0]["Designation"].ToString();
                    if (dt.Rows[0]["ManagerId"] != DBNull.Value)
                    {
                        obj.ManagerId = Convert.ToInt32(dt.Rows[0]["ManagerId"]);
                    }
                    if (dt.Rows[0]["ManagerId_II"] != DBNull.Value)
                    {
                        obj.ManagerId_II = Convert.ToInt32(dt.Rows[0]["ManagerId_II"]);
                    }
                    if (dt.Rows[0]["ManagerId_III"] != DBNull.Value)
                    {
                        obj.ManagerId_III = Convert.ToInt32(dt.Rows[0]["ManagerId_III"]);
                    }
                    obj.ContactNo = dt.Rows[0]["PhoneNo"]?.ToString();
                    if (dt.Rows[0]["AlternateConatctNo"] != DBNull.Value)
                    {
                        obj.AlterNetContactNo = dt.Rows[0]["AlternateConatctNo"]?.ToString();
                    }


             

                    if (dt.Rows[0]["BirthDate"] != DBNull.Value)
                    {
                        var convertedString = dt.Rows[0]["BirthDate"].ToString();
                        obj.birthDayToDisplay = Convert.ToDateTime(dt.Rows[0]["BirthDate"]).ToShortDateString().Replace('-', '/');
                    }
                    else
                    {
                        obj.birthDayToDisplay = "";
                    }
                    if (dt.Rows[0]["FunctionalRole"] != DBNull.Value)
                    {
                        obj.FunctionalRoleId = Convert.ToInt32(dt.Rows[0]["FunctionalRole"]);
                    }
                    if (dt.Rows[0]["MaritalStatus"] != DBNull.Value)
                    {
                        obj.MaritalStatusID = Convert.ToInt32(dt.Rows[0]["MaritalStatus"]);
                    }
                    if (dt.Rows[0]["AnniversaryDate"] != DBNull.Value)
                    {
                        var convertedString = dt.Rows[0]["AnniversaryDate"].ToString();
                        obj.AnniversaryDateDisplay = Convert.ToDateTime(dt.Rows[0]["AnniversaryDate"]).ToShortDateString().Replace('-', '/');
                    }
                    else
                    {
                        obj.AnniversaryDateDisplay = "";
                    }
                    if (dt.Rows[0]["ExitDate"] != DBNull.Value)
                    {
                        var convertedString = dt.Rows[0]["ExitDate"].ToString();
                        obj.ExitDateDisplay = Convert.ToDateTime(dt.Rows[0]["ExitDate"]).ToShortDateString().Replace('-', '/');
                    }
                    else
                    {
                        obj.ExitDateDisplay = "";
                    }

                    if (dt.Rows[0]["JoiningDate"] != DBNull.Value)
                    {
                        var convertedString = dt.Rows[0]["JoiningDate"].ToString();
                        obj.JoiningDateDisplay = Convert.ToDateTime(dt.Rows[0]["JoiningDate"]).ToShortDateString().Replace('-', '/');
                    }
                    else
                    {
                        obj.JoiningDateDisplay = "";
                    }
                    obj.UserCost = Convert.ToDouble(dt.Rows[0]["UserCostMonthly"]);
                    obj.Gender = dt.Rows[0]["Gender"]?.ToString();
                    obj.IsActive = Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString());

                    if (dt.Rows[0]["OrgID"] != DBNull.Value)
                    {
                        obj.UserOrgId = Convert.ToInt32(dt.Rows[0]["OrgID"]);
                    }


                    obj.UsersList.Clear();
                    obj.UsersList = obj.GetAllUsersInList(obj.UserOrgId).Where(x => x.IsActive == true).ToList();
                    obj.DepartmentList.Clear();
                    obj.DepartmentList = obj.GetAllDepartmentInList(obj.UserOrgId).Where(x => x.IsActive == true).ToList();

                    if (obj.DepartmentList.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            obj.DepartmentIds.Add(Convert.ToInt32(item["DepartmentId"]));
                            obj.DepartmentIdsInString += item["DepartmentId"].ToString() + ",";
                        }
                        obj.DepartmentIdsInString = obj.DepartmentIdsInString.TrimEnd(',');

                    }




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
                        if (model.birthDayToDisplay != "")
                        {
                            DateTime dateTimeConverted;
                            if (DateTime.TryParse(model.birthDayToDisplay, out dateTimeConverted))
                            {
                                model.BirthDate = dateTimeConverted;
                            }
                            else
                            {
                                model.birthDayToDisplay.Replace('-', '/');
                                var stringDateArray = model.birthDayToDisplay.Split('/');

                                var newBirthDayString = stringDateArray[1] + "/" + stringDateArray[0] + "/" + stringDateArray[2];
                                DateTime newDate = Convert.ToDateTime(newBirthDayString);
                                model.BirthDate = newDate;
                            }


                        }
                        if (model.AnniversaryDateDisplay != null)
                        {
                            DateTime dateTimeConverted;
                            if (DateTime.TryParse(model.AnniversaryDateDisplay, out dateTimeConverted))
                            {
                                model.AnniversaryDate = dateTimeConverted;
                            }
                            else
                            {
                                model.AnniversaryDateDisplay.Replace('-', '/');
                                var stringDateArray = model.AnniversaryDateDisplay.Split('/');

                                var newAnniversaryDateString = stringDateArray[1] + "/" + stringDateArray[0] + "/" + stringDateArray[2];
                                DateTime newDate = Convert.ToDateTime(newAnniversaryDateString);
                                model.AnniversaryDate = newDate;
                            }

                        }
                       
                        if (model.JoiningDateDisplay != null)
                        {
                            DateTime dateTimeConverted;
                            if (DateTime.TryParse(model.JoiningDateDisplay, out dateTimeConverted))
                            {
                                model.DOJ = dateTimeConverted;
                            }
                            else
                            {
                                model.JoiningDateDisplay.Replace('-', '/');
                                var stringDateArray = model.JoiningDateDisplay.Split('/');

                                var newJoiningDtString = stringDateArray[1] + "/" + stringDateArray[0] + "/" + stringDateArray[2];
                                DateTime newDate = Convert.ToDateTime(newJoiningDtString);
                                model.DOJ = newDate;
                            }

                        }
                        if (model.ExitDateDisplay != null)
                        {
                            DateTime dateTimeConverted;
                            if (DateTime.TryParse(model.ExitDateDisplay, out dateTimeConverted))
                            {
                                model.ExitDate = dateTimeConverted;
                            }
                            else
                            {
                                model.ExitDateDisplay.Replace('-', '/');
                                var stringDateArray = model.ExitDateDisplay.Split('/');

                                var newExitdtDtString = stringDateArray[1] + "/" + stringDateArray[0] + "/" + stringDateArray[2];
                                DateTime newDate = Convert.ToDateTime(newExitdtDtString);
                                model.ExitDate = newDate;
                            }

                        }


                        model.EditedBy = System.Web.HttpContext.Current.Session["sessUser"].ToString();
                        model.EditedDate = DateTime.Now;
                        bool result = um.Update_UserDetails(model);
                        if (result)
                        {
                            TempData["ErrStatus"] = model.ISErr;
                            TempData["ErrMsg"] = model.ErrString;
                        }
                        else
                        {
                            TempData["ErrStatus"] = model.ISErr;
                            TempData["ErrMsg"] = model.ErrString;
                        }


                    }

                }
                else
                {
                    if (ValidateRequest)
                    {
                        if (System.Web.HttpContext.Current.Session["sessUser"] != null)
                        {
                            model.CreatedBy = System.Web.HttpContext.Current.Session["sessUser"].ToString();
                        }

                        if (model.birthDayToDisplay != "")
                        {
                            var stringDateArray = model.birthDayToDisplay.Split('/');
                            DateTime dob = new DateTime(Convert.ToInt32(stringDateArray[2]), Convert.ToInt16(stringDateArray[0]), Convert.ToInt16(stringDateArray[1]));
                            model.BirthDate = dob;
                        }
                        if (model.AnniversaryDateDisplay != null)
                        {
                            var stringDateArray = model.AnniversaryDateDisplay.Split('/');
                            DateTime AnniversaryDate = new DateTime(Convert.ToInt32(stringDateArray[2]), Convert.ToInt16(stringDateArray[0]), Convert.ToInt16(stringDateArray[1]));
                            model.AnniversaryDate = AnniversaryDate;
                        }
                        if (model.JoiningDateDisplay != null)
                        {
                            var stringDateArray = model.JoiningDateDisplay.Split('/');
                            DateTime JoinningDate = new DateTime(Convert.ToInt32(stringDateArray[2]), Convert.ToInt16(stringDateArray[0]), Convert.ToInt16(stringDateArray[1]));
                            model.DOJ = JoinningDate;
                        }
                        if (model.ExitDateDisplay != null)
                        {
                            var stringDateArray = model.JoiningDateDisplay.Split('/');
                            DateTime ExitDt = new DateTime(Convert.ToInt32(stringDateArray[2]), Convert.ToInt16(stringDateArray[0]), Convert.ToInt16(stringDateArray[1]));
                            model.ExitDate = ExitDt;
                        }

                        model.CreateDate = DateTime.Now;
                        string password = model.Password;
                        password = EncryptionHelper.ConvertStringToMD5(password);
                        model.IsActive = model.IsActive;
                        model.Password = password;

                        um.InsertUserdata(model, out int id);
                        if (model.ISErr)
                        {
                            TempData["ErrStatus"] = model.ISErr;
                            TempData["ErrMsg"] = model.ErrString;
                        }
                        else
                        {
                            TempData["ErrStatus"] = model.ISErr;
                            TempData["ErrMsg"] = model.ErrString;
                        }


                    }
                }

            }
            catch (Exception)
            {

                //throw;
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

            StringBuilder strUserData = new StringBuilder();

            int i = 0;
            UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);
            DataTable dtUsers = new DataTable();
            if (userInfo.IsRoleSysAdmin)
            {
                dtUsers = obj.GetAllUsers();

            }
            else
            {
                dtUsers = obj.GetAllUsers(userInfo.UserOrganisationID);

            }


            foreach (DataRow dr in dtUsers.Rows)
            {

                string status = Convert.ToBoolean(dr["IsActive"]) == true ? "Active" : "In Active";

                strUserData.Append("<tr><td class='text-center'>" + dr["Id"].ToString() + "</td><td class='text-center'>" + dr["UserName"].ToString() + "</td>" + "<td class='text-center'>" + dr["Name"].ToString() + "</td>" + "<td class='text-center'>" + dr["EmailId"] + "</td>");
                strUserData.Append("<td class='text-center'>" + dr["PhoneNo"] + "</td>");
                strUserData.Append("<td class='text-center'>" + dr["Designation"] + "</td>");
                if (dr["BirthDate"] != DBNull.Value)
                {
                    strUserData.Append("<td class='text-center'>" + Convert.ToDateTime(dr["BirthDate"]).ToShortDateString() + "</td>");
                }
                else
                {
                    strUserData.Append("<td class='text-center'>" + dr["BirthDate"] + "</td>");
                }
                strUserData.Append("<td class='text-center'>" + dr["Gender"] + "</td>");
                strUserData.Append("<td class='text-center'>" + dr["ManagerName"] + "</td>");
                strUserData.Append("<td class='text-center'>" + dr["orgname"] + "</td>");
                strUserData.Append("<td class='text-center'>" + status + "</td>");
                strUserData.Append("<td class='text-center'><a href = 'ManageUsers?ID=" + dr["ID"].ToString() + "'>Edit </a> </td>");

                strUserData.Append("</tr>");

            }
            return Content(strUserData.ToString());
        }

        public ActionResult GetManagers(int orgId)
        {
            UserModel user = new UserModel();
            string strUserData = string.Empty;
            try
            {
                var listUsers = user.GetAllUsersInList(orgId).Where(x => x.IsActive == true).ToList();
                strUserData += "<option value = 0>Please select</option>";
                foreach (UserModel item in listUsers)
                {
                    strUserData += "<option value=" + Convert.ToInt32(item.ID) + ">" + item.Name + "</option>";
                }
            }
            catch (Exception)
            {

                //throw;
            }

            return Json(strUserData);
        }
        public ActionResult LoadStatus(int IssueId)
        {
            ProjectIssueModel issueModel = new ProjectIssueModel();
            try
            {
                   
                    UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);
                    DataTable dtIssueData = issueModel.GetProjectIssueByIssueId(IssueId);
                
                issueModel.ActualIssueStartDate = (dtIssueData.Rows[0]["IssueStartDateActual"] != System.DBNull.Value) ? Convert.ToDateTime(dtIssueData.Rows[0]["IssueStartDateActual"]) : (DateTime?)null;
                issueModel.ActualIssueEndDate = (dtIssueData.Rows[0]["IssueEndDateActual"] != System.DBNull.Value) ? Convert.ToDateTime(dtIssueData.Rows[0]["IssueEndDateActual"]) : (DateTime?)null;
                issueModel.Timespent = dtIssueData.Rows[0]["TimeSpent"].ToString() != "" ? dtIssueData.Rows[0]["TimeSpent"].ToString() : "0.00";
                //return Json(JsonConvert.SerializeObject(issueModel));

                DataTable dt = issueModel.Get_MasterStatus(userInfo.UserOrganisationID);
                    strStatusData += "<option value = 0>Please select</option>";
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                        strStatusData += "<option value=" + Convert.ToInt32(item["StatusID"]) + ">" + Convert.ToString(item["StatusName"]) + "</option>";
                        }
                    }
                  
            }
              catch (Exception ex)
             {

                //throw;
             }

            return Json(strStatusData+'|'+ JsonConvert.SerializeObject(issueModel));
        }
        public ActionResult GetTask(int ProjID)
        {
            UserModel user = new UserModel();
            DailyTaskViewModel model = new DailyTaskViewModel();
            try
            {
                if (ProjID == 0)
                {   
                strTaskData += "<option value = 0>Please select</option>";
                strIssueData += "<option value = 0>Please select</option>";
                }
                else
                {
                    ProjectTaskModel taskModel = new ProjectTaskModel();
                  
                    UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);
                   // model.IssueList = model.Get_IssueListByProject(ProjID, loggedInUser);
                    DataTable dt= model.GetIssueListByProject(ProjID, loggedInUser, userInfo.UserOrganisationID);
                    strIssueData += "<option value = 0>Please select</option>";
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            strIssueData += "<option value=" + Convert.ToInt32(item["IssueId"]) + ">" + Convert.ToString(item["IssueCode"]) + "</option>";
                        }
                    }
                    DataSet dsTaskData = taskModel.GetTasksData("DailyTask",ProjID, userInfo.UserOrganisationID,userInfo.UserId);
                    strTaskData += "<option value = 0>Please select</option>";
                    if (dsTaskData != null && dsTaskData.Tables.Count > 0 && dsTaskData.Tables[0] != null && dsTaskData.Tables[0].Rows.Count > 0)
                    {

                       //create by malabika 14-11-2019
                        foreach (DataRow item in dsTaskData.Tables[0].Rows)
                        {
                            if (Convert.ToInt32(item["ParentTaskID"]) == 0)
                            {
                                strTaskData += "<option value=" + Convert.ToInt32(item["TaskID"]) + ">" + Convert.ToString(item["TaskName"])+ "</option>";
                                GetSubTaskDetails(dsTaskData.Tables[0], Convert.ToInt32(item["TaskID"]));
                                
                            }    
                        }
                        //End****

                    }

         }
            }
            catch (Exception ex)
            {

                //throw;
            }

            return Json(strTaskData+'|'+strIssueData);
           // return View(model);
        }
        //created by malabika 14-11-2019
        public string GetSubTaskDetails(DataTable TaskDt, int TaskID)
        {
            
            for (int i = 0; i < TaskDt.Rows.Count; i++)
            {
                if (TaskID == Convert.ToInt32(TaskDt.Rows[i]["ParentTaskID"]))
                {
                    strTaskData += "<option value="+ Convert.ToInt32(TaskDt.Rows[i]["TaskID"]) +">" + strspace + (TaskDt.Rows[i]["TaskName"])+ "</option>";
                    strspace += "&nbsp;&nbsp;&nbsp";
                    GetSubTaskDetails(TaskDt,Convert.ToInt32(TaskDt.Rows[i]["TaskId"]));
                   
                }
            }

            //strTaskData = strTaskData1;
            strspace = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp";
            return strTaskData;


        }
        //***End


        public ActionResult GetDepartments(int orgId)
         {
            UserModel user = new UserModel();
            string strDeptData = string.Empty;
            try
            {
                var listUsers = user.GetAllDepartmentInList(orgId).Where(x => x.IsActive == true).ToList();
                strDeptData += "<option value = 0>Please select</option>";
                foreach (DepartmentModel item in listUsers)
                {
                    strDeptData += "<option value=" + Convert.ToInt32(item.DepartmentID) + ">" + item.Name + "</option>";
                }
            }
            catch (Exception)
            {

                //throw;
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
                TempData["ErrStatus"] = status.ToString();
            }
            catch (Exception)
            {

                status = "Something Went wrong. Please try later";
            }


            return Json(status);
        }
        #endregion

        #region Project managment region"

        public ActionResult LoadProjectTaskData()
        {
            ProjectIssueModel obj = new ProjectIssueModel();
            StringBuilder strUserData = new StringBuilder();
            try
            {
                var loggedInUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
                UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);
                DataTable dt = new DataTable();
                if (userInfo.IsRoleSysAdmin)
                {
                    dt = obj.GetAllProjectsTask();
                }
                else
                {
                    dt = obj.GetAllProjectsTask(loggedInUser);
                }
                foreach (DataRow item in dt.Rows)
                {
                    
                    string bgcolor = "#FFF";
                    string forecolor = "#333";
                    obj.TodayDate = System.DateTime.Now;
                    obj.TaskEndDate = Convert.ToDateTime(item["TaskEndDate"]);
                    obj.OneDayBeforeDate = obj.TaskEndDate.AddDays(-1);
                    if ((obj.TaskEndDate.Date < obj.TodayDate.Date) && (item["StatusName"].ToString() != "CLOSED"))
                    {
                        bgcolor = "#FF0000";
                        forecolor = "#FFF";
                    }
                    if (((obj.TaskEndDate.Date == obj.TodayDate.Date) || (obj.OneDayBeforeDate.Date == obj.TodayDate.Date)) && (item["StatusName"].ToString() != "CLOSED"))
                    {
                        bgcolor = "#FF8C00";
                        forecolor = "#FFF";
                    }

                    string status = Convert.ToBoolean(item["IsActive"]) == true ? "Active" : "In Active";

                    var CompletePercent = (item["CompletePercent"] == DBNull.Value) ? "" : item["CompletePercent"].ToString();
                    string Milestone;
                    if (item["isMilestone"] != null)
                    {
                        Milestone = (Convert.ToBoolean(item["isMilestone"]) == true) ? "Yes" : "No";
                    }
                    else
                    {
                        Milestone = "";
                    }
                    strUserData.Append("<tr  style='background-color:" + bgcolor + ";color:" + forecolor + " '>");

                    strUserData.Append("<td class='text-center'>" + item["TaskID"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["TaskName"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["TaskCode"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["ParentTaskName"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["ProjectName"].ToString() + "</td>");
                    strUserData.Append("<td hidden='hidden' class='text-center'>" + item["TaskStartDate"].ToString() + "</td>");
                    strUserData.Append("<td hidden='hidden' class='text-center'>" + item["TaskEndDate"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["ExpectedTime"].ToString() + "</td>");

                    strUserData.Append("<td class='text-center'>" + item["TaskStartDateActual"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["TaskEndDateActual"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["StatusName"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + status + "</td>");
                    strUserData.Append("<td class='text-center'>" + CompletePercent + "</td>");
                    strUserData.Append("<td class='text-center'>" + Milestone + "</td>");



                    if (item["StatusName"].ToString() != "CLOSED")
                    {
                        strUserData.Append("<td hidden='hidden' class='text-center'><a href='javascript:void(0);' id='projectTaskEdit' onclick='EditProjectTask(" + item["TaskID"].ToString() + ")'>Edit</a> </td>");
                    }
                    else
                    {
                         strUserData.Append("<td hidden='hidden' class='text-center'></td>");

                    }
                    strUserData.Append("<td class='text-center' ><a href='javascript:void(0);' id='projectTaskEdit' onclick='ShowPopupforPreview(" + item["TaskID"].ToString() + ")'>View</a> </td>");
                    strUserData.Append("</tr>");

                }
            }
            catch (Exception ex)
            {

                ////throw;
            }
            return Content(strUserData.ToString());
        }
        public ActionResult LoadProjectIssueData()
        {
            ProjectIssueModel obj = new ProjectIssueModel();
            StringBuilder strUserData = new StringBuilder();
            try
            {
                var loggedInUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
                UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);
                DataTable dt = new DataTable();
                if (userInfo.IsRoleSysAdmin)
                {
                    dt = obj.GetAllProjectsIssue();
                }
                else
                {
                    dt = obj.GetAllProjectsIssue(loggedInUser);
                }
                foreach (DataRow item in dt.Rows)
                {
                    string ExpecterdHours = item["ExpectedTime"].ToString();
                    string bgcolor = "#FFF";
                    string forecolor = "#333";
                    //if (ExpecterdHours != "")
                    //{
                    //    string[] Arr = new string[2];
                    //    Arr = ExpecterdHours.Split('.');  
                    //    ExpecterdHours = Arr[0] + ':' + Arr[1];
                       
                    //}
                    obj.TodayDate = System.DateTime.Now;
                    obj.IssueEndDate = Convert.ToDateTime(item["IssueEndDate"]);
                    obj.OneDayBeforeDate = obj.IssueEndDate.AddDays(-1);
                    if ((obj.IssueEndDate.Date < obj.TodayDate.Date) && (item["StatusName"].ToString() != "CLOSED"))
                    {
                        bgcolor = "#FF0000";
                        forecolor = "#FFF";
                    }
                    if (((obj.IssueEndDate.Date == obj.TodayDate.Date) || (obj.OneDayBeforeDate.Date == obj.TodayDate.Date) ) && (item["StatusName"].ToString() != "CLOSED"))
                    {
                        bgcolor = "#FF8C00";
                        forecolor = "#FFF";
                    }

                    string status = Convert.ToBoolean(item["IsActive"]) == true ? "Active" : "In Active";
                   
                    var CompletePercent = (item["CompletePercent"] == DBNull.Value) ? "" : item["CompletePercent"].ToString();
                    var SeverityName = (item["SeverityName"] == DBNull.Value) ? "" : item["SeverityName"].ToString();

                    strUserData.Append("<tr  style='background-color:" + bgcolor + ";color:"+ forecolor+" '>");
                    strUserData.Append("<td class='text-center' > TI" + item["IssueID"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["ProjectName"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["IssueCode"].ToString() + "</td>");
                    strUserData.Append(" <td class='text-center'>" +item["IssueName"].ToString() + "</td>");
                    strUserData.Append(" <td class='text-center'>" +item["TicketTypeName"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["IssueStartDate"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["IssueEndDate"].ToString() + "</td>");

                    strUserData.Append("<td class='text-center'>" + ExpecterdHours + "</td>");
                    strUserData.Append("<td class='text-center'>" + SeverityName + "</td>");
                    strUserData.Append("<td class='text-center'>" + CompletePercent + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["StatusName"].ToString() + "</td>");
                  
                    strUserData.Append("<td class='text-center'>" + status + "</td>");


                    if (item["StatusName"].ToString() != "CLOSED")
                    {
                        strUserData.Append("<td class='text-center' hidden='hidden'><a href='javascript:void(0);' id='projectIssueEdit' onclick='EditProjectIssue(" + item["IssueID"].ToString() + ")'>Edit</a> </td>");
                    }
                    else
                    {
                        strUserData.Append("<td class='text-center' hidden='hidden'></td>");
                    }
                    strUserData.Append("<td class='text-center'><a href='javascript:void(0);' onclick='PreviewTheIssueDetails("+ item["IssueID"].ToString() +")'>View</a></td>");

                    strUserData.Append("</tr>");

                }
            }
            catch (Exception ex)
            {

                ////throw;
            }
            return Content(strUserData.ToString());
        }
        public ActionResult LoadProjectTaskAssignedtoUser()
        {
            ProjectIssueModel obj = new ProjectIssueModel();
            StringBuilder strUserData = new StringBuilder();
            try
            {
                var loggedInUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
                UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);
                DataTable dt = new DataTable();

                if (userInfo.IsRoleSysAdmin)
                {
                    dt = obj.Get_ProjectTaskAssignedToUser();
                }
                else
                {
                    dt = obj.Get_ProjectTaskAssignedToUser(loggedInUser);
                }
                foreach (DataRow item in dt.Rows)
                {
                    string bgcolor = "#FFF";
                    string forecolor = "#333";
                   
                    obj.TodayDate = System.DateTime.Now;
                    obj.TaskEndDate = Convert.ToDateTime(item["TaskEndDate"]);
                    obj.OneDayBeforeDate = obj.TaskEndDate.AddDays(-1);
                    if ((obj.TaskEndDate.Date < obj.TodayDate.Date) && (item["StatusName"].ToString() != "CLOSED"))
                    {
                        bgcolor = "#FF0000";
                        forecolor = "#FFF";
                    }
                    if (((obj.TaskEndDate.Date == obj.TodayDate.Date) || (obj.OneDayBeforeDate.Date == obj.TodayDate.Date)) && (item["StatusName"].ToString() != "CLOSED"))
                    {
                        bgcolor = "#FF8C00";
                        forecolor = "#FFF";
                    }

                    string status = Convert.ToBoolean(item["IsActive"]) == true ? "Active" : "In Active";
                    
                    var CompletePercent = (item["CompletePercent"] == DBNull.Value) ? "" : item["CompletePercent"].ToString();
                    string Milestone;
                    if (item["isMilestone"] != null)
                    {
                        Milestone = (Convert.ToBoolean(item["isMilestone"]) == true) ? "Yes" : "No";
                    }
                    else
                    {
                        Milestone = "";
                    }
                    
                    strUserData.Append("<tr  style='background-color:" + bgcolor + ";color:" + forecolor + " '>");
                    strUserData.Append("<td class='text-center'>" + item["TaskID"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["TaskName"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["TaskCode"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["ParentTaskName"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["ProjectName"].ToString() + "</td>");      
                    strUserData.Append("<td class='text-center'>" + item["TaskStartDate"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["TaskEndDate"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["ExpectedTime"].ToString() + "</td>");

                    strUserData.Append("<td hidden='hidden' class='text-center'>" + item["TaskStartDateActual"].ToString() + "</td>");
                    strUserData.Append("<td hidden='hidden' class='text-center'>" + item["TaskEndDateActual"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["StatusName"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + status + "</td>");
                    strUserData.Append("<td class='text-center'>" + CompletePercent + "</td>");
                    strUserData.Append("<td class='text-center'>" + Milestone + "</td>");




                    if (item["StatusName"].ToString() != "CLOSED")
                    {
                          strUserData.AppendFormat(@"<td class='text-center'><a href ='javascript:void(0);' onclick=""ShowPopupforChangeStatus({0},'{1}','{2}','{3}');""> Change Status </a> </td>", Convert.ToInt32(item["TaskID"]), item["StatusID"].ToString(), item["TaskCode"].ToString(), item["TaskName"].ToString());
                    }
                    else
                    {
                        strUserData.Append("<td class='text-center'></td>");
                    }
                    strUserData.AppendFormat(@"<td class='text-center'><a href ='javascript:void(0);' onclick=""ShowPopupforPreview({0});"">View</td>", Convert.ToInt32(item["TaskID"]));
                    strUserData.Append("</tr>");

                }
            }
            catch (Exception ex)
            {

                ////throw;
            }
            return Content(strUserData.ToString());
        }

        public ActionResult LoadProjectIssueAssignedtoUser()
        {
            ProjectIssueModel obj = new ProjectIssueModel();
            StringBuilder strUserData = new StringBuilder();
            try
            {
                var loggedInUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
                UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);
                DataTable dt = new DataTable();
                
                if (userInfo.IsRoleSysAdmin)
                {
                    dt = obj.Get_ProjectIssueAssignedToUser();
                }
                else
                {
                    dt = obj.Get_ProjectIssueAssignedToUser(loggedInUser);
                }
                foreach (DataRow item in dt.Rows)
                {
                    string bgcolor = "#FFF";
                    string forecolor = "#333";
                    string ExpecterdHours = item["ExpectedTime"].ToString();
                    //if (ExpecterdHours != "")
                    //{
                    //    string[] Arr = new string[2];
                    //    Arr = ExpecterdHours.Split('.');
                    //    ExpecterdHours = Arr[0] + ':' + Arr[1];
                    //}
                    obj.TodayDate = System.DateTime.Now;
                    obj.IssueEndDate = Convert.ToDateTime(item["IssueEndDate"]);
                    obj.OneDayBeforeDate = obj.IssueEndDate.AddDays(-1);
                    if ((obj.IssueEndDate.Date < obj.TodayDate.Date) && (item["StatusName"].ToString() != "CLOSED"))
                    {
                        bgcolor = "#FF0000";
                        forecolor = "#FFF";
                    }
                    if (((obj.IssueEndDate.Date == obj.TodayDate.Date) || (obj.OneDayBeforeDate.Date == obj.TodayDate.Date)) && (item["StatusName"].ToString() != "CLOSED"))
                    {
                        bgcolor = "#FF8C00";
                        forecolor = "#FFF";
                    }
                    string status = Convert.ToBoolean(item["IsActive"]) == true ? "Active" : "In Active";

                    var CompletePercent = (item["CompletePercent"] == DBNull.Value) ? "" : item["CompletePercent"].ToString();
                    var SeverityName = (item["SeverityName"] == DBNull.Value) ? "" : item["SeverityName"].ToString();


                    strUserData.Append("<tr  style='background-color:" + bgcolor + ";color:" + forecolor + " '>");
                    strUserData.Append("<td class='text-center'> TI" + item["IssueID"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["ProjectName"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["IssueCode"].ToString() + "</td>");
                    strUserData.Append(" <td class='text-center'>" + item["IssueName"].ToString() + "</td>");
                    strUserData.Append(" <td class='text-center'>" + item["TicketTypeName"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["IssueStartDate"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["IssueEndDate"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + ExpecterdHours + "</td>");

                    strUserData.Append("<td class='text-center'>" + SeverityName + "</td>");
                    strUserData.Append("<td class='text-center'>" + CompletePercent + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["StatusName"].ToString() + "</td>");

                    strUserData.Append("<td class='text-center'>" + status + "</td>");


                    if (item["StatusName"].ToString() != "CLOSED")
                    {
                        //strUserData.Append("<td class='text-center'><a href='javascript:void(0);' id='Changestatus' onclick='EditProjectIssue(" + item["IssueID"].ToString() + ")'>Change Status</a> </td>");
                        strUserData.AppendFormat(@"<td class='text-center'><a href ='javascript:void(0);' onclick=""ShowPopupforChangeStatus({0},'{1}','{2}','{3}');""> Change Status </a> </td>", Convert.ToInt32(item["IssueID"]), item["StatusID"].ToString(), item["IssueCode"].ToString(), item["IssueName"].ToString());
                    }
                    else
                    {
                        strUserData.Append("<td class='text-center'></td>");
                    }
                    strUserData.Append("<td class='text-center'><a href='javascript:void(0);' onclick='PreviewTheIssueDetails(" + item["IssueID"].ToString() + ")'>View</a></td>");
                    strUserData.Append("</tr>");

                }
            }
            catch (Exception ex)
            {

                ////throw;
            }
            return Content(strUserData.ToString());
        }
        public ActionResult LoadProjectData()
        {
            ProjectModel obj = new ProjectModel();
            StringBuilder strUserData = new StringBuilder();
            try
            {
                var loggedInUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
                UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);
                DataTable dt = new DataTable();

                if (userInfo.IsRoleSysAdmin)
                {
                    dt = obj.GetAllProjects();
                }
                else
                {
                    dt = obj.GetAllProjects(userInfo.UserOrganisationID);
                }
                foreach (DataRow item in dt.Rows)
                {

                    string status = Convert.ToBoolean(item["IsActive"]) == true ? "Active" : "In Active";
                    var departmentName = (item["DepartmentName"] == DBNull.Value) ? "" : item["DepartmentName"].ToString();
                    var ManagerName = (item["ProjectManagerName"] == DBNull.Value) ? "" : item["ProjectManagerName"].ToString();
                    var clientName = (item["ClientName"] == DBNull.Value) ? "" : item["ClientName"].ToString();
                    var strProjectTypeName = (item["ProjectTypeName"] == DBNull.Value) ? "" : item["ProjectTypeName"].ToString();
                    var PricingModelName= (item["PricingModelName"] == DBNull.Value) ? "" : item["PricingModelName"].ToString();
                    var BillingTypeName = (item["BillingTypeName"] == DBNull.Value) ? "" : item["BillingTypeName"].ToString();
                    var CurrencyTypeName = (item["CurrencyTypeName"] == DBNull.Value) ? "" : item["CurrencyTypeName"].ToString();

                    strUserData.Append("<tr>");
                    strUserData.Append("<td class='text-center'>" + item["Id"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["Name"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["ProjectCode"].ToString() + "</td>");
                    strUserData.Append(" <td class='text-center'>" + item["Description"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + strProjectTypeName + "</td>");
                    strUserData.Append("<td class='text-center'>" + PricingModelName + "</td>");
                    strUserData.Append("<td class='text-center'>" + BillingTypeName + "</td>");
                    strUserData.Append("<td class='text-center'>" + departmentName + "</td>");
                    strUserData.Append("<td class='text-center'>" + ManagerName + "</td>");
                    strUserData.Append("<td class='text-center'>" + clientName + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["ClientPoNo"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + item["ClientPoDate"].ToString()+ "</td>");
                    strUserData.Append("<td class='text-center'>" + item["StartDate"].ToString()+ "</td>");
                    strUserData.Append("<td class='text-center'>" + item["EndDate"].ToString()+ "</td>");
                    strUserData.Append("<td class='text-center'>" + CurrencyTypeName+ "</td>");
                    strUserData.Append("<td class='text-center'>" + item["ProjectRate"].ToString()+ "</td>");
                    strUserData.Append("<td class='text-center'>" + item["OrgName"].ToString() + "</td>");
                    strUserData.Append("<td class='text-center'>" + status + "</td>");

                    strUserData.AppendFormat(@"<td class='text-center'><a href ='javascript:void(0);' onclick=""ShowTeamMemberList({0},'{1}');""> {2} </a> </td>", item["Id"].ToString(), item["Name"].ToString(), item["TotalMemberCount"].ToString());
                    strUserData.AppendFormat(@"<td class='text-center'><a href ='javascript:void(0);' onclick=""ShowTaskNameListPopup({0},'{1}');""> {2} </a> </td>", item["Id"].ToString(), item["Name"].ToString(), item["TaskCount"].ToString());
                    // strUserData.Append("<td class='text-center'>" + item["TaskCount"] + " </td>");


                    strUserData.Append("<td class='text-center'><a href = 'ManageProject?ID=" + item["ID"].ToString() + "'>Edit </a> </td>");
                    if (Convert.ToBoolean(item["IsActive"]))
                    {
                        strUserData.AppendFormat(@"<td class='text-center'><a href ='javascript:void(0);' onclick=""ShowTaskPopup({0},'{1}');""> Add Task </a> </td>", item["Id"].ToString(), item["Name"].ToString());
                    }
                    else
                    {
                        strUserData.Append("<td class='text-center'></td>");
                    }
                    strUserData.AppendFormat(@"<td class='text-center'><a href ='javascript:void(0);' onclick=""ShowUserPopup({0},'{1}');""> Add User </a> </td>", item["Id"].ToString(), item["Name"].ToString());


                    strUserData.Append("</tr>");

                }
            }
            catch (Exception ex)
            {

                ////throw;
            }
            return Content(strUserData.ToString());
        }

        //****
        
        public ActionResult AddProjectTask(int ID = 0)
        {
            ProjectModel obj = new ProjectModel();
            ProjectModel pm = new ProjectModel();
            ProjectTypeModel PTM = new ProjectTypeModel();

            List<ProjectModel> objRole = new List<ProjectModel>();


            var loggedInUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
            UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);

            if (userInfo.IsRoleSysAdmin)
            {
                UserModel sysAdmin = obj.UserList.Single(x => x.ID == 13 || x.Name.ToLower() == "sysAdmin".ToLower());
                obj.UserList.Remove(sysAdmin);
                obj.ProjectTypeList = PTM.GetProjectType().Where(x => x.IsActive == true).ToList().OrderBy(x => x.OrganisationName).ThenBy(x => x.Name).ToList();
                obj.ActiveProjectList = obj.Get_ActiveProjectMappedwithUser(loggedInUser);
            }
            else
            {
                obj.ProjectTypeList = PTM.GetProjectType(userInfo.UserOrganisationID).Where(x => x.IsActive == true).ToList();
                obj.ActiveProjectList = obj.Get_ActiveProjectMappedwithUser(loggedInUser);
            }


            return View(obj);
        }
        public ActionResult ManageProjectIssue(int ID = 0)
        {

            ProjectModel obj = new ProjectModel();
            ProjectModel pm = new ProjectModel();
            ProjectTypeModel PTM = new ProjectTypeModel();
            MasterSeverityModel MSM = new MasterSeverityModel();
            TicketTypeModel TTM = new TicketTypeModel();
            List<ProjectModel> objRole = new List<ProjectModel>();
           

            var loggedInUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
            UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);

            if (userInfo.IsRoleSysAdmin)
            {
                UserModel sysAdmin = obj.UserList.Single(x => x.ID == 13 || x.Name.ToLower() == "sysAdmin".ToLower());
                obj.UserList.Remove(sysAdmin);
                obj.ProjectTypeList = PTM.GetProjectType().Where(x => x.IsActive == true).ToList().OrderBy(x => x.OrganisationName).ThenBy(x => x.Name).ToList();
                obj.SeverityList = MSM.Get_SeverityDetails().Where(x => x.IsActive == true).ToList().OrderBy(x => x.OrganisationName).ThenBy(x => x.Name).ToList();
                obj.ActiveProjectList = obj.Get_ActiveProjectMappedwithUser(loggedInUser);
                obj.TicketTypeList= TTM.Get_GetAllTicketTypes().Where(x => x.isActive == true).ToList().OrderBy(x => x.OrganisationName).ThenBy(x => x.Name).ToList();
            }
            else
            {
                obj.ProjectTypeList = PTM.GetProjectType(userInfo.UserOrganisationID).Where(x => x.IsActive == true).ToList();
                obj.SeverityList=MSM.Get_SeverityDetails(userInfo.UserOrganisationID).Where(x => x.IsActive == true).ToList();
                obj.ActiveProjectList = obj.Get_ActiveProjectMappedwithUser(loggedInUser);
                obj.TicketTypeList = TTM.Get_GetAllTicketTypes(userInfo.UserOrganisationID).Where(x => x.isActive == true).ToList();
            }
           

            return View(obj);
        }
        //**
        public ActionResult ManageProject(int ID = 0)
        {
            ProjectModel obj = new ProjectModel();
            ProjectTypeModel PTM = new ProjectTypeModel();
            ProjectPricingModel PPM = new ProjectPricingModel();
            ProjectBillingModel PBM = new ProjectBillingModel();
            List<ProjectModel> objRole = new List<ProjectModel>();
            MasterCurrencyModel currency = new MasterCurrencyModel();

            var loggedInUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
            UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);

            if (userInfo.IsRoleSysAdmin)
            {
                obj.DepartmentList = obj.GetDepartments().Where(x => x.IsActive == true).ToList().OrderBy(x => x.OrganisationName).ThenBy(x => x.Name).ToList();
                obj.UserList = obj.GetManagers().Where(x => x.IsActive == true).OrderBy(x => x.OrganisationName).ThenBy(x => x.Name).ToList();

                UserModel sysAdmin = obj.UserList.Single(x => x.ID == 13 || x.Name.ToLower() == "sysAdmin".ToLower());
                obj.UserList.Remove(sysAdmin);

                obj.ClientList = obj.GetClients().Where(x => x.IsActive == true).OrderBy(x => x.OrganisationName).ThenBy(x => x.ClientName).ToList();
                obj.ProjectTypeList = PTM.GetProjectType().Where(x => x.IsActive == true).ToList().OrderBy(x => x.OrganisationName).ThenBy(x => x.Name).ToList();
                obj.ProjectPricingList = PPM.Get_ProjectPricingDetails().Where(x => x.IsActive == true).ToList().OrderBy(x => x.OrganisationName).ThenBy(x => x.Name).ToList();
                obj.ProjectBillingList = PBM.Get_ProjectBillingDetails().Where(x => x.IsActive == true).ToList().OrderBy(x => x.OrganisationName).ThenBy(x => x.Name).ToList();
                obj.CurrencyList = currency.Get_CurrencyDetails().Where(x => x.IsActive == true).ToList().OrderBy(x => x.OrganisationName).ThenBy(x => x.Name).ToList();
            }
            else
            {
                obj.DepartmentList = obj.GetDepartments(userInfo.UserOrganisationID).Where(x => x.IsActive == true).ToList();
                obj.UserList = obj.GetManagers(userInfo.UserOrganisationID).Where(x => x.IsActive == true).OrderBy(x => x.OrganisationName).ThenBy(x => x.Name).ToList();
                obj.ClientList = obj.GetClients(userInfo.UserOrganisationID).Where(x => x.IsActive == true).OrderBy(x => x.OrganisationName).ThenBy(x => x.ClientName).ToList();
                obj.ProjectTypeList = PTM.GetProjectType(userInfo.UserOrganisationID).Where(x => x.IsActive == true).ToList();
                obj.ProjectPricingList = PPM.Get_ProjectPricingDetails(userInfo.UserOrganisationID).Where(x => x.IsActive == true).ToList();
                obj.ProjectBillingList = PBM.Get_ProjectBillingDetails(userInfo.UserOrganisationID).Where(x => x.IsActive == true).ToList();
                obj.CurrencyList = currency.Get_CurrencyDetails(userInfo.UserOrganisationID).Where(x => x.IsActive == true).ToList();

            }

            if (ID > 0)
            {
                try
                {
                    DataTable dt = new DataTable();
                    dt = obj.GetProjectByID(ID);
                    obj.ProjectID = Convert.ToInt32(dt.Rows[0]["ID"]);
                    obj.ProjectName = dt.Rows[0]["Name"].ToString();
                    obj.ProjectCode = dt.Rows[0]["ProjectCode"]?.ToString();
                    obj.Description = dt.Rows[0]["Description"].ToString();
                    obj.MaxProjectTimeInHours = Convert.ToInt32(dt.Rows[0]["MaxProjectTimeInHours"]);
                    obj.IsActive = Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString());
                    if (dt.Rows[0]["PMUserID"] != System.DBNull.Value)
                    {
                        obj.PMUserID = Convert.ToInt32(dt.Rows[0]["PMUserID"]);
                    }
                    else
                    {
                        obj.PMUserID = 0;
                    }
                    if (dt.Rows[0]["ClientID"] != System.DBNull.Value)
                    {
                        obj.ClientD = Convert.ToInt32(dt.Rows[0]["ClientID"]);
                    }
                    else
                    {
                        obj.ClientD = 0;
                    }
                    // obj.ClientD = Convert.ToInt32(dt.Rows[0]["ClientID"]);
                    if (dt.Rows[0]["DepartmentID"] != System.DBNull.Value)
                    {
                        obj.DepartmentID = Convert.ToInt32(dt.Rows[0]["DepartmentID"]);

                    }
                    if (dt.Rows[0]["ProjectTypeID"] != System.DBNull.Value)
                    {
                        obj.ProjectTypeID = Convert.ToInt32(dt.Rows[0]["ProjectTypeID"]);

                    }
                    if (dt.Rows[0]["PricingModelID"] != System.DBNull.Value)
                    {
                        obj.ProjectPricingID = Convert.ToInt32(dt.Rows[0]["PricingModelID"]);

                    }
                    if (dt.Rows[0]["BillingTypeID"] != System.DBNull.Value)
                    {
                        obj.ProjectBillingID = Convert.ToInt32(dt.Rows[0]["BillingTypeID"]);

                    }
                    if (dt.Rows[0]["CurrencyTypeID"] != System.DBNull.Value)
                    {
                        obj.CurrencyID = Convert.ToInt32(dt.Rows[0]["CurrencyTypeID"]);

                    }
                    obj.ClientPoNo= dt.Rows[0]["ClientPoNo"].ToString(); 
                    obj.ClientPoDateToDisplay= dt.Rows[0]["ClientPoDate"].ToString();
                    obj.ProjectStartDateToDisplay= dt.Rows[0]["StartDate"].ToString();
                    obj.ProjectEndDateToDisplay= dt.Rows[0]["EndDate"].ToString();
                    obj.ProjectRate = Convert.ToDouble(dt.Rows[0]["ProjectRate"]);

                   
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
        public DateTime GetDate(string date)
        {
            return DateTime.ParseExact(date, "MM/dd/yyyy", null);
        }
        [HttpPost]
        public ActionResult ManageProject(ProjectModel model)
        {
            ProjectModel obj = new ProjectModel();
            try
            {
                string formattedClientPoDate = GetDate(model.ClientPoDateToDisplay).ToString();//.ToString("dd/MM/yyyy");
                model.ClientPoDate = Convert.ToDateTime(formattedClientPoDate);

                string formattedStartDate = GetDate(model.ProjectStartDateToDisplay).ToString();//.ToString("dd/MM/yyyy");
                model.ProjectStartDate = Convert.ToDateTime(formattedStartDate);

                string formattedEndDate = GetDate(model.ProjectEndDateToDisplay).ToString();//.ToString("dd/MM/yyyy");
                model.ProjectEndDate = Convert.ToDateTime(formattedEndDate);

              

                if (model.ProjectID > 0)
                {
                    try
                    {
                        obj = model;
                        obj.EditedBy = System.Web.HttpContext.Current.Session["sessUser"]?.ToString();
                        obj.EditedDate = DateTime.Now;
                        obj.IsActive = model.IsActive;
                        bool result = obj.Update_ProjectDetails(obj);
                        if (result)
                        {
                            obj.ISErr = false;
                            obj.ErrString = "Data Saved Successfully.";
                            TempData["ErrStatus"] = obj.ISErr;
                            TempData["ErrMsg"] = obj.ErrString.ToString();
                            ////TODO need to remove this line when testing is complete.
                            //TempData["JavaScriptFunction"] = $"ShowUserPopup('{model.ProjectID}','{model.ProjectName}');";
                        }
                        else
                        {
                            obj.ISErr = true;
                            obj.ErrString = "Error occured.";
                            TempData["ErrStatus"] = obj.ISErr;
                            TempData["ErrMsg"] = obj.ErrString.ToString();
                        }
                    }
                    catch
                    {

                    }
                }
                else
                {
                    model.CreatedBy = System.Web.HttpContext.Current.Session["sessUser"]?.ToString();
                    model.CreateDate = DateTime.Now;
                    model.IsActive = model.IsActive;
                    //TODO need to test.....
                    bool result =  obj.InsertProjectdata(model, out int id);
                    
                    if (result && id > 0)
                    {
                        obj.ProjectID = id;
                        obj.ISErr = false;
                        obj.ErrString = "Data Saved Successfully.";
                        TempData["ErrStatus"] = model.ISErr.ToString();
                        TempData["ErrMsg"] = obj.ErrString.ToString();
                        TempData["JavaScriptFunction"] = $"ShowUserPopup('{id}','{model.ProjectName}');";
                    }
                    else
                    {
                        obj.ISErr = true;
                        obj.ErrString = "Error occured.";
                        TempData["ErrStatus"] = obj.ISErr;
                        TempData["ErrMsg"] = obj.ErrString.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrStatus"] = model.ISErr.ToString();
                //throw;
            }
            return RedirectToAction("ManageProject", "Admin");
        }
        public ActionResult GetAllUserOfOrganisationByProjectID(int projectID)
        {
            ProjectTaskModel obj = new ProjectTaskModel();
            StringBuilder builder = new StringBuilder();

            DataTable dt = new DataTable();
            try
            {
                dt = obj.GetAllUserOfOrganisationByProjectID(projectID);
                builder.Append(@"<div id='divUserList' class='row' style='margin:10px;'>");

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        builder.Append(@"<div style='float: left;width: 25%; padding: 5px;'>");
                        if (Convert.ToBoolean(row["IsMapped"]))
                        {
                            if (Convert.ToBoolean(row["IsProjectManager"]))
                            {
                                builder.AppendFormat($"<input type = 'checkbox' class='check' style=' margin:5px;' name='modules' value={row["Id"].ToString()} checked disabled>{row["Name"].ToString()}");
                            }
                            else
                            {
                                builder.AppendFormat($"<input type = 'checkbox' class='check' style=' margin:5px;' name='modules' value={row["Id"].ToString()} checked >{row["Name"].ToString()}");
                            }
                        }
                        else
                        {
                            builder.AppendFormat($"<input type = 'checkbox' class='check' style=' margin:5px;' name='modules' value={row["Id"].ToString()} >{row["Name"].ToString()}");
                        }


                        builder.Append(@"</div>");
                    }
                }
                else
                {
                    builder.Append(@"<h5>No Users available</h5>");
                }
                builder.Append(@"</div>");
            }
            catch (Exception)
            {

                throw;
            }
            return Json(builder.ToString());
        }


        public ActionResult GetAllUserOfProjectByProjectID(int projectID)
        {
            ProjectModel obj = new ProjectModel();
            List<string> userList = new List<string>();

            DataTable dt = new DataTable();
            try
            {
                dt = obj.GetAllUserListByProjectID(projectID);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        userList.Add(item["Name"].ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                return Json(ex);

            }
            return Json(JsonConvert.SerializeObject(userList));
        }

        public ActionResult GetAllTasksOfProjectByProjectID(int projectID)
        {
            ProjectModel obj = new ProjectModel();
            List<string> taskList = new List<string>();

            DataTable dt = new DataTable();
            try
            {
                dt = obj.GetAllTaskListByProjectID(projectID);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        taskList.Add(item["TaskName"].ToString());
                    }
                }

            }
            catch (Exception ex)
            {

                return Json(ex);

            }
            return Json(JsonConvert.SerializeObject(taskList));
        }

        #region Project task managment region"
        public ActionResult ManageProjectTask(int ProjectId = 0)
        {

            ProjectTaskModel taskModel = new ProjectTaskModel();

            try
            {
                UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);
                DataSet dsTaskData = taskModel.GetTasksData("ManageTask",ProjectId, userInfo.UserOrganisationID);
                dsTaskData.Tables[0].TableName = "TaskList";
                dsTaskData.Tables[1].TableName = "StatusList";
                dsTaskData.Tables[2].TableName = "UserList";

                taskModel.TaskList.Clear();
                taskModel.StatusList.Clear();
                taskModel.UserList.Clear();
                taskModel.PercentageComplete.Clear();

                if (dsTaskData.Tables["TaskList"].Rows.Count > 0)
                {
                    foreach (DataRow item in dsTaskData.Tables["TaskList"].Rows)
                    {
                        ProjectTaskModel task = new ProjectTaskModel();
                        task.TaskId = Convert.ToInt32(item["TaskID"]);
                        task.TaskName = item["TaskName"].ToString();
                        task.TaskCode = item["TaskCode"].ToString();
                        task.ParentTaskName = item["ParentTaskName"].ToString() ?? "";
                        task.ParentTaskId = Convert.ToInt32(item["ParentTaskID"]);
                        task.ProjectName = item["ProjectName"].ToString();
                        task.TaskStartDate = Convert.ToDateTime(item["TaskStartDate"]);
                        task.TaskEndDate = Convert.ToDateTime(item["TaskEndDate"]);
                        task.ProjectTypeID= Convert.ToInt32(item["ProjectTypeID"]);
                        task.ActualTaskStartDate = (item["TaskStartDateActual"] != System.DBNull.Value) ? Convert.ToDateTime(item["TaskStartDateActual"]) : (DateTime?)null;
                        task.ActualTaskEndDate = (item["TaskEndDateActual"] != System.DBNull.Value) ? Convert.ToDateTime(item["TaskEndDateActual"]) : (DateTime?)null;
                        task.TaskStatusName = item["StatusName"].ToString() ?? "";
                        task.IsMilestone = (item["isMilestone"] != DBNull.Value) ? Convert.ToBoolean(item["isMilestone"]) : (bool?)null;
                        task.CompletePercent = Convert.ToInt32(item["CompletePercent"]);
                        task.IsActive = Convert.ToBoolean(item["isACTIVE"]);
                        taskModel.TaskList.Add(task);

                    }
                }

                if (dsTaskData.Tables["StatusList"].Rows.Count > 0)
                {
                    foreach (DataRow item in dsTaskData.Tables["StatusList"].Rows)
                    {
                        ProjectStatusModel statusModel = new ProjectStatusModel();
                        statusModel.StatusName = item["StatusName"].ToString();
                        statusModel.StatusID = Convert.ToInt32(item["StatusID"]);
                        statusModel.IsActive = Convert.ToBoolean(item["isACTIVE"]);
                        taskModel.StatusList.Add(statusModel);

                    }
                }

                DataTable dt = new DataTable();
                dt = dsTaskData.Tables["UserList"];
                if (dsTaskData.Tables["UserList"].Rows.Count > 0)
                {
                    foreach (DataRow item in dsTaskData.Tables["UserList"].Rows)
                    {
                        UserModel userModel = new UserModel();
                        userModel.Name = item["Name"].ToString();
                        userModel.ID = Convert.ToInt32(item["Id"]);
                        userModel.IsActive = Convert.ToBoolean(item["isACTIVE"]);
                        taskModel.UserList.Add(userModel);

                    }
                }


                for (int i = 0; i <= 100; i++)
                {
                    taskModel.PercentageComplete.Add(i);
                }
            }
            catch (Exception)
            {

                //throw;
            }

            return Json(JsonConvert.SerializeObject(taskModel));

        }

        public ActionResult GetProjectTaskByID(int taskId)
        {
            ProjectTaskModel task = new ProjectTaskModel();

            try
            {

                DataTable dtTaskData = task.GetProjectTasksByTaskId(taskId);
                if (dtTaskData.Rows.Count > 0)
                {

                    task.TaskId = Convert.ToInt32(dtTaskData.Rows[0]["TaskID"]); 
                    task.TaskCode = dtTaskData.Rows[0]["TaskCode"].ToString() ?? "";
                    task.TaskName = dtTaskData.Rows[0]["TaskName"].ToString();
                    task.ParentTaskName = dtTaskData.Rows[0]["ParentTaskName"].ToString() ?? "";
                    task.ParentTaskId = Convert.ToInt32(dtTaskData.Rows[0]["ParentTaskID"]);
                    task.ProjectName = dtTaskData.Rows[0]["ProjectName"].ToString();
                    task.ProjectID = Convert.ToInt32(dtTaskData.Rows[0]["ProjectID"]);
                    task.TaskStartDate = Convert.ToDateTime(dtTaskData.Rows[0]["TaskStartDate"]);
                    task.TaskEndDate = Convert.ToDateTime(dtTaskData.Rows[0]["TaskEndDate"]);
                    task.IsMilestone = (dtTaskData.Rows[0]["isMilestone"] != System.DBNull.Value) ? Convert.ToBoolean(dtTaskData.Rows[0]["isMilestone"]) : (bool?)null;
                    task.ActualTaskStartDate = (dtTaskData.Rows[0]["TaskStartDateActual"] != System.DBNull.Value) ? Convert.ToDateTime(dtTaskData.Rows[0]["TaskStartDateActual"]) : (DateTime?)null;
                    task.ActualTaskEndDate = (dtTaskData.Rows[0]["TaskEndDateActual"] != System.DBNull.Value) ? Convert.ToDateTime(dtTaskData.Rows[0]["TaskEndDateActual"]) : (DateTime?)null;
                    task.TaskStatusName = dtTaskData.Rows[0]["StatusName"].ToString() ?? "";
                    task.TaskStatusID = Convert.ToInt32(dtTaskData.Rows[0]["StatusID"]);
                    task.IsActive = Convert.ToBoolean(dtTaskData.Rows[0]["isACTIVE"]);
                    task.IsValueAdded= Convert.ToBoolean(dtTaskData.Rows[0]["isValueAdded"]);
                    task.CompletePercent = Convert.ToInt32(dtTaskData.Rows[0]["CompletePercent"]);

                    foreach (DataRow item in dtTaskData.Rows)
                    {
                        UserModel userModel = new UserModel
                        {
                            ID = Convert.ToInt32(item["UserID"]),
                            Name = item["UserName"].ToString(),
                            IsActive = Convert.ToBoolean(item["IsUserActive"])

                        };

                        task.UserIdsTaskAssigned += (item["UserID"]).ToString() + ",";
                        task.UserNameTaskAssigned+= (item["UserName"]).ToString() + ",";
                        task.UserList.Add(userModel);
                    }
                    task.UserIdsTaskAssigned = task.UserIdsTaskAssigned.TrimEnd(',');
                    // task.TaskList.Add(task);
                }

            }
            catch (Exception)
            {

                //throw;
            }
            return Json(JsonConvert.SerializeObject(task));


        }

        public ActionResult previewProjectTask(int taskId)
        {            
            ProjectTaskModel task = new ProjectTaskModel();
            ProjectTaskAttachmentModel attachment = new ProjectTaskAttachmentModel();
            try
            {

                DataTable dtTaskData = task.GetProjectTasksByTaskId(taskId);
                DataTable dtTaskAttachment = attachment.GetProjectTasksAttachments(taskId);

                if (dtTaskData.Rows.Count > 0)
                {

                    ViewBag.TaskId = Convert.ToInt32(dtTaskData.Rows[0]["TaskID"]);
                    ViewBag.TaskCode = dtTaskData.Rows[0]["TaskCode"].ToString() ?? "";
                    ViewBag.TaskName = dtTaskData.Rows[0]["TaskName"].ToString();
                    ViewBag.ParentTaskName = dtTaskData.Rows[0]["ParentTaskName"].ToString() ?? "";
                    ViewBag.ParentTaskId = Convert.ToInt32(dtTaskData.Rows[0]["ParentTaskID"]);
                    ViewBag.ProjectName = dtTaskData.Rows[0]["ProjectName"].ToString();
                    ViewBag.ProjectID = Convert.ToInt32(dtTaskData.Rows[0]["ProjectID"]);
                    ViewBag.TaskStartDate = Convert.ToDateTime(dtTaskData.Rows[0]["TaskStartDate"]).ToString("dd/MM/yyyy");
                    ViewBag.TaskEndDate = Convert.ToDateTime(dtTaskData.Rows[0]["TaskEndDate"]).ToString("dd/MM/yyyy"); ;
                    ViewBag.IsMilestone = (dtTaskData.Rows[0]["isMilestone"] != System.DBNull.Value) ? Convert.ToBoolean(dtTaskData.Rows[0]["isMilestone"]) : (bool?)null;
                    ViewBag.ActualTaskStartDate = (dtTaskData.Rows[0]["TaskStartDateActual"] != System.DBNull.Value) ? Convert.ToDateTime(dtTaskData.Rows[0]["TaskStartDateActual"]) : (DateTime?)null;
                    ViewBag.ActualTaskEndDate = (dtTaskData.Rows[0]["TaskEndDateActual"] != System.DBNull.Value) ? Convert.ToDateTime(dtTaskData.Rows[0]["TaskEndDateActual"]) : (DateTime?)null;
                    ViewBag.TaskStatusName = dtTaskData.Rows[0]["StatusName"].ToString() ?? "";
                    ViewBag.TaskStatusID = Convert.ToInt32(dtTaskData.Rows[0]["StatusID"]);
                    ViewBag.IsActive = Convert.ToBoolean(dtTaskData.Rows[0]["isACTIVE"]);
                    ViewBag.IsValueAdded = Convert.ToBoolean(dtTaskData.Rows[0]["isValueAdded"]);
                    ViewBag.CompletePercent = Convert.ToInt32(dtTaskData.Rows[0]["CompletePercent"]);
                    string usernameassigned = string.Empty;
                    

                    
                    
                    int i = 0;
                    foreach (DataRow item in dtTaskData.Rows)
                    {
                        
                                UserModel userModel = new UserModel
                                {

                                    ID = Convert.ToInt32(item["UserID"]),
                                    Name = item["UserName"].ToString(),
                                    IsActive = Convert.ToBoolean(item["IsUserActive"])



                                };


                                task.UserIdsTaskAssigned += (item["UserID"]).ToString() + ", ";
                                usernameassigned += (item["UserName"]).ToString() + ", ";
                                task.UserList.Add(userModel);
                            }
                        //}
                        //else
                        //{
                        //    UserModel userModel = new UserModel
                        //    {

                        //        ID = Convert.ToInt32(item["UserID"]),
                        //        Name = item["UserName"].ToString(),
                        //        IsActive = Convert.ToBoolean(item["IsUserActive"])
                        //    };


                        //    task.UserIdsTaskAssigned += (item["UserID"]).ToString() + ", ";
                        //    usernameassigned += (item["UserName"]).ToString() + ", ";
                        //    task.UserList.Add(userModel);
                        //}
                       
                        
                        

                        
                    
                    usernameassigned = usernameassigned.TrimEnd(',');
                    ViewBag.UserNameAssigned = usernameassigned.Remove(usernameassigned.Length - 2, 2);
                    
                    if (dtTaskAttachment.Rows.Count > 0)
                    {
                        string AttachmentName = string.Empty;
                        string path = string.Empty;
                        string URL = string.Empty;
                        string URLs = dtTaskAttachment.Rows[0]["URL"].ToString();
                        string[] URLlist = URLs.Split(',');

                        foreach(string url in URLlist)
                        {
                            URL+= "<a class='img' target='_blank'   href = '" + url +"' > " + url + " </a>&nbsp;&nbsp;&nbsp;&nbsp;";
                        }

                        ViewBag.URL = URL;

                        if (dtTaskAttachment.Rows[0]["DirectoryName"] != null)
                        {
                            path = Server.MapPath("~/ProjectTaskAttachments/" + dtTaskAttachment.Rows[0]["DirectoryName"].ToString());
                        }

                        if (Directory.Exists(path))
                        {
                            foreach(DataRow item in dtTaskAttachment.Rows)
                            {
                                AttachmentName += "<a class='img' target='_blank'  data-fancybox href = '/ProjectTaskAttachments/" + dtTaskAttachment.Rows[0]["DirectoryName"].ToString() + "/" + item["AttachmentName"].ToString() + "' > " + item["AttachmentName"].ToString() + " </a>&nbsp;&nbsp;&nbsp;&nbsp;";
                            }
                            
                        }
                        ViewBag.AttachmentName = AttachmentName;
                    }


                   // foreach()
                   // task.TaskList.Add(task);
                }

            }
            catch (Exception)
            {

                //throw;
            }

            return PartialView("_PreviewProjectTask");
        }

        public ActionResult LoadCommentsAndAttachmentsForProjectTasks(int id)
        {
            string strCommentDiv = string.Empty;
          
            ProjectTaskCommentModel ptcm = new ProjectTaskCommentModel();
            DataSet ds = ptcm.GetProjectTasksComments(id);
            strCommentDiv += @"<div class='row'><div class='col-md-12'> <div class='panel panel-default'><div class='panel-heading'><h4> Status and discussion history</h4></div><div class='panel-body'><div class='row form-group'><div class='col-md-12'><div class='col-md-12'>";
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                strCommentDiv += @"<section class='comments'>";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strCommentDiv += @"<article class='comment'><a class='comment-img' href='#non'><img src ='https://pbs.twimg.com/profile_images/444197466133385216/UA08zh-B.jpeg' alt = '' width = '50' height = '50'/></a><div class='comment-body'><div class='text'><p><b>Status:</b> " + ds.Tables[0].Rows[i]["StatusName"] + ".</p> <p><b>Comment:</b> " + ds.Tables[0].Rows[i]["Comment"].ToString() + "</p>";

                    //Load Attachment
                    strCommentDiv += "<p><b>Attachments:</b>";
                    if (ds != null && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                        {
                            if (ds.Tables[0].Rows[i]["CommentID"].ToString() == ds.Tables[1].Rows[j]["ProjectTaskCommentId"].ToString())
                            {
                                string path = Server.MapPath("~/ProjectTaskAttachments/" + ds.Tables[1].Rows[j]["DirectoryName"].ToString());
                                if (Directory.Exists(path))
                                {
                                    DirectoryInfo di = new DirectoryInfo(path);
                                    
                                    strCommentDiv += "<a class='img' target='_blank'  data-fancybox href = '/ProjectTaskAttachments/" + ds.Tables[1].Rows[j]["DirectoryName"].ToString() + "/" + ds.Tables[1].Rows[j]["AttachmentName"].ToString() + "' >&nbsp;" + ds.Tables[1].Rows[j]["AttachmentName"].ToString() + "</a>&nbsp;&nbsp;&nbsp;&nbsp;";

                                }
                            }
                        }

                    }
                    strCommentDiv += "</p>";

                    //Load URL
                    strCommentDiv += "<p><b>URL:</b>";
                    if (ds != null && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                        {

                            if ((ds.Tables[0].Rows[i]["CommentID"].ToString() == ds.Tables[1].Rows[j]["ProjectTaskCommentId"].ToString()))
                            {

                                string[] arrURL = ds.Tables[1].Rows[j]["URL"].ToString().Split(';');
                                
                                for (int k = 0; k < arrURL.Length; k++)
                                {
                                    strCommentDiv += "<a class='img' target='_blank'   href = '" + arrURL[k] + "' rel='noopener noreferrer'>" + arrURL[k] + " </a> &nbsp;&nbsp;&nbsp;&nbsp;";

                                }

                            }
                        }

                    }
                    strCommentDiv += "</p>";
                    strCommentDiv += "</div><p class='attribution'>by<a href='#non'> " + ds.Tables[0].Rows[i]["UserName"] + " </a>" + ds.Tables[0].Rows[i]["AddedTime"].ToString() + "</p></div></article>";

                }
                strCommentDiv += @"</section>";


            }
            else
            {
                strCommentDiv += "<div>No Comments Found</div>";
            }
            strCommentDiv += @"</div></div></div></div></div></div></div>";
            return Content(strCommentDiv);
        }

        [HttpPost]
        public JsonResult ProjectTaskAttachments()
        {
            ProjectTaskAttachmentModel tpam = new ProjectTaskAttachmentModel();
            string fName = "";
            string Directory = "";
            string FolderName = Request.Form["FileDirectoryForChangeStatus"].ToString();
            if (FolderName != "")
            {
                Directory = FolderName.Replace("\"", string.Empty).Trim();
            }
            else
            {
                Directory = DateTime.Now.Ticks.ToString();
            }

            foreach (string imageFile in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[imageFile];
                fName = file.FileName;
                int id = 0;                
                int pressID = 0;
                if (file != null && file.ContentLength > 0)
                {
                    var originalDirectory = new DirectoryInfo(string.Format("{0}ProjectTaskAttachments", Server.MapPath(@"\")));
                    string pathString = System.IO.Path.Combine(originalDirectory.ToString(), Directory);
                    var fileName = file.FileName;
                    bool isExists = System.IO.Directory.Exists(pathString);
                    if (!isExists)
                        System.IO.Directory.CreateDirectory(pathString);
                    var path = string.Format("{0}\\{1}", pathString, fileName);
                    file.SaveAs(path);
                    tpam.ProjectTaskID = 0;
                    tpam.DirectoryName = Directory;
                    tpam.AttachmentName = fileName;
                    tpam.AddedTS = DateTime.Now;
                    tpam.AddedBy = loggedInUser;

                    //   _bl.savePressPhotoAlbum(pressID, fileName, out id);
                    tpam.InsertAttachmentdata(tpam, out id);
                }
            }
            return Json(Directory);
        }

        public ActionResult previewIssue(int IssueId)
        {
            ProjectIssueModel projectIssue = new ProjectIssueModel();

            try
            {

                DataTable dtIssueData = projectIssue.GetProjectIssueByIssueId(IssueId);
                if (dtIssueData.Rows.Count > 0)
                {
                    ViewBag.status = Convert.ToBoolean(dtIssueData.Rows[0]["isACTIVE"]) == true ? "Active" : "In Active";
                    ViewBag.ID= Convert.ToInt32(dtIssueData.Rows[0]["IssueID"]);
                    ViewBag.TicketNo = "TI"+Convert.ToInt32(dtIssueData.Rows[0]["IssueID"]);
                    ViewBag.IssueCode = dtIssueData.Rows[0]["IssueCode"].ToString() ?? "";
                    ViewBag.IssueName = dtIssueData.Rows[0]["IssueName"].ToString();
                    ViewBag.IssueDescription = dtIssueData.Rows[0]["IssueDescription"].ToString() ?? "";
                    ViewBag.ProjectName = dtIssueData.Rows[0]["ProjectName"].ToString();
                    ViewBag.ProjectID = Convert.ToInt32(dtIssueData.Rows[0]["ProjectID"]);
                    ViewBag.IssuestartDate = dtIssueData.Rows[0]["IssueStartDate"].ToString();
                    ViewBag.IssueEndDate = dtIssueData.Rows[0]["IssueEndDate"].ToString();

                    ViewBag.ExpectedTime = dtIssueData.Rows[0]["ExpectedTime"].ToString();
                    ViewBag.Timespent = dtIssueData.Rows[0]["Timespent"].ToString();

                    //if (ExpecterdHours != "")
                    //{
                    //    string[] Arr = new string[2];
                    //    Arr = ExpecterdHours.Split('.');
                    //    ViewBag.ExpectedTime = Arr[0] + ':' + Arr[1];
                    //}



                    ViewBag.TicketTypeName = dtIssueData.Rows[0]["TicketTypeName"].ToString();
                    ViewBag.ActualIssueStartDate = (dtIssueData.Rows[0]["IssueStartDateActual"] != System.DBNull.Value) ? dtIssueData.Rows[0]["IssueStartDateActual"] : (DateTime?)null;
                    ViewBag.ActualIssueEndDate = (dtIssueData.Rows[0]["IssueEndDateActual"] != System.DBNull.Value) ? dtIssueData.Rows[0]["IssueEndDateActual"] : (DateTime?)null;
                    ViewBag.StatusName = dtIssueData.Rows[0]["StatusName"].ToString() ?? "";
                    ViewBag.StatusID = Convert.ToInt32(dtIssueData.Rows[0]["StatusID"]);
                    ViewBag.SeverityID = Convert.ToInt32(dtIssueData.Rows[0]["SeverityID"]);
                    ViewBag.SeverityName = dtIssueData.Rows[0]["SeverityName"].ToString();
                    //ViewBag.IsActive = Convert.ToBoolean(dtIssueData.Rows[0]["isACTIVE"]);
                    ViewBag.IsValueAdded = Convert.ToBoolean(dtIssueData.Rows[0]["isValueAdded"]) == true ? "Yes" : "No";
                    ViewBag.CompletePercent = Convert.ToInt32(dtIssueData.Rows[0]["CompletePercent"]);
                    string usernameassigned = string.Empty;
                    foreach (DataRow item in dtIssueData.Rows)
                    {
                        UserModel userModel = new UserModel
                        {
                            ID = Convert.ToInt32(item["UserID"]),
                            Name = item["UserName"].ToString(),
                            IsActive = Convert.ToBoolean(item["IsUserActive"])

                        };

                        ViewBag.UserIdAssigned += (item["UserID"]).ToString() + ",";
                        usernameassigned += (item["UserName"]).ToString() + ", ";
                        //projectIssue.UserList.Add(userModel);
                    }
                    ViewBag.UserNameAssigned = usernameassigned.Remove(usernameassigned.Length - 2, 2);
                }
            }
            catch (Exception ex)
            {

                //throw;
            }

            return PartialView("_PreviewProjectIssue");
        }
        public string MailCommentsForIssues(int id)
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                ProjectIssueCommentModel acm = new ProjectIssueCommentModel();
                DataSet ds = acm.GetIssueCommentByIssueID(id);
                sbContent.Append("<div class='row'><div class='col-md-12'> <div class='panel panel-default'><div class='panel-body'><div class='row form-group'><div class='col-md-12'><div class='col-md-12'>");
                if (ds!=null && ds.Tables[0]!=null && ds.Tables[0].Rows.Count > 0)
                {
                    sbContent.Append("<section class='comments'>");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        //strApproverCommentDiv += "<div class='row'><div class='col-md-12'><div class='col-md-2'><label>"+dt.Rows[i]["CommentedBy"] +" wrote on:"+ Convert.ToDateTime(dt.Rows[i]["AddedTS"].ToString()) +"</label></div><div class='col-md-10'><label class='form-control'>" + dt.Rows[i]["ApproverComment"].ToString() + "</label></div></div></div>&nbsp;&nbsp;";
                        sbContent.Append("<article class='comment'><a class='comment-img' href='#non'><img src ='https://pbs.twimg.com/profile_images/444197466133385216/UA08zh-B.jpeg' alt = '' width = '50' height = '50'/></a><div class='comment-body'><div class='text'><p><b>Status:</b> " + ds.Tables[0].Rows[i]["StatusName"] + ".</p> <p><b>Comment:</b> " + ds.Tables[0].Rows[i]["Comment"].ToString() + "</p>");
                        //Load URL
                        if (ds.Tables[0].Rows[i]["url"].ToString() != "" && ds.Tables[0].Rows[i]["url"] != null)
                        {
                            string[] arrURL = ds.Tables[0].Rows[i]["url"].ToString().Split(';');
                            sbContent.Append( "<p><b>URL:</b>");
                            for (int j = 0; j < arrURL.Length; j++)
                            {
                                sbContent.Append( "<a class='img' target='_blank'  data-fancybox href = '" + arrURL[j] + "' >" + arrURL[j] + " </a></b> ");

                            }
                            sbContent.Append("</p>");
                        }
                        sbContent.Append( "</div><p class='attribution'>by<a href='#non'> " + ds.Tables[0].Rows[i]["UserName"] + " </a>" + ds.Tables[0].Rows[i]["AddedTS"].ToString() + "</p></div></article>");
                    }
                    sbContent.Append("</section>");


                }
                else
                {
                    sbContent.Append("<div>No Comments Found</div>");
                }
                sbContent.Append("</div></div></div></div></div></div></div>");
            }
            catch( Exception ex)
            {

            }
            return sbContent.ToString();

        }
        public ActionResult LoadCommentsForIssues(int id)
        {
            string strCommentDiv = string.Empty;
            ProjectIssueCommentModel acm = new ProjectIssueCommentModel();
            DataSet ds = acm.GetIssueCommentByIssueID(id);
            strCommentDiv += @"<div class='row'><div class='col-md-12'> <div class='panel panel-default'><div class='panel-heading'><h4> Status and discussion history</h4></div><div class='panel-body'><div class='row form-group'><div class='col-md-12'><div class='col-md-12'>";
            if (ds!=null && ds.Tables[0]!=null && ds.Tables[0].Rows.Count > 0)
            {
                strCommentDiv += @"<section class='comments'>";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strCommentDiv += @"<article class='comment'><a class='comment-img' href='#non'><img src ='https://pbs.twimg.com/profile_images/444197466133385216/UA08zh-B.jpeg' alt = '' width = '50' height = '50'/></a><div class='comment-body'><div class='text'><p><b>Status:</b> " + ds.Tables[0].Rows[i]["StatusName"]+".</p> <p><b>Comment:</b> "+ ds.Tables[0].Rows[i]["Comment"].ToString() + "</p>";
                 
                    //Load Attachment
                    if (ds != null && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0 )
                    {
                        for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                        {
                            if (ds.Tables[0].Rows[i]["ID"].ToString() == ds.Tables[1].Rows[j]["IssueCommentId"].ToString())
                            {
                                string path = Server.MapPath("~/IssueAttachments/" + ds.Tables[1].Rows[j]["DirectoryName"].ToString());
                                if (Directory.Exists(path))
                                {
                                    DirectoryInfo di = new DirectoryInfo(path);
                                    strCommentDiv += "<p><b>Attachments:</b>";

                                    strCommentDiv += "<a class='img' target='_blank'  data-fancybox href = '/IssueAttachments/" + ds.Tables[1].Rows[j]["DirectoryName"].ToString() + "/" + ds.Tables[1].Rows[j]["AttachmentName"].ToString() + "' >" + ds.Tables[1].Rows[j]["AttachmentName"].ToString() + "</a></p>";

                                }
                            }
                        }
                        
                    }

                    //Load URL
                    if (ds.Tables[0].Rows[i]["url"].ToString() != "" && ds.Tables[0].Rows[i]["url"] != null)
                    {

                        string[] arrURL = ds.Tables[0].Rows[i]["url"].ToString().Split(';');
                        strCommentDiv += "<p><b>URL:</b>";
                        for (int j = 0; j < arrURL.Length; j++)
                        {
                           // strCommentDiv += "<a class='img' target='_blank'  data-fancybox href = '" + arrURL[j] + "' >" + arrURL[j] + " </ a > &nbsp;";
                            strCommentDiv += "<a class='img' target='_blank'  href = '" + arrURL[j] + "'  rel='noopener noreferrer'>" + arrURL[j] + " </ a > &nbsp;";


                        }
                        strCommentDiv += "</p>";
                    }
                    strCommentDiv += "</div><p class='attribution'>by<a href='#non'> " + ds.Tables[0].Rows[i]["UserName"] + " </a>" + ds.Tables[0].Rows[i]["AddedTS"].ToString() + "</p></div></article>";
                }
                strCommentDiv += @"</section>";


            }
            else
            {
                strCommentDiv += "<div>No Comments Found</div>";
            }
            strCommentDiv += @"</div></div></div></div></div></div></div>";
            return Content(strCommentDiv);
        }

        public ActionResult GetProjectIssueByID(int IssueId)
        {
            ProjectIssueModel projectIssue = new ProjectIssueModel();

            try
            {

                DataTable dtIssueData = projectIssue.GetProjectIssueByIssueId(IssueId);
                if (dtIssueData.Rows.Count > 0)
                {

                    projectIssue.IssueId = Convert.ToInt32(dtIssueData.Rows[0]["IssueID"]);
                    projectIssue.IssueCode = dtIssueData.Rows[0]["IssueCode"].ToString() ?? "";
                    projectIssue.IssueName = dtIssueData.Rows[0]["IssueName"].ToString();
                    projectIssue.IssueDescription = dtIssueData.Rows[0]["IssueDescription"].ToString() ?? "";
                    projectIssue.ProjectName =dtIssueData.Rows[0]["ProjectName"].ToString();
                    projectIssue.ProjectID = Convert.ToInt32(dtIssueData.Rows[0]["ProjectID"]);

                    projectIssue.IssuestartDate = Convert.ToDateTime(dtIssueData.Rows[0]["IssueStartDate"]);
                    projectIssue.IssueEndDate = Convert.ToDateTime(dtIssueData.Rows[0]["IssueEndDate"]);

                    string ExpecterdHours  = dtIssueData.Rows[0]["ExpectedTime"].ToString();
                  
                    //if (ExpecterdHours != "")
                    //{
                    //    string[] Arr = new string[2];
                       
                    //    Arr = ExpecterdHours.Split('.');   // your input string
                       

                    //    projectIssue.ExpectedTime = Arr[0] + ':' + Arr[1];
                    //}

                    //projectIssue.TicketTypeName = dtIssueData.Rows[0]["TicketTypeName"].ToString();
                    projectIssue.ActualIssueStartDate = (dtIssueData.Rows[0]["IssueStartDateActual"] != System.DBNull.Value) ? Convert.ToDateTime(dtIssueData.Rows[0]["IssueStartDateActual"]) : (DateTime?)null;
                    projectIssue.ActualIssueEndDate = (dtIssueData.Rows[0]["IssueEndDateActual"] != System.DBNull.Value) ? Convert.ToDateTime(dtIssueData.Rows[0]["IssueEndDateActual"]) : (DateTime?)null;
                    projectIssue.StatusName = dtIssueData.Rows[0]["StatusName"].ToString() ?? "";
                    projectIssue.StatusID = Convert.ToInt32(dtIssueData.Rows[0]["StatusID"]);
                    projectIssue.TicketTypeID = Convert.ToInt32(dtIssueData.Rows[0]["TicketTypeID"]);
                    projectIssue.SeverityID = Convert.ToInt32(dtIssueData.Rows[0]["SeverityID"]);
                    projectIssue.SeverityName = dtIssueData.Rows[0]["SeverityName"].ToString();
                    projectIssue.IsActive = Convert.ToBoolean(dtIssueData.Rows[0]["isACTIVE"]);
                    projectIssue.IsValueAdded = Convert.ToBoolean(dtIssueData.Rows[0]["isValueAdded"]);
                    projectIssue.CompletePercent = Convert.ToInt32(dtIssueData.Rows[0]["CompletePercent"]);

                    foreach (DataRow item in dtIssueData.Rows)
                    {
                        UserModel userModel = new UserModel
                        {
                            ID = Convert.ToInt32(item["UserID"]),
                            Name = item["UserName"].ToString(),
                            IsActive = Convert.ToBoolean(item["IsUserActive"])

                        };

                        projectIssue.UserIdAssigned += (item["UserID"]).ToString() + ",";
                        projectIssue.UserNameAssigned += (item["UserName"]).ToString() + ",";
                        projectIssue.UserList.Add(userModel);
                    }
                    projectIssue.UserIdAssigned = projectIssue.UserIdAssigned.TrimEnd(',');
                    // task.TaskList.Add(task);
                }

            }
            catch (Exception ex)
            {

                //throw;
            }
            return Json(JsonConvert.SerializeObject(projectIssue));


        }

        public ActionResult LoadProjectTaskStatus(int Taskid)
        {
            ProjectTaskModel task = new ProjectTaskModel();

            try
            {

                UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);
                DataTable dtTaskData = task.GetProjectTasksByTaskId(Taskid);

                task.ActualTaskStartDateDisplayforstatus = dtTaskData.Rows[0]["TaskStartDateActual"].ToString();

                task.ActualTaskStartDateDisplayforstatus = dtTaskData.Rows[0]["TaskEndDateActual"].ToString();
               // task.ExpectedTime =Convert.ToDouble(dtTaskData.Rows[0]["ExpectedTime"].ToString());
                //return Json(JsonConvert.SerializeObject(issueModel));

                DataTable dt = task.GetProjectTaskStatusList(userInfo.UserOrganisationID);
                strStatusData += "<option value = 0>Please select</option>";
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        strStatusData += "<option value=" + Convert.ToInt32(item["StatusID"]) + ">" + Convert.ToString(item["StatusName"]) + "</option>";
                    }
                }

            }
            catch (Exception ex)
            {

                //throw;
            }

            return Json(strStatusData + '|' + JsonConvert.SerializeObject(task));
        }

        public ActionResult SaveProjectTask(ProjectTaskModel model)
        {
            ProjectTaskModel pm = new ProjectTaskModel();
            string result = "";
            try
            {

                if (model.TaskId > 0)
                {

                    model.EditedBy = loggedInUser;
                    model.EditedTS = DateTime.Now;
                    if (model.TaskStartDateDisplay != null)
                    {
                        model.TaskStartDate = DateTimeHelper.ConvertStringToValidDate(model.TaskStartDateDisplay);
                    }
                    if (model.TaskEndDateDisplay != null)
                    {
                        model.TaskEndDate = DateTimeHelper.ConvertStringToValidDate(model.TaskEndDateDisplay);
                    }
                    if (model.ActualTaskStartDateDisplay != null)
                    {
                        model.ActualTaskStartDate = DateTimeHelper.ConvertStringToValidDate(model.ActualTaskStartDateDisplay);
                    }
                    if (model.ActualTaskEndDateDisplay != null)
                    {
                        model.ActualTaskEndDate = DateTimeHelper.ConvertStringToValidDate(model.ActualTaskEndDateDisplay);

                    }


                    var updateStatus = pm.UpdateTaskdata(model);

                    if (updateStatus)
                    {
                        model.ISErr = false;

                        model.ErrString = "Data Saved Successfully.";
                        TempData["ErrStatus"] = model.ISErr;
                        TempData["ErrMsg"] = model.ErrString.ToString();
                        result = "Success";
                    }
                    else
                    {
                        model.ISErr = true;
                        model.ErrString = "Error Occured.";
                        TempData["ErrStatus"] = model.ISErr;
                        TempData["ErrMsg"] = model.ErrString.ToString();
                        result = "Error";
                    }

                }

                else
                {
                    //TODO check the time format and the insert
                    if (model.TaskStartDateDisplay != null)
                    {
                        model.TaskStartDate = DateTimeHelper.ConvertStringToValidDate(model.TaskStartDateDisplay);
                    }
                    if (model.TaskEndDateDisplay != null)
                    {
                        model.TaskEndDate = DateTimeHelper.ConvertStringToValidDate(model.TaskEndDateDisplay);
                    }
                    if (model.ActualTaskStartDateDisplay != null)
                    {
                        model.ActualTaskStartDate = DateTimeHelper.ConvertStringToValidDate(model.ActualTaskStartDateDisplay);
                    }
                    if (model.ActualTaskEndDateDisplay != null)
                    {
                        model.ActualTaskEndDate = DateTimeHelper.ConvertStringToValidDate(model.ActualTaskEndDateDisplay);

                    }

                    model.AddedBy = loggedInUser;
                    model.AddedTS = DateTime.Now;
                    var insertStatus = pm.InsertTaskdata(model, out int id);
                    if (insertStatus)
                    {
                        if (id > 0)
                        {

                            if (model.DirectoryName != null)
                            {
                                ProjectTaskAttachmentModel ptam = new ProjectTaskAttachmentModel();

                                int outCommentID = 0;
                                ProjectTaskCommentModel ptcm = new ProjectTaskCommentModel();
                                ptcm.ProjectTaskID = id;
                                ptcm.Comment = "";
                                ptcm.TaskStatusID = model.TaskStatusID;
                                ptcm.AddedBy = loggedInUser;
                                ptcm.AddedTS = DateTime.Now;
                                ptcm.InsertCommentdata(ptcm, out outCommentID);

                                if (model.DirectoryName != null)
                                {
                                  //  ProjectTaskAttachmentModel ptam = new ProjectTaskAttachmentModel();
                                    ptam.ProjectTaskCommentID = outCommentID;

                                    ptam.DirectoryName = model.DirectoryName.Replace("\"", string.Empty).Trim();
                                    ptam.ProjectTaskID = id;
                                    ptam.URL = model.URL;
                                    ptam.UpdateAttachmentsdataWithProjectTaskID(ptam);
                                }


                                result = "Success";
                            }
                        }
                        else
                        {
                            result = "Error";
                        }
                    }
                }
            }
            catch (Exception)
            {
                result = "Error";
                return Json(result);

            }
            return Json(result);
        }
        public ActionResult UpdateStatusTask(ProjectTaskModel model)
        {
            ProjectTaskModel pm = new ProjectTaskModel();
            string result = "";
            try
            {

                if (model.TaskIdforstatus > 0)
                {

                    model.EditedBy = loggedInUser;
                    model.EditedTS = DateTime.Now;

                    if (model.ActualTaskStartDateDisplayforstatus != null)
                    {
                        model.ActualTaskStartDate = DateTimeHelper.ConvertStringToValidDate(model.ActualTaskStartDateDisplayforstatus);
                    }
                    if (model.ActualTaskEndDateDisplayforstatus != null)
                    {
                        model.ActualTaskEndDate = DateTimeHelper.ConvertStringToValidDate(model.ActualTaskEndDateDisplayforstatus);

                    }
                    model.ExpectedTime = Convert.ToDouble(model.ExpectedTime);
                    var updateStatus = model.UpdateTaskstatus(model);


                    //if (model.DirectoryName != null)
                    //{
                        ProjectTaskAttachmentModel ptam = new ProjectTaskAttachmentModel();

                        int outCommentID = 0;
                        ProjectTaskCommentModel ptcm = new ProjectTaskCommentModel();
                        ptcm.ProjectTaskID = model.TaskIdforstatus;
                        ptcm.Comment = model.Comment;
                        ptcm.TaskStatusID = model.TaskStatusID;
                        ptcm.AddedBy = loggedInUser;
                        ptcm.AddedTS = DateTime.Now;
                        ptcm.InsertCommentdata(ptcm, out outCommentID);

                        if (outCommentID >0)
                        {
                            //  ProjectTaskAttachmentModel ptam = new ProjectTaskAttachmentModel();
                            ptam.ProjectTaskCommentID = outCommentID;
                        if (model.DirectoryName != null)
                        {
                            ptam.DirectoryName = model.DirectoryName.Replace("\"", string.Empty).Trim();
                        }
                        else
                        {
                            ptam.DirectoryName = null;
                        } 
                            ptam.ProjectTaskID = model.TaskIdforstatus;
                            ptam.URL = model.URL;
                            ptam.AddedBy = loggedInUser;
                            ptam.AddedTS = DateTime.Now;
                            ptam.UpdateAttachmentsdataWithProjectTaskID(ptam);
                        }
                    //}


                        

                    if (updateStatus)
                    {
                        model.ISErr = false;
                        model.ErrString = "Data Saved Successfully.";
                        TempData["ErrStatus"] = model.ISErr;
                        TempData["ErrMsg"] = model.ErrString.ToString();
                        result = "Success";
                    }
                    else
                    {
                        model.ISErr = true;
                        model.ErrString = "Error Occured.";
                        TempData["ErrStatus"] = model.ISErr;
                        TempData["ErrMsg"] = model.ErrString.ToString();
                        result = "Error";
                    }

                }


            }
            catch (Exception)
            {
                result = "Error";
                return Json(result);

            }
            return Json(result);
        }
        public ActionResult UpdateStatusIssue(ProjectIssueModel model)
        {
            ProjectIssueModel pm = new ProjectIssueModel();
            string result = "";
            string strMailToName = string.Empty;
            string strMailTo = string.Empty;
            try
            {

                if (model.IssueIdforstatus > 0)
                {

                    model.EditedBy = loggedInUser;
                    model.EditedTS = DateTime.Now;
                 
                    if (model.ActualIssueStartDateDisplayforstatus != null)
                    {
                        model.ActualIssueStartDate = DateTimeHelper.ConvertStringToValidDate(model.ActualIssueStartDateDisplayforstatus);
                    }
                    if (model.ActualIssueEndDateDisplayforstatus != null)
                    {
                        model.ActualIssueEndDate = DateTimeHelper.ConvertStringToValidDate(model.ActualIssueEndDateDisplayforstatus);

                    }

                   var updateStatus = model.UpdateIssuestatus(model, out strMailToName, out strMailTo);

                    if (updateStatus)
                    {
                            #region Mail Sending
                            ProjectIssueCommentModel acm = new ProjectIssueCommentModel();
                            DataSet dtIssueComment = acm.GetIssueCommentByIssueID(model.IssueIdforstatus);

                             string []MailToName_arr = strMailToName.Split(';');
                             string[] MailTo_arr = strMailTo.Split(';');
                            if (dtIssueComment !=null && dtIssueComment.Tables[0] != null && dtIssueComment.Tables[0].Rows.Count > 0)
                            {
                            #region :  
                            string strBody = "";
                            string strSubject = "";
                            for (int i = 0; i < MailToName_arr.Length; i++)
                            {
                                 strSubject = @"Project Issue Status and discussion history";
                                 strBody = string.Format(@"Dear {0},
                                                        <br><br>
                                                        {1}
                                                        <br><br>
                                                        Please login to timetracker System to view the status Details .
                                                        <br><br>
                                                        Thanks & Regards,<br>
                                                        QBA Administrator
                                                        <br><br><br><br>
                                                        *This is a system generated email. Please do not respond.
                                                        ", MailToName_arr[i], MailCommentsForIssues(model.IssueIdforstatus));

                                 using (SendMailClass sm = new SendMailClass())
                                 { sm.SendMail(MailTo_arr[i], strSubject, strBody, ConfigurationManager.AppSettings["smtpFrom"], ConfigurationManager.AppSettings["smtpPass"]); }
                              
                            }
                                #endregion
                            }
                        model.ISErr = false;
                        model.ErrString = "Data Saved Successfully.";
                        TempData["ErrStatus"] = model.ISErr;
                        TempData["ErrMsg"] = model.ErrString.ToString();
                        result = "Success";
                        #endregion
                    }
                    else
                    {
                        model.ISErr = true;
                        model.ErrString = "Error Occured.";
                        TempData["ErrStatus"] = model.ISErr;
                        TempData["ErrMsg"] = model.ErrString.ToString();
                        result = "Error";
                    }

                }

                
            }
            catch (Exception)
            {
                result = "Error";
                return Json(result);

            }
            return Json(result);
        }
        public ActionResult SaveProjectIssue(ProjectIssueModel model)
        {
            
            ProjectIssueModel pm = new ProjectIssueModel();
            string result = "";
            try
            {

                if (model.IssueId > 0)
                {

                    model.EditedBy = loggedInUser;
                    model.EditedTS = DateTime.Now;
                    if (model.IssueStartDateDisplay != null)
                    {
                        model.IssuestartDate = DateTimeHelper.ConvertStringToValidDate(model.IssueStartDateDisplay);
                    }
                    if (model.IssueEndDateDisplay != null)
                    {
                        model.IssueEndDate = DateTimeHelper.ConvertStringToValidDate(model.IssueEndDateDisplay);
                    }
                    if (model.ActualIssueStartDateDisplay != null)
                    {
                        model.ActualIssueStartDate = DateTimeHelper.ConvertStringToValidDate(model.ActualIssueStartDateDisplay);
                    }
                    if (model.ActualIssueEndDateDisplay != null)
                    {
                        model.ActualIssueEndDate = DateTimeHelper.ConvertStringToValidDate(model.ActualIssueEndDateDisplay);

                    }



                    var updateStatus = pm.UpdateIssuedata(model);

                    if (updateStatus)
                    {
                        model.ISErr = false;
                        model.ErrString = "Data Saved Successfully.";
                        TempData["ErrStatus"] = model.ISErr;
                        TempData["ErrMsg"] = model.ErrString.ToString();
                        result = "Success";
                    }
                    else
                    {
                        model.ISErr = true;
                        model.ErrString = "Error Occured.";
                        TempData["ErrStatus"] = model.ISErr;
                        TempData["ErrMsg"] = model.ErrString.ToString();
                        result = "Error";
                    }

                }

                else
                {
                    //TODO check the time format and the insert
                    if (model.IssueStartDateDisplay != null)
                    {
                        model.IssuestartDate = DateTimeHelper.ConvertStringToValidDate(model.IssueStartDateDisplay);
                    }
                    if (model.IssueEndDateDisplay != null)
                    {
                        model.IssueEndDate = DateTimeHelper.ConvertStringToValidDate(model.IssueEndDateDisplay);
                    }
                    if (model.ActualIssueStartDateDisplay != null)
                    {
                        model.ActualIssueStartDate = DateTimeHelper.ConvertStringToValidDate(model.ActualIssueStartDateDisplay);
                    }
                    if (model.ActualIssueEndDateDisplay != null)
                    {
                        model.ActualIssueEndDate = DateTimeHelper.ConvertStringToValidDate(model.ActualIssueEndDateDisplay);

                    }


                    model.AddedBy = loggedInUser;
                    model.AddedTS = DateTime.Now;
                    var insertStatus = pm.InsertIssuedata(model, out int id);//out string strMailToName,out string strMailTo);
                    if (insertStatus)
                    {
                        if (id > 0)
                        {

                            ProjectIssueModel projectIssueModel = new ProjectIssueModel();
                            projectIssueModel.IssueIdforstatus = id;
                            projectIssueModel.Comment = "";
                            projectIssueModel.StatusID = model.StatusID;
                            projectIssueModel.url = model.url;
                            projectIssueModel.DirectoryName = model.DirectoryName;
                            projectIssueModel.Duration = 0;
                            projectIssueModel.ActualIssueStartDate = model.ActualIssueStartDate;
                            projectIssueModel.ActualIssueEndDate = model.ActualIssueEndDate;
                            projectIssueModel.EditedBy = loggedInUser;
                            projectIssueModel.EditedTS = DateTime.Now;
                            var updateStatus = projectIssueModel.UpdateIssuestatus(projectIssueModel, out string strMailToName, out string strMailTo);
                         
                            if (updateStatus)
                            {
                                UserModel userModel = new UserModel();
                                DataTable dtUSER = userModel.GetUsersByID(int.Parse(HttpContext.Session["sessUser"].ToString()));
                                userModel.UserName = dtUSER.Rows[0]["Name"].ToString();

                                string[] usernameArr = strMailToName.Split(';');
                                string[] userEmailArr = strMailTo.Split(';');
                                for (int j = 0; j < usernameArr.Length; j++)
                                {
                                    sendMail_afterSaveTicket(usernameArr[j], userEmailArr[j], "Ticket has been assigned to you on project " + model.ProjectName + " by " + userModel.UserName);
                                }
                                model.ISErr = false;
                                model.ErrString = "Data Saved Successfully.";
                                TempData["ErrStatus"] = model.ISErr;
                                TempData["ErrMsg"] = model.ErrString.ToString();
                                result = "Success";
                            }
                        }
                    }
                    else
                    {
                        model.ISErr = true;
                        model.ErrString = "Error Occured.";
                        TempData["ErrStatus"] = model.ISErr;
                        TempData["ErrMsg"] = model.ErrString.ToString();
                        result = "Error";
                    }
                }
            }
            catch (Exception)
            {
                result = "Error";
                return Json(result);

            }
             return Json(result);
            //return RedirectToAction("ManageProjectIssue", "Admin");
        }


        //public ActionResult GetAllUserOfOrganisationByProjectID(int projectID)
        //{
        //    ProjectTaskModel obj = new ProjectTaskModel();

        //    DataTable dt = new DataTable();
        //    dt = obj.GetAllUserOfOrganisationByProjectID(projectID);
        //    StringBuilder builder = new StringBuilder();
        //    builder.Append(@"<div id='divUserList' class='row' style='margin:10px;'>");
        //    if (dt.Rows.Count > 0)
        //    {

        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            builder.Append(@"<div style='float: left;width: 25%; padding: 5px;'>");
        //            builder.Append(@"<input type='checkbox' class='check' style=' margin:5px;' name='modules' value='" + dr["Id"].ToString() + "'>");

        //            builder.Append("<span id='span_" + dr["Id"].ToString() + "'> " + dr["Name"].ToString() + "</span>");

        //            builder.Append(@"</div>");
        //        }
        //        builder.Append(@"</div>");
        //    }
        //    else
        //    {
        //        builder.Append(@"<div style='float: left;width: 100%; padding: 5px;'>");
        //        builder.Append("<h5 >" + "No users available" + "</h5>");
        //        builder.Append(@"</div>");
        //    }

        //    return Json(builder.ToString());
        //}

        public ActionResult SaveProjectUserMapping(UserProjectMappingModel[] itemlist)
        {
            // return Content("test");

            //foreach (UserProjectMappingModel i in itemlist)   //loop through the array and insert value into database.
            //{
            //    UserProjectMappingModel mm = new UserProjectMappingModel();

            //    mm.UserId = i.UserId;
            //    mm.ProjectId = i.ProjectId;
            //    bool result = mm.InsertUserProjectMappingdata(mm);
            //    if (!result)
            //    {
            //        return Json("Error");
            //    }
            //}

            //return Json("success");

            ProjectModel UPM = new ProjectModel();
            int UserID = itemlist[0].UserId;
            try
            {
                DataTable dt = UPM.GetAllUserByProjectID(itemlist[0].ProjectId);

                if (dt.Rows.Count > 0)
                {
                    UPM.DeleteAllExistingMappingByProjectID(itemlist[0].ProjectId);
                }

                foreach (UserProjectMappingModel i in itemlist)
                {
                    UserProjectMappingModel mm = new UserProjectMappingModel();

                    mm.UserId = i.UserId;
                    mm.ProjectId = i.ProjectId;
                    mm.InsertUserProjectMappingdata(mm);
                }

            }
            catch (Exception)
            {
                //return Json(false);
                throw;
            }
            return Json("success");
        }
        #endregion

        #endregion

        #region User Project Mapping region
        public ActionResult UserProjectMapping()
        {
            return View();
        }


        public ActionResult LoadALLUserMapped()
        {
            StringBuilder strProjectMapped = new StringBuilder();
            var loggedInUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
            UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);

            UserProjectMappingModel USM = new UserProjectMappingModel();
            DataTable dt = new DataTable();
            if (userInfo.IsRoleSysAdmin)
            {
                var dtActiveUsers = USM.GetAllUsers().Select("IsActive=1 AND OrganisationStatus=1");
                if (dtActiveUsers.Length > 0)
                {
                    dt = dtActiveUsers.CopyToDataTable();
                }

            }
            else
            {
                var dtActiveUsers = USM.GetAllUsers(userInfo.UserOrganisationID).Select("IsActive=1 AND OrganisationStatus=1");
                if (dtActiveUsers.Length > 0)
                {
                    dt = dtActiveUsers.CopyToDataTable();
                }

            }
            if (dt.Rows.Count > 0)
            {


                foreach (DataRow dr in dt.Rows)
                {
                    //int i = 0;

                    strProjectMapped.Append("<tr>");
                    strProjectMapped.Append("<td align='center'>" + dr["ID"].ToString() + "</td>");
                    strProjectMapped.Append("<td align='center'>" + dr["Name"].ToString() + "</td>");
                    if (dr["UserCode"] != DBNull.Value)
                        strProjectMapped.Append("<td align='center'>" + dr["UserName"]?.ToString() + " - " + dr["UserCode"]?.ToString() + "</td>");
                    else
                        strProjectMapped.Append("<td align='center'>" + dr["UserName"]?.ToString() + "</td>");

                    strProjectMapped.Append("<td align='center'>" + dr["Designation"].ToString() + "</td>");
                    strProjectMapped.Append("<td align='center'>" + dr["DepartmentName"]?.ToString() + "</td>");
                    strProjectMapped.Append("<td align = 'center' > " + dr["orgname"]?.ToString() + "</td>");
                    strProjectMapped.Append("<td align='center'><button data-toggle='modal' data-target='#myModalForModule' onclick='ShowPermission(" + dr["ID"].ToString() + ")'>Map</button></td>");
                    strProjectMapped.Append("</tr>");

                }
            }


            return Content(strProjectMapped.ToString());
        }

        //public ActionResult LoadAllModules(int Userid)
        //{
        //    UserProjectMappingModel obj = new UserProjectMappingModel();


        //    UserInfoHelper userInfo = new UserInfoHelper(Userid);
        //    DataTable dt = new DataTable();

        //    if (userInfo.IsRoleSysAdmin)
        //    {

        //        var dataRows = obj.GetAllProjects().Select("IsActive=1");
        //        if (dataRows.Length > 0)
        //        {
        //            dt = dataRows.CopyToDataTable();
        //        }
        //    }
        //    else
        //    {
        //        var dataRows = obj.GetAllProjects(userInfo.UserOrganisationID).Select("IsActive=1");
        //        if (dataRows.Length > 0)
        //        {
        //            dt = dataRows.CopyToDataTable();
        //        }
        //    }


        //    if (Session["AllProjectList"] != null)
        //    {
        //        Session.Remove("AllProjectList");
        //        Session["AllProjectList"] = dt;
        //    }
        //    else
        //    {
        //        Session["AllProjectList"] = dt;
        //    }


        //    string strModules = @"<div id='divProjectList' class='row' style='margin:10px;'>";
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        strModules += @"<div style='float: left;width: 25%; padding: 5px;'>";
        //        strModules += "<input type='checkbox' class='check' style=' margin:5px;' name='modules' value='" + dr["Id"].ToString() + "'>" + dr["Name"].ToString();
        //        strModules += "</div>";
        //    }
        //    strModules += @"</div>";

        //    return Json(strModules);
        //}

        public ActionResult LoadAllModules(int Userid)
        {
            UserProjectMappingModel obj = new UserProjectMappingModel();
            StringBuilder builder = new StringBuilder();

            try
            {
                UserInfoHelper userInfo = new UserInfoHelper(Userid);
                DataTable dt = new DataTable();
                var data = obj.GetAllProjectMappedStatusByUserID(Userid, userInfo.UserOrganisationID);
                builder.Append(@"<div id='divProjectList' class='row' style='margin:10px;'>");
                if (data.Rows.Count > 0)
                {
                    foreach (DataRow row in data.Rows)
                    {
                        builder.Append(@"<div style='float: left;width: 25%; padding: 5px;'>");
                        if (Convert.ToBoolean(row["IsMapped"]))
                        {
                            builder.AppendFormat($"<input type = 'checkbox' class='check' style=' margin:5px;' name='modules' value={row["Id"].ToString()} checked>{row["Name"].ToString()}");
                        }
                        else
                        {
                            builder.AppendFormat($"<input type = 'checkbox' class='check' style=' margin:5px;' name='modules' value={row["Id"].ToString()} >{row["Name"].ToString()}");
                        }

                        builder.Append(@"</div>");
                    }
                }
                else
                {
                    builder.Append(@"<h5>No Projects available</h5>");
                }

                builder.Append(@"</div>");
            }
            catch (Exception)
            {
                throw;
            }
            return Json(builder.ToString());
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

                //throw;
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

                //throw;
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
                                     "<td class='text-center'>" + Status + "</td>";
                    if (dr["OrgID"].ToString() != "")
                        strUserData += "<td class='text-center'><a href = 'ManageRole?ID=" + dr["ID"].ToString() + "'>Edit </a> </td>";
                    else
                        strUserData += "<td class='text-center'></td>";
                    strUserData += "</tr>";
                    i++;
                }
            }
            catch (Exception ex)
            {

                //throw;
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
            StringBuilder strProjectMapped = new StringBuilder();
            UserProjectMappingModel USM = new UserProjectMappingModel();
            UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);
            DataTable dtUsers = new DataTable();
            try
            {
                if (userInfo.IsRoleSysAdmin)
                {
                    var dtActiveUsers = USM.GetAllUsers().Select("IsActive=1 AND OrganisationStatus=1");
                    if (dtActiveUsers.Length > 0)
                    {
                        dtUsers = dtActiveUsers.CopyToDataTable();
                    }
                }
                else
                {
                    var dtActiveUsers = USM.GetAllUsers(userInfo.UserOrganisationID).Select("IsActive=1 AND OrganisationStatus=1");
                    if (dtActiveUsers.Length > 0)
                    {
                        dtUsers = dtActiveUsers.CopyToDataTable();
                    }

                }

                foreach (DataRow dr in dtUsers.Rows)
                {
                    //int i = 0;

                    strProjectMapped.Append("<tr>");
                    strProjectMapped.Append("<td align='center'>" + dr["ID"].ToString() + "</td>");
                    strProjectMapped.Append("<td align='center'>" + dr["Name"].ToString() + "</td>");
                    if (dr["UserCode"] != DBNull.Value)
                        strProjectMapped.Append("<td align='center'>" + dr["UserName"]?.ToString() + " - " + dr["UserCode"]?.ToString() + "</td>");
                    else
                        strProjectMapped.Append("<td align='center'>" + dr["UserName"]?.ToString() + "</td>");

                    strProjectMapped.Append("<td align='center'>" + dr["Designation"].ToString() + "</td>");
                    strProjectMapped.Append("<td align='center'>" + dr["DepartmentName"]?.ToString() + "</td>");
                    strProjectMapped.Append("<td align = 'center' > " + dr["orgname"]?.ToString() + "</td>");
                    strProjectMapped.Append("<td align='center'><button data-toggle='modal' data-target='#myModalForModule' onclick='ShowRoleMapped(" + dr["ID"].ToString() + ")'>Map</button></td>");
                    strProjectMapped.Append("</tr>");
                    //i++;
                }
            }
            catch (Exception)
            {

                //throw;
            }


            return Content(strProjectMapped.ToString());
        }
        public ActionResult LoadAllRoles(int userID)
        {
            UserRoleMappingModel obj = new UserRoleMappingModel();

            UserInfoHelper userInfo = new UserInfoHelper(userID);

            DataTable dtRoles = new DataTable();
            StringBuilder builder = new StringBuilder();

            try
            {
                dtRoles = obj.GetAllRolesMappedStatusByUserID(userID, userInfo.UserOrganisationID);
                builder.Append(@"<div id='divProjectList' class='row' style='margin:10px;'>");
                foreach (DataRow item in dtRoles.Rows)
                {
                    builder.Append(@"<div style='float: left;width: 25%; padding: 5px;'>");
                    if (Convert.ToBoolean(item["IsMapped"]))
                    {
                        builder.AppendFormat($"<input type = 'checkbox' class='check' style=' margin:5px;' name='modules' value={item["Id"].ToString()} checked >{item["Name"].ToString()}");
                    }
                    else
                    {
                        builder.AppendFormat($"<input type = 'checkbox' class='check' style=' margin:5px;' name='modules' value={item["Id"].ToString()} >{item["Name"].ToString()}");
                    }
                    builder.Append(@"</div>");
                }

            }
            catch (Exception)
            {

                throw;
            }

            return Json(builder.ToString());
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
                    UserRoleMappingModel mm = new UserRoleMappingModel
                    {
                        UserId = i.UserId,
                        RoleId = i.RoleId
                    };
                    mm.InsertUserRoleMappingdata(mm);
                }
            }
            catch (Exception)
            {
                return Json(false);
                //throw;
            }
            return Json(true);
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

            UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);
            RoleMouduleMappingModel USM = new RoleMouduleMappingModel();
            DataTable dt = new DataTable();
            if (userInfo.IsRoleSysAdmin)
            {
                dt = USM.GetAllRoles();
                //var datarow = USM.GetAllRoles().Select("IsActive=1");
                //if (datarow.Length > 0)
                //{
                //    dt = datarow.CopyToDataTable();
                //}
            }
            else
            {
                dt = USM.GetAllRoles(userInfo.UserOrganisationID);

                //var datarow = USM.GetAllRoles(userInfo.UserOrganisationID).Select("IsActive=1");
                //if (datarow.Length > 0)
                //{
                //    dt = datarow.CopyToDataTable();
                //}
            }

            foreach (DataRow dr in dt.Rows)
            {
                var status = Convert.ToBoolean(dr["IsActive"]);
                int i = 0;

                strProjectMapped += "<tr>";
                strProjectMapped += "<td align='center'>" + dr["ID"].ToString() + "</td><td align='center'>" + dr["Name"].ToString() + "</td><td align='center'>" + dr["orgname"].ToString() + "</td>";
                if (status)
                {
                    strProjectMapped += "<td align='center'><button data-toggle='modal' data-target='#myModalForModule'  onclick='ShowPermission(" + dr["Id"].ToString() + ")'>Map</button></td>";

                }
                else
                {
                    strProjectMapped += "<td align='center'><button data-toggle='modal' data-target='#myModalForModule' disabled style='background-color:gray; color:white;'  onclick='ShowPermission(" + dr["Id"].ToString() + ")'>Map</button></td>";

                }
                strProjectMapped += "</td>";
                i++;
            }

            return Content(strProjectMapped);
        }


        public ActionResult LoadAllSysModules()
        {
            RoleMouduleMappingModel obj = new RoleMouduleMappingModel();
            StringBuilder strModules = new StringBuilder();

            try
            {
                DataTable dt = obj.GetAllModules();


                if (dt.Rows.Count > 0)
                {
                    List<ModuleModel> moduleList = obj.ConvertModuleDatatableToList(dt);

                    var ParentModules = moduleList.Where(x => x.ParentID == 0).ToList();
                    var ChildModules = moduleList.Where(x => x.ParentID != 0).ToList();

                    strModules.Append(@"<div id='divModuleList' class='row' style='margin:10px;'>");

                    foreach (ModuleModel item in ParentModules)
                    {

                        strModules.Append(@"<div style='float: left;width: 25%; padding: 5px;'>");
                        strModules.Append("<input type='checkbox' class='check' style=' margin:5px;' name='modules' value='" + item.ID + "'>");
                        //strModules.Append(item.DisplayName);
                        strModules.Append("<span>" + item.DisplayName + "</span>");
                        strModules.Append(LoadChildModules(item.ID, ChildModules));

                        strModules.Append("</div>");

                    }
                    strModules.Append("</div>");

                }

            }
            catch (Exception)
            {
                //throw;
            }

            return Content(strModules.ToString());
        }

        public string LoadChildModules(int parentId, List<ModuleModel> childModules)
        {
            StringBuilder stringBuilder = new StringBuilder();

            try
            {
                var childs = childModules.Where(x => x.ParentID == parentId).OrderBy(x => x.Rank).ToList();
                if (childs.Count > 0)
                {
                    stringBuilder.Append("<ul class='module' style='list-style: none; margin:5px;'>");
                    foreach (ModuleModel item in childs)
                    {
                        stringBuilder.Append("<li class='limodule' style='list-style: none;margin:2px;'>");
                        stringBuilder.Append("<input type='checkbox' class='check' style=' margin:2px;' name='modules' value='" + item.ID + "'>" + item.DisplayName);

                        var innerChild = childModules.Where(x => x.ParentID == item.ID).OrderBy(x => x.Rank).ToList();
                        if (innerChild.Count > 0)
                        {
                            stringBuilder.Append(LoadChildModules(item.ID, childModules));
                        }
                        stringBuilder.Append("</li>");
                    }
                    stringBuilder.Append("</ul>");
                }
            }
            catch (Exception)
            {

                //throw;
            }


            return stringBuilder.ToString();
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
                    mm.CreatedBy = System.Web.HttpContext.Current.Session["sessUser"]?.ToString();
                    mm.CreateDate = DateTime.Now;
                    mm.InsertRoleModuleMappingdata(mm);
                }


            }
            catch (Exception)
            {

                //throw;
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
                    org.NoOfUserLicense =Convert.ToInt32(dt.Rows[0]["NoOfUserLicense"]);
                    org.logo = dt.Rows[0]["logo"].ToString();
                    org.isActive = Convert.ToBoolean(dt.Rows[0]["isActive"]);
                    org.createdBy = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
                    org.createdTS = DateTime.Now;
                }
                else
                {
                    org.orgname = "";
                    org.wikiurl = "";
                    org.NoOfUserLicense = 0;
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
                    strOrganisation += "<tr>" +
                        "<td class='text-center'>" + dr["id"].ToString() + "</td>" +
                        "<td class='text-center'>" + dr["orgname"] + "</td>" +
                        "<td class='text-center'>" + dr["address"].ToString() + "</td>" +
                        "<td class='text-center'>" + dr["contact_email_id"].ToString() + "</td>" +
                        "<td class='text-center'>" + dr["NoOfUserLicense"].ToString() + "</td>" +
                        "<td class='text-center'>" + dr["isActive"].ToString() + "</td>" +
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
                    strOrganisation += "<tr><td class='text-center'>" + dr["id"].ToString() + "</td><td class='text-center'>" + dr["Name"] + "</td><td class='text-center'>" + dr["ParentModule"] + "</td>" + "<td class='text-center'>" + dr["URL"].ToString() + "</td>" +
                        "<td class='text-center'>" + dr["DisplayName"].ToString() + "</td>" + "<td class='text-center'><i class='" + dr["DisplayCSS"].ToString() + "'></i>  " + dr["DisplayCSS"].ToString() + "</td>" + "<td class='text-center'>" + dr["isActive"].ToString() + "</td>" +
                       "<td  class='text-center'><a href = 'ManageModule?ID=" + dr["ID"].ToString() + "'>Edit</a></td></tr>";
                    i++;
                }
            }
            catch (Exception exc)
            {
                //throw exc;
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

                //throw;
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


        #region Client managment region
        public ActionResult ManageClient(int ID = 0)
        {
            ClientModel clientModel = new ClientModel();
           
            try
            {
                DataTable MgrDetlDT = new DataTable();
                UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);
                clientModel.OrganisationList.Clear(); 

                if (!userInfo.IsRoleSysAdmin)
                {
                    var organisation = clientModel.GetAllOrgInList().FirstOrDefault(x => x.id == userInfo.UserOrganisationID && x.isActive == true);
                    clientModel.OrganisationList.Add(organisation);
                    clientModel.ClientOrganisationID = userInfo.UserOrganisationID;
                   
                }
                else
                {
                    var list = clientModel.GetAllOrgInList().Where(x => x.isActive == true).ToList();
                    if (list.Count > 0)
                        clientModel.OrganisationList = list;
                    

                }
                if (ID > 0)
                {
                    DataTable dt = new DataTable();
                   
                    dt = clientModel.GetClientByID(ID);
                    MgrDetlDT = clientModel.GetClientManagerByID(ID);
                    if (dt.Rows.Count > 0)
                    {
                        clientModel.ClientID = ID;
                        clientModel.ClientName = dt.Rows[0]["ClientName"].ToString();
                        clientModel.ClientCode = dt.Rows[0]["ClientCode"].ToString();
                        clientModel.ClientAddress= dt.Rows[0]["ClientAddress"].ToString();
                        clientModel.ClientPhno= dt.Rows[0]["ClientContactNo"].ToString();
                        clientModel.Location= dt.Rows[0]["ClientLocation"].ToString();
                        clientModel.ClientOrganisationID = Convert.ToInt32(dt.Rows[0]["ORGID"]);
                        clientModel.IsActive = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
                    }
                   
                }
         

                StringBuilder sbOut = new StringBuilder();
                ViewBag.HtmlStr = sbOut.Append(BindClintManagerDetail(ID.ToString(), MgrDetlDT));
               
            }
            catch (Exception ex)
            {

                TempData["ErrStatus"] = true;
            }

            return View(clientModel);  
        }

        [HttpPost]
        public ActionResult ManageClient(ClientModel model,string vManagerName,string vMgrAddress,string vMgrPhno,string vMgrEmail)
        {

            ClientModel cm = new ClientModel();

            try
            {
                //start 
                string[] ArrManagerName = vManagerName.Split('|');
                string[] ArrManagerAddress = vMgrAddress.Split('|');
                string[] ArrManagerPhno = vMgrPhno.Split('|');
                string[] ArrManagerEmail = vMgrEmail.Split('|');

                List<ClientManagerDetail> lstClientMgrDetails = new List<ClientManagerDetail>();
                for (int i = 0; i < ArrManagerName.Length; i++)
                {
                    ClientManagerDetail MgrDetl = new ClientManagerDetail();
                    MgrDetl.ClientMgrName = ArrManagerName[i];
                    MgrDetl.ClientMgrAddress = ArrManagerAddress[i];
                    MgrDetl.ClientMgrPhno = ArrManagerPhno[i];
                    MgrDetl.ClientMgrEmail = ArrManagerEmail[i];

                    lstClientMgrDetails.Add(MgrDetl);
                }
                //end
                if (model.ClientID > 0)
                {
                    model.EditedBy = loggedInUser.ToString();
                    model.EditedDate = DateTime.Now;
                    var insertResult =  cm.UpdateClientdata(model, lstClientMgrDetails);
                    if (insertResult)
                    {
                        TempData["ErrStatus"] = false;
                    }
                    else
                    {
                        TempData["ErrStatus"] = true;
                    }
                }
                else
                {
                    model.CreateDate = DateTime.Now;
                    model.CreatedBy = loggedInUser.ToString();


                    var insertResult = cm.InsertClientdata(model, out int clientID, lstClientMgrDetails);
                    if (insertResult && clientID > 0)
                    {
                        TempData["ErrStatus"] = false;
                    }
                    else
                    {
                        TempData["ErrStatus"] = true;
                    }
                }
            }
            catch (Exception)
            {
                TempData["ErrStatus"] = true;
            }
            return Json(TempData["ErrStatus"]);
        }
      
        public ActionResult LoadClientData()
        {
            ClientModel obj = new ClientModel();
            StringBuilder strClienData = new StringBuilder();
            
            try
            {
                var loggedInUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
                UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);
                DataTable dt = new DataTable();

                if (userInfo.IsRoleSysAdmin)
                {
                    dt = obj.GetAllClients();
                }
                else
                {
                    dt = obj.GetAllClients(userInfo.UserOrganisationID);
                }


                foreach (DataRow item in dt.Rows)
                {
                    string status = Convert.ToBoolean(item["IsActive"]) == true ? "Active" : "In Active";
                    strClienData.Append("<tr>");
                    strClienData.Append("<td class='text-center'>" + item["ClientID"].ToString() + "</td>");
                    strClienData.Append("<td class='text-center'>" + item["ClientCode"].ToString() + "</td>");
                    strClienData.Append(" <td class='text-center'>" + item["ClientName"].ToString() + "</td>");
                    strClienData.Append(" <td class='text-center'>" + item["ClientAddress"].ToString() + "</td>");
                    strClienData.Append(" <td class='text-center'>" + item["ClientContactNo"].ToString() + "</td>");
                    strClienData.Append(" <td class='text-center'>" + item["ClientLocation"].ToString() + "</td>");
                    strClienData.Append("<td class='text-center'>" + item["orgname"].ToString() + "</td>");
                    strClienData.Append("<td class='text-center'>" + status + "</td>");
                   
                    strClienData.AppendFormat(@"<td class='text-center'><a href ='javascript:void(0);' onclick=""ShowManagerDetlPopup({0});""> view </a> </td>",Convert.ToInt32( item["ClientID"]));
                    strClienData.Append("<td class='text-center'><a href = 'ManageClient?ID=" + item["ClientID"].ToString() + "'>Edit </a> </td>");
                    strClienData.Append("</tr>");

                }
            }
            catch (Exception ex)
            {
                TempData["ErrStatus"] = true;
                ////throw;
            }
            return Content(strClienData.ToString());
        }
        #endregion

        #region Task Status managment region
        public ActionResult ManageTaskStatus(int ID = 0)
        {
            TaskStatusModel statusModel = new TaskStatusModel();
            try
            {

                UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);
                statusModel.OrganisationList.Clear();

                if (!userInfo.IsRoleSysAdmin)
                {
                    //var organisation = statusModel.GetOrgInList(userInfo.UserOrganisationID).FirstOrDefault(x => x.isActive == true);
                    var organisation = statusModel.GetOrgInList(userInfo.UserOrganisationID);
                    statusModel.OrganisationList = organisation;

                    statusModel.StatusOrgId = userInfo.UserOrganisationID;
                }
                else
                {
                    var list = statusModel.GetOrgInList();
                    if (list.Count > 0)
                        statusModel.OrganisationList = list;

                }
                if (ID > 0)
                {
                    DataTable dt = new DataTable();
                    dt = statusModel.GetTaskStatus(0, ID);
                    if (dt.Rows.Count > 0)
                    {
                        statusModel.StatusId = ID;
                        statusModel.StatusName = dt.Rows[0]["StatusName"].ToString();
                        statusModel.StatusCode = dt.Rows[0]["StatusCode"].ToString();
                        statusModel.Rank = Convert.ToInt32(dt.Rows[0]["Rank"]);
                        statusModel.StatusOrgId = Convert.ToInt32(dt.Rows[0]["ORGID"]);
                        statusModel.IsActive = Convert.ToBoolean(dt.Rows[0]["isACTIVE"]);
                    }
                }
            }
            catch (Exception ex)
            {

                TempData["ErrStatus"] = true;
            }

            return View(statusModel);
        }

        [HttpPost]
        public ActionResult ManageTaskStatus(TaskStatusModel model)
        {
            TaskStatusModel tm = new TaskStatusModel();
            try
            {
                if (model.StatusId > 0)
                {
                    model.EditedBy = loggedInUser;
                    model.EditedTS = DateTime.Now;
                    var result = tm.UpdateTaskStatusData(model);
                    if (result == true)
                    {
                        TempData["ErrStatus"] = false;
                    }
                    else
                    {
                        TempData["ErrStatus"] = true;
                    }
                }
                else
                {
                    model.AddedBy = loggedInUser;
                    model.AddedTS = DateTime.Now;
                    var result = tm.InsertTaskStatusData(model, out int id);
                    if (result == true && id > 0)
                    {
                        TempData["ErrStatus"] = false;
                    }
                    else
                    {
                        TempData["ErrStatus"] = true;
                    }
                }
            }
            catch (Exception)
            {

                TempData["ErrStatus"] = true;
            }

            return RedirectToAction("ManageTaskStatus", "Admin");
        }

        public ActionResult LoadTaskStatusData()
        {
            TaskStatusModel taskStatusModel = new TaskStatusModel();
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                var loggedInUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["sessUser"]);
                UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);
                DataTable dt = new DataTable();

                if (userInfo.IsRoleSysAdmin)
                {
                    dt = taskStatusModel.GetTaskStatus(0, 0);
                }
                else
                {
                    dt = taskStatusModel.GetTaskStatus(userInfo.UserOrganisationID);
                }
                foreach (DataRow item in dt.Rows)
                {
                    string status = Convert.ToBoolean(item["isACTIVE"]) == true ? "Active" : "In Active";
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td class='text-center'>" + item["StatusID"].ToString() + "</td>");
                    stringBuilder.Append("<td class='text-center'>" + item["StatusCode"].ToString() + "</td>");
                    stringBuilder.Append(" <td class='text-center'>" + item["StatusName"].ToString() + "</td>");
                    stringBuilder.Append(" <td class='text-center'>" + item["Rank"].ToString() + "</td>");
                    stringBuilder.Append("<td class='text-center'>" + item["OrgName"].ToString() + "</td>");
                    stringBuilder.Append("<td class='text-center'>" + status + "</td>");
                    stringBuilder.Append("<td class='text-center'><a href = 'ManageTaskStatus?ID=" + item["StatusID"].ToString() + "'>Edit </a> </td>");
                    stringBuilder.Append("</tr>");

                }

            }
            catch (Exception)
            {

                throw;
            }
            return Content(stringBuilder.ToString());
        }
        #endregion

        #region Master skill
        public ActionResult ManageMasterSkill(int ID = 0)
        {
            MasterSkill ms = new MasterSkill();
            UserModel obj = new UserModel();
            UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);

            ViewBag.IsUserSysAdmin = userInfo.IsRoleSysAdmin;
            DataTable dtUsers = new DataTable();
            if (userInfo.IsRoleSysAdmin)
            {
                ms.OrganisationList.Clear();
                ms.OrganisationList = obj.GetAllOrgInList().Where(x => x.isActive == true).ToList();
            }

            else
            {
                obj.OrganisationList.Clear();
                var organisation = obj.GetAllOrgInList().FirstOrDefault(x => x.id == userInfo.UserOrganisationID && x.isActive == true);
                obj.OrganisationList.Add(organisation);
                ms.OrganisationList = obj.OrganisationList;
                ms.UserOrgId = userInfo.UserOrganisationID;

                obj.UsersList.Clear();
                obj.UsersList = obj.GetAllUsersInList(userInfo.UserOrganisationID).Where(x => x.IsActive == true).ToList();
                obj.DepartmentList.Clear();
                ms.DepartmentList = obj.GetAllDepartmentInList(userInfo.UserOrganisationID).Where(x => x.IsActive == true).ToList();
            }


            // MasterSkill ms = new MasterSkill();
            //RoleModel obj = new RoleModel();
            List<ProjectModel> objRole = new List<ProjectModel>();
            //ms.OrganisationList = obj.GetAllOrgInList();

            UserModel dptObj = new UserModel();
            // ms.DepartmentList = dptObj.GetAllDepartmentInList();
            try
            {
                if (ID > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ms.GetMasterSkillByID(ID);
                    ms.Id = int.Parse(dt.Rows[0]["Id"].ToString());
                    ms.CategoryID = dt.Rows[0]["CategoryID"].ToString();
                    ms.SkillName = dt.Rows[0]["SkillName"].ToString();
                    ms.SkillCode = dt.Rows[0]["SkillCode"].ToString();
                    ms.Rank = dt.Rows[0]["Rank"].ToString();
                    ms.Description = dt.Rows[0]["Description"].ToString();
                    ms.isMandatory = Convert.ToBoolean(dt.Rows[0]["isMandatory"].ToString());
                    ms.OrgID = int.Parse(dt.Rows[0]["OrgID"].ToString());
                    ms.DepartmentList = obj.GetAllDepartmentInList(ms.OrgID).Where(x => x.IsActive == true).ToList();
                    ms.IsActive = Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString());

                }
                else
                {
                    ms.Id = 0;
                    ms.CategoryID = "";
                    ms.SkillName = "";
                    ms.SkillCode = "";
                    ms.Description = "";
                    ms.Rank = "";
                    ms.isMandatory = false;
                    ms.OrgID = 0;
                    ms.IsActive = false;

                }
            }
            catch (Exception exx)
            {

            }

            return View(ms);
        }

        public ActionResult GetDepartmentList(int OrgID)
        {
            UserModel obj = new UserModel();
            List<DepartmentModel> viewModelList = obj.GetAllDepartmentInList(OrgID).Where(x => x.IsActive == true).ToList();
            return Json(viewModelList);

        }

        public ActionResult MasterSkills()
        {
            UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);
            ViewBag.IsUserSysAdmin = userInfo.IsRoleSysAdmin;

            MasterSkill MS = new MasterSkill();
            string strMasterSkill = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                int i = 0;
                dt = MS.GetAllMasterSkill(userInfo.IsRoleSysAdmin, userInfo.UserOrganisationID);

                foreach (DataRow dr in dt.Rows)
                {
                    strMasterSkill += "<tr><td class='text-center'>" + dr["id"].ToString() + "</td><td class='text-center'>" + dr["SkillCode"] + "</td><td class='text-center'>" + dr["SkillName"] + "</td>" + "<td class='text-center'>" + dr["Department"].ToString() + "</td>" +
                        "<td class='text-center'>" + dr["orgname"].ToString() + "</td>" + "<td class='text-center'>" + dr["isMandatory"].ToString() + "</td>" + "<td class='text-center'>" + dr["isActive"].ToString() + "</td>" +
                       "<td  class='text-center'><a href = 'ManageMasterSkill?ID=" + dr["ID"].ToString() + "'>Edit</a></td></tr>";
                    i++;
                }
            }
            catch (Exception exc)
            {
                //throw exc;
            }
            return Content(strMasterSkill);

        }

        [HttpPost]
        public ActionResult ManageMasterSkill(MasterSkill MSmodel)
        {
            MasterSkill obj = new MasterSkill();
            if (MSmodel.Id > 0)
            {
                try
                {
                    obj = MSmodel;
                    obj.EditedBy = Convert.ToInt32(Session["sessUser"]);
                    obj.EditedDate = Convert.ToDateTime(DateTime.Now.ToString());
                    obj.IsActive = Convert.ToBoolean(MSmodel.IsActive);
                    obj.isMandatory = Convert.ToBoolean(MSmodel.isMandatory);
                    obj.Update_MasterSkill(obj);

                    //TempData["ErrStatus"] = obj.ISErr.ToString();
                }
                catch
                {

                }
            }
            else
            {

                MSmodel.CreatedBy = int.Parse(Session["sessUser"].ToString());
                MSmodel.CreateDate = DateTime.Now;

                MSmodel.IsActive = Convert.ToBoolean(MSmodel.IsActive);
                int outID = 0;
                MSmodel.InsertSkillMasterdata(MSmodel, out outID);
                //obj.InsertModuledata(model, out int id);
                //if (id > 0)
                //{

                //}
                //TempData["ErrStatus"] = MSmodel.ISErr.ToString();
            }

            return RedirectToAction("ManageMasterSkill", "Admin");
        }
        #endregion master skill

        #region master skill rating
        public ActionResult ManageMasterSkillRating(int ID = 0)
        {
            MasterSkillRating msr = new MasterSkillRating();
            UserModel obj = new UserModel();
            UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);

            ViewBag.IsUserSysAdmin = userInfo.IsRoleSysAdmin;
            DataTable dtUsers = new DataTable();
            if (userInfo.IsRoleSysAdmin)
            {
                msr.OrganisationList.Clear();
                msr.OrganisationList = obj.GetAllOrgInList().Where(x => x.isActive == true).ToList();
            }

            else
            {
                obj.OrganisationList.Clear();
                var organisation = obj.GetAllOrgInList().FirstOrDefault(x => x.id == userInfo.UserOrganisationID && x.isActive == true);
                obj.OrganisationList.Add(organisation);
                msr.OrganisationList = obj.OrganisationList;
                msr.UserOrgId = userInfo.UserOrganisationID;
                obj.UsersList.Clear();
                obj.UsersList = obj.GetAllUsersInList(userInfo.UserOrganisationID).Where(x => x.IsActive == true).ToList();
                obj.DepartmentList.Clear();
            }


            // MasterSkill ms = new MasterSkill();
            //RoleModel obj = new RoleModel();
            List<ProjectModel> objRole = new List<ProjectModel>();
            //ms.OrganisationList = obj.GetAllOrgInList();

            UserModel dptObj = new UserModel();
            // ms.DepartmentList = dptObj.GetAllDepartmentInList();
            try
            {
                if (ID > 0)
                {
                    DataTable dt = new DataTable();
                    dt = msr.GetMasterSkillRatingByID(ID);
                    msr.ID = int.Parse(dt.Rows[0]["Id"].ToString());
                    msr.SkillCode = dt.Rows[0]["SkillCode"].ToString();
                    msr.SkillLevel = dt.Rows[0]["SkillLevel"].ToString();
                    msr.SkillScore = int.Parse(dt.Rows[0]["SkillScore"].ToString());
                    msr.Description = dt.Rows[0]["Description"].ToString();
                    msr.OrgID = int.Parse(dt.Rows[0]["OrgID"].ToString());
                    msr.isActive = Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString());

                }
                else
                {
                    msr.ID = 0;
                    msr.SkillCode = "";
                    msr.SkillLevel = "";
                    msr.SkillCode = "";
                    msr.Description = "";
                    msr.SkillScore = 0;
                    msr.OrgID = 0;
                    msr.isActive = false;

                }
            }
            catch (Exception exx)
            {

            }

            return View(msr);
        }

        public ActionResult MasterSkillRatings()
        {
            UserInfoHelper userInfo = new UserInfoHelper(loggedInUser);
            ViewBag.IsUserSysAdmin = userInfo.IsRoleSysAdmin;

            MasterSkillRating MSR = new MasterSkillRating();
            string strMasterSkill = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                int i = 0;
                dt = MSR.GetAllMasterSkillRating(userInfo.IsRoleSysAdmin, userInfo.UserOrganisationID);

                foreach (DataRow dr in dt.Rows)
                {
                    strMasterSkill += "<tr><td class='text-center'>" + dr["id"].ToString() + "</td><td class='text-center'>" + dr["SkillCode"] + "</td><td class='text-center'>" + dr["SkillLevel"] + "</td>" + "<td class='text-center'>" + dr["Description"].ToString() + "</td>" +
                        "<td class='text-center'>" + dr["SkillScore"].ToString() + "</td>" + "<td class='text-center'>" + dr["OrgName"].ToString() + "</td>" + "<td class='text-center'>" + dr["isActive"].ToString() + "</td>" +
                       "<td  class='text-center'><a href = 'ManageMasterSkillRating?ID=" + dr["ID"].ToString() + "'>Edit</a></td></tr>";
                    i++;
                }
            }
            catch (Exception exc)
            {
                //throw exc;
            }
            return Content(strMasterSkill);

        }

        [HttpPost]
        public ActionResult ManageMasterSkillRating(MasterSkillRating MSRmodel)
        {
            MasterSkillRating obj = new MasterSkillRating();
            if (MSRmodel.ID > 0)
            {
                try
                {
                    obj = MSRmodel;
                    obj.EditedBy = Convert.ToInt32(Session["sessUser"]);
                    obj.EditedDate = Convert.ToDateTime(DateTime.Now.ToString());
                    obj.isActive = Convert.ToBoolean(MSRmodel.isActive);
                    //obj.upda(obj);
                    MSRmodel.Update_MasterSkillRating(obj);
                    //TempData["ErrStatus"] = obj.ISErr.ToString();
                }
                catch
                {

                }
            }
            else
            {

                MSRmodel.CreatedBy = int.Parse(Session["sessUser"].ToString());
                MSRmodel.CreatedDate = DateTime.Now;

                MSRmodel.isActive = Convert.ToBoolean(MSRmodel.isActive);
                int outID = 0;
                MSRmodel.InsertMasterSkillRatingdata(MSRmodel, out outID);
                //obj.InsertModuledata(model, out int id);
                //if (id > 0)
                //{

                //}
                //TempData["ErrStatus"] = MSmodel.ISErr.ToString();
            }

            return RedirectToAction("ManageMasterSkillRating", "Admin");
        }
        #endregion


        public ActionResult GetUserActivityTracking(string userAgent, string absURL)
        {
            string strRet = string.Empty;
            try
            {
                UserActivityLog aLogger = new UserActivityLog()
                {
                    LoggerId = Guid.NewGuid(),
                    LogedUserId = Convert.ToString(((DataSet)Session["sessUserAllData"]).Tables[0].Rows[0]["Id"]),
                    IPAddress = Request.UserHostAddress,
                    UrlAccessed = absURL,
                    UserAgent = userAgent,
                    IsMobileDevice = HttpContext.Request.Browser.IsMobileDevice ? "Mobile" : "Desktop",
                    Browser = HttpContext.Request.Browser.Browser,
                    MACAddress = GetMACID(),
                    Platform = Request.Browser.Platform,
                    AccessDateTime = DateTime.Now
                };
                aLogger.SetUserActivityLog(aLogger);
                //var botParser = new BotParser();
                //botParser.SetUserAgent(userAgent);
            }
            catch (Exception exx)
            { }
            return Json(strRet);
        }
        private string GetMACID()
        {
            string strMAC = string.Empty;
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                for (int j = 0; j <= 1; j++)
                {
                    PhysicalAddress address = nics[j].GetPhysicalAddress();
                    byte[] bytes = address.GetAddressBytes();
                    string M = nics[j].Name + ":";
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        M = M + bytes[i].ToString("X2");
                        if (i != bytes.Length - 1)
                        {
                            M = M + ("-");
                        }
                    }
                    strMAC = M;
                }
            }
            catch (Exception exx)
            { strMAC = string.Empty; }
            return strMAC;
        }


        private string BindClintManagerDetail( string ManagerID,DataTable dt)
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                sbContent.Append("<div class='row'>");
                sbContent.Append("<div class='col-md-12'>");
                sbContent.Append("<div class='panel-body dvBorder form-group'>");
                sbContent.Append("<div class='panel-body'>");
                sbContent.Append("<div class='table-responsive'>");
                sbContent.Append("<table class='table table-bordered' id='tblManagerDetail'  width='100%'>");
                sbContent.Append("<thead>");
                sbContent.Append("<tr>");
              
                sbContent.Append("<th class='text-center tblHeaderColor' width='30%'>Manager Name</th>");
                sbContent.Append("<th class='text-center tblHeaderColor' width='30%'>Address</th>");
                sbContent.Append("<th class='text-center tblHeaderColor' width='20%'>Phone No.</th>");
                sbContent.Append("<th class='text-center tblHeaderColor' width='20%'>Email</th>");
                sbContent.Append("<th class='text-center tblHeaderColor'><div class='pull-right'><button type='button' id='btnMgrAddNew' class='btn btn-xs btn-primary classAdd'><span class='glyphicon glyphicon-plus'></span></button></div></th>");

                sbContent.Append("</tr>");
                sbContent.Append("</thead>");
                #region Bind table body
                if (ManagerID != "0" && dt != null && dt.Rows.Count > 0)
                {
                    int counter = 1;
                    sbContent.Append("<tbody id='tbodyMgrDetail'>");
                    foreach (DataRow dr in dt.Rows)
                    {
                        sbContent.Append("<tr class='trMgrDetail'>");
                        sbContent.Append("<td><input class='form-control' name='txtManagerName[]' value='" + Convert.ToString(dr["Name"]) + "' id='txtManagerName"+ counter + "'> </input></td>");
                        sbContent.Append("<td><input class='form-control' name='txtMgrAddress[]' value='" + Convert.ToString(dr["Address"]) + "' id='txtMgrAddress" + counter + "'> </input></td>");
                        sbContent.Append("<td><input class='form-control'  name='txtMgrPhno[]' value='" + Convert.ToString(dr["phone"]) + "' id='txtMgrPhno" + counter + "'> </input></td>");
                        sbContent.Append("<td><input class='form-control' name='txtMgrEmail[]' value='" + Convert.ToString(dr["email"]) + "' id='txtMgrEmail" + counter + "'> </input></td>");
                       
                        sbContent.Append("<td><button type='button' id='btnMgrDelete' name='btnMgrDelete' class='btnMgrDelete btn btn btn-danger btn-xs'><span class='glyphicon glyphicon-remove'></span></button></td>");
                        sbContent.Append("</tr>");
                        counter++;
                    }
                    sbContent.Append("</tbody>");
                }
                else 
                {
                    sbContent.Append("<tbody id='tbodyMgrDetail'>");
                    sbContent.Append("<tr class='trMgrDetail'>");
                    sbContent.Append("<td><input class='form-control' type='txtManagerName[]' name='txtManagerName[]' id='txtManagerName1' ></input></td>");
                    sbContent.Append("<td><input class='form-control' type='text' name='txtMgrAddress[]' id='txtMgrAddress1' /></td>");
                    sbContent.Append("<td><input class='form-control' type='number'   name='txtMgrPhno[]' id='txtMgrPhno1' /></td>");
                    sbContent.Append("<td><input class='form-control' type='text' name='txtMgrEmail[]' id='txtMgrEmail1' /></td>");
                    sbContent.Append("<td><button type='button' id='btnMgrDelete' name='btnMgrDelete' class='btnMgrDelete btn btn btn-danger btn-xs'><span class='glyphicon glyphicon-remove'></span></button></td>");
                    sbContent.Append("</tr>");
                    sbContent.Append("</tbody>");
                }
               
                #endregion
                sbContent.Append("</table>");
                sbContent.Append("</div>");
                sbContent.Append("</div>");
                sbContent.Append("</div>");
                sbContent.Append("</div>");
                sbContent.Append("</div>");
            }
            catch (Exception exE)
            {
                try
                {
                    using (ErrorHandle errH = new ErrorHandle())
                    { errH.WriteErrorLog(exE); }
                }
                catch (Exception exC) { }
            }
            return sbContent.ToString();
        }
     /*   private List<string> GetClintManagerDetail(string ManagerID, DataTable dt)
        {
            StringBuilder sbContent = new StringBuilder();
            List<string> ManagerList = new List<string>();
            ManagerList.Clear();
            try
            {
    
                if (ManagerID != "0" && dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                      

                        ManagerList.Add("<tr class='trMgrDetail1'>");

                        ManagerList.Add("<tr>");

                        ManagerList.Add("<td class='text-center'>" + Convert.ToString(dr["Name"]) + "</td>");
                        ManagerList.Add("<td class='text-center'>" + Convert.ToString(dr["Address"]) + "</td>");
                        ManagerList.Add("<td class='text-center'>" + Convert.ToString(dr["phone"]) + "</td>");
                        ManagerList.Add("<td class='text-center'>" + Convert.ToString(dr["email"]) + "</td>");
                        ManagerList.Add("</tr>");
                      

                    }
                }
                else
                {
                    ManagerList.Add("<tr>");
                    ManagerList.Add("<td></td>");
                    ManagerList.Add("<td></td>");
                    ManagerList.Add("<td></td>");
                    ManagerList.Add("<td></td>");
                    ManagerList.Add("</tr>");
                   
                }

            }
            catch (Exception exE)
            {
                try
                {
                    using (ErrorHandle errH = new ErrorHandle())
                    { errH.WriteErrorLog(exE); }
                }
                catch (Exception exC) { }
            }
            
            return ManagerList;
        }*/
        private string GetClintManagerDetail(DataTable dt)
        {
            StringBuilder sbContent = new StringBuilder();
            List<string> ManagerList = new List<string>();
            try
            {

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {

                        sbContent.Append("<tr>");
                        sbContent.Append("<td class='text-center'>" + Convert.ToString(dr["Name"]) + "</td>");
                        sbContent.Append("<td class='text-center'>" + Convert.ToString(dr["Address"]) + "</td>");
                        sbContent.Append("<td class='text-center'>" + Convert.ToString(dr["phone"]) + "</td>");
                        sbContent.Append("<td class='text-center'>" + Convert.ToString(dr["email"]) + "</td>");
                        sbContent.Append("</tr>");
                    }
                }
                else
                {
                    sbContent.Append("<tr>");
                    sbContent.Append("<td></td>");
                    sbContent.Append("<td></td>");
                    sbContent.Append("<td></td>");
                    sbContent.Append("<td></td>");
                    sbContent.Append("</tr>");
                }

            }
            catch (Exception exE)
            {
                try
                {
                    using (ErrorHandle errH = new ErrorHandle())
                    { errH.WriteErrorLog(exE); }
                }
                catch (Exception exC) { }
            }
            return sbContent.ToString();
        }
        public ActionResult GetManagerDetailsById(string ID)
        {
            ClientModel clientModel = new ClientModel();
            List<string> MgrList = new List<string>();
            StringBuilder sbOut = new StringBuilder();
           int id = Convert.ToInt32(ID);
            try
            {
                DataTable MgrDetlDT = new DataTable();
                MgrDetlDT = clientModel.GetClientManagerByID(id);
                //MgrList=(GetClintManagerDetail(ID, MgrDetlDT));
                sbOut.Append(GetClintManagerDetail(MgrDetlDT));
            }
            catch (Exception ex)
            {

                TempData["ErrStatus"] = true;
            }
            return Json(sbOut.ToString());
            //return Json(JsonConvert.SerializeObject(MgrList));
        }
        
        public JsonResult IssueAttachments()
        {
            string fName = "";
            string Directory = "";
            string FolderName = Request.Form["DirectoryName"].ToString();
            if (FolderName != "")
            {
                Directory = FolderName.Replace("\"", string.Empty).Trim();
            }
            else
            {
                Directory = DateTime.Now.Ticks.ToString();
            }

            foreach (string imageFile in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[imageFile];
                fName = file.FileName;
                int id = 0;
                int pressID = 0;
                if (file != null && file.ContentLength > 0)
                {


                    var originalDirectory = new DirectoryInfo(string.Format("{0}IssueAttachments", Server.MapPath(@"\")));
                    string pathString = System.IO.Path.Combine(originalDirectory.ToString(), Directory);
                    var fileName = file.FileName;
                    bool isExists = System.IO.Directory.Exists(pathString);
                    if (!isExists)
                        System.IO.Directory.CreateDirectory(pathString);
                    var path = string.Format("{0}\\{1}", pathString, fileName);


                    file.SaveAs(path);
                 
                }
            }
            return Json(Directory);
        }
        [HttpGet]
        public virtual ActionResult DownloadExcelTemplateForIssue(string fileid)
        {
            if (TempData[fileid] != null)
            {
                byte[] data = TempData[fileid] as byte[];
                return File(data, "application/vnd.ms-excel", "ExcelForIssue.xlsx");
            }
            else
            {
                return new EmptyResult();
            }
        }
        [HttpGet]
        public virtual ActionResult DownloadExcelTemplateForTask(string fileid)
        {
            if (TempData[fileid] != null)
            {
                byte[] data = TempData[fileid] as byte[];
                return File(data, "application/vnd.ms-excel", "ExcelForProjectTask.xlsx");
            }
            else
            {
                return new EmptyResult();
            }
        }

        public ActionResult GenerateExcelForIssue()
        {
            // Generate a new unique identifier against which the file can be stored
            string handle = DateTime.Now.Ticks.ToString();
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet ws = package.Workbook.Worksheets.Add("Sheet1");
                    ws.Cells["A1"].Value = "Project Name";
                    ws.Cells["B1"].Value = "Ticket Code";
                    ws.Cells["C1"].Value = "Ticket Type";
                    ws.Cells["D1"].Value = "Ticket Name";
                    ws.Cells["E1"].Value = "Ticket Description";
                    ws.Cells["F1"].Value = "start Date";
                    ws.Cells["G1"].Value = "End Date";
                    ws.Cells["H1"].Value = "Expected Time";
                    ws.Cells["I1"].Value = "Status";
                    ws.Cells["J1"].Value = "Severity";
                    ws.Cells["K1"].Value = "Percentage Complete";
                    ws.Cells["L1"].Value = "Assigned to";
                    ws.Cells["M1"].Value = "Actual Start Date";
                    ws.Cells["N1"].Value = "Actual End Date";
                    ws.Cells["O1"].Value = "IsActive";
                    ws.Cells["P1"].Value = "IsValueAdded";
                    ws.Cells["Q1"].Value = "URL";

                    ws.Cells["A1:Q1"].Style.Font.Bold = true;
                    ws.Cells["A1:Q1"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    ws.Cells["A1:Q1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells["A1:Q1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Navy);
                    ws.Cells["A1:Q1"].Style.Locked = true;

                    // Format  cells as TEXT in a spreadsheet

                    ws.Cells["A:Q"].Style.Numberformat.Format = "@";
                  


                    ws.Cells[ws.Dimension.Address].AutoFitColumns();
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        package.SaveAs(memoryStream);
                        memoryStream.Position = 0;
                        TempData[handle] = memoryStream.ToArray();
                    }
                }
            }
            catch (Exception exE)
            {
                try
                {
                    using (ErrorHandle errH = new ErrorHandle())
                    { errH.WriteErrorLog(exE); }
                }
                catch (Exception exC) { }
            }
            return Json(handle);

        }
        public ActionResult UploadExcelForCreateNewTicket(FormCollection formCollection)
        {
            string result = "";
            string errMsg = string.Empty;
            if (Request != null)
            {
                try
                {
                    StringBuilder sbContent = new StringBuilder();

                    HttpPostedFileBase file = Request.Files[0];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;

                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        DataSet ds = ConvertExcelToDataSet(file.InputStream);

                        #region create ticket
                        DataTable dtHD = ds.Tables[0];
                        if (dtHD != null && dtHD.Rows.Count > 0)
                        {
                            UserInfoHelper UIH = new UserInfoHelper(int.Parse(HttpContext.Session["sessUser"].ToString()));
                            UserModel userModel = new UserModel();
                            DataTable dtUSER = userModel.GetUsersByID(UIH.UserId);

                            userModel.EmailId = dtUSER.Rows[0]["EmailId"].ToString();
                            userModel.UserName = dtUSER.Rows[0]["Name"].ToString();
                            int uploadSuccess = 0;
                            foreach (DataRow dr in dtHD.Rows)
                            {

                                //checking mandatory field
                                if (Convert.ToString(dr["ProjectName"]) != "" && Convert.ToString(dr["TicketCode"]) != "" && Convert.ToString(dr["TicketName"]) != "" && Convert.ToString(dr["TicketType"]) != "" &&
                                    Convert.ToString(dr["startDate"]) != "" && Convert.ToString(dr["EndDate"]) != "" && Convert.ToString(dr["Severity"]) != "" && Convert.ToString(dr["Status"]) != ""
                                    && Convert.ToString(dr["Assignedto"]) != "" && Convert.ToString(dr["PercentagComplete"]) != "")
                                {
                                    ProjectIssueModel model = new ProjectIssueModel();
                                    model.IssueCode = Convert.ToString(dr["TicketCode"]);
                                    model.IssueName = Convert.ToString(dr["TicketName"]);
                                    model.IssueDescription = Convert.ToString(dr["TicketDescription"]);

                                    model.IssuestartDate = DateTimeHelper.ConvertStringToValidDate(dr["startDate"].ToString());
                                    model.IssueEndDate = DateTimeHelper.ConvertStringToValidDate(dr["EndDate"].ToString());
                                    var startdt = model.IssuestartDate;
                                    var enddt = model.IssueEndDate;
                                    if (startdt > enddt)
                                    {
                                        int index = dtHD.Rows.IndexOf(dr);
                                        int excelROW = index + 2;
                                        sbContent.Append("<div class='row'><b> Row No:" + excelROW + "<b>&nbsp; Start date should not be greater than end date please check your excel sheet </div></br>");
                                        goto outer;
                                    }

                                    //  string[] Arr = new string[2];
                                    // Arr = (dr["ExpectedTime"].ToString()).Split(':');
                                    // string ExpectedTime = Arr[0] + '.' + Arr[1];
                                    if (dr["ExpectedTime"].ToString() != "")
                                    {
                                        model.ExpectedDuration = Convert.ToDouble(dr["ExpectedTime"]);
                                    }
                                    else
                                    {
                                        model.ExpectedDuration = 0.00;
                                    }


                                    model.CompletePercent = Convert.ToInt32(dr["PercentageComplete"]);
                                    if (Convert.ToString(dr["ActualStartDate"]) != "")
                                    {
                                        model.ActualIssueStartDate = DateTimeHelper.ConvertStringToValidDate(dr["ActualStartDate"].ToString());
                                    }
                                    if (Convert.ToString(dr["ActualEndDate"]) != "")
                                    {
                                        model.ActualIssueEndDate = DateTimeHelper.ConvertStringToValidDate(dr["ActualEndDate"].ToString());
                                    }
                                    if (dr["IsActive"].ToString() != "") { model.IsActive = Convert.ToBoolean(dr["IsActive"]); }
                                    else { model.IsActive = true; }

                                    if (dr["IsValueAdded"].ToString() != "") { model.IsValueAdded = Convert.ToBoolean(dr["IsValueAdded"]); }
                                    else { model.IsValueAdded = true; }

                                    model.AddedBy = loggedInUser;
                                    model.AddedTS = DateTime.Now;
                                    model.url = Convert.ToString(dr["URL"]);
                                    model.DirectoryName = "";

                                    //Get ProjectId
                                    DataTable dtProjectId = model.GetProjectIDByProjectName(Convert.ToString(dr["ProjectName"]));
                                    if (dtProjectId.Rows.Count > 0)
                                    {
                                        model.ProjectID = Convert.ToInt32(dtProjectId.Rows[0]["ID"]);
                                        model.ProjectName = Convert.ToString(dr["ProjectName"]);
                                    }
                                    else
                                    {
                                        //For Project Name not correct
                                        int index = dtHD.Rows.IndexOf(dr);
                                        int excelROW = index + 2;
                                        sbContent.Append("<div class='row'><b> Row No:" + excelROW + "<b>&nbsp; project name does not correct check your excel sheet</div><br/>");

                                        goto outer;
                                    }

                                    //Get UserID
                                    String username = Convert.ToString(dr["Assignedto"]);

                                    string[] userNameArr = username.Split(';');
                                    for (int j = 0; j < userNameArr.Length; j++)
                                    {
                                        DataTable dtUserId = model.GetUserIDByUserName(userNameArr[j], model.ProjectID);
                                        if (dtUserId.Rows.Count > 0)
                                        {
                                            model.UserIdAssigned = model.UserIdAssigned + dtUserId.Rows[0]["Id"].ToString() + ',';
                                        }
                                        else
                                        {
                                            //For UserID not correct
                                            int index = dtHD.Rows.IndexOf(dr);
                                            int excelROW = index + 2;
                                            sbContent.Append("<div class='row'><b> Row No:" + excelROW + "<b>&nbsp; User name does not correct please check your excel sheet and put ';' between two username</div><br/>");

                                            goto outer;
                                        }
                                    }

                                    //Get StatusID,Servity,TicktType 
                                    DataSet dataset = model.Get_Status_Servity_TicketName(Convert.ToString(dr["Severity"]), Convert.ToString(dr["Status"]), Convert.ToString(dr["TicketType"]), UIH.UserId);
                                    if (dataset != null && dataset.Tables[0] != null && dataset.Tables[1] != null && dataset.Tables[2] != null)
                                    {

                                        model.SeverityID = Convert.ToInt32(dataset.Tables[0].Rows[0]["SeverityID"]);
                                        model.StatusID = Convert.ToInt32(dataset.Tables[1].Rows[0]["StatusID"]);
                                        model.TicketTypeID = Convert.ToInt32(dataset.Tables[2].Rows[0]["ID"]);
                                    }
                                    else
                                    {
                                        int index = dtHD.Rows.IndexOf(dr);
                                        int excelROW = index + 2;
                                        sbContent.Append("<div class='row'><b> Row No:" + excelROW + "<b>&nbsp; status name/Servity/Ticket type does not correct please check your excel sheet </div></br>");
                                        goto outer;
                                    }
                                    //save ticket
                                    var insertStatus = model.InsertIssuedata(model, out int id);// out string strMailToName,out string strMailTo);
                                    if (insertStatus)
                                    {
                                        if (id > 0)
                                        {
                                            uploadSuccess++;
                                            //Save Comment details ...satrt
                                            ProjectIssueModel projectIssueModel = new ProjectIssueModel();
                                            projectIssueModel.IssueIdforstatus = id;
                                            projectIssueModel.Comment = "";
                                            projectIssueModel.StatusID = model.StatusID;
                                            projectIssueModel.url = model.url;
                                            projectIssueModel.DirectoryName = model.DirectoryName;
                                            projectIssueModel.Duration = 0;
                                            projectIssueModel.ActualIssueStartDate = model.ActualIssueStartDate;
                                            projectIssueModel.ActualIssueEndDate = model.ActualIssueEndDate;
                                            projectIssueModel.EditedBy = loggedInUser;
                                            projectIssueModel.EditedTS = DateTime.Now;
                                            var updateStatus = projectIssueModel.UpdateIssuestatus(projectIssueModel, out string strMailToName, out string strMailTo);

                                            if (updateStatus)
                                            {
                                                string[] usernameArr = strMailToName.Split(';');
                                                string[] userEmailArr = strMailTo.Split(';');
                                                for (int j = 0; j < usernameArr.Length; j++)
                                                {
                                                    sendMail_afterSaveTicket(usernameArr[j], userEmailArr[j], "Ticket has been assigned to you on project " + model.ProjectName + " by " + userModel.UserName);
                                                }

                                                model.ISErr = false;
                                                model.ErrString = "Data Saved Successfully.";
                                                TempData["ErrStatus"] = model.ISErr;
                                                TempData["ErrMsg"] = model.ErrString.ToString();
                                                result = "Success";
                                            }
                                            //save Comment details .. End

                                        }
                                    }  //ticket save ..End

                                    else
                                    {
                                        model.ISErr = true;
                                        model.ErrString = "Error Occured.";
                                        TempData["ErrStatus"] = model.ISErr;
                                        TempData["ErrMsg"] = model.ErrString.ToString();
                                        result = "Error";
                                    }
                                    //****save end


                                }
                                else
                                {
                                    int index = dtHD.Rows.IndexOf(dr);
                                    int excelROW = index + 2;
                                    sbContent.Append("<div class='row'><b> Row No:" + excelROW + "<b>&nbsp;Please put all mandatory value  in your excel sheet </div></br>");

                                    goto outer;
                                }
                            //***Next row
                            outer:
                                continue;
                            }
                            int totalEXCELRecord = dtHD.Rows.Count;
                            int failure = totalEXCELRecord - uploadSuccess;
                            sbContent.Append("<p><b>" + uploadSuccess + " rows Successfully saved out of " + totalEXCELRecord + " record in your excel sheet<b></p>");
                            sbContent.Append("<p><b>" + failure + " rows can not save out of " + totalEXCELRecord + " record in your excel sheet<b></p>");
                            sendMail_InBulkTicketUploadingTime(userModel.UserName, userModel.EmailId, sbContent.ToString());
                        }
                        #endregion
                    }
                }
                catch (Exception exE)
                {
                    try
                    {
                        using (ErrorHandle errH = new ErrorHandle())
                        { errH.WriteErrorLog(exE); }
                    }
                    catch (Exception exC) { }
                    return null;
                }
            }
            return Json(result);
        }

        public ActionResult GenerateExcelForProjectTask()
        {
            // Generate a new unique identifier against which the file can be stored
            string handle = DateTime.Now.Ticks.ToString();
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet ws = package.Workbook.Worksheets.Add("Sheet1");
                    ws.Cells["A1"].Value = "Project Name";
                    ws.Cells["B1"].Value = "Task Name";
                    ws.Cells["C1"].Value = "Task code";
                    ws.Cells["D1"].Value = "Parent Task";
                    ws.Cells["E1"].Value = "start Date";
                    ws.Cells["F1"].Value = "End Date";
                    ws.Cells["G1"].Value = "IsMilestone";
                    ws.Cells["H1"].Value = "Assigned to";
                    ws.Cells["I1"].Value = "Percentage Complete";
                    ws.Cells["J1"].Value = "Actual Start Date";
                    ws.Cells["K1"].Value = "Actual End Date";
                    ws.Cells["L1"].Value = "Status";
                    ws.Cells["M1"].Value = "Expected Time";
                    ws.Cells["N1"].Value = "IsActive";
                    ws.Cells["O1"].Value = "IsValueAdded";
                    ws.Cells["P1"].Value = "URL";

                    ws.Cells["A1:P1"].Style.Font.Bold = true;
                    ws.Cells["A1:P1"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    ws.Cells["A1:P1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells["A1:P1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Navy);
                    ws.Cells["A1:P1"].Style.Locked = true;

                    // Format  cells as TEXT in a spreadsheet

                    ws.Cells["A:P"].Style.Numberformat.Format = "@";



                    ws.Cells[ws.Dimension.Address].AutoFitColumns();
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        package.SaveAs(memoryStream);
                        memoryStream.Position = 0;
                        TempData[handle] = memoryStream.ToArray();
                    }
                }
            }
            catch (Exception exE)
            {
                try
                {
                    using (ErrorHandle errH = new ErrorHandle())
                    { errH.WriteErrorLog(exE); }
                }
                catch (Exception exC) { }
            }
            return Json(handle);
        }
        
        public ActionResult UploadExcelForCreateNewProjectTask(FormCollection formCollection)
        {
            string result = "";
            string errMsg = string.Empty;
            if (Request != null)
            {
                try
                {
                    StringBuilder sbContent = new StringBuilder();

                    HttpPostedFileBase file = Request.Files[0];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;

                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        DataSet ds = ConvertExcelToDataSet(file.InputStream);

                        #region create ticket
                        DataTable dtHD = ds.Tables[0];
                        if (dtHD != null && dtHD.Rows.Count > 0)
                        {
                            UserInfoHelper UIH = new UserInfoHelper(int.Parse(HttpContext.Session["sessUser"].ToString()));
                            UserModel userModel = new UserModel();
                            DataTable dtUSER = userModel.GetUsersByID(UIH.UserId);

                            userModel.EmailId = dtUSER.Rows[0]["EmailId"].ToString();
                            userModel.UserName = dtUSER.Rows[0]["Name"].ToString();
                            int uploadSuccess = 0;
                            foreach (DataRow dr in dtHD.Rows)
                            {

                                //checking mandatory field
                                if (Convert.ToString(dr["ProjectName"]) != "" && Convert.ToString(dr["TaskName"]) != "" && Convert.ToString(dr["Taskcode"]) != "" && Convert.ToString(dr["startDate"]) != "" &&
                                  Convert.ToString(dr["Status"]) != "" && Convert.ToString(dr["Assignedto"]) != "" && Convert.ToString(dr["PercentageComplete"]) != "")
                                {
                                    ProjectIssueModel projectIssueModel = new ProjectIssueModel();
                                    ProjectTaskModel model = new ProjectTaskModel();

                                    model.TaskName = Convert.ToString(dr["TaskName"]);
                                    model.TaskCode = Convert.ToString(dr["Taskcode"]);

                                    model.TaskStartDate = DateTimeHelper.ConvertStringToValidDate(dr["startDate"].ToString());
                                    model.TaskEndDate = DateTimeHelper.ConvertStringToValidDate(dr["EndDate"].ToString());
                                    var startdt = model.TaskStartDate;
                                    var enddt = model.TaskEndDate;
                                    if (startdt > enddt)
                                    {
                                        int index = dtHD.Rows.IndexOf(dr);
                                        int excelROW = index + 2;
                                        sbContent.Append("<div class='row'><b> Row No:" + excelROW + "<b>&nbsp; Start date should not be greater than end date please check your excel sheet </div></br>");
                                        goto outer;
                                    }

                                    if (dr["ExpectedTime"].ToString() != "")
                                    {
                                        model.ExpectedTime = Convert.ToDouble(dr["ExpectedTime"]);
                                    }
                                    else
                                    {
                                        model.ExpectedTime = 0.00;
                                    }


                                    model.CompletePercent = Convert.ToInt32(dr["PercentageComplete"]);
                                    if (Convert.ToString(dr["ActualStartDate"]) != "")
                                    {
                                        model.ActualTaskStartDate = DateTimeHelper.ConvertStringToValidDate(dr["ActualStartDate"].ToString());
                                    }
                                    if (Convert.ToString(dr["ActualEndDate"]) != "")
                                    {
                                        model.ActualTaskEndDate = DateTimeHelper.ConvertStringToValidDate(dr["ActualEndDate"].ToString());
                                    }
                                    if (dr["IsActive"].ToString() != "") { model.IsActive = Convert.ToBoolean(dr["IsActive"]); }
                                    else { model.IsActive = true; }

                                    if (dr["IsValueAdded"].ToString() != "") { model.IsValueAdded = Convert.ToBoolean(dr["IsValueAdded"]); }
                                    else { model.IsValueAdded = true; }

                                    if (dr["IsMilestone"].ToString() != "") { model.IsMilestone = Convert.ToBoolean(dr["IsMilestone"]); }
                                    else { model.IsMilestone = true; }

                                    model.AddedBy = loggedInUser;
                                    model.AddedTS = DateTime.Now;
                                    model.URL = Convert.ToString(dr["URL"]);
                                    model.DirectoryName = "";

                                    //Get ProjectId
                                    DataTable dtProjectId = projectIssueModel.GetProjectIDByProjectName(Convert.ToString(dr["ProjectName"]));
                                    if (dtProjectId.Rows.Count > 0)
                                    {
                                        model.ProjectID = Convert.ToInt32(dtProjectId.Rows[0]["ID"]);
                                        model.ProjectName = Convert.ToString(dr["ProjectName"]);
                                    }
                                    else
                                    {
                                        //For Project Name not correct
                                        int index = dtHD.Rows.IndexOf(dr);
                                        int excelROW = index + 2;
                                        sbContent.Append("<div class='row'><b> Row No:" + excelROW + "<b>&nbsp; project name does not correct check your excel sheet</div><br/>");

                                        goto outer;
                                    }
                                    //Get Parent TaskId
                                    if (Convert.ToString(dr["ParentTask"]) != "")
                                    {
                                        DataTable dtParentTaskId = model.GetParentTasksId_ByParentTaskName(Convert.ToString(dr["ParentTask"]), model.ProjectID);
                                        if (dtParentTaskId.Rows.Count > 0)
                                        {
                                            model.ParentTaskId = Convert.ToInt32(dtParentTaskId.Rows[0]["TaskID"]);
                                        }
                                        else
                                        {
                                            //For Parent task Name not correct
                                            int index = dtHD.Rows.IndexOf(dr);
                                            int excelROW = index + 2;
                                            sbContent.Append("<div class='row'><b> Row No:" + excelROW + "<b>&nbsp; Parent Task name does not correct check your excel sheet</div><br/>");

                                            goto outer;
                                        }
                                    }
                                    else
                                    {
                                        model.ParentTaskId = 0;
                                    }
                                    //Get UserID
                                    String username = Convert.ToString(dr["Assignedto"]);

                                    string[] userNameArr = username.Split(';');
                                    for (int j = 0; j < userNameArr.Length; j++)
                                    {
                                        DataTable dtUserId = projectIssueModel.GetUserIDByUserName(userNameArr[j], model.ProjectID);
                                        if (dtUserId.Rows.Count > 0)
                                        {
                                            model.UserIdsTaskAssigned = model.UserIdsTaskAssigned + dtUserId.Rows[0]["Id"].ToString() + ',';
                                        }
                                        else
                                        {
                                            //For UserID not correct
                                            int index = dtHD.Rows.IndexOf(dr);
                                            int excelROW = index + 2;
                                            sbContent.Append("<div class='row'><b> Row No:" + excelROW + "<b>&nbsp; User name does not correct please check your excel sheet and put ';' between two username</div><br/>");

                                            goto outer;
                                        }
                                    }

                                    //Get StatusID 
                                    DataSet dataset = projectIssueModel.Get_Status_Servity_TicketName("0", Convert.ToString(dr["Status"]),"0", UIH.UserId);
                                    if (dataset != null && dataset.Tables[1] != null )
                                    {

                                        model.TaskStatusID = Convert.ToInt32(dataset.Tables[1].Rows[0]["StatusID"]);
                                    }
                                    else
                                    {
                                        int index = dtHD.Rows.IndexOf(dr);
                                        int excelROW = index + 2;
                                        sbContent.Append("<div class='row'><b> Row No:" + excelROW + "<b>&nbsp; status name does not correct please check your excel sheet </div></br>");
                                        goto outer;
                                    }
                                    //save Project task
                                    var insertStatus = model.InsertTaskdata(model, out int id);
                                    if (insertStatus)
                                    {
                                        if (id > 0)
                                        {
                                            uploadSuccess++;
                                             //Save Comment details ...satrt
                                              
                                                int outCommentID = 0;
                                                ProjectTaskCommentModel ptcm = new ProjectTaskCommentModel();
                                                ptcm.ProjectTaskID = id;
                                                ptcm.Comment = "";
                                                ptcm.TaskStatusID = model.TaskStatusID;
                                                ptcm.AddedBy = loggedInUser;
                                                ptcm.AddedTS = DateTime.Now;
                                                ptcm.InsertCommentdata(ptcm, out outCommentID);

                                            // if (model.DirectoryName != null)
                                            // {
                                            //  ProjectTaskAttachmentModel ptam = new ProjectTaskAttachmentModel();
                                            ProjectTaskAttachmentModel ptam = new ProjectTaskAttachmentModel();
                                            ptam.ProjectTaskCommentID = outCommentID;

                                                    ptam.DirectoryName = "";
                                                    ptam.ProjectTaskID = id;
                                                    ptam.URL = model.URL;
                                                    ptam.AddedBy = loggedInUser;
                                                    ptam.AddedTS = DateTime.Now;
                                            ptam.UpdateAttachmentsdataWithProjectTaskID(ptam);
                                            // }


                                            model.ISErr = false;
                                            model.ErrString = "Data Saved Successfully.";
                                            TempData["ErrStatus"] = model.ISErr;
                                            TempData["ErrMsg"] = model.ErrString.ToString();
                                            result = "Success";

                                            //save Comment details .. End

                                        }
                                    }  //Project task save ..End

                                    else
                                    {
                                        model.ISErr = true;
                                        model.ErrString = "Error Occured.";
                                        TempData["ErrStatus"] = model.ISErr;
                                        TempData["ErrMsg"] = model.ErrString.ToString();
                                        result = "Error";
                                    }
                                    //****save end
                                }
                                else
                                {
                                    int index = dtHD.Rows.IndexOf(dr);
                                    int excelROW = index + 2;
                                    sbContent.Append("<div class='row'><b> Row No:" + excelROW + "<b>&nbsp;Please put all mandatory value  in your excel sheet </div></br>");

                                    goto outer;
                                }
                            //***Next row
                            outer:
                                continue;
                            }
                            int totalEXCELRecord = dtHD.Rows.Count;
                            int failure = totalEXCELRecord - uploadSuccess;
                            sbContent.Append("<p><b>" + uploadSuccess + " rows Successfully saved out of " + totalEXCELRecord + " record in your excel sheet<b></p>");
                            sbContent.Append("<p><b>" + failure + " rows can not save out of " + totalEXCELRecord + " record in your excel sheet<b></p>");
                            sendMail_InBulkTaskUploadingTime(userModel.UserName, userModel.EmailId, sbContent.ToString());
                        }
                        #endregion
                    }
                }
                catch (Exception exE)
                {
                    try
                    {
                        using (ErrorHandle errH = new ErrorHandle())
                        { errH.WriteErrorLog(exE); }
                    }
                    catch (Exception exC) { }
                    return null;
                }
            }
            return Json(result);
        }

        public void sendMail_InBulkTaskUploadingTime(string username, string emailid, string body)
        {
            string strSubject = @" Data processing report of Bulk Project Task";
            string strBody = string.Format(@"Dear {0},
                                                        <br><br>
                                                        {1}
                                                        <br><br>
                                                        Please login to timetracker System to view the uploaded create Project Task.
                                                        <br><br>
                                                        Thanks & Regards,<br>
                                                        QBA Administrator
                                                        <br><br><br><br>
                                                        *This is a system generated email. Please do not respond.
                                                        ", username, body);

            using (SendMailClass sm = new SendMailClass())
            { sm.SendMail(emailid, strSubject, strBody, ConfigurationManager.AppSettings["smtpFrom"], ConfigurationManager.AppSettings["smtpPass"]); }

        }
        public void sendMail_InBulkTicketUploadingTime(string username,string emailid,string body)
        {
           string strSubject = @" Data processing report of Bulk Ticket";
           string strBody = string.Format(@"Dear {0},
                                                        <br><br>
                                                        {1}
                                                        <br><br>
                                                        Please login to timetracker System to view the uploaded create Ticket.
                                                        <br><br>
                                                        Thanks & Regards,<br>
                                                        QBA Administrator
                                                        <br><br><br><br>
                                                        *This is a system generated email. Please do not respond.
                                                        ", username, body);

            using (SendMailClass sm = new SendMailClass())
            { sm.SendMail(emailid, strSubject, strBody, ConfigurationManager.AppSettings["smtpFrom"], ConfigurationManager.AppSettings["smtpPass"]); }

        }
        public void sendMail_afterSaveTicket(string username, string emailid, string body)
        {
            string strSubject = @"Ticket Created";
            string strBody = string.Format(@"Dear {0},
                                                        <br><br>
                                                        {1}
                                                        <br><br>
                                                        Please login to timetracker System to view the create Ticket.
                                                        <br><br>
                                                        Thanks & Regards,<br>
                                                        QBA Administrator
                                                        <br><br><br><br>
                                                        *This is a system generated email. Please do not respond.
                                                        ", username, body);

            using (SendMailClass sm = new SendMailClass())
            { sm.SendMail(emailid, strSubject, strBody, ConfigurationManager.AppSettings["smtpFrom"], ConfigurationManager.AppSettings["smtpPass"]); }

        }
        private DataSet ConvertExcelToDataSet(System.IO.Stream newStream)
        {
            DataSet ds = null;
            try
            {
                using (var package = new ExcelPackage(newStream))
                {
                    ds = new DataSet();
                    foreach (ExcelWorksheet ew in package.Workbook.Worksheets)
                    {
                        DataTable dt = new DataTable(ew.Name);
                        var currentSheet = ew;// package.Workbook.Worksheets;
                        var workSheet = ew;// currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;

                        for (int intCol = 1; intCol <= noOfCol; intCol++)
                        {
                            string strColName = (workSheet.Cells[1, intCol].Value != null) ? workSheet.Cells[1, intCol].Value.ToString().Trim() : "NoName" + intCol.ToString();
                            dt.Columns.Add(Regex.Replace(strColName, @"[^0-9a-zA-Z]+", ""), typeof(string));
                        }
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            DataRow dr = dt.NewRow();
                            for (int intCol = 1; intCol <= noOfCol; intCol++)
                            {
                                dr[intCol - 1] = (workSheet.Cells[rowIterator, intCol].Text != null) ? workSheet.Cells[rowIterator, intCol].Text.ToString() : "";
                            }
                            dt.Rows.Add(dr);
                        }
                        ds.Tables.Add(dt);
                    }
                }
            }
            catch (Exception exE)
            {
                try
                {
                    using (ErrorHandle errH = new ErrorHandle())
                    { errH.WriteErrorLog(exE); }
                }
                catch (Exception exC) { }
            }
            return ds;
        }


    }
}