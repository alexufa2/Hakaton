using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImportDataContracts;
using Microsoft.AspNetCore.Mvc;
using TemplateApp.DAL.Entities;
using TemplateApp.Models;
using TemplateApp.Services;

namespace TemplateApp.Controllers
{
    [Route("api/[controller]")]
    public class AddressInfoController : ControllerBase
    {
        private IDataStoreService _dataStoreService;


        public AddressInfoController(IDataStoreService dataStoreService)
        {
            _dataStoreService = dataStoreService;
        }

        // GET api/<controller>/5
        [HttpPost("filter")]
        [IgnoreAntiforgeryToken]
        public IActionResult Filter([FromBody] AddressInfoFilter filter)
        {
            IEnumerable<Street> data =
                _dataStoreService.GetStreetsByString(filter.SearchString);
            return Ok(data);
        }

        [HttpGet("get/id")]
        [IgnoreAntiforgeryToken]
        public IActionResult GetById(int id)
        {
            var data = _dataStoreService.GetJsonForStreet(id);
            return Ok(data);
        }

    }
}
