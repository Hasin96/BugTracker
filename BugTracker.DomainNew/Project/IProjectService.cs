using System.Threading.Tasks;

namespace BugTracker.Domain
{
    public interface IProjectService
    {
        Task<ProjectServiceResult> Create(string projectName);
        Task<ProjectServiceResult> AddRequirementToProject(int projectId, string requirement);
        Task<Project> UpdateProjectRequirement(int projectId, int requirementId, Requirement updatedRequirement);
    }
}
