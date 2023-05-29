using ReminderToEmail.Models;

namespace ReminderToEmail.Dtos.Response
{
    public class ReminderDto
    {
        public ReminderDto(Guid Id,string to,string content,DateTime sendAt,string method,Guid createBy, bool isSent)
        {
            this.Id = Id;
            this.to = to;
            this.content = content;
            this.sendAt = sendAt;
            this.method = method;
            this.createBy = createBy;
            this.isSent = isSent;

        }
        public Guid Id { get; set; }
        public string to { get; set; }
        public string content { get; set; }
        public DateTime sendAt { get; set; }
        public string method { get; set; }
        public Guid createBy { get; set; }
        public bool isSent { get; set; } = false;
    }
}
