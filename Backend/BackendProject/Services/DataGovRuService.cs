using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TemplateApp.Models.Requests;

namespace TemplateApp.Services
{

	public interface IDataGovRuService
	{
		Task<List<DataGovRuDataSetEntry>> GetEntries();
	}

	public class DataGovRuService : IDataGovRuService
	{

		private IConfiguration _configuration;

		public DataGovRuService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<List<DataGovRuDataSetEntry>> GetEntries()
		{

			string key = _configuration.GetSection("Default").GetValue<string>("datagovru_accesstoken");
			List<DataGovRuDataSetEntry> result = new List<DataGovRuDataSetEntry>();


			using (var httpClient = new HttpClient())
			{
				var response =  await httpClient.GetStringAsync("https://data.gov.ru/api/json/dataset?key=" + key);
				
				result = JsonConvert.DeserializeObject<List<DataGovRuDataSetEntry>>(response);
			}

			return result;
		}
	}
}
