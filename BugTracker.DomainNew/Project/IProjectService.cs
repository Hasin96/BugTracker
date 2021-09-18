using System.Threading.Tasks;

namespace BugTracker.Domain
{
    public interface IProjectService
    {
        Task<ProjectServiceResult> Create(string projectName);
    }
}
