using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QueueManagementSystemAPI.Data;
using QueueManagementSystemAPI.DTOs;
using QueueManagementSystemAPI.Interfaces;
using QueueManagementSystemAPI.Models;

namespace QueueManagementSystemAPI.Controllers
{
    public class TicketController : BaseApiController
    {
        private ITicketService _ticketService;
        private readonly ApplicationDbContext _context;

        public TicketController(ITicketService ticketService, ApplicationDbContext context)
        {
            _ticketService = ticketService;
            _context = context;
        }

        [HttpGet]
        public ActionResult<ICollection<Ticket>> getAllTickets()
        {
            return Ok(_ticketService.GetAllTickets());
        }

        [HttpGet("created/{id}")]
        public ActionResult<ICollection<Ticket>> getTicketsByUser(int id)
        {
            return Ok(_ticketService.getTicketsCreatedByUser(id));
        }

        [HttpGet("assigned/{id}")]
        public ActionResult<ICollection<Ticket>> getTicketsAssignedToUser(int id)
        {
            return Ok(_ticketService.getTicketsAssignedToUser(id));
        }

        [HttpGet("assigned/{ticketId}/{assignedId}")]
        public ActionResult<bool> getTicketsAssignedToUser(int ticketId, int assignedId)
        {
            // Пример: добиваме тикет од база преку сервис/репозиториум
            var ticket = _ticketService.GetTicketById(ticketId);

            // Проверуваме дали тикетот постои
            if (ticket == null)
            {
                return NotFound(false); // Ако тикетот не постои, враќаме NotFound со false
            }

            // Проверка дали assignedId е исто со assignedId на тикетот
            bool isAssigned = ticket.AssignedToId == assignedId;

            // Враќаме true ако е назначен, false ако не е
            return Ok(isAssigned);
        }

        [HttpPut("assigned/{ticketId}/{username}")]
        public async Task<IActionResult> setTicketAssignedToUser(int ticketId, string username)
        {
            // Пример: добиваме тикет од база преку сервис/репозиториум
            var ticket = _ticketService.GetTicketById(ticketId);

            // Проверуваме дали тикетот постои
            if (ticket == null)
            {
                return NotFound(false); // Ако тикетот не постои, враќаме NotFound со false
            }

            // Проверка дали assignedId е исто со assignedId на тикетот
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null) {
                return BadRequest("User not found");
            }

            ticket.AssignedToId = user.Id;


            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult<Ticket> GetTicketById(int id)
        {
            var ticket = _ticketService.GetTicketById(id);

            if (ticket == null)
            {
                return NotFound(); // Враќа статус 404 ако тикетот не е пронајден
            }

            return Ok(ticket);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddTicket([FromForm] AddTicketDto ticketDto)
        {
            if (ticketDto == null)
            {
                return BadRequest("No ticket to be added!");
            }

            string username = ticketDto.CreatedByUsername;

            var user = _context.Users.FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return BadRequest($"User with username '{ticketDto.CreatedByUsername}' not found.");
            }

            string picturePath = null;

            if (ticketDto.PicturePath != null && ticketDto.PicturePath.Length > 0)
            {
                var folderPath = @"E:\Vo tek Aplickacii Firma\pictures";
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(ticketDto.PicturePath.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ticketDto.PicturePath.CopyToAsync(stream);
                }

                // Зачувувај релативна патека до сликата, која ќе биде достапна преку "/images/"
                picturePath = $"/images/{fileName}";
            }

            var newTicket = new Ticket
            {
                Title = ticketDto.Title,
                Message = ticketDto.Message,
                PicturePath = picturePath, // Релативна патека
                CreatedById = user.Id,
                GeneratedTime = DateTime.Now,
                Status = TicketStatus.ASSIGNED
            };

            _ticketService.AddTicket(newTicket);

            return Ok();
        }


        //Ова работи со слика.
        //[HttpPost("add")]
        //public async Task<IActionResult> AddTicket([FromForm] AddTicketDto ticketDto)
        //{
        //    if (ticketDto == null)
        //    {
        //        return BadRequest("No ticket to be added!");
        //    }

        //    string username = ticketDto.CreatedByUsername;

        //    var user = _context.Users.FirstOrDefault(u => u.UserName == username);

        //    if (user == null)
        //    {
        //        return BadRequest($"User with username '{ticketDto.CreatedByUsername}' not found.");
        //    }

