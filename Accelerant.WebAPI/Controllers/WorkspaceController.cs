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
    [Route("workspace")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class WorkspaceController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromQuery]Guid WorkspaceId, [FromQuery]Guid UserId)
        {
            var workspace = ServiceFactory.WorkspaceService.Get(WorkspaceId);
            if (workspace.UserId != UserId)
                return new NotFoundResult();
            return Ok(workspace);
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAllForUser([FromQuery]Guid UserId)
        {
            return Ok(ServiceFactory.WorkspaceService.GetAllForUser(UserId));
        }

        [HttpPost]
        [Route("add")] 
        public IActionResult Add([FromBody]WorkspaceAddModel workSpace)
        {
            var workSpaceData = new Accelerant.DataTransfer.Events.Workspace
            {
                Description = workSpace.Description,
                Id = Guid.NewGuid(),
                Name = workSpace.Name,
                TaskGraphIds = null,
                UserId = workSpace.UserId
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
                EventType = "Accelerant.Workspaces.AddItem",
                Data = workSpaceData,
                EventTime = DateTime.Now,
                Subject = "accelerant-task-topic",
                DataVersion = "2.0"
            });

            client.PublishEventsAsync(topicHostname, eventsList).GetAwaiter().GetResult();
            return Ok(workSpaceData);
        }
    }
}
