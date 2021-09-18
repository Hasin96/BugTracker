using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using BugTracker.ViewModels;
using BugTracker.Controllers;
using BugTracker.Domain;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using BugTracker.Common;

namespace BugTracker.UI.UnitTests
{
    public class Index
    {
        [Fact]
        public async Task ShouldReturnView_WithViewModelHeadingPropertyAndProjects()
        {
            var viewModel = new ProjectFormViewModel { Heading = "Add a project" };
            IEnumerable<Project> projects = new List<Project>
            {
                new Project {  Name = "test", Requirements = new List<Requirement>(), Status = ProjectStatus.NotStarted }
            };
            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(prm => prm.GetAll())
                .Returns(Task.FromResult(projects));
            var projectServiceMock = new Mock<IProjectService>();

            var projectController = new ProjectController(projectServiceMock.Object, projectRepositoryMock.Object);

            var result = await projectController.Index("Failure");

            result.Should().BeOfType <ViewResult>();
            var viewResult = result.As<ViewResult>();
            var model = viewResult.ViewData.Model as ProjectIndexViewModel;
            model.ErrorMessage.Should().Be("Failure");
            model.Projects.Count().Should().Be(1);
            model.Projects.First().Name.Should().Be(projects.First().Name);
            model.Projects.First().Requirements.Count().Should().Be(projects.First().Requirements.Count());
            model.Projects.First().Status.Should().Be(projects.First().Status.GetDescription());
        }
    }
}
