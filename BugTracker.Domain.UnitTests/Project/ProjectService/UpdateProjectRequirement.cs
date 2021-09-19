using Xunit;
using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using System.Linq;

namespace BugTracker.Domain
{
    public class UpdateProjectRequirement
    {
        [Fact]
        public async Task ShouldUpdateProjectRequirementStatus()
        {
            int projectId = 1;
            int requirementId = 1;
            Project proj = new Project { Name = "test1", Status = ProjectStatus.NotStarted, Id = projectId };
            Requirement requirement = new Requirement { Id = requirementId, Description = "test1", ProjectId = 1, Status = RequirementStatus.NotStarted };
            proj.Requirements.Add(requirement);
            Requirement updatedRequirement = new Requirement { Id = requirementId, Description = "test1", ProjectId = 1, Status = RequirementStatus.Started };
            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(prm => prm.GetProjectWithRequirements(projectId))
                .Returns(Task.FromResult(proj));
            ProjectService service = new ProjectService(projectRepositoryMock.Object);

            Project project = await service.UpdateProjectRequirement(projectId, requirementId, updatedRequirement);

            project.Status.Should().Be(ProjectStatus.Started);
            project.Requirements.Count.Should().Be(project.Requirements.Count);
            project.Requirements.First().Status.Should().Be(RequirementStatus.Started);
        }
    }
}
