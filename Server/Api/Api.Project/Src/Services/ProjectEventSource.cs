using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using FocusMark.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace FocusMark.Api.Services
{
    public class ProjectEventSource
    {
        private readonly IAmazonSimpleNotificationService snsService;
        private readonly ILogger<ProjectEventSource> logger;
        private readonly EventSourceConfiguration options;

        public ProjectEventSource(IAmazonSimpleNotificationService snsService, IOptions<EventSourceConfiguration> options, ILogger<ProjectEventSource> logger)
        {
            logger.LogDebug($"Enter {nameof(ProjectEventSource)} constructor");

            this.snsService = snsService ?? throw new ArgumentNullException(nameof(snsService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.options = options.Value ?? throw new ArgumentNullException(nameof(options));

            logger.LogDebug($"Leaving {nameof(ProjectEventSource)} constructor");
        }

        public async Task PublishNewProject(Project newProject)
        {
            var jsonPayload = JsonSerializer.Serialize(newProject);
            var request = new PublishRequest
            {
                Message = jsonPayload,
                TopicArn = this.options.Topics.ApiProjectArn
            };

            this.logger.LogInformation("Payload ready - publishing message");
            var response = await this.snsService.PublishAsync(request);
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                this.logger.LogError($"Publish to SNS Topic {request.TopicArn} with response code {response.HttpStatusCode} with payload {jsonPayload}");
                throw new EventSourcePublishFailureException(response.HttpStatusCode.ToString(), request.TopicArn);
            }

            this.logger.LogInformation("Message published");
        }
    }
}
