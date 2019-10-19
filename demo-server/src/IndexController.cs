using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MyApp
{
    [Route("{**args}")]
    public class IndexController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(object[] args = null)
        {
            var html = await System.IO.File.ReadAllTextAsync(
                    Path.Combine(Directory.GetCurrentDirectory(),
                        "modules/app/build/body.html")
                );

            var head = await System.IO.File.ReadAllTextAsync(
                               Path.Combine(Directory.GetCurrentDirectory(),
                               "modules/app/build/head.html")
                           );

            var state = MyState.Serialize(
                MyState.GetState(),
                encoded: true
            );

            Response.Cookies.Append(
                "user", MyState.Serialize(MyState.User(), encoded: true),
                new CookieOptions()
                {
                    SameSite = SameSiteMode.Strict,
                    // HttpOnly = true,
                    Expires = DateTime.Now.AddHours(24),
                    Domain = "localhost"
                });
            return View("/index.cshtml", (html, state, publicUrl: "", head));
        }
    }
}
