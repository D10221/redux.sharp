using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace MyApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "modules/app/build")
              )
            });
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                // endpoints.MapGet("/", Render);
            });
        }
        // RequestDelegate
        async Task Render(HttpContext context)
        {
            await context.Response.WriteAsync(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "modules/app/build/index.html")));
        }
    }
    
    [Route("/")]
    public class MyControler : Controller
    {
        [HttpGet]
        public  IActionResult Get()
        {
            var html = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "modules/app/build/index.html"));
            ViewBag.html = html;
            return View("/index.cshtml");
        }
    }
}
