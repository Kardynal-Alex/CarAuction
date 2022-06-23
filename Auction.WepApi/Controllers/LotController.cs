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
      
        [HttpGet("getfreshlots")]
        public async Task<ActionResult<List<LotViewModel>>> GetFreshLots()
        {
            var listDTO = await lotService.GetFreshLots();
            return Ok(mapper.Map<List<LotViewModel>>(listDTO));
        }
       
        [HttpGet("getsoldlots")]
        public async Task<ActionResult<List<LotViewModel>>> GetSoldLots()
        {
            var listDTOs = await lotService.GetSoldLotsAsync();
            return Ok(mapper.Map<List<LotViewModel>>(listDTOs));
        }
   
        [HttpGet]
        public async Task<ActionResult<List<LotViewModel>>> GetAllLots()
        {
            var listLotDTO = await lotService.GetAllLotsAsync();
            return Ok(mapper.Map<List<LotViewModel>>(listLotDTO));
        }
      
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
    }
}
