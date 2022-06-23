using Auction.DAL.EF;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Auction.DAL.Repositories
{
    public class AuthorDescriptionRepository : IAuthorDescriptionRepository
    {
        private readonly ApplicationContext context;
        public AuthorDescriptionRepository(ApplicationContext ctx)
        {
            context = ctx;
        }
        
        public async Task AddAuthorDescriptionAsync(AuthorDescription authorDescription)
        {
            await context.AuthorDescriptions.AddAsync(authorDescription);
        }
      
        public void UpdateAuthorDescription(AuthorDescription authorDescription)
        {
            context.AuthorDescriptions.Update(authorDescription);
        }
     
        public async Task<AuthorDescription> GetAuthorDescriptionByLotIdAsync(int lotId)
        {
            return await context.AuthorDescriptions.FirstOrDefaultAsync(_ => _.LotId == lotId);
        }
    }
}
