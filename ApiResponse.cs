using System.Runtime.CompilerServices;

public class ApiResponse
{
    public int StatusCode { get; set; } = 200;
    public string ContentType { get; set; } = "application(json)";
    public string Body { get; set; } = string.Empty;
    public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

}

