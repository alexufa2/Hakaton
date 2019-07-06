using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateApp.DAL.Dto
{
	public class UpdateTimeZoneRequestDto
	{
		public string TimeZoneName { get; set; }
		public int TimeZoneOffset { get; set; }
	}
}
