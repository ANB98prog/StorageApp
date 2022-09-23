namespace Elasticsearch.Exceptions
{
    /// <summary>
    /// Error messages
    /// </summary>
    public static class ErrorMessages
    {
        public static string INDEX_ALREADY_EXISTS(string indexName) => $"Index '{indexName}' already exists!";

        public static string INDEX_NOT_EXISTS(string indexName) => $"Index '{indexName}' not exists!";

        public static string ERROR_CREATING_INDEX(string indexName) => $"Error occured while index creating. Index name '{indexName}'";
        public static string ERROR_REMOVING_INDEX(string indexName) => $"Error occured while index delete. Index name '{indexName}'";
        public static string ERROR_ADDITION_DOCUMENT(string indexName) => $"Error occured while add document to {indexName} index.";
    }
}
