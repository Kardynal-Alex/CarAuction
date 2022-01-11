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
    /// Admin Controller
    /// </summary>
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
        /// <summary>
        /// Get users with role user
        /// </summary>
        /// <returns></returns>
        [HttpGet("users")]
        public ActionResult<List<UserViewModel>> GetUser()
        {
            var user = service.GetUsersWithRoleUser();
            return Ok(mapper.Map<List<UserViewModel>>(user));
        }
        /// <summary>
        /// Delete user by id and his lot and lostate
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            try
            {
                return Ok(await service.DeleteUser(id));
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
    }
}
