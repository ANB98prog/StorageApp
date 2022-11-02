using Nest;
using Storage.Domain;

namespace ElasticIndexer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             To do
            Need to add some method to create different index
             */
            var elasticUrl = "http://localhost:9200";
            var elasticUser = "elastic";
            var elasticPassword = "";

            var settings = new ConnectionSettings(new Uri(elasticUrl))
                .EnableDebugMode()
                                .BasicAuthentication(elasticUser, elasticPassword);

            var nestClient = new Nest.ElasticClient(settings);

            var client = new Elasticsearch.ElasticClient(nestClient);

            var indexBaseFiles = new IndexBaseFile(client);

            indexBaseFiles.Index("temp");

            indexBaseFiles.Reindex(ElasticIndices.FILES_INDEX, "temp");
        }


    }
}