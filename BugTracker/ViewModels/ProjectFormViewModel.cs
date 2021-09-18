using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.ViewModels
{
    public class ProjectFormViewModel
    {
        public string Heading { get; set; }
        [Required]
        [MaxLength(55)]
        public string ProjectName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
