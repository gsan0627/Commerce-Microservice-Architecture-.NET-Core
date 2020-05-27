﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Domain;
using Identity.Service.EventHandlers.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Identity.Api.Controllers
{
    [Route("v1/identity")]
    [ApiController]
    public class IdentityController : ControllerBase
    {

        private readonly ILogger<IdentityController> _logger;
        private readonly SignInManager<ApplicationUser> _signManager;
        private readonly IMediator _mediator;

        public IdentityController(ILogger<IdentityController> logger,
            SignInManager<ApplicationUser> signManager,
            IMediator mediator)
        {
            _logger = logger;
            _signManager = signManager;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateCommand command)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("authentication")]
        public async Task<IActionResult> Authentication(UserLoginCommand command)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);
                if (!result.Succeeded)
                {
                    return BadRequest("Access denied");
                }

                return Ok(result);
            }

            return BadRequest();
        }



    }
}