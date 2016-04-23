using System;
using System.IO;
using Newtonsoft.Json;

namespace marklogic.net
{
    public class MarkLogicSession : IDisposable
    {
        private MarkLogicConnection _connection;

        public MarkLogicSession(MarkLogicConnection connection)
        {
            _connection = connection;
        }

        public T QueryString<T>(string query) where T : new()
        {
            return new T();
        }

        public void IngestDocument<T>(T document, DocumentProperties properties)
        {
            var documentJson = JsonConvert.SerializeObject(document);

            
        }

        public void Dispose()
        {

        }
    }
}