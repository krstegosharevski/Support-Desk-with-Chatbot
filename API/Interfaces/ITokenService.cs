using QueueManagementSystemAPI.Models;

namespace QueueManagementSystemAPI.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(App1User user);
    }
}
