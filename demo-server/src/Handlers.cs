using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace MyApp
{
    class Handlers
    {
        public static RequestDelegate ServeApp(string pathToIndex)
        {
            async Task serveApp(HttpContext context)
            {
                await context.Response.WriteAsync(
                    await File.ReadAllTextAsync(
                        Path.Combine(Directory.GetCurrentDirectory(), pathToIndex)));
            }
            return serveApp;
        }
    }
}
