using Microsoft.AspNetCore.Mvc;
using Neuralm.Services.Common.Rest;
using Neuralm.Services.UserService.Application.Dtos;
using Neuralm.Services.UserService.Application.Interfaces;
using Neuralm.Services.UserService.Application.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Neuralm.Services.UserService.Rest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            UserDto user = await _userService.FindSingleOrDefaultAsync(id);
            return user == null ? (IActionResult) new NotFoundResult() : new OkObjectResult(user);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            return new OkObjectResult(await _userService.GetAllAsync());
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync(UserDto user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());
            (bool success, Guid id, bool updated) = await _userService.UpdateAsync(user);
            if (!success)
                return new ConflictResult();
            if (!updated)
                return new CreatedAtActionResult(nameof(GetAsync), nameof(UserController), new { id }, "");
            return new NoContentResult();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(UserDto user)
        {
            (bool success, bool found) = await _userService.DeleteAsync(user);
            if (!found)
                return new NotFoundResult();
            return success ? new NoContentResult() : new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticateRequest authenticateRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());
            AuthenticateResponse authenticateResponse = await _userService.AuthenticateAsync(authenticateRequest);
            return new OkObjectResult(authenticateResponse);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());
            RegisterResponse registerResponse = await _userService.RegisterAsync(registerRequest);
            return new OkObjectResult(registerResponse);
        }
    }
}
