using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyLab.Log;
using MyLab.Log.Dsl;
using Newtonsoft.Json;

namespace MyLab.WebErrors
{
    class UnhandledExceptionFilter : IExceptionFilter
    {
        private readonly ExceptionProcessingOptions _options;
        private readonly IDslLogger _log;

        public const string DefaultErrorMessage = "An error occurred during the operation";
        public string ErrorMessage { get; set; } = DefaultErrorMessage;

        public UnhandledExceptionFilter(
            IOptions<ExceptionProcessingOptions> options,
            ILogger<UnhandledExceptionFilter> logger = null)
        {
            _options = options.Value;
            _log = logger.Dsl();
        }

        public void OnException(ExceptionContext context)
        {
            HttpStatusCode actualResultCode;

            if (ErrorToResponseMap.GetFromActionProperties(context.ActionDescriptor.Properties, out var map) &&
                map.TryGetBinding(context.Exception.GetType(), out var binding))
            {
                actualResultCode = SetBoundResult(context, binding);
            }
            else 
            {
                actualResultCode = SetErrorResult(context);
            }

            if ((int)actualResultCode >= 500)
            {

                _log.Error(context.Exception)
                    .AndFactIs("response-code", actualResultCode)
                    .AndFactIs(HttpTraceIdFact.Key, context.HttpContext.TraceIdentifier)
                    .Write();

            } else if ((int)actualResultCode >= 400)
            {
                _log.Warning(context.Exception)
                    .AndFactIs("response-code", actualResultCode)
                    .AndFactIs(HttpTraceIdFact.Key, context.HttpContext.TraceIdentifier)
                    .Write();
            }
        }

        HttpStatusCode SetErrorResult(ExceptionContext context)
        {
            bool hideErr = _options?.HideError ?? true;

            var dto = new InterlevelErrorDto
            {
                Id = context.HttpContext.TraceIdentifier,
                
                Message = hideErr 
                    ? _options?.HidesMessage ?? ErrorMessage ?? DefaultErrorMessage
                    : context.Exception.Message,

                TechDetails = hideErr
                    ? null
                    : context.Exception.StackTrace
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

            return HttpStatusCode.InternalServerError;
        }

        HttpStatusCode SetBoundResult(ExceptionContext context, ErrorToResponseBinding binding)
        {
            if (binding.ResponseCode == HttpStatusCode.NoContent)
            {
                context.Result = new NoContentResult();
            }
            else
            {
                context.Result = new ContentResult
                {
                    Content = binding.Message ?? context.Exception.Message,
                    ContentType = "text/plain",
                    StatusCode = (int)binding.ResponseCode
                };
            }

            context.ExceptionHandled = true;

            return binding.ResponseCode;
        }
    }
}
