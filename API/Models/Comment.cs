namespace QueueManagementSystemAPI.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }

        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public int UserId { get; set; }
        public App1User User { get; set; }

    }
}
