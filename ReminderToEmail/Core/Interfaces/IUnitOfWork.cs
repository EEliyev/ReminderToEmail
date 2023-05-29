namespace ReminderToEmail.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IReminderRepository reminderRepository { get; }
        IUserRepository userRepository { get; }
        Task saveAsync();
    }
}
