using System;

namespace Accelerant.DataTransfer.Events
{
    public class TaskGraph
    {
        public enum Event
        {
            Add,
            Delete,
            Update,
            Read
        }

        public Guid Id { get; set; }
        public DataTransfer.Models.TaskNode Root { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid WorkspaceId { get; set; }
        public Guid UserId { get; set; }
        public Guid? RootId { get; set; }
    }
}
