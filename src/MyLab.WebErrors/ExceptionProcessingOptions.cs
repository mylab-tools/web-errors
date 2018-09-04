namespace MyLab.WebErrors
{
    /// <summary>
    /// Unhandled exception
    /// </summary>
    public class ExceptionProcessingOptions
    {
        /// <summary>
        /// Determines that unhandled exception details will be hidden. True by default.
        /// </summary>
        public bool HideError { get; set; } = true;

        /// <summary>
        /// Gets or sets message for client when error was hidden
        /// </summary>
        public string HidesMessage { get; set; }
    }
}