namespace Storage.Application.Common.Exceptions
{
    public class UnsupportedMimeTypeException : BaseServiceException
    {
        public UnsupportedMimeTypeException() : base(ErrorMessages.UNSUPPORTED_MIME_TYPE_ERROR_MESSAGE)
        {
        }
    }
}
