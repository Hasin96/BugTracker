using BugTracker.Common;
using BugTracker.Domain;
using BugTracker.Models;
using BugTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Controllers
{
    public partial class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IProjectRepository _projectRepository;

        public ProjectController(IProjectService projectService, IProjectRepository projectRepository)
        {
            _projectService = projectService;
            _projectRepository = projectRepository;
        }

        public async Task<IActionResult> Index(string errorMsg = "")
        {
            var projects = await _projectRepository.GetAll();

            var viewModel = new ProjectIndexViewModel
            {
                ErrorMessage = errorMsg,
                Projects = projects.Select(p => new ProjectViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Status = p.Status.GetDescription()
                })
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            ProjectFormViewModel viewModel = new ProjectFormViewModel
            {
                Heading = "Add a Project"
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectFormViewModel projectVM)
        {
            if (!ModelState.IsValid)
                return View(projectVM);

            var serviceResponse = await _projectService.Create(projectVM.ProjectName);

            if (serviceResponse.Code == ProjectServiceCode.ErrorDuplicateProjectName)
            {
                projectVM.ErrorMessage = "Project name is already in use";

                return View(projectVM);
            }
            
            return RedirectToAction("Detail", new { id = serviceResponse.Result.Id });
        }

        public async Task<IActionResult> Detail(int id)
        {
            var project = await _projectRepository.GetProjectWithRequirements(id);
            if (project == null)
                return RedirectToAction("Index", new { errorMessage = "Project not found" });

            var viewModel = new ProjectViewModel
            {
                Name = project.Name,
                Status = project.Status.GetDescription(),
                Requirements = project.Requirements.Select(r => new RequirementViewModel
                {
                    Id = r.Id,
                    Description = r.Description,
                    Status = r.Status.GetDescription()
                }).ToList(),
                RequirementStatuses = Enum
                    .GetValues(typeof(RequirementStatus))
                    .Cast<RequirementStatus>()
                    .Select(requirementStatus => new RequirementStatusViewModel
                    {
                        Description = requirementStatus.GetDescription(),
                        Status = (int)requirementStatus
                    })
                    .ToList()
        };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

     
    }
}
