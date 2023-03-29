using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MyLab.WebErrors;
using Newtonsoft.Json;
using TestServerCore31;
using Xunit;
using Xunit.Abstractions;

namespace FuncTestsCore31
{
    public class PassingExceptionBehavior : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _output;

        public PassingExceptionBehavior(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
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
