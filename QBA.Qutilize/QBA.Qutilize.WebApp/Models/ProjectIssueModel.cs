using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace QBA.Qutilize.WebApp.Models
{
    public class ProjectIssueModel
    {
        public int IssueId { get; set; }
        public int ProjectID { get; set; }
        [Display(Name = "Issue Code")]
        public string IssueCode { get; set; }
        [Display(Name = "Issue Name")]
        public string IssueName { get; set; }
        [Display(Name = "Issue Description")]
        public string IssueDescription { get; set; }

        public string ProjectName { get; set; }
     
        public DateTime IssuestartDate { get; set; }

        [Display(Name = "Start Date")]
        public string IssueStartDateDisplay { get; set; }
        public DateTime IssueEndDate { get; set; }

        [Display(Name = "End Date")]
        public string IssueEndDateDisplay { get; set; }

        public int StatusID { get; set; }
        [Display(Name = "Status")]
        public string StatusName { get; set; }

        public List<ProjectStatusModel> StatusList { get; set; }
        public List<UserModel> UserList { get; set; }
       
        public string UserIdAssigned { get; set; }
        public string UserNameAssigned { get; set; }

        public List<int> PercentageComplete { get; set; }

        [Display(Name = "Complete percent")]
        public int CompletePercent { get; set; }

        public int SeverityID { get; set; }
        [Display(Name = "Severity")]
        public string SeverityName { get; set; }

        public DateTime? ActualIssueStartDate { get; set; }

        [Display(Name = "Actual Start Date")]
        public string ActualIssueStartDateDisplay { get; set; }


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

        public ProjectIssueModel()
        {
           
            StatusList = new List<ProjectStatusModel>();
            UserList = new List<UserModel>();
            PercentageComplete = new List<int>();

        }
        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion

        public DataTable GetAllProjectsIssue(int orgId = 0)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@OrgID",orgId)
                                      };

                dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_tblMasterProjectIssue_Get]", param);
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
        public Boolean InsertIssuedata(ProjectIssueModel model, out int id)
        {
            string str = string.Empty;
            bool result = false;
            DataTable dt = null;
            id = 0;

            try
            {
                SqlParameter Status = new SqlParameter("@Identity", SqlDbType.Int);
                Status.Direction = ParameterDirection.Output;
                SqlParameter[] param ={Status,
                    new SqlParameter("@ProjectID",model.ProjectID),
                    new SqlParameter("@IssueCode",model.IssueCode),
                    new SqlParameter("@IssueName",model.IssueName),
                    new SqlParameter("@IssueDescription", model.IssueDescription),
                    new SqlParameter("@IssuestartDate",model.IssuestartDate),
                    new SqlParameter("@IssueEndDate",model.IssueEndDate),
                    new SqlParameter("@SeverityID",model.SeverityID),
                    new SqlParameter("@StatusID",model.StatusID),
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
                    new SqlParameter("@IssueDescription", model.IssueDescription),
                    new SqlParameter("@IssuestartDate",model.IssuestartDate),
                    new SqlParameter("@IssueEndDate",model.IssueEndDate),
                    new SqlParameter("@SeverityID",model.SeverityID),
                    new SqlParameter("@StatusID",model.StatusID),
                    new SqlParameter("@CompletePercent",model.CompletePercent),
                    new SqlParameter("@IssueStartDateActual",model.ActualIssueStartDate),
                    new SqlParameter("@IssueEndDateActual",model.ActualIssueEndDate),
                    new SqlParameter("@isACTIVE",model.IsActive),
                    new SqlParameter("@isValueAdded",model.IsValueAdded),
                    new SqlParameter("@EditedBY",model.EditedBy),
                    new SqlParameter("@EditedTS",model.EditedTS),
                    new SqlParameter("@UserIds",model.UserIdAssigned)
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
      
    }
}