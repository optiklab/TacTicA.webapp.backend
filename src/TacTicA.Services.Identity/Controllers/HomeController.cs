using Microsoft.AspNetCore.Mvc;

namespace TacTicA.Services.Identity.Controllers
{
    [Route("api")]
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Get() => Content("Hello from TacTicA.Services.Identity API!");
    }
}