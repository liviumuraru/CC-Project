using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Accelerant.DataLayer.DataProviders.Mongo
{
    public abstract class AMongoDbProvider<T>
        : IDataProvider<T>
    {
        protected IMongoCollection<T> dataCollection;

        public AMongoDbProvider(IMongoCollection<T> dataCollection)
        {
            this.dataCollection = dataCollection;
        }

        public abstract T Get(Guid Id);

        public abstract IEnumerable<T> GetAll();

        public abstract IEnumerable<T> GetAllForUser(Guid Id);

        public abstract IEnumerable<T> GetMany(IEnumerable<Guid> Id);
    }
}
