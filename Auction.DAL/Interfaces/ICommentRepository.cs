using Auction.DAL.Entities;
using Auction.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.DAL.Interfaces
{
    /// <summary>
    /// ICommentRepository
    /// </summary>
    public interface ICommentRepository
    {
        /// <summary>
        /// Add comment Entity
        /// </summary>
        Task AddCommnetAsync(Comment addComment);
        /// <summary>
        /// Get comments by lot id
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns></returns>
        Task<List<Comment>> GetCommentsByLotIdAsync(int lotId);
        /// <summary>
        /// Delete comment by comment id
        /// </summary>
        /// <param name="commentId"></param>
        void DeleteCommentById(Guid commentId);
        /// <summary>
        /// Delete comments in range
        /// </summary>
        /// <param name="comments"></param>
        void DeleteCommentsRange(List<Comment> comments);
    }
}
