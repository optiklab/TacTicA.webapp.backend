using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using TacTicA.ApiGateway.Controllers;

namespace TacTicA.Api.Tests.Unit.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void Home_Controller_Get_Returns_String_Content()
        {
            var result = (new HomeController()).Get();

            var contentResult = result as ContentResult;

            contentResult.Should().NotBeNull();
            contentResult.Content.Should().Be("Hello from TacTicA API!");
        }
    }
}