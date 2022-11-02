namespace ElasticIndexer
{
    public interface IIndex
    {
        public void Index(string indexName);
        public void Reindex(string source, string dest);
    }
}
