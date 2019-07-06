using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TemplateApp.DAL.Entities;

namespace TemplateApp.Models.Requests
{
	public class DataGovRuDataSetEntry
	{
		public string title { get; set; }
		public string organization { get; set; }
		public string organization_name { get; set; }
		public string topic { get; set; }
		public string identifier { get; set; }

		internal DataGovRuEntry ToDto()
		{
			return new DataGovRuEntry()
			{
				identifier = identifier,
				organization = organization,
				organization_name = organization_name,
				title = title,
				topic = topic
			};
		}
	}
}
