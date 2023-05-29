using ReminderToEmail.Dtos.Response;
using ReminderToEmail.Models;

namespace ReminderToEmail.Helper
{
    public static class Extensions
    {
        public static ReminderDto AsDto(this Reminder item)
        {
            return new ReminderDto(item.Id,item.to,item.content,item.sendAt,item.method,item.createBy,item.isSent);
        }
    }
}
