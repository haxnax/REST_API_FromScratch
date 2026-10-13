using System.IO.Compression;
using System.Text;

public class CompressionMiddleware : IMiddleware
{
    public async Task<ApiResponse> InvokeAsync(ApiRequest request, Func<ApiRequest, Task<ApiResponse>> next)
    {
        var response= await next(request);

        if (request.Headers.TryGetValue("Accept-Encoding", out var encoding)&& 
        encoding.Contains("gzip")&&
        !string.IsNullOrEmpty(response.Body)) 
        {
            var compressed= CompressString(response.Body);
            if(compressed.Length<response.Body.Length)
            {
                response.Body= Convert.ToBase64String(compressed);
                response.Headers["Content-Encoding"]="gzip";
                response.Headers["Content-Length"] =compressed.Length.ToString();

            }
        }
        return response;

    }

    private byte[] CompressString(string text)
    {
        var bytes= Encoding.UTF8.GetBytes(text);
        using var output= new MemoryStream();
        using(var gzip=new GZipStream(output, CompressionMode.Compress))
        {
            gzip.Write(bytes,0,bytes.Length);
        }
        return output.ToArray();
    }
}