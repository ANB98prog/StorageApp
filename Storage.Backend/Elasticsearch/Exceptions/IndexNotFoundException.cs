namespace Elasticsearch.Exceptions
{
    /// <summary>
    /// Index not found exception
    /// </summary>
    public class IndexNotFoundException : Exception
    {
        public IndexNotFoundException(string indexName)
            :base(ErrorMessages.INDEX_NOT_EXISTS(indexName))
        {}
    }
}
