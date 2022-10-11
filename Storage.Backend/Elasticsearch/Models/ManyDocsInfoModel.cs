using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elasticsearch.Models
{
    public class ManyDocsInfoModel<TDocument> where TDocument : class
    {
        public IEnumerable<TDocument> Docs { get; set; }

        public IEnumerable<GetErrorModel> Errors { get; set; }
    }

    public class GetErrorModel
    {

    }
}
