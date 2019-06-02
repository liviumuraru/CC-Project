using Accelerant.DataTransfer.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Accelerant.DataLayer.DataProviders.Mongo
{
    public class TaskSetProvider
        : AMongoDbProvider<TaskSet>
    {
        public TaskSetProvider(IMongoCollection<TaskSet> dataCollection)
            : base(dataCollection)
        {
        }

        public override TaskSet Get(Guid Id)
        {
            return dataCollection.Find(x => x.Id == Id).First();
        }

        public override IEnumerable<TaskSet> GetAll()
        {
            return dataCollection.AsQueryable().ToList();
        }

        public override IEnumerable<TaskSet> GetAllForUser(Guid Id)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<TaskSet> GetMany(IEnumerable<Guid> Id)
        {
            throw new NotImplementedException();
        }
    }
}
