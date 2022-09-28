using Elasticsearch.Interfaces;
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

        public void Index()
        {
            var task = Task.Run(() =>
                           _elasticsearchClient
                                            .CreateIndexAsync(ElasticIndices.FILES_INDEX, d => d
                                                .Map<BaseFile>(m => m
                                                .AutoMap())));
            task.Wait();
        }

        public void ReIndex()
        {
            throw new NotImplementedException();
        }
    }
}
