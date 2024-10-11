using System.Text.Json.Serialization;

namespace QueueManagementSystemAPI.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string? Response { get; set; }
        public string PicturePath { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime GeneratedTime { get; set; }
        public DateTime? FinishTime { get; set; }

        //Kreator
        public App1User CreatedBy { get; set; }
        public int CreatedById { get; set; }

        //Dodelen 
        public int? AssignedToId { get; set; }
        public App1User? AssignedTo { get; set; }

        [JsonIgnore]
        public ICollection<Comment>? Comments { get; set; }

    }
}
