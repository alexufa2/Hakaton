using TemplateApp.DAL;
using TemplateApp.DAL.Dto;
using TemplateApp.DAL.Extensions;
using TemplateApp.Utils.Notifications;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace TemplateApp.Services
{

    public interface INotificationService
    {
        void ProccessEmailNotifications();
        void AddUserRegistrationNotification(DAL.Users.User user);
    }


    public class NotificationService : INotificationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApplicationContext _context;
        private ILogger _logger;
        private IConfiguration _config;

        public NotificationService(ApplicationContext appContext,
            IHttpContextAccessor httpContextAccessor,
            ILogger<NotificationService> logger, IConfiguration config
            )
        {
            _context = appContext;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _config = config;
        }


        public void ProccessEmailNotifications()
        {
            var notififcationsNotSent = _context.Notifications.Where(x => x.State == DAL.enums.EmailStateEnum.NotSent &&
            x.SendingDate <= DateTime.UtcNow && x.SendAttemptsLimit <= 5).ToList();

            foreach (var notification in notififcationsNotSent)
            {
                using (var client = new SmtpClient())
                {
                    //client.Port = 587;
                    //client.Host = "smtp.yandex.ru";
                    //client.EnableSsl = true;
                    //client.Credentials = new NetworkCredential("evgrafovbbc509", "");
                    //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //client.Timeout = 30000;
                    var configSection = _config.GetSection("SmtpConfig");

                    client.Port = configSection.GetValue<int>("smtpPort", 0);
                    client.Host = configSection["smtpHost"];
                    client.EnableSsl = configSection.GetValue<bool>("ssl");
                    client.Credentials = new NetworkCredential(configSection["login"],
                        configSection["password"]);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Timeout = 30000;



                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(configSection["from"]); // Адрес отправителя
                    mail.To.Add(new MailAddress(notification.ReceiverEmail)); // Адрес получателя
                    mail.Subject = notification.Subject;
                    mail.Body = notification.Body;
                    mail.IsBodyHtml = true;
                    //mail.IsBodyHtml = notification.IsHtml;

                    try
                    {
                        client.Send(mail);
                        //client.Send("evgrafovbbc509@yandex.ru", notification.ReceiverEmail, notification.Subject, notification.Body);
                        notification.State = DAL.enums.EmailStateEnum.Sent;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Fail to send notification to {notification.ReceiverEmail} exception: {ex.Message}");
                        notification.SendAttemptsLimit++;
                    }

                }

                _context.SaveChanges();
            }

        }

        public void AddUserRegistrationNotification(DAL.Users.User user)
        {

            var notification = new UserRegistrationNotification(_config);
            notification.ActivationKey = user.ActivationKey;
            notification.UserName = user.Name;

            var entity = notification.GetNotificationEntity();
            entity.ReceiverEmail = user.Email;
            entity.State = DAL.enums.EmailStateEnum.NotSent;

            _context.Notifications.Add(entity);
            _context.SaveChanges();

        }

    }
}
