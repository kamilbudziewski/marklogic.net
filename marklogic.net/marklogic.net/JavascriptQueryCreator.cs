namespace marklogic.net
{
    internal static class JavascriptQueryCreator
    {
        public static string IngestDocument(string documentJson, DocumentProperties properties)
        {
            return string.Format("declareUpdate();xdmp.documentInsert('{0}', {1});", properties.DocumentUri, documentJson);
        }

        public static string DeleteDocument(string documentUri)
        {
            return string.Format("declareUpdate();xdmp.documentDelete('{0}');", documentUri);
        }
    }
}