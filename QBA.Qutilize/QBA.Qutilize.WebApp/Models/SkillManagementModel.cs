using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.Data;
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
        public string CategoryID { get; set; }
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
    }
    public class MasterSkillRating
    {
        public int Id { get; set; }
        public string SkillCode { get; set; }
        public string SkillLevel { get; set; }
        public string Description { get; set; }
        public int SkillScore { get; set; }
        public int ORGID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int EditedBy { get; set; }
        public DateTime EditedDate { get; set; }
        public bool IsActive { get; set; }
    }
}