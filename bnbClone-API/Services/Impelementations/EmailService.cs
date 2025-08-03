using bnbClone_API.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace bnbClone_API.Services.Impelementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using (var client = new SmtpClient(_config["EmailSettings:SmtpServer"], int.Parse(_config["EmailSettings:Port"])))
            {

                client.Host = _config["EmailSettings:SmtpServer"];
                client.Port = int.Parse(_config["EmailSettings:Port"]);
                client.Credentials = new NetworkCredential(
                    _config["EmailSettings:Username"],
                    _config["EmailSettings:Password"]);
                client.EnableSsl = true; // تأكيد استخدام SSL
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_config["EmailSettings:SenderEmail"], _config["EmailSettings:SenderName"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);

                await client.SendMailAsync(mailMessage);
            }
        }

    }
}
