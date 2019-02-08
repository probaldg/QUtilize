﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class QUtilizeDBCon : DbContext
    {
        public QUtilizeDBCon()
            : base("name=QUtilizeDBCon")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DailyTask> DailyTasks { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
    
        public virtual int USPDailyTask_UpdateEndTaskTime(Nullable<int> dailyTaskId)
        {
            var dailyTaskIdParameter = dailyTaskId.HasValue ?
                new ObjectParameter("DailyTaskId", dailyTaskId) :
                new ObjectParameter("DailyTaskId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("USPDailyTask_UpdateEndTaskTime", dailyTaskIdParameter);
        }
    
        public virtual int USPDailyTasks_InsertTaskStartTime(Nullable<int> userID, Nullable<int> projectId, Nullable<System.DateTime> startDateTime, string createdby, Nullable<bool> isActive)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var projectIdParameter = projectId.HasValue ?
                new ObjectParameter("ProjectId", projectId) :
                new ObjectParameter("ProjectId", typeof(int));
    
            var startDateTimeParameter = startDateTime.HasValue ?
                new ObjectParameter("StartDateTime", startDateTime) :
                new ObjectParameter("StartDateTime", typeof(System.DateTime));
    
            var createdbyParameter = createdby != null ?
                new ObjectParameter("Createdby", createdby) :
                new ObjectParameter("Createdby", typeof(string));
    
            var isActiveParameter = isActive.HasValue ?
                new ObjectParameter("IsActive", isActive) :
                new ObjectParameter("IsActive", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("USPDailyTasks_InsertTaskStartTime", userIDParameter, projectIdParameter, startDateTimeParameter, createdbyParameter, isActiveParameter);
        }
    
        public virtual ObjectResult<USPUsers_Get_Result> USPUsers_Get(string userID, string password)
        {
            var userIDParameter = userID != null ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USPUsers_Get_Result>("USPUsers_Get", userIDParameter, passwordParameter);
        }
    }
}
