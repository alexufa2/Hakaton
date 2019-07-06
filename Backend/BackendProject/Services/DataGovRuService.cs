using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TemplateApp.DAL;
using TemplateApp.Models.Requests;

namespace TemplateApp.Services
{

	public interface IDataGovRuService
	{
		Task<List<DataGovRuDataSetEntry>> GetEntries();
		Task<DateTime> GetLastVersionForEntry(string identifier);
		Task<List<object>> GetRows(string identifier, DateTime date);
		int SaveEntry(DataGovRuDataSetEntry entry);
		void SaveEntryRow(int entryId, string rowJson);
	}

	public class DataGovRuService : IDataGovRuService
	{

		private IConfiguration _configuration;
		ApplicationContext _appContext;

		public DataGovRuService(IConfiguration configuration, ApplicationContext appContext)
		{
			_configuration = configuration;
			_appContext = appContext;
		}

		public async Task<List<DataGovRuDataSetEntry>> GetEntries()
		{

			string key = _configuration.GetSection("Default").GetValue<string>("datagovru_accesstoken");
			List<DataGovRuDataSetEntry> result = new List<DataGovRuDataSetEntry>();


			using (var httpClient = new HttpClient())
			{
				var response = await httpClient.GetStringAsync("https://data.gov.ru/api/json/dataset?access_token=" + key + "&topic=Construction");

				result = JsonConvert.DeserializeObject<List<DataGovRuDataSetEntry>>(response);
			}

			return result;
		}

		public async Task<DateTime> GetLastVersionForEntry(string identifier)
		{
			string key = _configuration.GetSection("Default").GetValue<string>("datagovru_accesstoken");
			List<DataSetGovRuEntryVersion> result = new List<DataSetGovRuEntryVersion>();

			DateTime lastDate = DateTime.Now;

			using (var httpClient = new HttpClient())
			{
				var response = await httpClient.GetStringAsync($"https://data.gov.ru/api/json/dataset/{identifier}/version?access_token=" + key + "&topic=Construction");

				result = JsonConvert.DeserializeObject<List<DataSetGovRuEntryVersion>>(response, new IsoDateTimeConverter() { DateTimeFormat = "yyyyMMddTHHmmss" });

				lastDate = result.Where(x => x.created.HasValue).OrderByDescending(x => x.created.Value).FirstOrDefault().created.Value;
			}

			return lastDate;

		}

		public async Task<List<object>> GetRows(string identifier, DateTime date)
		{
			string key = _configuration.GetSection("Default").GetValue<string>("datagovru_accesstoken");
			List<object> result = new List<object>();

			string dateJson = CreateSpecialFormatDate(date);

			using (var httpClient = new HttpClient())
			{
				var response = await httpClient.GetStringAsync($"https://data.gov.ru/api/json/dataset/{identifier}/version/{CreateSpecialFormatDate(date)}/content?access_token=" + key);

				result = JsonConvert.DeserializeObject<List<object>>(response);

			}

			return result;
		}

		private string CreateSpecialFormatDate(DateTime date)
		{
			return date.ToString("yyyyMMddTHHmmss");
		}

		public int SaveEntry(DataGovRuDataSetEntry entry)
		{
			var result = _appContext.DataGovRuEntries.Add(entry.ToDto());

			_appContext.SaveChanges();

			return result.Entity.Id;
		}

		public void SaveEntryRow(int entryId, string rowJson)
		{

			var entry = _appContext.DataGovRuEntries.FirstOrDefault(x => x.Id == entryId);

			_appContext.DataGovRuEntryRows.Add(new DAL.Entities.DataGovRuEntryRow() { Entry = entry, Row = rowJson });

			_appContext.SaveChanges();
		}
	}
}
