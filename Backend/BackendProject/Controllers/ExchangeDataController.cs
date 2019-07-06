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


		public ExchangeDataController(IConfiguration configuration, IDataGovRuService dataGovRuService)
		{
			_configuration = configuration;
			_dataGovRuService = dataGovRuService;
		}


		[HttpGet("importdatagovrudata")]
		[IgnoreAntiforgeryToken]
		public async Task<IActionResult> ImportDataGovRuData(int number)
		{

			var result = await _dataGovRuService.GetEntries();

			string id = result[number].identifier;

			var data = await _dataGovRuService.GetLastVersionForEntry(id);

			var res2 = await _dataGovRuService.GetRows(id, data);

			return Ok(res2);
		}

        [HttpGet("test")]
        public IActionResult testFile()
        {
            var result = _parser.ParseFile(@"c:\Hackaton\Дата.Гов\data-20190703T0648-structure-20190703T0648.csv");

            return Ok("");
	}
}
}
