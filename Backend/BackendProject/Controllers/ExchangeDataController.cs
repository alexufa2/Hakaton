using FileParsing;
using ImportDataContracts;
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
		[IgnoreAntiforgeryToken]
		public async Task<IActionResult> ImportDataGovRuData(int number)
		{

			var result = await _dataGovRuService.GetEntries();
			int commonCount = result.Count;
			int commonRows = 0;
			int failed = 0;
			int failedRows = 0;


			foreach (var entry in result)
			{
				try
				{
					string identifier = entry.identifier;

					int entryId = _dataGovRuService.SaveEntry(entry);

					var data = await _dataGovRuService.GetLastVersionForEntry(identifier);

					if (data != null)
					{
						var rows = await _dataGovRuService.GetRows(identifier, data);

						foreach (object row in rows)
						{
							commonRows++;

							try
							{
								_dataGovRuService.SaveEntryRow(entryId, row.ToString());
        }
							catch
							{
								failed++;
							}
						}
					}
				}
				catch
				{
					failedRows++;
				}
			}

			return Ok($"OK - count: {commonCount} failed: {failed}");
		}

		[HttpGet("test")]
 		[IgnoreAntiforgeryToken]
		public IActionResult testFile()
		{
            IEnumerable<AddressInfo> result =
                _parser.ParseFile(@"c:\Hackaton\Дата.Гов\data-20190703T0648-structure-20190703T0648.csv");

			return Ok(result);
		}
	}
}
