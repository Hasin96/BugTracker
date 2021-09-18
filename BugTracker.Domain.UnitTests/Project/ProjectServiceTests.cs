using Xunit;
using FluentAssertions;
using System;
using Moq;
using System.Threading.Tasks;

namespace BugTracker.Domain
{
    public class ProjectServiceTests
    {
        private Mock<IProjectRepository> _projectRepository;
        private ProjectService _projectService;

        public ProjectServiceTests()
        {
            _projectRepository = new Mock<IProjectRepository>();
            _projectService = new ProjectService(_projectRepository.Object);
        }

        [Fact]
        public async Task ProjectService_ShouldReturnNewProject_WithInitializtedValues()
        {
            var projectName = "BugTracker";

            var result = await _projectService.Create(projectName);
            var project = result.Result;

            project.Name.Should().Be(projectName);
            project.Status.Should().Be(ProjectStatus.NotStarted);
            project.Requirements.Count.Should().Be(0);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ProjectSerice_ShouldThrowNullException_WithInvalidName(string projectName)
        {
            Func<Task> act = async () => await _projectService.Create(projectName);

            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(act);

            exception.ParamName.Should().Be("Name");
        }

        [Fact]
        public async Task ProjectService_ShouldCallAddAndSaveChanges()
        {
            var projectName = "BugTracker";
            _projectRepository.Setup(x => x.SaveAsync(It.IsAny<Project>()));

            await _projectService.Create(projectName);
            
            _projectRepository.Verify(m => m.SaveAsync(It.IsAny<Project>()), Times.Exactly(1));
        }

        [Fact]
        public async Task ProjectService_ShouldReturnError_WhenProjectNameAlreadyExists()
        {
            var projectName = "BugTracker";
            _projectRepository.Setup(x => x.IsProjectNameADuplicate(It.IsAny<string>()))
                .Returns(true);

            ProjectServiceResult result = await _projectService.Create(projectName);

            result.Code.Should().Be(ProjectServiceCode.ErrorDuplicateProjectName);
            result.Result.Should().Be(null);
        }

        [Fact]
        public async Task ProjectService_ShouldReturnSuccess_WhenProjectNameIsUnique()
        {
            var projectName = "BugTracker";
            _projectRepository.Setup(x => x.IsProjectNameADuplicate(It.IsAny<string>()))
                .Returns(false);

            ProjectServiceResult result = await _projectService.Create(projectName);

            result.Code.Should().Be(ProjectServiceCode.Success);
            result.Result.Name.Should().Be(projectName);
        }
    }
}
