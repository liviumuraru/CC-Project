using System;

namespace Accelerant.DataLayer.DataCollectors
{
    public interface IDataCollector<T>
    {
        T Add(T obj);
        T Update(T obj);
    }
}
