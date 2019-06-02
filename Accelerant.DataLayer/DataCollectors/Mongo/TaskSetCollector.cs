using Accelerant.DataTransfer.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accelerant.DataLayer.DataCollectors.Mongo
{
    public class TaskSetCollector
        : AMongoDbCollector<TaskSet>
    {
        public TaskSetCollector(IMongoCollection<TaskSet> taskGraphCollection)
            : base(taskGraphCollection)
        {
        }

        public override TaskSet Add(TaskSet item)
        {
            dataCollection.InsertOne(item);
            return item;
        }

        public override TaskSet Update(TaskSet item)
        {
            var updateOp = Builders<TaskSet>.Update
                .Set(x => x.Tasks, item.Tasks);

            dataCollection.UpdateOne(x => x.Id == item.Id, updateOp);

            return dataCollection.Find(x => x.Id == item.Id).First();
        }
    }
}
