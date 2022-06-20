using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Auction.DAL.Repositories
{
    /// <summary>
    /// EmailService
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration EmailConfiguration;
        public EmailService(EmailConfiguration EmailConfiguration)
        {
            this.EmailConfiguration = EmailConfiguration;
        }
        /// <summary>
        /// message fro owner in email
        /// </summary>
        /// <param name="askOwner"></param>
        /// <returns></returns>
        public string AskOwnerMessage(AskOwner askOwner)
        {
            StringBuilder text = new StringBuilder();
            text.Append(@"<html>
                            <head>
                            </head>
                            <body>");

            text.Append("<div style='font-weight:600;font-size:30px;'")
                .Append($"<h5>From {askOwner.UserEmail} </h5>")
                .Append("</div>")
                .Append("<div style='font-size:25px;'>")
                .Append($"<h5> {askOwner.Text} </h5>")
                .Append("</div>");

            text.Append(@"  </body>
                          </html>");
            return text.ToString();
        }
        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="emailMessage"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(EmailMessage emailMessage)
        {
            var bodyBuilder = emailMessage.Content;
            MailMessage mail = new MailMessage(EmailConfiguration.From, emailMessage.To)
            {
                Subject = emailMessage.Subject,
                Body = bodyBuilder,
                IsBodyHtml = true,
                Priority = MailPriority.High
            };

            if (emailMessage.PDFFile != null && emailMessage.PDFFile.Length > 0) 
            {
                var memoryStream = new MemoryStream(emailMessage.PDFFile);
                mail.Attachments.Add(new Attachment(memoryStream, "Info.pdf", MediaTypeNames.Application.Pdf));
            }

            NetworkCredential credentials = new NetworkCredential(EmailConfiguration.From, EmailConfiguration.Password);
            SmtpClient smtp = new SmtpClient()
            {
                Host = EmailConfiguration.SmtpServer,
                Port = EmailConfiguration.Port,
                EnableSsl = true,
                //UseDefaultCredentials = true,
                Credentials = credentials
            };

            await smtp.SendMailAsync(mail);
        }
    }
}
