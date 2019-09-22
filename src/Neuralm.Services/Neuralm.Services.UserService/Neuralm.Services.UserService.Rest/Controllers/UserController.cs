using Microsoft.AspNetCore.Mvc;
using Neuralm.Services.Common.Rest;
using Neuralm.Services.UserService.Application.Dtos;
using Neuralm.Services.UserService.Application.Interfaces;
using Neuralm.Services.UserService.Application.Models;
using System.Threading.Tasks;

namespace Neuralm.Services.UserService.Rest.Controllers
{
    public class UserController : RestController<UserDto>
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) : base(userService)
        {
            _userService = userService;
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
