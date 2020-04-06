using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.XRay.Recorder.Core;
using FocusMark.Api.Services;
using FocusMark.Domain;
using FocusMark.Domain.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Shared;

namespace FocusMark.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly ProjectEventSource eventSource;

        public ProjectController(ProjectEventSource eventSource, ILogger<ProjectController> logger)
        {
            this.eventSource = eventSource ?? throw new ArgumentNullException(nameof(eventSource));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [RequiredCommand("command", "create-project")]
        public async Task<IActionResult> Post(CreateProjectRequest createProjectRequest)
        {
            // Build our model
            bool parseResults = Enum.TryParse(createProjectRequest.Priority, out Priority priority);
            if (!parseResults)
            {
                priority = Priority.None;
            }

            var project = new Project
            {
                DueDate = createProjectRequest.DueDate,
                Owner = "foo@bar.com",//base.User.Identity.Name,
                Path = createProjectRequest.Path ?? "/",
                PercentageCompleted = createProjectRequest.PercentageCompleted,
                Priority = priority,
                StartDate = createProjectRequest.StartDate,
                Title = createProjectRequest.Title
            };

            // Validate the business rules
            var validator = new ProjectValidator();
            var validationResults = validator.Validate(project);
            if (!validationResults.IsValid)
            {
                string[] errors = validationResults
                    .Errors
                    .Select(failure => $"Error {failure.ErrorCode} on Property '{failure.PropertyName}': {failure.ErrorMessage}")
                    .ToArray();

                string concatErrors = string.Join(". ", errors);
                this._logger.LogWarning($"Failed validation for {this.User.Identity.Name} with {concatErrors}");
                return base.BadRequest();
            }

            // Publish the new project
            AWSXRayRecorder.Instance.BeginSegment("Publish Create-Project");
            try
            {
                await this.eventSource.PublishNewProject(project);
            }
            catch (Exception ex)
            {
                AWSXRayRecorder.Instance.AddException(ex);
                return base.BadRequest();
            }
            finally
            {
                AWSXRayRecorder.Instance.EndSegment();
            }

            return Accepted(new { project });
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.Delay(1);
            return base.Ok(new { Id = Guid.NewGuid(), Title = "Hello" });
        }
    }
}
