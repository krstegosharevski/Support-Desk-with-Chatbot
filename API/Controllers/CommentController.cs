using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QueueManagementSystemAPI.Data;
using QueueManagementSystemAPI.DTOs;
using QueueManagementSystemAPI.Models;

namespace QueueManagementSystemAPI.Controllers
{
    public class CommentController : BaseApiController
    {
        private readonly ApplicationDbContext _context;

        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("add-comment")]
        public ActionResult AddComment([FromBody] CommentDto commentDto)
        {
            var ticket = _context.Ticket.Include(t => t.Comments).FirstOrDefault(t => t.TicketId == commentDto.TicketId);
            if (ticket == null)
            {
                return NotFound("Ticket not found");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == commentDto.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var comment = new Comment
            {
                CommentText = commentDto.CommentText,
                CreatedAt = DateTime.Now,
                TicketId = commentDto.TicketId,
                UserId = commentDto.UserId,
                Ticket = ticket,
                User = user
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return Ok(comment);
        }

        [HttpGet("{ticketId}")]
        public ActionResult<IEnumerable<CommentDto>> GetCommentsByTicketId(int ticketId)
        {
            var comments = _context.Comments
                .Where(c => c.TicketId == ticketId)
                .Select(c => new CommentDto
                {
                    CommentText = c.CommentText,
                    CreatedAt = c.CreatedAt,
                    UserId = c.UserId,
                    TicketId = c.TicketId
                }).ToList();

            return Ok(comments);
        }


    }
}
