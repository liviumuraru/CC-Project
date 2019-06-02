using Accelerant.DataLayer.DataCollectors;
using Accelerant.DataLayer.DataProviders;
using Accelerant.DataTransfer.Events;
using Accelerant.DataTransfer.Models;
using Accelerant.Services.Collectors;
using Accelerant.Services.Mongo;
using System;
using System.Collections.Generic;

namespace Accelerant.Services
{
    public interface ITaskSetService
        : IService<DataTransfer.Events.TaskSet, DataTransfer.Models.TaskSet>
    {
    }

    public class TaskSetService
        : ITaskSetService
    {
        private IDataProvider<DataTransfer.Models.TaskSet> dataProvider;
        private IDataCollector<DataTransfer.Models.TaskSet> dataCollector;

        public TaskSetService(IDataProvider<DataTransfer.Models.TaskSet> dataProvider, IDataCollector<DataTransfer.Models.TaskSet> dataCollector)
        {
            this.dataCollector = dataCollector;
            this.dataProvider = dataProvider;
        }

        public DataTransfer.Models.TaskSet Add(DataTransfer.Events.TaskSet item)
        {
            var mappedItem = new DataTransfer.Models.TaskSet
            {
                Id = item.Id,
                Tasks = item.Tasks
            };

            return dataCollector.Add(mappedItem);
        }

        public DataTransfer.Models.TaskSet Get(Guid Id)
        {
            return dataProvider.Get(Id);
        }

        public IEnumerable<DataTransfer.Models.TaskSet> GetAll()
        {
            return dataProvider.GetAll();
        }

        public IEnumerable<DataTransfer.Models.TaskSet> GetMany(IEnumerable<Guid> Ids)
        {
            throw new NotImplementedException();
        }

        public DataTransfer.Models.TaskSet Update(DataTransfer.Events.TaskSet item)
        {
            var mappedItem = new DataTransfer.Models.TaskSet
            {
                Id = item.Id,
                Tasks = item.Tasks
            };

            return dataCollector.Update(mappedItem);
        }
    }
}
