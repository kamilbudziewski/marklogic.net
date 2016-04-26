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
            var connection = new MarkLogicConnection("gda-marklogic-20", "test_user", "water1", 8108, 50000);

            using (var session = connection.OpenSession())
            {
                var result3 = session.IngestDocument(new DummyDocument() { Default = 5, Name = "asd", Type = "test document", Inner = new Inner() { InnerName = "brd" } }, new DocumentProperties() { DocumentUri = "brrrr2.json", Permissions = new List<Permission>() });
                var result = session.IngestDocument(new DummyDocument() { Default = 5, Name = "asd2", Type = "test document", Inner = new Inner() { InnerName = "cdd" } }, new DocumentProperties() { DocumentUri = "brrrr.json", Permissions = new List<Permission>() });
                //                var result4 = session.IngestDocument(new DummyDocument() { Default = 6, Name = "asd", Type = "ssstest document" }, new DocumentProperties() { DocumentUri = "brrrr2.json", Permissions = new List<Permission>() });

                var sss = session.Linq<DummyDocument>("asd").Where(x => x.Name == "asd" || x.Name=="asd2" || x.Name=="bcd");

                var rr = sss.ToList();

                //                var result = session.Query<DummyDocument>("fn.doc('brrrr.json')");

                //                var mmm = session.QueryString("fn.doc('brrrr.json')");

                var result2 = session.DeleteDocument("brrrr.json");
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
