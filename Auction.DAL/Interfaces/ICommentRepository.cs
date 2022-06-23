using Auction.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.DAL.Interfaces
{
    public interface ICommentRepository
    {
        Task AddCommnetAsync(Comment addComment);
        Task<List<Comment>> GetCommentsByLotIdAsync(int lotId);
        void DeleteCommentById(Guid commentId);
        void DeleteCommentsRange(List<Comment> comments);
    }
}
