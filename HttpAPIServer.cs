using System.Net;
using System.Text;
using System.Text.Json;

public class HttpAPIServer
{
    private HttpListener _listner;
    private ApiRouter _router ;
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
        ApiRequest apiRequest=null;
        try
        {
            apiRequest= await RequestParser.ParseAsync(context.Request);
            var apiResponse = await _router.RouteAsync(apiRequest);
            await SendResponseAsync(context.Response,apiResponse);

        }catch(Exception ex)
        {
            var errorResponse= await GlobalErrorHandler.HandleAsync(ex, apiRequest);
            await SendResponseAsync(context.Response, errorResponse);
        }
    }

    private async Task SendResponseAsync(HttpListenerResponse response, ApiResponse apiResponse)
    {
        response.StatusCode= apiResponse.StatusCode;
        response.ContentType= apiResponse.ContentType;

        foreach(var header in apiResponse.Headers)
        {
            response.Headers.Add(header.Key, header.Value);
        }
        if(!string.IsNullOrEmpty(apiResponse.Body))
        {
            var buffer = Encoding.UTF8.GetBytes(apiResponse.Body);
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer,0,buffer.Length);
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
