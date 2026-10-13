public class ApiRequest
{
    public string Method { get; set; }
    public Uri Url { get; set; }
    public Dictionary<string, string> Headers { get; set; }
    public Dictionary<string, string> QueryParameters { get; set; }
    public string Body { get; set; }
}