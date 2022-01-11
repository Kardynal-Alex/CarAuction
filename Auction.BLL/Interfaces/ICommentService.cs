using Auction.BLL.DTO;
using Auction.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auction.BLL.Interfaces
{
    /// <summary>
    /// ICommentService
    /// </summary>
    public interface ICommentService
    {
        /// <summary>
        /// Add comment
        /// </summary>
        /// <param name="addComment"></param>
        /// <returns></returns>
        Task AddCommentAsync(CommentDTO addComment);
        /// <summary>
        /// Get comments by lot id
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns></returns>
        Task<List<CommentDTO>> GetCommentsByLotIdAsync(int lotId);
        /// <summary>
        /// Delete comment by id
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        Task DeleteCommentByIdAsync(Guid commentId);
    }
}
