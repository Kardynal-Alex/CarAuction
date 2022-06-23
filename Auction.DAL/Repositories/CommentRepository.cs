using Auction.DAL.EF;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace Auction.DAL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationContext context;
        public CommentRepository(ApplicationContext ctx)
        {
            context = ctx;
        }
      
        public async Task AddCommnetAsync(Comment addComment)
        {
            await context.Comments.AddAsync(addComment);
        }
        
        public void DeleteCommentById(Guid commentId)
        {
            context.Comments.Remove(new Comment { Id = commentId });
        }
       
        public void DeleteCommentsRange(List<Comment> comments)
        {
            context.Comments.RemoveRange(comments);
        }
      
        public async Task<List<Comment>> GetCommentsByLotIdAsync(int lotId)
        {
            return await context.Comments.AsNoTracking().Where(x => x.LotId == lotId).ToListAsync();
        }
    }
}
