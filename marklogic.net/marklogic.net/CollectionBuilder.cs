using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace marklogic.net
{
    public static class CollectionBuilder
    {
        public static string CreateCollectionsTable(List<string> collections)
        {
            if (!collections.Any())
            {
                return "xdmp.defaultCollections()";
            }

            var sb = new StringBuilder("[");
            foreach (var collection in collections)
            {
                sb.AppendFormat("\"{0}\",", collection);
            }
            var result = sb.ToString().TrimEnd(',') + "]";

            return result;
        }
    }
}
