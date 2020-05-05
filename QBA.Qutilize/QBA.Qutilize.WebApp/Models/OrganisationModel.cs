using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using QBA.Qutilize.WebApp.DAL;
using System.ComponentModel.DataAnnotations;

namespace QBA.Qutilize.WebApp.Models
{
    public class OrganisationModel
    {
        public int id { get; set; }
        public string orgname { get; set; }
        public string address { get; set; }
        public string url { get; set; }
        public string contact_email_id { get; set; }
        public string logo { get; set; }
        public string wikiurl { get; set; }
        [Display(Name = "No Of User License")]
        public int NoOfUserLicense { get; set; }
        public Boolean isActive { get; set; }
        public DateTime createdTS { get; set; }
        public DateTime editedTS { get; set; }
        public int createdBy { get; set; }
        public int editedBy { get; set; }

        string connectionType = "W";
        public DataTable GetALLOrganisationData()
        {
            DataTable dt = null;

            try
            {
                dt = DataAccess.GetALLOrganisationData();
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public DataTable GetALLActiveOrganisationData()
        {
            DataTable dt = null;

            try
            {
                dt = DataAccess.GetALLActiveOrganisationData(); ;
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public Boolean insert_OrganisationData(OrganisationModel model, out int id)
        {
            bool bln = false;
            bln = DataAccess.insert_OrganisationData(model, out id);
            return bln;
        }


        public DataTable GetOrganisationDataByID(int id)
        {
            bool bln = false;
            DataTable dt = null;

            try
            {

                dt = DataAccess.GetOrganisationDataByID(id);

            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public Boolean updateOrganisation(OrganisationModel model)
        {
            bool bln = false;
            try
            {

                bln = DataAccess.updateOrganisation(model);

            }
            catch (Exception ex)
            {

                bln = false;
            }
            return bln;
        }

        public DataTable GetOrganisationDataByURL(string url)
        {
            bool bln = false;
            DataTable dt = null;

            try
            {
                dt = DataAccess.GetOrganisationDataByURL(url);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }


        public DataTable GetALLOrganisationForCategory()
        {
            DataTable dt = null;

            try
            {
                dt = DataAccess.GetALLOrganisationForCategory();
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public int GetOrganisationIDByName(string orgName)
        {
            int orgId = 0;
            
            try
            {
                orgId = DataAccess.GetOrganisationIDByName(orgName);
            }
            catch (Exception ex)
            {

            }
            return orgId;
        }

    }
}