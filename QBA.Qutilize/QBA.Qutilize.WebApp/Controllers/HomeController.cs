//using QBA.Qutilize.WebApp.DAL;
using QBA.Qutilize.WebApp.Helper;
using QBA.Qutilize.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace QBA.Qutilize.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //string st = null;
            //string  st1 =st;
            //DateTime dd;
            //DateTime.TryParse(st, out dd);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                LoginViewModel lvm = new LoginViewModel();
                model.Password = EncryptionHelper.ConvertStringToMD5(model.Password);
                DataSet ds = lvm.VerifyLogin(model.UserID, model.Password);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    Session.Add("sessUserAllData", ds);
                    Session.Add("sessUser", Convert.ToString(ds.Tables[0].Rows[0]["Id"]));
                    try
                    {
                        UserSessionLog mUSL = new UserSessionLog();
                        mUSL.LoggerId = Guid.NewGuid();
                        mUSL.LogedUserId = (Session["sessUser"] != null) ? Convert.ToInt32(Session["sessUser"]) : 0;
                        mUSL.IPAddress = Request.UserHostAddress;
                        mUSL.Application = "Web";
                        mUSL.StartTime = DateTime.Now;
                        mUSL.EndTime = DateTime.Now;
                        mUSL.SetUserSessionLog(mUSL);
                    }
                    catch (Exception exx) { }
                    return RedirectToAction("DashBoard", new { U = EncryptionHelper.Encryptdata(model.UserID), P = EncryptionHelper.Encryptdata(model.Password) });
                }
                else
                {
                    ViewData["ErrStatus"] = "N";
                    ModelState.AddModelError("CustomError", "Invalid login attempt.");
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrStatus"] = "N";
                ModelState.AddModelError("CustomError", "Invalid login attempt.");
            }
            //return RedirectToAction("Login");
            return View(model);
        }

        public ActionResult Logout() //log out 
        {
            //ExternalLoginListViewModel obj = new Models.ExternalLoginListViewModel();
            //userLog model = new userLog();
            //model.logout_time = DateTime.Now;
            //model.id = int.Parse(System.Web.HttpContext.Current.Session["LogID"].ToString());
            //obj.UpdateUserLogOutLog(model);
            try
            {
                UserSessionLog mUSL = new UserSessionLog();
                mUSL.LogedUserId = (Session["sessUser"] != null) ? Convert.ToInt32(Session["sessUser"]) : 0;
                mUSL.Application = "Web";
                mUSL.EndTime = DateTime.Now;
                mUSL.SetUserSessionLogout(mUSL);
            }
            catch (Exception exx) { }
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult DashBoard()
        {
            try
            {
                #region Session or Querystring checking for authenticate login
                LoginViewModel lvm = new LoginViewModel();
                if (Session["sessUserAllData"] == null)
                {
                    //get value from query string and create session
                    string strUser = EncryptionHelper.Decryptdata(Request.QueryString["U"]);
                    string strPass = EncryptionHelper.Decryptdata(Request.QueryString["P"]);
                    //LoginViewModel lvm = new LoginViewModel();
                    //strPass = EncryptionHelper.ConvertStringToMD5(strPass);
                    DataSet ds = lvm.VerifyLogin(strUser, strPass);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        Session.Add("sessUser", Convert.ToString(ds.Tables[0].Rows[0]["Id"]));
                        Session.Add("sessUserAllData", ds);
                        try
                        {
                            UserSessionLog mUSL = new UserSessionLog();
                            mUSL.LoggerId = Guid.NewGuid();
                            mUSL.LogedUserId = (Session["sessUser"] != null) ? Convert.ToInt32(Session["sessUser"]) : 0;
                            mUSL.IPAddress = Request.UserHostAddress;
                            mUSL.Application = "Console2Web";
                            mUSL.StartTime = DateTime.Now;
                            mUSL.EndTime = DateTime.Now;
                            mUSL.SetUserSessionLog(mUSL);
                        }
                        catch (Exception exx) { }
                    }
                }
                else
                {
                    try
                    {
                        DataSet dsSess = (DataSet)Session["sessUserAllData"];
                        string strUser = EncryptionHelper.Decryptdata(Request.QueryString["U"]);
                        string strPass = EncryptionHelper.Decryptdata(Request.QueryString["P"]);
                        if (strUser.Trim() != string.Empty && Convert.ToString(dsSess.Tables[0].Rows[0]["UserName"]).Trim() != strUser)
                        {
                            //LoginViewModel lvm = new LoginViewModel();
                            //strPass = EncryptionHelper.ConvertStringToMD5(strPass);
                            DataSet ds1 = lvm.VerifyLogin(strUser, strPass);
                            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0] != null && ds1.Tables[0].Rows.Count > 0)
                            {
                                Session.Add("sessUser", Convert.ToString(ds1.Tables[0].Rows[0]["Id"]));
                                try
                                {
                                    UserSessionLog mUSL = new UserSessionLog();
                                    mUSL.LoggerId = Guid.NewGuid();
                                    mUSL.LogedUserId = (Session["sessUser"] != null) ? Convert.ToInt32(Session["sessUser"]) : 0;
                                    mUSL.IPAddress = Request.UserHostAddress;
                                    mUSL.Application = "Web";
                                    mUSL.StartTime = DateTime.Now;
                                    mUSL.EndTime = DateTime.Now;
                                    mUSL.SetUserSessionLog(mUSL);
                                }
                                catch (Exception exx) { }
                                Session.Add("sessUserAllData", ds1);
                            }
                        }
                    }
                    catch (Exception exx)
                    { }
                }
                if (Session["sessUserAllData"] != null)
                {
                    DataSet ds = (DataSet)Session["sessUserAllData"];// lvm.GetUserDetailData(Convert.ToInt32(Session["sessUser"]));
                    Session.Add("UserID", ds.Tables[0].Rows[0]["Id"]);
                    Session.Add("Name", ds.Tables[0].Rows[0]["Name"]);
                    Session.Add("Email", ds.Tables[0].Rows[0]["EmailId"]);
                    Session.Add("EmployeeCode", ds.Tables[0].Rows[0]["EmployeeCode"]);
                    Session.Add("Designation", ds.Tables[0].Rows[0]["Designation"]);
                    Session.Add("ManagerName", ds.Tables[0].Rows[0]["ManagerName"]);
                    Session.Add("ManagerEmpCode", ds.Tables[0].Rows[0]["ManagerEmpCode"]);
                    Session.Add("UserGender", ds.Tables[0].Rows[0]["Gender"]);
                    Session.Add("ORGID", ds.Tables[0].Rows[0]["ORGID"]);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            if (Convert.ToString(dr["roleID"]) == "1")//sysadmin
                            { Session.Add("SysAdmin", Convert.ToString(dr["roleID"])); }
                            else if (Convert.ToString(dr["roleID"]) == "2" || Convert.ToString(dr["roleID"]) == "16")//org user
                            { Session.Add("OrgUser", Convert.ToString(dr["roleID"])); }
                            else if (Convert.ToString(dr["roleID"]) == "3" || Convert.ToString(dr["roleID"]) == "14")//org admin
                            { Session.Add("OrgAdmin", Convert.ToString(dr["roleID"])); }
                            else if (Convert.ToString(dr["roleID"]) == "4" || Convert.ToString(dr["roleID"]) == "15")//org PM
                            { Session.Add("OrgPM", Convert.ToString(dr["roleID"])); }
                        }
                    }

                    if (Session["sessAdminProject"] != null) Session.Remove("sessAdminProject");
                    if (Session["sessAdminTask"] != null) Session.Remove("sessAdminTask");
                    if (Session["sessAdminTicket"] != null) Session.Remove("sessAdminTicket");
                    if (Session["sessAdminUtilization"] != null) Session.Remove("sessAdminUtilization");

                    if (Session["sessPMProject"] != null) Session.Remove("sessPMProject");
                    if (Session["sessPMTask"] != null) Session.Remove("sessPMTask");
                    if (Session["sessPMTicket"] != null) Session.Remove("sessPMTicket");
                    if (Session["sessPMUtilization"] != null) Session.Remove("sessPMUtilization");

                    if (Session["sessSelfTask"] != null) Session.Remove("sessSelfTask");
                    if (Session["sessSelfTicket"] != null) Session.Remove("sessSelfTicket");
                    if (Session["sessSelfUtilization"] != null) Session.Remove("sessSelfUtilization");

                    if (Session["OrgAdmin"] != null)
                    {
                        Session.Add("sessAdminProject", lvm.GetDashBoardAdminProjectData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(Session["ORGID"])));
                        Session.Add("sessAdminTask", lvm.GetDashBoardAdminTaskData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(Session["ORGID"])));
                        Session.Add("sessAdminTicket", lvm.GetDashBoardAdminTicketData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(Session["ORGID"])));
                        Session.Add("sessAdminUtilization", lvm.GetDashBoardAdminTisheetData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(Session["ORGID"])));
                        //ds = (DataSet)Session["sessAdminProject"];

                    }
                    if (Session["OrgPM"] != null)
                    {
                        Session.Add("sessPMProject", lvm.GetDashBoardPMProjectData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(Session["ORGID"])));
                        Session.Add("sessPMTask", lvm.GetDashBoardPMTaskData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(Session["ORGID"])));
                        Session.Add("sessPMTicket", lvm.GetDashBoardPMTicketData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(Session["ORGID"])));
                        Session.Add("sessPMUtilization", lvm.GetDashBoardPMTisheetData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(Session["ORGID"])));
                    }
                    if (Session["OrgUser"] != null)
                    {
                        Session.Add("sessSelfTask", lvm.GetDashBoardSelfTaskData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(Session["ORGID"])));
                        Session.Add("sessSelfTicket", lvm.GetDashBoardSelfTicketData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(Session["ORGID"])));
                        Session.Add("sessSelfUtilization", lvm.GetDashBoardSelfTisheetData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(Session["ORGID"])));
                    }

                }
                else
                { return RedirectToAction("Index", "Home"); }
                #endregion
            }
            catch (Exception exx)
            { }
            return View();
        }
        #region Old Dashboard
        public ActionResult GetDateRange()
        {
            StringBuilder sbOut = new StringBuilder();
            try
            {
                DateTime startdate = DateTime.Now;
                DateTime endDate = DateTime.Now;
                string strUser = "0";
                string strProject = "0";
                string isSelectedUser = "False";
                string isSelectedProject = "False";
                if (Session["DateRange"] == null)
                {
                    DayOfWeek day = DateTime.Now.DayOfWeek;
                    int days = day - DayOfWeek.Monday;
                    startdate = DateTime.Now.AddDays(-days);
                    endDate = startdate.AddDays(6);
                    Session.Add("DateRange", startdate + "|" + endDate + "|" + strUser + "|" + strProject + "|" + isSelectedUser + "|" + isSelectedProject);
                }
                else
                {
                    string[] arrdate = Session["DateRange"].ToString().Split('|');
                    startdate = Convert.ToDateTime(arrdate[0]);
                    endDate = Convert.ToDateTime(arrdate[1]);
                    strUser = arrdate[2];
                    strProject = arrdate[3];
                    isSelectedProject = arrdate[4].ToString();
                    isSelectedUser = arrdate[5].ToString();
                }
                LoginViewModel lvm = new LoginViewModel();
                DataSet ds = lvm.GetDashBoardData(Convert.ToInt32(Session["sessUser"]), startdate, endDate, strUser, strProject);
                if (ds != null && ds.Tables.Count > 0 && ((ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) || (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0) || (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0) || (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)))
                {
                    Session.Add("DashBoardDetail", ds);
                }
                sbOut.Append("<table class='table myTable' id='tblFilter'>");
                sbOut.Append("<tr>");
                sbOut.Append("<td class='text-right control-label'><nobr>Start Date: </nobr></td>");
                sbOut.Append("<td class='text-left'><input type='text' class='form-control' id='txtStartDate' value='" + startdate.ToShortDateString() + "' /></td>");
                sbOut.Append("<td class='text-right control-label'><nobr>End Date: </nobr></td>");
                sbOut.Append("<td class='text-left'><input type='text' class='form-control' id='txtEndDate' value='" + endDate.ToShortDateString() + "' /></td>");
                #region organization admin option
                if (HttpContext.Session["OrgAdmin"] != null)
                {
                    UserInfoHelper UIH = new UserInfoHelper(int.Parse(HttpContext.Session["sessUser"].ToString()));
                    UserProjectMappingModel USM = new UserProjectMappingModel();
                    DataTable dt = new DataTable();
                    sbOut.Append("<td class='text-right control-label'><nobr>Select User: </nobr></td>");
                    sbOut.Append("<td class='text-left'>");
                    sbOut.Append("<select class='form-control' id='ddlUsers' name='ddlUsers' onchange='GetUsersProject()'>");
                    sbOut.Append("<option value='0'>Select</option>");
                    try
                    {
                        var dtActiveUsers = USM.GetAllUsers(UIH.UserOrganisationID).Select("IsActive=1");
                        if (dtActiveUsers.Length > 0)
                        {
                            if (isSelectedProject != "False")
                            {
                                UserModel um = new UserModel();
                                um.ActiveMemberList = um.Get_GetProjectMembersByProjectID(Convert.ToInt32(strProject));
                                foreach (var member in um.ActiveMemberList)
                                {
                                    if (strUser == Convert.ToString(member.ID))
                                        sbOut.Append("<option value='" + member.ID + "' selected>" + member.Name + "</option>");
                                    else
                                        sbOut.Append("<option value='" + member.ID + "'>" + member.Name + "</option>");
                                }
                            }
                            else
                            {
                                dt = dtActiveUsers.CopyToDataTable();
                                for (int i = 0; i < dtActiveUsers.Length; i++)
                                {
                                    if (strUser == Convert.ToString(dt.Rows[i]["Id"]))
                                        sbOut.Append("<option value='" + dt.Rows[i]["Id"] + "' selected>" + dt.Rows[i]["Name"] + "</option>");
                                    else
                                        sbOut.Append("<option value='" + dt.Rows[i]["Id"] + "'>" + dt.Rows[i]["Name"] + "</option>");
                                }
                            }


                        }
                    }
                    catch (Exception ex) { }
                    sbOut.Append("</select>");
                    sbOut.Append("</td>");

                    sbOut.Append("<td class='text-right control-label'><nobr>Select Project:</nobr></td>");
                    ProjectModel pm = new ProjectModel();
                    DataTable dtAllProjects = new DataTable();
                    if (isSelectedUser != "False")
                    {
                        dtAllProjects = pm.GetAllProjectsByUserID(Convert.ToInt32(strUser.ToString()));
                    }
                    else
                    {
                        dtAllProjects = pm.GetAllProjects(UIH.UserOrganisationID);
                    }

                    sbOut.Append("<td class='text-left'>");
                    sbOut.Append("<select class='form-control' id='ddlProjects' name='ddlProjects' onchange='GetProjectUsers()'>");
                    sbOut.Append("<option value='0'>Select</option>");
                    try
                    {
                        if (dtAllProjects.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtAllProjects.Rows.Count; i++)
                            {
                                if (strProject == Convert.ToString(dtAllProjects.Rows[i]["Id"]))
                                    sbOut.Append("<option value='" + dtAllProjects.Rows[i]["Id"] + "' selected>" + dtAllProjects.Rows[i]["Name"] + "</option>");
                                else
                                    sbOut.Append("<option value='" + dtAllProjects.Rows[i]["Id"] + "'>" + dtAllProjects.Rows[i]["Name"] + "</option>");
                            }
                        }
                    }
                    catch (Exception ex) { }
                    sbOut.Append("</select>");
                    sbOut.Append("</td>");
                }
                #endregion
                sbOut.Append("<td class='text-center'><input type='submit' id='btnSearch' value='Show' name='btnSearch' class='btn btn-primary' onclick='RefreshData();' /></td>");
                sbOut.Append("<td class='text-center'><input type='submit' id='btnReset' value='Reset' name='btnSearch' class='btn btn-primary' onclick='ResetData();' /></td>");
                sbOut.Append("</tr>");
                sbOut.Append("</table>");

                //sbOut.Append("<div class='form-group col-md-12'>");
                //sbOut.Append("<label class='control-label col-md-2'>Start Date: </label>");
                //sbOut.Append("<div class='col-md-2'>");
                //sbOut.Append("<input type='text' class='form-control' id='txtStartDate' value='" + startdate.ToShortDateString() + "'  />");
                //sbOut.Append("</div>");
                //sbOut.Append("<label class='control-label col-md-2'>End Date: </label>");
                //sbOut.Append("<div class='col-md-2'>");
                //sbOut.Append("<input type='text' class='form-control' id='txtEndDate' value='" + endDate.ToShortDateString() + "' />");
                //sbOut.Append("</div>");
                //sbOut.Append("<div class='col-md-2'>");
                //sbOut.Append("<input type='submit' id='btnSearch' value='Show' name='btnSearch' class='btn btn-primary' onclick='RefreshData();' />");
                //sbOut.Append("</div>");
                //sbOut.Append("</div>");
                //sbOut.Append("<hr>");

            }
            catch (Exception exx)
            { }
            return Content(sbOut.ToString());
        }
        public ActionResult GetTableForDaywisedetailsinMinuteForAdmin()
        {
            StringBuilder sbContent = new StringBuilder();
            DataSet ds = (DataSet)Session["DashBoardDetail"];
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
            {
                sbContent.Append("<div class='panel panel-info'>");
                sbContent.Append("<div class='panel-heading'><label>User wise details :</label></div>");
                sbContent.Append("<div class='panel-body'>");
                sbContent.Append("<div class='table-responsive'>");
                sbContent.Append("<table id='TableuserWiseDetailsInMinutes' class='table table-bordered dataTable no-footer' width='100%'>");
                sbContent.Append("<thead><tr><th class='text-center tblHeaderColor'>Date</th><th class='text-center tblHeaderColor'>Employee</th><th class='text-center tblHeaderColor'>Project</th><th class='text-center tblHeaderColor'>Total Hours(HH:MM:SS)</th><th style='display: none;'>Total Sec</th></tr></thead>");
                sbContent.Append("<tbody id='tbodyuserWiseDetailsInMinutes'>");

                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                {
                    sbContent.Append("<tr>");
                    sbContent.Append("<td><span class='control-text'>" + ds.Tables[3].Rows[i]["Date"] + "</span></td><td><span class='control-text'>" + ds.Tables[3].Rows[i]["UserName"] + "</span></td><td><span class='control-text'>" + ds.Tables[3].Rows[i]["projectName"] + "</span></td><td><span class='control-text'>" + ds.Tables[3].Rows[i]["hms"] + "</span></td><td style='display: none;'>" + ds.Tables[3].Rows[i]["totalSec"] + "</td>");
                    sbContent.Append("</tr>");
                }


                sbContent.Append("</tbody>");
                sbContent.Append("</table>");
                sbContent.Append("</div>");
                sbContent.Append("</div>");
                sbContent.Append("</div>");
            }
            return Content(sbContent.ToString());
        }
        public ActionResult GetTableForTicketDetailsForAdmin()
        {
            StringBuilder sbContent = new StringBuilder();
            DataSet ds = (DataSet)Session["DashBoardDetail"];
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
            {
                sbContent.Append("<div class='panel panel-info'>");
                sbContent.Append("<div class='panel-heading'><label>Project wise Ticket status :</label></div>");
                sbContent.Append("<div class='panel-body'>");
                sbContent.Append("<div class='table-responsive'>");
                sbContent.Append("<table id='TableForDaywiseTicketDetails' class='table table-bordered dataTable no-footer' width='100%'>");
                sbContent.Append("<thead><tr>" +
                    "<th class='text-center tblHeaderColor'>Project</th>" +
                    "<th class='text-center tblHeaderColor'>OPEN</th>" +
                    "<th class='text-center tblHeaderColor'>INPROGRESS</th>" +
                    "<th class='text-center tblHeaderColor'>CLOSED</th></tr></thead>");
                sbContent.Append("<tbody id='tbodyuserWiseDetailsInMinutes'>");

                for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                {
                    sbContent.Append("<tr>");
                    sbContent.Append("<td><span class='control-text'>" + ds.Tables[4].Rows[i]["projectName"] + "</span></td>" +
                        "<td><span class='control-text'>" + ds.Tables[4].Rows[i]["OPEN"] + "</span></td>" +
                        "<td><span class='control-text'>" + ds.Tables[4].Rows[i]["INPROGRESS"] + "</span></td>" +
                        "<td><span class='control-text'>" + ds.Tables[4].Rows[i]["CLOSED"] + "</span></td>");
                    sbContent.Append("</tr>");
                }


                sbContent.Append("</tbody>");
                sbContent.Append("</table>");
                sbContent.Append("</div>");
                sbContent.Append("</div>");
                sbContent.Append("</div>");
            }
            return Content(sbContent.ToString());
        }
        public ActionResult GetTableForTaskDetailsForAdmin()
        {
            StringBuilder sbContent = new StringBuilder();
            DataSet ds = (DataSet)Session["DashBoardDetail"];
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[5] != null && ds.Tables[5].Rows.Count > 0)
            {
                sbContent.Append("<div class='panel panel-info'>");
                sbContent.Append("<div class='panel-heading'><label>Project wise Task status :</label></div>");
                sbContent.Append("<div class='panel-body'>");
                sbContent.Append("<div class='table-responsive'>");
                sbContent.Append("<table id='TableForDaywiseTaslDetails' class='table table-bordered dataTable no-footer' width='100%'>");
                sbContent.Append("<thead><tr>" +
                    "<th class='text-center tblHeaderColor'>Project</th>" +
                    "<th class='text-center tblHeaderColor'>OPEN</th>" +
                    "<th class='text-center tblHeaderColor'>INPROGRESS</th>" +
                    "<th class='text-center tblHeaderColor'>COMPLETED</th></tr></thead>");
                sbContent.Append("<tbody id='tbodyuserWiseDetailsInMinutes'>");

                for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                {
                    sbContent.Append("<tr>");
                    sbContent.Append("<td><span class='control-text'>" + ds.Tables[5].Rows[i]["projectName"] + "</span></td>" +
                        "<td><span class='control-text'>" + ds.Tables[5].Rows[i]["OPEN"] + "</span></td>" +
                        "<td><span class='control-text'>" + ds.Tables[5].Rows[i]["INPROGRESS"] + "</span></td>" +
                        "<td><span class='control-text'>" + ds.Tables[5].Rows[i]["CLOSED"] + "</span></td>");
                    sbContent.Append("</tr>");
                }


                sbContent.Append("</tbody>");
                sbContent.Append("</table>");
                sbContent.Append("</div>");
                sbContent.Append("</div>");
                sbContent.Append("</div>");
            }
            return Content(sbContent.ToString());
        }
        public ActionResult GetTableForDaywisedetailsinMinuteForPM()
        {
            StringBuilder sbContent = new StringBuilder();
            DataSet ds = (DataSet)Session["DashBoardDetail"];
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
            {
                sbContent.Append("<div class='panel panel-info'>");
                sbContent.Append("<div class='panel-heading'><label>User Wise details in minute:</label></div>");
                sbContent.Append("<div class='panel-body'>");
                sbContent.Append("<div class='table-responsive'>");
                sbContent.Append("<table id='TableuserWiseDetailsInMinutesForPM' class='table table-bordered dataTable no-footer' width='100%'>");
                sbContent.Append("<thead><tr><th class='text-center tblHeaderColor'>Date</th><th class='text-center tblHeaderColor'>Employee</th><th class='text-center tblHeaderColor'>Project</th><th class='text-center tblHeaderColor'>Total Hours(HH:MM:SS)</th><th style='display: none;'>Total Sec</th></tr></thead>");
                sbContent.Append("<tbody id='tbodyuserWiseDetailsInMinutes'>");

                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    sbContent.Append("<tr>");
                    sbContent.Append("<td><span class='control-text'>" + ds.Tables[2].Rows[i]["Date"] + "</span></td><td><span class='control-text'>" + ds.Tables[2].Rows[i]["UserName"] + "</span></td><td><span class='control-text'>" + ds.Tables[2].Rows[i]["projectName"] + "</span></td><td><span class='control-text'>" + ds.Tables[2].Rows[i]["hms"] + "</span></td><td style='display: none;'>" + ds.Tables[2].Rows[i]["totalSec"] + "</td>");
                    sbContent.Append("</tr>");
                }


                sbContent.Append("</tbody>");
                sbContent.Append("</table>");
                sbContent.Append("</div>");
                sbContent.Append("</div>");
                sbContent.Append("</div>");
            }
            return Content(sbContent.ToString());
        }

        public JsonResult BarChartData()
        {
            Chart _chart = new Chart();
            try
            {
                DataSet ds = (DataSet)Session["DashBoardDetail"];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable uniqueColsProj = ds.Tables[0].DefaultView.ToTable(true, "projectName");
                    string[] arrrayProj = uniqueColsProj.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                    DataTable uniqueColsDate = ds.Tables[0].DefaultView.ToTable(true, "Date");
                    string[] arrrayDate = uniqueColsDate.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                    //_chart.labels = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "Novemeber", "December" };
                    _chart.labels = arrrayDate;
                    _chart.datasets = new List<Datasets>();
                    List<Datasets> _dataSet = new List<Datasets>();
                    List<string> arrbg1 = new List<string>();
                    var random = new Random();
                    for (int iColor = 0; iColor < ((arrrayDate.Length > arrrayProj.Length) ? arrrayDate.Length : arrrayProj.Length); iColor++)
                    {
                        var color = String.Format("#{0:X6}", random.Next(0x1000000));
                        arrbg1.Add(color);
                    }
                    string[] arrbg = arrbg1.ToArray();// = new string[] {  "#f39c12", "#00c0ef", "#0073b7", "#3c8dbc", "#00a65a", "#001f3f", "#39cccc", "#3d9970", "#01ff70", "#ff851b", "#f012be", "#605ca8", "#d81b60", "#020219", "#07074c", "#0f0f99", "#1616e5", "#4646ff", "#8c8cff", "#d1d1ff", "#a3a3ff", "#babaff", "#d1d1ff", "#e8e8ff", "#E6FCDD", "#EFF7B5", "#EFB5F7",    "#194C66", "#1F5D7C", "#246E93", "#2A7FAA", "#3090C0", "#3E9ECE", "#55AAD4", "#6BB5DA", "#82C0DF"};
                    int intColor = 0;
                    foreach (string stproj in arrrayProj)
                    {
                        Datasets dss = new Datasets();
                        dss.label = stproj;
                        string[] arrData = new string[arrrayDate.Length];
                        int i = 0;
                        foreach (string stDate in arrrayDate)
                        {
                            try
                            {
                                DataRow[] result = ds.Tables[0].Select("projectName = '" + stproj + "' AND Date = '" + stDate + "'");
                                if (result.Length > 0)
                                {
                                    int totalSec = Convert.ToInt32(result[0]["totalSec"]);
                                    //arrData[i] = (totalSec / 60).ToString() + "." + (totalSec % 60).ToString();
                                    arrData[i] = (totalSec / 3600).ToString() + "." + ((totalSec % 3600) / 60).ToString();
                                }
                                else
                                { arrData[i] = "0"; }
                            }
                            catch (Exception exo) { arrData[i] = "0"; }
                            i++;
                        }
                        dss.data = arrData;
                        //"#" + ((1 << 24) * Math.random() | 0).toString(16)
                        dss.backgroundColor = GetBackColor(arrbg, intColor, ((arrrayDate.Length > arrrayProj.Length) ? arrrayDate.Length : arrrayProj.Length));// new string[] { arrbg[intColor] };// new string[] { "#" + ((1 << 24) * new Random().Next() | 0).ToString("16") };
                        dss.borderColor = new string[] { "#020219", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" };
                        //dss.backgroundColor = new string[] { "#FF0000", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" };
                        //dss.borderColor = new string[] { "#FF0000", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" };
                        dss.borderWidth = "1";
                        _dataSet.Add(dss);
                        intColor++;
                    }
                    _chart.datasets = _dataSet;
                }
            }
            catch (Exception exx) { }
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDataActual()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                DataSet ds = (DataSet)Session["DashBoardDetail"];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable uniqueColsProj = ds.Tables[0].DefaultView.ToTable(true, "projectName");
                    string[] arrrayProj = uniqueColsProj.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                    DataTable uniqueColsDate = ds.Tables[0].DefaultView.ToTable(true, "Date");
                    string[] arrrayDate = uniqueColsDate.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                    //sbContent.Append("<div class='panel-body dvBorder form-group'>");
                    sbContent.Append("<div class='table-responsive'>");
                    sbContent.Append("<table class='table table-bordered' id='tblAppraisalRating'  width='100%'>");
                    sbContent.Append("<thead>");
                    sbContent.Append("<tr>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Date</th>");
                    foreach (string strProjName in arrrayProj)
                    {
                        sbContent.Append("<th class='text-center tblHeaderColor'>" + strProjName + "</th>");
                    }
                    sbContent.Append("</tr>");
                    sbContent.Append("</thead>");
                    sbContent.Append("<tbody id='tbodyDateWiseData'>");
                    foreach (string stDate in arrrayDate)
                    {
                        sbContent.Append("<tr>");
                        sbContent.Append("<td><span class='control-text'>" + stDate + "</span></td>");
                        foreach (string stproj in arrrayProj)
                        {
                            DataRow[] result = ds.Tables[0].Select("projectName = '" + stproj + "' AND Date = '" + stDate + "'");
                            if (result.Length > 0)
                            {
                                sbContent.Append("<td><span class='control-text'>" + result[0]["hms"] + "</span></td>");
                            }
                            else
                            { sbContent.Append("<td><span class='control-text'> - </span></td>"); }
                        }
                        sbContent.Append("</tr>");
                    }
                    sbContent.Append("</tbody>");
                    sbContent.Append("</table>");
                    sbContent.Append("</div>");
                    //sbContent.Append("</div>");
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }
        public ActionResult GetTableForTicketDetailsForUser()
        {
            StringBuilder sbContent = new StringBuilder();
            DataSet ds = (DataSet)Session["DashBoardDetail"];
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[6] != null && ds.Tables[6].Rows.Count > 0)
            {
                sbContent.Append("<div class='panel panel-info'>");
                sbContent.Append("<div class='panel-heading'><label>Project wise Ticket status :</label></div>");
                sbContent.Append("<div class='panel-body'>");
                sbContent.Append("<div class='table-responsive'>");
                sbContent.Append("<table id='TableForDaywiseTicketDetailsUser' class='table table-bordered dataTable no-footer' width='100%'>");
                sbContent.Append("<thead><tr>" +
                    "<th class='text-center tblHeaderColor'>Project</th>" +
                    "<th class='text-center tblHeaderColor'>OPEN</th>" +
                    "<th class='text-center tblHeaderColor'>INPROGRESS</th>" +
                    "<th class='text-center tblHeaderColor'>CLOSED</th></tr></thead>");
                sbContent.Append("<tbody id='tbodyuserWiseDetailsInMinutes'>");

                for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                {
                    sbContent.Append("<tr>");
                    sbContent.Append("<td><span class='control-text'>" + ds.Tables[6].Rows[i]["projectName"] + "</span></td>" +
                        "<td><span class='control-text'>" + ds.Tables[6].Rows[i]["OPEN"] + "</span></td>" +
                        "<td><span class='control-text'>" + ds.Tables[6].Rows[i]["INPROGRESS"] + "</span></td>" +
                        "<td><span class='control-text'>" + ds.Tables[6].Rows[i]["CLOSED"] + "</span></td>");
                    sbContent.Append("</tr>");
                }


                sbContent.Append("</tbody>");
                sbContent.Append("</table>");
                sbContent.Append("</div>");
                sbContent.Append("</div>");
                sbContent.Append("</div>");
            }
            return Content(sbContent.ToString());
        }
        public ActionResult GetTableForTaskDetailsForUser()
        {
            StringBuilder sbContent = new StringBuilder();
            DataSet ds = (DataSet)Session["DashBoardDetail"];
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[7] != null && ds.Tables[7].Rows.Count > 0)
            {
                sbContent.Append("<div class='panel panel-info'>");
                sbContent.Append("<div class='panel-heading'><label>Project wise Task status :</label></div>");
                sbContent.Append("<div class='panel-body'>");
                sbContent.Append("<div class='table-responsive'>");
                sbContent.Append("<table id='TableForDaywiseTaslDetailsUser' class='table table-bordered dataTable no-footer' width='100%'>");
                sbContent.Append("<thead><tr>" +
                    "<th class='text-center tblHeaderColor'>Project</th>" +
                    "<th class='text-center tblHeaderColor'>OPEN</th>" +
                    "<th class='text-center tblHeaderColor'>INPROGRESS</th>" +
                    "<th class='text-center tblHeaderColor'>COMPLETED</th></tr></thead>");
                sbContent.Append("<tbody id='tbodyuserWiseDetailsInMinutes'>");

                for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                {
                    sbContent.Append("<tr>");
                    sbContent.Append("<td><span class='control-text'>" + ds.Tables[7].Rows[i]["projectName"] + "</span></td>" +
                        "<td><span class='control-text'>" + ds.Tables[7].Rows[i]["OPEN"] + "</span></td>" +
                        "<td><span class='control-text'>" + ds.Tables[7].Rows[i]["INPROGRESS"] + "</span></td>" +
                        "<td><span class='control-text'>" + ds.Tables[7].Rows[i]["CLOSED"] + "</span></td>");
                    sbContent.Append("</tr>");
                }


                sbContent.Append("</tbody>");
                sbContent.Append("</table>");
                sbContent.Append("</div>");
                sbContent.Append("</div>");
                sbContent.Append("</div>");
            }
            return Content(sbContent.ToString());
        }
        public JsonResult GetDonutChartData()
        {
            Chart _chart = new Chart();
            try
            {
                DataSet ds = (DataSet)Session["DashBoardDetail"];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    DataTable uniqueColsProj = ds.Tables[1].DefaultView.ToTable(true, "projectName");
                    string[] arrrayProj = uniqueColsProj.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                    string[] arrData = new string[arrrayProj.Length];
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        try
                        {
                            int totalSec = Convert.ToInt32(ds.Tables[1].Rows[i]["totalSec"]);
                            //arrData[i] = (totalSec / 60).ToString() + "." + (totalSec % 60).ToString();
                            arrData[i] = (totalSec / 3600).ToString() + "." + ((totalSec % 3600) / 60).ToString();
                        }
                        catch (Exception exo) { arrData[i] = "0"; }
                    }
                    _chart.labels = arrrayProj;// new string[] { "January", "February", "March", "April", "May", "June", "July" };
                    _chart.datasets = new List<Datasets>();
                    List<Datasets> _dataSet = new List<Datasets>();
                    List<string> arrbg1 = new List<string>();
                    var random = new Random();
                    for (int iColor = 0; iColor < arrrayProj.Length; iColor++)
                    {
                        var color = String.Format("#{0:X6}", random.Next(0x1000000));
                        arrbg1.Add(color);
                    }
                    _dataSet.Add(new Datasets()
                    {
                        label = "",
                        data = arrData,// new string[] { "28", "48", "40", "19", " 86", "27", "90" },
                        backgroundColor = arrbg1.ToArray(),// new string[] { "#f39c12", "#00c0ef", "#0073b7", "#3c8dbc", "#00a65a", "#001f3f", "#39cccc", "#3d9970", "#01ff70", "#ff851b", "#f012be", "#605ca8", "#d81b60",  "#020219", "#07074c", "#0f0f99", "#1616e5", "#4646ff", "#8c8cff", "#d1d1ff", "#a3a3ff", "#babaff", "#d1d1ff", "#e8e8ff", "#1A5276", "#27AE60", "#65184B", "#7C1E5C", "#93246D", "#A9297E", "#C02F8F", "#CF3C9D", "#D453A9", "#DA6AB4", "#E081C0"},
                        borderColor = arrbg1.ToArray(),//new string[] { "#020219", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" },
                        borderWidth = "1"
                    });
                    _chart.datasets = _dataSet;
                }
            }
            catch (Exception exx) { }
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }
        public string[] GetBackColor(string[] arrOrg, int counter, int DataLen)
        {
            string[] arrRet = new string[DataLen];
            //if (counter == 0) { return arrRet; }
            //else
            {
                for (int i = 0; i < DataLen; i++)
                {
                    //if (counter == DataLen) counter = 0;
                    arrRet[i] = arrOrg[counter];
                    //counter++;
                }
            }
            return arrRet;
        }
        public ActionResult GetRefreshedData(string startdate, string endDate, string User, string Project, bool selectedproject, bool selecteduser)
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                Session.Remove("DashBoardDetail");
                if (Session["DateRange"] == null) Session.Remove("DateRange");
                Session.Add("DateRange", startdate + "|" + endDate + "|" + User + "|" + Project + "|" + selectedproject + "|" + selecteduser);
                sbContent.Append("Reload");
            }
            catch (Exception exx) { }
            return Json(sbContent.ToString());
        }

        public ActionResult GetResetData()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {

                Session.Remove("DashBoardDetail");
                if (Session["DateRange"] != null) Session.Remove("DateRange");
                var DateRangeContent = GetDateRange() as ContentResult;
                sbContent.Append(DateRangeContent.Content);
            }
            catch (Exception exx) { }
            return Json(sbContent.ToString());
        }
        public ActionResult GetProjectsAssociatedWithYou()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                DataSet ds = (DataSet)Session["DashBoardDetail"];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable uniqueColsProj = ds.Tables[0].DefaultView.ToTable(true, "projectName");
                    string[] arrrayProj = uniqueColsProj.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                    DataTable uniqueColsDate = ds.Tables[0].DefaultView.ToTable(true, "Date");
                    string[] arrrayDate = uniqueColsDate.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                    //sbContent.Append("<div class='panel-body dvBorder form-group'>");
                    sbContent.Append("<div class='table-responsive'>");
                    sbContent.Append("<table class='table table-bordered' id='tblAppraisalRating'  width='100%'>");
                    sbContent.Append("<thead>");
                    sbContent.Append("<tr>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Date</th>");
                    foreach (string strProjName in arrrayProj)
                    {
                        sbContent.Append("<th class='text-center tblHeaderColor'>" + strProjName + "</th>");
                    }
                    sbContent.Append("</tr>");
                    sbContent.Append("</thead>");
                    sbContent.Append("<tbody id='tbodyDateWiseData'>");
                    foreach (string stDate in arrrayDate)
                    {
                        sbContent.Append("<tr>");
                        sbContent.Append("<td><span class='control-text'>" + stDate + "</span></td>");
                        foreach (string stproj in arrrayProj)
                        {
                            DataRow[] result = ds.Tables[0].Select("projectName = '" + stproj + "' AND Date = '" + stDate + "'");
                            if (result.Length > 0)
                            {
                                sbContent.Append("<td><span class='control-text'>" + result[0]["hms"] + "</span></td>");
                            }
                            else
                            { sbContent.Append("<td><span class='control-text'> - </span></td>"); }
                        }
                        sbContent.Append("</tr>");
                    }
                    sbContent.Append("</tbody>");
                    sbContent.Append("</table>");
                    sbContent.Append("</div>");
                    //sbContent.Append("</div>");
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }
        public ActionResult GetReporteeList()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                DataSet ds = (DataSet)Session["DashBoardDetail"];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable uniqueColsProj = ds.Tables[0].DefaultView.ToTable(true, "projectName");
                    string[] arrrayProj = uniqueColsProj.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                    DataTable uniqueColsDate = ds.Tables[0].DefaultView.ToTable(true, "Date");
                    string[] arrrayDate = uniqueColsDate.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                    //sbContent.Append("<div class='panel-body dvBorder form-group'>");
                    sbContent.Append("<div class='table-responsive'>");
                    sbContent.Append("<table class='table table-bordered' id='tblAppraisalRating'  width='100%'>");
                    sbContent.Append("<thead>");
                    sbContent.Append("<tr>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Date</th>");
                    foreach (string strProjName in arrrayProj)
                    {
                        sbContent.Append("<th class='text-center tblHeaderColor'>" + strProjName + "</th>");
                    }
                    sbContent.Append("</tr>");
                    sbContent.Append("</thead>");
                    sbContent.Append("<tbody id='tbodyDateWiseData'>");
                    foreach (string stDate in arrrayDate)
                    {
                        sbContent.Append("<tr>");
                        sbContent.Append("<td><span class='control-text'>" + stDate + "</span></td>");
                        foreach (string stproj in arrrayProj)
                        {
                            DataRow[] result = ds.Tables[0].Select("projectName = '" + stproj + "' AND Date = '" + stDate + "'");
                            if (result.Length > 0)
                            {
                                sbContent.Append("<td><span class='control-text'>" + result[0]["hms"] + "</span></td>");
                            }
                            else
                            { sbContent.Append("<td><span class='control-text'> - </span></td>"); }
                        }
                        sbContent.Append("</tr>");
                    }
                    sbContent.Append("</tbody>");
                    sbContent.Append("</table>");
                    sbContent.Append("</div>");
                    //sbContent.Append("</div>");
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }

        public ActionResult getProjectsByUserID(int userid)
        {

            ProjectModel obj = new ProjectModel();
            obj.ActiveProjectList = obj.Get_ActiveProjectMappedwithUser(userid);
            return Json(obj.ActiveProjectList);
        }
        public ActionResult getUsersbyProjectID(int projectID)
        {

            UserModel obj = new UserModel();
            obj.ActiveMemberList = obj.Get_GetProjectMembersByProjectID(projectID);
            return Json(obj.ActiveMemberList);
        }

        #region Admin
        public ActionResult LoadDashBoardAdminSummary()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                if (Session["DashBoardDetail"] != null)
                {
                    DataSet ds = (DataSet)Session["DashBoardDetail"];
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                    {
                        #region No of Projects
                        DataView view = new DataView(ds.Tables[3]);
                        DataTable distinctValues = view.ToTable(true, "projectID");
                        sbContent.Append("<div class='col-lg-3 col-xs-6'>");
                        sbContent.Append("<div class='small-box bg-aqua'>");
                        sbContent.Append("<div class='inner'>");
                        sbContent.Append("<h3>" + distinctValues.Rows.Count + "</h3>");
                        sbContent.Append("<p>No of projects</p>");
                        sbContent.Append("</div>");
                        sbContent.Append("<div class='icon'>");
                        sbContent.Append("<i class='fa fa-archive iconSize'></i>");
                        sbContent.Append("</div>");
                        sbContent.Append("</div>");
                        sbContent.Append("</div>");
                        #endregion
                        #region No of User
                        distinctValues = view.ToTable(true, "userID");
                        sbContent.Append("<div class='col-lg-3 col-xs-6'>");
                        sbContent.Append("<div class='small-box bg-green'>");
                        sbContent.Append("<div class='inner'>");
                        sbContent.Append("<h3>" + distinctValues.Rows.Count + "</h3>");
                        sbContent.Append("<p>No of Users</p>");
                        sbContent.Append("</div>");
                        sbContent.Append("<div class='icon'>");
                        sbContent.Append("<i class='fa fa-group iconSize'></i>");
                        sbContent.Append("</div>");
                        sbContent.Append("</div>");
                        sbContent.Append("</div>");
                        #endregion
                        #region No of Utilize hr
                        object sumObject;
                        sumObject = ds.Tables[3].Compute("Sum(totalSec)", string.Empty);
                        TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(sumObject));
                        string answer = string.Format("{0:D2}h:{1:D2}m",//:{2:D2}s:{3:D3}ms
                                        (t.Days * 24) + t.Hours,
                                        t.Minutes//,
                                                 //t.Seconds
                                        );//t.Milliseconds
                        sbContent.Append("<div class='col-lg-3 col-xs-6'>");
                        sbContent.Append("<div class='small-box bg-yellow'>");
                        sbContent.Append("<div class='inner'>");
                        sbContent.Append("<h3>" + answer + "</h3>");
                        sbContent.Append("<p>Total Utilized Hour</p>");
                        sbContent.Append("</div>");
                        sbContent.Append("<div class='icon'>");
                        sbContent.Append("<i class='fa fa-clock-o iconSize'></i>");
                        sbContent.Append("</div>");
                        sbContent.Append("</div>");
                        sbContent.Append("</div>");
                        #endregion
                        #region Utilize %
                        distinctValues = view.ToTable(true, "Date");

                        sbContent.Append("<div class='col-lg-3 col-xs-6'>");
                        sbContent.Append("<div class='small-box bg-red'>");
                        sbContent.Append("<div class='inner'>");
                        sbContent.Append("<h3>" + (Convert.ToDouble((Convert.ToDouble((t.Days * 24) + t.Hours) / Convert.ToDouble(distinctValues.Rows.Count * 8)) * 100) / (view.ToTable(true, "userID").Rows.Count)).ToString("0.##") + " % " + "</h3>");
                        sbContent.Append("<p>Total Utilization</p>");
                        sbContent.Append("</div>");
                        sbContent.Append("<div class='icon'>");
                        sbContent.Append("<i class='fa fa-area-chart iconSize'></i>");
                        sbContent.Append("</div>");
                        sbContent.Append("</div>");
                        sbContent.Append("</div>");
                        #endregion
                    }
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }
        public JsonResult GetDonutChartUserWiseAdmin()
        {
            Chart _chart = new Chart();
            try
            {
                if (Session["OrgAdmin"] != null)
                {
                    DataSet ds = (DataSet)Session["DashBoardDetail"];
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                    {
                        DataTable uniqueColsProj = ds.Tables[3].DefaultView.ToTable(true, "UserName");
                        string[] arrrayProj = uniqueColsProj.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                        string[] arrData = new string[arrrayProj.Length];
                        for (int i = 0; i < arrrayProj.Length; i++)
                        {
                            try
                            {
                                DataTable dtFilter = ds.Tables[3].Select("UserName = '" + arrrayProj[i] + "'").CopyToDataTable();
                                object sumObject;
                                sumObject = dtFilter.Compute("Sum(totalSec)", string.Empty);
                                int totalSec = Convert.ToInt32(sumObject);
                                //arrData[i] = (totalSec / 60).ToString() + "." + (totalSec % 60).ToString();
                                arrData[i] = (totalSec / 3600).ToString() + "." + ((totalSec % 3600) / 60).ToString();
                            }
                            catch (Exception exo) { arrData[i] = "0"; }
                        }
                        _chart.labels = arrrayProj;// new string[] { "January", "February", "March", "April", "May", "June", "July" };
                        _chart.datasets = new List<Datasets>();
                        List<Datasets> _dataSet = new List<Datasets>();
                        List<string> arrbg1 = new List<string>();
                        var random = new Random();
                        for (int iColor = 0; iColor < arrrayProj.Length; iColor++)
                        {
                            var color = String.Format("#{0:X6}", random.Next(0x1000000));
                            arrbg1.Add(color);
                        }
                        _dataSet.Add(new Datasets()
                        {
                            label = "",
                            data = arrData,// new string[] { "28", "48", "40", "19", " 86", "27", "90" },
                            backgroundColor = arrbg1.ToArray(),// new string[] { "#f39c12", "#00c0ef", "#0073b7", "#3c8dbc", "#00a65a", "#001f3f", "#39cccc", "#3d9970", "#01ff70", "#ff851b", "#f012be", "#605ca8", "#d81b60", "#020219", "#07074c", "#0f0f99", "#1616e5", "#4646ff", "#8c8cff", "#d1d1ff", "#a3a3ff", "#babaff", "#d1d1ff", "#e8e8ff", "#1A5276", "#27AE60" },
                            borderColor = new string[] { "#020219", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" },
                            borderWidth = "1"
                        });
                        _chart.datasets = _dataSet;
                    }
                }
            }
            catch (Exception exx) { }
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDonutChartProjectWiseAdmin()
        {
            Chart _chart = new Chart();
            try
            {
                if (Session["OrgAdmin"] != null)
                {
                    DataSet ds = (DataSet)Session["DashBoardDetail"];
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                    {
                        DataTable uniqueColsProj = ds.Tables[3].DefaultView.ToTable(true, "projectName");
                        string[] arrrayProj = uniqueColsProj.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                        string[] arrData = new string[arrrayProj.Length];
                        for (int i = 0; i < arrrayProj.Length; i++)
                        {
                            try
                            {
                                DataTable dtFilter = ds.Tables[3].Select("projectName = '" + arrrayProj[i] + "'").CopyToDataTable();
                                object sumObject;
                                sumObject = dtFilter.Compute("Sum(totalSec)", string.Empty);
                                int totalSec = Convert.ToInt32(sumObject);
                                //arrData[i] = (totalSec / 60).ToString() + "." + (totalSec % 60).ToString();
                                arrData[i] = (totalSec / 3600).ToString() + "." + ((totalSec % 3600) / 60).ToString();
                            }
                            catch (Exception exo) { arrData[i] = "0"; }
                        }
                        _chart.labels = arrrayProj;// new string[] { "January", "February", "March", "April", "May", "June", "July" };
                        _chart.datasets = new List<Datasets>();
                        List<Datasets> _dataSet = new List<Datasets>();
                        List<string> arrbg1 = new List<string>();
                        var random = new Random();
                        for (int iColor = 0; iColor < arrrayProj.Length; iColor++)
                        {
                            var color = String.Format("#{0:X6}", random.Next(0x1000000));
                            arrbg1.Add(color);
                        }
                        _dataSet.Add(new Datasets()
                        {
                            label = "",
                            data = arrData,// new string[] { "28", "48", "40", "19", " 86", "27", "90" },
                            backgroundColor = arrbg1.ToArray(),// new string[] { "#f39c12", "#00c0ef", "#0073b7", "#3c8dbc", "#00a65a", "#001f3f", "#39cccc", "#3d9970", "#01ff70", "#ff851b", "#f012be", "#605ca8", "#d81b60", "#020219", "#07074c", "#0f0f99", "#1616e5", "#4646ff", "#8c8cff", "#d1d1ff", "#a3a3ff", "#babaff", "#d1d1ff", "#e8e8ff", "#1A5276", "#27AE60" },
                            borderColor = new string[] { "#020219", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" },
                            borderWidth = "1"
                        });
                        _chart.datasets = _dataSet;
                    }
                }
            }
            catch (Exception exx) { }
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BarChartDataAdmin()
        {
            Chart _chart = new Chart();
            try
            {
                if (Session["OrgAdmin"] != null)
                {
                    DataSet ds = (DataSet)Session["DashBoardDetail"];
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                    {
                        DataTable uniqueColsProj = ds.Tables[3].DefaultView.ToTable(true, "UserName");
                        string[] arrrayProj = uniqueColsProj.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                        DataTable uniqueColsDate = ds.Tables[3].DefaultView.ToTable(true, "Date");
                        string[] arrrayDate = uniqueColsDate.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                        _chart.labels = arrrayDate;
                        _chart.datasets = new List<Datasets>();
                        List<Datasets> _dataSet = new List<Datasets>();
                        List<string> arrbg1 = new List<string>();
                        var random = new Random();
                        for (int iColor = 0; iColor < ((arrrayDate.Length > arrrayProj.Length) ? arrrayDate.Length : arrrayProj.Length); iColor++)
                        {
                            var color = String.Format("#{0:X6}", random.Next(0x1000000));
                            arrbg1.Add(color);
                        }
                        string[] arrbg = arrbg1.ToArray();// new string[] { "#f39c12", "#00c0ef", "#0073b7", "#3c8dbc", "#00a65a", "#001f3f", "#39cccc", "#3d9970", "#01ff70", "#ff851b", "#f012be", "#605ca8", "#d81b60", "#020219", "#07074c", "#0f0f99", "#1616e5", "#4646ff", "#8c8cff", "#d1d1ff", "#a3a3ff", "#babaff", "#d1d1ff", "#e8e8ff", "#E6FCDD", "#EFF7B5", "#EFB5F7", "#00c0ef", "#0073b7", "#3c8dbc", "#00a65a", "#001f3f" };
                        int intColor = 0;
                        foreach (string stproj in arrrayProj)
                        {
                            Datasets dss = new Datasets();
                            dss.label = stproj;
                            string[] arrData = new string[arrrayDate.Length];
                            int i = 0;
                            foreach (string stDate in arrrayDate)
                            {
                                try
                                {
                                    arrData[i] = "0";
                                    //DataRow[] result = ds.Tables[0].Select("UserName = '" + stproj + "' AND Date = '" + stDate + "'");
                                    DataTable dtFilter = ds.Tables[3].Select("UserName = '" + stproj + "' AND Date = '" + stDate + "'").CopyToDataTable();
                                    object sumObject;
                                    sumObject = dtFilter.Compute("Sum(totalSec)", string.Empty);
                                    int totalSec = Convert.ToInt32(sumObject);
                                    arrData[i] = (totalSec / 3600).ToString() + "." + ((totalSec % 3600) / 60).ToString();
                                    //arrData[i] = (totalSec / 60).ToString() + "." + (totalSec % 60).ToString();
                                    //if (result.Length > 0)
                                    //{
                                    //    int totalSec = Convert.ToInt32(result[0]["totalSec"]);
                                    //    arrData[i] = (totalSec / 60).ToString() + "." + (totalSec % 60).ToString();
                                    //}
                                    //else
                                    //{ arrData[i] = "0"; }
                                }
                                catch (Exception exo) { arrData[i] = "0"; }
                                i++;
                            }
                            dss.data = arrData;
                            //"#" + ((1 << 24) * Math.random() | 0).toString(16)
                            dss.backgroundColor = GetBackColor(arrbg, intColor, ((arrrayDate.Length > arrrayProj.Length) ? arrrayDate.Length : arrrayProj.Length));// new string[] { arrbg[intColor] };// new string[] { "#" + ((1 << 24) * new Random().Next() | 0).ToString("16") };
                            dss.borderColor = new string[] { "#020219", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" };
                            //dss.backgroundColor = new string[] { "#FF0000", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" };
                            //dss.borderColor = new string[] { "#FF0000", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" };
                            dss.borderWidth = "1";
                            _dataSet.Add(dss);
                            intColor++;
                        }
                        _chart.datasets = _dataSet;
                    }
                }
            }
            catch (Exception exx) { }
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PM
        public ActionResult LoadDashBoardPMSummary()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                if (Session["DashBoardDetail"] != null)
                {
                    DataSet ds = (DataSet)Session["DashBoardDetail"];
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    {
                        #region No of Projects
                        DataView view = new DataView(ds.Tables[2]);
                        DataTable distinctValues = view.ToTable(true, "projectID");
                        sbContent.Append("<div class='col-lg-3 col-xs-6'>");
                        sbContent.Append("<div class='small-box bg-aqua'>");
                        sbContent.Append("<div class='inner'>");
                        sbContent.Append("<h3>" + distinctValues.Rows.Count + "</h3>");
                        sbContent.Append("<p>No of projects</p>");
                        sbContent.Append("</div>");
                        sbContent.Append("<div class='icon'>");
                        sbContent.Append("<i class='fa fa-archive iconSize'></i>");
                        sbContent.Append("</div>");
                        sbContent.Append("</div>");
                        sbContent.Append("</div>");
                        #endregion
                        #region No of User
                        distinctValues = view.ToTable(true, "userID");
                        sbContent.Append("<div class='col-lg-3 col-xs-6'>");
                        sbContent.Append("<div class='small-box bg-green'>");
                        sbContent.Append("<div class='inner'>");
                        sbContent.Append("<h3>" + distinctValues.Rows.Count + "</h3>");
                        sbContent.Append("<p>No of Users</p>");
                        sbContent.Append("</div>");
                        sbContent.Append("<div class='icon'>");
                        sbContent.Append("<i class='fa fa-group iconSize'></i>");
                        sbContent.Append("</div>");
                        sbContent.Append("</div>");
                        sbContent.Append("</div>");
                        #endregion
                        #region No of Utilize hr
                        object sumObject;
                        sumObject = ds.Tables[2].Compute("Sum(totalSec)", string.Empty);
                        TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(sumObject));
                        string answer = string.Format("{0:D2}h:{1:D2}m",//:{2:D2}s:{3:D3}ms
                                        (t.Days * 24) + t.Hours,
                                        t.Minutes//,
                                                 //t.Seconds
                                        );//t.Milliseconds
                        sbContent.Append("<div class='col-lg-3 col-xs-6'>");
                        sbContent.Append("<div class='small-box bg-yellow'>");
                        sbContent.Append("<div class='inner'>");
                        sbContent.Append("<h3>" + answer + "</h3>");
                        sbContent.Append("<p>Total Utilized Hour</p>");
                        sbContent.Append("</div>");
                        sbContent.Append("<div class='icon'>");
                        sbContent.Append("<i class='fa fa-clock-o iconSize'></i>");
                        sbContent.Append("</div>");
                        sbContent.Append("</div>");
                        sbContent.Append("</div>");
                        #endregion
                        #region Utilize %
                        distinctValues = view.ToTable(true, "Date");
                        sbContent.Append("<div class='col-lg-3 col-xs-6'>");
                        sbContent.Append("<div class='small-box bg-red'>");
                        sbContent.Append("<div class='inner'>");
                        sbContent.Append("<h3>" + (Convert.ToDouble((Convert.ToDouble(sumObject) / Convert.ToDouble(distinctValues.Rows.Count * 8 * 60 * 60)) * 100) / (view.ToTable(true, "userID").Rows.Count)).ToString("0.##") + " % " + "</h3>");
                        sbContent.Append("<p>Total Utilization</p>");
                        sbContent.Append("</div>");
                        sbContent.Append("<div class='icon'>");
                        sbContent.Append("<i class='fa fa-area-chart iconSize'></i>");
                        sbContent.Append("</div>");
                        sbContent.Append("</div>");
                        sbContent.Append("</div>");
                        #endregion
                    }
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }
        public JsonResult GetDonutChartUserWisePM()
        {
            Chart _chart = new Chart();
            try
            {
                if (Session["OrgPM"] != null)
                {
                    DataSet ds = (DataSet)Session["DashBoardDetail"];
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    {
                        DataTable uniqueColsProj = ds.Tables[2].DefaultView.ToTable(true, "UserName");
                        string[] arrrayProj = uniqueColsProj.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                        string[] arrData = new string[arrrayProj.Length];
                        for (int i = 0; i < arrrayProj.Length; i++)
                        {
                            try
                            {
                                DataTable dtFilter = ds.Tables[2].Select("UserName = '" + arrrayProj[i] + "'").CopyToDataTable();
                                object sumObject;
                                sumObject = dtFilter.Compute("Sum(totalSec)", string.Empty);
                                int totalSec = Convert.ToInt32(sumObject);
                                //arrData[i] = (totalSec / 60).ToString() + "." + (totalSec % 60).ToString();
                                arrData[i] = (totalSec / 3600).ToString() + "." + ((totalSec % 3600) / 60).ToString();
                            }
                            catch (Exception exo) { arrData[i] = "0"; }
                        }
                        _chart.labels = arrrayProj;// new string[] { "January", "February", "March", "April", "May", "June", "July" };
                        _chart.datasets = new List<Datasets>();
                        List<Datasets> _dataSet = new List<Datasets>();
                        List<string> arrbg1 = new List<string>();
                        var random = new Random();
                        for (int iColor = 0; iColor < arrrayProj.Length; iColor++)
                        {
                            var color = String.Format("#{0:X6}", random.Next(0x1000000));
                            arrbg1.Add(color);
                        }
                        _dataSet.Add(new Datasets()
                        {
                            label = "",
                            data = arrData,// new string[] { "28", "48", "40", "19", " 86", "27", "90" },
                            backgroundColor = arrbg1.ToArray(),// new string[] { "#f39c12", "#00c0ef", "#0073b7", "#3c8dbc", "#00a65a", "#001f3f", "#39cccc", "#3d9970", "#01ff70", "#ff851b", "#f012be", "#605ca8", "#d81b60", "#020219", "#07074c", "#0f0f99", "#1616e5", "#4646ff", "#8c8cff", "#d1d1ff", "#a3a3ff", "#babaff", "#d1d1ff", "#e8e8ff", "#1A5276", "#27AE60" },
                            borderColor = new string[] { "#020219", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" },
                            borderWidth = "1"
                        });
                        _chart.datasets = _dataSet;
                    }
                }
            }
            catch (Exception exx) { }
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDonutChartProjectWisePM()
        {
            Chart _chart = new Chart();
            try
            {
                if (Session["OrgPM"] != null)
                {
                    DataSet ds = (DataSet)Session["DashBoardDetail"];
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    {
                        DataTable uniqueColsProj = ds.Tables[2].DefaultView.ToTable(true, "projectName");
                        string[] arrrayProj = uniqueColsProj.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                        string[] arrData = new string[arrrayProj.Length];
                        for (int i = 0; i < arrrayProj.Length; i++)
                        {
                            try
                            {
                                DataTable dtFilter = ds.Tables[2].Select("projectName = '" + arrrayProj[i] + "'").CopyToDataTable();
                                object sumObject;
                                sumObject = dtFilter.Compute("Sum(totalSec)", string.Empty);
                                int totalSec = Convert.ToInt32(sumObject);
                                //arrData[i] = (totalSec / 60).ToString() + "." + (totalSec % 60).ToString();
                                arrData[i] = (totalSec / 3600).ToString() + "." + ((totalSec % 3600) / 60).ToString();
                            }
                            catch (Exception exo) { arrData[i] = "0"; }
                        }
                        _chart.labels = arrrayProj;// new string[] { "January", "February", "March", "April", "May", "June", "July" };
                        _chart.datasets = new List<Datasets>();
                        List<Datasets> _dataSet = new List<Datasets>();
                        List<string> arrbg1 = new List<string>();
                        var random = new Random();
                        for (int iColor = 0; iColor < arrrayProj.Length; iColor++)
                        {
                            var color = String.Format("#{0:X6}", random.Next(0x1000000));
                            arrbg1.Add(color);
                        }
                        _dataSet.Add(new Datasets()
                        {
                            label = "",
                            data = arrData,// new string[] { "28", "48", "40", "19", " 86", "27", "90" },
                            backgroundColor = arrbg1.ToArray(),// new string[] { "#f39c12", "#00c0ef", "#0073b7", "#3c8dbc", "#00a65a", "#001f3f", "#39cccc", "#3d9970", "#01ff70", "#ff851b", "#f012be", "#605ca8", "#d81b60", "#020219", "#07074c", "#0f0f99", "#1616e5", "#4646ff", "#8c8cff", "#d1d1ff", "#a3a3ff", "#babaff", "#d1d1ff", "#e8e8ff", "#1A5276", "#27AE60" },
                            borderColor = new string[] { "#020219", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" },
                            borderWidth = "1"
                        });
                        _chart.datasets = _dataSet;
                    }
                }
            }
            catch (Exception exx) { }
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BarChartDataPM()
        {
            Chart _chart = new Chart();
            try
            {
                if (Session["OrgPM"] != null)
                {
                    DataSet ds = (DataSet)Session["DashBoardDetail"];
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    {
                        DataTable uniqueColsProj = ds.Tables[2].DefaultView.ToTable(true, "UserName");
                        string[] arrrayProj = uniqueColsProj.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                        DataTable uniqueColsDate = ds.Tables[2].DefaultView.ToTable(true, "Date");
                        string[] arrrayDate = uniqueColsDate.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                        _chart.labels = arrrayDate;
                        _chart.datasets = new List<Datasets>();
                        List<Datasets> _dataSet = new List<Datasets>();
                        List<string> arrbg1 = new List<string>();
                        var random = new Random();
                        for (int iColor = 0; iColor < ((arrrayDate.Length > arrrayProj.Length) ? arrrayDate.Length : arrrayProj.Length); iColor++)
                        {
                            var color = String.Format("#{0:X6}", random.Next(0x1000000));
                            arrbg1.Add(color);
                        }
                        string[] arrbg = arrbg1.ToArray();// new string[] { "#f39c12", "#00c0ef", "#0073b7", "#3c8dbc", "#00a65a", "#001f3f", "#39cccc", "#3d9970", "#01ff70", "#ff851b", "#f012be", "#605ca8", "#d81b60", "#020219", "#07074c", "#0f0f99", "#1616e5", "#4646ff", "#8c8cff", "#d1d1ff", "#a3a3ff", "#babaff", "#d1d1ff", "#e8e8ff", "#E6FCDD", "#EFF7B5", "#EFB5F7" };
                        int intColor = 0;
                        foreach (string stproj in arrrayProj)
                        {
                            Datasets dss = new Datasets();
                            dss.label = stproj;
                            string[] arrData = new string[arrrayDate.Length];
                            int i = 0;
                            foreach (string stDate in arrrayDate)
                            {
                                try
                                {
                                    arrData[i] = "0";
                                    DataTable dtFilter = ds.Tables[2].Select("UserName = '" + stproj + "' AND Date = '" + stDate + "'").CopyToDataTable();
                                    object sumObject;
                                    sumObject = dtFilter.Compute("Sum(totalSec)", string.Empty);
                                    int totalSec = Convert.ToInt32(sumObject);
                                    //arrData[i] = (totalSec / 60).ToString() + "." + (totalSec % 60).ToString();
                                    arrData[i] = (totalSec / 3600).ToString() + "." + ((totalSec % 3600) / 60).ToString();
                                }
                                catch (Exception exo) { arrData[i] = "0"; }
                                i++;
                            }
                            dss.data = arrData;
                            //"#" + ((1 << 24) * Math.random() | 0).toString(16)
                            dss.backgroundColor = GetBackColor(arrbg, intColor, ((arrrayDate.Length > arrrayProj.Length) ? arrrayDate.Length : arrrayProj.Length));// new string[] { arrbg[intColor] };// new string[] { "#" + ((1 << 24) * new Random().Next() | 0).ToString("16") };
                            dss.borderColor = new string[] { "#020219", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" };
                            //dss.backgroundColor = new string[] { "#FF0000", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" };
                            //dss.borderColor = new string[] { "#FF0000", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" };
                            dss.borderWidth = "1";
                            _dataSet.Add(dss);
                            intColor++;
                        }
                        _chart.datasets = _dataSet;
                    }
                }
            }
            catch (Exception exx) { }
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Birthday Anniversary and info
        public ActionResult GetBirthDayDetail()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                if (Session["sessUserAllData"] != null)
                {
                    DataSet ds = (DataSet)Session["sessUserAllData"];
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[4].Rows)
                        { sbContent.Append("<div class='row'><div class='col-md-9'>" + dr["name"] + "</div><div class='col-md-3'>" + dr["birthday"] + "</div></div>"); }
                    }
                    else
                    { sbContent.Append("<span class='progress-description'>No birthday in next 30 days</span>"); }
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }
        public ActionResult GetAnniversaryDayDetail()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                if (Session["sessUserAllData"] != null)
                {
                    DataSet ds = (DataSet)Session["sessUserAllData"];
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[5] != null && ds.Tables[5].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[5].Rows)
                        { sbContent.Append("<div class='row'><div class='col-md-9'>" + dr["name"] + "</div><div class='col-md-3'>" + dr["anniversaryDay"] + "</div></div>"); }
                    }
                    else
                    { sbContent.Append("<span class='progress-description'>No Anniversary in next 30 days</span>"); }
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }
        public ActionResult GetHoliDayDetail()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                if (Session["sessUserAllData"] != null)
                {
                    DataSet ds = (DataSet)Session["sessUserAllData"];
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[6] != null && ds.Tables[6].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[6].Rows)
                        { sbContent.Append("<div class='row'><div class='col-md-9'>" + dr["Description"] + "</div><div class='col-md-3'>" + dr["Holiday"] + "</div></div>"); }
                    }
                    else
                    { sbContent.Append("<span class='progress-description'>No holiday found</span>"); }
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }
        public ActionResult GetNewsDetail()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                if (Session["sessUserAllData"] != null)
                {
                    DataSet ds = (DataSet)Session["sessUserAllData"];
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[7] != null && ds.Tables[7].Rows.Count > 0)
                    {
                        sbContent.Append("<marquee direction='up' onmouseover='this.stop();' onmouseout='this.start();' scrolldelay='150'><ul>");
                        foreach (DataRow dr in ds.Tables[7].Rows)
                        {
                            try
                            {
                                sbContent.Append("<li><b>" + dr["Heading"] + "</b><br>" + dr["Description"] + "</li><br>");
                            }
                            catch (Exception exx) { }
                        }
                        sbContent.Append("</ul></marquee>");
                    }
                    else
                    { sbContent.Append("<span class='progress-description'>No News/Events found</span>"); }
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }
        public ActionResult GetOnlineUserActive(string CurrDate)
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                if (Session["sessUser"] != null)
                {
                    LoginViewModel objLoginViewModel = new LoginViewModel();
                    DateTime dtCurr = Convert.ToDateTime(CurrDate.Replace("_", " "));// DateTime.ParseExact(CurrDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    DataSet ds = objLoginViewModel.GetOnlineUser(Convert.ToInt32(Session["sessUser"]), dtCurr);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            try
                            {
                                if (Convert.ToString(dr["DRank"]).Trim() == "1")
                                {
                                    sbContent.Append("<div class='row'>");
                                    sbContent.Append("<div class='col-md-3'>");
                                    string[] arrName = Convert.ToString(dr["UserName"]).Substring(0, Convert.ToString(dr["UserName"]).IndexOf("(")).Trim().Split(' ');
                                    string strShortName = "UN";
                                    if (arrName.Length >= 2)
                                    {
                                        strShortName = arrName[0].FirstOrDefault().ToString() + "" + arrName[1].FirstOrDefault().ToString();
                                    }
                                    else if (arrName.Length == 1)
                                    {
                                        strShortName = arrName[0].FirstOrDefault().ToString();
                                    }
                                    sbContent.Append("<span class='btn btn-info user-image' style='font-size:15px;border-radius: 50%; color:white;'>" + strShortName + "</span>");
                                    string strTimeElapsed = "";
                                    int intElapsedHr = Convert.ToInt32(dr["TimeLeft"]) / 60;
                                    if (Convert.ToInt32(dr["TimeLeft"]) < (8 * 60))
                                    {
                                        sbContent.Append("<div class='green_icon'></div>");
                                        if (intElapsedHr == 0)
                                        { strTimeElapsed = "<span style='color: #4cd137;'>(for last " + (Convert.ToInt32(dr["TimeLeft"]) % 60) + " minutes)</span>"; }
                                        else
                                        { strTimeElapsed = "<span style='color: #4cd137;'>(for last " + intElapsedHr + " hr/s)</span>"; }
                                    }
                                    else if (Convert.ToInt32(dr["TimeLeft"]) < (24 * 60))
                                    {
                                        sbContent.Append("<div class='orange_icon'></div>");
                                        { strTimeElapsed = "<span style='color: #FFA500;'>(for last " + intElapsedHr + " hrs)</span>"; }
                                    }
                                    else
                                    {
                                        sbContent.Append("<div class='red_icon'></div>");
                                        { strTimeElapsed = "<span style='color: #FF0000;'>(for last " + intElapsedHr + " hrs)</span>"; }
                                    }
                                    sbContent.Append("</div>");
                                    sbContent.Append("<div class='col-md-9'>");
                                    sbContent.Append("" + Convert.ToString(dr["UserName"]) + "<br><b>Working on:</b>" + Convert.ToString(dr["ProjectName"]) + "<br>" + strTimeElapsed + "<br>");//
                                    sbContent.Append("</div>");
                                    sbContent.Append("</div>");
                                }
                            }
                            catch (Exception exx) { }
                        }
                    }
                    else
                    { sbContent.Append("<span class='progress-description'>No News/Events found</span>"); }
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }
        #endregion
        #endregion

        public ActionResult GetAdminProject()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                //LoginViewModel lvm = new LoginViewModel();
                DataSet ds = (DataSet)Session["sessAdminProject"];// lvm.GetDashBoardAdminProjectData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(Session["ORGID"]));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    sbContent.Append("<div class='panel panel-info'>");
                    //sbContent.Append("<div class='panel-heading'>");

                    //sbContent.Append("</div>");
                    sbContent.Append("<div class='panel-body'>");
                    sbContent.Append("<div class='table-responsive'>");
                    sbContent.Append("<table id='tblDashBoardAdminProjectData' class='table table-bordered dataTable no-footer' width='100%'>");
                    sbContent.Append("<thead><tr>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Department</th>");
                    //sbContent.Append("<th class='text-center tblHeaderColor'>ProjectCode</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Project</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Project Type</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Project Cost</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Client</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Project Manager</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Team Size</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Task</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Start Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>End Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Estimated Time</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Eplapsed Time</th>");
                    sbContent.Append("</tr></thead>");
                    sbContent.Append("<tbody id='tbodyDashBoardAdminProjectData'>");

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(dr["DaysLeft"]) > 0)
                            sbContent.Append("<tr class='text-red'>");
                        else if (Convert.ToInt32(dr["DaysLeft"]) > -5)
                            sbContent.Append("<tr class='text-yellow'>");
                        else
                            sbContent.Append("<tr>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["NAME"]) + "</td>");
                        //sbContent.Append("<td>" + Convert.ToString(dr["ProjectCode"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["ProjectName"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["ProjectType"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["ProjectRate"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["ClientName"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["PM"]) + "</td>");
                        sbContent.AppendFormat(@"<td class='text-center'><a href ='javascript:void(0);' onclick=""ShowTeamMemberList('{0}','{2}');""> {1} </a> </td>", Convert.ToString(dr["AssignedUser"]), Convert.ToString(dr["NoOfUser"]), Convert.ToString(dr["ProjectName"]));
                        sbContent.AppendFormat(@"<td class='text-center'><a href ='javascript:void(0);' onclick=""ShowTaskNameListPopup('{0}','{2}');""> {1} </a> </td>", Convert.ToString(dr["TaslList"]), Convert.ToString(dr["NoOfTask"]), Convert.ToString(dr["ProjectName"]));

                        //sbContent.AppendFormat(@"<td class='text-center'><a href ='javascript:void(0);' onclick='ShowTeamMemberList('{0}','{2}');'> {1} </a> </td>", Convert.ToString(dr["AssignedUser"]), Convert.ToString(dr["NoOfUser"]), Convert.ToString(dr["ProjectName"]));
                        //sbContent.AppendFormat(@"<td class='text-center'><a href ='javascript:void(0);' onclick='ShowTaskNameListPopup('{0}','{2}');'> {1} </a> </td>", Convert.ToString(dr["TaslList"]), Convert.ToString(dr["NoOfTask"]), Convert.ToString(dr["ProjectName"]));
                        //sbContent.Append("<td>" +
                        //    "<a href='#' title='Team Member' data-toggle='popover' data-trigger='focus' data-content='" + Convert.ToString(dr["AssignedUser"]).Replace(";","<br>") + "'>" +
                        //    "" + Convert.ToString(dr["NoOfUser"]) + "</a></td>");
                        ////sbContent.Append("<td>" + Convert.ToString(dr["NoOfUser"]) + "</td>");
                        ////sbContent.Append("<td>" + Convert.ToString(dr["NoOfTask"]) + "</td>");
                        //sbContent.Append("<td>" +
                        //    "<a href='#' title='Team Member' data-toggle='popover' data-trigger='focus' data-content='" + Convert.ToString(dr["TaslList"]).Replace(";", "<br>").Replace("|", "\t\t") + "'>" +
                        //    "" + Convert.ToString(dr["NoOfTask"]) + "</a></td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["StartDate"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["EndDate"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["MaxProjectTimeInHours"]) + "</td>");
                        if (Convert.ToDecimal(dr["MaxProjectTimeInHours"]) < Convert.ToDecimal(dr["EplapsedTime"]))
                            sbContent.Append("<td class='control-text text-red'>" + Convert.ToString(dr["EplapsedTime"]) + "</td>");
                        else if ((Convert.ToDecimal(dr["MaxProjectTimeInHours"]) - Convert.ToDecimal(dr["EplapsedTime"])) <= 16)
                            sbContent.Append("<td class='control-text text-yellow'>" + Convert.ToString(dr["EplapsedTime"]) + "</td>");
                        else
                            sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["EplapsedTime"]) + "</td>");
                        sbContent.Append("</tr>");
                    }

                    sbContent.Append("</tbody>");
                    sbContent.Append("</table>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }
        public ActionResult GetAdminTask()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                //LoginViewModel lvm = new LoginViewModel();
                DataSet ds = (DataSet)Session["sessAdminTask"];// lvm.GetDashBoardAdminTaskData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(Session["ORGID"]));

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    sbContent.Append("<div class='panel panel-info'>");
                    sbContent.Append("<div class='panel-heading'>");
                    if (ds != null && ds.Tables.Count > 1 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        sbContent.Append("<div align='right'>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["Total"]) + "</b> Total Task | </span>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["OPEN"]) + "</b> Open Task  | </span>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["CLOSED"]) + "</b> Closed Task  | </span>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["INPROGRESS"]) + "</b> Task In-Progress </span>");
                        sbContent.Append("</div>");
                    }
                    sbContent.Append("</div>");
                    sbContent.Append("<div class='panel-body'>");
                    sbContent.Append("<div class='table-responsive'>");
                    sbContent.Append("<table id='tblDashBoardAdminTaskData' class='table table-bordered dataTable no-footer' width='100%'>");
                    sbContent.Append("<thead><tr>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Project Name</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Task No</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Task Name</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Task Start Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Task End Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Actual Start Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Actual End Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Current Status</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Estimated Time</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Eplapsed Time</th>");
                    sbContent.Append("</tr></thead>");
                    sbContent.Append("<tbody id='tbodyDashBoardSelfTaskData'>");

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sbContent.Append("<tr>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["Name"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["TaskID"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["TaskName"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["TaskStartDate"]) + "</td>");
                        if (Convert.ToInt32(dr["DaysLeft"]) > 0 && Convert.ToString(dr["StatusName"]).ToLower() != "closed")
                            sbContent.Append("<td class='control-text text-red'>" + Convert.ToString(dr["TaskEndDate"]) + "</td>");
                        else if (Convert.ToInt32(dr["DaysLeft"]) > -5 && Convert.ToString(dr["StatusName"]).ToLower() != "closed")
                            sbContent.Append("<td class='control-text text-yellow'>" + Convert.ToString(dr["TaskEndDate"]) + "</td>");
                        else
                            sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["TaskEndDate"]) + "</td>");
                        //sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["TaskEndDate"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["TaskStartDateActual"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["TaskEndDateActual"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["StatusName"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["ExpectedTime"]) + "</td>");
                        //sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["EplapsedTime"]) + "</td>");
                        if (Convert.ToDecimal(dr["ExpectedTime"]) < Convert.ToDecimal(dr["EplapsedTime"]))
                            sbContent.Append("<td class='control-text text-red'>" + Convert.ToString(dr["EplapsedTime"]) + "</td>");
                        else if ((Convert.ToDecimal(dr["ExpectedTime"]) - Convert.ToDecimal(dr["EplapsedTime"])) <= 16 && Convert.ToString(dr["StatusName"]).ToLower() != "closed")
                            sbContent.Append("<td class='control-text text-yellow'>" + Convert.ToString(dr["EplapsedTime"]) + "</td>");
                        else
                            sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["EplapsedTime"]) + "</td>");
                        sbContent.Append("</tr>");
                    }

                    sbContent.Append("</tbody>");
                    sbContent.Append("</table>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }
        public ActionResult GetAdminTicket()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                //LoginViewModel lvm = new LoginViewModel();
                //DataSet ds = lvm.GetDashBoardAdminTicketData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(Session["ORGID"]));
                DataSet ds = (DataSet)Session["sessAdminTicket"];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    sbContent.Append("<div class='panel panel-info'>");
                    sbContent.Append("<div class='panel-heading'>");
                    if (ds != null && ds.Tables.Count > 1 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        sbContent.Append("<div align='right'>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["Total"]) + "</b> Total Ticket | </span>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["OPEN"]) + "</b> Open Ticket  | </span>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["CLOSED"]) + "</b> Closed Ticket  | </span>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["INPROGRESS"]) + "</b> Ticket In-Progress </span>");
                        sbContent.Append("</div>");
                    }
                    sbContent.Append("</div>");
                    sbContent.Append("<div class='panel-body'>");
                    sbContent.Append("<div class='table-responsive'>");
                    sbContent.Append("<table id='tblDashBoardAdminTicketData' class='table table-bordered dataTable no-footer' width='100%'>");
                    sbContent.Append("<thead><tr>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Project Name</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Ticket No</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Ticket Name</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Ticket Type</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Severity</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Ticket Start Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Ticket End Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Actual Start Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Actual End Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Current Status</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Estimated Time</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Eplapsed Time</th>");
                    sbContent.Append("</tr></thead>");
                    sbContent.Append("<tbody id='tbodyDashBoardAdminTicketData'>");

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sbContent.Append("<tr>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["Name"]) + "</td>");
                        sbContent.Append("<td class='control-text'>T" + Convert.ToString(dr["IssueID"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["IssueName"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["TicketType"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["SeverityName"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["IssueStartDate"]) + "</td>");
                        if (Convert.ToInt32(dr["DaysLeft"]) > 0 && Convert.ToString(dr["StatusName"]).ToLower() != "closed")
                            sbContent.Append("<td class='control-text text-red'>" + Convert.ToString(dr["IssueEndDate"]) + "</td>");
                        else if (Convert.ToInt32(dr["DaysLeft"]) > -5 && Convert.ToString(dr["StatusName"]).ToLower() != "closed")
                            sbContent.Append("<td class='control-text text-yellow'>" + Convert.ToString(dr["IssueEndDate"]) + "</td>");
                        else
                            sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["IssueEndDate"]) + "</td>");
                        //sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["IssueEndDate"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["IssueStartDateActual"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["IssueEndDateActual"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["StatusName"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["ExpectedTime"]) + "</td>");
                        //sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["Timespent"]) + "</td>");
                        if (Convert.ToDecimal(dr["ExpectedTime"]) < Convert.ToDecimal(dr["Timespent"]))
                            sbContent.Append("<td class='control-text text-red'>" + Convert.ToString(dr["Timespent"]) + "</td>");
                        else if ((Convert.ToDecimal(dr["ExpectedTime"]) - Convert.ToDecimal(dr["Timespent"])) <= 16 && Convert.ToString(dr["StatusName"]).ToLower() != "closed")
                            sbContent.Append("<td class='control-text text-yellow'>" + Convert.ToString(dr["Timespent"]) + "</td>");
                        else
                            sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["Timespent"]) + "</td>");
                        sbContent.Append("</tr>");
                    }
                    sbContent.Append("</tbody>");
                    sbContent.Append("</table>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }
        public ActionResult GetAdminUtilization()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                //LoginViewModel lvm = new LoginViewModel();
                //DataSet ds = lvm.GetDashBoardAdminTisheetData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(Session["ORGID"]));
                DataSet ds = (DataSet)Session["sessAdminUtilization"];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    sbContent.Append("<div class='panel panel-info'>");
                    sbContent.Append("<div class='panel-heading'>");
                    sbContent.Append("<span class='control-text'><b>Resource Utilization for " + String.Format("{0:MMMM}", DateTime.Now) + ", " + DateTime.Now.Year + "</b> </span>");
                    sbContent.Append("</div>");
                    sbContent.Append("<div class='panel-body'>");
                    sbContent.Append("<div class='table-responsive'>");
                    sbContent.Append("<table id='tblDashBoardAdminTisheetData' class='table table-bordered dataTable no-footer' width='100%'>");
                    sbContent.Append("<thead><tr>");
                    foreach (DataColumn dc in ds.Tables[0].Columns)
                    {
                        sbContent.Append("<th class='text-center tblHeaderColor'>" + Convert.ToString(dc.ColumnName) + "</th>");
                    }
                    //sbContent.Append("<th class='text-center tblHeaderColor'>Eplapsed Time</th>");
                    sbContent.Append("</tr></thead>");
                    sbContent.Append("<tbody id='tbodyDashBoardAdminTisheetData'>");

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Convert.ToDecimal(dr["Utilization"]) > 120)
                            sbContent.Append("<tr class='text-green'>");
                        else if (Convert.ToDecimal(dr["Utilization"]) < 80)
                            sbContent.Append("<tr class='text-red'>");
                        else
                            sbContent.Append("<tr>");
                        for (int iItem = 0; iItem < ds.Tables[0].Columns.Count; iItem++)
                        {
                            sbContent.Append("<td class='control-text'>" + Convert.ToString(dr[iItem]) + "</td>");
                        }
                        sbContent.Append("</tr>");
                    }
                    sbContent.Append("</tbody>");
                    sbContent.Append("</table>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }

        public ActionResult GetPMProject()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                //LoginViewModel lvm = new LoginViewModel();
                //DataSet ds = lvm.GetDashBoardPMProjectData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(Session["ORGID"]));
                DataSet ds = (DataSet)Session["sessPMProject"];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    sbContent.Append("<div class='panel panel-info'>");
                    //sbContent.Append("<div class='panel-heading'>");

                    //sbContent.Append("</div>");
                    sbContent.Append("<div class='panel-body'>");
                    sbContent.Append("<div class='table-responsive'>");
                    sbContent.Append("<table id='tblDashBoardPMProjectData' class='table table-bordered dataTable no-footer' width='100%'>");
                    sbContent.Append("<thead><tr>");
                    //sbContent.Append("<th class='text-center tblHeaderColor'>Department</th>");
                    //sbContent.Append("<th class='text-center tblHeaderColor'>ProjectCode</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Project</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Project Type</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Project Cost</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Client</th>");
                    //sbContent.Append("<th class='text-center tblHeaderColor'>Project Manager</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Team Size</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Task</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Start Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>End Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Estimated Time</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Eplapsed Time</th>");
                    sbContent.Append("</tr></thead>");
                    sbContent.Append("<tbody id='tbodyDashBoardPMProjectData'>");

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(dr["DaysLeft"]) > 0)
                            sbContent.Append("<tr class='text-red'>");
                        else if (Convert.ToInt32(dr["DaysLeft"]) > -5)
                            sbContent.Append("<tr class='text-yellow'>");
                        else
                            sbContent.Append("<tr>");
                        //sbContent.Append("<td>" + Convert.ToString(dr["NAME"]) + "</td>");
                        //sbContent.Append("<td>" + Convert.ToString(dr["ProjectCode"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["ProjectName"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["ProjectType"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["ProjectRate"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["ClientName"]) + "</td>");
                        //sbContent.Append("<td>" + Convert.ToString(dr["PM"]) + "</td>");
                        sbContent.AppendFormat(@"<td class='text-center'><a href ='javascript:void(0);' onclick=""ShowTeamMemberList('{0}','{2}');""> {1} </a> </td>", Convert.ToString(dr["AssignedUser"]), Convert.ToString(dr["NoOfUser"]), Convert.ToString(dr["ProjectName"]));
                        sbContent.AppendFormat(@"<td class='text-center'><a href ='javascript:void(0);' onclick=""ShowTaskNameListPopup('{0}','{2}');""> {1} </a> </td>", Convert.ToString(dr["TaslList"]), Convert.ToString(dr["NoOfTask"]), Convert.ToString(dr["ProjectName"]));
                        //sbContent.Append("<td>" +
                        //    "<a href='#' title='Team Member' data-toggle='popover' data-trigger='focus' data-content='" + Convert.ToString(dr["AssignedUser"]).Replace(";", "<br>") + "'>" +
                        //    "" + Convert.ToString(dr["NoOfUser"]) + "</a></td>");
                        ////sbContent.Append("<td>" + Convert.ToString(dr["NoOfUser"]) + "</td>");
                        ////sbContent.Append("<td>" + Convert.ToString(dr["NoOfTask"]) + "</td>");
                        //sbContent.Append("<td>" +
                        //    "<a href='#' title='Team Member' data-toggle='popover' data-trigger='focus' data-content='" + Convert.ToString(dr["TaslList"]).Replace(";", "<br>").Replace("|", "  ") + "'>" +
                        //    "" + Convert.ToString(dr["NoOfTask"]) + "</a></td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["StartDate"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["EndDate"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["MaxProjectTimeInHours"]) + "</td>");
                        if (Convert.ToDecimal(dr["MaxProjectTimeInHours"]) < Convert.ToDecimal(dr["EplapsedTime"]))
                            sbContent.Append("<td class='control-text text-red'>" + Convert.ToString(dr["EplapsedTime"]) + "</td>");
                        else if ((Convert.ToDecimal(dr["MaxProjectTimeInHours"]) - Convert.ToDecimal(dr["EplapsedTime"])) <= 16)
                            sbContent.Append("<td  class='control-text text-yellow'>" + Convert.ToString(dr["EplapsedTime"]) + "</td>");
                        else
                            sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["EplapsedTime"]) + "</td>");
                        sbContent.Append("</tr>");
                    }

                    sbContent.Append("</tbody>");
                    sbContent.Append("</table>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }
        public ActionResult GetPMTask()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                //LoginViewModel lvm = new LoginViewModel();
                //DataSet ds = lvm.GetDashBoardPMTaskData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(Session["ORGID"]));
                DataSet ds = (DataSet)Session["sessPMTask"];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    sbContent.Append("<div class='panel panel-info'>");
                    sbContent.Append("<div class='panel-heading'>");
                    if (ds != null && ds.Tables.Count > 1 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        sbContent.Append("<div align='right'>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["Total"]) + "</b> Total Task | </span>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["OPEN"]) + "</b> Open Task  | </span>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["CLOSED"]) + "</b> Closed Task  | </span>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["INPROGRESS"]) + "</b> Task In-Progress </span>");
                        sbContent.Append("</div>");
                    }
                    sbContent.Append("</div>");
                    sbContent.Append("<div class='panel-body'>");
                    sbContent.Append("<div class='table-responsive'>");
                    sbContent.Append("<table id='tblDashBoardPMTaskData' class='table table-bordered dataTable no-footer' width='100%'>");
                    sbContent.Append("<thead><tr>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Project Name</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Task No</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Task Name</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Task Start Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Task End Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Actual Start Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Actual End Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Current Status</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Estimated Time</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Eplapsed Time</th>");
                    sbContent.Append("</tr></thead>");
                    sbContent.Append("<tbody id='tbodyDashBoardPMTaskData'>");

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sbContent.Append("<tr>");
                        sbContent.Append("<td class='text-center'>" + Convert.ToString(dr["Name"]) + "</td>");
                        sbContent.Append("<td class='text-center'>" + Convert.ToString(dr["TaskID"]) + "</td>");
                        sbContent.Append("<td class='text-center'>" + Convert.ToString(dr["TaskName"]) + "</td>");
                        sbContent.Append("<td class='text-center'>" + Convert.ToString(dr["TaskStartDate"]) + "</td>");
                        if (Convert.ToInt32(dr["DaysLeft"]) > 0 && Convert.ToString(dr["StatusName"]).ToLower() != "closed")
                            sbContent.Append("<td class='control-text text-red'>" + Convert.ToString(dr["TaskEndDate"]) + "</td>");
                        else if (Convert.ToInt32(dr["DaysLeft"]) > -5 && Convert.ToString(dr["StatusName"]).ToLower() != "closed")
                            sbContent.Append("<td class='control-text text-yellow'>" + Convert.ToString(dr["TaskEndDate"]) + "</td>");
                        else
                            sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["TaskEndDate"]) + "</td>");
                        //sbContent.Append("<td class='text-center'>" + Convert.ToString(dr["TaskEndDate"]) + "</td>");
                        sbContent.Append("<td class='text-center'>" + Convert.ToString(dr["TaskStartDateActual"]) + "</td>");
                        sbContent.Append("<td class='text-center'>" + Convert.ToString(dr["TaskEndDateActual"]) + "</td>");
                        sbContent.Append("<td class='text-center'>" + Convert.ToString(dr["StatusName"]) + "</td>");
                        sbContent.Append("<td class='text-center'>" + Convert.ToString(dr["ExpectedTime"]) + "</td>");
                        //sbContent.Append("<td class='text-center'>" + Convert.ToString(dr["EplapsedTime"]) + "</td>");
                        if (Convert.ToDecimal(dr["ExpectedTime"]) < Convert.ToDecimal(dr["EplapsedTime"]))
                            sbContent.Append("<td class='control-text text-red'>" + Convert.ToString(dr["EplapsedTime"]) + "</td>");
                        else if ((Convert.ToDecimal(dr["ExpectedTime"]) - Convert.ToDecimal(dr["EplapsedTime"])) <= 16 && Convert.ToString(dr["StatusName"]).ToLower() != "closed")
                            sbContent.Append("<td class='control-text text-yellow'>" + Convert.ToString(dr["EplapsedTime"]) + "</td>");
                        else
                            sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["EplapsedTime"]) + "</td>");
                        sbContent.Append("</tr>");
                    }

                    sbContent.Append("</tbody>");
                    sbContent.Append("</table>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }
        public ActionResult GetPMTicket()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                //LoginViewModel lvm = new LoginViewModel();
                //DataSet ds = lvm.GetDashBoardPMTicketData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(Session["ORGID"]));
                DataSet ds = (DataSet)Session["sessPMTicket"];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    sbContent.Append("<div class='panel panel-info'>");
                    sbContent.Append("<div class='panel-heading'>");
                    if (ds != null && ds.Tables.Count > 1 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        sbContent.Append("<div align='right'>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["Total"]) + "</b> Total Ticket | </span>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["OPEN"]) + "</b> Open Ticket  | </span>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["CLOSED"]) + "</b> Closed Ticket  | </span>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["INPROGRESS"]) + "</b> Ticket In-Progress </span>");
                        sbContent.Append("</div>");
                    }
                    sbContent.Append("</div>");
                    sbContent.Append("<div class='panel-body'>");
                    sbContent.Append("<div class='table-responsive'>");
                    sbContent.Append("<table id='tblDashBoardPMTicketData' class='table table-bordered dataTable no-footer' width='100%'>");
                    sbContent.Append("<thead><tr>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Project Name</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Ticket No</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Ticket Name</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Ticket Type</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Severity</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Ticket Start Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Ticket End Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Actual Start Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Actual End Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Current Status</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Estimated Time</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Eplapsed Time</th>");
                    sbContent.Append("</tr></thead>");
                    sbContent.Append("<tbody id='tbodyDashBoardPMTicketData'>");

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sbContent.Append("<tr>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["Name"]) + "</td>");
                        sbContent.Append("<td class='control-text'>T" + Convert.ToString(dr["IssueID"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["IssueName"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["TicketType"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["SeverityName"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["IssueStartDate"]) + "</td>");
                        if (Convert.ToInt32(dr["DaysLeft"]) > 0 && Convert.ToString(dr["StatusName"]).ToLower() != "closed")
                            sbContent.Append("<td class='control-text text-red'>" + Convert.ToString(dr["IssueEndDate"]) + "</td>");
                        else if (Convert.ToInt32(dr["DaysLeft"]) > -5 && Convert.ToString(dr["StatusName"]).ToLower() != "closed")
                            sbContent.Append("<td class='control-text text-yellow'>" + Convert.ToString(dr["IssueEndDate"]) + "</td>");
                        else
                            sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["IssueEndDate"]) + "</td>");
                        //sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["IssueEndDate"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["IssueStartDateActual"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["IssueEndDateActual"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["StatusName"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["ExpectedTime"]) + "</td>");
                        //sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["Timespent"]) + "</td>");
                        if (Convert.ToDecimal(dr["ExpectedTime"]) < Convert.ToDecimal(dr["Timespent"]))
                            sbContent.Append("<td class='control-text text-red'>" + Convert.ToString(dr["Timespent"]) + "</td>");
                        else if ((Convert.ToDecimal(dr["ExpectedTime"]) - Convert.ToDecimal(dr["Timespent"])) <= 16 && Convert.ToString(dr["StatusName"]).ToLower() != "closed")
                            sbContent.Append("<td class='control-text text-yellow'>" + Convert.ToString(dr["Timespent"]) + "</td>");
                        else
                            sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["Timespent"]) + "</td>");
                        sbContent.Append("</tr>");
                    }
                    sbContent.Append("</tbody>");
                    sbContent.Append("</table>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }
        public ActionResult GetPMUtilization()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                //LoginViewModel lvm = new LoginViewModel();
                //DataSet ds = lvm.GetDashBoardPMTisheetData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(Session["ORGID"]));
                DataSet ds = (DataSet)Session["sessPMUtilization"];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    sbContent.Append("<div class='panel panel-info'>");
                    sbContent.Append("<div class='panel-heading'>");
                    sbContent.Append("<span class='control-text'><b>Resource Utilization for " + String.Format("{0:MMMM}", DateTime.Now) + ", " + DateTime.Now.Year + "</b> </span>");
                    sbContent.Append("</div>");
                    sbContent.Append("<div class='panel-body'>");
                    sbContent.Append("<div class='table-responsive'>");
                    sbContent.Append("<table id='tblDashBoardPMTisheetData' class='table table-bordered dataTable no-footer' width='100%'>");
                    sbContent.Append("<thead><tr>");
                    foreach (DataColumn dc in ds.Tables[0].Columns)
                    {
                        sbContent.Append("<th class='text-center tblHeaderColor'>" + Convert.ToString(dc.ColumnName) + "</th>");
                    }
                    //sbContent.Append("<th class='text-center tblHeaderColor'>Eplapsed Time</th>");
                    sbContent.Append("</tr></thead>");
                    sbContent.Append("<tbody id='tbodyDashBoardPMTisheetData'>");

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Convert.ToDecimal(dr["Utilization"]) > 120)
                            sbContent.Append("<tr class='text-green'>");
                        else if (Convert.ToDecimal(dr["Utilization"]) < 80)
                            sbContent.Append("<tr class='text-red'>");
                        else
                            sbContent.Append("<tr>");
                        for (int iItem = 0; iItem < ds.Tables[0].Columns.Count; iItem++)
                        {
                            sbContent.Append("<td class='control-text'>" + Convert.ToString(dr[iItem]) + "</td>");
                        }
                        sbContent.Append("</tr>");
                    }
                    sbContent.Append("</tbody>");
                    sbContent.Append("</table>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }

        public ActionResult GetSelfTask()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                //LoginViewModel lvm = new LoginViewModel();
                //DataSet ds = lvm.GetDashBoardSelfTaskData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(Session["ORGID"]));
                DataSet ds = (DataSet)Session["sessSelfTask"];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    sbContent.Append("<div class='panel panel-info'>");
                    sbContent.Append("<div class='panel-heading'>");
                    if (ds != null && ds.Tables.Count > 1 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        sbContent.Append("<div align='right'>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["Total"]) + "</b> Total Assigned | </span>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["OPEN"]) + "</b> Open Task  | </span>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["CLOSED"]) + "</b> Closed Task  | </span>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["INPROGRESS"]) + "</b> Task In-Progress </span>");
                        sbContent.Append("</div>");
                    }
                    sbContent.Append("</div>");
                    sbContent.Append("<div class='panel-body'>");
                    sbContent.Append("<div class='table-responsive'>");
                    sbContent.Append("<table id='tblDashBoardSelfTaskData' class='table table-bordered dataTable no-footer' width='100%'>");
                    sbContent.Append("<thead><tr>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Project Name</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Task No</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Task Name</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Task Start Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Task End Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Actual Start Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Actual End Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Current Status</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Estimated Time</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Eplapsed Time</th>");
                    sbContent.Append("</tr></thead>");
                    sbContent.Append("<tbody id='tbodyDashBoardSelfTaskData'>");

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sbContent.Append("<tr>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["Name"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["TaskID"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["TaskName"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["TaskStartDate"]) + "</td>");
                        if (Convert.ToInt32(dr["DaysLeft"]) > 0 && Convert.ToString(dr["StatusName"]).ToLower() != "closed")
                            sbContent.Append("<td class='control-text text-red'>" + Convert.ToString(dr["TaskEndDate"]) + "</td>");
                        else if (Convert.ToInt32(dr["DaysLeft"]) > -5 && Convert.ToString(dr["StatusName"]).ToLower() != "closed")
                            sbContent.Append("<td class='control-text text-yellow'>" + Convert.ToString(dr["TaskEndDate"]) + "</td>");
                        else
                            sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["TaskEndDate"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["TaskStartDateActual"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["TaskEndDateActual"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["StatusName"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["ExpectedTime"]) + "</td>");
                        //sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["EplapsedTime"]) + "</td>");
                        if (Convert.ToDecimal(dr["ExpectedTime"]) < Convert.ToDecimal(dr["EplapsedTime"]))
                            sbContent.Append("<td class='control-text text-red'>" + Convert.ToString(dr["EplapsedTime"]) + "</td>");
                        else if ((Convert.ToDecimal(dr["ExpectedTime"]) - Convert.ToDecimal(dr["EplapsedTime"])) <= 16 && Convert.ToString(dr["StatusName"]).ToLower() != "closed")
                            sbContent.Append("<td class='control-text text-yellow'>" + Convert.ToString(dr["EplapsedTime"]) + "</td>");
                        else
                            sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["EplapsedTime"]) + "</td>");
                        sbContent.Append("</tr>");
                    }

                    sbContent.Append("</tbody>");
                    sbContent.Append("</table>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }
        public ActionResult GetSelfTicket()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                //LoginViewModel lvm = new LoginViewModel();
                //DataSet ds = lvm.GetDashBoardSelfTicketData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(Session["ORGID"]));
                DataSet ds = (DataSet)Session["sessSelfTicket"];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    sbContent.Append("<div class='panel panel-info'>");
                    sbContent.Append("<div class='panel-heading'>");
                    if (ds != null && ds.Tables.Count > 1 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        sbContent.Append("<div align='right'>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["Total"]) + "</b> Total Assigned | </span>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["OPEN"]) + "</b> Open Ticket  | </span>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["CLOSED"]) + "</b> Closed Ticket  | </span>");
                        sbContent.Append("<span class='control-text'><b>" + Convert.ToString(ds.Tables[1].Rows[0]["INPROGRESS"]) + "</b> Ticket In-Progress </span>");
                        sbContent.Append("</div>");
                    }
                    sbContent.Append("</div>");
                    sbContent.Append("<div class='panel-body'>");
                    sbContent.Append("<div class='table-responsive'>");
                    sbContent.Append("<table id='tblDashBoardSelfTicketData' class='table table-bordered dataTable no-footer' width='100%'>");
                    sbContent.Append("<thead><tr>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Project Name</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Ticket No</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Ticket Name</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Ticket Type</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Severity</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Ticket Start Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Ticket End Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Actual Start Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Actual End Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Current Status</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Estimated Time</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Eplapsed Time</th>");
                    sbContent.Append("</tr></thead>");
                    sbContent.Append("<tbody id='tbodyDashBoardSelfTicketData'>");

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sbContent.Append("<tr>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["Name"]) + "</td>");
                        sbContent.Append("<td class='control-text'>T" + Convert.ToString(dr["IssueID"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["IssueName"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["TicketType"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["SeverityName"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["IssueStartDate"]) + "</td>");
                        if (Convert.ToInt32(dr["DaysLeft"]) > 0 && Convert.ToString(dr["StatusName"]).ToLower() != "closed")
                            sbContent.Append("<td class='control-text text-red'>" + Convert.ToString(dr["IssueEndDate"]) + "</td>");
                        else if (Convert.ToInt32(dr["DaysLeft"]) > -5 && Convert.ToString(dr["StatusName"]).ToLower() != "closed")
                            sbContent.Append("<td class='control-text text-yellow'>" + Convert.ToString(dr["IssueEndDate"]) + "</td>");
                        else
                            sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["IssueEndDate"]) + "</td>");
                        //sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["IssueEndDate"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["IssueStartDateActual"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["IssueEndDateActual"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["StatusName"]) + "</td>");
                        sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["ExpectedTime"]) + "</td>");
                        //sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["Timespent"]) + "</td>");
                        if (Convert.ToDecimal(dr["ExpectedTime"]) < Convert.ToDecimal(dr["Timespent"]))
                            sbContent.Append("<td class='control-text text-red'>" + Convert.ToString(dr["Timespent"]) + "</td>");
                        else if ((Convert.ToDecimal(dr["ExpectedTime"]) - Convert.ToDecimal(dr["Timespent"])) <= 16 && Convert.ToString(dr["StatusName"]).ToLower() != "closed")
                            sbContent.Append("<td class='control-text text-yellow'>" + Convert.ToString(dr["Timespent"]) + "</td>");
                        else
                            sbContent.Append("<td class='control-text'>" + Convert.ToString(dr["Timespent"]) + "</td>");
                        sbContent.Append("</tr>");
                    }
                    sbContent.Append("</tbody>");
                    sbContent.Append("</table>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }
        public ActionResult GetSelfTimeSheet()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                //LoginViewModel lvm = new LoginViewModel();
                //DataSet ds = lvm.GetDashBoardSelfTisheetData(Convert.ToInt32(Session["sessUser"]), Convert.ToInt32(Session["ORGID"]));
                DataSet ds = (DataSet)Session["sessSelfUtilization"];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    sbContent.Append("<div class='panel panel-info'>");
                    sbContent.Append("<div class='panel-heading'>");
                    sbContent.Append("<span class='control-text'><b> Timesheet for current week</b> </span>");
                    sbContent.Append("</div>");
                    sbContent.Append("<div class='panel-body'>");
                    sbContent.Append("<div class='table-responsive'>");
                    sbContent.Append("<table id='tblDashBoardSelfTisheetData' class='table table-bordered dataTable no-footer' width='100%'>");
                    sbContent.Append("<thead><tr>");
                    foreach (DataColumn dc in ds.Tables[0].Columns)
                    {
                        sbContent.Append("<th class='text-center tblHeaderColor'>" + Convert.ToString(dc.ColumnName) + "</th>");
                    }
                    //sbContent.Append("<th class='text-center tblHeaderColor'>Eplapsed Time</th>");
                    sbContent.Append("</tr></thead>");
                    sbContent.Append("<tbody id='tbodyDashBoardSelfTisheetData'>");

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sbContent.Append("<tr>");
                        for (int iItem = 0; iItem < ds.Tables[0].Columns.Count; iItem++)
                        {
                            sbContent.Append("<td class='control-text'>" + Convert.ToString(dr[iItem]) + "</td>");
                        }
                        sbContent.Append("</tr>");
                    }
                    sbContent.Append("</tbody>");
                    sbContent.Append("</table>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                    sbContent.Append("</div>");
                }
            }
            catch (Exception exx) { }
            return Content(sbContent.ToString());
        }


        public ActionResult DisplayTeamMemberList(string TextToDisplay)
        {
            StringBuilder sbOut = new StringBuilder();
            if (TextToDisplay.Trim() != string.Empty)
            {
                string[] arrNoofRecord = TextToDisplay.Split(';');
                if (arrNoofRecord.Length > 0)
                {
                    foreach (string strRowVal in arrNoofRecord)
                    {
                        sbOut.Append("<tr>");
                        string[] arrNoofColData = strRowVal.Split('|');
                        if (arrNoofColData.Length > 0)
                        {
                            for (int i = 0; i < arrNoofColData.Length; i++)
                            {
                                sbOut.Append("<td class='text-center'>" + arrNoofColData[i] + "</td>");
                            }
                        }
                        sbOut.Append("</tr>");
                    }
                }
                else
                    sbOut.Append("<tr><td class='text-center'></td></tr>");
            }
            else
            { sbOut.Append("<tr><td class='text-center'></td></tr>"); }
            return Json(sbOut.ToString());
        }
        public ActionResult DisplayTaskNameListPopup(string TextToDisplay)
        {
            StringBuilder sbOut = new StringBuilder();
            if (TextToDisplay.Trim() != string.Empty)
            {
                string[] arrNoofRecord = TextToDisplay.Split(';');
                if (arrNoofRecord.Length > 0)
                {
                    foreach (string strRowVal in arrNoofRecord)
                    {
                        sbOut.Append("<tr>");
                        string[] arrNoofColData = strRowVal.Split('|');
                        if (arrNoofColData.Length > 0)
                        {
                            for (int i = 0; i < arrNoofColData.Length; i++)
                            {
                                sbOut.Append("<td class='text-center'>" + arrNoofColData[i] + "</td>");
                            }
                        }
                        sbOut.Append("</tr>");
                    }
                }
                else
                    sbOut.Append("<tr><td class='text-center'></td></tr>");
            }
            else
            { sbOut.Append("<tr><td class='text-center'></td></tr>"); }
            return Json(sbOut.ToString());
        }
        public ActionResult ChartLoadTask(int FilterVal, string strFor)
        {
            List<ChartValue> cv = new List<ChartValue>();
            try
            {
                DataSet ds;
                if(strFor=="A")
                    ds = (DataSet)Session["sessAdminTask"];
                else if (strFor == "P")
                    ds = (DataSet)Session["sessPMTask"];
                else 
                    ds = (DataSet)Session["sessSelfTask"];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (FilterVal == 0)//<option value="0">By Status</option>
                    {
                        DataTable uniqueCols = ds.Tables[0].DefaultView.ToTable(true, "StatusName");
                        string[] arrrayLbl = uniqueCols.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                        foreach (string strLbl in arrrayLbl)
                        {
                            try
                            {
                                DataRow[] result = ds.Tables[0].Select("StatusName = '" + strLbl + "'");
                                cv.Add(new ChartValue { label = strLbl, value = result.Length.ToString() });
                            }
                            catch (Exception exo) { }
                        }
                    }
                    else if (FilterVal == 1)//<option value="1">By project - Open</option>
                    {
                        DataTable uniqueCols = ds.Tables[0].DefaultView.ToTable(true, "Name");
                        string[] arrrayLbl = uniqueCols.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                        foreach (string strLbl in arrrayLbl)
                        {
                            try
                            {
                                DataRow[] result = ds.Tables[0].Select("Name = '" + strLbl + "'");
                                cv.Add(new ChartValue { label = strLbl, value = result.Length.ToString() });
                            }
                            catch (Exception exo) { }
                        }
                    }
                    else if (FilterVal == 2)//<option value="2">Closure Date</option>
                    {
                        try
                        {
                            ds.CaseSensitive = false;
                            DataRow[] result = ds.Tables[0].Select("DaysLeft > 0 AND StatusName <> 'closed' ");
                            cv.Add(new ChartValue { label = "Expired", value = result.Length.ToString() });
                            DataRow[] result1 = ds.Tables[0].Select("DaysLeft > (-5) AND DaysLeft <= 0 AND StatusName <> 'closed' ");
                            cv.Add(new ChartValue { label = "Expire in 5days", value = result1.Length.ToString() });
                            DataRow[] result2 = ds.Tables[0].Select("StatusName <> 'closed' ");
                            cv.Add(new ChartValue { label = "Expire not in 5days", value = (result2.Length - result.Length - result1.Length).ToString() });
                        }
                        catch (Exception exo) { }
                    }
                    else if (FilterVal == 3)//<option value="3">Elapsed Time</option>
                    {
                        try
                        {
                            DataRow[] result = ds.Tables[0].Select("ExpectedTime < EplapsedTime AND StatusName <> 'closed' ");
                            cv.Add(new ChartValue { label = "Overflow", value = result.Length.ToString() });
                            DataRow[] result1 = ds.Tables[0].Select("ExpectedTime > EplapsedTime AND (Convert(ExpectedTime, System.Decimal)-Convert(EplapsedTime, System.Decimal)) <= 16 AND StatusName <> 'closed' ");
                            cv.Add(new ChartValue { label = "About to Overflow", value = result1.Length.ToString() });
                            DataRow[] result2 = ds.Tables[0].Select("StatusName <> 'closed' ");
                            cv.Add(new ChartValue { label = "Underflow", value = (result2.Length - result.Length - result1.Length).ToString() });
                        }
                        catch (Exception exo) { }
                    }
                }
            }
            catch (Exception) { }
            return Json(cv);
        }
    }
    public class ChartValue
    {
        public string label { get; set; }
        public string value { get; set; }
    }
}