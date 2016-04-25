using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using marklogic.net;
using marklogic.net.Linq;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {


            //            var connection = new MarkLogicConnection("localhost", "admin", "water1", 8091, 50000);
            var connection = new MarkLogicConnection("gda-marklogic-20", "test_user", "water1", 8091, 50000);


            QueryProvider provider = new MlQueryProvider(connection);

            var a = new Query<DummyDocument>(provider);

            var aresult = a.Where(x => x.Name == "asd");

            var ennnn = aresult.ToList();

            using (var session = connection.OpenSession())
            {
                var result3 = session.IngestDocument(new DummyDocument() { Default = 5, Name = "test doc", Type = "test document" }, new DocumentProperties() { DocumentUri = "brrrr.json", Permissions = new List<Permission>() });

                var result = session.Query<DummyDocument>("fn.doc('brrrr.json')");

                var mmm = session.QueryString("fn.doc('brrrr.json')");

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
