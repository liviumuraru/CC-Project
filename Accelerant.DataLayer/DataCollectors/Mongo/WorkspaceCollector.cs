using Accelerant.DataTransfer.Models;
using MongoDB.Driver;
using System;

namespace Accelerant.DataLayer.DataCollectors.Mongo
{
    public class WorkspaceCollector
        : AMongoDbCollector<Workspace>
    {
        public WorkspaceCollector(IMongoCollection<Workspace> workspaceCollection)
            : base(workspaceCollection)
        {
        }

        public override Workspace Add(Workspace item)
        {
            dataCollection.InsertOne(item);
            return item;
        }

        public override Workspace Update(Workspace item)
        {
            var updateOp = Builders<Workspace>.Update
                .Set(x => x.Description, item.Description)
                .Set(x => x.Name, item.Name)
                .Set(x => x.TaskGraphIds, item.TaskGraphIds);

            dataCollection.UpdateOne(x => x.Id == item.Id, updateOp);

            return dataCollection.Find(x => x.Id == item.Id).First();
        }
    }
}
