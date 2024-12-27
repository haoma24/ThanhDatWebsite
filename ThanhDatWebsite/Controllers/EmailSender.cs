using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace ThanhDatWebsite.Controllers
{
    public class EmailSender:IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "haomaxga@gmail.com";
            var pw = "vzbgffmxucviddun";
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(mail);
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;
            return client.SendMailAsync(mailMessage);
        }
    }
}