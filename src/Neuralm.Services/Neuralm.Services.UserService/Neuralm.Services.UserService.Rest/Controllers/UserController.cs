﻿using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neuralm.Services.Common.Rest;
using Neuralm.Services.UserService.Application.Interfaces;
using Neuralm.Services.UserService.Messages;
using Neuralm.Services.UserService.Messages.Dtos;
using System.Threading.Tasks;

namespace Neuralm.Services.UserService.Rest.Controllers
{
    [Authorize(Roles = "MessageQueue")]
    public class UserController : RestController<UserDto>
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) : base(userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "MessageQueue, Service")]
        public override Task<IActionResult> GetAsync(Guid id)
        {
            return base.GetAsync(id);
        }

        [AllowAnonymous, HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticateRequest authenticateRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());
            AuthenticateResponse authenticateResponse = await _userService.AuthenticateAsync(authenticateRequest);
            return new OkObjectResult(authenticateResponse);
        }

        [AllowAnonymous, HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());
            RegisterResponse registerResponse = await _userService.RegisterAsync(registerRequest);
            return new OkObjectResult(registerResponse);
        }
    }
}
