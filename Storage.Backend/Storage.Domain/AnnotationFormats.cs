using System.ComponentModel;

namespace Storage.Domain
{
    /// <summary>
    /// Amnnotation formats
    /// </summary>
    public enum AnnotationFormats
    {
        /// <summary>
        /// Yolo format
        /// </summary>
        [Description("yolo")]
        yolo,

        /// <summary>
        /// Label MG format
        /// </summary>
        [Description("LabelMG")]
        labelMG,

        /// <summary>
        /// Cvat format
        /// </summary>
        [Description("cvat")]
        cvat
    }
}
