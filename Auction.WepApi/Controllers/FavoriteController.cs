using Auction.BLL.DTO;
using Auction.BLL.Interfaces;
using Auction.WepApi.Logs;
using Auction.WepApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auction.WepApi.Controllers
{
    /// <summary>
    /// Favorite controller
    /// </summary>
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
        /// <summary>
        /// Get favorite list by user id 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Get favorite
        /// </summary>
        /// <param name="favoriteViewModel"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Add favorite
        /// </summary>
        /// <param name="favoriteViewModel"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Delete favorit e by user id and lot id
        /// </summary>
        /// <param name="favoriteViewModel"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Delete favorite by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
