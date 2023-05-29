using System.ComponentModel.DataAnnotations;

namespace ReminderToEmail.Dtos.Request
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 8)]
        [Required]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$",ErrorMessage ="You need at least 8 chars. At least 1 number 1 letter")]
        public string Password { get; set; }
    }
}
