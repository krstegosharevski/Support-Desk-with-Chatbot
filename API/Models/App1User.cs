using Microsoft.AspNetCore.Identity;

namespace QueueManagementSystemAPI.Models
{
    public class App1User : IdentityUser<int>
    {

        public string? Name { get; set; }
        public string? LastName { get; set; }

        public bool Authenticated { get; set; }

        public ICollection<AppUserRole>? UserRoles { get; set; }

        public UserRoleEnum? SelectedRole { get; set; }


     
        public int? ProjectId { get; set; } 
        public Project? Project { get; set; }  

    }
}
