﻿using Auction.BLL.DTO;
using Auction.BLL.Interfaces;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auction.BLL.Validation;
using System;

namespace Auction.BLL.Services
{
    /// <summary>
    /// Comment Service class
    /// </summary>
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        /// <summary>
        /// Add comment
        /// </summary>
        /// <param name="addComment"></param>
        /// <returns></returns>
        public async Task AddCommentAsync(CommentDTO addComment)
        {
            ValidateLotDTO(addComment);

            addComment.DateTime = DateTime.Now;
            await unitOfWork.CommentRepository.AddCommnetAsync(mapper.Map<CommentDTO, Comment>(addComment));
            await unitOfWork.SaveAsync();
        }
        /// <summary>
        /// Delete comment by comment id
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public async Task DeleteCommentByIdAsync(Guid commentId)
        {
            if (commentId == null)
                throw new AuctionException("Comment id is null");

            unitOfWork.CommentRepository.DeleteCommentById(commentId);
            await unitOfWork.SaveAsync();
        }
        /// <summary>
        /// Get comments by lotId
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns></returns>
        public async Task<List<CommentDTO>> GetCommentsByLotIdAsync(int lotId)
        {
            if (!int.TryParse(lotId.ToString(), out _) || lotId < 0) 
                throw new AuctionException("Invalid lot id");

            var comments = await unitOfWork.CommentRepository.GetCommentsByLotIdAsync(lotId);
            return mapper.Map<List<CommentDTO>>(comments);
        }
        /// <summary>
        /// Validate commentDTO model
        /// </summary>
        /// <param name="commentDTO"></param>
        private void ValidateLotDTO(CommentDTO commentDTO)
        {
            if (string.IsNullOrEmpty(commentDTO.Author) || string.IsNullOrEmpty(commentDTO.Text) ||
                string.IsNullOrEmpty(commentDTO.UserId))
                throw new AuctionException("incorect text data");

            if (!int.TryParse(commentDTO.LotId.ToString(), out int lotid) || lotid < 0) 
                throw new AuctionException("Invalid lot id");
        }
    }
}
