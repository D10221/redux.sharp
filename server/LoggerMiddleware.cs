using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace server
{
    class LoggerMiddleware
    {
        public static Func<HttpContext, Func<Task>, Task> CreateMiddleware(IServiceProvider services)
        {
            var logger = (ILogger<LoggerMiddleware>)services.GetService(typeof(ILogger<LoggerMiddleware>));

            return async (context, next) =>
            {
                var req = context.Request;
                logger.LogInformation($"enter: {req.Scheme}://{req.PathBase}{req.RouteValues["args"]}{req.QueryString}");
                await next.Invoke();
                logger.LogInformation($"exit: {req.Scheme}://{req.PathBase}{req.RouteValues["args"]}{req.QueryString}");
            };
        }
    }
}
