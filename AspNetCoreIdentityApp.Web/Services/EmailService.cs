
using AspNetCoreIdentityApp.Web.Models.OptionsModel;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AspNetCoreIdentityApp.Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _options;

        public EmailService(IOptions<EmailSettings> options)
        {
            _options = options.Value;
        }

        public async Task SendResetPasswordEmail(string resetPasswordEmailLink, string toEmail)
        {
            var smtpClient = new SmtpClient();
            smtpClient.Host = _options.Host;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(_options.Email, _options.Password);
            smtpClient.EnableSsl = true;
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_options.Email);
            mailMessage.To.Add(toEmail);
            mailMessage.Subject = "Localhost | Şifre sıfırlama linki";
            mailMessage.Body = @$"<h4>Şifrenizi yenilemek için aşağıdaki linke tıklayınız
            </h4>
            </br>
            <p><a href='{resetPasswordEmailLink}'>Şifre yenileme link<a/></p>";
            mailMessage.IsBodyHtml = true;
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
