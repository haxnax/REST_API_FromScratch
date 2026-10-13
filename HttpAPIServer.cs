using System.Net;
using System.Text;
using System.Text.Json;

public class HttpAPIServer
{
    private HttpListener _listner;
    private ApiRouter _router;
    private readonly string _baseUrl = "http://localhost:8080/";
    private readonly MiddlewarePipeline _pipeline;

    public HttpAPIServer()
    {
        _router = new ApiRouter();
        _pipeline = new MiddlewarePipeline();
        ConfigureRoutes();
        ConfigureMiddleware();
    }
    private void ConfigureRoutes()
    {
        _router.AddRoute("GET", "/api/users", UserController.GetAllUsers);
        _router.AddRoute("GET", "/api/users/{id}", UserController.GetUserById);
        _router.AddRoute("POST", "/api/users", UserController.CreateUser);
        _router.AddRoute("PUT", "/api/users/{id}", UserController.UpdateUser);
        _router.AddRoute("DELETE", "/api/users/{id}", UserController.DeleteUser);
    }
    private void ConfigureMiddleware()
    {
        _pipeline.Use(new RequestLoggingMiddleware());
        _pipeline.Use(new RateLimitingMiddleware(maxRequests: 100, TimeSpan.FromMinutes(1)));
        _pipeline.Use(new CompressionMiddleware());
    }

    public async Task StartAsync()
    {
        _listner = new HttpListener();
        _listner.Prefixes.Add(_baseUrl);
        _listner.Start();

        Console.WriteLine($"RESTful API Server started at {_baseUrl}");
        Console.WriteLine("Available endpoints:");
        Console.WriteLine("  GET    /api/users     - Get all users");
        Console.WriteLine("  GET    /api/users/{id} - Get user by ID");
        Console.WriteLine("  POST   /api/users     - Create new user");
        Console.WriteLine("  PUT    /api/users/{id} - Update user");
        Console.WriteLine("  DELETE /api/users/{id} - Delete user");
        Console.WriteLine("\n Press Ctrl+C to stop the server...");

        await ListenForRequestAsync();
    }

    private async Task ListenForRequestAsync()
    {
        while (_listner.IsListening)
        {
            var context = await _listner.GetContextAsync();
            _ = ProcessRequestAsync(context);
        }


    }
     private async Task ProcessRequestAsync(HttpListenerContext context)
    {
        ApiRequest apiRequest = null;
        try
        {
            apiRequest = await RequestParser.ParseAsync(context.Request);
            
            var response = await _pipeline.ExecuteAsync(apiRequest, async (req) =>
            {
                return await _router.RouteAsync(req);
            });
            
            await SendResponseAsync(context.Response, response);
        }
        catch (Exception ex)
        {
            var errorResponse = await GlobalErrorHandler.HandleAsync(ex, apiRequest);
            await SendResponseAsync(context.Response, errorResponse);
        }
    }

    private async Task SendResponseAsync(HttpListenerResponse response, ApiResponse apiResponse)
    {
        response.StatusCode = apiResponse.StatusCode;
        response.ContentType = apiResponse.ContentType;

        foreach (var header in apiResponse.Headers)
        {
            response.Headers.Add(header.Key, header.Value);
        }
        if (!string.IsNullOrEmpty(apiResponse.Body))
        {
            var buffer = Encoding.UTF8.GetBytes(apiResponse.Body);
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        }
        response.Close();
    }

    private async Task HandleErrorAsync(HttpListenerResponse response, Exception ex)
    {
        response.StatusCode = 500;
        response.ContentType = "application/json";
        var errorResponse = new
        {
            error = "internal Server Error",
            message = ex.Message,
        };
        var jsonResponse = JsonSerializer.Serialize(errorResponse);
        var buffer = Encoding.UTF8.GetBytes(jsonResponse);

        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        response.Close();
    }

    private async Task RouteRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
    {
        throw new NotImplementedException();
    }

}
