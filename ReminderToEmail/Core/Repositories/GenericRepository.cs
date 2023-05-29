using Microsoft.EntityFrameworkCore;
using ReminderToEmail.Core.Interfaces;
using ReminderToEmail.Data;
using ReminderToEmail.Models;

namespace ReminderToEmail.Core.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ReminderContext _context;
        private readonly ILogger _logger;

        public GenericRepository(ReminderContext context,ILogger logger)
        {
            this._logger = logger;
            this._context = context;
        }
        public async Task<bool> Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return true;
        }

        public async Task<bool> Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            return true;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(Guid id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return true;
        }
    }
}
