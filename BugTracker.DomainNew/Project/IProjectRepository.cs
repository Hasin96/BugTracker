using System.Collections.Generic;
using System.Threading.Tasks;

namespace BugTracker.Domain
{
    public interface IProjectRepository
    {
        Task SaveAsync(Project project);
        bool IsProjectNameADuplicate(string projectName);
        Task<Project> GetProjectWithRequirements(int projectId);
        Task<IEnumerable<Project>> GetAll();
    }
}