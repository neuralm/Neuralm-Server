using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neuralm.Services.Common.Rest;
using Neuralm.Services.RegistryService.Application.Dtos;
using Neuralm.Services.RegistryService.Application.Interfaces;
using System.Threading.Tasks;

namespace Neuralm.Services.RegistryService.Rest.Controllers
{
    public class RegistryController : RestController<ServiceDto>
    {
        private readonly IRegistryService _registryService;

        public RegistryController(IRegistryService registryService) : base(registryService)
        {
            _registryService = registryService;
        }

        [Authorize(Roles = "Service"), HttpPost("create")]
        public override Task<IActionResult> CreateAsync(ServiceDto dto)
        {
            // TODO: on success notify message queue.
            return base.CreateAsync(dto);
        }

        [Authorize(Roles = "Service"), HttpPut("update")]
        public override Task<IActionResult> UpdateAsync(ServiceDto dto)
        {
            return base.UpdateAsync(dto);
        }

        [Authorize(Roles = "Service"), HttpDelete("delete")]
        public override Task<IActionResult> DeleteAsync(ServiceDto dto)
        {
            return base.DeleteAsync(dto);
        }
    }
}
