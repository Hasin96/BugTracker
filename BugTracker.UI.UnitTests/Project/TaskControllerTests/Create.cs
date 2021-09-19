using Xunit;
using FluentAssertions;
using BugTracker.Domain;
using BugTracker.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Linq;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BugTracker.UI.UnitTests.TaskControllerTests
{
    public class Create
    {
        [Fact]
        public async Task ShouldAddNewRequirementToProject()
        {
            int projectId = 1;
            string requirement = "Add A FOOTER WHICH SHOWS THE STATUS";
            var proj = new Project
            {
                Id = projectId,
                Requirements = new List<Requirement>
                {
                    new Requirement
                    {
                        Description = requirement,
                        Status = RequirementStatus.NotStarted,
                        ProjectId = projectId
                    }
                }
            };
            var projectServiceResult = new ProjectServiceResult(proj, ProjectServiceCode.Success);
            var projectSerivceMock = new Mock<IProjectService>();
            projectSerivceMock.Setup(psm => psm.AddRequirementToProject(projectId, requirement))
                .Returns(Task.FromResult(projectServiceResult));
            

            var taskController = new TasksApiController(projectSerivceMock.Object);

            var result = await taskController.Create(projectId, requirement);
            var r = result.As<OkObjectResult>();
            r.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var project = r.Value.As<Project>();
                          
            project.Id.Should().Be(projectId);
            project.Requirements.First().Description.Should().Be(requirement);
        }
        
    }
}
