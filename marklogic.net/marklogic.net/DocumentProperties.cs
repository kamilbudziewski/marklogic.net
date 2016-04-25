using System.Collections.Generic;

namespace marklogic.net
{
    public class DocumentProperties
    {
        public DocumentProperties()
        {
            Collections = new List<string>();
            Permissions = new List<Permission>();
        }

        public string DocumentUri { get; set; }
        public List<Permission> Permissions { get; set; }
        public List<string> Collections { get; set; }
    }
}