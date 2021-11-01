using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;


namespace QBA.Qutilize.WebApp.Models
{
    public class ProjectIssueCommentModel
    {
        public int ID { get; set; }
        public int IssueID { get; set; }
        public int UserID { get; set; }
        public string Comment { get; set; }
        public DateTime AddedTS { get; set; }



        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion

        public DataTable GetIssueCommentByIssueID(int ISSUEID)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@ISSUEID",ISSUEID)
                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_tblMapIssueComment_GetByISSUEID]", param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
    }
}