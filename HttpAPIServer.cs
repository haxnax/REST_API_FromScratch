using System.Net;
using System.Text;
using System.Text.Json;

public class HttpAPIServer
{
    private HttpListener _listner;
    private readonly string _baseUrl = "http://localhost:8080/";


    public async Task StartAsync()
    {
        _listner = new HttpListener();
        _listner.Prefixes.Add(_baseUrl);
        _listner.Start();

        Console.WriteLine($"API Server started at {_baseUrl}");
        Console.WriteLine("Press Ctrl+C to stop...");

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
        var request = context.Request;
        var response = context.Response;
        try
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {request.HttpMethod} {request.Url}");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

            if (request.HttpMethod == "OPTIONS")
            {
                response.StatusCode = 200;
                response.Close();
                return;
            }
            await RouteRequestAsync(request, response);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(response, ex);
        }
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
