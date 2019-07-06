using FileParsing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TemplateApp.Services;

namespace TemplateApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeDataController : ControllerBase
    {

        private IConfiguration _configuration;
        private IDataGovRuService _dataGovRuService;

        private IFileService _fileService;
        private ITextFileParser _parser;


        public ExchangeDataController(IConfiguration configuration, IDataGovRuService dataGovRuService,
            IFileService fileService, ITextFileParser parser)
        {
            _configuration = configuration;
            _dataGovRuService = dataGovRuService;
            _fileService = fileService;
            _parser = parser;

        }


        [HttpGet("importdatagovrudata")]
        public IActionResult ImportDataGovRuData()
        {

            var result = _dataGovRuService.GetEntries();

            return Ok(result);
        }

        [IgnoreAntiforgeryToken]
        [HttpGet("test")]
        public IActionResult testFile()
        {
            var result = _parser.ParseFile(@"c:\Hackaton\Дата.Гов\data-20190703T0648-structure-20190703T0648.csv");

            return Ok("");
        }
    }
}
