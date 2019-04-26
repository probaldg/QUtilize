using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QBA.Qutilize.WebApp.Models
{
    public class SkillManagementModel
    {
        public DataSet GetSkillManagementDetailData(int userID,int ORGID)
        {
            DataSet ds = null;
            try
            {
                ds = DataAccess.GetSkillManagementDetailData(userID,ORGID);
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
    }
    public class MasterSkill
    {
        public int Id { get; set; }
        public List<DepartmentModel> DepartmentList { get; set; }
        public string CategoryID { get; set; }
        public int UserOrgId { get; set; }
        public List<OrganisationModel> OrganisationList { get; set; }
        public int OrgID { get; set; }
        public string SkillCode { get; set; }
        public string SkillName { get; set; }
        public string Description { get; set; }
        public bool isMandatory { get; set; }
        public string Rank { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int EditedBy { get; set; }
        public DateTime EditedDate { get; set; }
        public bool IsActive { get; set; }

        public bool ISErr { get; set; }
        public string ErrString { get; set; }

        #region Global Variable Decleartion::
        SqlHelper objSQLHelper = new SqlHelper();
        #endregion
        public MasterSkill()
        {
            OrganisationList = new List<OrganisationModel>();
            DepartmentList = new List<DepartmentModel>();
        }

        public DataTable GetAllMasterSkill(bool isSysAdmin, int OrgID)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@isSysAdmin",isSysAdmin),
                                        new SqlParameter("@OrgID",OrgID),
                                      };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USP_GetAllMasterSkill]", param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }


        public DataTable GetMasterSkillByID(int ID)
        {
            DataTable dt = null;
            try
            {
                SqlParameter[] param ={
                                        new SqlParameter("@ID",ID)
                                      };

                dt = objSQLHelper.ExecuteDataTable("USP_GetAllMasterSkillByID", param);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }


        public Boolean InsertSkillMasterdata(MasterSkill model, out int id)
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
                    new SqlParameter("@SkillCode",model.SkillCode),
                    new SqlParameter("@SkillName",model.SkillName),
                    new SqlParameter("@Description",model.Description),
                    new SqlParameter("@OrgID",model.OrgID),
                    new SqlParameter("@CategoryID",model.CategoryID),
                    new SqlParameter("@isMandatory",model.isMandatory),
                    new SqlParameter("@Rank",model.Rank),
                   new SqlParameter("@CreatedBy",model.CreatedBy),
                    new SqlParameter("@CreatedDate",model.CreateDate),
                    new SqlParameter("@IsActive",model.IsActive),


                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPSkillMaster_Insert]", param);

                if (!(Status.Value is DBNull))
                {
                    id = Convert.ToInt32(Status.Value);
                    model.Id = id;
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
            catch (Exception ex)
            {
                model.ISErr = true;
                model.ErrString = "Error Occured.";
                result = false;
            }
            return result;

        }

        public Boolean Update_MasterSkill(MasterSkill model)
        {
            bool result = false;
            DataTable dt = null;

            try
            {
                SqlParameter[] param = {
                    new SqlParameter("@Id",model.Id),
                    new SqlParameter("@SkillCode",model.SkillCode),
                    new SqlParameter("@SkillName",model.SkillName),
                    new SqlParameter("@Description",model.Description),
                    new SqlParameter("@CategoryID",model.CategoryID),
                    new SqlParameter("@IsActive",model.IsActive),
                    new SqlParameter("@OrgId",model.OrgID),
                    new SqlParameter ("@Rank",model.Rank),
                    new SqlParameter("@isMandatory",model.isMandatory),
                    new SqlParameter("@EditedBy",model.EditedBy),
                    new SqlParameter("@EditedDate",model.EditedDate),

                };
                dt = objSQLHelper.ExecuteDataTable("[dbo].[USPMasterSkill_Update]", param);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }



    }
    public class MapUserSkill
    {
        public int Id { get; set; }
        public int skillID { get; set; }
        public int userID { get; set; }
        public int SkillRatingID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int EditedBy { get; set; }
        public DateTime EditedDate { get; set; }
        public bool IsActive { get; set; }
        public bool InsertUserSkillData(List<MapUserSkill> lstUserSkill)
        {
            bool bln = false;
            try
            {
                bln = DataAccess.InsertUserSkillData(lstUserSkill);
            }
            catch (Exception ex)
            {
                bln = false;
            }
            return bln;
        }
    }
}