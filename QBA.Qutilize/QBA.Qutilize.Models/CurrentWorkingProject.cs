using System;

namespace QBA.Qutilize.Models
{
    public class CurrentWorkingProject
    {
        //public Project Project { get; set; }
        public int? ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int DailyTaskId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool IsCurrentProject { get; set; }
        public int? MaxProjectTimeInHours { get; set; }
        public string TimeElapsedHeading { get; set; }
        public string TimeElapsedValue { get; set; }
        // public DateTime StartDateTime { get; set; }

        public int? DifferenceInSecondsInCurrentDate { get; set; }
        public int? ProjectTaskID { get; set; }
        public string ProjectTaskName { get; set; }

        public DateTime ProjectTaskStartDateTime { get; set; }
        public DateTime ProjectTaskEndDateTime { get; set; }
    }
}
