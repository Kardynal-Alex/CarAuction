using Auction.DAL.Entities;
using System;
using System.Threading.Tasks;

namespace Auction.DAL.Interfaces
{
    /// <summary>
    ///  ]IEmailService
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Send message
        /// </summary>
        /// <param name="emailMessage"></param>
        /// <returns></returns>
        Task SendEmailAsync(EmailMessage emailMessage);
        /// <summary>
        /// Ask owner text message for email
        /// </summary>
        /// <param name="askOwner"></param>
        /// <returns></returns>
        string AskOwnerMessage(AskOwner askOwner);
    }
}
