using Auction.DAL.Entities;
using System.Threading.Tasks;

namespace Auction.DAL.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage emailMessage);
        string AskOwnerMessage(AskOwner askOwner);
    }
}
