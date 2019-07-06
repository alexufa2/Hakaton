using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateApp.Models
{
	public class OperationResult
	{
		public bool Success { get; set; }
		public object Data { get; set; }
		public int Code { get; set; }
	}
}
