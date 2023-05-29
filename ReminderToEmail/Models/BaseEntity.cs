using System.ComponentModel.DataAnnotations;

namespace ReminderToEmail.Models
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }= Guid.NewGuid();

        public DateTime createAt { get; set; }= DateTime.Now;
    }
}
