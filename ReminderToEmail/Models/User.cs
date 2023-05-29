namespace ReminderToEmail.Models
{
    public class User:BaseEntity
    {
        public string email { get; set; }
        public byte [] password { get; set; }
        public ICollection<Reminder> reminders { get; set; }
    }
}
