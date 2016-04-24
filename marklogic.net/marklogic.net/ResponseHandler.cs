using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace marklogic.net
{
    public class ResponseHandler
    {
        private static readonly int _markLogicHeadersLineCount = 6;
        private static readonly List<string> ResponseLines = new List<string> { Capacity = 250000 }; //large capacity needed for containing some huge responses

        public static string ClearRestResult(Stream text)
        {
            var parts = new List<string>();

            using (var sr = new StreamReader(text))
            {
                var part = new StringBuilder();
                var text1 = sr.ReadLine();
                var separator = sr.ReadLine();
                sr.ReadLine();
                sr.ReadLine();
                sr.ReadLine();

                while ((text1 = sr.ReadLine()) != null)
                {
                    if (text1.StartsWith(separator))
                    {
                        parts.Add(part.ToString());
                        part.Clear();
                        sr.ReadLine();
                        sr.ReadLine();
                        sr.ReadLine();
                    }
                    else
                        part.AppendLine(text1);
                }
                if (part.Length > 0)
                    parts.Add(part.ToString());
            }

            return string.Join("", parts);
        }

        public static IEnumerable<string> ClearRestGarbageFromSinglePartResponse(Stream text)
        {
            ResponseLines.Clear();
            using (var reader = new StreamReader(text))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    ResponseLines.Add(line);
                }
            }
            return ResponseLines.Skip(_markLogicHeadersLineCount)
                .TakeWhile(x => !x.StartsWith("--"));
        }
    }
}