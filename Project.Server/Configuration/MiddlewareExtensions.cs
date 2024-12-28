namespace Project.Server.Configuration
{
    public static class MiddlewareExtensions
    {
        public static WebApplication RegisterMiddleware(this WebApplication app)
        {
            app.UseMiddleware<LoggingMiddleware>();
            return app;
        }
    }

}
