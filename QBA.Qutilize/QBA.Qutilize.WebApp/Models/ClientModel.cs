using System;

namespace QBA.Qutilize.WebApp.Models
{
    public class ClientModel
    {
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public int OrganisationID { get; set; }
        public string OrganisationName { get; set; }
        //OrgName_UserNameForCombo
        public string OrgName_ClientNameForCombo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public bool IsActive { get; set; }
        public bool ISErr { get; set; }
        public string ErrString { get; set; }


    }
}