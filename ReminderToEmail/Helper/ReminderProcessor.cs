using ReminderToEmail.Core.Interfaces;
using ReminderToEmail.Data;

namespace ReminderToEmail.Helper
{
    public class ReminderProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ReminderProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                        var emailsender = scope.ServiceProvider.GetRequiredService<IEmailService>();

                        var reminders = await unitOfWork.reminderRepository.GetAll();

                        reminders = reminders.Where(x => x.isSent == false&&x.sendAt<DateTime.Now);

                        foreach (var item in reminders)
                        {
                            await emailsender.SendEmail(item.to, "FSCode", $"You have test message: \n {item.content} ");

                            item.isSent = true;
                            await unitOfWork.reminderRepository.Update(item);
                            await unitOfWork.saveAsync();
                        }


                    }
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
             

                await Task.Delay(TimeSpan.FromSeconds(40), stoppingToken);
            }
        }
    }
}
