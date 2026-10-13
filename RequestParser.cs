using System.Net;

public class RequestParser
{
    public static async Task<ApiRequest> ParseAsync(HttpListenerRequest request)
    {
        var ApiRequest = new ApiRequest
        {
            Method = request.HttpMethod,
            Url = request.Url,
            Headers = new Dictionary<string, string>(),
            QueryParameters = ParseQueryString(request.Url.Query),
            Body = await ReadRequestBodyAsync(request)
        };
        foreach (string headerName in request.Headers.AllKeys)
        {
            ApiRequest.Headers[headerName] = request.Headers[headerName];
        }
        return ApiRequest;
    }

    private static async Task<string> ReadRequestBodyAsync(HttpListenerRequest request)
    {
        if (request.ContentLength64 == 0) return string.Empty;
        using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
        return await reader.ReadToEndAsync();
    }

    private static Dictionary<string, string> ParseQueryString(string queryString)
    {
        var parameters = new Dictionary<string, string>();
        if (string.IsNullOrEmpty(queryString)) return parameters;

        queryString = queryString.TrimStart('?');
        var pairs = queryString.Split('&');
        foreach (var pair in pairs)
        {
            var keyValue = pair.Split('=');
            if (keyValue.Length == 2)
            {
                parameters[Uri.UnescapeDataString(keyValue[0])] =
                Uri.UnescapeDataString(keyValue[1]);
            }

        }
        return parameters;
    }
}

