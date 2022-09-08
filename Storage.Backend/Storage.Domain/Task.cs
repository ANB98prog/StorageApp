using System;
using System.ComponentModel;

namespace Storage.Domain
{
    /// <summary>
    /// Task model
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Task id
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Task status
        /// </summary>
        public TaskStatus Status { get; set; }

        /// <summary>
        /// Task description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Message if error occured
        /// </summary>
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// Task status
    /// </summary>
    public enum TaskStatus
    {
        [Description("Preparing")]
        Preparing,

        [Description("InProcess")]
        InProcess,

        [Description("Finished")]
        Finished,

        [Description("Failed")]
        Failed
    }
}
