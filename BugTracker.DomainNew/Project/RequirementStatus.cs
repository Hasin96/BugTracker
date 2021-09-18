using System.ComponentModel;

namespace BugTracker.Domain
{
    public enum RequirementStatus
    {
        [Description("Not Started")]
        NotStarted = 1,
        [Description("Started")]
        Started = 2,
        [Description("Complete")]
        Complete = 3
    }
}
