using Auction.BLL.DTO.Lot;
using Auction.BLL.Interfaces;
using Auction.WepApi.Logs;
using Auction.WepApi.Models.Lot;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Auction.WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorDescriptionController : ControllerBase
    {
        private readonly IAuthorDescriptionService service;
        private readonly IMapper mapper;
        private readonly ILogger<AdminController> logger;
        public AuthorDescriptionController(IAuthorDescriptionService srvc, IMapper mapper, ILogger<AdminController> logger)
        {
            service = srvc;
            this.mapper = mapper;
            this.logger = logger;
        }
       
        [HttpPost]
        public async Task<ActionResult> AddAuthorDescription([FromBody] AuthorDescriptionViewModel authorDescriptionViewModel)
        {
            try
            {
                var authorDescriptionDTO = mapper.Map<AuthorDescriptionViewModel, AuthorDescriptionDTO>(authorDescriptionViewModel);
                await service.AddAuthorDescriptionAsync(authorDescriptionDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
       
        [HttpPut]
        public async Task<ActionResult> UpdateAuthorDescription([FromBody] AuthorDescriptionViewModel authorDescriptionViewModel)
        {
            try
            {
                var authorDescriptionDTO = mapper.Map<AuthorDescriptionViewModel, AuthorDescriptionDTO>(authorDescriptionViewModel);
                await service.UpdateAuthorDescriptionAsync(authorDescriptionDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }

        [HttpGet("GetAuthorDescriptionByLotId/{id}")]
        public async Task<ActionResult<AuthorDescriptionViewModel>> GetUserBids(int id)
        {
            try
            {
                var authorDescriptionDTO = await service.GetAuthorDescriptionByLotIdAsync(id);
                return Ok(mapper.Map<AuthorDescriptionDTO, AuthorDescriptionViewModel>(authorDescriptionDTO));
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
    }
}
