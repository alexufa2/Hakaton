using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateApp.DAL.Entities
{
	public class Street
	{
		public int Id { get; set; }
		public string StreetName { get; set; }
        public string FullAddress { get; set; }

    }
}
