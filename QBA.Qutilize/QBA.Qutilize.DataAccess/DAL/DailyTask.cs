//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QBA.Qutilize.DataAccess.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class DailyTask
    {
        public int DailyTaskId { get; set; }
        public int? UserID { get; set; }
        public int? ProjectID { get; set; }
        public System.DateTime StartDateTime { get; set; }
        public Nullable<System.DateTime> EndDateTime { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string EditedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public bool IsActive { get; set; }
    }
}