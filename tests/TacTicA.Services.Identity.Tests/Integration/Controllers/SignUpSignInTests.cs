using TacTicA.Common.Auth;
using FluentAssertions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TacTicA.Services.Identity.Tests.Integration.Controllers
{
    public class SignUpSignInTests
    {
        private readonly TestServer _identityServer;
        private readonly HttpClient _identityClient;

        public SignUpSignInTests()
        {
            _identityServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<Identity.Startup>());
            _identityClient = _identityServer.CreateClient();
        }

        [Fact]
        public async Task Account_controller_registers_user_and_signs_in_successfully()
        {
            var signUpPayload = GetPayload(new { email = "user1@email.com", name = "anton", password = "test1234" });
            var signUpResponse = await _identityClient.PostAsync(@"api/accounts/signup", signUpPayload);
            signUpResponse.EnsureSuccessStatusCode();

            var siginInPayload = GetPayload(new { email = "user1@email.com", password = "test1234" });
            var signInResponse = await _identityClient.PostAsync(@"api/accounts/signin", siginInPayload);
            signInResponse.EnsureSuccessStatusCode();

            var content = await signInResponse.Content.ReadAsStringAsync();
            var jwt = JsonConvert.DeserializeObject<JsonWebToken>(content);

            jwt.Should().NotBeNull();
            jwt.Token.Should().NotBeEmpty();
            jwt.Expires.Should().BeGreaterThan(0);
        }

        protected static StringContent GetPayload(object data)
        {
            var json = JsonConvert.SerializeObject(data);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}