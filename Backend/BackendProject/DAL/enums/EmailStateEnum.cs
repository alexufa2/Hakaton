using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateApp.DAL.enums
{
	public enum EmailStateEnum
	{
		NotRequired = 0,
		WaitForSent = 1,
		Sent = 2,
		NotSent = 3
	}
}
