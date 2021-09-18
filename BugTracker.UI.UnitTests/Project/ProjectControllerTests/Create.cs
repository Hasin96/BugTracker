using BugTracker.Controllers;
using System;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using BugTracker.ViewModels;
using System.Threading.Tasks;
using BugTracker.Domain;
using Moq;
using System.Linq;

namespace BugTracker.UI.UnitTests
{
    public class Create
    {
        private Mock<IProjectService> _projectServiceMock;
        private Mock<IProjectRepository> _projectRepositoryMock;
        private readonly ProjectController _projectController;

        public Create()
        {
            _projectServiceMock = new Mock<IProjectService>();
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _projectController = new ProjectController(_projectServiceMock.Object, _projectRepositoryMock.Object);
        }
        [Fact]
        public void ShouldReturnViewModel_WithHeading()
        {
            var result = _projectController.Create();

            result.Should().BeOfType<ViewResult>();
            var model = (result as ViewResult).ViewData.Model;
            model.Should().BeOfType<ProjectFormViewModel>();
            (model as ProjectFormViewModel).Heading.Should().ContainAny("Add a");

        }

        [Fact]
        public async Task ShouldReturnView_WhenCalledWithModelStateError()
        {
            _projectController.ModelState.AddModelError("Name", "Required");
            var viewModel = new ProjectFormViewModel { Heading = "Add a project", ProjectName = "Test1" };

            var result = await _projectController.Create(viewModel);

            result.Should().BeOfType<ViewResult>();
            var viewResult = (result as ViewResult);
            var model = viewResult.ViewData.Model as ProjectFormViewModel;
            model.Heading.Should().Be(viewModel.Heading);
            model.ProjectName.Should().Be(viewModel.ProjectName);
        }

        [Fact]
        public async Task ShouldReturnView_WhenProjectNameIsDuplicate_WithErrorMessage()
        {
            var viewModel = new ProjectFormViewModel { Heading = "Add a project", ProjectName = "Test1" };
            _projectServiceMock.Setup(psm => psm.Create(It.IsAny<string>()))
                .Returns(Task.FromResult(new ProjectServiceResult(null, ProjectServiceCode.ErrorDuplicateProjectName)));

            var result = await _projectController.Create(viewModel);

            result.Should().BeOfType<ViewResult>();
            var viewResult = (result as ViewResult);
            var model = viewResult.ViewData.Model as ProjectFormViewModel;
            model.ErrorMessage.Should().Be("Project name is already in use");
        }


        [Fact]
        public async Task ShouldReturnRedirectToAction_WhenProjectNameIsNotDuplicate()
        {
            var viewModel = new ProjectFormViewModel { Heading = "Add a project", ProjectName = "Test1" };
            _projectServiceMock.Setup(psm => psm.Create(It.IsAny<string>()))
                .Returns(Task.FromResult(new ProjectServiceResult(new Project {Id = 1 }, ProjectServiceCode.Success)));

            var result = await _projectController.Create(viewModel);

            result.Should().BeOfType<RedirectToActionResult>();
            var redirectToActionResult = result.As<RedirectToActionResult>();
            redirectToActionResult.RouteValues.Keys.First().Should().Contain("id");
            redirectToActionResult.RouteValues.Values.First().Should().Be(1);

        }


    }
}
