using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace QBA.Qutilize.WebApp.Models
{
    public class ProjectIssueModel
    {
        public int IssueId { get; set; }
        public int ProjectID { get; set; }
        [Display(Name = "Ticket Code")]
        public string IssueCode { get; set; }
        [Display(Name = "Ticket Name")]
        public string IssueName { get; set; }
        [Display(Name = "Ticket Description")]
        public string IssueDescription { get; set; }

        [Display(Name = "Ticket Name")]
        public string IssueNameforChangeStatus { get; set; }
        [Display(Name = "Ticket Code")]
        public string IssueCodeforChangeStatus { get; set; }

        public string ProjectName { get; set; }
     
        public DateTime IssuestartDate { get; set; }

        [Display(Name = "Start Date")]
        public string IssueStartDateDisplay { get; set; }
        public DateTime  IssueEndDate { get; set; }

        [Display(Name = "End Date")]
        public string IssueEndDateDisplay { get; set; }

        public int StatusID { get; set; }
        [Display(Name = "Status")]
        public string StatusName { get; set; }


        public List<ProjectStatusModel> StatusList { get; set; }
        public List<UserModel> UserList { get; set; }

        public string UserEmailId { get; set; }
        public string UserName { get; set; }
        public string UserIdAssigned { get; set; }
        public string UserNameAssigned { get; set; }

        public List<int> PercentageComplete { get; set; }

        [Display(Name = "Complete percent")]
        public int CompletePercent { get; set; }

        public int SeverityID { get; set; }
        [Display(Name = "Severity")]
        public string SeverityName { get; set; }

        public int TicketTypeID { get; set; }
        [Display(Name = "Ticket Type")]
        public string TicketTypeName { get; set; }

        public DateTime? ActualIssueStartDate { get; set; }

        [Display(Name = "Actual Start Date")]
        public string ActualIssueStartDateDisplay { get; set; }

        [Display(Name = "Actual Start Date")]
        public string ActualIssueStartDateDisplayforstatus { get; set; }
        [Display(Name = "Actual End Date")]
        public string ActualIssueEndDateDisplayforstatus { get; set; }
        public int IssueIdforstatus { get; set; }
        [Display(Name = "Comment")]
        public String Comment { get; set; }
        [Display(Name = "URL")]
        public string url { get; set; }
        
        public string DirectoryName { get; set; }

        [Display(Name = "Time Spent")]
        public string Timespent { get; set; }
        public double Duration { get; set; }

        [Display(Name = "Expected Time")]
        public string ExpectedTime { get; set; }
        public double ExpectedDuration { get; set; }

        public DateTime TodayDate { get; set; }
        public DateTime OneDayBeforeDate { get; set; }

        public DateTime? ActualIssueEndDate { get; set; }

        [Display(Name = "Actual End Date")]
        public string ActualIssueEndDateDisplay { get; set; }
        public bool IsActive { get; set; }
        public bool IsValueAdded { get; set; }

        public int AddedBy { get; set; }
        public int EditedBy { get; set; }
        public DateTime AddedTS { get; set; }
        public DateTime EditedTS { get; set; }
        public bool ISErr { get; set; }
        public string ErrString { get; set; }
        public DateTime ss { get; set;}
        public ProjectIssueModel()
        {
           
            StatusList = new List<ProjectStatusModel>();
            UserList = new List<UserModel>();
            PercentageComplete = new List<int>();            

        }
        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion

        public DataTable GetAllProjectsTask(int userId = 0)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@Id",userId)
                                      };

                dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_Get_ProjectTaskByUSer]", param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public DataTable GetAllProjectsIssue(int userId = 0)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@Id",userId)
                                      };

                dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_tblMasterProjectIssue_Get]", param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public DataTable Get_MasterStatus(int OrgId = 0)
        {
            DataTable dt = null;
            try
            {
                if (OrgId != 0)
                {
                    SqlParameter[] param ={
                                        new SqlParameter("@OrgId",OrgId)
                                      };

                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_MasterStatus_GET]", param);
                }
                else
                {
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_MasterStatus_GET]");
                }
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public DataTable Get_ProjectTaskAssignedToUser(int UserId = 0)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@UserId",UserId)
                                      };
                dt = objSQLHelper.ExecuteDataTable("USP_UserWiseProjectTaskAssigned_Get", param);
            }
            catch (Exception ex)
            {

            }
            return dt;

        }

        public DataTable Get_ProjectIssueAssignedToUser(int UserId=0)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@UserId",UserId)
                                      };
                dt = objSQLHelper.ExecuteDataTable("USP_UserWiseProjectIssueAssigned_Get", param);
            }
            catch (Exception ex)
            {

            }
            return dt;

        }

        public DataTable GetProjectIssueByIssueId(int TaskId)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@IssueId",TaskId)
                                      };
                dt = objSQLHelper.ExecuteDataTable("USPtblMasterProjectIssue_Get", param);
            }
            catch (Exception ex)
            {

            }
            return dt;

        }
        public Boolean InsertIssuedata(ProjectIssueModel model, out int id,out string strMailToName, out string strMailTo)
        {
            string str = string.Empty;
            bool result = false;
            DataTable dt = null;
            id = 0;
            strMailToName = string.Empty;
            strMailTo = string.Empty;

            try
            {
                SqlParameter Status = new SqlParameter("@Identity", SqlDbType.Int);
                Status.Direction = ParameterDirection.Output;
                SqlParameter[] param ={Status,
                    new SqlParameter("@ProjectID",model.ProjectID),
                    new SqlParameter("@IssueCode",model.IssueCode),
                    new SqlParameter("@TicketTypeID",model.TicketTypeID),
                    new SqlParameter("@IssueName",model.IssueName),
                    new SqlParameter("@IssueDescription", model.IssueDescription!= null?model.IssueDescription:""),
                    new SqlParameter("@IssuestartDate",model.IssuestartDate),
                    new SqlParameter("@IssueEndDate",model.IssueEndDate),
                    new SqlParameter("@ExpectedTime",model.ExpectedDuration!=0?model.ExpectedDuration:0.00),
                    new SqlParameter("@StatusID",model.StatusID),
                    new SqlParameter("@SeverityID",model.SeverityID),
                    new SqlParameter("@CompletePercent",model.CompletePercent),
                    new SqlParameter("@IssueStartDateActual",model.ActualIssueStartDate!= null?model.ActualIssueStartDate:null),
                    new SqlParameter("@IssueEndDateActual",model.ActualIssueEndDate!= null?model.ActualIssueEndDate:null),
                    new SqlParameter("@isACTIVE",model.IsActive),
                    new SqlParameter("@isValueAdded",model.IsValueAdded),
                    new SqlParameter("@ADDEDBY",model.AddedBy),
                    new SqlParameter("@ADDEDTS",model.AddedTS),
                    new SqlParameter("@UserIds",model.UserIdAssigned)
                };
                dt = objSQLHelper.ExecuteDataTable("USPtblMasterProjectIssue_Insert", param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    strMailToName = Convert.ToString(dt.Rows[0]["MailToName"]);
                    strMailTo = Convert.ToString(dt.Rows[0]["MailTo"]);
                 }

                if (!(Status.Value is DBNull))
                {
                    id = Convert.ToInt32(Status.Value);
                    if (id > 0)
                    {
                        model.IssueId = id;
                        model.ISErr = false;
                        model.ErrString = "Data Saved Successfully.";
                        result = true;
                    }
                    else
                    {
                        id = 0;
                        result = false;
                        model.ISErr = true;
                        model.ErrString = "Error Occured.";
                    }

                }
                else
                {
                    id = 0;
                    result = false;
                    model.ISErr = true;
                    model.ErrString = "Error Occured.";
                }
            }
            catch (Exception ex)
            {
                model.ISErr = true;
                model.ErrString = "Error Occured.";
                result = false;
            }
            return result;

        }

        public DataTable GetProjectIDByProjectName(string  projectName)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@projectName",projectName)
                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_GetProjectId_ByProjectName]", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }


        public DataTable GetUserIDByUserName(string UserName,int projectID)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@userName",UserName),
                                        new SqlParameter("@ProjectId",projectID)
                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_GetUserId_ByUserName]", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataSet Get_Status_Servity_TicketName(string SeverityName, string statusName,string TicketTypeName,int userID)
        {
            DataSet dataset = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@SeverityName",SeverityName),
                                        new SqlParameter("@statusName",statusName),
                                        new SqlParameter("@TicketTypeName",TicketTypeName),
                                        new SqlParameter("@userID",userID)
                                      };
                dataset = objSQLHelper.ExecuteDataset("[dbo].[USP_Get_Status_Servity_TicketName]", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataset;
        }

        public Boolean UpdateIssuedata(ProjectIssueModel model)
        {
            string str = string.Empty;
            bool result = false;
            DataTable dt = null;

            try
            {

                SqlParameter[] param ={
                    new SqlParameter("@IssueID",model.IssueId),
                    new SqlParameter("@IssueCode",model.IssueCode),
                    new SqlParameter("@IssueName",model.IssueName),
                    new SqlParameter("@IssueDescription",  model.IssueDescription!= null?model.IssueDescription:""),
                    new SqlParameter("@IssuestartDate",model.IssuestartDate),
                    new SqlParameter("@IssueEndDate",model.IssueEndDate),
                    new SqlParameter("@SeverityID",model.SeverityID),
                    new SqlParameter("@TicketTypeID",model.TicketTypeID),
                    new SqlParameter("@StatusID",model.StatusID),
                    new SqlParameter("@CompletePercent",model.CompletePercent),
                    new SqlParameter("@IssueStartDateActual",model.ActualIssueStartDate!=null?model.ActualIssueStartDate:null),
                    new SqlParameter("@IssueEndDateActual",model.ActualIssueEndDate!=null?model.ActualIssueEndDate:null),
                    new SqlParameter("@isACTIVE",model.IsActive),
                    new SqlParameter("@isValueAdded",model.IsValueAdded),
                    new SqlParameter("@EditedBY",model.EditedBy),
                    new SqlParameter("@EditedTS",model.EditedTS),
                    new SqlParameter("@UserIds",model.UserIdAssigned),
                    new SqlParameter("@ExpectedTime",model.ExpectedDuration!=0?model.ExpectedDuration:0)

                

                };
                dt = objSQLHelper.ExecuteDataTable("USPtblMasterProjectIssue_Update", param);


                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;

        }


        public Boolean UpdateIssuestatus(ProjectIssueModel model, out string strMailToName, out string strMailTo)
        {
            string str = string.Empty;
            bool result = false;
            DataTable dt = null;
            DataTable dtAttachment = null;
            strMailToName = string.Empty;
            strMailTo = string.Empty;
            string FileDirectoryName = "";
            int IssueCommentId;
            if (model.DirectoryName != null && model.DirectoryName != "")
            {
                FileDirectoryName = model.DirectoryName.Replace("\"", string.Empty).Trim();
            }
              

            try
            {

                SqlParameter[] param ={
                    new SqlParameter("@IssueID",model.IssueIdforstatus),
                    new SqlParameter("@StatusID",model.StatusID),
                    new SqlParameter("@IssueStartDateActual",model.ActualIssueStartDate!=null?model.ActualIssueStartDate:null),
                    new SqlParameter("@IssueEndDateActual",model.ActualIssueEndDate!=null?model.ActualIssueEndDate:null),
                    new SqlParameter("@Comment", model.Comment!= null?model.Comment:""),
                    new SqlParameter("@Timespent",model.Duration!=0?model.Duration:0),
                    new SqlParameter("@EditedBY",model.EditedBy),
                    new SqlParameter("@EditedTS",model.EditedTS),
                     new SqlParameter("@url",model.url!=null?model.url:""),
                    new SqlParameter("@DirectoryName",FileDirectoryName!=null?FileDirectoryName:"")

                };
                dt = objSQLHelper.ExecuteDataTable("USPtblMasterProjectIssue_UpdateStatus", param);

                if (dt != null && dt.Rows.Count > 0)
                {
                    // int.TryParse(dt.Rows[0][0].ToString(), out ID);
                    strMailToName = Convert.ToString(dt.Rows[0]["MailToName"]);
                    strMailTo = Convert.ToString(dt.Rows[0]["MailTo"]);
                    IssueCommentId = Convert.ToInt32(dt.Rows[0]["IssueCommentId"]);
                    if (model.DirectoryName != null && model.DirectoryName != "")
                    {
                        FileDirectoryName = model.DirectoryName.Replace("\"", string.Empty).Trim();
                        var originalDirectory = new DirectoryInfo(string.Format("{0}IssueAttachments", System.Web.HttpContext.Current.Server.MapPath(@"\")));
                        string pathString = Path.Combine(originalDirectory.ToString(), FileDirectoryName);
                        string[] files = Directory.GetFiles(pathString);
                        foreach (string file in files)
                        {
                            string Attachments = Path.GetFileName(file);
                            SqlParameter[] param1 ={
                        new SqlParameter("@IssueID",model.IssueIdforstatus),
                        new SqlParameter("@DirectoryName",FileDirectoryName),
                        new SqlParameter("@AttachmentName",Attachments),
                        new SqlParameter("@AddedTS",model.EditedTS),
                        new SqlParameter("@AddedBy",model.EditedBy),
                        new SqlParameter("@IssueCommentId",IssueCommentId)
                         };
                            dtAttachment = objSQLHelper.ExecuteDataTable("USPtblMasterProjectIssueAttachments_Insert", param1);
                        }
                    }
                }

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

         
            return result;

        }
    }
}