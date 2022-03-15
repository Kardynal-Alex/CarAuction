using Auction.BLL.DTO;
using Auction.BLL.Interfaces;
using Auction.WepApi.Logs;
using Auction.WepApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auction.WepApi.Controllers
{
    /// <summary>
    /// Comment Controller
    /// </summary>
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
        /// <summary>
        /// Add comment
        /// </summary>
        /// <param name="commentViewModel"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Get Comments by lot id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Delete comment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
