using System;

namespace Elasticsearch.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string itemId)
           : base(ErrorMessages.ITEM_NOT_FOUND(itemId))
        { }
    }
}
