using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using OpenCredentialPublisher.Data.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class EmailService : IEmailSender
    {
        private readonly MailSettings _mailSettings;
        private readonly HostSettings _hostSettings;
        public EmailService(IConfiguration configuration)
        {
            _mailSettings = configuration.GetSection(nameof(MailSettings)).Get<MailSettings>();
            _hostSettings = configuration.GetSection(nameof(HostSettings)).Get<HostSettings>();
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var assembly = typeof(EmailService).Assembly;
            var emailTemplateResourceStream = assembly.GetManifestResourceStream("OpenCredentialPublisher.Services.Resources.Templates.email.html");

            if (emailTemplateResourceStream == null) return;

            string messageTemplate;

            using (TextReader reader = new StreamReader(emailTemplateResourceStream))
            {
                messageTemplate = reader.ReadToEnd();
            }

            var messageHtml = String.Format(messageTemplate, _hostSettings.DnsName, subject, htmlMessage);

            var message = new MailMessage
            {
                Subject = subject,
                IsBodyHtml = true
            };

            var htmlView = AlternateView.CreateAlternateViewFromString(messageHtml, null, "text/html");

            using (var resource = assembly.GetManifestResourceStream("OpenCredentialPublisher.Services.Resources.Images.ocp-logo.png"))
            {
                if (resource != null)
                {
                    var logo = new LinkedResource(resource, new ContentType("img/png")) { ContentId = "logoId" };
                    htmlView.LinkedResources.Add(logo);
                }

                message.AlternateViews.Add(htmlView);
                message.To.Add(new MailAddress(email));

                if (_mailSettings.RedirectToInternal)
                {
                    message.To.Clear();
                    message.To.Add(_mailSettings.RedirectAddress);
                }

                message.From = new MailAddress(_mailSettings.From);
                message.Sender = new MailAddress(_mailSettings.From);

                using var client = new SmtpClient(_mailSettings.Server, _mailSettings.Port) { EnableSsl = _mailSettings.UseSSL, Credentials = new NetworkCredential(_mailSettings.User, _mailSettings.Password) };

                await client.SendMailAsync(message);
            }
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage, bool hideClosing)
        {
            var assembly = typeof(EmailService).Assembly;
            var emailTemplateResourceStream = hideClosing ? assembly.GetManifestResourceStream("OpenCredentialPublisher.Services.Resources.Templates.email-no-closing.html") : assembly.GetManifestResourceStream("OpenCredentialPublisher.Services.Resources.Templates.email.html");

            if (emailTemplateResourceStream == null) return;

            string messageTemplate;

            using (TextReader reader = new StreamReader(emailTemplateResourceStream))
            {
                messageTemplate = reader.ReadToEnd();
            }


            var messageHtml = String.Format(messageTemplate, _hostSettings.DnsName, subject, htmlMessage);

            var message = new MailMessage
            {
                Subject = subject,
                IsBodyHtml = true
            };

            var htmlView = AlternateView.CreateAlternateViewFromString(messageHtml, null, "text/html");

            using (var resource = assembly.GetManifestResourceStream("OpenCredentialPublisher.Services.Resources.Images.ocp-logo.png"))
            {
                if (resource != null)
                {
                    var logo = new LinkedResource(resource, new ContentType("img/png")) { ContentId = "logoId" };
                    htmlView.LinkedResources.Add(logo);
                }

                message.AlternateViews.Add(htmlView);
                message.To.Add(new MailAddress(email));

                if (_mailSettings.RedirectToInternal)
                {
                    message.To.Clear();
                    message.To.Add(_mailSettings.RedirectAddress);
                }

                message.From = new MailAddress(_mailSettings.From);
                message.Sender = new MailAddress(_mailSettings.From);

                using var client = new SmtpClient(_mailSettings.Server, _mailSettings.Port) { EnableSsl = _mailSettings.UseSSL, Credentials = new NetworkCredential(_mailSettings.User, _mailSettings.Password) };

                await client.SendMailAsync(message);
            }
        }
    }
}
