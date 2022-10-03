using System;

namespace Storage.Application.Common.Exceptions
{
    public class NotFoundException : BaseServiceException
    {
        public string ItemId { get; private set; }

        public NotFoundException(string id) : base(ErrorMessages.ItemNotFoundErrorMessage(id))
        {
            ItemId = id;
        }

        public NotFoundException(string id, Exception innerException) : base(ErrorMessages.ItemNotFoundErrorMessage(id), innerException)
        {
            ItemId = id;
        }

        public NotFoundException(string id, string message, Exception innerException) : base(message, innerException)
        {
            ItemId = id;
        }

        public NotFoundException(string id, string message, string userFriendlyMessage) : base(message, userFriendlyMessage)
        {
            ItemId = id;
        }

        public NotFoundException(string id, string message, string userFriendlyMessage, Exception innerException) : base(message, userFriendlyMessage, innerException)
        {
            ItemId = id;
        }
    }
}
