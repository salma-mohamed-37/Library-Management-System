using backend.Models;

namespace backend.Interfaces
{
    public interface IUserRepository: IAsyncRepository<ApplicationUser>
    {
    }
}
