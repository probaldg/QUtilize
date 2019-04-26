using System;

namespace QBA.Qutilize.Models
{
    public class ProjectTask
    {
        public int TaskId { get; set; }
        public int ProjectID { get; set; }
        public string TaskCode { get; set; }
        public string TaskName { get; set; }
        public int ParentTaskId { get; set; }
        public string ProjectName { get; set; }
        public string ParentTaskName { get; set; }
        public DateTime TaskStartDate { get; set; }
        public string TaskStartDateDisplay { get; set; }
        public DateTime TaskEndDate { get; set; }
        public string TaskEndDateDisplay { get; set; }
        public int TaskStatusID { get; set; }
        public string TaskStatusName { get; set; }
        public int CompletePercent { get; set; }
        public DateTime? ActualTaskStartDate { get; set; }
        public string ActualTaskStartDateDisplay { get; set; }
        public DateTime? ActualTaskEndDate { get; set; }
        public string ActualTaskEndDateDisplay { get; set; }
        public int TaskDepthLevel { get; set; }
        public bool IsActive { get; set; }
        public bool? IsMilestone { get; set; }
        public int AddedBy { get; set; }
        public int EditedBy { get; set; }
        public DateTime AddedTS { get; set; }
        public DateTime EditedTS { get; set; }

        //public List<ProjectTaskModel> TaskList { get; set; }
        //public List<ProjectStatusModel> StatusList { get; set; }
        //public List<UserModel> UserList { get; set; }
        //public List<int> PercentageComplete { get; set; } 



    }
}
