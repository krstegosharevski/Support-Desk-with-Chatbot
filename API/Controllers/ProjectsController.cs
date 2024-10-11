using Microsoft.AspNetCore.Mvc;
using QueueManagementSystemAPI.Data;
using QueueManagementSystemAPI.DTOs;
using QueueManagementSystemAPI.Interfaces;
using QueueManagementSystemAPI.Models;

namespace QueueManagementSystemAPI.Controllers
{
    public class ProjectsController : BaseApiController
    {

        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {

            _context = context;
        }

        [HttpGet]
        public ActionResult<ICollection<ProjectDto>> GetAllProjects()
        {
            var projectDtos = _context.Projects
                .Select(p => new ProjectDto
                {
                    Id = p.Id,
                    ProjectName = p.ProjectName
                })
                .ToList();

            return Ok(projectDtos);
        }
    }
}
