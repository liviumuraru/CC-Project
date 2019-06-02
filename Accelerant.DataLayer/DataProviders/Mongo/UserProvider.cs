using Accelerant.DataTransfer.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Accelerant.DataLayer.DataProviders.Mongo
{
    public class UserProvider
        : AMongoDbProvider<User>
    {
        public UserProvider(IMongoCollection<User> dataCollection)
            : base(dataCollection)
        {
        }

        public override User Get(Guid Id)
        {
            return dataCollection.Find(x => x.Id == Id).First();
        }

        public override IEnumerable<User> GetAll()
        {
            return dataCollection.AsQueryable().ToList();
        }

        public override IEnumerable<User> GetAllForUser(Guid Id)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<User> GetMany(IEnumerable<Guid> Id)
        {
            throw new NotImplementedException();
        }
    }
}
