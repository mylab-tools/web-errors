using System;
using Newtonsoft.Json;

namespace MyLab.WebErrors
{
    /// <summary>
    /// Contains error data for client
    /// </summary>
    public class InterlevelErrorDto
    {
        /// <summary>
        /// Identifier
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        [JsonProperty("msg")]
        public string Message { get; set; }
        /// <summary>
        /// Technical details
        /// </summary>
        [JsonProperty("tech")]
        public string TechDetails{ get; set; }
    }
}
