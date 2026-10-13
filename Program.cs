using System.Net;
using System.Text;
using System.Text.Json;

class Program
{
    static async Task Main(string[] args)
    {
        var server = new HttpAPIServer();
        await server.StartAsync();

    }
}



