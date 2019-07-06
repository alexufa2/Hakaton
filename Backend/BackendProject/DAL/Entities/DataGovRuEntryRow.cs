using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateApp.DAL.Entities
{
	public class DataGovRuEntryRow
	{
		public int Id { get; set; }

		public DataGovRuEntry Entry { get; set; }

		public string Row { get; set; }
	}
}
