namespace BugTracker.Domain
{
    public class ProjectServiceResult
    {

        public ProjectServiceResult(Project project, ProjectServiceCode code)
        {
            Result = project;
            Code = code;
        }

        public Project Result { get; set; }
        public ProjectServiceCode Code { get; set; }
    }

    public enum ProjectServiceCode
    {
        ErrorDuplicateProjectName = 1,
        Success = 2,
        ProjectNotFound = 3
    }
}