using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MyLab.Log.Dsl;

namespace MyLab.WebErrors
{
    class LogUnhandledExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<LogUnhandledExceptionFilter> _logger;

        public LogUnhandledExceptionFilter(ILogger<LogUnhandledExceptionFilter> logger = null)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger?.Dsl()
                .Error(context.Exception)
                .AndFactIs(HttpTraceIdFact.Key, context.HttpContext.TraceIdentifier)
                .Write();
        }
    }
}
