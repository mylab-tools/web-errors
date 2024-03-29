using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TestServer;
using Xunit;
using Xunit.Abstractions;

namespace MyLab.WebErrors.Tests
{
    public class ExceptionMappingBehavior : IClassFixture<WebApplicationFactory<TestServer.Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _output;

        public ExceptionMappingBehavior(WebApplicationFactory<TestServer.Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
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
            _output.WriteLine("Response: " + content);

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
            _output.WriteLine("Response: " + content);

            Assert.Equal("bar", content);
        }

        [Fact]
        public async Task ShouldReturnNoContentResponseCorrectly()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("api/exception-mapping/no-content");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
