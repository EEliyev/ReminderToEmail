using ReminderToEmail.Models;

namespace ReminderToEmail.Core.Interfaces
{
    public interface IUserRepository:IGenericRepository<User>
    {
        Task<User> GetByEmail(string email);
    }
}
