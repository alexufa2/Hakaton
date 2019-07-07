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
        IEnumerable<AddressInfo> ParseFile(string filePath, string serviceName);
        AddressInfo ParseString(string str, string serviceName);
    }


    public class TextFileParser : ITextFileParser
    {
        private Regex _streetRegExp = new Regex(@"(ул)|(мкрн)|(микрорайон)|(пос)|(пр)");
        private readonly string[] Splitters = new string[] { ",", ";" };


        public IEnumerable<AddressInfo> ParseFile(string filePath, string serviceName)
        {
            string[] lines = File.ReadAllLines(filePath);
            var result = new List<AddressInfo>(lines.Length);

            string[] header = Split(lines[0]);
            string[] lineParts = Split(lines[1]);

            int addrIndex = GetAddrIndex(lineParts);
            if (addrIndex < 0)
            {
                return result;
            }

            AddressInfo data = null;
            // index c 1 чтобы пропустить первую строку - заголовок
            for (uint index = 1; index < lines.Length; index++)
            {
                data = ParseString(lines[index], header, addrIndex, serviceName);
                if (data != null)
                {
                    result.Add(data);
                }
            }

            return result;
        }

        public AddressInfo ParseString(string str, string serviceName)
        {
            string[] lineParts = Split(str);
            int addrIndex = GetAddrIndex(lineParts);
            return ParseString(str, null, addrIndex, serviceName);
        }

        private AddressInfo ParseString(string str, string[] header, int addrIndex, string serviceName)
        {
            if (string.IsNullOrEmpty(str) || addrIndex < 0)
            {
                return null;
            }

            try
            {
                string[] parts = Split(str);
                List<string> list = new List<string>(parts);
                string street = GetStreet(list[addrIndex]);

                //оставим весь адрес
                //int streetIndex = list[addrIndex].IndexOf(street);
                //string addressFromStreet = list[addrIndex].Substring(streetIndex);
                //list[addrIndex] = addressFromStreet;

                return new AddressInfo
                {
                    Street = street,
                    Address = list[addrIndex],
                    JsonInfo = GetJson(list.ToArray(), header, serviceName)
                };
            }
            catch
            {
                return null;
            }
        }

        private string[] Split(string str)
        {
            return str.Split(Splitters, StringSplitOptions.None)
                      .Select(s => s.Replace("\"", string.Empty).Replace("  ", " "))
                      .ToArray();
        }

        private int GetAddrIndex(string[] lineParts)
        {
            int result = -1;
            int index = 0;
            bool finded = false;

            do
            {
                Match match = _streetRegExp.Match(lineParts[index].ToLower());
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

        private string GetJson(string[] lineParts, string[] header, string serviceName)
        {
            JsonData jsonData = null;
            bool headerIsValid = header != null && header.Any();
            bool sizeEquals = lineParts.Length == header.Length;

            if (!sizeEquals || !headerIsValid)
            {
                jsonData = new JsonData
                {
                    IsSimpleData = true,
                    StringData = string.Join(", ", lineParts),
                    NameValueData = new NameValue[0],
                    ServiceName = serviceName
                };

                return JsonConvert.SerializeObject(jsonData);
            }

            List<NameValue> list = new List<NameValue>(header.Length);
            for (uint index = 0; index < header.Length; index++)
            {
                list.Add(new NameValue
                {
                    Name = header[index],
                    Value = lineParts[index]
                });
            }

            jsonData = new JsonData
            {
                IsSimpleData = false,
                StringData = string.Empty,
                NameValueData = list.ToArray(),
                ServiceName = serviceName
            };
            return JsonConvert.SerializeObject(jsonData);
        }

    }
}
