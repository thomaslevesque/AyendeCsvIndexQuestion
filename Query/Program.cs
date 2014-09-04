using System;
using System.IO;
using System.Linq;
using System.Text;
using BigDataSearch.Core;

namespace Query
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: Query <datafilename> <fieldIndex> [<searchTerm>]");
                return;
            }

            string dataPath = Path.GetFullPath(args[0]);
            int fieldIndex = int.Parse(args[1]);
            string searchTerm = null;
            bool interactive = true;
            if (args.Length > 2)
            {
                searchTerm = args[2];
                interactive = false;
            }
            string indexPath = Index.GetIndexFileName(dataPath, fieldIndex);

            using (var index = Index.Open(indexPath, Encoding.Default))
            using (var reader = new StreamReader(dataPath, Encoding.Default))
            {
                do
                {
                    if (interactive)
                    {
                        Console.Write("Enter search term: ");
                        searchTerm = Console.ReadLine();
                        if (string.IsNullOrEmpty(searchTerm))
                            break;
                    }

                    var offsets = index.Query(searchTerm.ToLower()).ToList();
                    if (offsets.Count == 0)
                    {
                        Console.WriteLine("No results found");
                    }
                    else
                    {
                        foreach (long offset in offsets)
                        {
                            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
                            reader.DiscardBufferedData();
                            string line = reader.ReadLine();
                            Console.WriteLine(line);
                        }
                    }

                // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
                } while (interactive);
            }
        }
    }
}
