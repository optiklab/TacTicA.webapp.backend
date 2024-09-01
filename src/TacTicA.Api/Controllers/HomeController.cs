using Microsoft.AspNetCore.Mvc;

namespace TacTicA.ApiGateway.Controllers
{
    /// <summary>
    /// The goal is to show that app is up and running.
    /// </summary>
    [Route("api")]
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Get() => Content("Hello from TacTicA API!");
    }
}