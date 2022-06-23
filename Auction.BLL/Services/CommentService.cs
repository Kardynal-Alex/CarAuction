using Auction.BLL.DTO;
using Auction.BLL.Interfaces;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auction.BLL.Validation;
using System;

namespace Auction.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task AddCommentAsync(CommentDTO addComment)
        {
            ValidateLotDTO(addComment);

            addComment.DateTime = DateTime.Now;
            await unitOfWork.CommentRepository.AddCommnetAsync(mapper.Map<CommentDTO, Comment>(addComment));
            await unitOfWork.SaveAsync();
        }
      
        public async Task DeleteCommentByIdAsync(Guid commentId)
        {
            Precognitions.GuidIsNullOrEmpty(commentId, "Comment id is null");

            unitOfWork.CommentRepository.DeleteCommentById(commentId);
            await unitOfWork.SaveAsync();
        }
      
        public async Task<List<CommentDTO>> GetCommentsByLotIdAsync(int lotId)
        {
            Precognitions.IntIsNotNumberOrNegative(lotId, "Invalid lot id");

            var comments = await unitOfWork.CommentRepository.GetCommentsByLotIdAsync(lotId);
            return mapper.Map<List<CommentDTO>>(comments);
        }
       
        private static void ValidateLotDTO(CommentDTO commentDTO)
        {
            Precognitions.StringIsNullOrEmpty(commentDTO.Author);
            Precognitions.StringIsNullOrEmpty(commentDTO.UserId);
            Precognitions.StringIsNullOrEmpty(commentDTO.Text);
            Precognitions.IntIsNotNumberOrNegative(commentDTO.LotId, "Invalid lot id");
        }
    }
}
