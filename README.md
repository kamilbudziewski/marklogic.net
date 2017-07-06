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

# Sample call

```cs 
var result = session.IngestDocument(new {id = 5, text = "ddasdad"}, new DocumentProperties() {DocumentUri = "/testdocument.json"});
```

# Return
  ```cs
  public class MlResult
  {
    public bool Success { get; set; }

    public Exception Exception { get; set; }

    public string StringResult { get; set; }
  }
```

# Getting documents from Marklogic
You can use strongly typed method `GetDocument` which returns document with given URI
```cs
T GetDocument<T>(string docId, string database = null) where T : new()
...
var document = session.GetDocument<MyDocumentType>("/testdocument.json");
```

OR

strongly typed `Query` method, which will return document(s) for any javascript query

```cs
T Query<T>(string query, string database = null) where T : new()
...
var document = session.Query<MyDocumentType>("fn.doc('/testdocument.json')");
```


# Query for any result 

You can query marklogic for any result, it will be returned as string

```cs
MlResult QueryString(string query, string database = null)
...
var result = session.QueryString("cts.search(cts.jsonPropertyValueQuery('source', 'news'))")
```

# Deleting document

You can delete document by URI

```cs
MlResult DeleteDocument(string documentUri, string database = null)
...
var result = session.DeleteDocument("/testdocument.json");
```

