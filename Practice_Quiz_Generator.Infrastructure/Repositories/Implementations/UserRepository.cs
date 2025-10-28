using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Implementations
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(ExamPortalContext context) : base(context) { }

        public async Task<User> FindUserById(string id)
        {
            return await FindByCondition(u => u.Id == id, false)
                .FirstOrDefaultAsync();
        }

        public async Task<User> FindUserByEmail(string email)
        {
            return await FindByCondition(u => u.Email == email, false)
                 .FirstOrDefaultAsync();
        }

        public void AttachAsUnchanged(User user)
        {
            if (user != null)
            {
                _context.Entry(user).State = EntityState.Unchanged;
            }
        }
    }
}
