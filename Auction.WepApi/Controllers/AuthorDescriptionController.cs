﻿using Auction.BLL.DTO;
using Auction.BLL.Interfaces;
using Auction.WepApi.Logs;
using Auction.WepApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Auction.WepApi.Controllers
{
    /// <summary>
    /// AuthorDescription controller
    /// </summary>
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
        /// <summary>
        /// AddAuthorDescription
        /// </summary>
        /// <param name="authorDescriptionViewModel"></param>
        /// <returns></returns>
        [HttpPost("addAuthorDescription")]
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
                await service.UpdateAuthorDescriptionAsync(authorDescriptionDTO);
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