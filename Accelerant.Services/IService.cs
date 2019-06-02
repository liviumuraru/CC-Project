using Accelerant.DataTransfer.Models;
using System;
using System.Collections.Generic;

namespace Accelerant.Services
{
    public interface IService<TParam, TReturn>
    {
        TReturn Get(Guid Id);
        IEnumerable<TReturn> GetAll();
        IEnumerable<TReturn> GetMany(IEnumerable<Guid> Ids);
        TReturn Update(TParam item);
        TReturn Add(TParam item);
    }
}
