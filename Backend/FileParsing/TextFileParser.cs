using ImportDataContracts;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System;
using Newtonsoft.Json;

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
            string[] lines = File.ReadAllLines(filePath);
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
            try
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
            catch
            {
                return null;
            }
        }

        private string[] Split(string str)
        {
            return str.Split(_splitters, StringSplitOptions.None)
                                       .Select(s =>
                                       s.Replace("\"", string.Empty)
                                       .Replace("  ", " "))
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
                result = index;
                index++;
            }
            while (!finded && index < lineParts.Length);

            return result;
        }

        private string GetStreet(string part)
        {
            Match match = _streetRegExp.Match(part);
            int firstSpaceInd = part.IndexOf(" ", match.Index);
            int secondSpaceInd = part.IndexOf(" ", firstSpaceInd + 1);
            return part.Substring(match.Index, secondSpaceInd - match.Index);
        }

        private string GetJson(string[] lineParts, string[] headers)
        {
            if (headers == null || !headers.Any())
            {
                var jsonData = new JsonData
                {
                    IsSimpleData = true,
                    StringData = string.Join(", ", lineParts)
                };

                return JsonConvert.SerializeObject(jsonData);
            }

            List<NameValue> list = new List<NameValue>(headers.Length);
            for (uint index=0; index<  headers.Length; index++) {

            }
            return "";
        }


    }


}
