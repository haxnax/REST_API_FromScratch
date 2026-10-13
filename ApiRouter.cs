public class ApiRouter
{
    private readonly Dictionary<string, Dictionary<string, Func<ApiRequest, Task<ApiResponse>>>> _routes;
    public ApiRouter()
    {
        _routes = new Dictionary<string, Dictionary<string, Func<ApiRequest, Task<ApiResponse>>>>();
    }
    
    public void AddRoute(string method,string pattern, Func<ApiRequest,Task<ApiResponse>> handler)
    {
        if (!_routes.ContainsKey(method))
        {
            _routes[method] = new Dictionary<string, Func<ApiRequest, Task<ApiResponse>>>();
        }
        _routes[method][pattern] = handler;
    }
}