namespace BugTracker.Domain
{
    public class Requirement : Entity
    {
        public string Description { get; set; }
        public RequirementStatus Status { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
