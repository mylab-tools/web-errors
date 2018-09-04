using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using MyLab.Logging;
using Newtonsoft.Json;
using TestServer;
using Xunit;
using Xunit.Abstractions;

namespace MyLab.WebErrors.Tests
{
    public class LoggingExceptionBehavior : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _output;

        public LoggingExceptionBehavior(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

        [Fact]
        public async Task ShouldSetTheSameIdWithLogEntity()
        {
            //Arrange
            LogEntity logEntity = null;
            Exception exception = null;

            var loggerMock = new Mock<ILogger>();
            loggerMock.Setup(p => p.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<LogEntity>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<LogEntity, Exception, string>>()
                ))
                .Callback((Action<LogLevel, EventId, LogEntity, Exception, Func<LogEntity, Exception, string>>)
                    ((ll, ei, le, ex, f) =>
                    {
                        exception = ex;
                        logEntity = le;
                    }));

            var logProviderMock = new Mock<ILoggerProvider>();
            logProviderMock.Setup(p => p.CreateLogger(It.IsAny<string>()))
                .Returns((Func<string, ILogger>) (c => loggerMock.Object));

            var client = _factory.WithWebHostBuilder(b =>
            {
                b.ConfigureServices(services =>
                {
                    services.AddMvc(o => o.AddExceptionProcessing());
                    services.AddLogging(c => c.AddProvider(logProviderMock.Object));
                });
            }).CreateClient();

            //Act
            var response = await client.GetAsync("api/exception-hiding");
            var content = await response.Content.ReadAsStringAsync();
            
            InterlevelErrorDto dto;
            try
            {
                dto = JsonConvert.DeserializeObject<InterlevelErrorDto>(content);
            }
            catch (Exception)
            {
                _output.WriteLine("Content: " + content);

                throw;
            }

            //Assert
            Assert.NotNull(logEntity);

            _output.WriteLine("ExceptionId: " + (exception?.GetId().ToString() ?? "[exception not found]"));
            _output.WriteLine("LogEntityId: " + logEntity.InstanceId);

            Assert.Equal(dto.Id, logEntity.InstanceId);
        }
    }
}
