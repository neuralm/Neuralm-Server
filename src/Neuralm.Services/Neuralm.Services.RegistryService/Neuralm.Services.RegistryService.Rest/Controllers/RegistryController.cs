using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neuralm.Services.Common.Rest;
using Neuralm.Services.RegistryService.Application.Dtos;
using Neuralm.Services.RegistryService.Application.Interfaces;
using System.Threading.Tasks;

namespace Neuralm.Services.RegistryService.Rest.Controllers
{
    [Authorize(Roles = "Service")]
    public class RegistryController : RestController<ServiceDto>
    {
        private readonly IRegistryService _registryService;

        public RegistryController(IRegistryService registryService) : base(registryService)
        {
            _registryService = registryService;
        }

        [HttpGet("{serviceName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(string serviceName)
        {
            ServiceDto serviceDto = await _registryService.GetServiceByNameAsync(serviceName);
            return serviceDto == null ? (IActionResult)new NotFoundResult() : new OkObjectResult(serviceDto);
        }

        [HttpPost("")]
        public override Task<IActionResult> CreateAsync(ServiceDto dto)
        {
            return base.CreateAsync(dto);
        }

        [HttpPut("")]
        public override Task<IActionResult> UpdateAsync(ServiceDto dto)
        {
            return base.UpdateAsync(dto);
        }

        [HttpDelete("")]
        public override Task<IActionResult> DeleteAsync(ServiceDto dto)
        {
            return base.DeleteAsync(dto);
        }
    }
}
