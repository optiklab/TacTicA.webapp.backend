using Microsoft.AspNetCore.Mvc;

namespace TacTicA.Services.Items.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Get() => Content("Hello from TacTicA.Services.Items API!");
    }
}