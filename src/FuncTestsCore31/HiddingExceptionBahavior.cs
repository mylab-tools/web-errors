using System;
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
    public class HidingExceptionBehavior : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _output;

        public HidingExceptionBehavior(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

        [Fact]
        public async Task ShouldHideUnhandledException()
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(ConfigureWithExceptionHiding()).CreateClient();

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

            Assert.Equal(UnhandledExceptionFilter.DefaultErrorMessage, dto.Message);
            Assert.True(string.IsNullOrEmpty(dto.TechDetails));
        }

        [Fact]
        public async Task ShouldHideUnhandledExceptionWithCustomMessage()
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(ConfigureWithExceptionHiding("foo")).CreateClient();

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

            Assert.Equal("foo", dto.Message);
            Assert.True(string.IsNullOrEmpty(dto.TechDetails));
        }

        [Fact]
        public async Task ShouldSetId()
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(ConfigureWithExceptionHiding()).CreateClient();

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

        private Action<IWebHostBuilder> ConfigureWithExceptionHiding(string overrideMessage = null)
        {
            return b => b.ConfigureServices(services =>
            {
                services.AddMvc(o => o.AddExceptionProcessing());
                services.Configure<ExceptionProcessingOptions>(o => o.HidesMessage = overrideMessage);
            });
        }
    }
}
