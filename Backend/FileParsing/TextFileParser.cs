using DataImportContracts;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileParsing
{
    public class TextFIleParser : ITextFileParser
    {
        public IEnumerable<AddressInfo> ParseFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            var result = new List<AddressInfo>(lines.Count());

            foreach (string line in lines)
            {
                result.Add(ParseString(line));
            }

            return result;
        }

        private AddressInfo ParseString(string str)
        {
            return new AddressInfo();

        }

    }
}
