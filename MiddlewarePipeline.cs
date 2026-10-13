using System.Diagnostics;

public interface IMiddleware
{
    Task<ApiResponse> InvokeAsync(ApiRequest request, Func<ApiRequest, Task<ApiResponse>> next);
}
public class MiddlewarePipeline
{
    private readonly List<IMiddleware> _middlewares = new List<IMiddleware>();

    public void Use(IMiddleware middleware)
    {
        _middlewares.Add(middleware);
    }

    public async Task<ApiResponse> ExecuteAsync(ApiRequest request, Func<ApiRequest, Task<ApiResponse>> finalHandler)
    {

        Func<ApiRequest, Task<ApiResponse>> pipeline = finalHandler;
        for (int i = _middlewares.Count - 1; i >= 0; i--)
        {
            var currentNiddleWare = _middlewares[i];
            var nextHandler = pipeline;
            pipeline = (req) => currentNiddleWare.InvokeAsync(req, nextHandler);
        }
        return await pipeline(request);


    }



}

public class RequestLoggingMiddleware : IMiddleware
{
    public async Task<ApiResponse> InvokeAsync(ApiRequest request, Func<ApiRequest, Task<ApiResponse>> next)
    {
        var stopwatch = Stopwatch.StartNew();
        Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] {request.Method} {request.Url.PathAndQuery} - Started");

        var response = await next(request);
        stopwatch.Stop();
        Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] {request.Method} {request.Url.PathAndQuery} - Completed in {stopwatch.ElapsedMilliseconds}ms (Status: {response.StatusCode})");
        return response;


    }
}

public class RateLimitingMiddleware : IMiddleware
{
    private readonly Dictionary<string, Queue<DateTime>> _requestHistory = new Dictionary<string, Queue<DateTime>>();
    private readonly int _maxRequests;
    private readonly TimeSpan _timeWindow;
    private readonly object _lock = new object();

    public RateLimitingMiddleware(int maxRequests = 100, TimeSpan timeWindow = default)
    {
        _maxRequests = maxRequests;
        _timeWindow = timeWindow == default ? TimeSpan.FromMinutes(1) : timeWindow;
    }


    public async Task<ApiResponse> InvokeAsync(ApiRequest request, Func<ApiRequest, Task<ApiResponse>> next)
    {
        var clientId = GetClientIdentifier(request);
        lock (_lock)
        {
            if (!_requestHistory.ContainsKey(clientId))
            {
                _requestHistory[clientId] = new Queue<DateTime>();
            }

            var clientRequests = _requestHistory[clientId];
            var cutoffTime = DateTime.UtcNow - _timeWindow;

            while (clientRequests.Count > 0 && clientRequests.Peek() < cutoffTime)
            {
                clientRequests.Dequeue();
            }

            if (clientRequests.Count >= _maxRequests)
            {
                return new ApiResponse
                {
                    StatusCode = 429,
                    Body = JsonHelper.Serialize(new { error = "Rate limit exceeded", retryAfter = _timeWindow.TotalSeconds })

                };
            }
            clientRequests.Enqueue(DateTime.UtcNow);
        }


        return await next(request);
    }
    private string GetClientIdentifier(ApiRequest request)
    {
        // In a real application, you might use IP address, API key, or user ID
        return request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor) ? forwardedFor : "unknown";
    }
}