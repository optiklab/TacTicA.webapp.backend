using Microsoft.AspNetCore.Mvc;

namespace TacTicA.Services.Cities.Controllers
{
    /// <summary>
    /// This Controller exists only for showing something initially after start.
    /// But better to run swagger.
    /// </summary>
    [Route("api")]
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Get() => Content("Hello from TacTicA.Services.Cities API!");
    }
}