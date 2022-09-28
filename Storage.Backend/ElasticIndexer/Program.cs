﻿using Nest;

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
            var elasticPassword = "TsrxH2l62Q90Dfdyi0Nd";

            var settings = new ConnectionSettings(new Uri(elasticUrl))
                                .BasicAuthentication(elasticUser, elasticPassword);

            var nestClient = new Nest.ElasticClient(settings);

            var client = new Elasticsearch.ElasticClient(nestClient);

            var indexBaseFiles = new IndexBaseFile(client);

            indexBaseFiles.Index();
            
        }


    }
}