using TemplateApp.DAL.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace TemplateApp
{
	public abstract class NotificationBase
	{
		private Dictionary<string, string> parameters = new Dictionary<string, string>();

		private string _subjectTemplate { get; set; }
		private string _bodyTemplate { get; set; }

		public NotificationBase(IConfiguration configuration)
		{
			parameters["%maindomain%"]= configuration.GetSection("Default").GetValue<string>("defaultDomain");
		}

		// TODO Add notificationType

		public Notification GetNotificationEntity()
		{
			return new Notification()
			{
				Body = GetBody(),
				Subject = GetTitle()
			};
		}

		private string GetTitle()
		{
			string template = GetSubjectTemplate();

			parameters.ToList().ForEach(p =>
			{
				template = template.Replace(p.Key, p.Value);
			});

			return template;
		}

		protected abstract string GetTemplateBody();

		protected abstract string GetSubjectTemplate();


		protected void SetParameter(string key, string value)
		{
			parameters[key] = value;
		}

		protected string GetParameter(string key)
		{
			if (parameters.ContainsKey(key))
			{
				return parameters[key];
			}
			else
			{
				return string.Empty;
			}
		}

		private string GetBody()
		{
			string template = GetTemplateBody();

			parameters.ToList().ForEach(p =>
			{
				template = template.Replace(p.Key, p.Value);
			});

			return template;

		}
	}

}
