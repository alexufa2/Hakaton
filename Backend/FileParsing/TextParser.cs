using DataImportContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileParsing
{
    public static class TextParser
    {
        public static IEnumerable<AddressInfo> ParseFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            return ParseLines(lines);
        }

        public static IEnumerable<AddressInfo> ParseLines(IEnumerable<string> lines)
        {
            var result = new List<AddressInfo>(lines.Count());

            foreach (string line in lines)
            {
                result.Add(ParseString(line));
            }

            return result;
        }


        private static AddressInfo ParseString(string str)
        {
            return new AddressInfo();

        }

    }
}
