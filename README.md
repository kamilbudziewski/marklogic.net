# marklogic.net

marklogic.net is an API for Marklogic

# How to connect to database
 
you need to provide:
- marklogic server host address
- account
- password
- REST API port
- query timeout
```cs
var connection = new MarkLogicConnection(http://marklogicserver.com, "admin", "pass", 8091, 50000);
```

# Loading document to Marklogic

you need to open session

```cs
using (var session = connection.OpenSession())
{
...
```

Ingest document accepts any type of document which will be serialized by JSON.net library
```cs
MlResult IngestDocument<T>(T document, DocumentProperties properties, string database = null)
```
Document properties define 
- DocumentUri - identifier of document laoded to Marklogic (obligatory parameter)
- Permissions - custom permissioning for document
- Collections - set of collections assigned to document
```cs
  public class DocumentProperties
  {
    public string DocumentUri { get; set; }
    public List<Permission> Permissions { get; set; }
    public List<string> Collections { get; set; }
  }
```

database parameter allows you to load document to database which is not connected to REST API provided in connection. It is possible by using ```xdmp.eval()```

Sample call
```cs 
var result = session.IngestDocument(new {id = 5, text = "ddasdad"}, new DocumentProperties() {DocumentUri = "/testdocument.json"});
```


