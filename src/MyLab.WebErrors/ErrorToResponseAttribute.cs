using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyLab.WebErrors
{
    /// <summary>
    /// Describes mapping exception to response
    /// </summary>
    public class ErrorToResponseAttribute : Attribute, IExceptionFilter
    {
        /// <summary>
        /// Exception type
        /// </summary>
        public Type ExceptionType { get; }

        /// <summary>
        /// Result response code
        /// </summary>
        public HttpStatusCode ResponseCode { get; }

        /// <summary>
        /// Use this message instead message from exception if specified
        /// </summary>
        public string Message { get; }
        
        /// <summary>
        /// Initializes a new instance of <see cref="ErrorToResponseAttribute"/>
        /// </summary>
        public ErrorToResponseAttribute(Type exceptionType, HttpStatusCode responseCode, string message = null)
        {
            ExceptionType = exceptionType;
            ResponseCode = responseCode;
            Message = message;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() != ExceptionType)
                return;

            context.Result = new ContentResult
            {
                Content = Message ?? context.Exception.Message,
                ContentType = "text/plain",
                StatusCode = (int)ResponseCode
            };

            context.ExceptionHandled = true;
        }
    }
}
