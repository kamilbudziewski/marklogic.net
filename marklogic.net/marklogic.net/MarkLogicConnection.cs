using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace marklogic.net
{
    public class MarkLogicConnection
    {
        private string _host;
        private string _userName;
        private string _password;
        private int _port;

        public MarkLogicConnection(string host, string userName, string password, int port)
        {
            _host = host;
            _userName = userName;
            _password = password;
            _port = port;
        }

        public MarkLogicSession OpenSession()
        {
            return new MarkLogicSession(this);
        }
    }

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

        public void Dispose()
        {

        }
    }
}
