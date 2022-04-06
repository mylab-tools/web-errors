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
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ErrorToResponseAttribute : Attribute
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
    }
}
