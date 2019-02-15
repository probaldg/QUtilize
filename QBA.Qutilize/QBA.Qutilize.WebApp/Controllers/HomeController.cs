﻿using QBA.Qutilize.WebApp.Helper;
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
                DataTable dt = lvm.VerifyLogin(model.UserID, model.Password);
                if (dt != null && dt.Rows.Count > 0)
                {
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
                if (Session["sessUser"] == null)
                {
                    //get value from query string and create session
                    string strUser = EncryptionHelper.Decryptdata(Request.QueryString["U"]);
                    string strPass = EncryptionHelper.Decryptdata(Request.QueryString["P"]);
                    LoginViewModel lvm = new LoginViewModel();
                    //strPass = EncryptionHelper.ConvertStringToMD5(strPass);
                    DataTable dt = lvm.VerifyLogin(strUser, strPass);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Session.Add("sessUser", dt.Rows[0]["ID"]);
                    }
                }
                if (Session["sessUser"] == null) { return RedirectToAction("Index", "Home"); }
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
                    sbOut.Append(@"<h4>Date range:</h4><label>Start Date</label><span id='txtStartDate'>" + startdate + "</span><label>End Date</label><span id='txtEndDate'>" + endDate + "</span>");
                    Session.Add("DashBoardDetail", ds);
                }
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

                    foreach (string stproj in arrrayProj)
                    {
                        Datasets dss = new Datasets();
                        dss.label = stproj;
                        string[] arrData = new string[arrrayProj.Length];
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
                        dss.backgroundColor = new string[] { "#" + ((1 << 24) * new Random().Next() | 0).ToString("16") };
                        dss.borderColor = new string[] { "#FF0000", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" };
                        //dss.backgroundColor = new string[] { "#FF0000", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" };
                        //dss.borderColor = new string[] { "#FF0000", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" };
                        dss.borderWidth = "1";
                        _dataSet.Add(dss);
                    }
                    //_dataSet.Add(new Datasets()
                    //{
                    //    label = "Current Year",
                    //    data = new int[] { 28, 48, 40, 19, 86, 27, 90, 20, 45, 65, 34, 22 },
                    //    backgroundColor = new string[] { "#FF0000", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" },
                    //    borderColor = new string[] { "#FF0000", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" },
                    //    borderWidth = "1"
                    //});
                    //_dataSet.Add(new Datasets()
                    //{
                    //    label = "Last Year",
                    //    data = new int[] { 65, 59, 80, 81, 56, 55, 40, 55, 66, 77, 88, 34 },
                    //    backgroundColor = new string[] { "#FF0000", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" },
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
                    sbContent.Append("<div class='panel-body dvBorder form-group'>");
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
                    sbContent.Append("</div>");
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
                        backgroundColor = new string[] { "#FF0000", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" },
                        borderColor = new string[] { "#FF0000", "#800000", "#808000", "#008080", "#800080", "#0000FF", "#000080", "#999999", "#E9967A", "#CD5C5C", "#1A5276", "#27AE60" },
                        borderWidth = "1"
                    });
                    _chart.datasets = _dataSet;
                }
            }
            catch (Exception exx) { }
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }
    }
}