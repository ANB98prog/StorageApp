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
        public static string ERROR_GET_BY_ID_DOCUMENT(string indexName, string id) => $"Error occured while get document {id} from {indexName} index.";
        public static string ERROR_GET_MANY_BY_IDS_DOCUMENTS(string indexName) => $"Error occured while get many documents from {indexName} index.";
        public static string ERROR_DELETE_BY_ID_DOCUMENT(string indexName, string id) => $"Error occured while remove document {id} from {indexName} index.";
        public static string ERROR_DELETE_BULK_DOCUMENTS(string indexName, string ids) => $"Error occured while remove bulk of documents {ids} from {indexName} index.";

        public static string UNEXPECTED_ERROR_REMOVING_BULK_DOCUMENTS(string indexName) => $"Unexpected error occured while remove bulk of documents from {indexName} index.";
        public static string UNEXPECTED_ERROR_REMOVING_BULK_DOCUMENTS_BY_QUERY() => $"Unexpected error occured while remove bulk of documents by query.";
        public static string UNEXPECTED_ERROR_SEARCHING_DOCUMENTS() => $"Unexpected error occured while search documents by query.";
        public static string UNEXPECTED_ERROR_COUNTING_DOCUMENTS() => $"Unexpected error occured while count documents.";
    }
}
