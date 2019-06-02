using System;
using Accelerant.DataTransfer.Models;
using MongoDB.Driver;

namespace Accelerant.DataLayer.DataCollectors.Mongo
{
    public class TaskGraphCollector
        : AMongoDbCollector<TaskGraph>
    {
        public TaskGraphCollector(IMongoCollection<TaskGraph> taskGraphCollection) 
            : base(taskGraphCollection)
        {
        }

        public override TaskGraph Add(TaskGraph item)
        {
            dataCollection.InsertOne(item);
            return item;
        }

        public override TaskGraph Update(TaskGraph item)
        {
            var updateOp = Builders<TaskGraph>.Update
                .Set(x => x.Description, item.Description)
                .Set(x => x.Name, item.Name)
                .Set(x => x.RootId, item.RootId)
                .Set(x => x.TaskSetId, item.TaskSetId);

            dataCollection.UpdateOne(x => x.Id == item.Id, updateOp);

            return dataCollection.Find(x => x.Id == item.Id).First();
        }
    }
}
