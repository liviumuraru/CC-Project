using System;
using System.Collections.Generic;

namespace Accelerant.DataLayer.DataProviders
{
    public interface IDataProvider<T>
    {
        T Get(Guid Id);
        IEnumerable<T> GetMany(IEnumerable<Guid> Id);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllForUser(Guid Id);
    }
}
