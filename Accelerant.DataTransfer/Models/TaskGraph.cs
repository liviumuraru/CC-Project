using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Accelerant.DataTransfer.Models
{
    public class TaskGraph
    {
        public Guid Id { get; set; }
        public Guid? RootId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? TaskSetId { get; set; }
        public Guid UserId { get; set; }
    }
}
