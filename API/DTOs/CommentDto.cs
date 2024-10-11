namespace QueueManagementSystemAPI.DTOs
{
    public class CommentDto
    {
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public string CommentText { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
