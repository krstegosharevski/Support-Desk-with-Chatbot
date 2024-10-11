using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QueueManagementSystemAPI.Data;
using QueueManagementSystemAPI.DTOs;
using QueueManagementSystemAPI.Models;

namespace QueueManagementSystemAPI.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<App1User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<App1User>> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        [HttpGet("get-id")]
        public ActionResult<int> GetUserId([FromQuery] string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return NotFound("User with username " + username + " not found");
            }
            return Ok(user.Id);
        }

        [HttpGet("get-role")]
        public ActionResult<int> GetUserRole([FromQuery] string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return NotFound("User with username " + username + " not found");
            }
            if (user.SelectedRole == null)
            {
                return BadRequest("Not selected role!");
            }
            return Ok(user.SelectedRole);
        }

        [HttpGet("get-not/authenticated")]
        public async Task<ActionResult<IEnumerable<App1User>>> GetNotAuthenticatedUsers()
        {
            var users = await _context.Users
                .Where(t => t.Authenticated == false)
                .Select(user => new UserNotAuthenticated
                    {
                        Name = user.Name,
                        Surname = user.LastName,
                        UserName = user.UserName
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("get/authenticated")]
        public async Task<ActionResult<IEnumerable<App1User>>> GetAuthenticatedUsers()
        {
            var users = await _context.Users
                .Where(t => t.Authenticated == true)
                .Select(user => new UserNotAuthenticated
                {
                    Name = user.Name,
                    Surname = user.LastName,
                    UserName = user.UserName
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpPut("authorize/{username}/{projectId}")]
        public async Task<IActionResult> AuthorizeUser(string username, int projectId)
        {
            // Пронаоѓање на корисникот по username
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == username);

            // Проверка дали корисникот постои
            if (user == null)
            {
                return NotFound(); // Ако корисникот не постои, враќа NotFound
            }

            // Проверка дали корисникот веќе е аутентициран
            if (user.Authenticated)
            {
                return BadRequest("User is already authenticated"); // Ако е веќе аутентициран, враќа BadRequest
            }

            // Промена на Authenticated во true
            user.Authenticated = true;
            user.ProjectId = projectId;

            // Зачувување на промените во базата податоци
            await _context.SaveChangesAsync();

            return NoContent(); // Враќа NoContent по успешно ажурирање
        }


    }
}
