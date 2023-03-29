using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyLab.Log;
using MyLab.WebErrors;
using Newtonsoft.Json;
using TestServerCore31;
using Xunit;
using Xunit.Abstractions;

namespace FuncTestsCore31
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
            var logger=  new TestLogger();
            var loggerProvider = new TestLoggerProvider(logger);

            var client = _factory.WithWebHostBuilder(b =>
            {
                b.ConfigureServices(services =>
                {
                    services.AddMvc(o => o.AddExceptionProcessing());
                    services.AddLogging(c => c.AddProvider(loggerProvider));
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
            Assert.NotNull(logger.LastLog);
            Assert.Equal(dto.Id, logger.LastLog.Facts[HttpTraceIdFact.Key]);
        }

        class TestLoggerProvider : ILoggerProvider
        {
            private readonly ILogger _logger;

            public TestLoggerProvider(ILogger logger)
            {
                _logger = logger;
            }

            public void Dispose()
            {
                
            }

            public ILogger CreateLogger(string categoryName)
            {
                return _logger;
            }
        }

        class TestLogger : ILogger
        {
            public LogEntity LastLog { get; private set; }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (state is LogEntity le)
                    LastLog = le;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return logLevel == LogLevel.Error;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }
    }
}
