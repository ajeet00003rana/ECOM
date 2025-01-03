
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Project.BusinessLogic.Service.Email;
using System.Collections.Concurrent;

namespace Project.BusinessLogic.Service.BackgroundJobs
{
    public interface IEmailBackgroundService
    {
        void QueueEmail(string email, string message);
    }

    public class EmailBackgroundService : BackgroundService, IEmailBackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentQueue<(string Email, string Message)> _emailQueue = new ConcurrentQueue<(string, string)>();

        public EmailBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void QueueEmail(string email, string message)
        {
            _emailQueue.Enqueue((email, message));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_emailQueue.TryDequeue(out var emailData))
                {
                    using var scope = _serviceProvider.CreateScope();
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                    // Send email using EmailService
                    await emailService.SendConfirmationEmailAsync(emailData.Email, emailData.Message);
                }

                await Task.Delay(1000, stoppingToken); // Poll the queue every second
            }
        }
    }

}
