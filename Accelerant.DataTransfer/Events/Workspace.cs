using System;
using System.Collections.Generic;

namespace Accelerant.DataTransfer.Events
{
    public class Workspace
    {
        public enum Event
        {
            Add,
            Delete,
            Update,
            Read
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public IEnumerable<Guid> TaskGraphIds;
    }
}
