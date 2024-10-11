namespace QueueManagementSystemAPI.DTOs
{
    public class AddTicketDto
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public IFormFile? PicturePath { get; set; }
        public string CreatedByUsername { get; set; }
    }
}
