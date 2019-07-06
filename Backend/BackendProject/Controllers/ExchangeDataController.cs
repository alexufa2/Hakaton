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


		public ExchangeDataController(IConfiguration configuration, IDataGovRuService dataGovRuService)
		{
			_configuration = configuration;
			_dataGovRuService = dataGovRuService;
		}


		[HttpGet("importdatagovrudata")]
		public IActionResult ImportDataGovRuData()
		{

			var result = _dataGovRuService.GetEntries();

			return Ok(result);
		}
	}
}
