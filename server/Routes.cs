using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using static System.Text.Json.JsonSerializer;

namespace server
{
    class Routes
    {
        public static RequestDelegate ServeIt(string filePath)
        {
            return async ctx =>
                        await ctx.Response.WriteAsync(
                            await File.ReadAllTextAsync(filePath));
            ;
        }
        public static (string, RequestDelegate) Route(IServiceProvider services)
        {
            var config = services.GetService<IConfiguration>();
            var routeParams = "{*args}";
            var getInfo = GetRequestInfo(config);

            async Task Handler(HttpContext context)
            {
                var req = context.Request;

                if (req.Method == HttpMethods.Get)
                {
                    await context.Response.WriteAsync(getInfo(req));
                    return;
                }
                if (req.Method == HttpMethods.Post)
                {
                    var body = await ReadBody(req);
                    await context.Response.WriteAsync(getInfo(req) + $@"body: {body}");
                    return;
                }
                context.Response.StatusCode = (int)HttpStatusCode.NotImplemented;
                await context.Response.WriteAsync($"{req.Method} Not Implemented.");
            }
            // ...
            return (routeParams, Handler);
        }

        private static async Task<string> ReadBody(HttpRequest req)
        {
            string body;
            using (var reader = new StreamReader(req.Body))
            {
                body = await reader.ReadToEndAsync();
            }
            return body;
        }

        private static Func<HttpRequest, string> GetRequestInfo(IConfiguration config)
        {
            var allowedHosts = config.GetValue<string>("AllowedHosts");
            return req =>
            {
                var query = req.Query.ToDictionary(kv => kv.Key, kv => kv.Value);
                var info = $@"
                Path: {req.Path}
                PathBase: {req.PathBase}
                Scheme: {req.Scheme}
                Protocol: {req.Protocol}                
                RouteValues: {req.RouteValues["args"]}
                Query: {Serialize(query)}
                QueryString: {req.QueryString.Value}
                AllowedHosts: {allowedHosts}
                ";
                return info;
            };
        }
    }
}
