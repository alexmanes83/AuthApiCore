
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
using Newtonsoft.Json.Linq;
namespace AuthApiCore.Tests
{
    public class AuthoControllerTest : IClassFixture<WebApplicationFactory<AuthController>>
    {

        private readonly HttpClient _client;

        public AuthoControllerTest(WebApplicationFactory<AuthController> factory)
        {
            _client = factory.CreateClient();
        }
        public bool IsUnauthorized(HttpStatusCode statusCode)
        {
            return statusCode == HttpStatusCode.Unauthorized;
        }

        [Theory]
        [InlineData("alexmanes83@gmail.com", "Hlpm1741@")]
        [InlineData("invalidUser@gmail.com", "invalidPassword@")]
        public async Task Login_ValidUser_ReturnsOk(string email,string password)
        {
            // Arrange
            //var loginRequest = new
            //{
            //    Email = email,
            //    Password = password
            //};

            //var content = CreateJsonContent(loginRequest);

            //// Act
            //var response = await _client.PostAsync("/api/auth/login", content);
            //var responseBody = await response.Content.ReadAsStringAsync();


            //if (!IsUnauthorized(response.StatusCode))
            //{
            //    // Verificar se a resposta foi bem-sucedida
            //    response.StatusCode.Should().Be(HttpStatusCode.OK);

            //    var result = JsonConvert.DeserializeObject<JObject>(responseBody);  // Mudança para JObject

            //    // Assert
            //    var token = result["token"]; // Acessando a propriedade específica
            //    token.Should().NotBeNull(); // Certifique-se de que o token não seja nulo
            //    token.ToString().Should().NotBeEmpty(); // Certifique-se de que o token não esteja vazio
            //}else
            //{
            //    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            //}
        }
        private StringContent CreateJsonContent(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
