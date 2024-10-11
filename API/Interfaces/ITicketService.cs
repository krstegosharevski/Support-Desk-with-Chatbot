using QueueManagementSystemAPI.Models;

namespace QueueManagementSystemAPI.Interfaces
{
    public interface ITicketService
    {
        ICollection<Ticket> GetAllTickets();

        ICollection<Ticket> getTicketsCreatedByUser(int id);

        ICollection<Ticket> getTicketsAssignedToUser(int id);

        Ticket GetTicketById(int id);


        void UpdateTicket(Ticket ticket);

        void AddTicket(Ticket ticket);
    }
}
