using Nest;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //var settings = new ConnectionSettings(new Uri("http://0.0.0.0:9200"));
            var connectionSettings = new ConnectionSettings()
                .DefaultMappingFor<Post>(i => i
                    .IndexName("post")
                    .TypeName("post"))
                .EnableDebugMode()
                .PrettyJson()
                .ThrowExceptions(alwaysThrow: true)
                .RequestTimeout(TimeSpan.FromSeconds(5));

            var client = new ElasticClient(connectionSettings);

            if (!client.IndexExists("post").Exists)
            {
                var createIndexResponse = client.CreateIndex("post", c => c
                            .Mappings(ms => ms
                                .Map<Post>(m => m
                                    .AutoMap()
                                )
                            ));

            }
            var guid = Guid.NewGuid();
            var response =  client.IndexDocument<Post>(new Post { Name = "aga", ElasticGuid = guid });
            response.Result.ToString();


        }

    }

    [ElasticsearchType(Name = "post")]
    public class Post
    {
        [Ignore]
        public Guid ElasticGuid { get; set; }
        [Text]
        public string Name { get; set; }
    }

}
