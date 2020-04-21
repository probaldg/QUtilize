using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using QBA.Qutilize.WebApp.DAL;

namespace QBA.Qutilize.WebApp.Models
{
    public class TicketTypeModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool isActive { get; set; }
        public int OrgID { get; set; }        
        public string OrganisationName { get; set; }

        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion
        public List<TicketTypeModel> Get_GetAllTicketTypes(int orgId = 0)
        {
            DataTable dt = null;
            List<TicketTypeModel> lstTcktDetl = new List<TicketTypeModel>();
            try
            {

                if (orgId != 0)
                {
                    SqlParameter[] param ={
                                        new SqlParameter("@OrgId",orgId)
                                      };
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_MasterTicketType_GET]", param);

                }
                else
                    dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_MasterTicketType_GET]");
                

                

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        TicketTypeModel tickettypemodel = new TicketTypeModel();
                        tickettypemodel.ID = Convert.ToInt32(item["Id"]);
                        tickettypemodel.Name = item["Name"].ToString();
                        tickettypemodel.isActive = Convert.ToBoolean(item["IsActive"].ToString());
                        tickettypemodel.OrganisationName = item["OrganisationName"].ToString();
                        lstTcktDetl.Add(tickettypemodel);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lstTcktDetl;
        }
    }
}