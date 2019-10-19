using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace MyApp
{
    public class Startup
    {
        string baseDir = Directory.GetCurrentDirectory();

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(baseDir, "modules/app/build")
                ),
                // RequestPath= "/**/(*.js|*.ico|*.jpg|*.png|*.json)",                
            });
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                // endpoints.MapGet("/", Handlers.ServeApp("modules/app/build/index.html"));
            });
            logger.Log(LogLevel.Information, "BaseDir: '{0}'", baseDir);
        }
    }
}
