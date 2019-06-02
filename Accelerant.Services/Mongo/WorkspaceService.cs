using Accelerant.DataLayer.DataCollectors;
using Accelerant.DataLayer.DataProviders;
using Accelerant.DataTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Accelerant.Services
{
    public interface IWorkspaceService
        : IService<DataTransfer.Events.Workspace, DataTransfer.Models.Workspace>
    {
        IEnumerable<DataTransfer.Models.Workspace> GetAllForUser(Guid Id);
    }

    public class WorkspaceService
        : IWorkspaceService
    {
        private IDataProvider<DataTransfer.Models.Workspace> dataProvider;
        private IDataCollector<DataTransfer.Models.Workspace> dataCollector;

        public WorkspaceService(IDataProvider<DataTransfer.Models.Workspace> dataProvider, IDataCollector<DataTransfer.Models.Workspace> dataCollector)
        {
            this.dataProvider = dataProvider;
            this.dataCollector = dataCollector;
        }

        public DataTransfer.Models.Workspace Get(Guid Id)
        {
            return dataProvider.Get(Id);
        }

        public IEnumerable<DataTransfer.Models.Workspace> GetMany(IEnumerable<Guid> Ids)
        {
            return dataProvider.GetMany(Ids);
        }

        public DataTransfer.Models.Workspace Update(DataTransfer.Events.Workspace item)
        {
            var mappedItem = new DataTransfer.Models.Workspace
            {
                Description = item.Description,
                Name = item.Name,
                Id = item.Id,
                TaskGraphIds = item.TaskGraphIds.ToList(),
                UserId = item.UserId
            };

            return dataCollector.Update(mappedItem);
        }

        public DataTransfer.Models.Workspace Add(DataTransfer.Events.Workspace item)
        {
            item.TaskGraphIds = new List<Guid>();

            var mappedItem = new DataTransfer.Models.Workspace
            {
                Description = item.Description,
                Name = item.Name,
                Id = item.Id,
                TaskGraphIds = new List<Guid>(),
                UserId = item.UserId
            };

            return dataCollector.Add(mappedItem);
        }

        public IEnumerable<DataTransfer.Models.Workspace> GetAll()
        {
            return dataProvider.GetAll();
        }

        public IEnumerable<Workspace> GetAllForUser(Guid Id)
        {
            return dataProvider.GetAllForUser(Id);
        }
    }
}
