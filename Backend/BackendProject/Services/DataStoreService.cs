using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TemplateApp.DAL;
using TemplateApp.DAL.Entities;

namespace TemplateApp.Services
{
	public interface IDataStoreService
	{

		List<Street> GetStreets();
		void SaveStreet(Street street, StreetJsonInfo json);
		List<string> GetJsonForStreet(int streetId);

	}

	public class DataStoreService : IDataStoreService
	{
		ApplicationContext _applicationContext;

		public DataStoreService(ApplicationContext applicationContext)
		{
			_applicationContext = applicationContext;
		}

		public List<Street> GetStreets()
		{
			return _applicationContext.Streets.ToList();
		}

		public List<string> GetJsonForStreet(int streetId)
		{
			return _applicationContext.StreetJsonInfos.Where(x => x.Street.Id == streetId).Select(x => x.Json).ToList();
		}

		public void SaveStreet(Street street, StreetJsonInfo json)
		{
			var streetEntry = _applicationContext.Streets.FirstOrDefault(x => x.StreetName.ToLower().Trim() == street.StreetName.ToLower().Trim());

			if (streetEntry == null)
			{
				streetEntry = _applicationContext.Add(new Street() { StreetName = street.StreetName }).Entity;
			}

			_applicationContext.StreetJsonInfos.Add(new StreetJsonInfo() { Street = streetEntry, Json = json.Json });

			_applicationContext.SaveChanges();
		}

	}
}
