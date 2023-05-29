using Microsoft.EntityFrameworkCore;
using ReminderToEmail.Core.Interfaces;
using ReminderToEmail.Data;
using ReminderToEmail.Models;

namespace ReminderToEmail.Core.Repositories
{
    public class UserRepository : GenericRepository<User>,IUserRepository
    {
        private readonly ReminderContext context;
        private readonly ILogger logger;

        public UserRepository(ReminderContext context, ILogger logger) : base(context, logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<User> GetByEmail(string email)
        {
            return await context.Set<User>().FirstOrDefaultAsync(x=>x.email==email);
        }
    }
}
