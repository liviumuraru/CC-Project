using Accelerant.DataLayer.DataCollectors;
using Accelerant.DataLayer.DataProviders;
using Accelerant.DataTransfer.Models;
using Accelerant.Services.Collectors;
using Accelerant.Services.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Accelerant.Services
{
    public class GraphNode
    {
        public TaskData Task;
        public ICollection<GraphNode> OutNeighbors;
    }

    public interface ITaskGraphService
        : IService<DataTransfer.Events.TaskGraph, DataTransfer.Models.TaskGraph>
    {
        DataTransfer.Models.TaskNode GetTask(Guid taskGraphId, Guid taskId);
        DataTransfer.Models.TaskNode AddTask(DataTransfer.Events.TaskData item, Guid taskGraphId);
        IEnumerable<DataTransfer.Models.TaskGraph> GetAllForUser(Guid UserId);
        bool AssignUserToTask(Guid asigneeUserId, Guid taskId, Guid TaskGraphId);
        GraphNode GetGraph(Guid Id);
        bool AddLink(Guid taskGraphId, Guid parentId, Guid childId);
    }

    public class TaskGraphService
        : ITaskGraphService
    {
        private IDataProvider<DataTransfer.Models.TaskGraph> dataProvider;
        private IDataCollector<DataTransfer.Models.TaskGraph> dataCollector;

        public TaskGraphService(IDataProvider<DataTransfer.Models.TaskGraph> dataProvider, IDataCollector<DataTransfer.Models.TaskGraph> dataCollector)
        {
            this.dataCollector = dataCollector;
            this.dataProvider = dataProvider;
        }

        public DataTransfer.Models.TaskGraph Add(DataTransfer.Events.TaskGraph item)
        {
            var mappedItem = new DataTransfer.Models.TaskGraph
            {
                Id = item.Id,
                Description = item.Description,
                Name = item.Name,
                RootId = null,
                UserId = item.UserId,
                TaskSetId = null
            };

            var added = dataCollector.Add(mappedItem);

            var ws = ServiceFactory.WorkspaceService.Get(item.WorkspaceId);
            if (ws.TaskGraphIds == null)
                ws.TaskGraphIds = new List<Guid>();
            ws.TaskGraphIds.Add(added.Id);

            DataCollectorFactory.workspaceCollector.Update(ws);

            return added;
        }

        public DataTransfer.Models.TaskGraph Get(Guid Id)
        {
            return dataProvider.Get(Id);
        }

        public IEnumerable<DataTransfer.Models.TaskGraph> GetAll()
        {
            return dataProvider.GetAll();
        }

        public IEnumerable<DataTransfer.Models.TaskGraph> GetMany(IEnumerable<Guid> Ids)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DataTransfer.Models.TaskGraph> GetAllForUser(Guid UserId)
        {
            return dataProvider.GetAllForUser(UserId);
        }

        public DataTransfer.Models.TaskGraph Update(DataTransfer.Events.TaskGraph item)
        {
            var mappedItem = new DataTransfer.Models.TaskGraph
            {
                Id = item.Id,
                Description = item.Description,
                Name = item.Name,
                RootId = item.RootId
            };

            return dataCollector.Update(mappedItem);
        }

        public DataTransfer.Models.TaskNode GetTask(Guid taskGraphId, Guid taskId)
        {
            var taskGraph = Get(taskGraphId);
            var taskSet = ServiceFactory.TaskSetService.Get(taskGraph.TaskSetId.Value);
            return taskSet.Tasks.Where(x => x.Data.Id == taskId).First();
        }

        public DataTransfer.Models.TaskNode AddTask(DataTransfer.Events.TaskData item, Guid taskGraphId)
        {
            var taskGraph = Get(taskGraphId);

            var task = new DataTransfer.Models.TaskNode
            {
                Data = new TaskData
                {
                    Id = item.Id,
                    CurrentStatus = item.CurrentStatus,
                    Description = item.Description,
                    Name = item.Name
                },
                AssignedUser = null,
                EstimatedCompletionTimespan = 0,
                OutNeighbors = new List<Guid>()
            };

            if (taskGraph.RootId is null && !taskGraph.TaskSetId.HasValue)
            {
                taskGraph.RootId = task.Data.Id;
                taskGraph.TaskSetId = Guid.NewGuid();
                DataCollectorFactory.taskGraphCollector.Update(taskGraph);
                var taskSet = new TaskSet
                {
                    Id = taskGraph.TaskSetId.Value,
                    Tasks = new List<TaskNode>()
                };
                taskSet.Tasks.Add(task);
                DataCollectorFactory.taskSetCollector.Add(taskSet);
            }
            else
            {
                var taskSet = ServiceFactory.TaskSetService.Get(taskGraph.TaskSetId.Value);
                taskSet.Tasks.Add(task);
                DataCollectorFactory.taskSetCollector.Update(taskSet);
            }

            return task;
        }

        public bool AssignUserToTask(Guid asigneeUserId, Guid taskId, Guid TaskGraphId)
        {
            var taskGraph = Get(TaskGraphId);
            var taskSet = ServiceFactory.TaskSetService.Get(taskGraph.TaskSetId.Value);

            var task = taskSet.Tasks.Where(x => x.Data.Id == taskId).First();
            taskSet.Tasks.Remove(task);
            task.AssignedUser = asigneeUserId;
            taskSet.Tasks.Add(task);

            DataCollectorFactory.taskSetCollector.Update(taskSet);

            return true;
        }

        public GraphNode GetGraph(Guid Id)
        {
            var graph = Get(Id);
            var taskSet = ServiceFactory.TaskSetService.Get(graph.TaskSetId.Value);
            var rootTaskData = taskSet.Tasks.Where(x => x.Data.Id == graph.RootId).First();

            var root = new GraphNode
            {
                Task = rootTaskData.Data,
                OutNeighbors = new List<GraphNode>()
            };
            var initRoot = root;

            var queue = new Queue<GraphNode>();
            queue.Enqueue(root);

            while(queue.Count != 0)
            {
                root = queue.Dequeue();

                var taskData = taskSet.Tasks.Where(x => x.Data.Id == root.Task.Id).First();

                foreach(var id in taskData.OutNeighbors)
                {
                    var outNeighbor = new GraphNode
                    {
                        Task = taskSet.Tasks.Where(x => x.Data.Id == id).First().Data,
                        OutNeighbors = new List<GraphNode>()
                    };

                    root.OutNeighbors.Add(outNeighbor);
                    queue.Enqueue(outNeighbor);
                }
            }

            return initRoot;
        }

        public bool AddLink(Guid graphId, Guid parentId, Guid childId)
        {
            var graph = Get(graphId);
            var taskSet = ServiceFactory.TaskSetService.Get(graph.TaskSetId.Value);

            var parent = taskSet.Tasks.Where(x => x.Data.Id == parentId).First();
            taskSet.Tasks.Remove(parent);
            parent.OutNeighbors.Add(childId);
            taskSet.Tasks.Add(parent);
            DataCollectorFactory.taskSetCollector.Update(taskSet);
            return true;
        }
    }
}
