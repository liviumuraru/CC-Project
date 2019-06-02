using MongoDB.Driver;
using System;

namespace Accelerant.DataLayer.DataCollectors.Mongo
{
    public abstract class AMongoDbCollector<T>
        : IDataCollector<T>
    {
        protected IMongoCollection<T> dataCollection;

        public AMongoDbCollector(IMongoCollection<T> dataCollection)
        {
            this.dataCollection = dataCollection;
        }

        public abstract T Add(T obj);

        public abstract T Update(T obj);
    }
}
