using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accelerant.DataTransfer.Models
{
    public class TaskSet
    {
        public Guid Id { get; set; }
        public ICollection<TaskNode> Tasks;
    }
}
