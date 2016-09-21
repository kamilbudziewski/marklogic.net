using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace marklogic.net
{
    internal static class MlRestApi
    {
        public static MlResult QueryMarkLogic(MarkLogicConnection connection, string query, string database = null)
        {
            try
            {
                var result = DoQuery(connection, query, database);
                return new MlResult()
                {
                    StringResult = result,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new MlResult()
                {
                    Exception = ex,
                    Success = false
                };
            }
        }

        private static string DoQuery(MarkLogicConnection connection, string query, string database)
        {
            if (database != null)
            {
                query = string.Format(@"xdmp.eval(""{0}"", [], {{""database"":xdmp.database(""{1}"")}})", query.Replace("\"", "\\\""), database);
            }

            var uribuilder = new UriBuilder("http", connection.Host, connection.Port, "/LATEST/eval");
            var request = WebRequest.Create(uribuilder.Uri);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = connection.Timeout;

            var encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(connection.UserName + ":" + connection.Password));
            request.Headers.Add("Authorization", "Basic " + encoded);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string data = string.Format("{0}={1}%0A", "javascript", HttpUtility.UrlEncode(query));
                streamWriter.Write(data);
            }
            var response = request.GetResponse();

            var dataStream = response.GetResponseStream();
            var result = ResponseHandler.ClearRestResult(dataStream);

            dataStream.Close();
            response.Close();

            return result;
        }
    }
}