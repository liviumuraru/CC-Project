using MongoDB.Bson.Serialization.Attributes;
using System;
using static Accelerant.DataTransfer.Events.TaskData;

namespace Accelerant.DataTransfer.Models
{
    public class TaskData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Status CurrentStatus { get; set; }
    }
}
