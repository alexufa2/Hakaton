using System;
using System.Collections.Generic;
using System.Text;

namespace ImportDataContracts
{
    public class JsonData
    {
        public bool IsSimpleData { get; set; }
        public string StringData { get; set; }
        public NameValue[] NameValueData { get; set; }
        public Coords Coords { get; set; }
    }

    public class NameValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Coords
    {
        public string X { get; set; }
        public string Y { get; set; }
    }
}
