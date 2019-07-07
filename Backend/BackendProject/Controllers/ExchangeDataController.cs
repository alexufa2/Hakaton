using FileParsing;
using ImportDataContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
        private IDataStoreService _dataService;



        public ExchangeDataController(IConfiguration configuration, IDataGovRuService dataGovRuService,
            IFileService fileService, ITextFileParser parser, IDataStoreService dataService)
        {
            _configuration = configuration;
            _dataGovRuService = dataGovRuService;
            _fileService = fileService;
            _dataService = dataService;
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

        [HttpGet("load")]
        [IgnoreAntiforgeryToken]
        public IActionResult Load()
        {
            IEnumerable<AddressInfo> result =
                _parser.ParseFile(@"c:\Hackaton\Дата.Гов\data-20190703T0648-structure-20190703T0648.csv",
                                  "Сведения о местах нахождения многоквартирных жилых домов, в которых осуществлен капитальный ремонт");

            _dataService.SaveData(result);

            return Ok(result);
        }

        [HttpGet("loadfromdb")]
        [IgnoreAntiforgeryToken]
        public IActionResult LoadfromDb()
        {
            return Ok();
        }
    }
}
