using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace QBA.Qutilize.WebApp.Models
{
    public class ProjectTaskCommentModel
    {
        public int ID { get; set; }
        public int ProjectTaskID { get; set; }
        public string Comment { get; set; }
        public string URL { get; set; }
        public DateTime AddedTS { get; set; }
        public int AddedBy { get; set; }

        public bool ISErr { get; set; }
        public string ErrString { get; set; }


        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion

        public DataTable GetProjectTasksComments(int ProjectTaskID)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@ProjectTaskID",ProjectTaskID)
                                      };
                dt = objSQLHelper.ExecuteDataTable("USP_tblMasterProjectTaskComments_Get", param);
            }
            catch (Exception ex)
            {

            }
            return dt;

        }
    }
}