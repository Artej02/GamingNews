using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

using System.Threading.Tasks;
using System;
using GamingWeb.Custom.DatabaseHelpers;
using GamingWeb.Custom.Models.User;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using GamingNews.Custom.Helpers;

namespace GamingNews.Custom.Helpers
{
  
    

public class MailHelper
    {
       
        private IConfiguration Configuration;
        private HttpContext Context;
        public MailHelper(IConfiguration _Configuration)
        {
            Configuration = _Configuration;
        }

        public async Task<object> SendEmail(string name, string username, string msg)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(Configuration["EmailCredenciale:Email"]);
            email.To.Add(MailboxAddress.Parse("artej02@gmail.com"));
            email.Subject = "Lënda Laboratorike 2";
            email.Body = new TextPart(TextFormat.Html)
            {


                Text = $"<p>{name}, with email: {username} <br /> has sent you a message: <br /> {msg}</p>"
            };

            // send email
            using var smtp = new SmtpClient();
            try
            {
                smtp.CheckCertificateRevocation = false;
                smtp.Connect(Configuration["EmailCredenciale:Smtp"], Int32.Parse(Configuration["EmailCredenciale:Port"]), SecureSocketOptions.StartTls);
                smtp.Authenticate(Configuration["EmailCredenciale:Email"], Configuration["EmailCredenciale:Password"]);
                smtp.Send(email);
                smtp.Disconnect(true);

                return new { Sent = true };

            }
            catch (Exception e)
            {
                String message = e.Message;
                return new { Sent = false, Message = message };
            }

        }

    }
}
