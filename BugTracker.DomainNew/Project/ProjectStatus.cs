using System.ComponentModel;

namespace BugTracker.Domain
{
    public enum ProjectStatus
    {
        [Description("Backlog")]
        NotStarted = 1,
        Started = 2,
        Complete = 3
    }
}
