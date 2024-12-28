namespace Project.BusinessLogic.Email
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string email, string message);
    }

    public class EmailService : IEmailService
    {
        public async Task SendConfirmationEmailAsync(string email, string message)
        {
            // Simulate sending an email
            await Task.Delay(1000); // Simulate some processing delay
            Console.WriteLine($"Email sent to {email} with message: {message}");
        }
    }

}
