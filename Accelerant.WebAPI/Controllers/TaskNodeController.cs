using Accelerant.Services.Collectors;
using Accelerant.Services.Mongo;
using Accelerant.WebAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Accelerant.WebAPI.Controllers
{
    public class AssignTaskModel
    {
        public Guid SelfUserId;
        public Guid NewUserId;
        public Guid TaskId;
        public Guid TaskGraphId;
    }

    public class AddLinkModel
    {
        public Guid ParentId;
        public Guid ChildId;
        public Guid TaskGraphId;
    }

    [Route("node")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class TaskNodeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromQuery]Guid TaskGraphId, [FromQuery]Guid taskId)
        {
            var taskGraph = ServiceFactory.TaskGraphService.Get(TaskGraphId);
            
            if (taskGraph.TaskSetId.HasValue)
            {
                var taskSet = ServiceFactory.TaskSetService.Get(taskGraph.TaskSetId.Value);
                var task = taskSet.Tasks.Where(x => x.Data.Id == taskId);
                return Ok(ServiceFactory.TaskSetService.Get(taskGraph.TaskSetId.Value));
            }

            return new NotFoundResult();
        }

        [HttpPost]
        [Route("link")]
        public IActionResult AddLink([FromBody]AddLinkModel addLinkModel)
        {
            var result = ServiceFactory.TaskGraphService.AddLink(addLinkModel.TaskGraphId, addLinkModel.ParentId, addLinkModel.ChildId);
            if (result)
                return Ok(result);
            else
                return BadRequest();
        }

        [HttpPost]
        [Route("assign")]
        public IActionResult Assign([FromBody]AssignTaskModel assignTaskData)
        {
            var result = ServiceFactory.TaskGraphService.AssignUserToTask(assignTaskData.NewUserId, assignTaskData.TaskId, assignTaskData.TaskGraphId);
            if (result)
                return Ok(result);
            else
                return BadRequest();
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(TaskNodeAddModel taskNodeAddData)
        {
            var taskData = new DataTransfer.Events.TaskData
            {
                CurrentStatus = taskNodeAddData.TaskData.CurrentStatus,
                Description = taskNodeAddData.TaskData.Description,
                Name = taskNodeAddData.TaskData.Name,
                Id = Guid.NewGuid(),
                TaskGraphId = taskNodeAddData.TaskGraphId
            };

            string topicEndpoint = "https://accelerant-task-topic.francecentral-1.eventgrid.azure.net/api/events";

            string topicKey = "xBCWj/db0/+GiJnkgAsdLClZxCtPZStDbwKFJxQ40R0=";

            string topicHostname = new Uri(topicEndpoint).Host;
            TopicCredentials topicCredentials = new TopicCredentials(topicKey);
            EventGridClient client = new EventGridClient(topicCredentials);

            var eventsList = new List<EventGridEvent>();

            eventsList.Add(new EventGridEvent()
            {
                Id = Guid.NewGuid().ToString(),
                EventType = "Accelerant.TaskNodes.AddItem",
                Data = taskData,
                EventTime = DateTime.Now,
                Subject = "accelerant-task-topic",
                DataVersion = "2.0"
            });

            client.PublishEventsAsync(topicHostname, eventsList).GetAwaiter().GetResult();
            return Ok(taskData);
        }
    }
}