using Elasticsearch.Interfaces;
using Nest;
using Storage.Domain;
using Task = System.Threading.Tasks.Task;

namespace ElasticIndexer
{
    /// <summary>
    /// Creates index <see cref="ElasticIndices.FILES_INDEX"/>
    /// </summary>
    public class IndexBaseFile : IIndex
    {
        private readonly IElasticsearchClient _elasticsearchClient;

        public IndexBaseFile(IElasticsearchClient elasticsearchClient)
        {
            _elasticsearchClient = elasticsearchClient;
        }

        public void Index(string indexName)
        {
            var task = Task.Run(() =>
                           _elasticsearchClient
                                            .CreateIndexAsync(indexName, d => d
                                                .Map<BaseFile>(m => m
                                                    .Properties(ps => ps
                                                        .Keyword(k => k
                                                            .Name(n => n.MimeType)))
                                                .AutoMap())));
            task.Wait();
        }

        public void Reindex(string sourceIndex, string destIndex)
        {
            var task = Task.Run(() => _elasticsearchClient.ReindexAsync(sourceIndex, destIndex));                                            
            task.Wait();
            task = Task.Run(() => _elasticsearchClient.DeleteIndexAsync(sourceIndex));
            task.Wait();
            Index(sourceIndex);
            task = Task.Run(() => _elasticsearchClient.ReindexAsync(destIndex, sourceIndex));
            task.Wait();
            task = Task.Run(() => _elasticsearchClient.DeleteIndexAsync(destIndex));
            task.Wait();
        }

        public void ReIndex()
        {
            throw new NotImplementedException();
        }
    }
}
