using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

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
                Id = context.Exception.GetId(),
                Message = context.Exception.Message,
                TechDetails = context.Exception.StackTrace
            };

            var res = new JsonResult(dto)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            context.Result = res;
            context.ExceptionHandled = true;
        }
    }
}
