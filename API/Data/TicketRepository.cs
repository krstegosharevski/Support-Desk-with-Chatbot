using Microsoft.AspNetCore.Mvc;
using QueueManagementSystemAPI.Interfaces;
using QueueManagementSystemAPI.Models;

namespace QueueManagementSystemAPI.Data
{
    public class TicketRepository : ITicketService
    {
        private readonly ApplicationDbContext _context;

        public TicketRepository(ApplicationDbContext context)
        {

            _context = context; 
        }

      

        public ICollection<Ticket> GetAllTickets()
        {
            return _context.Ticket.ToList();
        }

        public Ticket GetTicketById(int id)
        {
            return _context.Ticket.FirstOrDefault(ul => ul.TicketId == id);
        }

        public void UpdateTicket(Ticket ticket)
        {
            _context.Ticket.Update(ticket);
            _context.SaveChanges();
        }

        public void AddTicket(Ticket ticket)
        {
            _context.Ticket.Add(ticket);
            _context.SaveChanges();
        }

        public ICollection<Ticket> getTicketsCreatedByUser(int id)
        {
            return _context.Ticket
                .Where(t => t.CreatedById == id)
                .ToList();
        }

        public ICollection<Ticket> getTicketsAssignedToUser(int id)
        {
            return _context.Ticket
                .Where(t => t.AssignedToId == id)
                .ToList();
        }
    }
}



