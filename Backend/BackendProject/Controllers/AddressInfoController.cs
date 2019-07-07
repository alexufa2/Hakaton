using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImportDataContracts;
using Microsoft.AspNetCore.Mvc;
using TemplateApp.Models;
using TemplateApp.Services;

namespace TemplateApp.Controllers
{
    [Route("api/[controller]")]
    public class AddressInfoController : ControllerBase
    {
        private IAddressInfoService _service;

        public AddressInfoController(IAddressInfoService service)
        {
            _service = service;
        }


        [HttpGet("get")]
        [IgnoreAntiforgeryToken]
        public IActionResult GetAll()
        {
            IEnumerable<AddressInfo> data = _service.GetAll();
            return Ok(data);
        }

        // GET api/<controller>/5
        [HttpPost("filter")]
        [IgnoreAntiforgeryToken]
        public IActionResult Filter([FromBody] AddressInfoFilter filter)
        {
            IEnumerable<AddressInfo> data = _service.FilterByAddress(filter.Address);
            return Ok(data);
        }

    }
}
