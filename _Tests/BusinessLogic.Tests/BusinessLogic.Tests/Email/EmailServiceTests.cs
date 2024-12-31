using Project.BusinessLogic.Email;

namespace BusinessLogic.Tests.Email
{
    public class EmailServiceTests
    {
        private readonly EmailService _emailService;

        public EmailServiceTests()
        {
            _emailService = new EmailService();
        }

        [Fact]
        public async Task SendConfirmationEmailAsync_ShouldWriteCorrectMessageToConsole()
        {
            // Arrange
            var email = "test@example.com";
            var message = "Test Message";
            var expectedOutput = $"Email sent to {email} with message: {message}{Environment.NewLine}";

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                await _emailService.SendConfirmationEmailAsync(email, message);

                // Assert
                var result = sw.ToString();
                Assert.Equal(expectedOutput, result);
            }
        }
    }

}
