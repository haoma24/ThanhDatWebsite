using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace ThanhDatWebsite.Controllers
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email,string subject, string message);
    }
}