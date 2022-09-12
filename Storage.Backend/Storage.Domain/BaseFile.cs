﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Storage.Domain
{
    public class BaseFile
    {
        /// <summary>
        /// Image id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Image owner id
        /// </summary>
        public Guid OwnerId { get; set; }

        /// <summary>
        /// Image department owner id
        /// </summary>
        public Guid DepartmentOwnerId { get; set; }

        /// <summary>
        /// Original image name
        /// </summary>
        public string OriginalName { get; set; }

        /// <summary>
        /// System image name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// File extension
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// Content type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Original file path
        /// </summary>
        public string OriginalFilePath { get; set; }

        /// <summary>
        /// Image attributes
        /// </summary>
        public IEnumerable<string> Attributes { get; set; }

        /// <summary>
        /// Created at
        /// </summary>
        public DateTime CreatedAt { get; private set; } = DateTime.Now;

        /// <summary>
        /// Edited at
        /// </summary>
        public DateTime EditedAt { get; set; }

        /// <summary>
        /// Describes is file annotated
        /// </summary>
        public bool IsAnnotated { get; set; }
    }
}
