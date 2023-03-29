using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MyLab.WebErrors;
using Newtonsoft.Json;
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
    public class PassingExceptionBehavior : IClassFixture<WebApplicationFactory<App>>
    {
        private readonly WebApplicationFactory<App> _factory;
        private readonly ITestOutputHelper _output;

        public PassingExceptionBehavior(WebApplicationFactory<App> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

        [Fact]
        public async Task ShouldHideUnhandledException()
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(ConfigureWithExceptionPassing).CreateClient();

            // Act
            var response = await client.GetAsync("api/exception-hiding");

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();

            InterlevelErrorDto dto;
            try
            {
                dto = JsonConvert.DeserializeObject<InterlevelErrorDto>(content);
            }
            finally 
            {
                _output.WriteLine("Content: " + content);
            }

            Assert.Equal("bar", dto.Message);
            Assert.False(string.IsNullOrEmpty(dto.TechDetails));
        }

        [Fact]
        public async Task ShouldSetId()
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(ConfigureWithExceptionPassing).CreateClient();

            // Act
            var response = await client.GetAsync("api/exception-hiding");

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();

            InterlevelErrorDto dto;
            try
            {
                dto = JsonConvert.DeserializeObject<InterlevelErrorDto>(content);
            }
            finally 
            {
                _output.WriteLine("Content: " + content);
            }

            Assert.NotNull(dto.Id);
        }

        private void ConfigureWithExceptionPassing(IWebHostBuilder b)
        {
            b.ConfigureServices(services =>
            {
                services.AddMvc(o => o.AddExceptionProcessing());
                services.Configure<ExceptionProcessingOptions>(o => o.HideError = false);
            });
        }
    }
}
