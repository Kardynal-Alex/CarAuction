using Auction.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.BLL.Interfaces
{
    public interface ICommentService
    {
        Task AddCommentAsync(CommentDTO addComment);
        Task<List<CommentDTO>> GetCommentsByLotIdAsync(int lotId);
        Task DeleteCommentByIdAsync(Guid commentId);
    }
}
