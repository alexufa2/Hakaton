using Newtonsoft.Json;

namespace ImportDataContracts
{
    public class AddressInfo
    {
        public string Address { get; set; }
        public string Street { get; set; }
        public string JsonInfo { get; set; }

        public JsonData GetJsonData()
        {
            if (string.IsNullOrEmpty(JsonInfo))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<JsonData>(JsonInfo);
        }
    }

}
