using System;
using static Accelerant.DataTransfer.Models.TaskData;

namespace Accelerant.DataTransfer.Events
{
    public class TaskData
    {
        public enum Status
        {
            Blocked,
            Assignable,
            Assigned,
            Completed,
            Suspended,
            Closed
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Status CurrentStatus { get; set; }
        public Guid TaskGraphId { get; set; }
        
    }
}
