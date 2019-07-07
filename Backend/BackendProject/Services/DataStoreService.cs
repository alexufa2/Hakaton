using ImportDataContracts;
using System.Collections.Generic;
using System.Linq;
using TemplateApp.DAL;
using TemplateApp.DAL.Entities;

namespace TemplateApp.Services
{
    public interface IDataStoreService
    {
        List<Street> GetStreets();
        void SaveStreet(Street street, StreetJsonInfo json);
        void SaveData(IEnumerable<AddressInfo> data);
        List<string> GetJsonForStreet(int streetId);
        List<Street> GetStreetsByString(string substring);
    }

    public class DataStoreService : IDataStoreService
    {
        ApplicationContext _applicationContext;

        public DataStoreService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public List<Street> GetStreets()
        {
            return _applicationContext.Streets.ToList();
        }

        public List<string> GetJsonForStreet(int streetId)
        {
            return _applicationContext.StreetJsonInfos.Where(x => x.Street.Id == streetId).Select(x => x.Json).ToList();
        }

        public void SaveStreet(Street street, StreetJsonInfo json)
        {
            SaveStreetToDb(street, json, true);
        }

        private void SaveStreetToDb(Street street, StreetJsonInfo json, bool confirm)
        {
            var streetEntry = _applicationContext.Streets.FirstOrDefault(x => x.StreetName.ToLower().Trim() == street.StreetName.ToLower().Trim());

            if (streetEntry == null)
            {
                streetEntry = _applicationContext.Add(new Street()
                {
                    StreetName = street.StreetName,
                    FullAddress = street.FullAddress
                }).Entity;
            }

            _applicationContext.StreetJsonInfos.Add(new StreetJsonInfo() { Street = streetEntry, Json = json.Json });

            if (confirm)
            {
                _applicationContext.SaveChanges();
            }
        }

        public void SaveData(IEnumerable<AddressInfo> data)
        {
            if (data == null || !data.Any())
            {
                return;
            }

            foreach (AddressInfo info in data)
            {
                var street = new Street()
                {
                    StreetName = info.Street,
                    FullAddress = info.Address
                };
                var streetJsonInfo = new StreetJsonInfo { Json = info.JsonInfo };
                SaveStreetToDb(street, streetJsonInfo, false);
            }

            _applicationContext.SaveChanges();
        }

        public List<Street> GetStreetsByString(string substring)
        {
            return _applicationContext.Streets.Where(x =>
                                                 x.FullAddress.ToLower().Contains(
                                                      substring.ToLower().Trim())
                                                ).ToList();
        }
    }
}
