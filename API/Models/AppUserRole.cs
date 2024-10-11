using Microsoft.AspNetCore.Identity;

namespace QueueManagementSystemAPI.Models
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public App1User? User { get; set; }
        public AppRole? Role { get; set; }

    }
}
