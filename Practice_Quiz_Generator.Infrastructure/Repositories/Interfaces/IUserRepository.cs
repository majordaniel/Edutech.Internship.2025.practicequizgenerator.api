using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.Repositories.Implementations;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        //Task<User> FindUserByEmail(string email);
        //Task<User> FindUserById(string id);

        Task<User> FindUserByEmail(string email);
        Task<User> FindUserById(string id);

    }
}
