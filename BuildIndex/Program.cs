using System;
using System.IO;
using System.Text;
using BigDataSearch.Core;

namespace BuildIndex
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: BuildIndex <datafilename> <fieldIndex>");
                return;
            }
            string dataPath = Path.GetFullPath(args[0]);
            int fieldIndex = int.Parse(args[1]);
            string indexPath = Index.GetIndexFileName(dataPath, fieldIndex);

            using (var index = Index.Create(indexPath, Encoding.Default, 1024 * 1024))
            using (var sReader = new StreamReader(dataPath, Encoding.Default))
            using (var tReader = new TrackingTextReader(sReader))
            {
                string line;
                long position = tReader.Position;
                while ((line = tReader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    string value = parts[fieldIndex];
                    index.Add(value.ToLower(), position);
                    position = tReader.Position;
                }
            }
        }
    }
}
