using Storage.Application.Common.Models;
using Storage.Domain;
using System.Collections.Generic;

namespace Storage.Application.Interfaces
{
    public interface IAnnotatedDataProcessor
    {
        /// <summary>
        /// Processes annotated data
        /// </summary>
        /// <param name="files">Files data</param>
        /// <param name="annotationFormat">Annotation format</param>
        /// <returns>Processed data</returns>
        public List<AnnotatedFileDataModel> ProcessAnnotatedData(List<BaseFile> files, AnnotationFormats annotationFormat);

        /// <summary>
        /// Converts annotated data
        /// </summary>
        /// <param name="files">Files data</param>
        /// <param name="requiredFormat">Required annotation format</param>
        public void ConvertAnnotatedData(List<BaseFile> files, AnnotationFormats requiredFormat);
    }
}
