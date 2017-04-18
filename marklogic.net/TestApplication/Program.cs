using System.Collections.Generic;
using System.Linq;
using marklogic.net;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {


            //                        var connection = new MarkLogicConnection("localhost", "admin", "water1", 8091, 50000);
            var connection = new MarkLogicConnection("gda-marklogic-20", "kamil", "legion88", 8091, 50000);

            using (var session = connection.OpenSession())
            {
//                var result3 = session.IngestDocument(new DummyDocument() { Default = 5, Name = "asd", Type = "test document", Inner = new Inner() { InnerName = "brd" } }, new DocumentProperties() { DocumentUri = "brrrr2.json", Permissions = new List<Permission>() });
//                var result = session.IngestDocument(new DummyDocument() { Default = 5, Name = "asd2", Type = "test document", Inner = new Inner() { InnerName = "cdd" } }, new DocumentProperties() { DocumentUri = "brrrr.json", Permissions = new List<Permission>() });
                //                var result4 = session.IngestDocument(new DummyDocument() { Default = 6, Name = "asd", Type = "ssstest document" }, new DocumentProperties() { DocumentUri = "brrrr2.json", Permissions = new List<Permission>() });

//                                var resultasda = session.GetDocument<dynamic>("/Query_Granting_38163mon.json");
//                                var resultasda = session.QueryString("cts.uriMatch('/Query_Granting_*')");
                                var resultasda = session.GetDocumentTimestamp("/Query_Granting_-22gig.json");

                //                var mmm = session.QueryString("fn.doc('brrrr.json')");

//                var result2 = session.DeleteDocument("brrrr.json");
                //                var result555 = session.DeleteDocument("brrrr2.json");

            }
        }
    }

    public class DummyDocument
    {
        public Inner Inner { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }
        public int Default { get; set; }
    }

    public class Inner
    {
        public string InnerName { get; set; }
    }
}
