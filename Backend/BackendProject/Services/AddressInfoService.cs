using ImportDataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateApp.Services
{
    public interface IAddressInfoService
    {
        IEnumerable<AddressInfo> GetAll();
        IEnumerable<AddressInfo> FilterByAddress(string address);
    }

    public class AddressInfoService : IAddressInfoService
    {
        private IFileService _fileService;
        private IEnumerable<AddressInfo> _data;

        public AddressInfoService(IFileService fileService)
        {
            _fileService = fileService;
            string json = _fileService.ReadFileByName(GlobalConstants.DataFileName);
            _data = JsonConvert.DeserializeObject<IEnumerable<AddressInfo>>(json);
            _data = _data.OrderBy(d => d.Street);
        }

        public IEnumerable<AddressInfo> FilterByAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                return GetAll();
            }

            return _data.Where(w => w.Address.ToLower().Contains(address.ToLower()))
                        .OrderBy(o => o.Street);
        }

        public IEnumerable<AddressInfo> GetAll()
        {
            return _data;
        }
    }
}
