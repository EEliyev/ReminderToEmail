using System.ComponentModel.DataAnnotations;

namespace ReminderToEmail.Dtos.Request
{
    public class ReminderDto: IValidatableObject
    {
        [Required]
        [EmailAddress]
        public string to { get; set; }
        [Required]
        public string content { get; set; }
        [Required]
        public DateTime sendAt { get; set; }
        [Required]
        public string method { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (sendAt <=DateTime.Now)
            {
                yield return new ValidationResult("SendAt must be greater than now");
            }
            if(method.ToLower()!="email" && method.ToLower() != "telegram")
            {
                yield return new ValidationResult("method can be \"email\" or \"telegram\"");
            }
        }
    }
}
