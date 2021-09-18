using BugTracker.Controllers;
using BugTracker.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using BugTracker.Common;
using BugTracker.ViewModels;
using System;

namespace BugTracker.UI.UnitTests
{
    public class Detail
    {
        [Fact]
        public async Task ShouldReturnRedirectToAction_WithErrorMessage()
        {
            var projectServiceMock = new Mock<IProjectService>();
            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(prm => prm.GetProjectWithRequirements(It.IsAny<int>()))
                .Returns(Task.FromResult((Project)null));
            var projectController = new ProjectController(projectServiceMock.Object, projectRepositoryMock.Object);

            var result = await projectController.Detail(1);

            result.Should().BeOfType<RedirectToActionResult>();
            var redirectToActionResult = result.As<RedirectToActionResult>();
            redirectToActionResult.RouteValues.Keys.First().Should().Contain("errorMessage");
            redirectToActionResult.RouteValues.Values.First().Should().Be("Project not found");
        }

        [Fact]
        public async Task ShouldReturnView_WithViewModel()
        {
            var project = new Project
            {
                Id = 1,
                Name = "test1",
                Status = ProjectStatus.NotStarted,
            };

            var requirements = new List<Requirement>
            {
                new Requirement {Id = 1, Status = RequirementStatus.NotStarted, Description = "lel"},
            };

            project.Requirements = requirements;

            var projectServiceMock = new Mock<IProjectService>();
            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(prm => prm.GetProjectWithRequirements(It.IsAny<int>()))
                .Returns(Task.FromResult(project));
            var projectController = new ProjectController(projectServiceMock.Object, projectRepositoryMock.Object);

            IEnumerable<RequirementStatusViewModel> requirementStatuses = Enum
                .GetValues(typeof(RequirementStatus))
                .Cast<RequirementStatus>()
                .Select(requirementStatus => new RequirementStatusViewModel 
                { 
                    Description = requirementStatus.GetDescription(),
                    Status = (int)requirementStatus
                })
                .ToList();

            var result = await projectController.Detail(1);

            result.Should().BeOfType<ViewResult>();
            var viewResult = (result as ViewResult);
            var model = viewResult.ViewData.Model as ProjectViewModel;
            model.Name.Should().Be(project.Name);
            model.Status.Should().Be(ProjectStatus.NotStarted.GetDescription());
            model.Requirements.First().Status.Should().Be(RequirementStatus.NotStarted.GetDescription());
            model.Requirements.First().Description.Should().Be(requirements.First().Description);
            model.Requirements.First().Id.Should().Be(requirements.First().Id);

            model.RequirementStatuses.Count().Should().Be(requirementStatuses.Count());
            model.RequirementStatuses.ElementAt(0).Description.Should().Be(requirementStatuses.ElementAt(0).Description);
            model.RequirementStatuses.ElementAt(0).Status.Should().Be(requirementStatuses.ElementAt(0).Status);
            model.RequirementStatuses.ElementAt(1).Description.Should().Be(requirementStatuses.ElementAt(1).Description);
            model.RequirementStatuses.ElementAt(1).Status.Should().Be(requirementStatuses.ElementAt(1).Status);
            model.RequirementStatuses.ElementAt(2).Description.Should().Be(requirementStatuses.ElementAt(2).Description);
            model.RequirementStatuses.ElementAt(2).Status.Should().Be(requirementStatuses.ElementAt(2).Status);
        }
    }
}
