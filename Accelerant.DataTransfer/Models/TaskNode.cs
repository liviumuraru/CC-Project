using System;
using System.Collections.Generic;

namespace Accelerant.DataTransfer.Models
{
    public class TaskNode
    {
        public TaskData Data { get; set; }
        public ICollection<Guid> OutNeighbors;
        public Guid? AssignedUser { get; set; }
        public int? EstimatedCompletionTimespan { get; set; }
    }
}
