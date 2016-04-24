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

            var result = MlRestApi.QueryMarkLogic(_connection, JavascriptQueryCreator.IngestDocument(documentJson, properties));

            if (result.Success)
            {

            }
            else
            {

            }
        }

        public void Dispose()
        {

        }
    }

    public class MarkLogicException : Exception
    {

    }
}

/*        public static Stream CallRESTEndpointWithParameters(string endpointName, string parameters, string databaseId, ILog log = null)
        {
            var uriForEndpoint = ConfigurationProvider.GetUri(databaseId, endpointName);
            if (log != null) log.InfoFormat("Calling search on address: {0} with parameters: {1}.", uriForEndpoint, parameters);

            var request = (HttpWebRequest)WebRequest.Create(uriForEndpoint);
            request.Method = "POST";
            request.Accept = "multipart/mixed";
            request.Headers.Add("Authorization", ConfigurationProvider.GetCredentials(databaseId));
            request.ContentType = "application/x-www-form-urlencoded";

            byte[] formData = Encoding.UTF8.GetBytes(parameters);
            request.ContentLength = formData.Length;
            using (Stream post = request.GetRequestStream())
            {
                post.Write(formData, 0, formData.Length);
            }

            try
            {
                var response = request.GetResponse();
                var str = response.GetResponseStream();
                return str;
            }
            catch (Exception exc)
            {
                throw new MLRestException(exc, string.Format("Failed call to MarkLogic on adress: {0}",uriForEndpoint));
            }
        }
        
       public static Stream GetResponseStreamWithWebExceptionWrapping(HttpWebRequest request)
            {
                try
                {
                    var response = (HttpWebResponse)request.GetResponse();
                    return response.GetResponseStream();
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        var responseStream = ex.Response.GetResponseStream();
                        if (responseStream != null)
                        {
                            var response = new StreamReader(responseStream).ReadToEnd();
                            var doc = new HtmlDocument();
                            doc.LoadHtml(response);
                            var stringBuilder = new StringBuilder();
                            var nodes = doc.DocumentNode.SelectNodes("//dt");
                            if (nodes != null)
                            {
                                foreach (var node in nodes)
                                {
                                    stringBuilder.AppendLine(node.InnerText);
                                }
                            }
                            else
                            {
                                stringBuilder.Append(response);
                            }
                            var message = stringBuilder.ToString();
                            throw new MLRestException(ex, message);
                        }
                    }
                    throw new MLRestException(ex, MLRestException.NoMoreInfo);
                }
            }*/
