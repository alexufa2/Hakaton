using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateApp.DAL.Entities
{
	public class DataGovRuEntry
	{
		public int Id { get; set; }
		public string title { get; set; }
		public string organization { get; set; }
		public string organization_name { get; set; }
		public string topic { get; set; }
		public string identifier { get; set; }
	}
}
