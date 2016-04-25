using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace marklogic.net
{
    public static class JavascriptQueryCreator
    {
        public static string IngestDocument(string documentJson, DocumentProperties properties)
        {
            return string.Format("declareUpdate();xdmp.documentInsert('{0}', {1}, {2}, {3});", properties.DocumentUri, documentJson, PermissionBuilder.CreatePermissionsTable(properties.Permissions), CollectionBuilder.CreateCollectionsTable(properties.Collections));
        }

        public static string DeleteDocument(string documentUri)
        {
            return string.Format("declareUpdate();xdmp.documentDelete('{0}');", documentUri);
        }
    }
}