using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class EmailService : IEmailSender
    {
        private readonly MailSettings _mailSettings;
        private readonly HostSettings _hostSettings;
        private readonly ILogger<EmailService> _logger;
        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _mailSettings = configuration.GetSection(nameof(MailSettings)).Get<MailSettings>();
            _hostSettings = configuration.GetSection(nameof(HostSettings)).Get<HostSettings>();
            _logger = logger;
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

                message.From = new MailAddress(_mailSettings.From, _mailSettings.From);

                using (var client = new SmtpClient(_mailSettings.Server, _mailSettings.Port))
                {
                    client.Credentials = new NetworkCredential(_mailSettings.User, _mailSettings.Password);
                    client.EnableSsl = _mailSettings.UseSSL;

                    try
                    {
                        await client.SendMailAsync(message);
                    }
                    catch (Exception ex)
                    {
                        var builder = new StringBuilder();

                        builder.AppendLine(ex.Message);
                        builder.AppendLine($"Server: {_mailSettings.Server}");
                        builder.AppendLine($"Port: {_mailSettings.Port}");
                        builder.AppendLine($"User: {_mailSettings.User}");
                        builder.AppendLine($"From: {_mailSettings.From}");
                        builder.AppendLine($"Enable SSL: {_mailSettings.UseSSL}");
                        builder.AppendLine($"To: {email}");
                        _logger.LogError(ex, builder.ToString());
                    }
                }
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

                message.From = new MailAddress(_mailSettings.From, _mailSettings.From);

                using (var client = new SmtpClient(_mailSettings.Server, _mailSettings.Port))
                {
                    client.Credentials = new NetworkCredential(_mailSettings.User, _mailSettings.Password);
                    client.EnableSsl = _mailSettings.UseSSL;

                    try
                    {
                        await client.SendMailAsync(message);
                    }
                    catch (Exception ex)
                    {
                        var builder = new StringBuilder();

                        builder.AppendLine(ex.Message);
                        builder.AppendLine($"Server: {_mailSettings.Server}");
                        builder.AppendLine($"Port: {_mailSettings.Port}");
                        builder.AppendLine($"User: {_mailSettings.User}");
                        builder.AppendLine($"From: {_mailSettings.From}");
                        builder.AppendLine($"Enable SSL: {_mailSettings.UseSSL}");
                        builder.AppendLine($"To: {email}");
                        _logger.LogError(ex, builder.ToString());
                    }
                }
            }
        }
    }
}
