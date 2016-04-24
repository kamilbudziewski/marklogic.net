using System;

namespace marklogic.net
{
    public class MarkLogicException : Exception
    {
        public MarkLogicException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}