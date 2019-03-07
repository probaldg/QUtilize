using QBA.Qutilize.WebApp.Helper;
using QBA.Qutilize.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
                    return RedirectToAction("DashBoard", new { U = EncryptionHelper.Encryptdata(model.UserID), P = EncryptionHelper.Encryptdata(model.Password) });
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
                    }
                }
                else
                {
                    try
                    {
                        DataSet dsSess = (DataSet)Session["sessUserAllData"];
                        string strUser = EncryptionHelper.Decryptdata(Request.QueryString["U"]);
                        string strPass = EncryptionHelper.Decryptdata(Request.QueryString["P"]);
                        if (strUser.Trim()!=string.Empty &&  Convert.ToString(dsSess.Tables[0].Rows[0]["UserName"]).Trim() != strUser)
                        {
                            //LoginViewModel lvm = new LoginViewModel();
                            //strPass = EncryptionHelper.ConvertStringToMD5(strPass);
                            DataSet ds1 = lvm.VerifyLogin(strUser, strPass);
                            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0] != null && ds1.Tables[0].Rows.Count > 0)
                            {
                                Session.Add("sessUser", Convert.ToString(ds1.Tables[0].Rows[0]["Id"]));
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
                    Session.Add("Name", ds.Tables[0].Rows[0]["Name"]);
                    Session.Add("Email", ds.Tables[0].Rows[0]["EmailId"]);
                    Session.Add("EmployeeCode", ds.Tables[0].Rows[0]["EmployeeCode"]);
                    Session.Add("Designation", ds.Tables[0].Rows[0]["Designation"]);
                    Session.Add("ManagerName", ds.Tables[0].Rows[0]["ManagerName"]);
                    Session.Add("ManagerEmpCode", ds.Tables[0].Rows[0]["ManagerEmpCode"]);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            if (Convert.ToString(dr["roleID"]) == "1")//sysadmin
                            { Session.Add("SysAdmin", Convert.ToString(dr["roleID"])); }
                            else if (Convert.ToString(dr["roleID"]) == "2")//org user
                            { Session.Add("OrgUser", Convert.ToString(dr["roleID"])); }
                            else if (Convert.ToString(dr["roleID"]) == "3")//org admin
                            { Session.Add("OrgAdmin", Convert.ToString(dr["roleID"])); }
                            else if (Convert.ToString(dr["roleID"]) == "4")//org PM
                            { Session.Add("OrgPM", Convert.ToString(dr["roleID"])); }
                        }
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
        public ActionResult GetDateRange()
        {
            StringBuilder sbOut = new StringBuilder();
            try
            {
                DayOfWeek day = DateTime.Now.DayOfWeek;
                int days = day - DayOfWeek.Monday;
                DateTime startdate = DateTime.Now.AddDays(-days);
                DateTime endDate = startdate.AddDays(6);
                LoginViewModel lvm = new LoginViewModel();
                DataSet ds = lvm.GetDashBoardData(Convert.ToInt32(Session["sessUser"]), startdate, endDate);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    Session.Add("DashBoardDetail", ds);
                }
                sbOut.Append("<table id='tblSearch'>");
                sbOut.Append("<tr>");
                sbOut.Append("<td><label>Date range:</label></td>");
                sbOut.Append("<td><label>Start Date:</label></td>");
                sbOut.Append("<td><input type='text' class='form-control input-sm datepicker hasDatepicker' id='txtStartDate' value='" + startdate.ToShortDateString() + "'  /></td>");
                sbOut.Append("<td><label>End Date:</label></td>");
                sbOut.Append("<td><input type='text' class='form-control input-sm datepicker hasDatepicker' id='txtEndDate' value='" + endDate.ToShortDateString() + "' /></td>");
                sbOut.Append("<td><input type='submit' id='btnSearch' value='Show' name='btnSearch' class='btn btn-default' onclick='RefreshData();' /></td>");
                sbOut.Append("</tr>");
                sbOut.Append("</table>");
            }
            catch (Exception exx)
            { }
            return Content(sbOut.ToString());
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
                    string[] arrbg = new string[] { "#f39c12", "#00c0ef", "#0073b7", "#3c8dbc", "#00a65a", "#001f3f", "#39cccc", "#3d9970", "#01ff70", "#ff851b", "#f012be", "#605ca8", "#d81b60", "#020219", "#07074c", "#0f0f99", "#1616e5", "#4646ff", "#8c8cff", "#d1d1ff", "#a3a3ff", "#babaff", "#d1d1ff", "#e8e8ff", "#E6FCDD", "#EFF7B5", "#EFB5F7" };
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
                                    arrData[i] = (totalSec / 60).ToString() + "." + (totalSec % 60).ToString();
                                }
                                else
                                { arrData[i] = "0"; }
                            }
                            catch (Exception exo) { arrData[i] = "0"; }
                            i++;
                        }
                        dss.data = arrData;
                        //"#" + ((1 << 24) * Math.random() | 0).toString(16)
                        dss.backgroundColor = GetBackColor(arrbg, intColor, ((arrrayDate.Length> arrrayProj.Length)? arrrayDate.Length: arrrayProj.Length));// new string[] { arrbg[intColor] };// new string[] { "#" + ((1 << 24) * new Random().Next() | 0).ToString("16") };
                        dss.borderColor = new string[] { "#020219", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" };
                        //dss.backgroundColor = new string[] { "#FF0000", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" };
                        //dss.borderColor = new string[] { "#FF0000", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" };
                        dss.borderWidth = "1";
                        _dataSet.Add(dss);
                        intColor++;
                    }
                    //_dataSet.Add(new Datasets()
                    //{
                    //    label = "Current Year",
                    //    data = new string[] { "28", "48", "40", "19", "86", "27", "90", "20", "45", "65", "34", "22" },
                    //    backgroundColor = new string[] { "#FF0000", "#FF0000", "#FF0000", "#FF0000", "#FF0000", "#FF0000", "#FF0000", "#FF0000", "#FF0000", "#FF0000", "#FF0000", "#FF0000" },
                    //    borderColor = new string[] { "#FF0000", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" },
                    //    borderWidth = "1"
                    //});
                    //_dataSet.Add(new Datasets()
                    //{
                    //    label = "Last Year",
                    //    data = new string[] { "65", "59", "80", "81", "56", "55", "40", "55", "66", "77", "88", "34" },
                    //    backgroundColor = new string[] {  "#800000", "#800000", "#800000", "#800000", "#800000", "#800000", "#800000", "#800000", "#800000", "#800000", "#800000", "#800000" },
                    //    borderColor = new string[] { "#FF0000", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" },
                    //    borderWidth = "1"
                    //});
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
                            arrData[i] = (totalSec / 60).ToString() + "." + (totalSec % 60).ToString();
                        }
                        catch (Exception exo) { arrData[i] = "0"; }
                    }
                    _chart.labels = arrrayProj;// new string[] { "January", "February", "March", "April", "May", "June", "July" };
                    _chart.datasets = new List<Datasets>();
                    List<Datasets> _dataSet = new List<Datasets>();
                    _dataSet.Add(new Datasets()
                    {
                        label = "",
                        data = arrData,// new string[] { "28", "48", "40", "19", " 86", "27", "90" },
                        backgroundColor = new string[] { "#f39c12", "#00c0ef", "#0073b7", "#3c8dbc", "#00a65a", "#001f3f", "#39cccc", "#3d9970", "#01ff70", "#ff851b", "#f012be", "#605ca8", "#d81b60",  "#020219", "#07074c", "#0f0f99", "#1616e5", "#4646ff", "#8c8cff", "#d1d1ff", "#a3a3ff", "#babaff", "#d1d1ff", "#e8e8ff", "#1A5276", "#27AE60" },
                        borderColor = new string[] { "#020219", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" },
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
        public ActionResult GetRefreshedData(string startdate, string endDate)
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                Session.Remove("DashBoardDetail");
                LoginViewModel lvm = new LoginViewModel();
                DataSet dsNew = lvm.GetDashBoardData(Convert.ToInt32(Session["sessUser"]),Convert.ToDateTime(startdate),Convert.ToDateTime(endDate));
                Session.Add("DashBoardDetail", dsNew);

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

        #region Admin
        public ActionResult LoadDashBoardAdminSummary()
        {
            StringBuilder sbContent = new StringBuilder();
            try
            {
                if (Session["DashBoardDetail"] != null)
                {
                    DataSet ds = (DataSet)Session["DashBoardDetail"];
                    if (ds!=null && ds.Tables.Count>0 && ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
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
                        string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",//:{3:D3}ms
                                        t.Hours,
                                        t.Minutes,
                                        t.Seconds
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
                        sbContent.Append("<h3>" + (Convert.ToDouble((Convert.ToDouble(sumObject) / Convert.ToDouble(distinctValues.Rows.Count * 9*60*60)) * 100)).ToString("0.##") + " % " + "</h3>");
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
                                arrData[i] = (totalSec / 60).ToString() + "." + (totalSec % 60).ToString();
                            }
                            catch (Exception exo) { arrData[i] = "0"; }
                        }
                        _chart.labels = arrrayProj;// new string[] { "January", "February", "March", "April", "May", "June", "July" };
                        _chart.datasets = new List<Datasets>();
                        List<Datasets> _dataSet = new List<Datasets>();
                        _dataSet.Add(new Datasets()
                        {
                            label = "",
                            data = arrData,// new string[] { "28", "48", "40", "19", " 86", "27", "90" },
                            backgroundColor = new string[] { "#f39c12", "#00c0ef", "#0073b7", "#3c8dbc", "#00a65a", "#001f3f", "#39cccc", "#3d9970", "#01ff70", "#ff851b", "#f012be", "#605ca8", "#d81b60", "#020219", "#07074c", "#0f0f99", "#1616e5", "#4646ff", "#8c8cff", "#d1d1ff", "#a3a3ff", "#babaff", "#d1d1ff", "#e8e8ff", "#1A5276", "#27AE60" },
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
                                DataTable dtFilter = ds.Tables[3].Select("projectName = '" + arrrayProj[i]+"'").CopyToDataTable();
                                object sumObject;
                                sumObject = dtFilter.Compute("Sum(totalSec)", string.Empty);
                                int totalSec = Convert.ToInt32(sumObject);
                                arrData[i] = (totalSec / 60).ToString() + "." + (totalSec % 60).ToString();
                            }
                            catch (Exception exo) { arrData[i] = "0"; }
                        }
                        _chart.labels = arrrayProj;// new string[] { "January", "February", "March", "April", "May", "June", "July" };
                        _chart.datasets = new List<Datasets>();
                        List<Datasets> _dataSet = new List<Datasets>();
                        _dataSet.Add(new Datasets()
                        {
                            label = "",
                            data = arrData,// new string[] { "28", "48", "40", "19", " 86", "27", "90" },
                            backgroundColor = new string[] { "#f39c12", "#00c0ef", "#0073b7", "#3c8dbc", "#00a65a", "#001f3f", "#39cccc", "#3d9970", "#01ff70", "#ff851b", "#f012be", "#605ca8", "#d81b60", "#020219", "#07074c", "#0f0f99", "#1616e5", "#4646ff", "#8c8cff", "#d1d1ff", "#a3a3ff", "#babaff", "#d1d1ff", "#e8e8ff", "#1A5276", "#27AE60" },
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
                        string[] arrbg = new string[] { "#f39c12", "#00c0ef", "#0073b7", "#3c8dbc", "#00a65a", "#001f3f", "#39cccc", "#3d9970", "#01ff70", "#ff851b", "#f012be", "#605ca8", "#d81b60", "#020219", "#07074c", "#0f0f99", "#1616e5", "#4646ff", "#8c8cff", "#d1d1ff", "#a3a3ff", "#babaff", "#d1d1ff", "#e8e8ff", "#E6FCDD", "#EFF7B5", "#EFB5F7" };
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
                                    arrData[i] = (totalSec / 60).ToString() + "." + (totalSec % 60).ToString();
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
                        string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",//:{3:D3}ms
                                        t.Hours,
                                        t.Minutes,
                                        t.Seconds
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
                        sbContent.Append("<h3>" + (Convert.ToDouble((Convert.ToDouble(sumObject) / Convert.ToDouble(distinctValues.Rows.Count * 9 * 60 * 60)) * 100)).ToString("0.##") + " % " + "</h3>");
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
                                arrData[i] = (totalSec / 60).ToString() + "." + (totalSec % 60).ToString();
                            }
                            catch (Exception exo) { arrData[i] = "0"; }
                        }
                        _chart.labels = arrrayProj;// new string[] { "January", "February", "March", "April", "May", "June", "July" };
                        _chart.datasets = new List<Datasets>();
                        List<Datasets> _dataSet = new List<Datasets>();
                        _dataSet.Add(new Datasets()
                        {
                            label = "",
                            data = arrData,// new string[] { "28", "48", "40", "19", " 86", "27", "90" },
                            backgroundColor = new string[] { "#f39c12", "#00c0ef", "#0073b7", "#3c8dbc", "#00a65a", "#001f3f", "#39cccc", "#3d9970", "#01ff70", "#ff851b", "#f012be", "#605ca8", "#d81b60", "#020219", "#07074c", "#0f0f99", "#1616e5", "#4646ff", "#8c8cff", "#d1d1ff", "#a3a3ff", "#babaff", "#d1d1ff", "#e8e8ff", "#1A5276", "#27AE60" },
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
                                arrData[i] = (totalSec / 60).ToString() + "." + (totalSec % 60).ToString();
                            }
                            catch (Exception exo) { arrData[i] = "0"; }
                        }
                        _chart.labels = arrrayProj;// new string[] { "January", "February", "March", "April", "May", "June", "July" };
                        _chart.datasets = new List<Datasets>();
                        List<Datasets> _dataSet = new List<Datasets>();
                        _dataSet.Add(new Datasets()
                        {
                            label = "",
                            data = arrData,// new string[] { "28", "48", "40", "19", " 86", "27", "90" },
                            backgroundColor = new string[] { "#f39c12", "#00c0ef", "#0073b7", "#3c8dbc", "#00a65a", "#001f3f", "#39cccc", "#3d9970", "#01ff70", "#ff851b", "#f012be", "#605ca8", "#d81b60", "#020219", "#07074c", "#0f0f99", "#1616e5", "#4646ff", "#8c8cff", "#d1d1ff", "#a3a3ff", "#babaff", "#d1d1ff", "#e8e8ff", "#1A5276", "#27AE60" },
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
                        string[] arrbg = new string[] { "#f39c12", "#00c0ef", "#0073b7", "#3c8dbc", "#00a65a", "#001f3f", "#39cccc", "#3d9970", "#01ff70", "#ff851b", "#f012be", "#605ca8", "#d81b60", "#020219", "#07074c", "#0f0f99", "#1616e5", "#4646ff", "#8c8cff", "#d1d1ff", "#a3a3ff", "#babaff", "#d1d1ff", "#e8e8ff", "#E6FCDD", "#EFF7B5", "#EFB5F7" };
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
                                    arrData[i] = (totalSec / 60).ToString() + "." + (totalSec % 60).ToString();
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
    }
}