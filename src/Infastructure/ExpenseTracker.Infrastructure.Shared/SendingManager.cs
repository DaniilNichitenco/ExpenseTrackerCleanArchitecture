using ExpenseTracker.Core.Application.Interfaces;
using ExpenseTracker.Core.Domain.Mails;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.Repository.Shared
{
    public class SendingManager : ISendingManager
    {
        private readonly ILogger<SendingManager> _logger;
        private readonly IConfiguration _configuration;
        private readonly IEmailSettings _emailSettings;

        public SendingManager(ILogger<SendingManager> logger, IConfiguration configuration, IEmailSettings emailSettings)
        {
            _logger = logger;
            _configuration = configuration;
            _emailSettings = emailSettings;
        }
        public bool SendMessage(string subject, string body, List<MailAddress> addresses)
        {
            try
            {
                var mail = new MailMessage();
                var server = new SmtpClient(_emailSettings.Server);
                mail.From = new MailAddress(_configuration["Emails:Server:Username"]);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = body;
                addresses.ForEach(address => mail.To.Add(address));

                    //if (!string.IsNullOrEmpty(emailModel.FileName) && emailModel.AttachmentContent != null)
                    //    AttachFile(emailModel, mail);

                    //if (linkToZivver && PhoneNumberValidator.IsValid(receiver.PhoneNumber))
                    //    mail.Headers.Add(new NameValueCollection
                    //        {{ZivverAccessRightHeader, $"{receiver.Email} {AccessRightType} {receiver.PhoneNumber}"}});

                server.Port = int.Parse(_emailSettings.Port);

                server.UseDefaultCredentials = false;
                server.Credentials = new NetworkCredential(_configuration["Emails:Server:Username"], _configuration["Emails:Server:Password"]);
                server.EnableSsl = true;

                server.DeliveryMethod = SmtpDeliveryMethod.Network;
                server.SendCompleted += (sender, args) =>
                {
                    mail.Attachments.Clear();
                    mail.Dispose();
                    server.Dispose();
                };

                var backupSecurityProtocol = ServicePointManager.SecurityProtocol;
                try
                {
                    server.SendAsync(mail, null);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
