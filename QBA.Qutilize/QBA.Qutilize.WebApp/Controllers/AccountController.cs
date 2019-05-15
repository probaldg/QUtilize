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
using System.Text;

namespace QBA.Qutilize.WebApp.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult LoadSideMenu()
        {
            AccountViewModels obj = new Models.AccountViewModels();
            StringBuilder strMenu = new StringBuilder();
            DataTable dt = obj.GetDashBoardMenu(Convert.ToInt32(Session["sessUser"]));
            if (dt != null && dt.Rows.Count > 0)
            {
                try
                {
                    strMenu.Append(GetSideMenuContent(dt, ""));
                }
                catch (Exception exx) { }
            }
            return Content(strMenu.ToString());
        }
        public string GetSideMenuContent(DataTable orgDT, string parentID)
        {
            StringBuilder strMenu = new StringBuilder();
            if (parentID.Trim() == "")
            {
                DataRow[] drParentAll = orgDT.Select("lvl = 0");
                if (drParentAll.Length > 0)
                {
                    foreach (DataRow dr in drParentAll)
                    {
                        strMenu.Append(GetSideMenuContent(orgDT, Convert.ToString(dr["ID"])));
                    }
                }
            }
            else
            {
                DataRow[] drParent = orgDT.Select("ID = " + parentID);
                if (drParent.Length > 0)
                {
                    strMenu.Append("<div class='row lien'>");
                    if (Convert.ToInt32(drParent[0]["lvl"]) == 0)
                        strMenu.Append("<div class='col-md-12'>");
                    else
                    {
                        int lvl = Convert.ToInt32(drParent[0]["lvl"]);
                        strMenu.Append("<div class='col-md-12' style='padding-left: " + (lvl * 35) + "px;'>");
                    }
                    strMenu.Append("<a href='" + drParent[0]["URL"] + "' class='fa " + drParent[0]["DisplayCSS"].ToString() + "' style='color: white;'></a> &nbsp;");
                    strMenu.Append(" <a href = '" + ConfigurationSettings.AppSettings["SiteAddress"] + drParent[0]["URL"] + "' style='color: white;'> " + drParent[0]["DisplayName"].ToString() + " </a> ");
                    strMenu.Append("<hr>");
                    strMenu.Append("</div>");
                    strMenu.Append("</div>");
                }
                DataRow[] dtChild = orgDT.Select("ParentID = " + parentID);
                foreach (DataRow dr in dtChild)
                {
                    strMenu.Append(GetSideMenuContent(orgDT, Convert.ToString(dr["ID"])));
                }
            }
            return strMenu.ToString();
        }

        public ActionResult MyAccount()
        {

            if (System.Web.HttpContext.Current.Session["sessUser"] != null)
            {
                UserModel user = new UserModel();
                try
                {
                    if (Session["sessUserAllData"] != null)
                    {
                        DataSet dsSess = (DataSet)Session["sessUserAllData"];
                        if (dsSess != null && dsSess.Tables.Count > 0)
                        {
                            if (dsSess.Tables[0] != null && dsSess.Tables[0].Rows.Count > 0)
                            {
                                user.ID = int.Parse(dsSess.Tables[0].Rows[0]["Id"].ToString());
                                user.UserName = dsSess.Tables[0].Rows[0]["UserName"]?.ToString();
                                user.Name = dsSess.Tables[0].Rows[0]["Name"]?.ToString();
                                int intMGRID = 0;
                                int.TryParse(dsSess.Tables[0].Rows[0]["managerID"]?.ToString(),out intMGRID);
                                user.ManagerId = intMGRID;
                                user.ManagerName = dsSess.Tables[0].Rows[0]["ManagerName"]?.ToString();
                                user.ManagerEmpCode = dsSess.Tables[0].Rows[0]["ManagerEmpCode"]?.ToString();
                                user.EmailId = dsSess.Tables[0].Rows[0]["EmailId"]?.ToString();
                                user.EmployeeCode = dsSess.Tables[0].Rows[0]["EmployeeCode"]?.ToString();
                                user.Designation = dsSess.Tables[0].Rows[0]["Designation"]?.ToString();
                                user.ContactNo = dsSess.Tables[0].Rows[0]["PhoneNo"].ToString();
                                user.AlterNetContactNo = dsSess.Tables[0].Rows[0]["AlternateConatctNo"]?.ToString();
                                if (dsSess.Tables[0].Rows[0]["BirthDate"] != DBNull.Value)
                                {
                                    user.BirthDate = Convert.ToDateTime(dsSess.Tables[0].Rows[0]["BirthDate"]).Date;
                                }
                                else
                                    user.BirthDate = null;
                                user.Gender = dsSess.Tables[0].Rows[0]["Gender"]?.ToString();
                            }
                            if (dsSess.Tables[1] != null && dsSess.Tables[1].Rows.Count > 0)
                            {
                                foreach (DataRow drR in dsSess.Tables[1].Rows)
                                {
                                    user.RoleName += "<li>"+Convert.ToString(drR["RoleName"])+"</li>";
                                }
                            }
                            else
                                user.RoleName = string.Empty;
                            if (dsSess.Tables[2] != null && dsSess.Tables[2].Rows.Count > 0)
                            {
                                user.OrganisationName = Convert.ToString(dsSess.Tables[2].Rows[0]["OrganisationName"]);
                                foreach (DataRow drR in dsSess.Tables[2].Rows)
                                {
                                    user.DepartmentName += "<li>" + Convert.ToString(drR["DepartmentName"]) + "</li>";
                                }
                            }
                            else
                            { user.OrganisationName = string.Empty; user.DepartmentName = string.Empty; }
                            if (dsSess.Tables[3] != null && dsSess.Tables[3].Rows.Count > 0)
                            {
                                foreach (DataRow drR in dsSess.Tables[3].Rows)
                                {
                                    user.ProjectName += "<li>" + Convert.ToString(drR["ProjectName"]) + "</li>";
                                }
                            }
                            else
                            { user.ProjectName = string.Empty;  }
                        }
                    }
                    //DataTable dt = user.GetMyAccountData(int.Parse(System.Web.HttpContext.Current.Session["sessUser"].ToString()));
                    //if (dt.Rows.Count > 0)
                    //{
                    //    user.ID = int.Parse(dt.Rows[0]["Id"].ToString());
                    //    user.Name = dt.Rows[0]["Name"].ToString();

                    //    user.EmailId = dt.Rows[0]["EmailId"].ToString();


                    //    ViewBag.ID = int.Parse(dt.Rows[0]["ID"].ToString());
                    //    ViewBag.Name = dt.Rows[0]["Name"].ToString();
                    //    ViewBag.Email = dt.Rows[0]["EmailId"].ToString();
                    //    //  ViewBag.OrgName = dt.Rows[0]["orgname"].ToString();
                    //    // ViewBag.Designation = dt.Rows[0]["Designation"].ToString();
                    //}
                    return View(user);
                }
                catch (Exception exx) { throw; }
            }
            else
            {
                return RedirectToAction("Index", "Home");
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
        public ActionResult LoadSessionHistoryLast10()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                LoginViewModel lvm = new LoginViewModel();
                DataSet ds = lvm.getSessionHistory(Convert.ToInt32(Session["sessUser"]),10);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    sbContent.Append("<div class='table-responsive'>");
                    sbContent.Append("<table class='table table-bordered' id='tblSessionHistory'  width='100%'>");
                    sbContent.Append("<thead>");
                    sbContent.Append("<tr>");
                    //sbContent.Append("<th class='text-center tblHeaderColor'>Name</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>IP Address</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Application</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Start Date Time</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>End Date Time</th>");
                    sbContent.Append("</tr>");
                    sbContent.Append("</thead>");
                    sbContent.Append("<tbody id='tbodySessionHistoryData'>");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sbContent.Append("<tr>");
                        //sbContent.Append("<td><span class='control-text'>" + Convert.ToString(dr["userName"]) + "</span></td>");
                        sbContent.Append("<td><span class='control-text'>" + Convert.ToString(dr["IPAddress"]) + "</span></td>");
                        sbContent.Append("<td><span class='control-text'>" + Convert.ToString(dr["Application"]) + "</span></td>");
                        sbContent.Append("<td><span class='control-text'>" + Convert.ToString(dr["StartDateTime"]) + "</span></td>");
                        sbContent.Append("<td><span class='control-text'>" + Convert.ToString(dr["EndDateTime"]) + "</span></td>");
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
        public ActionResult LoadActivityHistoryLast10()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                LoginViewModel lvm = new LoginViewModel();
                DataSet ds = lvm.getActivityHistory(Convert.ToInt32(Session["sessUser"]), 10);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    sbContent.Append("<div class='table-responsive'>");
                    sbContent.Append("<table class='table table-bordered' id='tblSessionHistory'  width='100%'>");
                    sbContent.Append("<thead>");
                    sbContent.Append("<tr>");
                    //sbContent.Append("<th class='text-center tblHeaderColor'>Name</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Access Date</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Device</th>");
                    //sbContent.Append("<th class='text-center tblHeaderColor'>MAC ID</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Browser</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>IP Address</th>");
                    sbContent.Append("<th class='text-center tblHeaderColor'>Url Accessed</th>");
                    sbContent.Append("</tr>");
                    sbContent.Append("</thead>");
                    sbContent.Append("<tbody id='tbodySessionHistoryData'>");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sbContent.Append("<tr>");
                        //sbContent.Append("<td><span class='control-text'>" + Convert.ToString(dr["userName"]) + "</span></td>");
                        sbContent.Append("<td><span class='control-text'>" + Convert.ToString(dr["StartDateTime"]) + "</span></td>");
                        sbContent.Append("<td><span class='control-text'>" + Convert.ToString(dr["IsMobileDevice"]) + "</span></td>");
                        //sbContent.Append("<td><span class='control-text'>" + Convert.ToString(dr["MACAddress"]) + "</span></td>");
                        sbContent.Append("<td><span class='control-text'>" + Convert.ToString(dr["Browser"]) + "</span></td>");
                        sbContent.Append("<td><span class='control-text'>" + Convert.ToString(dr["IPAddress"]) + "</span></td>");
                        sbContent.Append("<td><span class='control-text'>" + Convert.ToString(dr["UrlAccessed"]) + "</span></td>");
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
    }
}