using System;
using System.Collections.Generic;
using System.Text;

namespace Accelerant.DataTransfer.Events
{
    public class TaskSet
    {
        public enum Event
        {
            Add,
            Delete,
            Update,
            Read
        }

        public Guid Id { get; set; }
        public ICollection<DataTransfer.Models.TaskNode> Tasks;
    }
}
