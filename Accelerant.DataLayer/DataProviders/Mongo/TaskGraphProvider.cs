using Accelerant.DataTransfer.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Accelerant.DataLayer.DataProviders.Mongo
{
    public class TaskGraphProvider
        : AMongoDbProvider<TaskGraph>
    {
        public TaskGraphProvider(IMongoCollection<TaskGraph> dataCollection) 
            : base(dataCollection)
        {
        }

        public override TaskGraph Get(Guid Id)
        {
            return dataCollection.Find(x => x.Id == Id).First();
        }

        public override IEnumerable<TaskGraph> GetAll()
        {
            return dataCollection.AsQueryable().ToList();
        }

        public override IEnumerable<TaskGraph> GetAllForUser(Guid Id)
        {
            return dataCollection.AsQueryable().Where(x => x.Id == Id).ToList();
        }

        public override IEnumerable<TaskGraph> GetMany(IEnumerable<Guid> Id)
        {
            throw new NotImplementedException();
        }
    }
}
