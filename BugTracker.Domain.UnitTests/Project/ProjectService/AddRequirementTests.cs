using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BugTracker.Domain
{
    public class AddRequirementTests
    {
        private const int _projectId = 1;
        private readonly Requirement _newRequirement;
        private Mock<IProjectRepository> _projectRepository;
        private IProjectService _projectService;

        public AddRequirementTests()
        {
            _newRequirement = new Requirement { Description = "test1", Status = RequirementStatus.NotStarted, ProjectId = 1 };
            _projectRepository = new Mock<IProjectRepository>();
            _projectRepository.Setup(x => x.GetProjectWithRequirements(1))
                .Returns(Task.FromResult(new Project() ));
            _projectService = new ProjectService(_projectRepository.Object);
        }

        [Fact]
        public async Task ShouldCallAddRequirementToProject()
        {
            ProjectServiceResult serviceResult =  await _projectService.AddRequirementToProject(_projectId, _newRequirement.Description);

            serviceResult.Result.Requirements.Count.Should().Be(1);
            serviceResult.Result.Requirements.First().Description = "test1";
            serviceResult.Result.Requirements.First().Status = RequirementStatus.NotStarted;
            serviceResult.Result.Requirements.First().ProjectId = _projectId;
            serviceResult.Code.Should().Be(ProjectServiceCode.Success);
        }

        [Fact]
        public async Task ShouldCallSaveChanges()
        {
            await _projectService.AddRequirementToProject(_projectId, _newRequirement.Description);

            _projectRepository.Verify(m => m.SaveAsync(It.IsAny<Project>()), Times.Exactly(1));
        }

        [Fact]
        public async Task ShouldReturnError_WhenProjectNameAlreadyExists()
        {
            _projectRepository.Setup(x => x.GetProjectWithRequirements(It.IsAny<int>()))
                .Returns(Task.FromResult((Project)null));

            ProjectServiceResult result = await _projectService.AddRequirementToProject(_projectId, _newRequirement.Description);

            result.Code.Should().Be(ProjectServiceCode.ProjectNotFound);
            result.Result.Should().Be(null);
        }
    }
}
