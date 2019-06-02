using Accelerant.Services.Mongo;
using Accelerant.WebAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using System;
using System.Collections.Generic;

namespace Accelerant.WebAPI.Controllers
{
    [Route("graph")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class TaskGraphController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromQuery]Guid TaskGraphId, [FromQuery]Guid UserId)
        {
            var taskGraph = ServiceFactory.TaskGraphService.Get(TaskGraphId);
            if (taskGraph.UserId != UserId)
                return new NotFoundResult();
            return Ok(taskGraph);
        }

        [HttpGet]
        [Route("data")]
        public IActionResult GetGraphModel([FromQuery]Guid TaskGraphId, [FromQuery]Guid UserId)
        {
            var taskGraph = ServiceFactory.TaskGraphService.Get(TaskGraphId);
            if (taskGraph.UserId != UserId)
                return new NotFoundResult();
            var taskGraphModel = ServiceFactory.TaskGraphService.GetGraph(TaskGraphId);
            return Ok(taskGraphModel);
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAllForUser([FromQuery]Guid UserId)
        {
            return Ok(ServiceFactory.TaskGraphService.GetAllForUser(UserId));
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add([FromBody]TaskGraphAddModel taskGraph)
        {
            var taskGraphData = new DataTransfer.Events.TaskGraph
            {
                Description = taskGraph.Description,
                Name = taskGraph.Name,
                Root = null,
                Id = Guid.NewGuid(),
                WorkspaceId = taskGraph.WorkspaceId,
                UserId = taskGraph.UserId
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
                EventType = "Accelerant.TaskGraphs.AddItem",
                Data = taskGraphData,
                EventTime = DateTime.Now,
                Subject = "accelerant-task-topic",
                DataVersion = "2.0"
            });

            client.PublishEventsAsync(topicHostname, eventsList).GetAwaiter().GetResult();

            return Ok(taskGraphData);
        }
    }
}