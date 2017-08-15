using System.Collections.Generic;

namespace UsdaCosmos
{
    // Parses lines like this: ~001~^~This is a test~ to this: [["001", "This is a test"]]
    public class Parser 
    {
        public string[][] Parse(string src)
        {
            var documents = new List<string[]>();
            var lines = src.Split(new char[] { '\r', '\n'});
            foreach(var line in lines) {
                if (line.Trim() != "") {
                    var fields = line.Split('^');
                    var document = new List<string>();
                    foreach(var field in fields)
                    {
                        document.Add(field.Replace('~', ' ').Trim());
                    }
                    documents.Add(document.ToArray());
                }
            }
            return documents.ToArray();
        }
    }
}