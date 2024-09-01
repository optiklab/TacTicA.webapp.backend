using System.Threading.Tasks;
using TacTicA.Services.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TacTicA.Common.EventModel.CommandEvents;
using TacTicA.Common.QueueServices;

namespace TacTicA.Services.Identity.Controllers
{
    [Route("api/accounts")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEventBus _eventBus;

        public AccountController(IUserService userService, IEventBus eventBus)
        {
            _userService = userService;
            _eventBus = eventBus;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody]AuthenticateUserCommand command)
        {
            return Json(await _userService.LoginAsync(command.Email, command.Password));
        }

        /// <summary>
        /// TODO This is the duplication of API Gateway method in Users controller.
        /// </summary>
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody]CreateUserCommand command)
        {
            await _eventBus.PublishAsync(command);

            return Accepted();
        }

        [HttpGet("currentuserinfo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CurrentUserInfo()
        {
            var user = HttpContext.User.Identity.Name;

            if (!string.IsNullOrWhiteSpace(user))
            {
                // TODO How exception handled?
                var currentUser = await _userService.GetCurrentUserInfoAsync(System.Guid.Parse(user));

                return Json(currentUser);
            }

            return Json("No such user!");
        }
    }
}