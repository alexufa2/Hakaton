using ImportDataContracts;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System;

namespace FileParsing
{
    public interface ITextFileParser
    {
        IEnumerable<AddressInfo> ParseFile(string filePath);
        AddressInfo ParseString(string str);
    }


    public class TextFileParser : ITextFileParser
    {
        private Regex _streetRegExp = new Regex(@"(ул)|(мкрн)|(микрорайон)");
        private string[] _splitters = new string[] { ",", ";" };


        public IEnumerable<AddressInfo> ParseFile(string filePath)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding enc1252 = Encoding.GetEncoding(1252);
            string[] lines = File.ReadAllLines(filePath, enc1252);
            var result = new List<AddressInfo>(lines.Length);

            string[] header = Split(lines[0]);
            string[] lineParts = Split(lines[1]);

            int streetIndex = GetStreetIndex(lineParts);
            if (streetIndex < 0)
            {
                return result;
            }

            AddressInfo data = null;
            // index c 1 чтобы пропустить первую строку - заголовок
            for (uint index = 1; index < lines.Length; index++)
            {
                data = ParseString(lines[index], header, streetIndex);
                if (data != null)
                {
                    result.Add(data);
                }
            }

            return result;
        }

        public AddressInfo ParseString(string str)
        {
            string[] lineParts = Split(str);
            int streetIndex = GetStreetIndex(lineParts);
            if (streetIndex < 0)
            {
                return null;
            }

            return ParseString(str, null, streetIndex);
        }

        private AddressInfo ParseString(string str, string[] header, int streetIndex)
        {
            string[] parts = Split(str);
            List<string> list = new List<string>(parts);
            string street = GetStreet(list[streetIndex]);
            string partWithoutStreet = list[streetIndex].Replace(street, string.Empty);
            list[streetIndex] = partWithoutStreet;

            return new AddressInfo
            {
                Street = street,
                JsonInfo = GetJson(list.ToArray(), header)
            };
        }

        private string[] Split(string str)
        {
            return str.Split(_splitters, StringSplitOptions.None)
                                       .Select(s => s.Replace("\"", string.Empty))
                                       .ToArray();
        }

        private int GetStreetIndex(string[] lineParts)
        {
            int result = -1;
            int index = 0;
            bool finded = false;

            do
            {
                Match match = _streetRegExp.Match(lineParts[index]);
                finded = match.Success;
                result = match.Index;
                index++;
            }
            while (!finded && index < lineParts.Length);

            return result;
        }

        private string GetStreet(string part)
        {
            Match match = _streetRegExp.Match(part);
            int length = part.IndexOf(" ", match.Index);
            return part.Substring(match.Index, length);
        }

        private string GetJson(string[] lineParts, string[] header)
        {
            if (header == null || !header.Any())
            {
                return string.Join(", ", lineParts);
            }

            return "";
        }


    }


}
