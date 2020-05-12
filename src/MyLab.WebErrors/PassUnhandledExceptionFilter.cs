using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MyLab.WebErrors
{
    class PassUnhandledExceptionFilter : IExceptionFilter
    {
        private readonly ExceptionProcessingOptions _options;

        public PassUnhandledExceptionFilter(IOptions<ExceptionProcessingOptions> options)
        {
            _options = options.Value;
        }

        public void OnException(ExceptionContext context)
        {
            if (_options?.HideError ?? true)
                return;

            var dto = new InterlevelErrorDto
            {
                Id = context.HttpContext.TraceIdentifier,
                Message = context.Exception.Message,
                TechDetails = context.Exception.StackTrace
            };

            var strContent = JsonConvert.SerializeObject(dto, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var res = new ContentResult
            {
                Content = strContent,
                ContentType = "application/json",
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            context.Result = res;
            context.ExceptionHandled = true;
        }
    }
}
