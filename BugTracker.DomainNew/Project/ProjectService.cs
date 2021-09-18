using System;
using System.Threading.Tasks;

namespace BugTracker.Domain
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repository;

        public ProjectService(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProjectServiceResult> Create(string projectName)
        {
            if (string.IsNullOrEmpty(projectName))
                    throw new ArgumentNullException("Name");

            if (_repository.IsProjectNameADuplicate(projectName))
                return new ProjectServiceResult(null, ProjectServiceCode.ErrorDuplicateProjectName);

            var project = new Project
            {
                Name = projectName,
                Status = ProjectStatus.NotStarted,
            };

            await _repository.SaveAsync(project);

            return new ProjectServiceResult(project, ProjectServiceCode.Success);
        }
    }

   
}