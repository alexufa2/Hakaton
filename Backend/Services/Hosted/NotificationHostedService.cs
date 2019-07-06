using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TemplateApp.Services.Hosted
{
	internal class NotificationHostedService : IHostedService, IDisposable
	{
		private readonly ILogger _logger;
		//private readonly ITaskService _taskService;
		//private readonly INotificationService _notificationService;
		private readonly IServiceScopeFactory _serviceScopeFactory;
		private Timer _timer;

		public NotificationHostedService(ILogger<NotificationHostedService> logger,
			 IServiceScopeFactory serviceScopeFactory
			)
		{
			_logger = logger;
			_serviceScopeFactory = serviceScopeFactory;
			//_taskService = new TaskService(;
			//_notificationService = notificationService;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Notification Background Service is starting.");

			_timer = new Timer(DoWork, null, TimeSpan.Zero,
				TimeSpan.FromMinutes(1));

			return Task.CompletedTask;
		}

		private void DoWork(object state)
		{
			_logger.LogInformation("Starting notification proccessing.");

			using (var scope = _serviceScopeFactory.CreateScope())
			{
				
				_logger.LogInformation("Starting sending emails.");

				scope.ServiceProvider.
					GetService<INotificationService>().ProccessEmailNotifications();

				_logger.LogInformation("Completed sending emails.");

			}
			//_notificationService.CheckTaskReminderNotifications();

			_logger.LogInformation("Notification proccessing complete.");
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Notification Background Service is starting.");

			_timer?.Change(Timeout.Infinite, 0);

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}
	}
}
