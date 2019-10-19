using Microsoft.AspNetCore.Mvc;

namespace MyApp
{
    [Route("/api")]
    public class ApiController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Json(MyState.GetState());
        }
    }
}
