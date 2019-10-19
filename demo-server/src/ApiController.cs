using Microsoft.AspNetCore.Mvc;

namespace MyApp
{
    [Route("/api/{route?}")]
    public class ApiController : Controller
    {
        [HttpGet]
        public IActionResult Get(string route)
        {
            var state = MyState.GetState();
            if (string.IsNullOrWhiteSpace(route))
            {
                return Json(state);
            }
            if (!state.ContainsKey(route))
            {
                return Json(null);
            }            
            return Json(state[route]);
        }
    }
}
