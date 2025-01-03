using Microsoft.Extensions.DependencyInjection;
using Moq;
using Project.BusinessLogic.Service.BackgroundJobs;
using Project.BusinessLogic.Service.Email;


namespace BusinessLogic.Tests.Service.BackgroundJobs
{
    public class EmailBackgroundServiceTests
    {
        private readonly Mock<IServiceProvider> _mockServiceProvider;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly EmailBackgroundService _emailBackgroundService;

        public EmailBackgroundServiceTests()
        {
            _mockServiceProvider = new Mock<IServiceProvider>();
            _mockEmailService = new Mock<IEmailService>();

            var serviceScope = new Mock<IServiceScope>();
            var serviceScopeFactory = new Mock<IServiceScopeFactory>();

            serviceScope.Setup(x => x.ServiceProvider).Returns(_mockServiceProvider.Object);
            serviceScopeFactory.Setup(x => x.CreateScope()).Returns(serviceScope.Object);
            _mockServiceProvider.Setup(x => x.GetService(typeof(IServiceScopeFactory)))
                                .Returns(serviceScopeFactory.Object);
            _mockServiceProvider.Setup(x => x.GetService(typeof(IEmailService)))
                                .Returns(_mockEmailService.Object);

            _emailBackgroundService = new EmailBackgroundService(_mockServiceProvider.Object);
        }

        [Fact]
        public async Task QueueEmail_Should_Process_Queued_Emails()
        {
            // Arrange
            var email = "test@example.com";
            var message = "Test Message";

            // Act
            _emailBackgroundService.QueueEmail(email, message);

            // Start the background service in a separate task
            var cts = new CancellationTokenSource();
            var executeTask = _emailBackgroundService.StartAsync(cts.Token);

            // Allow some time for the email to be processed
            await Task.Delay(1500);

            // Stop the background service
            cts.Cancel();
            await executeTask;

            // Assert
            _mockEmailService.Verify(x => x.SendConfirmationEmailAsync(email, message), Times.Once);
        }
    }

}
