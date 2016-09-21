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

        public T Query<T>(string query, string database = null) where T : new()
        {
            StartTimer();
            var result = MlRestApi.QueryMarkLogic(_connection, query, database);
            StopTimer();
            if (result.Success)
            {
                var deserialzedObject = JsonConvert.DeserializeObject<T>(result.StringResult);
                return deserialzedObject;
            }

            throw new MarkLogicException("Exception while querying marklogic", result.Exception);
        }

        public T GetDocument<T>(string docId, string database = null) where T : new()
        {
            StartTimer();
            var result = MlRestApi.QueryMarkLogic(_connection, string.Format("fn.doc('{0}')", docId), database);
            StopTimer();
            if (result.Success)
            {
                var deserialzedObject = JsonConvert.DeserializeObject<T>(result.StringResult);
                return deserialzedObject;
            }

            throw new MarkLogicException("Exception while querying marklogic", result.Exception);
        }

        internal Query<T> Linq<T>(string collection = "")
        {
            QueryProvider provider = new MlQueryProvider(_connection, collection);
            return new Query<T>(provider);
        }

        public MlResult QueryString(string query, string database = null)
        {
            StartTimer();
            var result = MlRestApi.QueryMarkLogic(_connection, query, database);
            StopTimer();
            return result;
        }

        public MlResult IngestDocument<T>(T document, DocumentProperties properties, string database = null)
        {
            if (string.IsNullOrEmpty(properties.DocumentUri))
            {
                throw new ArgumentException("Properties.DocumentUri can not be empty");
            }
            StartTimer();
            var documentJson = JsonConvert.SerializeObject(document);

            var result = MlRestApi.QueryMarkLogic(_connection, JavascriptQueryCreator.IngestDocument(documentJson, properties), database);
            StopTimer();
            return result;
        }

        public MlResult DeleteDocument(string documentUri, string database = null)
        {
            StartTimer();
            var result = MlRestApi.QueryMarkLogic(_connection, JavascriptQueryCreator.DeleteDocument(documentUri), database);
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
            LastExecutionTime = (int?)_stopwatch.ElapsedMilliseconds;
        }

        public void Dispose()
        {

        }
    }
}