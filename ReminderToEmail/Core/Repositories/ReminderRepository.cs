using ReminderToEmail.Core.Interfaces;
using ReminderToEmail.Data;
using ReminderToEmail.Models;

namespace ReminderToEmail.Core.Repositories
{
    public class ReminderRepository : GenericRepository<Reminder>,IReminderRepository
    {
        public ReminderRepository(ReminderContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
