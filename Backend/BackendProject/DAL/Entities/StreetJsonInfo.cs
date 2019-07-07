using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateApp.DAL.Entities
{
	public class StreetJsonInfo
	{
		public int Id { get; set; }

		public Street Street { get; set; }

		public string Json { get; set; }

	}
}
