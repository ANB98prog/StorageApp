using System;

namespace Storage.Domain
{
    /// <summary>
    /// Organization department
    /// </summary>
    public class Department
    {
        /// <summary>
        /// Department id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Department name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Department description
        /// </summary>
        public string Description { get; set; }
    }
}
