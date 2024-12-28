using System.Collections.Concurrent;

public class RateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly ConcurrentDictionary<string, ClientRequestInfo> _requestTracker = new ConcurrentDictionary<string, ClientRequestInfo>();
    private readonly int _requestLimit = 100; // Max requests per minute
    private readonly TimeSpan _timeWindow = TimeSpan.FromMinutes(1);

    public RateLimitMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var now = DateTime.UtcNow;

        var requestInfo = _requestTracker.GetOrAdd(clientIp, new ClientRequestInfo { LastRequestTime = now, RequestCount = 0 });

        if (now - requestInfo.LastRequestTime > _timeWindow)
        {
            // Reset count if time window has passed
            requestInfo.RequestCount = 0;
            requestInfo.LastRequestTime = now;
        }

        if (requestInfo.RequestCount >= _requestLimit)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.Response.Headers["Retry-After"] = _timeWindow.TotalSeconds.ToString();
            await context.Response.WriteAsync("Rate limit exceeded. Please try again later.");
            return;
        }

        // Increment request count
        requestInfo.RequestCount++;

        await _next(context);
    }

    private class ClientRequestInfo
    {
        public int RequestCount { get; set; }
        public DateTime LastRequestTime { get; set; }
    }
}
