using System.Linq;

namespace ImportDataContracts
{
    public class JsonData
    {
        public bool IsSimpleData { get; set; }
        public string StringData { get; set; }
        public NameValue[] NameValueData { get; set; }
        public Coords Coords
        {
            get
            {
                if (NameValueData == null || !NameValueData.Any())
                {
                    return null;
                }

                NameValue XData = NameValueData.FirstOrDefault(t => t.Name.ToLower() == "x");
                if (XData == null)
                {
                    return null;
                }

                NameValue YData = NameValueData.FirstOrDefault(t => t.Name.ToLower() == "y");
                if (YData == null)
                {
                    return null;
                }

                double x;
                bool xParsed = double.TryParse(XData.Value.Replace(".", ","), out x);
                if (!xParsed)
                {
                    return null;
                }

                double y;
                bool yParsed = double.TryParse(YData.Value.Replace(".", ","), out y);
                if (!yParsed)
                {
                    return null;
                }

                return new Coords { X = x, Y = y };
            }
        }
    }

    public class NameValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Coords
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
