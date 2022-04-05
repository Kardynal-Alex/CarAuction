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
    /// <summary>
    /// CommentRepository
    /// </summary>
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationContext context;
        /// <summary>
        /// Repository ctor
        /// </summary>
        public CommentRepository(ApplicationContext ctx)
        {
            context = ctx;
        }
        /// <summary>
        /// Add comment Entity
        /// </summary>
        /// <param name="addComment"></param>
        /// <returns></returns>
        public async Task AddCommnetAsync(Comment addComment)
        {
            await context.Comments.AddAsync(addComment);
        }
        /// <summary>
        /// Delete comment by comment id
        /// </summary>
        /// <param name="commentId"></param>
        public void DeleteCommentById(Guid commentId)
        {
            context.Comments.Remove(new Comment { Id = commentId });
        }
        /// <summary>
        /// Delete comments in range
        /// </summary>
        /// <param name="comments"></param>
        public void DeleteCommentsRange(List<Comment> comments)
        {
            context.Comments.RemoveRange(comments);
        }
        /// <summary>
        /// Get comments by lot id
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns></returns>
        public async Task<List<Comment>> GetCommentsByLotIdAsync(int lotId)
        {
            return await context.Comments.AsNoTracking().Where(x => x.LotId == lotId).ToListAsync();
        }
    }
}
