using System.Collections.Generic;
using marklogic.net;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new MarkLogicConnection("localhost", "admin", "water1", 8091, 50000);

            using (var sesion = connection.OpenSession())
            {
                sesion.IngestDocument(new { name = "asd" }, new DocumentProperties() { DocumentUri = "test.json", Permissions = new List<Permission>() });
            }
        }
    }
}
