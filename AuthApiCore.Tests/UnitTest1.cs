using AuthApiCore.Controllers;
using AuthApiCore.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using static AuthApiCore.Controllers.AuthController;

namespace AuthApiCore.Tests
{
    public class AuthoControllerTest : IClassFixture<WebApplicationFactory<AuthController>>
    {
        private readonly HttpClient _client;
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<SignInManager<IdentityUser>> _signInManagerMock;
        private readonly AuthController _controller;

        public AuthoControllerTest(WebApplicationFactory<AuthController> factory)
        {
            _client = factory.CreateClient();
            _userManagerMock = CreateMockUserManager();
            _signInManagerMock = CreateMockSignInManager();
            var configuration = new ConfigurationBuilder().Build();
            _controller = new AuthController(configuration, _userManagerMock.Object, _signInManagerMock.Object);
        }

        private Mock<UserManager<IdentityUser>> CreateMockUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            return new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private Mock<SignInManager<IdentityUser>> CreateMockSignInManager()
        {
            return new Mock<SignInManager<IdentityUser>>(
                _userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
                null, null);
        }

        [Theory]
        [InlineData("alexmanes83@gmail.com", "Hlpm1741@")]
        [InlineData("invalidUser@gmail.com", "invalidPassword@")]
        public async Task Login_ValidUser_ReturnsOk(string email, string password)
        {
            var loginRequest = new { Email = email, Password = password };
            var content = CreateJsonContent(loginRequest);
            var response = await _client.PostAsync("/api/auth/login", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = JsonConvert.DeserializeObject<JObject>(responseBody);
                var token = result["token"];
                token.Should().NotBeNull();
                token.ToString().Should().NotBeEmpty();
            }
            else
            {
                response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            }
        }

        [Fact]
        public async Task RegisterUser_WhenAlreadyExists_ReturnsBadRequest()
        {
            var email = "TestRegisterNotExist@gmail.com";
            var password = "TesteRegister1234@@";
            var content = CreateJsonContent(new { Email = email, Password = password });
            SetupMockUserExists(email);

            var response = await _client.PostAsync("/api/auth/register", content);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RegisterUser_WhenDoesNotExist_CreatesUserSuccessfully()
        {
            var email = "TestRegisterNotExist3@gmail.com";
            var password = "TesteRegister1234@@";
            var content = CreateJsonContent(new { Email = email, Password = password });

            SetupMockUserDoesNotExist(email);
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var response = await _client.PostAsync("/api/auth/register", content);

            response.StatusCode.Should().Be(HttpStatusCode.OK); 
        }

        [Fact]
        public async Task DeleteUser_WhenExists_ReturnsOk()
        {
            var email = "TestRegisterNotExist@gmail.com";
            SetupMockUserExists(email);

            var result = await _controller.DeleteUser(email) as OkObjectResult;

            result.StatusCode.Should().Be(200);
            
            var responseValue = result.Value as ApiResponse;
            responseValue.Should().NotBeNull();
            responseValue.Message.Should().Be("Usuário deletado com sucesso.");
        }

        [Fact]
        public async Task DeleteUser_WhenDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var email = "naoExistente@gmail.com";
            SetupMockUserDoesNotExist(email);

            // Act
            var result = await _controller.DeleteUser(email) as NotFoundObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);

            var responseValue = result.Value as NotFoundResponse;
            responseValue.Should().NotBeNull();
            responseValue.Message.Should().Be("Usuário não encontrado.");
        }
        

        [Fact]
        public async Task UpdateUser_WhenUserDoesNotExist_ReturnsNotFound()
        {
            var userId = "1";
            var updateModel = new UpdateUserModel { Email = "email@gmail.com" };
            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync((IdentityUser)null);

            var result = await _controller.UpdateUser(userId, updateModel);

            result.Should().BeOfType<NotFoundResult>();
        }

        private void SetupMockUserExists(string email)
        {
            var existingUser = new IdentityUser { UserName = email, Email = email };
            _userManagerMock.Setup(um => um.FindByEmailAsync(email)).ReturnsAsync(existingUser);
            _userManagerMock.Setup(um => um.DeleteAsync(existingUser)).ReturnsAsync(IdentityResult.Success);
        }

        private void SetupMockUserDoesNotExist(string email)
        {
            _userManagerMock.Setup(um => um.FindByEmailAsync(email)).ReturnsAsync((IdentityUser)null);
        }

        private static StringContent CreateJsonContent(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}