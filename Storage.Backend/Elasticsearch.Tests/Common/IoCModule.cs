using Moq;
using Nest;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elasticsearch.Tests.Common
{
    public class IoCModule : NinjectModule
    {
        public Mock<IElasticClient> ElasticClientMock;

        public IoCModule()
        {
            ElasticClientMock = new Mock<IElasticClient>();
        }

        public override void Load()
        {
        }
    }
}
