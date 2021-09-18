
using System.Collections.Generic;

namespace BugTracker.ViewModels
{
    public class ProjectIndexViewModel
    {
        public string ErrorMessage { get; set; }
        public IEnumerable<ProjectViewModel> Projects { get; set; }
    }
}
