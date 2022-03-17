using Auction.BLL.Interfaces;
using Auction.WepApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auction.BLL.DTO;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Auction.WepApi.Logs;

namespace Auction.WepApi.Controllers
{
    /// <summary>
    /// Lot Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LotController : ControllerBase
    {
        private readonly ILotService lotService;
        private readonly IMapper mapper;
        private readonly ILogger<LotController> logger;
        public LotController(ILotService lotSrvc, IMapper mapper, ILogger<LotController> logger)
        {
            lotService = lotSrvc;
            this.mapper = mapper;
            this.logger = logger;
        }
        /// <summary>
        /// Get lot by user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getuserlots/{id}")]
        public async Task<ActionResult<List<LotViewModel>>> GetLotsByUserId(string id)
        {
            try
            {
                var listLotDTO = await lotService.GetLotsByUserIdAsync(id);
                return Ok(mapper.Map<List<LotViewModel>>(listLotDTO));
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
        /// <summary>
        /// Get favorite user lot
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("favorites/{id}")]
        public async Task<ActionResult<List<LotViewModel>>> GetFavoriteLotsByUserId(string id)
        {
            try
            {
                var lotDTOs = await lotService.GetFavoriteLotsByUserIdAsync(id);
                return Ok(mapper.Map<List<LotViewModel>>(lotDTOs));
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
        /// <summary>
        /// Get fresh lots
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getfreshlots")]
        public async Task<ActionResult<List<LotViewModel>>> GetFreshLots()
        {
            var listDTO = await lotService.GetFreshLots();
            return Ok(mapper.Map<List<LotViewModel>>(listDTO));
        }
        /// <summary>
        /// Get sold lots
        /// </summary>
        /// <returns></returns>
        [HttpGet("getsoldlots")]
        public async Task<ActionResult<List<LotViewModel>>> GetSoldLots()
        {
            var listDTOs = await lotService.GetSoldLotsAsync();
            return Ok(mapper.Map<List<LotViewModel>>(listDTOs));
        }
        /// <summary>
        /// Get all lots
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<LotViewModel>>> GetAllLots()
        {
            var listLotDTO = await lotService.GetAllLotsAsync();
            return Ok(mapper.Map<List<LotViewModel>>(listLotDTO));
        }
        /// <summary>
        /// Get lot by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<LotViewModel>> GetLotByIdWithAllDetails(int id)
        {
            try
            {
                var lotDTO = await lotService.GetLotByIdWithDetailsAsync(id);
                return Ok(mapper.Map<LotDTO, LotViewModel>(lotDTO));
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
        /// <summary>
        /// Get user bids(all lots where he made bid(can be sold lot and no yet))
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("userbids/{id}")]
        public async Task<ActionResult<List<LotViewModel>>> GetUserBids(string id)
        {
            try
            {
                var userBids = await lotService.GetUserBidsAsync(id);
                return Ok(mapper.Map<List<LotViewModel>>(userBids));
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
        /// <summary>
        /// Add lot and lotstate
        /// </summary>
        /// <param name="lotViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<LotViewModel>> AddLot([FromBody] LotViewModel lotViewModel)
        {
            try
            {
                var lotDTO = mapper.Map<LotViewModel, LotDTO>(lotViewModel);
                await lotService.AddLotAsync(lotDTO);
                return Ok(lotViewModel);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
        /// <summary>
        /// Update lot after closing
        /// </summary>
        /// <param name="lotViewModel"></param>
        /// <returns></returns>
        [HttpPut("closebid")]
        public async Task<ActionResult> PutCloseBid([FromBody] LotViewModel lotViewModel)
        {
            try
            {
                var lotDTO = mapper.Map<LotViewModel, LotDTO>(lotViewModel);
                await lotService.UpdateLotAfterClosingAsync(lotDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
        /// <summary>
        /// Update only lot
        /// </summary>
        [HttpPut("onlydatelot")]
        public async Task<ActionResult> UpdateOnlyDateLot([FromBody] LotViewModel lotViewModel)
        {
            try
            {
                var lotDTO = mapper.Map<LotViewModel, LotDTO>(lotViewModel);
                await lotService.UpdateOnlyDateLotAsync(lotDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
        /// <summary>
        /// Update lot
        /// </summary>
        /// <param name="lotViewModel"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] LotViewModel lotViewModel)
        {
            try
            {
                var lotDTO = mapper.Map<LotViewModel, LotDTO>(lotViewModel);
                await lotService.UpdateLotAsync(lotDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
        /// <summary>
        /// Delete lot and lotstate
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await lotService.DeleteLotAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
        /// <summary>
        /// Ask Owner By sending email
        /// </summary>
        /// <returns></returns>
        [HttpPost("askowner")]
        public async Task<ActionResult> AskOwnerSendingEmail([FromBody] AskOwnerViewModel askOwnerViewModel)
        {
            try
            {
                var askOwnerDTO = mapper.Map<AskOwnerViewModel, AskOwnerDTO>(askOwnerViewModel);
                await lotService.AskOwnerSendingEmailAsync(askOwnerDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
        /// <summary>
        /// AddAuthorDescription
        /// </summary>
        /// <param name="authorDescriptionViewModel"></param>
        /// <returns></returns>
        [HttpPost("addAuthorDescription")]
        public async Task<ActionResult> AddAuthorDescription([FromBody]AuthorDescriptionViewModel authorDescriptionViewModel)
        {
            try
            {
                var authorDescriptionDTO = mapper.Map<AuthorDescriptionViewModel, AuthorDescriptionDTO>(authorDescriptionViewModel);
                await lotService.AddAuthorDescriptionAsync(authorDescriptionDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
        /// <summary>
        /// UpdateAuthorDescription        /// </summary>
        /// <param name="authorDescriptionViewModel"></param>
        /// <returns></returns>
        [HttpPut("updateAuthorDescription")]
        public async Task<ActionResult> UpdateAuthorDescription([FromBody] AuthorDescriptionViewModel authorDescriptionViewModel)
        {
            try
            {
                var authorDescriptionDTO = mapper.Map<AuthorDescriptionViewModel, AuthorDescriptionDTO>(authorDescriptionViewModel);
                await lotService.UpdateAuthorDescriptionAsync(authorDescriptionDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }

        [HttpGet("getAuthorDescriptionByLotId/{id}")]
        public async Task<ActionResult<AuthorDescriptionViewModel>> GetUserBids(int id)
        {
            try
            {
                var authorDescriptionDTO = await lotService.GetAuthorDescriptionByLotIdAsync(id);
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
