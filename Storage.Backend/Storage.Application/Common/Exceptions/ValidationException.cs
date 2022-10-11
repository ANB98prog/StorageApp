using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Storage.Application.Common.Exceptions
{
    public class ValidationException : FileHandlerServiceException
    {
        /// <summary>
        /// Validation errors
        /// </summary>
        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Errors { get; private set; }

        public ValidationException(List<string> messages) : base(JsonConvert.SerializeObject(messages, Formatting.Indented))
        {
        }

        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ValidationException(string message, string userFriendlyMessage) : base(message, userFriendlyMessage)
        {
        }

        public ValidationException(string message, string userFriendlyMessage, Exception innerException) : base(message, userFriendlyMessage, innerException)
        {
        }

        /// <summary>
        /// Adds validation error
        /// </summary>
        /// <param name="errorMessage">Validation error message</param>
        public void AddValidationError(string errorMessage)
        {
            Errors.Add(errorMessage);
            UserFriendlyMessage = JsonConvert.SerializeObject(Errors, Formatting.Indented);
        }
    }

    /// <summary>
    /// Validation error model
    /// </summary>
    public class ValidationErrorModel
    {
        /// <summary>
        /// Validation errors
        /// </summary>
        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Errors { get; set; }
    }
}
