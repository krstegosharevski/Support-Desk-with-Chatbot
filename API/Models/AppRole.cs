using Microsoft.AspNetCore.Identity;

namespace QueueManagementSystemAPI.Models
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole>? UserRoles { get; set; }
    }
}
