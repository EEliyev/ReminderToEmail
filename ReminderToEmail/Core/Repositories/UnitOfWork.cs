using ReminderToEmail.Core.Interfaces;
using ReminderToEmail.Data;

namespace ReminderToEmail.Core.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ReminderContext _context;
        private readonly ILogger _logger;

        public IReminderRepository reminderRepository { get ; private set; }

        public IUserRepository userRepository { get; private set; }

        public UnitOfWork(ReminderContext context, ILogger<UnitOfWork> logger)
        {
            this._context = context;
            this._logger = logger;

            reminderRepository = new ReminderRepository(_context, _logger);
            userRepository= new UserRepository(_context, _logger);
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task saveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
