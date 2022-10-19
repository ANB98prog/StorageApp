using Storage.Application.Common.Models;
using Storage.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Storage.Application.Interfaces
{
    public interface IAnnotatedDataProcessor
    {
        /// <summary>
        /// Annotation format
        /// </summary>
        public AnnotationFormats AnnotationFormat { get; }

        /// <summary>
        /// Processes annotated data
        /// </summary>
        /// <param name="files">Files data</param>
        /// <returns>Processed data</returns>
        public Task<Dictionary<Guid, AnnotationMetadata>> ProcessAnnotatedDataAsync(List<UploadFileRequestModel> files);

        /// <summary>
        /// Converts annotated data
        /// </summary>
        /// <param name="files">Files data</param>
        /// <param name="requiredFormat">Required annotation format</param>
        public void ConvertAnnotatedData(List<BaseFile> files, AnnotationFormats requiredFormat);
    }
}
