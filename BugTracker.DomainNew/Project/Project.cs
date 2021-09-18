using System.Collections.Generic;

namespace BugTracker.Domain
{
    public class Project : Entity
    {
        public Project()
        {
            Requirements = new List<Requirement>();
        }
        public string Name { get; set; }
        public ProjectStatus Status { get; set; }
        public List<Requirement> Requirements { get; set; }
    }
}
