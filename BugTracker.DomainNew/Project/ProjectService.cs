using System;
using System.Collections.Generic;
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

        public async Task<ProjectServiceResult> AddRequirementToProject(int projectId, Requirement requirement)
        {
            var project = await _repository.GetProjectWithRequirements(projectId);

            if (project is null)
                return new ProjectServiceResult(null, ProjectServiceCode.ProjectNotFound);

            project.Requirements.Add(requirement);

            await _repository.SaveAsync(project);

            return new ProjectServiceResult(project, ProjectServiceCode.Success);
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