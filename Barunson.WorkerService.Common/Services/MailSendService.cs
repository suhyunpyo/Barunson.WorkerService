using Barunson.WorkerService.Common.Models;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Barunson.WorkerService.Common.Services
{
    public interface IMailSendService
    {
        Task SendAsync(string subject, string body);
        Task<bool> SendAsync(string subject, string body, List<EmailAddress> toAddress, List<EmailAddress> ccAddress = null, EmailAddress fromAddress = null);
    }
    public class MailSendService : IMailSendService
    {
        private readonly ILogger<MailSendService> _logger;
        private readonly MailServerOption _options;
        private readonly ISendGridClient _client;
        public MailSendService(MailServerOption option, ISendGridClient SendGridClient, ILogger<MailSendService> logger)
        {
            _options = option;
            _logger = logger;
            _client = SendGridClient;
        }

        public async Task SendAsync(string subject, string body)
        {
            if (!_options.Active)
                return;

            await SendAsync(subject, body, _options.ToAddress);

        }
        public async Task<bool> SendAsync(string subject, string body, List<EmailAddress> toAddress, List<EmailAddress> ccAddress = null, EmailAddress fromAddress = null)
        {

            if (!_options.Active)
                return true;

            if (fromAddress == null)
                fromAddress = new EmailAddress(_options.FromAddress.Email, _options.FromAddress.Name);

            var mailMessage = new SendGridMessage()
            {
                From = fromAddress,
                Subject = subject,
                HtmlContent = body,
            };
            toAddress.ForEach(m =>
            {
                mailMessage.AddTo(m);
            });
            if (ccAddress != null && ccAddress.Count > 0)
            {
                foreach (var cc in ccAddress)
                {
                    mailMessage.AddCc(cc);
                }
            }
            try
            {
                var response = await _client.SendEmailAsync(mailMessage);
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{time:hh:mm:ss} MailSendService has error.", DateTime.Now);
                return false;
            }
        }
    }
}
