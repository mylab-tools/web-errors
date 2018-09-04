using System;
using System.ComponentModel.Design;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MyLab.LogDsl;

namespace MyLab.WebErrorManagement
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
            _logger?.Dsl().Error(context.Exception).Write();
        }
    }
}
