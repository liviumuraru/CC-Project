using Accelerant.DataTransfer.Models;
using MongoDB.Driver;

namespace Accelerant.DataLayer.DataCollectors.Mongo
{
    public class UserCollector
        : AMongoDbCollector<User>
    {
        public UserCollector(IMongoCollection<User> taskGraphCollection)
            : base(taskGraphCollection)
        {
        }

        public override User Add(User item)
        {
            dataCollection.InsertOne(item);
            return item;
        }

        public override User Update(User item)
        {
            var updateOp = Builders<User>.Update
                .Set(x => x.Name, item.Name);

            dataCollection.UpdateOne(x => x.Id == item.Id, updateOp);

            return dataCollection.Find(x => x.Id == item.Id).First();
        }
    }
}
