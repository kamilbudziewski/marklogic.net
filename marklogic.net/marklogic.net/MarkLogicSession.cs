using System;
using System.Diagnostics;
using marklogic.net.Linq;
using Newtonsoft.Json;

namespace marklogic.net
{
    public class MarkLogicSession : IDisposable
    {
        private readonly MarkLogicConnection _connection;
        private readonly Stopwatch _stopwatch;

        public int? LastExecutionTime { get; set; }

        public MarkLogicSession(MarkLogicConnection connection)
        {
            _connection = connection;
            _stopwatch = new Stopwatch();
        }

        public T Query<T>(string query) where T : new()
        {
            StartTimer();
            var result = MlRestApi.QueryMarkLogic(_connection, query);
            StopTimer();
            if (result.Success)
            {
                var deserialzedObject = JsonConvert.DeserializeObject<T>(result.StringResult);
                return deserialzedObject;
            }

            throw new MarkLogicException("Exception while querying marklogic", result.Exception);
        }

        public Query<T> Linq<T>()
        {
            QueryProvider provider = new MlQueryProvider(_connection);
            return new Query<T>(provider);
        }

        public MlResult QueryString(string query)
        {
            StartTimer();
            var result = MlRestApi.QueryMarkLogic(_connection, query);
            StopTimer();
            return result;
        }

        public MlResult IngestDocument<T>(T document, DocumentProperties properties)
        {
            StartTimer();
            var documentJson = JsonConvert.SerializeObject(document);

            var result = MlRestApi.QueryMarkLogic(_connection, JavascriptQueryCreator.IngestDocument(documentJson, properties));
            StopTimer();
            return result;
        }

        public MlResult DeleteDocument(string documentUri)
        {
            StartTimer();
            var result = MlRestApi.QueryMarkLogic(_connection, JavascriptQueryCreator.DeleteDocument(documentUri));
            StopTimer();
            return result;
        }

        private void StartTimer()
        {
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        private void StopTimer()
        {
            _stopwatch.Stop();
            LastExecutionTime = (int?) _stopwatch.ElapsedMilliseconds;
        }

        public void Dispose()
        {

        }
    }
}