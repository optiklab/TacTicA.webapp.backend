using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TacTicA.ApiGateway.Repositories;
using TacTicA.Common.EventModel.CommandEvents;
using TacTicA.Common.QueueServices;

namespace TacTicA.ApiGateway.Controllers
{
    /// <summary>
    /// Badly designed: POST method simply posts EVENT, so it's handling is asynchronous.
    ///                 GET method goes to database.
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ItemsController : Controller
    {
        private readonly IEventBus _eventBus;

        private readonly IFlattenedItemRepository _flattenedItemRepository;

        public ItemsController(IEventBus eventBus, IFlattenedItemRepository flattenedItemRepository)
        {
            _eventBus = eventBus;
            _flattenedItemRepository = flattenedItemRepository;
        }

        // This is just a stub used while developing and testing.
        // It just returns the word "Secured" emulating that service is working.
        //[HttpGet("")]
        //public IActionResult Get() => Content("Secured");

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            // TODO Should I check this for null reference every time?
            var username = HttpContext.User.Identity.Name;

            // TODO Should I check this every time?
            if (username == null)
            {
                return Unauthorized();
            }

            var items = await _flattenedItemRepository.BrowseAsync(Guid.Parse(username));

            return Json(items.Select(x => new {x.Id, x.Name, x.Category, x.CreatedAt}));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var username = HttpContext.User.Identity.Name;

            if (username == null)
            {
                return Unauthorized();
            }

            var item = await _flattenedItemRepository.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            if (item.UserId != Guid.Parse(username))
            {
                return Unauthorized();
            }
            return Json(item);
        }

        /// <summary>
        /// This method posts an event to create item in the Services.Item microservice.
        /// </summary>
        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]CreateItemCommand command)
        {
            var username = HttpContext.User.Identity.Name;

            if (username == null)
            {
                return Unauthorized();
            }

            command.Id = Guid.NewGuid();
            command.CreatedAt = DateTime.UtcNow;
            command.UserId = Guid.Parse(username);

            await _eventBus.PublishAsync(command);

            return Accepted($"items/{command.Id}");
        }
    }
}