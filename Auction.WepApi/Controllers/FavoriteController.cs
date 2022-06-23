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
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService service;
        private readonly IMapper mapper;
        private readonly ILogger<FavoriteController> logger;
        public FavoriteController(IFavoriteService srvc, IMapper mapper, ILogger<FavoriteController> logger)
        {
            service = srvc;
            this.mapper = mapper;
            this.logger = logger;
        }
    
        [HttpGet("{id}")]
        public async Task<ActionResult<List<FavoriteViewModel>>> GetFavoritesByUserId(string id)
        {
            try
            {
                var favoriteDTOs = await service.GetFavoriteByUserIdAsync(id);
                return Ok(mapper.Map<List<FavoriteViewModel>>(favoriteDTOs));
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
     
        [HttpPost("favorite")]
        public async Task<ActionResult<FavoriteViewModel>> GetFavoriteByLotIdAndUserId([FromBody]FavoriteViewModel favoriteViewModel)
        {
            try
            {
                var mappedFavorite = mapper.Map<FavoriteViewModel, FavoriteDTO>(favoriteViewModel);
                var favoriteDTO = await service.GetFavoriteByUserIdAndLotIdAsync(mappedFavorite);
                return Ok(mapper.Map<FavoriteDTO, FavoriteViewModel>(favoriteDTO));
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
      
        [HttpPost]
        public async Task<ActionResult> AddFavorite(FavoriteViewModel favoriteViewModel)
        {
            try
            {
                await service.AddFavoriteAsync(mapper.Map<FavoriteViewModel, FavoriteDTO>(favoriteViewModel));
                return Ok();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
      
        [HttpPost("deletepost")]
        public async Task<ActionResult> DeleteFavoriteByUserIdAndLotId(FavoriteViewModel favoriteViewModel)
        {
            try
            {
                await service.DeleteFavoriteByLotIdAndUserIdAsync(mapper.Map<FavoriteViewModel, FavoriteDTO>(favoriteViewModel));
                return NoContent();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
      
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFavorite(string id)
        {
            try
            {
                await service.DeleteFavoriteAsync(id);
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
