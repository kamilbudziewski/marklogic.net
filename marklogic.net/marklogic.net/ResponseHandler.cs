using System.Collections.Generic;
using System.IO;
using System.Text;

namespace marklogic.net
{
    public class ResponseHandler
    {
        public static string ClearRestResult(Stream stream)
        {
            var parts = new List<string>();
            var part = new StringBuilder();

            using (var sr = new StreamReader(stream))
            {
                sr.ReadLine();
                var separator = sr.ReadLine();
                sr.ReadLine();
                sr.ReadLine();
                sr.ReadLine();

                string text;
                while ((text = sr.ReadLine()) != null)
                {
                    if (separator != null && text.StartsWith(separator))
                    {
                        parts.Add(part.ToString());
                        part.Clear();
                        sr.ReadLine();
                        sr.ReadLine();
                        sr.ReadLine();
                    }
                    else
                        part.AppendLine(text);
                }
                if (part.Length > 0)
                    parts.Add(part.ToString());
            }

            return string.Join("", parts);
        }
    }
}