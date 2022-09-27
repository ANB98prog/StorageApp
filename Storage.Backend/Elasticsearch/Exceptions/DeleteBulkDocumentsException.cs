using System;
using System.Collections.Generic;

namespace Elasticsearch.Exceptions
{
    public class DeleteBulkDocumentsException : Exception
    {
        public IEnumerable<string> ErrorDocumentsIds { get; private set; }

        public DeleteBulkDocumentsException(IEnumerable<string> errorDocumentsIds)
        {
            ErrorDocumentsIds = errorDocumentsIds;
        }
    }
}
