using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var logger = app.ApplicationServices.GetService<ILogger<Startup>>();
            var isDev = env.IsDevelopment();
            if (isDev) app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.Use(LoggerMiddleware.CreateMiddleware(app.ApplicationServices));            
            app.UseEndpoints(endpoints =>
            {
                {
                    endpoints.MapGet("/", Routes.ServeIt(Path.Combine(
                                    Directory.GetCurrentDirectory(),
                                    "wwwroot",
                                    "index.html")));
                }
                {
                    var apiBase = "/api";
                    var (routeParams, route) = Routes.Route(app.ApplicationServices);
                    var routePath = $"{apiBase}/{routeParams}";
                    logger.LogInformation($"path: {routePath}");
                    endpoints.Map(routePath, route);
                }
            });
            app.UseStaticFiles();
        }
    }
}
