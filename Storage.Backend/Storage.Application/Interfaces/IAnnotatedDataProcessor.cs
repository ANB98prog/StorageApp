using Storage.Application.Common.Models;
using Storage.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Interfaces
{
    public interface IAnnotatedDataProcessor : IDisposable
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
        /// <param name="annotationData">Annotation data</param>
        /// <param name="groupPath">Annotation group files path</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Path to directory with annotated files info</returns>
        /// <remarks>
        /// groupName is needed if annotated files are from different data sets and has different classes
        /// </remarks>
        /// <exception cref="AnnotationConvertionException"></exception>
        public Task<string?> ConvertAnnotatedDataAsync(List<AnnotatedFileData> annotationData, string groupPath, CancellationToken cancellationToken);
    }
}
