using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TestServer;
using Xunit;

namespace MyLab.WebErrorManagement.Tests
{
    public class ExceptionMappingBehavior : IClassFixture<WebApplicationFactory<TestServer.Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public ExceptionMappingBehavior(WebApplicationFactory<TestServer.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ShouldMapExceptionAndOverrideExceptionMessageIfSpecified()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("api/exception-mapping/with-message");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal("foo", content);
        }

        [Fact]
        public async Task ShouldMapExceptionAndPassExceptionMessage()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("api/exception-mapping/without-message");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal("bar", content);
        }
    }
}
