using System;
using System.Collections.Generic;
using System.Text;

namespace Accelerant.DataTransfer.Events
{
    public class TaskNode
    {
        public enum Event
        {
            Add,
            Delete,
            Update,
            Read
        }

        public DataTransfer.Models.TaskData Task { get; set; }
        public IEnumerable<Guid> OutNeighbors { get; set; }
        public IEnumerable<Guid> InNeighbors { get; set; }
        public Guid TaskGraphId { get; set; }
        public Guid WorkspaceId { get; set; }
        public Guid? ParentId { get; set; }
        public Guid AssignedUser { get; set; }
        public int EstimatedCompletionTimespan { get; set; }
    }
}
