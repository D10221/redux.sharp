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
                    // Serve App when Nothing asked for ... 
                    endpoints.MapGet("/", Handlers.ServeFile(Path.Combine(
                                    Directory.GetCurrentDirectory(),
                                    "wwwroot",
                                    "index.html")));
                }                
                {
                    // serve '/api'
                    var apiBase = "/api";
                    var (routeParams, route) = Handlers.ServeInfo(app.ApplicationServices);
                    var routePath = $"{apiBase}/info/{routeParams}";
                    logger.LogInformation($"Serving: {routePath}");
                    endpoints.Map(routePath, route);
                }
            });
            // Server:/.*
            app.UseStaticFiles();
        }
    }
}
