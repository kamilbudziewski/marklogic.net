using System.Collections.Generic;
using marklogic.net;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new MarkLogicConnection("localhost", "admin", "water1", 8091, 50000);

            using (var session = connection.OpenSession())
            {
                session.IngestDocument(new DummyDocument() { Default = 5, Name = "test doc", Type = "test document" }, new DocumentProperties() { DocumentUri = "brrrr.json", Permissions = new List<Permission>() });

                var result = session.Query<DummyDocument>("fn.doc('brrrr.json')");

                var result2 = session.DeleteDocument("brrrr.json");
            }
        }
    }

    public class DummyDocument
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Default { get; set; }
    }
}
