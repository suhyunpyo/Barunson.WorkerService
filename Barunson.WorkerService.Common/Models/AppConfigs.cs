using SendGrid.Helpers.Mail;

namespace Barunson.WorkerService.Common.Models
{
    public class MailServerOption
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string MailId { get; set; }
        public string Password { get; set; }
        public EmailAddress FromAddress { get; set; }
        public List<EmailAddress> ToAddress { get; set; }
        public bool Active { get; set; }
    }


    public class TestDataOption
    {
        public string LMSDestInfo { get; set; }
        public string RecipientNum { get; set; }
    }

    public class PgMertInfo
    {
        public string Id { get; set; }
        public string MertKey { get; set; }
        public string ClientKey { get; set; }
        public string SecretKey { get; set; }
    }

    public class SMBUser
    {
        public string UserID { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
