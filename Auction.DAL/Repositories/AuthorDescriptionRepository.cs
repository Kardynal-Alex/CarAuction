using Auction.DAL.EF;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Auction.DAL.Repositories
{
    /// <summary>
    /// AuthorDescriptionRepository
    /// </summary>
    public class AuthorDescriptionRepository : IAuthorDescriptionRepository
    {
        private readonly ApplicationContext context;
        /// <summary>
        /// Repository ctor
        /// </summary>
        public AuthorDescriptionRepository(ApplicationContext ctx)
        {
            context = ctx;
        }
        /// <summary>
        /// Add AuthorDescription
        /// </summary>
        /// <param name="authorDescription"></param>
        public async Task AddAuthorDescriptionAsync(AuthorDescription authorDescription)
        {
            await context.AuthorDescriptions.AddAsync(authorDescription);
        }
        /// <summary>
        /// Update AuthorDescription
        /// </summary>
        /// <param name="authorDescription"></param>
        public void UpdateAuthorDescription(AuthorDescription authorDescription)
        {
            context.AuthorDescriptions.Update(authorDescription);
        }
        /// <summary>
        /// Get Author Description By LotId
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns></returns>
        public async Task<AuthorDescription> GetAuthorDescriptionByLotIdAsync(int lotId)
        {
            return await context.AuthorDescriptions.FirstOrDefaultAsync(_ => _.LotId == lotId);
        }
    }
}
