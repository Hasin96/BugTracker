using BugTracker.Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BugTracker.DataAccess
{
    public class ProjectRepositoryTests
    {
        private readonly DbContextOptions<BugTrackerContext> _options;

        public ProjectRepositoryTests()
        {
            var builder = new DbContextOptionsBuilder<BugTrackerContext>();
            builder.UseInMemoryDatabase(System.Guid.NewGuid().ToString());
            _options = builder.Options;
        }

        [Fact]
        public async Task ShouldSaveProject()
        {

            var project = new Project { Name = "test1", Status = ProjectStatus.NotStarted };
            using (var context = new BugTrackerContext(_options))
            {
                ProjectRepository repository = new ProjectRepository(context);
                await repository.SaveAsync(project);
            }


            Project projectToCheck;
            using (var context = new BugTrackerContext(_options))
            {
                projectToCheck = await context.Projects.Where(x => x.Name == project.Name).SingleOrDefaultAsync();
            }

            projectToCheck.Id.Should().Be(1);
            projectToCheck.Name.Should().Be(project.Name);
            projectToCheck.Status.Should().Be(project.Status);

        }

        [Fact]
        public async Task ShouldCheckForDuplicateProjectNames()
        {
            bool isDuplicate = false;

            var project = new Project { Name = "test1", Status = ProjectStatus.NotStarted };
            using (var context = new BugTrackerContext(_options))
            {
                ProjectRepository repository = new ProjectRepository(context);
                await repository.SaveAsync(project);
                isDuplicate = repository.IsProjectNameADuplicate(project.Name);
            }

            isDuplicate.Should().Be(true);
        }

        [Fact]
        public async Task ShouldGetProjectWithTasks()
        {
            var project = new Project { Name = "test1", Status = ProjectStatus.NotStarted };
            using (var context = new BugTrackerContext(_options))
            {
                ProjectRepository repository = new ProjectRepository(context);
                await repository.SaveAsync(project);
            }

            Project projectFromQuery;
            using (var context = new BugTrackerContext(_options))
            {
                ProjectRepository repository = new ProjectRepository(context);
                projectFromQuery = await repository.GetProjectWithRequirements(1);
            }

            project.Id.Should().Be(projectFromQuery.Id);
            project.Name.Should().Be(projectFromQuery.Name);
            project.Status.Should().Be(projectFromQuery.Status);
            project.Requirements.Count.Should().Be(projectFromQuery.Requirements.Count);
        }

        [Fact]
        public async Task ShouldGetProjectWithNewTask()
        {
            var project = new Project
            {
                Name = "test1",
                Status = ProjectStatus.NotStarted,
                Requirements = new List<Requirement>
                {
                    new Requirement
                    {
                        Status = RequirementStatus.NotStarted,
                        Description = "test1",
                    }
                }
            };
            using (var context = new BugTrackerContext(_options))
            {
                ProjectRepository repository = new ProjectRepository(context);
                await repository.SaveAsync(project);
            }

            project.Requirements.Add(new Requirement
            {
                Description = "test2",
                Status = RequirementStatus.NotStarted,
                ProjectId = project.Id
            });

            using (var context = new BugTrackerContext(_options))
            {
                ProjectRepository repository = new ProjectRepository(context);
                await repository.UpdateAsync(project);
            }

            Project projectFromQuery;
            using (var context = new BugTrackerContext(_options))
            {
                ProjectRepository repository = new ProjectRepository(context);
                projectFromQuery = await repository.GetProjectWithRequirements(1);
            }

            project.Id.Should().Be(projectFromQuery.Id);
            project.Name.Should().Be(projectFromQuery.Name);
            project.Status.Should().Be(projectFromQuery.Status);
            project.Requirements.Count.Should().Be(projectFromQuery.Requirements.Count);
        }

        [Fact]
        public async Task ShouldGetAllProjectsWithoutTasks()
        {
            var project = new Project { Name = "test1", Status = ProjectStatus.NotStarted };
            using (var context = new BugTrackerContext(_options))
            {
                ProjectRepository repository = new ProjectRepository(context);
                await repository.SaveAsync(project);
            }

            IEnumerable<Project> projectsFromQuery;
            using (var context = new BugTrackerContext(_options))
            {
                ProjectRepository repository = new ProjectRepository(context);
                projectsFromQuery = await repository.GetAll();
            }

            project.Id.Should().Be(projectsFromQuery.First().Id);
            project.Name.Should().Be(projectsFromQuery.First().Name);
            project.Status.Should().Be(projectsFromQuery.First().Status);
            project.Requirements.Count.Should().Be(projectsFromQuery.First().Requirements.Count);

        }
    }
}
