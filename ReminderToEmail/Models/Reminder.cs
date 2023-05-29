namespace ReminderToEmail.Models
{
    public class Reminder:BaseEntity
    {
        public string to { get; set; }
        public string content { get; set; }
        public DateTime sendAt { get; set; }
        public string method { get; set; }
        public Guid createBy { get; set; }
        public User user { get; set; }
        public bool isSent { get; set; } = false;
    }
}
