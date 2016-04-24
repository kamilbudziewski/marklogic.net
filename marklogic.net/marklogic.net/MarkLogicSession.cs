using System;
using Newtonsoft.Json;

namespace marklogic.net
{
    public class MarkLogicSession : IDisposable
    {
        private readonly MarkLogicConnection _connection;

        public MarkLogicSession(MarkLogicConnection connection)
        {
            _connection = connection;
        }

        public T Query<T>(string query) where T : new()
        {
            var result = MlRestApi.QueryMarkLogic(_connection, query);

            if (result.Success)
            {
                var deserialzedObject = JsonConvert.DeserializeObject<T>(result.StringResult);
                return deserialzedObject;
            }

            throw new MarkLogicException("Exception while querying marklogic", result.Exception);
        }

        public MlResult QueryString(string query)
        {
            var result = MlRestApi.QueryMarkLogic(_connection, query);

            return result;
        }

        public MlResult IngestDocument<T>(T document, DocumentProperties properties)
        {
            var documentJson = JsonConvert.SerializeObject(document);

            var result = MlRestApi.QueryMarkLogic(_connection, JavascriptQueryCreator.IngestDocument(documentJson, properties));

            return result;
        }

        public void Dispose()
        {

        }
    }

    public class MarkLogicException : Exception
    {
        public MarkLogicException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}