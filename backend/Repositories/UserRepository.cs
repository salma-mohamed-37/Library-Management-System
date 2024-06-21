using backend.Data;
using backend.Interfaces;
using backend.Models;

namespace backend.Repositories
{
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

    }
}
