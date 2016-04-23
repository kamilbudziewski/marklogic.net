using System;
using System.IO;
using System.Net;
using System.Text;

namespace marklogic.net
{
    internal static class MlRestApi
    {
        public static MlResult QueryMarkLogic(MarkLogicConnection connection, string query)
        {

            return new MlResult();
        }

        private static string DoQuery(MarkLogicConnection connection, string query)
        {
            var uribuilder = new UriBuilder("http",connection.Host, connection.Port);
            var request = WebRequest.Create(uribuilder.Uri);
            request.Method = "POST";
            var byteArray = Encoding.UTF8.GetBytes(query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            request.Timeout = connection.Timeout;

            var encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(connection.UserName + ":" + connection.Password));
            request.Headers.Add("Authorization", "Basic " + encoded);


            var dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            var response = request.GetResponse();

            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }
    }
}