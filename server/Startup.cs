using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.Use(LoggerMiddleware.CreateMiddleware(app.ApplicationServices));
            app.UseEndpoints(endpoints =>
            {
                var apiBase = "/api";
                var (routeParams, route) = Routes.Route(app.ApplicationServices);
                var routePath = $"{apiBase}/{routeParams}";
                logger.LogInformation($"path: {routePath}");
                endpoints.Map(routePath, route);
            });
        }
    }
}
