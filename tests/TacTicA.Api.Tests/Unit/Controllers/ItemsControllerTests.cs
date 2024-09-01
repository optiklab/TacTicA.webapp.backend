using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TacTicA.ApiGateway.Controllers;
using TacTicA.ApiGateway.Repositories;
using TacTicA.Common.EventModel.CommandEvents;
using TacTicA.Common.QueueServices;
using Xunit;

namespace TacTicA.Api.Tests.Unit.Controllers
{
    public class ItemsControllerTests
    {
        [Fact]
        public async Task Items_Contoller_Post_Returns_Accepted()
        {
            var busClientMock = new Mock<IEventBus>();
            var ItemRepositoryMock = new Mock<IFlattenedItemRepository>();
            var controller = new ItemsController(busClientMock.Object,
                ItemRepositoryMock.Object);
            var userId = Guid.NewGuid();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(
                        new Claim[] { new Claim(ClaimTypes.Name, userId.ToString()) }
                        , "test"))
                }
            };

            var command = new CreateItemCommand
            {
                Id = Guid.NewGuid(),
                UserId = userId
            };

            var result = await controller.Post(command);

            var contentResult = result as AcceptedResult;

            contentResult.Should().NotBeNull();

            contentResult.Location.Should().Be($"items/{command.Id}");
        }
    }
}