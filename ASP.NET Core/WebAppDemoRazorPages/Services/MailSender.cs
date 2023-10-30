using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace WebAppDemoRazorPages.Services
{
    public class MailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public MailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var pass = "";// _configuration.GetSection("");
            var login = "login";
            var smtpClient = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(login, pass)
            };
            using (var mail = new MailMessage(login,email,subject,htmlMessage))
            {
                mail.IsBodyHtml = true;
                await smtpClient.SendMailAsync(mail);
            }


        }
    }
}
