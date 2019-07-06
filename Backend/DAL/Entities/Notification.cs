using TemplateApp.DAL.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateApp.DAL.Entities
{
	public class Notification
	{

		public int Id { get; set; }

		[Required]
		public string ReceiverEmail { get; set; }

		[Required]
		public string Subject { get; set; }

		[Required]
		public string Body { get; set; }

		public bool IsHtml { get; set; }

		public DateTime SendingDate { get; set; }

		public EmailStateEnum State { get; set; }

		public int SendAttemptsLimit { get; set; }
	}
}
