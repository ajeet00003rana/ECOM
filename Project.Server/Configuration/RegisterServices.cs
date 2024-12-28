using Project.Server.Auth;
using Project.DataAccess.Services;
using Project.BusinessLogic.Service;
using Project.BusinessLogic.Email;

namespace Project.Server.Configuration
{
    public static class RegisterService
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
            services.AddScoped<IPaymentLogService, PaymentLogService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IEmailService, EmailService>();

            //Background Service
            services.AddSingleton<IEmailBackgroundService, EmailBackgroundService>();
            services.AddHostedService<EmailBackgroundService>(provider => (EmailBackgroundService)provider.GetRequiredService<IEmailBackgroundService>());

            return services;
        }
    }
}
