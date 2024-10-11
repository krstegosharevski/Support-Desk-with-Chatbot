using QueueManagementSystemAPI.Models;

namespace QueueManagementSystemAPI.DTOs
{
    public class UpdateTicketDto
    {
        public int TicketId { get; set; }
        public string? Response { get; set; }
        public int? Status { get; set; }
    }
}
