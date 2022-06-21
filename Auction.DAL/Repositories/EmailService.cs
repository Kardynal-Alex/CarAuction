using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using HtmlWorkflow.Constants;
using HtmlWorkflow.Extensions;
using HtmlWorkflow.Models;
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
            StringBuilder sb = new StringBuilder();
            sb.Append(HtmlConstants.OpenHTML)
              .Append(HtmlConstants.OpenHead)
              .Append(HtmlConstants.CloseHead)
              .Append(HtmlConstants.OpenBody);

            sb.AddTextBlock(
                new HtmlHelper { Text = $"From {askOwner.UserEmail}" },
                new HtmlDivHelper { Style = "font-weight:600;font-size:30px;" });
            sb.AddTextBlock(
                new HtmlHelper { Text = $"{askOwner.Text}" },
                new HtmlDivHelper { Style = "font-size:25px;" });

            sb.Append(HtmlConstants.CloseBody)
              .Append(HtmlConstants.CloseHTML);
            return sb.ToString();
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
