using TemplateApp.Resources;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateApp.Utils.Notifications
{
	/// <summary>
	/// Подтверждение регистрации
	/// </summary>
	public class UserRegistrationNotification : NotificationBase
	{
		public UserRegistrationNotification(IConfiguration configuration) : base(configuration)
		{

		}

		protected override string GetTemplateBody()
		{
			return NotificationsTemplates.UserRegistrationBodyTemplate;
		}

		protected override string GetSubjectTemplate()
		{
			return NotificationsTemplates.UserRegistrationSubjectTemplate;
		}

		public string ActivationKey
		{
			get
			{
				return GetParameter("%activationkey%");
			}

			set
			{
				SetParameter("%activationkey%", value);
			}
		}

		public string UserName
		{
			get
			{
				return GetParameter("%username%");
			}

			set
			{
				SetParameter("%username%", value);
			}
		}
	}
}