        //    string picturePath = null;

        //    if (ticketDto.PicturePath != null && ticketDto.PicturePath.Length > 0)
        //    {
        //        var folderPath = @"E:\Vo tek Aplickacii Firma\pictures";
        //        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(ticketDto.PicturePath.FileName)}";
        //        var filePath = Path.Combine(folderPath, fileName);

        //        if (!Directory.Exists(folderPath))
        //        {
        //            Directory.CreateDirectory(folderPath);
        //        }

        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await ticketDto.PicturePath.CopyToAsync(stream);
        //        }

        //        picturePath = filePath;
        //    }

        //    var newTicket = new Ticket
        //    {
        //        Title = ticketDto.Title,
        //        Message = ticketDto.Message,
        //        PicturePath = picturePath,
        //        CreatedById = user.Id,
        //        GeneratedTime = DateTime.Now,
        //        Status = TicketStatus.ASSIGNED
        //    };

        //    _ticketService.AddTicket(newTicket);

        //    return Ok();
        //}



        //Bez slika
        //[HttpPost("add")]
        //public IActionResult AddTicket([FromBody] AddTicketDto ticketDto)
        //{
        //    if (ticketDto == null)
        //    {
        //        return BadRequest("No ticket to be added!");
        //    }

        //    var user = _context.Users.FirstOrDefault(u => u.UserName == ticketDto.CreatedByUsername);

        //    if (user == null)
        //    {
        //        return BadRequest("User not found");
        //    }

        //    string picturePath = null;

        //    if (ticketDto.PicturePath == null)
        //    {
        //        picturePath = "https//localhost:2000";
        //    }
        //    else
        //    {
        //        picturePath = ticketDto.PicturePath;
        //    }

        //    if (ticketDto.PicturePath != null && ticketDto.PicturePath.Length > 0)
        //    {
        //        var folderPath = @"E:\Vo tek Aplickacii Firma\pictures";
        //        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(ticketDto.PicturePath.FileName)}";
        //        var filePath = Path.Combine(folderPath, fileName);

        //        // Се креира директорија ако не постои
        //        if (!Directory.Exists(folderPath))
        //        {
        //            Directory.CreateDirectory(folderPath);
        //        }

        //        // Чување на сликата на дискот
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await ticketDto.PicturePath.CopyToAsync(stream);
        //        }

        //        picturePath = filePath;
        //    }

        //    var newTicket = new Ticket
        //    {
        //        Title = ticketDto.Title,
        //        Message = ticketDto.Message,
        //        PicturePath = picturePath,
        //        CreatedById = user.Id,
        //        GeneratedTime = DateTime.Now,
        //        Status = TicketStatus.ASSIGNED
        //    };

        //    _ticketService.AddTicket(newTicket);

        //    return Ok();

        //}



        [HttpPut("{id}")]
        public IActionResult UpdateTicket(int id, [FromBody] UpdateTicketDto ticketDto)
        {
            var existingTicket = _ticketService.GetTicketById(id);

            if (existingTicket == null)
            {
                return NotFound();
            }

            // Ажурирајте ги само потребните полиња
            if(ticketDto.Status == 0)
            {
                existingTicket.Status = TicketStatus.ASSIGNED;
            }else if(ticketDto.Status == 1)
            {
                existingTicket.Status = TicketStatus.PROGRESS;
            }else existingTicket.Status = TicketStatus.DONE;

            existingTicket.Response = ticketDto.Response;
            existingTicket.FinishTime = ticketDto.Status == 2 ? DateTime.Now : existingTicket.FinishTime;

            _ticketService.UpdateTicket(existingTicket);

            return Ok();
        }


        [HttpGet("find/{id}")]
        public ActionResult<UserUsernameDto> FindUsertById(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            UserUsernameDto username =  new UserUsernameDto();
            username.Username = user.UserName;

            username.Username = username.Username.ToUpper();

            return Ok(username);
        }

        [HttpGet("findAll")]
        public ActionResult<IEnumerable<GetUsernameByIdDto>> GetAllUsers()
        {
            var users = _context.Users.Select(u => new GetUsernameByIdDto
            {
                Id = u.Id,
                Username = u.UserName.ToUpper()
            }).ToList();

            if (users == null || !users.Any())
            {
                return NotFound("No users found");
            }

            return Ok(users);
        }
    }
}
