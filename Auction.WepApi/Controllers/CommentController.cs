using Auction.BLL.DTO;
using Auction.BLL.Interfaces;
using Auction.WepApi.Logs;
using Auction.WepApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService service;
        private readonly IMapper mapper;
        private readonly ILogger<CommentController> logger;
        public CommentController(ICommentService srvc, IMapper mapper, ILogger<CommentController> logger)
        {
            service = srvc;
            this.mapper = mapper;
            this.logger = logger;
        }
      
        [HttpPost]
        public async Task<ActionResult> AddComment([FromBody] CommentViewModel commentViewModel)
        {
            try
            {
                await service.AddCommentAsync(mapper.Map<CommentViewModel, CommentDTO>(commentViewModel));
                return Ok();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
       
        [HttpGet("{id}")]
        public async Task<ActionResult<List<CommentViewModel>>> GetCommentsByLotId(int id)
        {
            try
            {
                var commentDTO = await service.GetCommentsByLotIdAsync(id);
                return Ok(mapper.Map<List<CommentViewModel>>(commentDTO));
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
       
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(Guid id)
        {
            try
            {
                await service.DeleteCommentByIdAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
    }
}
