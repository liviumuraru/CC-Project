using Accelerant.Services.Collectors;
using MongoDB.Driver;

namespace Accelerant.Services.Mongo
{
    public static class ServiceFactory
    {
        static ServiceFactory()
        {
            TaskGraphService = new TaskGraphService(DataProviderFactory.taskGraphProvider, DataCollectorFactory.taskGraphCollector);
            WorkspaceService = new WorkspaceService(DataProviderFactory.workspaceProvider, DataCollectorFactory.workspaceCollector);
            TaskSetService = new TaskSetService(DataProviderFactory.taskSetProvider, DataCollectorFactory.taskSetCollector);
        }

        public static IWorkspaceService WorkspaceService { get; set; }

        public static ITaskGraphService TaskGraphService { get; set; }

        public static ITaskSetService TaskSetService { get; set; }
    }
}
