using System;

namespace marklogic.net
{
    internal class MlResult
    {
        public bool Success { get; set; }
        public Exception Exception { get; set; }
        public string StringResult { get; set; }
    }
}