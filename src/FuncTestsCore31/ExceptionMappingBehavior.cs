using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

#if NET7_0_OR_GREATER

using TestServerNet7;

using App = TestServerNet7.Program;

#else

using TestServerCore31;
using System.Threading.Tasks;

using App = TestServerCore31.Startup;

#endif

namespace FuncTests
{
    public class ExceptionMappingBehavior : IClassFixture<WebApplicationFactory<App>>
    {
        private readonly WebApplicationFactory<App> _factory;
        private readonly ITestOutputHelper _output;

        public ExceptionMappingBehavior(WebApplicationFactory<App> factory, ITestOutputHelper output)
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
