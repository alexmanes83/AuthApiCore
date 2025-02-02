
using AuthApiCore.Controllers;
using AuthApiCore.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore.Query;
namespace AuthApiCore.Tests
{
    public class AuthoControllerTest : IClassFixture<WebApplicationFactory<AuthController>>
    {

        private readonly HttpClient _client;

        public AuthoControllerTest(WebApplicationFactory<AuthController> factory)
        {
            _client = factory.CreateClient();
        }


        [Theory]
        [InlineData("alexmanes83@gmail.com", "Hlpm1741@")]
        [InlineData("invalidUser@gmail.com", "invalidPassword@")]
        public async Task Login_ValidUser_ReturnsOk(string email,string password)
        {
            // Arrange
            var loginRequest = new
            {
                Email = email,
                Password = password
            };
            var json = JsonConvert.SerializeObject(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            // Act
            var response = await _client.PostAsync("/api/auth/login", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(responseBody);
            result.Should().Contain("Token");
        }

        //[Fact]
        //public async Task Login_InvalidUser_ReturnsUnauthorized()
        //{
        //    // Arrange
        //    var loginRequest = new
        //    {
        //        Email = "invalid_user",
        //        Password = "wrong_password"
        //    };

        //    // Act
        //    var response = await _client.PostAsync("/api/auth/login", CreateJsonContent(loginRequest));

        //    // Assert
        //    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        //}

        //[Fact]
        //public async Task CreateUser_ValidUser_ReturnsCreated()
        //{
        //    // Arrange
        //    var newUser = new RegisterModel { Id = 1, Email = "teste1234556@gmail.com", Password = "Password123" };

        //    // Act
        //    var response = await _client.PostAsync("api/auth/register", CreateJsonContent(newUser));

        //    // Assert
        //    response.StatusCode.Should().Be(HttpStatusCode.Created);
        //}

        //[Fact]
        //public async Task UpdateUser_ValidUser_ReturnsNoContent()
        //{
        //    // Arrange
        //    var updatedUser = new RegisterModel { Email = "teste1234556@gmail.com", Password = "Aftc2680inpu@" };

        //    // Act
        //    var response = await _client.PutAsync("/api/auth/1", CreateJsonContent(updatedUser));

        //    // Assert
        //    response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        //}

        //[Fact]
        //public async Task DeleteUser_ValidUser_ReturnsNoContent()
        //{
        //    // Act
        //    var response = await _client.DeleteAsync("/api/auth/1");

        //    // Assert
        //    response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        //}

        //[Fact]
        //public async Task Logout_ShouldReturnOk()
        //{
        //    // Act
        //    var response = await _client.PostAsync("/api/auth/logout", null);

        //    // Assert
        //    response.StatusCode.Should().Be(HttpStatusCode.OK);
        //    var responseBody = await response.Content.ReadAsStringAsync();
        //    responseBody.Should().Be("Logged out successfully");
        //}

        //private StringContent CreateJsonContent(object obj)
        //{
        //    var json = JsonConvert.SerializeObject(obj);
        //    return new StringContent(json, Encoding.UTF8, "application/json");
        //}
    }
}
