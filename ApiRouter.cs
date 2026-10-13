using System.Text.Json;

public class ApiRouter
{
    private readonly Dictionary<string, Dictionary<string, Func<ApiRequest, Task<ApiResponse>>>> _routes;
    public ApiRouter()
    {
        _routes = new Dictionary<string, Dictionary<string, Func<ApiRequest, Task<ApiResponse>>>>();
    }

    public void AddRoute(string method, string pattern, Func<ApiRequest, Task<ApiResponse>> handler)
    {
        if (!_routes.ContainsKey(method))
        {
            _routes[method] = new Dictionary<string, Func<ApiRequest, Task<ApiResponse>>>();
        }
        _routes[method][pattern] = handler;
    }

    public async Task<ApiResponse> RouteAsync(ApiRequest request)
    {
        if (!_routes.ContainsKey(request.Method))
        {
            return new ApiResponse
            {
                StatusCode = 405,
                Body = JsonSerializer.Serialize(
                    new
                    {
                        error = "Method not allowed"
                    })
            };
        }
        var methodRoutes= _routes[request.Method];
        var path= request.Url.AbsolutePath;
        if(methodRoutes.ContainsKey(path))
        {
            return await methodRoutes[path](request);
        }
        foreach(var route in methodRoutes)
        {
            if(MatchesPattern(route.Key,path,out var parameters))
            {
                request.QueryParameters= parameters;
                return await route.(request);

            }
        }
        return new ApiResponse{StatusCode=404,Body= JsonSerializer.Serialize(new{error= "Route Not Found"})};

    }

    private bool MatchesPattern(string pattern, string path, out Dictionary<string,string> parameters)
    {
        parameters = new Dictionary<string, string>();

        var patternParts= pattern.Split('/');
        var pathParts= path.Split('/');

        if (patternParts.Length != pathParts.Length)
        {
            return false;

        }
        for (int i = 0; i < patternParts.Length; i++)
        {
            if (patternParts[i].StartsWith("{") && patternParts[i].EndsWith("}"))
            {
                var paramName=patternParts[i].Trim('{','}');
                parameters[paramName]=pathParts[i];

            }
            else if (patternParts[i] != pathParts[i])
            {
                return false;
            }

        }
        return true;


    }
}
