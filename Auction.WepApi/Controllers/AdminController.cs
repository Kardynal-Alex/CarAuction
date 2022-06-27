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
    public class AdminController : ControllerBase
    {
        private readonly IUserService service;
        private readonly IMapper mapper;
        private readonly ILogger<AdminController> logger;
        public AdminController(IUserService srvc, IMapper mapper, ILogger<AdminController> logger)
        {
            service = srvc;
            this.mapper = mapper;
            this.logger = logger;
        }
        
        [HttpGet("Users")]
        public ActionResult<List<UserViewModel>> GetUser()
        {
            var user = service.GetUsersWithRoleUser();
            return Ok(mapper.Map<List<UserViewModel>>(user));
        }
      
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            try
            {
                await service.DeleteUser(id);
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
