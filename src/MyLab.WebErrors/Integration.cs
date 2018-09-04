using Microsoft.AspNetCore.Mvc;

namespace MyLab.WebErrors
{
    /// <summary>
    /// Integrates exception handling
    /// </summary>
    public static class Integration
    {

        /// <summary>
        /// Adds unhandled exception processing
        /// </summary>
        public static MvcOptions AddExceptionProcessing(this MvcOptions mvcOptions)
        {
            mvcOptions.Filters.Add<HideUnhandledExceptionFilter>();
            mvcOptions.Filters.Add<PassUnhandledExceptionFilter>();
            mvcOptions.Filters.Add<LogUnhandledExceptionFilter>();

            return mvcOptions;
        }
    }
}
