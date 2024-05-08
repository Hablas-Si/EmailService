using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
namespace EmailService.Services
{
    public class EmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public void SendEmail(string to, string subject, string body)
        {
            try
            {
                using (var smtpClient = new SmtpClient(_emailSettings.MailServer, _emailSettings.MailPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_emailSettings.Sender, _emailSettings.Password);
                    smtpClient.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_emailSettings.Sender),
                        To = { to },
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };

                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                // Handle exception (log or modify return accordingly)
                throw new InvalidOperationException("Failed to send email.", ex);
            }
        }

    }
}
