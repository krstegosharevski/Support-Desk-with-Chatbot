namespace QueueManagementSystemAPI.DTOs
{
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public int? SelectedRole { get; set; }
    }
}
