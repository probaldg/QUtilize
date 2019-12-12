using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Mvc;
using QBA.Qutilize.WebApp.Helper;
using QBA.Qutilize.WebApp.Models;

namespace QBA.Qutilize.WebApp.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetDateRange()
        {
            StringBuilder sbOut = new StringBuilder();
            try
            {
                DateTime startdate = DateTime.Now;
                DateTime endDate = DateTime.Now;
                if (Session["DateRange"] == null)
                {
                    DayOfWeek day = DateTime.Now.DayOfWeek;
                    int days = day - DayOfWeek.Monday;
                    startdate = DateTime.Now.AddDays(-days);
                    endDate = startdate.AddDays(6);
                    Session.Add("DateRange", startdate + "|" + endDate);
                }
                else
                {
                    string ss = Session["DateRange"].ToString();
                    string[] arrdate = Session["DateRange"].ToString().Split('|');
                    startdate = Convert.ToDateTime(arrdate[0]);
                    endDate = Convert.ToDateTime(arrdate[1]);
                }
                LoginViewModel lvm = new LoginViewModel();
                DataSet ds = lvm.GetDashBoardData(Convert.ToInt32(Session["sessUser"]), startdate, endDate,"0" ,"0");
                if (ds != null && ds.Tables.Count > 0 && ((ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) || (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0) || (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0) || (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)))
                {
                    Session.Add("DashBoardDetail", ds);
                }
                sbOut.Append("<div class='form-group col-md-12'>");
                sbOut.Append("<label class='control-label col-md-1'>Start Date: </label>");
                sbOut.Append("<div class='col-md-2'>");
                sbOut.Append("<input type='text' class='form-control' id='txtStartDate' value='" + startdate.ToShortDateString() + "'  />");
                sbOut.Append("</div>");
                sbOut.Append("<label class='control-label col-md-1'>End Date: </label>");
                sbOut.Append("<div class='col-md-2'>");
                sbOut.Append("<input type='text' class='form-control' onchange='ValidateEndDate();' id='txtEndDate' value='" + endDate.ToShortDateString() + "' />");
                sbOut.Append("</div>");
                sbOut.Append("<label class='control-label col-md-1'>Select User: </label>");
                

                UserInfoHelper UIH = new UserInfoHelper(int.Parse(HttpContext.Session["sessUser"].ToString()));
                if (HttpContext.Session["SysAdmin"] != null)
                {
                    UserProjectMappingModel USM = new UserProjectMappingModel();
                    DataTable dt = new DataTable();
                    sbOut.Append("<div class='col-md-2'>");
                    sbOut.Append("<select class='form-control' id='ddlUsers' name='ddlUsers'>");
                    sbOut.Append("<option value='0'>Select</option>");
                    var dtActiveUsers = USM.GetAllUsers().Select("IsActive=1 AND OrganisationStatus=1");
                        if (dtActiveUsers.Length > 0)
                        {
                            dt = dtActiveUsers.CopyToDataTable();
                            for (int i = 0; i < dtActiveUsers.Length; i++)
                                {
                                    sbOut.Append("<option value='"+dt.Rows[i]["Id"]+"'>"+dt.Rows[i]["Name"] +"</option>");                            
                                }
                        }
                    sbOut.Append("</select>");
                    sbOut.Append("</div>");
                    sbOut.Append("<label class='control-label col-md-1'>Select project: </label>");
                    ProjectModel pm = new ProjectModel();
                    DataTable dtAllProjects = new DataTable();
                    dtAllProjects = pm.GetAllProjects();
                    sbOut.Append("<div class='col-md-2'>");
                    sbOut.Append("<select class='form-control' id='ddlProjects' name='ddlProjects'>");
                    sbOut.Append("<option value='0'>Select</option>");
                    if (dtAllProjects.Rows.Count > 0)
                    {
                        for(int i = 0; i < dtAllProjects.Rows.Count; i++)
                        {
                            sbOut.Append("<option value='" + dtAllProjects.Rows[i]["Id"] + "'>" + dtAllProjects.Rows[i]["Name"] + "</option>");
                        }                        
                    }
                    sbOut.Append("</select>");
                    sbOut.Append("</div>");
                }
                else if(HttpContext.Session["OrgAdmin"] != null)
                {
                    UserProjectMappingModel USM = new UserProjectMappingModel();
                    DataTable dt = new DataTable();
                    sbOut.Append("<div class='col-md-2'>");
                    sbOut.Append("<select class='form-control' id='ddlUsers' name='ddlUsers'>");
                    sbOut.Append("<option value='0'>Select</option>");
                                        
                    var dtActiveUsers = USM.GetAllUsers(UIH.UserOrganisationID).Select("IsActive=1");
                    if (dtActiveUsers.Length > 0)
                    {
                        dt = dtActiveUsers.CopyToDataTable();
                        for (int i = 0; i < dtActiveUsers.Length; i++)
                        {

                            sbOut.Append("<option value='" + dt.Rows[i]["Id"] + "'>" + dt.Rows[i]["Name"] + "</option>");

                        }
                    }
                    sbOut.Append("</select>");
                    sbOut.Append("</div>");
                    
                    sbOut.Append("<label class='control-label col-md-1'>Select project: </label>");
                    ProjectModel pm = new ProjectModel();
                    DataTable dtAllProjects = new DataTable();
                    dtAllProjects = pm.GetAllProjects(UIH.UserOrganisationID);
                    sbOut.Append("<div class='col-md-2'>");
                    sbOut.Append("<select class='form-control' id='ddlProjects' name='ddlProjects'>");
                    sbOut.Append("<option value='0'>Select</option>");
                    if (dtAllProjects.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtAllProjects.Rows.Count; i++)
                        {
                            sbOut.Append("<option value='" + dtAllProjects.Rows[i]["Id"] + "'>" + dtAllProjects.Rows[i]["Name"] + "</option>");
                        }
                    }
                    sbOut.Append("</select>");
                    sbOut.Append("</div>");
                }
                else if (HttpContext.Session["OrgPM"] != null)
                {
                    UserProjectMappingModel USM = new UserProjectMappingModel();
                    DataTable dt = new DataTable();
                    sbOut.Append("<div class='col-md-2'>");
                    sbOut.Append("<select class='form-control' id='ddlUsers' name='ddlUsers'>");
                    sbOut.Append("<option value='0'>Select</option>");
                    sbOut.Append("<option value='" + HttpContext.Session["UserID"].ToString() + "'>" + HttpContext.Session["Name"].ToString() + "</option>");
                    var dtActiveUsers = USM.GetAllUsersByManagerID(int.Parse(HttpContext.Session["UserID"].ToString())).Select("IsActive=1");
                    if (dtActiveUsers.Length > 0)
                    {
                        dt = dtActiveUsers.CopyToDataTable();
                        for (int i = 0; i < dtActiveUsers.Length; i++)
                        {

                            sbOut.Append("<option value='" + dt.Rows[i]["Id"] + "'>" + dt.Rows[i]["Name"] + "</option>");

                        }
                    }
                    sbOut.Append("</select>");
                    sbOut.Append("</div>");

                    sbOut.Append("<label class='control-label col-md-1'>Select project: </label>");
                    UserProjectMappingModel UPM = new UserProjectMappingModel();
                    DataTable dtAllProjects = new DataTable();
                    dtAllProjects = UPM.GetAllProjectByManagerID(int.Parse(HttpContext.Session["UserID"].ToString()));
                    sbOut.Append("<div class='col-md-2'>");
                    sbOut.Append("<select class='form-control' id='ddlProjects' name='ddlProjects'>");
                    sbOut.Append("<option value='0'>Select</option>");
                    if (dtAllProjects.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtAllProjects.Rows.Count; i++)
                        {
                            sbOut.Append("<option value='" + dtAllProjects.Rows[i]["Id"] + "'>" + dtAllProjects.Rows[i]["ProjectName"] + "</option>");
                        }
                    }
                    sbOut.Append("</select>");
                    sbOut.Append("</div>");
                }
                else
                {
                    sbOut.Append("<div class='col-md-2'>");
                    sbOut.Append("<select class='form-control' id='ddlUsers' name='ddlUsers'>");
                    sbOut.Append("<option value='" + HttpContext.Session["UserID"] + "' selected>" + HttpContext.Session["Name"] + "</option>");
                    sbOut.Append("</select>");
                    sbOut.Append("</div>");
                    
                    sbOut.Append("<label class='control-label col-md-1'>Select project: </label>");                    
                    UserProjectMappingModel UPM = new UserProjectMappingModel();
                    DataTable dtAllProjects = new DataTable();
                    dtAllProjects = UPM.GetAllProjectByUserID(int.Parse(HttpContext.Session["UserID"].ToString()));
                    sbOut.Append("<div class='col-md-2'>");
                    sbOut.Append("<select class='form-control' id='ddlProjects' name='ddlProjects'>");
                    sbOut.Append("<option value='0'>Select</option>");
                    if (dtAllProjects.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtAllProjects.Rows.Count; i++)
                        {
                            sbOut.Append("<option value='" + dtAllProjects.Rows[i]["Id"] + "'>" + dtAllProjects.Rows[i]["Name"] + "</option>");
                        }
                    }
                    sbOut.Append("</select>");
                    sbOut.Append("</div>");
                }
                
                sbOut.Append("<div class='form-group col-md-12'>");
                sbOut.Append("<label class='control-label col-md-1'>Select Report Type: </label>");
                sbOut.Append("<div class='col-md-2'>");
                sbOut.Append("<select class='form-control' id='ddlReportType' name='ddlReportType'>");
                sbOut.Append("<option value='0'>Select</option>");
                if (HttpContext.Session["OrgAdmin"] != null)
                {
                    sbOut.Append("<option value='1'>Get detail break up</option>");
                    sbOut.Append("<option value='2'>Project Wise Summary</option>");
                    sbOut.Append("<option value='3'>Resource Utilization</option>");
                    sbOut.Append("<option value='4'>Resource Costing</option>");
                    sbOut.Append("<option value='5'>Project Wise Resource Costing</option>");
                }
                else
                    sbOut.Append("<option value='1'>Get detail break up</option>");
                sbOut.Append("</select>");
                sbOut.Append("</div>");
                sbOut.Append("<div class='col-md-1 pull-right' style='margin: 7px;'>");
                sbOut.Append("<input type='submit' id='btnSearch' value='Search' name='btnSearch' class='btn btn-primary' onclick='RefreshData();' />");
                sbOut.Append("</div>");
                sbOut.Append("</div>");
                sbOut.Append("</div>");
                sbOut.Append("<hr>");

            }
            catch (Exception exx)
            { }
            return Content(sbOut.ToString());
        }
        public ActionResult GetReportData(DateTime startdate, DateTime endDate, int userid, int projectid, int ReportType)
        {
            StringBuilder sbOut = new StringBuilder();
            try
            {
                string Role = "";
                if (HttpContext.Session["SysAdmin"] != null)
                {
                     Role = "SysAdmin";
                }
                else if (HttpContext.Session["SysAdmin"] == null && HttpContext.Session["OrgAdmin"] != null &&( HttpContext.Session["OrgPM"] != null || HttpContext.Session["OrgPM"]== null) )
                {
                     Role = "OrgAdmin";
                }
                else if (HttpContext.Session["SysAdmin"] == null && HttpContext.Session["OrgAdmin"] == null && HttpContext.Session["OrgPM"] != null)
                {
                     Role = "OrgPM";
                }
                else
                {
                    Role = "User";
                }

                LoginViewModel lvm = new LoginViewModel();
                if (ReportType == 1)
                {
                    DataSet ds = lvm.GetReportData(userid, startdate, endDate, projectid, int.Parse(HttpContext.Session["UserID"].ToString()), Role);
                    sbOut.Append("<table class='table table-bordered dataTable no-footer' width='100%' id='tableReportData'>");
                    sbOut.Append("<thead><tr><th class='text-center tblHeaderColor'>Date</th><th class='text-center tblHeaderColor'>User</th><th class='text-center tblHeaderColor'>Client</th><th class='text-center tblHeaderColor'>Project</th><th class='text-center tblHeaderColor'>Milestone</th><th class='text-center tblHeaderColor'>Activity Group</th><th class='text-center tblHeaderColor'>Activity</th><th class='text-center tblHeaderColor'>Naration</th><th class='text-center tblHeaderColor'>Total Hours</th><th style='display: none;'>Total Sec</th></tr></thead>");
                    sbOut.Append("<tbody id='tableBodyReportData'>");
                    if (ds != null && ds.Tables.Count > 0 && ((ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)))
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            double hours = 0;
                            double seconds = 0;
                            string strHr = string.Empty;
                            if (ds.Tables[0].Rows[i]["enddatetime"].ToString() != "")
                            {
                                TimeSpan diff = Convert.ToDateTime(ds.Tables[0].Rows[i]["enddatetime"].ToString()) - Convert.ToDateTime(ds.Tables[0].Rows[i]["startdatetime"].ToString());
                                hours = Math.Round(diff.TotalHours, 2);
                                strHr = diff.Hours.ToString() + ":" + diff.Minutes.ToString() + ":" + diff.Seconds.ToString();
                                seconds = diff.TotalSeconds;
                                seconds = diff.Hours * 3600 + diff.Minutes * 60 + diff.Seconds;
                            }
                            sbOut.Append("<tr><td><span class='control-text'>" + ds.Tables[0].Rows[i]["Date"] + "</span></td><td><span class='control-text'>" + ds.Tables[0].Rows[i]["UserName"] + "</span></td><td><span class='control-text'>" + ds.Tables[0].Rows[i]["clientName"] + "</span></td><td><span class='control-text'>" + ds.Tables[0].Rows[i]["projectName"] + "</span></td><td><span class='control-text'>-</span></td><td><span class='control-text'>" + ds.Tables[0].Rows[i]["TaskName"] + "</span></td><td><span class='control-text'>" + ds.Tables[0].Rows[i]["Description"] + "</span></td><td><span class='control-text'>" + ds.Tables[0].Rows[i]["Description"] + "</span></td><td><span class='control-text'>" + strHr + "</span></td><td style='display: none;'><span class='control-text'>" + seconds + "</span></td></tr>");
                        }
                    }
                    sbOut.Append("</tbody></table>");
                }
                else if (ReportType == 2)
                {
                    DataSet ds = lvm.GetReportDataProjectWiseSummary(userid, startdate, endDate, projectid, int.Parse(HttpContext.Session["UserID"].ToString()), Role);
                    sbOut.Append("<table class='table table-bordered dataTable no-footer' width='100%' id='tableReportData'>");
                    sbOut.Append("<thead><tr>");
                    sbOut.Append("<th class='text-center tblHeaderColor'>Resource Name</th>");
                    sbOut.Append("<th class='text-center tblHeaderColor'>Cumulative Hour(HH:MM)</th>");
                    sbOut.Append("<th class='text-center tblHeaderColor'>Billable Hour(HH:MM)</th>");
                    sbOut.Append("<th class='text-center tblHeaderColor'>Utilized % VA</th>");
                    sbOut.Append("<th class='text-center tblHeaderColor'>Non-Billable Hour(HH:MM)</th>");
                    sbOut.Append("<th class='text-center tblHeaderColor'>Utilized % NV</th>");
                    sbOut.Append("</tr></thead>");

                    sbOut.Append("<tbody id='tableBodyReportData'>");
                    if (ds != null && ds.Tables.Count > 0 && ((ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)))
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            sbOut.Append("<tr>");
                            sbOut.Append("<td><span class='control-text'>" + Convert.ToString(ds.Tables[0].Rows[i]["UserName"]) + "</span></td>");
                            sbOut.Append("<td><span class='control-text'>" + Convert.ToString(ds.Tables[0].Rows[i]["Total"]) + "</span></td>");
                            sbOut.Append("<td><span class='control-text'>" + Convert.ToString(ds.Tables[0].Rows[i]["Billable"]) + "</span></td>");
                            sbOut.Append("<td><span class='control-text'>" + Convert.ToString(ds.Tables[0].Rows[i]["Billable%"]) + "</span></td>");
                            sbOut.Append("<td><span class='control-text'>" + Convert.ToString(ds.Tables[0].Rows[i]["NonBillable"]) + "</span></td>");
                            sbOut.Append("<td><span class='control-text'>" + Convert.ToString(ds.Tables[0].Rows[i]["NonBillable%"]) + "</span></td>");
                            sbOut.Append("</tr>");
                        }
                    }
                    sbOut.Append("</tbody></table>");
                }
                else if (ReportType ==3)
                {
                    projectid = 0;
                     DataSet ds = lvm.GetReportDataResourceUtilizationSummary(userid, startdate, endDate, projectid, int.Parse(HttpContext.Session["UserID"].ToString()), Role);
                    
                    sbOut.Append("<table class='table table-bordered dataTable no-footer' width='100%' id='tableReportData'>");
                    sbOut.Append("<thead><tr>");
                    sbOut.Append("<th class='text-center tblHeaderColor'>Resource Name</th>");
                   
                    if (ds != null && ds.Tables.Count > 0 && ((ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)))
                    {
                        foreach (DataTable table in ds.Tables)
                        {
                            foreach (DataColumn column in table.Columns)
                            {
                                if (column.ColumnName != "UserName")
                                {
                                    sbOut.Append("<th class='text-center tblHeaderColor'>" + column.ColumnName + "(Hour)</th>");
                                    sbOut.Append("<th class='text-center tblHeaderColor'>" + column.ColumnName + "(%)</th>");
                                }
                            }

                        }
                    }
                    sbOut.Append("<th class='text-center tblHeaderColor'>Total Hour</th>");
                    sbOut.Append("<th class='text-center tblHeaderColor'>Total % </th>");
                    sbOut.Append("</tr></thead>");
                   
                    sbOut.Append("<tbody id='tableBodyReportData'>");
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    string[] Arr = new string[2];
                    double TotalHr = 0, TotalPercentage = 0;
                    if (ds != null && ds.Tables.Count > 0 && ((ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)))
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                            {
                                if (j!=0)
                                {
                                    string totalduration = Convert.ToString(ds.Tables[0].Rows[i][j]);
                                    
                                    if (totalduration != "")
                                    {  
                                        Arr = totalduration.Split('/');
                                        TotalHr = TotalHr + Convert.ToDouble(Arr[0]);
                                        TotalPercentage = TotalPercentage + Convert.ToDouble(Arr[1]);
                                        Arr[1] = Arr[1] + "%";
                                    }
                                    else
                                    {
                                        Arr[0] = "";
                                        Arr[1] = "";
                                    }
                                    sbOut.Append("<td><span class='control-text'>" + Arr[0] + "</span></td>");
                                    sbOut.Append("<td><span class='control-text'>" + Arr[1] + "</span></td>");
                                }
                                else
                                {
                                    sbOut.Append("<td><span class='control-text'>" + Convert.ToString(ds.Tables[0].Rows[i][j]) + "</span></td>");
                                }
                                
                            }
                            sbOut.Append("<td><span class='control-text'>" + TotalHr + "</span></td>");
                            sbOut.Append("<td><span class='control-text'>" + TotalPercentage + "%</span></td>");
                            sbOut.Append("</tr>");
                            TotalHr = 0; TotalPercentage = 0;
                        }
                       
                    }
                    sbOut.Append("</tbody></table>");
                }
                else if (ReportType == 4)
                {
                    DataSet ds = lvm.GetReportDataResourceCosting(userid, startdate, endDate, projectid, int.Parse(HttpContext.Session["UserID"].ToString()), Role);

                    sbOut.Append("<table class='table table-bordered dataTable no-footer' width='100%' id='tableReportData'>");
                    sbOut.Append("<thead><tr>");
                    sbOut.Append("<th class='text-center tblHeaderColor'>Resource Name</th>");

                    if (ds != null && ds.Tables.Count > 0 && ((ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)))
                    {
                        foreach (DataTable table in ds.Tables)
                        {
                            foreach (DataColumn column in table.Columns)
                            {
                                if (column.ColumnName != "UserName")
                                {
                                    sbOut.Append("<th class='text-center tblHeaderColor'>" + column.ColumnName + "(%)</th>");
                                    sbOut.Append("<th class='text-center tblHeaderColor'>" + column.ColumnName + "(Cost)</th>");
                                }
                            }

                        }
                    }
                    sbOut.Append("<th class='text-center tblHeaderColor'>Total %</th>");
                    sbOut.Append("<th class='text-center tblHeaderColor'>Total Cost </th>");
                    sbOut.Append("</tr></thead>");

                    sbOut.Append("<tbody id='tableBodyReportData'>");
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    string[] Arr = new string[2];
                    double TotalCost = 0, TotalPercentage = 0;
                    if (ds != null && ds.Tables.Count > 0 && ((ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)))
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                            {
                                if (j != 0)
                                {
                                    string totalCostingPersentage = Convert.ToString(ds.Tables[0].Rows[i][j]);

                                    if (totalCostingPersentage != "")
                                    {
                                        Arr = totalCostingPersentage.Split('/');
                                        TotalPercentage = TotalPercentage + Convert.ToDouble(Arr[0]);
                                        Arr[0] = Arr[0] + "%";
                                        TotalCost = TotalCost + Convert.ToDouble(Arr[1]);
                                    }
                                    else
                                    {
                                        Arr[0] = "";
                                        Arr[1] = "";
                                    }
                                    sbOut.Append("<td><span class='control-text'>" + Arr[0] + "</span></td>");
                                    sbOut.Append("<td><span class='control-text'>" + Arr[1] + "</span></td>");
                                }
                                else
                                {
                                    sbOut.Append("<td><span class='control-text'>" + Convert.ToString(ds.Tables[0].Rows[i][j]) + "</span></td>");
                                }

                            }
                            sbOut.Append("<td><span class='control-text'>" + TotalPercentage + "%</span></td>");
                            sbOut.Append("<td><span class='control-text'>" + TotalCost + "</span></td>");
                            sbOut.Append("</tr>");
                            TotalCost = 0; TotalPercentage = 0;
                        }

                    }
                    sbOut.Append("</tbody></table>");
                }
                else if (ReportType == 5)
                {
                    double TotalPercentage = 0, TotalCost = 0, TotalHr = 0;
                    DataSet ds = lvm.GetReportDataProjectWiseCosting(userid, startdate, endDate, projectid, int.Parse(HttpContext.Session["UserID"].ToString()), Role);
                    sbOut.Append("<table class='table table-bordered dataTable ' width='100%' id='tableReportData'>");
                    sbOut.Append("<thead><tr>");
                    sbOut.Append("<th class='text-center tblHeaderColor'>Resource Name</th>");
                    sbOut.Append("<th class='text-center tblHeaderColor'>Cumulative Hour(HH:MM)</th>");
                    sbOut.Append("<th class='text-center tblHeaderColor'>Project %</th>");    
                    sbOut.Append("<th class='text-center tblHeaderColor'>Project Cost</th>");
                 
                    sbOut.Append("</tr></thead>");

                    sbOut.Append("<tbody id='tableBodyReportData'>");
                    if (ds != null && ds.Tables.Count > 0 && ((ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)))
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            TotalPercentage = TotalPercentage + Convert.ToDouble(ds.Tables[0].Rows[i]["TotalPer"]);
                            TotalCost = TotalCost + Convert.ToDouble(ds.Tables[0].Rows[i]["Usercost"]);
                            TotalHr= TotalHr+ Convert.ToDouble(ds.Tables[0].Rows[i]["TotalHr"]);
                            sbOut.Append("<tr>");
                            sbOut.Append("<td class='text-center '>" + Convert.ToString(ds.Tables[0].Rows[i]["UserName"]) + "</td>");
                            sbOut.Append("<td class='text-center '>" + Convert.ToString(ds.Tables[0].Rows[i]["TotalHr"]) + "</td>");
                            sbOut.Append("<td class='text-center '>" + Convert.ToString(ds.Tables[0].Rows[i]["TotalPer"]) + "</td>");
                            sbOut.Append("<td class='text-center '>" + Convert.ToString(ds.Tables[0].Rows[i]["Usercost"]) + "</td>");
                            sbOut.Append("</tr>");
                        }
                    }

                  
                    sbOut.Append("</tbody><tfoot  > ");   
                    sbOut.Append("<tr>");
                    sbOut.Append("<td class='text-center '> Total </td>");
                    sbOut.Append("<td class='text-center '>" + TotalHr + "</td>");
                    sbOut.Append("<td class='text-center '>" + TotalPercentage + "</td>");
                    sbOut.Append("<td class='text-center '>" + TotalCost + "</td>");
                    sbOut.Append("</tr>");
                    sbOut.Append("</tfoot></ table >");
                }
            }
            catch (Exception exx)
            {
                
            }
            return Json(sbOut.ToString());
        }

    }
}