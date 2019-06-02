using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Accelerant.DataTransfer.Models
{
    public class Workspace
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public ICollection<Guid> TaskGraphIds;
        public Guid UserId { get; set; }
    }
}
