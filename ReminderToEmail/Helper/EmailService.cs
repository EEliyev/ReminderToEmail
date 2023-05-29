using System.Net.Mail;

namespace ReminderToEmail.Helper
{
    public interface IEmailService
    {
        Task SendEmail(string to, string subject, string body);
    }
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public async Task SendEmail(string to, string subject, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("your-email@example.com"),
                Subject = subject,
                Body = body
            };

            mailMessage.To.Add(new MailAddress(to));

            _smtpClient.Send(mailMessage);
        }
    }
}
