using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
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
    [Route("/api")]
    public class ApiController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Json(MyState.GetState());
        }
    }
    class MyState
    {

        public static object GetState()
        {
            return new { Message = "hello" };
        }
        public static string Serialize(object o)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(o)));
        }
    }
    [Route("/")]
    public class MyControler : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            var html = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "modules/app/build/index.html"));
            ViewBag.html = html;
            Response.Cookies.Append("state",
                MyState.Serialize(MyState.GetState())
            );
            return View("/index.cshtml");
        }
    }
}
