using BugTracker.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.DataAccess
{
    public class ProjectRepository : IProjectRepository
    {
        private BugTrackerContext _context;

        public ProjectRepository(BugTrackerContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }

        public bool IsProjectNameADuplicate(string name)
            => _context.Projects.Any(p => p.Name.Equals(name));

        public async Task<Project> GetProjectWithRequirements(int projectId)
            => await _context.Projects.Include(p => p.Requirements).SingleOrDefaultAsync(p => p.Id == projectId);

        public async Task<IEnumerable<Project>> GetAll()
            => await _context.Projects.ToListAsync();

        public async Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }
    }
}