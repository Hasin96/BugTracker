using System.Collections.Generic;

namespace BugTracker.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public IEnumerable<RequirementViewModel> Requirements { get; set; } = new List<RequirementViewModel>();
        public IEnumerable<RequirementStatusViewModel> RequirementStatuses { get; set; } = new List<RequirementStatusViewModel>();
    }
}
